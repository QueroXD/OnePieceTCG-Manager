using OnePieceTCG_Manager.Data;
using OnePieceTCG_Manager.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Decks
{
    public partial class FrmMyDecks : Form
    {
        private readonly string _codUsu;
        private readonly OnePieceContext _db;

        public FrmMyDecks(string codUsu)
        {
            InitializeComponent();
            _codUsu = codUsu;
            _db = new OnePieceContext();

            InitializeDataGridView(); // <-- columnas y eventos aquí

            LoadDecks();
        }

        private void InitializeDataGridView()
        {
            dgvDecks.AutoGenerateColumns = false;

            dgvDecks.Columns.Clear();
            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = "Id", Visible = false });
            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Deck", DataPropertyName = "deckName", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Líder", DataPropertyName = "Leader", Width = 120 });
            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Total cartas", DataPropertyName = "TotalCards", Width = 80 });
            dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Última actualización", DataPropertyName = "lastUpdatedDate", Width = 150 });

            dgvDecks.CellDoubleClick += dgvDecks_CellDoubleClick;
        }

        private void LoadDecks()
        {
            var decks = _db.Decks
                .Include(d => d.LeaderCard)
                .Where(d => d.codUsu == _codUsu)
                .OrderByDescending(d => d.lastUpdatedDate)
                .Select(d => new
                {
                    d.Id,
                    d.deckName,
                    Leader = d.LeaderCard != null ? d.LeaderCard.cardName : "",
                    TotalCards = d.DeckCards.Sum(dc => (int?)dc.quantity) ?? 0,
                    d.lastUpdatedDate
                })
                .ToList();

            dgvDecks.DataSource = decks;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            using (var frm = new FrmDeckEditor(_codUsu))
            {
                frm.ShowDialog();
            }
            LoadDecks();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDecks.CurrentRow == null) return;

            var deckId = (Guid)dgvDecks.CurrentRow.Cells["Id"].Value;
            using (var frm = new FrmDeckEditor(_codUsu, deckId))
            {
                frm.ShowDialog();
            }
            LoadDecks();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDecks.CurrentRow == null) return;

            var deckId = (Guid)dgvDecks.CurrentRow.Cells["Id"].Value;
            var deck = _db.Decks.Include(d => d.DeckCards).FirstOrDefault(d => d.Id == deckId);

            if (deck == null) return;

            if (MessageBox.Show($"¿Eliminar deck '{deck.deckName}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                // Borrar cartas asociadas primero
                _db.DeckCards.RemoveRange(deck.DeckCards);
                _db.Decks.Remove(deck);
                _db.SaveChanges();

                LoadDecks();
            }
        }

        private void dgvDecks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            btnEdit_Click(sender, EventArgs.Empty);
        }
    }
}
