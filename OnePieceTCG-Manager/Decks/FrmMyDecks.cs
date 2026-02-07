using OnePieceTCG_Manager.Data;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Services;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
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

        public FrmMyDecks(string codUsu)
        {
            InitializeComponent();

            _codUsu = codUsu;
            _decksService = new DecksService();

            InitializeGrid();
            LoadDecksAsync();
            pnlTop_Resize(this, EventArgs.Empty); // posiciona botones al inicio
        }

        private void InitializeGrid()
        {
            dgvDecks.AutoGenerateColumns = false;
            dgvDecks.Columns.Clear();

            dgvDecks.RowTemplate.Height = 60;
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
                Width = 70,
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
                HeaderText = "Líder (nombre)",
                DataPropertyName = nameof(DeckRow.LeaderName),
                Width = 180
            });

            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cartas (sin líder)",
                DataPropertyName = nameof(DeckRow.TotalCards),
                Width = 120
            });

            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Actualizado",
                DataPropertyName = nameof(DeckRow.LastUpdated),
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy HH:mm" }
            });

            dgvDecks.Columns.Add(new DataGridViewCheckBoxColumn
            {
                HeaderText = "Activo",
                DataPropertyName = nameof(DeckRow.IsActive),
                Width = 70
            });

            dgvDecks.CellDoubleClick += dgvDecks_CellDoubleClick;
            dgvDecks.DataBindingComplete += dgvDecks_DataBindingComplete;
        }

        private void dgvDecks_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvDecks.ClearSelection();
        }

        private void dgvDecks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnEdit_Click(sender, EventArgs.Empty);
        }

        private async Task LoadDecksAsync()
        {
            var rows = await _decksService.GetDecksByUserAsync(_codUsu);

            dgvDecks.DataSource = new BindingList<DeckRow>(rows);

            _ = LoadLeaderThumbnailsAsync(rows);
        }

        private async Task LoadLeaderThumbnailsAsync(List<DeckRow> rows)
        {
            foreach (var row in rows)
            {
                if (string.IsNullOrWhiteSpace(row.LeaderImageUrl)) continue;

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

            LoadDecksAsync();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDecks.CurrentRow == null) return;

            var deckId = (Guid)dgvDecks.CurrentRow.Cells["Id"].Value;
            using (var frm = new FrmDeckEditor(_codUsu, deckId))
                frm.ShowDialog(this);

            LoadDecksAsync();
        }

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDecks.CurrentRow == null) return;

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
                MessageBox.Show(
                    "Error eliminando deck:\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        // ✅ Handler clásico (sin lambda) para el Designer
        private void pnlTop_Resize(object sender, EventArgs e)
        {
            int right = pnlTop.ClientSize.Width - 12;

            btnDelete.Location = new Point(right - btnDelete.Width, 12);
            right -= (btnDelete.Width + 10);

            btnEdit.Location = new Point(right - btnEdit.Width, 12);
            right -= (btnEdit.Width + 10);

            btnCreate.Location = new Point(right - btnCreate.Width, 12);
        }
    }
}
