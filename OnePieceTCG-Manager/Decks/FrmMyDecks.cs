using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Services;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Decks
{
    public partial class FrmMyDecks : Form
    {
        private readonly string _codUsu;
        private readonly DecksService _decksService;
        private readonly Dictionary<string, Image> _thumbCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
        private List<DeckRow> _allDecks = new List<DeckRow>();

        private Panel _pnlInsights;
        private Panel _pnlFilters;
        private TextBox _txtSearch;
        private ComboBox _cbStatus;
        private Button _btnBrowse;
        private Label _lblDeckCount;
        private Label _lblDeckCountSub;
        private Label _lblActiveCount;
        private Label _lblActiveCountSub;
        private Label _lblAverageCards;
        private Label _lblAverageCardsSub;
        private Label _lblLatestUpdate;
        private Label _lblLatestUpdateSub;

        public FrmMyDecks(string codUsu)
        {
            InitializeComponent();

            _codUsu = codUsu;
            _decksService = new DecksService();

            InitializeGrid();
            InitializeModernLayout();
            _ = LoadDecksAsync();
            pnlTop_Resize(this, EventArgs.Empty);
        }

        private void InitializeModernLayout()
        {
            ModernUi.ApplyFormTheme(this);
            ModernUi.StylePanelCard(pnlTop);
            pnlTop.BackColor = ModernUi.Surface;
            lblTitle.Text = "Deck Center";
            lblTitle.ForeColor = ModernUi.TextPrimary;

            ModernUi.StyleButton(btnCreate, ModernUi.Accent, Color.White);
            ModernUi.StyleOutlineButton(btnEdit);
            ModernUi.StyleButton(btnDelete, ModernUi.Danger, Color.White);
            btnCreate.Text = "Nuevo deck";
            btnEdit.Text = "Editar";
            btnDelete.Text = "Eliminar";

            _btnBrowse = new Button
            {
                Text = "Explorar decks",
                Width = 140,
                Height = 34
            };
            ModernUi.StyleOutlineButton(_btnBrowse);
            _btnBrowse.Click += BtnBrowse_Click;
            pnlTop.Controls.Add(_btnBrowse);

            BuildInsightsPanel();
            BuildFiltersPanel();
            ModernUi.StyleDataGridView(dgvDecks);
            dgvDecks.Dock = DockStyle.Fill;
            dgvDecks.Margin = new Padding(8, 0, 8, 8);
        }

        private void BuildInsightsPanel()
        {
            _pnlInsights = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ModernUi.AppBack,
                Padding = new Padding(6, 8, 6, 4)
            };

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                BackColor = ModernUi.AppBack
            };
            for (int i = 0; i < 4; i++)
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));

            layout.Controls.Add(ModernUi.CreateKpiCard("Decks totales", ModernUi.Accent, out _lblDeckCount, out _lblDeckCountSub), 0, 0);
            layout.Controls.Add(ModernUi.CreateKpiCard("Decks activos", ModernUi.Success, out _lblActiveCount, out _lblActiveCountSub), 1, 0);
            layout.Controls.Add(ModernUi.CreateKpiCard("Promedio de cartas", ModernUi.Navy, out _lblAverageCards, out _lblAverageCardsSub), 2, 0);
            layout.Controls.Add(ModernUi.CreateKpiCard("Última actividad", ModernUi.Warning, out _lblLatestUpdate, out _lblLatestUpdateSub), 3, 0);

            _pnlInsights.Controls.Add(layout);

            tlpRoot.SuspendLayout();
            tlpRoot.RowCount = 4;
            tlpRoot.RowStyles.Clear();
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tlpRoot.Controls.Add(_pnlInsights, 0, 1);
            tlpRoot.SetRow(dgvDecks, 3);
            tlpRoot.ResumeLayout();
        }

        private void BuildFiltersPanel()
        {
            _pnlFilters = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 10, 12, 10),
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblSearch = new Label
            {
                Text = "Buscar deck:",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary,
                Location = new Point(12, 14)
            };

            _txtSearch = new TextBox
            {
                Width = 240,
                Height = 28,
                Location = new Point(104, 10)
            };
            ModernUi.StyleInput(_txtSearch);
            _txtSearch.TextChanged += (s, e) => ApplyDeckFilters();

            var lblStatus = new Label
            {
                Text = "Estado:",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary,
                Location = new Point(360, 14)
            };

            _cbStatus = new ComboBox
            {
                Width = 140,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(416, 10)
            };
            _cbStatus.Items.AddRange(new[] { "Todos", "Activos", "Inactivos", "Completos", "En construcción" });
            _cbStatus.SelectedIndex = 0;
            _cbStatus.SelectedIndexChanged += (s, e) => ApplyDeckFilters();
            ModernUi.StyleInput(_cbStatus);

            var btnClear = new Button
            {
                Text = "Limpiar",
                Width = 90,
                Height = 32,
                Location = new Point(572, 8)
            };
            ModernUi.StyleOutlineButton(btnClear);
            btnClear.Click += (s, e) =>
            {
                _txtSearch.Text = string.Empty;
                _cbStatus.SelectedIndex = 0;
            };

            _pnlFilters.Controls.Add(lblSearch);
            _pnlFilters.Controls.Add(_txtSearch);
            _pnlFilters.Controls.Add(lblStatus);
            _pnlFilters.Controls.Add(_cbStatus);
            _pnlFilters.Controls.Add(btnClear);

            tlpRoot.Controls.Add(_pnlFilters, 0, 2);
        }

        private void InitializeGrid()
        {
            dgvDecks.AutoGenerateColumns = false;
            dgvDecks.Columns.Clear();

            dgvDecks.RowTemplate.Height = 64;
            dgvDecks.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDecks.MultiSelect = false;
            dgvDecks.ReadOnly = true;
            dgvDecks.AllowUserToAddRows = false;
            dgvDecks.AllowUserToDeleteRows = false;

            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = nameof(DeckRow.Id),
                Visible = false
            });

            dgvDecks.Columns.Add(new DataGridViewImageColumn
            {
                Name = "LeaderImg",
                HeaderText = "Líder",
                DataPropertyName = nameof(DeckRow.LeaderImage),
                Width = 78,
                ImageLayout = DataGridViewImageCellLayout.Zoom
            });

            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Deck",
                DataPropertyName = nameof(DeckRow.DeckName),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Líder",
                DataPropertyName = nameof(DeckRow.LeaderName),
                Width = 180
            });

            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cartas",
                DataPropertyName = nameof(DeckRow.TotalCards),
                Width = 80
            });

            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Estado",
                DataPropertyName = nameof(DeckRow.IsActive),
                Width = 90
            });

            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Actualizado",
                DataPropertyName = nameof(DeckRow.LastUpdated),
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvDecks.CellFormatting += dgvDecks_CellFormatting;
            dgvDecks.CellDoubleClick += dgvDecks_CellDoubleClick;
            dgvDecks.DataBindingComplete += dgvDecks_DataBindingComplete;
        }

        private void dgvDecks_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDecks.Columns[e.ColumnIndex].HeaderText == "Estado" && e.Value is bool active)
            {
                var row = dgvDecks.Rows[e.RowIndex].DataBoundItem as DeckRow;
                var completed = row != null && row.TotalCards >= 50;
                e.Value = active ? (completed ? "Activo" : "Activo*") : "Pausado";
                e.FormattingApplied = true;
            }
        }

        private void dgvDecks_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvDecks.ClearSelection();
        }

        private void dgvDecks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                btnEdit_Click(sender, EventArgs.Empty);
        }

        private async Task LoadDecksAsync()
        {
            _allDecks = await _decksService.GetDecksByUserAsync(_codUsu) ?? new List<DeckRow>();
            ApplyDeckFilters();
            _ = LoadLeaderThumbnailsAsync(_allDecks);
        }

        private void ApplyDeckFilters()
        {
            IEnumerable<DeckRow> query = _allDecks;
            string search = (_txtSearch?.Text ?? string.Empty).Trim().ToLowerInvariant();
            string status = _cbStatus?.SelectedItem?.ToString() ?? "Todos";

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(d =>
                    (d.DeckName ?? string.Empty).ToLowerInvariant().Contains(search) ||
                    (d.LeaderName ?? string.Empty).ToLowerInvariant().Contains(search));
            }

            switch (status)
            {
                case "Activos":
                    query = query.Where(d => d.IsActive);
                    break;
                case "Inactivos":
                    query = query.Where(d => !d.IsActive);
                    break;
                case "Completos":
                    query = query.Where(d => d.TotalCards >= 50);
                    break;
                case "En construcción":
                    query = query.Where(d => d.TotalCards < 50);
                    break;
            }

            var rows = query.OrderByDescending(d => d.LastUpdated).ThenBy(d => d.DeckName).ToList();
            dgvDecks.DataSource = new BindingList<DeckRow>(rows);
            UpdateInsights(rows);
        }

        private void UpdateInsights(List<DeckRow> rows)
        {
            _lblDeckCount.Text = rows.Count.ToString();
            _lblDeckCountSub.Text = string.Format("{0} decks visibles", rows.Count);

            int activeCount = rows.Count(d => d.IsActive);
            _lblActiveCount.Text = activeCount.ToString();
            _lblActiveCountSub.Text = string.Format("{0} listos para jugar", rows.Count(d => d.TotalCards >= 50));

            _lblAverageCards.Text = rows.Count == 0 ? "0" : rows.Average(d => d.TotalCards).ToString("0.0");
            _lblAverageCardsSub.Text = "Objetivo: 50 + líder";

            var latest = rows.OrderByDescending(d => d.LastUpdated).FirstOrDefault();
            _lblLatestUpdate.Text = latest == null ? "-" : latest.LastUpdated.ToString("dd/MM");
            _lblLatestUpdateSub.Text = latest == null ? "Sin actividad aún" : latest.DeckName;
        }

        private async Task LoadLeaderThumbnailsAsync(List<DeckRow> rows)
        {
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.LeaderImageUrl))
                    continue;

                if (_thumbCache.TryGetValue(row.LeaderImageUrl, out var cached))
                {
                    row.LeaderImage = cached;
                    continue;
                }

                var img = await ImageUtils.TryLoadImageAsync(row.LeaderImageUrl);
                if (img != null)
                {
                    _thumbCache[row.LeaderImageUrl] = img;
                    row.LeaderImage = img;
                }
            }

            dgvDecks.Invalidate();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmDeckEditor(_codUsu))
                frm.ShowDialog(this);

            _ = LoadDecksAsync();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDecks.CurrentRow == null)
                return;

            var deckId = (Guid)dgvDecks.CurrentRow.Cells["Id"].Value;
            using (var frm = new FrmDeckEditor(_codUsu, deckId))
                frm.ShowDialog(this);

            _ = LoadDecksAsync();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDecks.CurrentRow == null)
                return;

            var row = (DeckRow)dgvDecks.CurrentRow.DataBoundItem;
            var deckId = row.Id;
            var name = row.DeckName ?? "(sin nombre)";

            if (MessageBox.Show(
                $"¿Eliminar deck '{name}'?\n\nEsto devolverá al stock todas las cartas usadas por este deck.",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            try
            {
                await _decksService.DeleteDeckAsync(deckId);
                await LoadDecksAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error eliminando deck:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmDeckBrowser>().FirstOrDefault();
            if (frm == null)
            {
                frm = new FrmDeckBrowser(_codUsu)
                {
                    MdiParent = MdiParent
                };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }
        }

        private void pnlTop_Resize(object sender, EventArgs e)
        {
            int right = pnlTop.ClientSize.Width - 12;

            btnDelete.Location = new Point(right - btnDelete.Width, 10);
            right -= btnDelete.Width + 8;

            btnEdit.Location = new Point(right - btnEdit.Width, 10);
            right -= btnEdit.Width + 8;

            btnCreate.Location = new Point(right - btnCreate.Width, 10);
            right -= btnCreate.Width + 8;

            if (_btnBrowse != null)
                _btnBrowse.Location = new Point(right - _btnBrowse.Width, 10);
        }
    }
}
