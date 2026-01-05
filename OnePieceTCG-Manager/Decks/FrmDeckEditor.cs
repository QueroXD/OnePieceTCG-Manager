using OnePieceTCG_Manager.Data;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Decks
{
    public partial class FrmDeckEditor : Form
    {
        private readonly OnePieceContext _db;
        private Deck _deck;
        private List<DeckCard> _deckCardsInMemory = new List<DeckCard>();
        private readonly string _codUsu;

        private Panel _selectedLeaderPanel;

        public FrmDeckEditor(string codUsu, Guid? deckId = null)
        {
            InitializeComponent();

            _db = new OnePieceContext();
            _codUsu = codUsu;

            InitializeDeckGrid();

            if (deckId.HasValue)
                LoadDeck(deckId.Value);
            else
                CreateNewDeck();
        }

        #region Inicialización

        private void InitializeDeckGrid()
        {
            dgvDeck.AutoGenerateColumns = false;
            dgvDeck.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDeck.MultiSelect = false;
            dgvDeck.ReadOnly = true;

            dgvDeck.Columns.Clear();
            dgvDeck.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = "Id",
                Visible = false
            });
            dgvDeck.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Carta",
                DataPropertyName = "cardName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });
            dgvDeck.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Cantidad",
                DataPropertyName = "quantity",
                Width = 80
            });
        }

        private void CreateNewDeck()
        {
            _deck = new Deck
            {
                deckName = "Nuevo Deck", // nombre por defecto en memoria
                codUsu = _codUsu,
                isActive = true,
                createdDate = DateTime.Now,
                lastUpdatedDate = DateTime.Now
            };

            txtDeckName.Text = _deck.deckName;

            _ = LoadStockAsync();
            _ = LoadLeadersAsync();
            UpdateTotalCards();
        }


        private void LoadDeck(Guid deckId)
        {
            _deck = _db.Decks
                .FirstOrDefault(d => d.Id == deckId);

            if (_deck == null)
            {
                MessageBox.Show("Deck no encontrado");
                Close();
                return;
            }

            txtDeckName.Text = _deck.deckName;

            _ = LoadStockAsync();
            LoadDeckCards();
            _ = LoadLeadersAsync();
            UpdateTotalCards();
        }

        #endregion

        #region Galería Stock

        private async Task LoadStockAsync()
        {
            flowStock.Controls.Clear();

            // Traemos solo datos fuertes (no dynamic) para EF
            var stock = _db.CardStock
                .Where(c => c.type != "Leader" && (c.units - c.usedCards) > 0)
                .Select(c => new
                {
                    c.Id,
                    c.cardName,
                    c.cardId,
                    c.color,
                    c.type,
                    c.subType,
                    c.cost,
                    c.counter,
                    StockLibre = c.units - c.usedCards,
                    c.cardImage
                })
                .OrderBy(c => c.cardName)
                .ToList();

            foreach (var card in stock)
            {
                var panel = await CrearPanelStock(card);
                flowStock.Controls.Add(panel);
            }
        }

        private async Task<Panel> CrearPanelStock(dynamic card)
        {
            var panel = new Panel
            {
                Width = 180,
                Height = 250,
                BackColor = Color.White,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            var pic = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 140,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            await ImageUtils.CargarImagenAsync(pic, card.cardImage);

            var lblName = new Label
            {
                Text = card.cardName,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblInfo = new Label
            {
                Text = $"Coste:{card.cost} | Counter:{card.counter} | {card.subType}\nID:{card.cardId}",
                Dock = DockStyle.Bottom,
                Font = new Font("Segoe UI", 8),
                Height = 50,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblInfo);
            panel.Controls.Add(lblName);
            panel.Controls.Add(pic);

            // Click para añadir carta
            panel.Click += (s, e) => AgregarCartaAlDeck(card.Id);
            pic.Click += (s, e) => AgregarCartaAlDeck(card.Id);
            lblName.Click += (s, e) => AgregarCartaAlDeck(card.Id);
            lblInfo.Click += (s, e) => AgregarCartaAlDeck(card.Id);

            return panel;
        }

        private void AgregarCartaAlDeck(Guid cardStockId)
        {
            var deckCard = _deckCardsInMemory
                .FirstOrDefault(dc => dc.cardStockId == cardStockId);

            var stockCard = _db.CardStock.First(c => c.Id == cardStockId);
            int maxUnits = stockCard.units;

            if (deckCard != null)
            {
                if (deckCard.quantity < maxUnits)
                    deckCard.quantity++;
            }
            else
            {
                deckCard = new DeckCard
                {
                    deckId = _deck.Id, // Temporal, solo para referencia, no se guarda aún
                    cardStockId = cardStockId,
                    quantity = 1
                };
                _deckCardsInMemory.Add(deckCard);
            }

            LoadDeckCardsFromMemory();
            UpdateTotalCards();
        }

        private void LoadDeckCardsFromMemory()
        {
            var cards = _deckCardsInMemory
                .Select(dc => {
                    var cardName = dc.CardStock != null
                        ? dc.CardStock.cardName
                        : _db.CardStock.First(c => c.Id == dc.cardStockId).cardName;

                    return new
                    {
                        dc.cardStockId,
                        cardName,
                        dc.quantity
                    };
                })
                .ToList();

            dgvDeck.DataSource = cards;
        }

        #endregion

        #region Deck DataGridView

        private void LoadDeckCards()
        {
            var cards = _db.DeckCards
                .Where(dc => dc.deckId == _deck.Id)
                .Select(dc => new
                {
                    dc.Id,
                    dc.cardStockId,
                    dc.CardStock.cardName,
                    dc.quantity
                })
                .ToList();

            dgvDeck.DataSource = cards;
        }

        #endregion

        #region Líderes

        private async Task LoadLeadersAsync()
        {
            flowLeaders.Controls.Clear();

            var leaders = _db.CardStock
                .Where(c => c.type == "Leader")
                .OrderBy(c => c.cardName)
                .ToList();

            foreach (var leader in leaders)
            {
                var panel = new Panel
                {
                    Width = 80,
                    Height = 100,
                    BackColor = Color.LightGray,
                    Margin = new Padding(5),
                    BorderStyle = BorderStyle.FixedSingle
                };

                var pic = new PictureBox
                {
                    Dock = DockStyle.Top,
                    Height = 60,
                    SizeMode = PictureBoxSizeMode.Zoom
                };

                await ImageUtils.CargarImagenAsync(pic, leader.cardImage);

                var lblName = new Label
                {
                    Text = leader.cardName,
                    Dock = DockStyle.Bottom,
                    Font = new Font("Segoe UI", 8, FontStyle.Bold),
                    Height = 20,
                    TextAlign = ContentAlignment.MiddleCenter
                };

                panel.Controls.Add(lblName);
                panel.Controls.Add(pic);

                panel.Click += (s, e) => SeleccionarLeader(leader, panel);
                pic.Click += (s, e) => SeleccionarLeader(leader, panel);
                lblName.Click += (s, e) => SeleccionarLeader(leader, panel);

                flowLeaders.Controls.Add(panel);

                // Si este líder ya está seleccionado
                if (_deck.leaderCardId.HasValue && _deck.leaderCardId.Value == leader.Id)
                    SeleccionarLeader(leader, panel);
            }
        }

        private void SeleccionarLeader(CardStock leader, Panel panel)
        {
            _deck.leaderCardId = leader.Id;

            if (_selectedLeaderPanel != null)
                _selectedLeaderPanel.BorderStyle = BorderStyle.FixedSingle;

            panel.BorderStyle = BorderStyle.Fixed3D;
            _selectedLeaderPanel = panel;
        }

        #endregion

        #region Total cartas y botones

        private void UpdateTotalCards()
        {
            int total = _db.DeckCards
                .Where(dc => dc.deckId == _deck.Id)
                .Sum(dc => (int?)dc.quantity) ?? 0;

            lblTotalCards.Text = $"Total cartas: {total} / 50";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeckName.Text))
            {
                MessageBox.Show("Debes poner un nombre al deck.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _deck.deckName = txtDeckName.Text.Trim();
            _deck.lastUpdatedDate = DateTime.Now;

            if (_deck.Id == Guid.Empty)
                _db.Decks.Add(_deck);

            _db.SaveChanges();

            // Guardar las cartas en la DB
            foreach (var dc in _deckCardsInMemory)
                dc.deckId = _deck.Id;

            _db.DeckCards.AddRange(_deckCardsInMemory);
            _db.SaveChanges();

            MessageBox.Show("Deck guardado correctamente", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
