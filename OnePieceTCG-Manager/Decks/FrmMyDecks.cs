using OnePieceTCG_Manager.Data;
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
        private readonly OnePieceContext _db;

        private readonly Dictionary<string, Image> _thumbCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        public FrmMyDecks(string codUsu)
        {
            InitializeComponent();

            _codUsu = codUsu;
            _db = new OnePieceContext();

            InitializeGrid();
            LoadDecks();
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

        private void LoadDecks()
        {
            var raw = _db.Decks
                .Include(d => d.LeaderCard)
                .Include(d => d.DeckCards)
                .Where(d => d.codUsu == _codUsu)
                .OrderByDescending(d => d.lastUpdatedDate)
                .Select(d => new
                {
                    d.Id,
                    d.deckName,
                    d.isActive,
                    d.lastUpdatedDate,
                    LeaderName = d.LeaderCard != null ? d.LeaderCard.cardName : "",
                    LeaderImageUrl = d.LeaderCard != null ? d.LeaderCard.cardImage : null,
                    TotalCards = d.DeckCards.Sum(dc => (int?)dc.quantity) ?? 0
                })
                .ToList();

            var rows = raw.Select(r => new DeckRow
            {
                Id = r.Id,
                DeckName = r.deckName,
                LeaderName = r.LeaderName,
                LeaderImageUrl = r.LeaderImageUrl,
                TotalCards = r.TotalCards,
                LastUpdated = r.lastUpdatedDate,
                IsActive = r.isActive
            }).ToList();

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

                var img = await ImageLoader.TryLoadImageAsync(row.LeaderImageUrl);
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

            LoadDecks();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDecks.CurrentRow == null) return;

            var deckId = (Guid)dgvDecks.CurrentRow.Cells["Id"].Value;
            using (var frm = new FrmDeckEditor(_codUsu, deckId))
                frm.ShowDialog(this);

            LoadDecks();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDecks.CurrentRow == null) return;

            var deckId = (Guid)dgvDecks.CurrentRow.Cells["Id"].Value;
            var deck = _db.Decks.Include(d => d.DeckCards).FirstOrDefault(d => d.Id == deckId);
            if (deck == null) return;

            var name = deck.deckName ?? "(sin nombre)";
            if (MessageBox.Show($"¿Eliminar deck '{name}'?\n\nEsto devolverá al stock todas las cartas usadas por este deck.",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;

            using (var tx = _db.Database.BeginTransaction())
            {
                try
                {
                    var deckCards = _db.DeckCards.Where(dc => dc.deckId == deckId).ToList();
                    var byCard = deckCards
                        .GroupBy(dc => dc.cardStockId)
                        .ToDictionary(g => g.Key, g => g.Sum(x => x.quantity));

                    if (deck.leaderCardId.HasValue)
                    {
                        var leaderId = deck.leaderCardId.Value;
                        if (byCard.ContainsKey(leaderId)) byCard[leaderId] += 1;
                        else byCard[leaderId] = 1;
                    }

                    var affectedIds = byCard.Keys.ToList();
                    var stocks = _db.CardStock.Where(c => affectedIds.Contains(c.Id)).ToList();

                    foreach (var st in stocks)
                    {
                        var dec = byCard[st.Id];
                        st.usedCards = Math.Max(0, st.usedCards - dec);
                        st.lastUpdatedCardDate = DateTime.Now;
                    }

                    _db.Decks.Remove(deck);
                    _db.SaveChanges();

                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    MessageBox.Show("Error eliminando deck:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadDecks();
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

        // ViewModel
        private class DeckRow : INotifyPropertyChanged
        {
            public Guid Id { get; set; }
            public string DeckName { get; set; }
            public string LeaderName { get; set; }
            public string LeaderImageUrl { get; set; }
            public int TotalCards { get; set; }
            public DateTime LastUpdated { get; set; }
            public bool IsActive { get; set; }

            private Image _leaderImage;
            public Image LeaderImage
            {
                get => _leaderImage;
                set
                {
                    _leaderImage = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeaderImage)));
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
        }
    }

    internal static class ImageLoader
    {
        public static async Task<Image> TryLoadImageAsync(string pathOrUrl)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (System.IO.File.Exists(pathOrUrl))
                    {
                        using (var fs = new System.IO.FileStream(pathOrUrl, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            return Image.FromStream(fs);
                        }
                    }

                    if (Uri.TryCreate(pathOrUrl, UriKind.Absolute, out var uri) &&
                        (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                    {
                        using (var wc = new System.Net.WebClient())
                        {
                            var bytes = wc.DownloadData(uri);
                            using (var ms = new System.IO.MemoryStream(bytes))
                            {
                                return Image.FromStream(ms);
                            }
                        }
                    }
                }
                catch { }

                return null;
            });
        }
    }
}
