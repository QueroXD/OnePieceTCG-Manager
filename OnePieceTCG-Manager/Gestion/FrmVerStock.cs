using OnePieceTCG_Manager.Gestion.Views;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Services;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion
{
    public partial class FrmVerStock : Form
    {
        private readonly CardStockService _cardStockService = new CardStockService();
        private List<CardStock> _allData = new List<CardStock>();
        private readonly LoadingService _loading;
        private bool _isReloading;
        private Label _lblSearch;
        private TextBox _txtSearch;

        public FrmVerStock()
        {
            InitializeComponent();
            _loading = new LoadingService(this);
            InitializeModernLayout();
        }

        private async void FrmVerStock_Load(object sender, EventArgs e)
        {
            await EnsureDataAsync();
            LoadFilters();
            ShowListView();
        }

        private void InitializeModernLayout()
        {
            ModernUi.ApplyFormTheme(this);

            toolStripMain.BackColor = ModernUi.Navy;
            toolStripMain.ForeColor = Color.White;
            lblMode.ForeColor = Color.White;
            toolListView.ForeColor = Color.White;
            toolGalleryView.ForeColor = Color.White;
            toolListView.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolGalleryView.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolListView.Text = "Lista";
            toolGalleryView.Text = "Galeria";

            panelFilters.Height = 74;
            panelFilters.Padding = new Padding(12, 10, 12, 10);
            ModernUi.StylePanelCard(panelFilters);
            panelContainer.BackColor = ModernUi.AppBack;

            ModernUi.StyleInput(cbOrder);
            ModernUi.StyleInput(cbType);
            ModernUi.StyleInput(cbSubType);
            ModernUi.StyleInput(cbColor);
            ModernUi.StyleButton(btnApplyFilters, ModernUi.Accent, Color.White);
            btnApplyFilters.Text = "Actualizar";
            btnApplyFilters.Width = 100;
            btnApplyFilters.Height = 32;

            _lblSearch = new Label
            {
                Text = "Buscar:",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary
            };

            _txtSearch = new TextBox
            {
                Width = 220,
                Height = 30,
                Font = new Font("Segoe UI", 9.5F)
            };
            ModernUi.StyleInput(_txtSearch);
            _txtSearch.TextChanged += (s, e) => RefreshView();

            panelFilters.Controls.Add(_lblSearch);
            panelFilters.Controls.Add(_txtSearch);
            panelFilters.Resize += PanelFilters_Resize;
            PanelFilters_Resize(this, EventArgs.Empty);
        }

        private void PanelFilters_Resize(object sender, EventArgs e)
        {
            int left = 12;

            lblOrden.Location = new Point(left, 12);
            cbOrder.Location = new Point(left + 74, 8);
            cbOrder.Size = new Size(150, 28);

            left = cbOrder.Right + 18;
            lblTipo.Location = new Point(left, 12);
            cbType.Location = new Point(left + 78, 8);
            cbType.Size = new Size(130, 28);

            left = cbType.Right + 18;
            lblSubtype.Location = new Point(left, 12);
            cbSubType.Location = new Point(left + 60, 8);
            cbSubType.Size = new Size(150, 28);

            left = cbSubType.Right + 18;
            lblColor.Location = new Point(left, 12);
            cbColor.Location = new Point(left + 42, 8);
            cbColor.Size = new Size(110, 28);

            left = cbColor.Right + 18;
            _lblSearch.Location = new Point(left, 12);
            _txtSearch.Location = new Point(left + 56, 8);
            _txtSearch.Size = new Size(Math.Max(160, panelFilters.Width - left - 180), 28);

            btnApplyFilters.Location = new Point(panelFilters.Width - btnApplyFilters.Width - 12, 8);
            btnApplyFilters.Size = new Size(100, 32);
        }

        private async Task EnsureDataAsync()
        {
            if (_allData.Count > 0)
                return;

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await _loading.RunAsync(async () =>
            {
                _allData = await _cardStockService.GetAllAsync();
            }, "Cargando stock...", 1000);
        }

        public async Task ReloadAsync()
        {
            if (_isReloading)
                return;

            _isReloading = true;
            try
            {
                await _loading.RunAsync(async () =>
                {
                    _allData = await _cardStockService.GetAllAsync(true);
                    LoadFilters();
                    RefreshView();
                }, "Actualizando stock...");
            }
            finally
            {
                _isReloading = false;
            }
        }

        private void LoadFilters()
        {
            cbColor.Items.Clear();
            cbColor.Items.Add("Todos");
            cbColor.Items.AddRange(_allData.Select(c => c.color).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToArray());
            cbColor.SelectedIndex = 0;

            cbType.Items.Clear();
            cbType.Items.Add("Todos");
            cbType.Items.AddRange(_allData.Select(c => c.type).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToArray());
            cbType.SelectedIndex = 0;

            cbSubType.Items.Clear();
            cbSubType.Items.Add("Todos");
            cbSubType.Items.AddRange(_allData.Select(c => c.subType).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToArray());
            cbSubType.SelectedIndex = 0;

            cbOrder.Items.Clear();
            cbOrder.Items.AddRange(new[]
            {
                "Nombre",
                "Rareza",
                "Color",
                "Coste",
                "Unidades",
                "Uso en decks",
                "Disponibles",
                "Última actualización"
            });
            cbOrder.SelectedIndex = 0;

            btnApplyFilters.Click -= BtnApplyFilters_Click;
            btnApplyFilters.Click += BtnApplyFilters_Click;
        }

        private void BtnApplyFilters_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private List<CardStock> GetFilteredData()
        {
            IEnumerable<CardStock> query = _allData;
            var search = (_txtSearch?.Text ?? string.Empty).Trim().ToLowerInvariant();

            if (cbColor.SelectedIndex > 0)
                query = query.Where(c => string.Equals(c.color, cbColor.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));

            if (cbType.SelectedIndex > 0)
                query = query.Where(c => string.Equals(c.type, cbType.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));

            if (cbSubType.SelectedIndex > 0)
                query = query.Where(c => string.Equals(c.subType, cbSubType.SelectedItem.ToString(), StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    (c.cardName ?? string.Empty).ToLowerInvariant().Contains(search) ||
                    (c.cardId ?? string.Empty).ToLowerInvariant().Contains(search) ||
                    (c.description ?? string.Empty).ToLowerInvariant().Contains(search));
            }

            switch (cbOrder.SelectedItem?.ToString())
            {
                case "Rareza":
                    query = query.OrderBy(c => c.rarity).ThenBy(c => c.cardName);
                    break;
                case "Color":
                    query = query.OrderBy(c => c.color).ThenBy(c => c.cardName);
                    break;
                case "Coste":
                    query = query.OrderByDescending(c => c.cost).ThenBy(c => c.cardName);
                    break;
                case "Unidades":
                    query = query.OrderByDescending(c => c.units).ThenBy(c => c.cardName);
                    break;
                case "Uso en decks":
                    query = query.OrderByDescending(c => c.usedCards).ThenBy(c => c.cardName);
                    break;
                case "Disponibles":
                    query = query.OrderByDescending(c => c.units - c.usedCards).ThenBy(c => c.cardName);
                    break;
                case "Última actualización":
                    query = query.OrderByDescending(c => c.lastUpdatedCardDate).ThenBy(c => c.cardName);
                    break;
                default:
                    query = query.OrderBy(c => c.cardName);
                    break;
            }

            return query.ToList();
        }

        private async void RefreshView()
        {
            if (panelContainer.Controls.Count == 0)
                return;

            var data = GetFilteredData();

            if (panelContainer.Controls[0] is UC_StockListView list)
            {
                list.LoadData(data);
            }
            else if (panelContainer.Controls[0] is UC_StockGalleryView gallery)
            {
                await gallery.LoadDataAsync(data);
            }
        }

        private void ShowListView()
        {
            lblMode.Text = "Modo: Lista";

            panelContainer.Controls.Clear();
            var view = new UC_StockListView { Dock = DockStyle.Fill };
            view.CardDoubleClicked += OpenEditStock;
            panelContainer.Controls.Add(view);
            view.LoadData(GetFilteredData());
        }

        private async void ShowGalleryView()
        {
            lblMode.Text = "Modo: Galeria";

            panelContainer.Controls.Clear();
            var view = new UC_StockGalleryView { Dock = DockStyle.Fill };
            view.CardDoubleClicked += OpenEditStock;
            panelContainer.Controls.Add(view);

            var data = GetFilteredData();
            await _loading.RunAsync(async () =>
            {
                await view.LoadDataAsync(data);
            }, "Cargando galeria...", 500);
        }

        private void toolListView_Click(object sender, EventArgs e)
        {
            ShowListView();
        }

        private void toolGalleryView_Click(object sender, EventArgs e)
        {
            ShowGalleryView();
        }

        private async void OpenEditStock(CardStock card)
        {
            using (var frm = new FrmAddStock(card.cardId, card.isAlter, card.cardImage, modoSoloUnidades: true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                    await ReloadAsync();
            }
        }
    }
}
