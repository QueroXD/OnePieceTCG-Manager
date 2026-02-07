using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Services;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Decks
{
    public partial class FrmDeckEditor : Form
    {
        private readonly DecksService _decksService;
        private readonly CardStockService _cardStockService;
        private readonly string _codUsu;

        private Deck _deck;

        // Estado en memoria (editor)
        private Guid? _selectedLeaderId = null;          // líder actual en editor
        private readonly Dictionary<Guid, int> _cards = new Dictionary<Guid, int>(); // cardStockId -> qty (sin líder)

        // Estado “en DB” del deck cargado (para excluir su propio consumo al calcular disponibilidad)
        private Guid? _dbLeaderId = null;
        private readonly Dictionary<Guid, int> _dbCards = new Dictionary<Guid, int>();

        // Cache de cartas (para no reconsultar por cada click)
        private readonly Dictionary<Guid, CardStock> _stockById = new Dictionary<Guid, CardStock>();
        private readonly Dictionary<string, Image> _imgCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);

        private const int MAX_COPIES_PER_CARD = 4;
        private const int MAX_DECK_CARDS = 50; // sin líder (One Piece TCG suele ser 50 + 1 líder)

        public FrmDeckEditor(string codUsu, Guid? deckId = null)
        {
            InitializeComponent();
            pnlBottom_Resize(this, EventArgs.Empty);

            _codUsu = codUsu;
            _decksService = new DecksService();
            _cardStockService = new CardStockService();

            // eventos UI
            txtSearch.TextChanged += (s, e) => _ = ReloadCatalogAsync();
            chkShowNoStock.CheckedChanged += (s, e) => _ = ReloadCatalogAsync();

            btnClearFilters.Click += (s, e) =>
            {
                txtSearch.Text = "";
                chkShowNoStock.Checked = false;
            };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += (s, e) => Close();

            // carga
            if (deckId.HasValue)
                LoadExistingDeck(deckId.Value);
            else
                CreateNewDeck();
        }

        private void pnlBottom_Resize(object sender, EventArgs e)
        {
            int right = pnlBottom.ClientSize.Width - 12;

            btnCancel.Left = right - btnCancel.Width;
            btnCancel.Top = 12;

            right -= (btnCancel.Width + 10);

            btnSave.Left = right - btnSave.Width;
            btnSave.Top = 12;
        }

        private void CreateNewDeck()
        {
            _deck = new Deck
            {
                deckName = "Nuevo Deck",
                deckDescription = null,
                codUsu = _codUsu,
                isActive = true,
                createdDate = DateTime.Now,
                lastUpdatedDate = DateTime.Now
            };

            txtDeckName.Text = _deck.deckName;

            _selectedLeaderId = null;
            _dbLeaderId = null;
            _cards.Clear();
            _dbCards.Clear();

            _ = BootstrapAsync();
        }

        private async void LoadExistingDeck(Guid deckId)
        {
            DeckEditDto dto;

            try
            {
                dto = await _decksService.GetDeckForEditAsync(deckId);
            }
            catch
            {
                MessageBox.Show("Deck no encontrado", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }

            _deck = new Deck
            {
                Id = dto.Id,
                deckName = dto.deckName,
                deckDescription = dto.deckDescription,
                codUsu = dto.codUsu,
                isActive = dto.isActive,
                createdDate = dto.createdDate,
                lastUpdatedDate = dto.lastUpdatedDate,
                leaderCardId = dto.LeaderCardId
            };

            txtDeckName.Text = _deck.deckName ?? "";

            // Estado DB
            _dbLeaderId = dto.LeaderCardId;
            _dbCards.Clear();
            foreach (var dc in dto.DeckCards)
                _dbCards[dc.cardStockId] = dc.quantity;

            // Estado editor inicial
            _selectedLeaderId = _dbLeaderId;
            _cards.Clear();
            foreach (var kv in _dbCards)
                _cards[kv.Key] = kv.Value;

            await BootstrapAsync();
        }


        private async Task BootstrapAsync()
        {
            await LoadStockCacheAsync();
            await ReloadLeaderStripAsync();
            await ReloadDeckViewAsync();
            await ReloadCatalogAsync();
        }

        // ============================
        // Stock cache
        // ============================
        private async Task LoadStockCacheAsync()
        {
            _stockById.Clear();

            var all = await _cardStockService.GetAllAsync();
            foreach (var c in all)
                _stockById[c.Id] = c;
        }

        // ============================
        // Cálculos clave
        // ============================
        private int GetDbReserved(Guid cardId)
        {
            // Lo que este deck ya estaba usando en DB (para excluirlo al calcular disponibilidad)
            int qty = 0;
            if (_dbCards.TryGetValue(cardId, out var q)) qty += q;
            if (_dbLeaderId.HasValue && _dbLeaderId.Value == cardId) qty += 1;
            return qty;
        }

        private int GetEditorReserved(Guid cardId)
        {
            // Lo que el editor está reservando ahora mismo (estado actual)
            int qty = 0;
            if (_cards.TryGetValue(cardId, out var q)) qty += q;
            if (_selectedLeaderId.HasValue && _selectedLeaderId.Value == cardId) qty += 1;
            return qty;
        }

        private int GetAvailable(Guid cardId)
        {
            if (!_stockById.TryGetValue(cardId, out var stock)) return 0;

            // usedCards incluye todos los decks, incluido este mismo cuando editas.
            // Para que el editor no “se coma” su propio stock, excluimos lo que este deck ya consumía en DB.
            int usedElsewhere = stock.usedCards - GetDbReserved(cardId);
            if (usedElsewhere < 0) usedElsewhere = 0;

            int available = stock.units - usedElsewhere - GetEditorReserved(cardId);
            return Math.Max(0, available);
        }

        private int TotalNonLeaderCardsInEditor()
        {
            return _cards.Values.Sum();
        }

        // ============================
        // UI: Deck (lado izquierdo)
        // ============================
        private async Task ReloadDeckViewAsync()
        {
            // leader preview
            await RenderLeaderPreviewAsync();

            // list deck cards (sin líder)
            pnlDeckCards.Controls.Clear();

            var sorted = _cards
                .Where(kv => kv.Value > 0)
                .OrderBy(kv => _stockById.TryGetValue(kv.Key, out var c) ? c.cardName : "")
                .ToList();

            foreach (var kv in sorted)
            {
                var cardId = kv.Key;
                var qty = kv.Value;

                if (!_stockById.TryGetValue(cardId, out var stock)) continue;

                var row = await CreateDeckRowAsync(stock, qty);
                pnlDeckCards.Controls.Add(row);
            }

            // total
            lblTotal.Text = $"Cartas: {TotalNonLeaderCardsInEditor()} / {MAX_DECK_CARDS}  (Líder aparte: 1)";
        }

        private async Task RenderLeaderPreviewAsync()
        {
            if (_selectedLeaderId.HasValue && _stockById.TryGetValue(_selectedLeaderId.Value, out var leader))
            {
                lblLeaderName.Text = leader.cardName;
                var img = await LoadImageCachedAsync(leader.cardImage);
                picLeader.Image = img;
                btnClearLeader.Enabled = true;
            }
            else
            {
                lblLeaderName.Text = "Sin líder seleccionado";
                picLeader.Image = null;
                btnClearLeader.Enabled = false;
            }
        }

        private async Task<Panel> CreateDeckRowAsync(CardStock stock, int qty)
        {
            var panel = new Panel
            {
                Height = 76,
                Dock = DockStyle.Top,
                Padding = new Padding(8),
                Margin = new Padding(0, 0, 0, 8),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var pic = new PictureBox
            {
                Width = 56,
                Height = 56,
                SizeMode = PictureBoxSizeMode.Zoom,
                Left = 8,
                Top = 8
            };
            pic.Image = await LoadImageCachedAsync(stock.cardImage);

            var lblName = new Label
            {
                AutoEllipsis = true,
                Left = 72,
                Top = 10,
                Width = 440,
                Height = 20,
                Text = stock.cardName,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            var lblMeta = new Label
            {
                AutoEllipsis = true,
                Left = 72,
                Top = 34,
                Width = 440,
                Height = 18,
                Text = $"{stock.cardId} · {stock.color} · {stock.type} · {stock.subType}",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.DimGray
            };

            var lblQty = new Label
            {
                Text = qty.ToString(),
                Width = 36,
                Height = 32,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var btnMinus = new Button
            {
                Text = "−",
                Width = 36,
                Height = 32,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var btnPlus = new Button
            {
                Text = "+",
                Width = 36,
                Height = 32,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var btnRemove = new Button
            {
                Text = "Quitar",
                Width = 64,
                Height = 32,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // colocación a la derecha
            panel.Resize += (s, e) =>
            {
                int right = panel.ClientSize.Width - 8;
                btnRemove.Left = right - btnRemove.Width; btnRemove.Top = 20;
                right -= (btnRemove.Width + 8);

                btnPlus.Left = right - btnPlus.Width; btnPlus.Top = 20;
                right -= (btnPlus.Width + 4);

                lblQty.Left = right - lblQty.Width; lblQty.Top = 20;
                right -= (lblQty.Width + 4);

                btnMinus.Left = right - btnMinus.Width; btnMinus.Top = 20;
            };

            btnMinus.Click += (s, e) => DecreaseCard(stock.Id);
            btnPlus.Click += (s, e) => IncreaseCard(stock.Id);
            btnRemove.Click += (s, e) => RemoveCard(stock.Id);

            panel.Controls.Add(pic);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblMeta);
            panel.Controls.Add(btnMinus);
            panel.Controls.Add(lblQty);
            panel.Controls.Add(btnPlus);
            panel.Controls.Add(btnRemove);

            return panel;
        }

        // ============================
        // UI: Leader strip
        // ============================
        private async Task ReloadLeaderStripAsync()
        {
            flowLeaders.Controls.Clear();

            var leaders = _stockById.Values
                .Where(c => string.Equals(c.type, "Leader", StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.cardName)
                .ToList();

            foreach (var leader in leaders)
            {
                int available = GetAvailable(leader.Id);
                bool show = chkShowNoStock.Checked || available > 0;

                if (!show) continue;

                var card = await CreateCatalogCardAsync(leader, available, isLeader: true);
                flowLeaders.Controls.Add(card);
            }
        }

        // ============================
        // UI: Catalog (derecha)
        // ============================
        private async Task ReloadCatalogAsync()
        {
            flowCatalog.Controls.Clear();

            string q = (txtSearch.Text ?? "").Trim().ToLowerInvariant();

            var list = _stockById.Values
                .Where(c => !string.Equals(c.type, "Leader", StringComparison.OrdinalIgnoreCase)) // líderes arriba
                .Where(c =>
                {
                    if (string.IsNullOrWhiteSpace(q)) return true;
                    return (c.cardName ?? "").ToLowerInvariant().Contains(q)
                        || (c.cardId ?? "").ToLowerInvariant().Contains(q)
                        || (c.subType ?? "").ToLowerInvariant().Contains(q);
                })
                .OrderBy(c => c.cardName)
                .ToList();

            foreach (var stock in list)
            {
                int available = GetAvailable(stock.Id);
                bool show = chkShowNoStock.Checked || available > 0;

                if (!show) continue;

                var card = await CreateCatalogCardAsync(stock, available, isLeader: false);
                flowCatalog.Controls.Add(card);
            }
        }

        private async Task<Panel> CreateCatalogCardAsync(CardStock stock, int available, bool isLeader)
        {
            var panel = new Panel
            {
                Width = 200,
                Height = 285,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var pic = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 165,
                SizeMode = PictureBoxSizeMode.Zoom
            };
            pic.Image = await LoadImageCachedAsync(stock.cardImage);

            var lblName = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                AutoEllipsis = true,
                Padding = new Padding(6, 6, 6, 0),
                Text = stock.cardName
            };

            var lblAvail = new Label
            {
                Dock = DockStyle.Top,
                Height = 26,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Text = $"Disponible: {available}"
            };

            var lblMeta = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Regular),
                ForeColor = Color.DimGray,
                Padding = new Padding(6, 4, 6, 6),
                Text = $"{stock.cardId}\n{stock.color} · Coste:{stock.cost} · Counter:{stock.counter}\n{stock.subType}"
            };

            panel.Controls.Add(lblMeta);
            panel.Controls.Add(lblAvail);
            panel.Controls.Add(lblName);
            panel.Controls.Add(pic);

            bool enabled = available > 0;

            if (!enabled)
            {
                panel.BackColor = Color.Gainsboro;
                lblAvail.ForeColor = Color.DarkRed;
            }

            void OnClickAdd(object s, EventArgs e)
            {
                if (!enabled)
                {
                    MessageBox.Show("No hay stock disponible para esta carta.", "Sin stock",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (isLeader)
                {
                    SelectLeader(stock.Id);
                }
                else
                {
                    AddCard(stock.Id);
                }
            }

            panel.Click += OnClickAdd;
            pic.Click += OnClickAdd;
            lblName.Click += OnClickAdd;
            lblAvail.Click += OnClickAdd;
            lblMeta.Click += OnClickAdd;

            return panel;
        }

        // ============================
        // Acciones: Leader
        // ============================
        private void SelectLeader(Guid leaderId)
        {
            // Si es el mismo, nada
            if (_selectedLeaderId.HasValue && _selectedLeaderId.Value == leaderId) return;

            // ¿hay disponible? (ya descontando editor)
            int available = GetAvailable(leaderId);
            if (available <= 0)
            {
                MessageBox.Show("No hay stock disponible para seleccionar este líder.", "Sin stock",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _selectedLeaderId = leaderId;

            // refrescar UI
            _ = RefreshAllAsync();
        }

        private void btnClearLeader_Click(object sender, EventArgs e)
        {
            _selectedLeaderId = null;
            _ = RefreshAllAsync();
        }

        // ============================
        // Acciones: Cards
        // ============================
        private void AddCard(Guid cardId)
        {
            // regla: max 50 (sin líder)
            if (TotalNonLeaderCardsInEditor() >= MAX_DECK_CARDS)
            {
                MessageBox.Show($"No puedes añadir más de {MAX_DECK_CARDS} cartas al deck (sin contar el líder).",
                    "Límite de deck", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // regla: max 4 copias
            int current = _cards.TryGetValue(cardId, out var q) ? q : 0;
            if (current >= MAX_COPIES_PER_CARD)
            {
                MessageBox.Show($"No puedes añadir más de {MAX_COPIES_PER_CARD} copias de la misma carta.",
                    "Límite por carta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // stock disponible (ya descontando editor)
            int available = GetAvailable(cardId);
            if (available <= 0)
            {
                MessageBox.Show("No hay stock disponible para esta carta.", "Sin stock",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _cards[cardId] = current + 1;

            _ = RefreshAllAsync();
        }

        private void IncreaseCard(Guid cardId)
        {
            AddCard(cardId);
        }

        private void DecreaseCard(Guid cardId)
        {
            if (!_cards.TryGetValue(cardId, out var q)) return;
            if (q <= 1) _cards.Remove(cardId);
            else _cards[cardId] = q - 1;

            _ = RefreshAllAsync();
        }

        private void RemoveCard(Guid cardId)
        {
            if (_cards.ContainsKey(cardId))
                _cards.Remove(cardId);

            _ = RefreshAllAsync();
        }

        private async Task RefreshAllAsync()
        {
            await ReloadDeckViewAsync();
            await ReloadLeaderStripAsync();
            await ReloadCatalogAsync();
        }

        // ============================
        // Guardar
        // ============================
        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeckName.Text))
            {
                MessageBox.Show("Debes poner un nombre al deck.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_selectedLeaderId.HasValue)
            {
                MessageBox.Show("Debes seleccionar un líder.", "Falta líder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validación stock final
            if (!await ValidateEditorStockAsync())
                return;

            try
            {
                // Preparar DTO para enviar a la API
                var dto = new DeckSaveDto
                {
                    Id = _deck.Id, // Guid.Empty si es nuevo
                    CodUsu = _codUsu,
                    DeckName = txtDeckName.Text.Trim(),
                    LeaderCardId = _selectedLeaderId.Value,
                    DeckCards = _cards.Select(kv => new DeckCardDto
                    {
                        cardStockId = kv.Key,
                        quantity = kv.Value
                    }).ToList()
                };

                // Llamada API
                await _decksService.SaveDeckAsync(dto);

                MessageBox.Show("Deck guardado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error guardando deck:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async Task<bool> ValidateEditorStockAsync()
        {
            var ids = _cards.Keys.ToList();
            if (_selectedLeaderId.HasValue) ids.Add(_selectedLeaderId.Value);

            ids = ids.Distinct().ToList();

            // Traer stock actualizado desde API
            var freshStocks = await _cardStockService.GetByIdsAsync(ids);
            var freshById = freshStocks.ToDictionary(x => x.Id, x => x);

            foreach (var id in ids)
            {
                if (!freshById.TryGetValue(id, out var stock))
                {
                    MessageBox.Show("Hay cartas del deck que ya no existen en stock.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                int usedElsewhere = stock.usedCards - GetDbReserved(id);
                if (usedElsewhere < 0) usedElsewhere = 0;

                int need = GetEditorReserved(id);
                int available = stock.units - usedElsewhere - need;

                if (available < 0)
                {
                    MessageBox.Show(
                        $"No hay stock suficiente para guardar.\n\nCarta: {stock.cardName}\nNecesitas: {need}\nDisponible real: {stock.units - usedElsewhere}",
                        "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (!(_selectedLeaderId.HasValue) && string.Equals(stock.type, "Leader", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Debes seleccionar un líder.", "Falta líder",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (_cards.TryGetValue(id, out var q) && q > MAX_COPIES_PER_CARD)
                {
                    MessageBox.Show($"Carta {stock.cardName} supera el máximo de {MAX_COPIES_PER_CARD} copias.", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (TotalNonLeaderCardsInEditor() > MAX_DECK_CARDS)
            {
                MessageBox.Show($"El deck supera {MAX_DECK_CARDS} cartas (sin contar el líder).", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


        // ============================
        // Imagen cache
        // ============================
        private async Task<Image> LoadImageCachedAsync(string pathOrUrl)
        {
            if (string.IsNullOrWhiteSpace(pathOrUrl)) return null;

            if (_imgCache.TryGetValue(pathOrUrl, out var cached))
                return cached;

            var img = await ImageUtils.TryLoadImageAsync(pathOrUrl);
            if (img != null)
                _imgCache[pathOrUrl] = img;

            return img;
        }
    }
}
