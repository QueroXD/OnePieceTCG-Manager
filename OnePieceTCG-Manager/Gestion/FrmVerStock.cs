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

        private LoadingService _loading;

        private const int MIN_LOADING_MS = 3000;

        public FrmVerStock()
        {
            InitializeComponent();
            _loading = new LoadingService(this);
        }

        private async void FrmVerStock_Load(object sender, EventArgs e)
        {
            await EnsureDataAsync();
            LoadFilters();
            ShowListView();
        }

        // --------------------------
        // Garantiza datos en memoria
        // --------------------------
        private async Task EnsureDataAsync()
        {
            if (_allData.Count > 0)
                return;

            await LoadDataAsync();
        }

        // --------------------------
        // Carga datos desde la API
        // --------------------------
        private async Task LoadDataAsync()
        {
            await _loading.RunAsync(async () =>
            {
                _allData = await _cardStockService.GetAllAsync();
            }, "Cargando stock...", 3000);
        }

        // --------------------------
        // Recarga completa explícita
        // --------------------------
        private bool _isReloading = false;

        public async Task ReloadAsync()
        {
            if (_isReloading)
                return;

            _isReloading = true;

            try
            {
                await _loading.RunAsync(async () =>
                {
                    _allData = await _cardStockService.GetAllAsync();
                    LoadFilters();
                    RefreshView();
                }, "Actualizando stock...");
            }
            finally
            {
                _isReloading = false;
            }
        }


        // --------------------------
        // Carga inicial de filtros
        // --------------------------
        private void LoadFilters()
        {
            cbColor.Items.Clear();
            cbColor.Items.Add("Todos");
            cbColor.Items.AddRange(
                _allData.Select(c => c.color)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Distinct()
                        .ToArray()
            );
            cbColor.SelectedIndex = 0;

            cbType.Items.Clear();
            cbType.Items.Add("Todos");
            cbType.Items.AddRange(
                _allData.Select(c => c.type)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Distinct()
                        .ToArray()
            );
            cbType.SelectedIndex = 0;

            cbSubType.Items.Clear();
            cbSubType.Items.Add("Todos");
            cbSubType.Items.AddRange(
                _allData.Select(c => c.subType)
                        .Where(x => !string.IsNullOrEmpty(x))
                        .Distinct()
                        .ToArray()
            );
            cbSubType.SelectedIndex = 0;

            cbOrder.Items.Clear();
            cbOrder.Items.AddRange(new[]
            {
                "Nombre",
                "Rareza",
                "Color",
                "Coste",
                "Unidades"
            });
            cbOrder.SelectedIndex = 0;

            btnApplyFilters.Click -= BtnApplyFilters_Click;
            btnApplyFilters.Click += BtnApplyFilters_Click;
        }

        private void BtnApplyFilters_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        // --------------------------
        // Obtiene los datos filtrados
        // --------------------------
        private List<CardStock> GetFilteredData()
        {
            IEnumerable<CardStock> query = _allData;

            if (cbColor.SelectedIndex > 0)
                query = query.Where(c => c.color == cbColor.SelectedItem.ToString());

            if (cbType.SelectedIndex > 0)
                query = query.Where(c => c.type == cbType.SelectedItem.ToString());

            if (cbSubType.SelectedIndex > 0)
                query = query.Where(c => c.subType == cbSubType.SelectedItem.ToString());

            switch (cbOrder.SelectedItem.ToString())
            {
                case "Nombre":
                    query = query.OrderBy(c => c.cardName);
                    break;
                case "Rareza":
                    query = query.OrderBy(c => c.rarity);
                    break;
                case "Color":
                    query = query.OrderBy(c => c.color);
                    break;
                case "Coste":
                    query = query.OrderBy(c => c.cost);
                    break;
                case "Unidades":
                    query = query.OrderBy(c => c.units);
                    break;
            }

            return query.ToList();
        }

        // --------------------------
        // Refresca la vista activa
        // --------------------------
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


        // --------------------------
        // Mostrar vista LISTA
        // --------------------------
        private void ShowListView()
        {
            lblMode.Text = "Modo: Lista";

            panelContainer.Controls.Clear();
            var view = new UC_StockListView
            {
                Dock = DockStyle.Fill
            };

            view.CardDoubleClicked += OpenEditStock;

            panelContainer.Controls.Add(view);
            view.LoadData(GetFilteredData());
        }


        // --------------------------
        // Mostrar vista GALERÍA
        // --------------------------
        private async void ShowGalleryView()
        {
            lblMode.Text = "Modo: Galería";

            panelContainer.Controls.Clear();
            var view = new UC_StockGalleryView
            {
                Dock = DockStyle.Fill
            };

            view.CardDoubleClicked += OpenEditStock;
            panelContainer.Controls.Add(view);

            var data = GetFilteredData();

            await _loading.RunAsync(async () =>
            {
                await view.LoadDataAsync(data);
            }, "Cargando galería...", 800);
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
            using (var frm = new FrmAddStock(
                card.cardId,
                card.isAlter,
                card.cardImage,
                modoSoloUnidades: true))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    await ReloadAsync();
                }
            }
        }

    }
}
