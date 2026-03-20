using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Services;
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
        private readonly DecksService _decksService;
        private readonly CardStockService _cardStockService;
        private readonly string _codUsu;

        private Deck _deck;
        private Guid? _selectedLeaderId;
        private readonly Dictionary<Guid, int> _cards = new Dictionary<Guid, int>();
        private Guid? _dbLeaderId;
        private readonly Dictionary<Guid, int> _dbCards = new Dictionary<Guid, int>();
        private readonly Dictionary<Guid, CardStock> _stockById = new Dictionary<Guid, CardStock>();
        private readonly Dictionary<string, Image> _imgCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
        private HashSet<string> _allowedLeaderColors = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private bool _isLeaderSelectionMode;

        private Label _lblDeckCount;
        private Label _lblUniqueCount;
        private Label _lblCurve;

        private const int MAX_COPIES_PER_CARD = 4;
        private const int MAX_DECK_CARDS = 50;

        public FrmDeckEditor(string codUsu, Guid? deckId = null)
        {
            InitializeComponent();
            _codUsu = codUsu;
            _decksService = new DecksService();
            _cardStockService = new CardStockService();

            InitializeModernLayout();
            pnlBottom_Resize(this, EventArgs.Empty);

            txtSearch.TextChanged += (s, e) => _ = ReloadCatalogAsync();
            chkShowNoStock.CheckedChanged += (s, e) => _ = RefreshAllAsync();

            btnClearFilters.Click += (s, e) =>
            {
                txtSearch.Text = string.Empty;
                chkShowNoStock.Checked = false;
            };

            btnSave.Click += btnSave_Click;
            btnCancel.Click += (s, e) => Close();

            if (deckId.HasValue)
                LoadExistingDeck(deckId.Value);
            else
                CreateNewDeck();
        }

        private void InitializeModernLayout()
        {
            ModernUi.ApplyFormTheme(this);
            BackColor = ModernUi.AppBack;
            lblTitle.Text = "Construccion de deck";
            lblTitle.ForeColor = ModernUi.TextPrimary;
            lblDeckName.ForeColor = ModernUi.TextPrimary;

            ModernUi.StylePanelCard(pnlLeader);
            ModernUi.StylePanelCard(pnlFilters);
            pnlBottom.BackColor = ModernUi.Surface;
            pnlLeft.BackColor = ModernUi.AppBack;
            pnlRight.BackColor = ModernUi.AppBack;
            pnlDeckCards.BackColor = ModernUi.AppBack;
            flowCatalog.BackColor = ModernUi.AppBack;
            flowLeaders.BackColor = ModernUi.AppBack;

            ModernUi.StyleInput(txtDeckName);
            ModernUi.StyleInput(txtSearch);
            ModernUi.StyleButton(btnSave, ModernUi.Accent, Color.White);
            ModernUi.StyleOutlineButton(btnCancel);
            ModernUi.StyleOutlineButton(btnClearFilters);
            ModernUi.StyleOutlineButton(btnClearLeader);

            lblCatalogTitle.ForeColor = ModernUi.TextPrimary;
            lblLeadersTitle.ForeColor = ModernUi.TextPrimary;
            lblDeckCardsTitle.ForeColor = ModernUi.TextPrimary;
            lblLeaderName.ForeColor = ModernUi.TextPrimary;
            lblTotal.ForeColor = ModernUi.TextMuted;
            lblSearch.ForeColor = ModernUi.TextPrimary;
            chkShowNoStock.ForeColor = ModernUi.TextPrimary;

            tlpRoot.RowStyles[0].Height = 96F;
            pnlTop.Padding = new Padding(16, 12, 16, 12);
            pnlTop.Resize += PnlTop_Resize;
            splitMain.SplitterDistance = Math.Max(440, Width / 2);
            splitMain.Panel1MinSize = 360;
            splitMain.Panel2MinSize = 360;

            BuildTopStats();
            PnlTop_Resize(this, EventArgs.Empty);
        }

        private void BuildTopStats()
        {
            _lblDeckCount = CreateHeaderStat("0/50", new Point(860, 10));
            _lblUniqueCount = CreateHeaderStat("0 unicas", new Point(980, 10));
            _lblCurve = CreateHeaderStat("Curva 0.0", new Point(1120, 10));
        }

        private Label CreateHeaderStat(string text, Point location)
        {
            var label = new Label
            {
                AutoSize = false,
                Size = new Size(112, 48),
                Location = location,
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary,
                Text = text
            };

            pnlTop.Controls.Add(label);
            return label;
        }

        private void PnlTop_Resize(object sender, EventArgs e)
        {
            txtDeckName.Width = Math.Min(360, Math.Max(210, pnlTop.Width - 560));
            int right = pnlTop.Width - 16;
            _lblCurve.Left = right - _lblCurve.Width;
            _lblCurve.Top = 18;
            right -= _lblCurve.Width + 8;
            _lblUniqueCount.Left = right - _lblUniqueCount.Width;
            _lblUniqueCount.Top = 18;
            right -= _lblUniqueCount.Width + 8;
            _lblDeckCount.Left = right - _lblDeckCount.Width;
            _lblDeckCount.Top = 18;
        }

        private void pnlBottom_Resize(object sender, EventArgs e)
        {
            int top = Math.Max(10, (pnlBottom.ClientSize.Height - btnSave.Height) / 2);
            int right = pnlBottom.ClientSize.Width - 12;
            btnCancel.Left = right - btnCancel.Width;
            btnCancel.Top = top;
            right -= btnCancel.Width + 10;
            btnSave.Left = right - btnSave.Width;
            btnSave.Top = top;
        }

        private void CreateNewDeck()
        {
            _deck = new Deck
            {
                deckName = "Nuevo Deck",
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
            SetLeaderSelectionMode(true);
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

            txtDeckName.Text = _deck.deckName ?? string.Empty;
            _dbLeaderId = dto.LeaderCardId;
            _dbCards.Clear();
            foreach (var dc in dto.DeckCards)
                _dbCards[dc.cardStockId] = dc.quantity;

            _selectedLeaderId = _dbLeaderId;
            _cards.Clear();
            foreach (var kv in _dbCards)
                _cards[kv.Key] = kv.Value;

            SetLeaderSelectionMode(false);
            await BootstrapAsync();
        }

        private async Task BootstrapAsync()
        {
            await LoadStockCacheAsync();
            RefreshLeaderRules();
            await RefreshAllAsync();
        }

        private async Task LoadStockCacheAsync()
        {
            _stockById.Clear();
            var all = await _cardStockService.GetAllAsync();
            foreach (var c in all)
                _stockById[c.Id] = c;
        }

        private int GetDbReserved(Guid cardId)
        {
            int qty = 0;
            if (_dbCards.TryGetValue(cardId, out var q)) qty += q;
            if (_dbLeaderId.HasValue && _dbLeaderId.Value == cardId) qty += 1;
            return qty;
        }

        private int GetEditorReserved(Guid cardId)
        {
            int qty = 0;
            if (_cards.TryGetValue(cardId, out var q)) qty += q;
            if (_selectedLeaderId.HasValue && _selectedLeaderId.Value == cardId) qty += 1;
            return qty;
        }

        private int GetAvailable(Guid cardId)
        {
            if (!_stockById.TryGetValue(cardId, out var stock)) return 0;

            int usedElsewhere = stock.usedCards - GetDbReserved(cardId);
            if (usedElsewhere < 0) usedElsewhere = 0;

            int available = stock.units - usedElsewhere - GetEditorReserved(cardId);
            return Math.Max(0, available);
        }

        private int TotalNonLeaderCardsInEditor()
        {
            return _cards.Values.Sum();
        }

        private async Task ReloadDeckViewAsync()
        {
            await RenderLeaderPreviewAsync();
            pnlDeckCards.SuspendLayout();
            pnlDeckCards.Controls.Clear();

            var sorted = _cards
                .Where(kv => kv.Value > 0)
                .OrderByDescending(kv => kv.Value)
                .ThenBy(kv => _stockById.TryGetValue(kv.Key, out var c) ? c.cardName : string.Empty)
                .ToList();

            foreach (var kv in sorted)
            {
                if (_stockById.TryGetValue(kv.Key, out var stock))
                    pnlDeckCards.Controls.Add(await CreateDeckRowAsync(stock, kv.Value));
            }

            pnlDeckCards.ResumeLayout(true);
            lblTotal.Text = string.Format("Cartas: {0} / {1}  |  Colores validos: {2}", TotalNonLeaderCardsInEditor(), MAX_DECK_CARDS, DescribeAllowedColors());
            UpdateHeaderStats();
        }

        private async Task RenderLeaderPreviewAsync()
        {
            if (_selectedLeaderId.HasValue && _stockById.TryGetValue(_selectedLeaderId.Value, out var leader))
            {
                lblLeaderName.Text = string.Format("{0}  |  {1}", leader.cardName, DescribeColors(ParseCardColors(leader.color)));
                picLeader.Image = await LoadImageCachedAsync(leader.cardImage);
                btnClearLeader.Enabled = true;
                btnClearLeader.Text = "Cambiar lider";
            }
            else
            {
                lblLeaderName.Text = "Elige un lider para empezar a construir el deck";
                picLeader.Image = null;
                btnClearLeader.Enabled = false;
                btnClearLeader.Text = "Cambiar lider";
            }
        }

        private void UpdateHeaderStats()
        {
            var selectedCards = _cards.Keys.Where(id => _stockById.ContainsKey(id)).Select(id => _stockById[id]).ToList();
            double avgCost = selectedCards.Count == 0 ? 0 : selectedCards.Average(c => c.cost);
            _lblDeckCount.Text = string.Format("{0}/50", TotalNonLeaderCardsInEditor());
            _lblUniqueCount.Text = string.Format("{0} unicas", _cards.Count);
            _lblCurve.Text = string.Format("Curva {0:0.0}", avgCost);
        }

        private async Task<Panel> CreateDeckRowAsync(CardStock stock, int qty)
        {
            var panel = new Panel
            {
                Height = 88,
                Dock = DockStyle.Top,
                Padding = new Padding(10),
                Margin = new Padding(0, 0, 0, 8),
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var pic = new PictureBox
            {
                Width = 56,
                Height = 66,
                SizeMode = PictureBoxSizeMode.Zoom,
                Left = 10,
                Top = 10,
                Image = await LoadImageCachedAsync(stock.cardImage)
            };

            var lblName = new Label
            {
                AutoEllipsis = true,
                Left = 76,
                Top = 10,
                Width = 300,
                Height = 20,
                Text = stock.cardName,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary
            };

            var lblMeta = new Label
            {
                AutoEllipsis = true,
                Left = 76,
                Top = 34,
                Width = 340,
                Height = 18,
                Text = string.Format("{0} | {1} | Coste {2} | Counter {3}", stock.cardId, stock.color, stock.cost, stock.counter),
                Font = new Font("Segoe UI", 8.8F),
                ForeColor = ModernUi.TextMuted
            };

            var lblSubtype = new Label
            {
                AutoEllipsis = true,
                Left = 76,
                Top = 54,
                Width = 340,
                Height = 18,
                Text = string.IsNullOrWhiteSpace(stock.subType) ? stock.type : stock.subType,
                Font = new Font("Segoe UI", 8.8F),
                ForeColor = ModernUi.TextMuted
            };

            var lblQty = new Label
            {
                Text = qty.ToString(),
                Width = 40,
                Height = 32,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                BackColor = ModernUi.Navy,
                ForeColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            var btnMinus = new Button { Text = "-", Width = 34, Height = 32, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            var btnPlus = new Button { Text = "+", Width = 34, Height = 32, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            var btnRemove = new Button { Text = "Quitar", Width = 70, Height = 32, Anchor = AnchorStyles.Top | AnchorStyles.Right };
            ModernUi.StyleOutlineButton(btnMinus);
            ModernUi.StyleOutlineButton(btnPlus);
            ModernUi.StyleOutlineButton(btnRemove);

            panel.Resize += (s, e) =>
            {
                int right = panel.ClientSize.Width - 10;
                btnRemove.Left = right - btnRemove.Width; btnRemove.Top = 24;
                right -= btnRemove.Width + 8;
                btnPlus.Left = right - btnPlus.Width; btnPlus.Top = 24;
                right -= btnPlus.Width + 6;
                lblQty.Left = right - lblQty.Width; lblQty.Top = 24;
                right -= lblQty.Width + 6;
                btnMinus.Left = right - btnMinus.Width; btnMinus.Top = 24;
                int contentWidth = Math.Max(180, btnMinus.Left - 86);
                lblName.Width = contentWidth;
                lblMeta.Width = contentWidth;
                lblSubtype.Width = contentWidth;
            };

            btnMinus.Click += (s, e) => DecreaseCard(stock.Id);
            btnPlus.Click += (s, e) => IncreaseCard(stock.Id);
            btnRemove.Click += (s, e) => RemoveCard(stock.Id);

            panel.Controls.Add(pic);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblMeta);
            panel.Controls.Add(lblSubtype);
            panel.Controls.Add(btnMinus);
            panel.Controls.Add(lblQty);
            panel.Controls.Add(btnPlus);
            panel.Controls.Add(btnRemove);
            return panel;
        }
        private async Task ReloadLeaderStripAsync()
        {
            flowLeaders.SuspendLayout();
            flowLeaders.Controls.Clear();

            bool shouldShowLeaders = _isLeaderSelectionMode || !_selectedLeaderId.HasValue;
            lblLeadersTitle.Visible = shouldShowLeaders;
            flowLeaders.Visible = shouldShowLeaders;

            if (shouldShowLeaders)
            {
                var leaders = _stockById.Values
                    .Where(c => string.Equals(c.type, "Leader", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(c => c.cardName)
                    .ToList();

                foreach (var leader in leaders)
                {
                    int available = GetAvailable(leader.Id);
                    bool show = chkShowNoStock.Checked || available > 0;
                    if (show)
                        flowLeaders.Controls.Add(await CreateCatalogCardAsync(leader, available, true));
                }
            }

            flowLeaders.ResumeLayout(true);
            UpdateSectionTexts();
        }

        private async Task ReloadCatalogAsync()
        {
            flowCatalog.SuspendLayout();
            flowCatalog.Controls.Clear();

            if (!_selectedLeaderId.HasValue)
            {
                flowCatalog.Controls.Add(CreateEmptyStateCard("Selecciona un lider para cargar el catalogo del deck."));
                flowCatalog.ResumeLayout(true);
                UpdateSectionTexts();
                return;
            }

            string q = (txtSearch.Text ?? string.Empty).Trim().ToLowerInvariant();
            var list = _stockById.Values
                .Where(c => !string.Equals(c.type, "Leader", StringComparison.OrdinalIgnoreCase))
                .Where(IsCardAllowedByLeader)
                .Where(c =>
                {
                    if (string.IsNullOrWhiteSpace(q)) return true;
                    return (c.cardName ?? string.Empty).ToLowerInvariant().Contains(q)
                        || (c.cardId ?? string.Empty).ToLowerInvariant().Contains(q)
                        || (c.subType ?? string.Empty).ToLowerInvariant().Contains(q)
                        || (c.color ?? string.Empty).ToLowerInvariant().Contains(q);
                })
                .OrderBy(c => c.cardName)
                .ToList();

            foreach (var stock in list)
            {
                int available = GetAvailable(stock.Id);
                bool show = chkShowNoStock.Checked || available > 0;
                if (show)
                    flowCatalog.Controls.Add(await CreateCatalogCardAsync(stock, available, false));
            }

            if (flowCatalog.Controls.Count == 0)
                flowCatalog.Controls.Add(CreateEmptyStateCard("No hay cartas disponibles con los filtros y colores actuales."));

            flowCatalog.ResumeLayout(true);
            UpdateSectionTexts();
        }

        private Panel CreateEmptyStateCard(string message)
        {
            var panel = new Panel
            {
                Width = Math.Max(260, flowCatalog.ClientSize.Width - 40),
                Height = 120,
                Margin = new Padding(10),
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lbl = new Label
            {
                Dock = DockStyle.Fill,
                Text = message,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = ModernUi.TextMuted
            };

            panel.Controls.Add(lbl);
            return panel;
        }

        private async Task<Panel> CreateCatalogCardAsync(CardStock stock, int available, bool isLeader)
        {
            var panel = new Panel
            {
                Width = isLeader ? 192 : 198,
                Height = isLeader ? 286 : 296,
                Margin = new Padding(10),
                BackColor = available > 0 ? ModernUi.Surface : Color.FromArgb(238, 240, 242),
                BorderStyle = BorderStyle.FixedSingle,
                Cursor = Cursors.Hand
            };

            var pic = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 160,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ModernUi.SurfaceAlt,
                Image = await LoadImageCachedAsync(stock.cardImage)
            };

            var lblName = new Label
            {
                Dock = DockStyle.Top,
                Height = 48,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9.5f, FontStyle.Bold),
                AutoEllipsis = true,
                Padding = new Padding(6, 6, 6, 0),
                Text = stock.cardName,
                ForeColor = ModernUi.TextPrimary
            };

            var lblAvail = new Label
            {
                Dock = DockStyle.Top,
                Height = 24,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                ForeColor = available > 0 ? ModernUi.Success : ModernUi.Danger,
                Text = string.Format("Disponible: {0}", available)
            };

            var metaText = isLeader
                ? string.Format("{0}\n{1}\nVida {2} | {3}", stock.cardId, DescribeColors(ParseCardColors(stock.color)), stock.life, stock.attribute)
                : string.Format("{0}\n{1}\nCoste {2} | Counter {3}\n{4}", stock.cardId, DescribeColors(ParseCardColors(stock.color)), stock.cost, stock.counter, string.IsNullOrWhiteSpace(stock.subType) ? stock.type : stock.subType);

            var lblMeta = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.TopCenter,
                Font = new Font("Segoe UI", 8.4f),
                ForeColor = ModernUi.TextMuted,
                Padding = new Padding(6, 4, 6, 6),
                Text = metaText
            };

            var lblAction = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 28,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = available > 0 ? (isLeader ? ModernUi.Navy : ModernUi.Accent) : ModernUi.TextMuted,
                Text = available > 0 ? (isLeader ? "Seleccionar lider" : "Anadir al deck") : "Sin stock"
            };

            panel.Controls.Add(lblMeta);
            panel.Controls.Add(lblAvail);
            panel.Controls.Add(lblName);
            panel.Controls.Add(pic);
            panel.Controls.Add(lblAction);

            bool enabled = available > 0;

            async void OnClickAdd(object s, EventArgs e)
            {
                if (!enabled)
                {
                    MessageBox.Show("No hay stock disponible para esta carta.", "Sin stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (isLeader) await SelectLeaderAsync(stock.Id);
                else AddCard(stock.Id);
            }

            panel.Click += OnClickAdd;
            pic.Click += OnClickAdd;
            lblName.Click += OnClickAdd;
            lblAvail.Click += OnClickAdd;
            lblMeta.Click += OnClickAdd;
            lblAction.Click += OnClickAdd;
            return panel;
        }

        private async Task SelectLeaderAsync(Guid leaderId)
        {
            if (_selectedLeaderId.HasValue && _selectedLeaderId.Value == leaderId)
            {
                SetLeaderSelectionMode(false);
                await RefreshAllAsync();
                return;
            }

            if (GetAvailable(leaderId) <= 0)
            {
                MessageBox.Show("No hay stock disponible para seleccionar este lider.", "Sin stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var nextColors = GetColorsForLeader(leaderId);
            var incompatible = _cards.Keys
                .Where(id => _stockById.ContainsKey(id) && !IsCardAllowedByColors(_stockById[id], nextColors))
                .ToList();

            if (_selectedLeaderId.HasValue)
            {
                string message = incompatible.Count > 0
                    ? string.Format("Si cambias el lider, se eliminaran {0} cartas incompatibles con los nuevos colores.\n\nQuieres continuar?", incompatible.Count)
                    : "Vas a cambiar el lider del deck.\n\nQuieres continuar?";

                if (MessageBox.Show(message, "Cambiar lider", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
            }

            foreach (var id in incompatible)
                _cards.Remove(id);

            _selectedLeaderId = leaderId;
            RefreshLeaderRules();
            SetLeaderSelectionMode(false);
            await RefreshAllAsync();
        }

        private async void btnClearLeader_Click(object sender, EventArgs e)
        {
            if (!_selectedLeaderId.HasValue)
                return;

            if (MessageBox.Show("Quieres elegir otro lider? Si eliges uno con otros colores, las cartas incompatibles se quitaran del deck.", "Cambiar lider", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            SetLeaderSelectionMode(true);
            await RefreshAllAsync();
        }

        private void AddCard(Guid cardId)
        {
            if (!_selectedLeaderId.HasValue)
            {
                MessageBox.Show("Primero debes seleccionar un lider.", "Falta lider", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_stockById.TryGetValue(cardId, out var stock))
                return;

            if (!IsCardAllowedByLeader(stock))
            {
                MessageBox.Show(string.Format("Solo puedes anadir cartas de estos colores: {0}.", DescribeAllowedColors()), "Regla de color", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (TotalNonLeaderCardsInEditor() >= MAX_DECK_CARDS)
            {
                MessageBox.Show(string.Format("No puedes anadir mas de {0} cartas al deck (sin contar el lider).", MAX_DECK_CARDS), "Limite de deck", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int current = _cards.TryGetValue(cardId, out var q) ? q : 0;
            if (current >= MAX_COPIES_PER_CARD)
            {
                MessageBox.Show(string.Format("No puedes anadir mas de {0} copias de la misma carta.", MAX_COPIES_PER_CARD), "Limite por carta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (GetAvailable(cardId) <= 0)
            {
                MessageBox.Show("No hay stock disponible para esta carta.", "Sin stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            RefreshLeaderRules();
            await ReloadDeckViewAsync();
            await ReloadLeaderStripAsync();
            await ReloadCatalogAsync();
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDeckName.Text))
            {
                MessageBox.Show("Debes poner un nombre al deck.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!_selectedLeaderId.HasValue)
            {
                MessageBox.Show("Debes seleccionar un lider.", "Falta lider", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!await ValidateEditorStockAsync())
                return;

            try
            {
                var dto = new DeckSaveDto
                {
                    Id = _deck.Id,
                    CodUsu = _codUsu,
                    DeckName = txtDeckName.Text.Trim(),
                    LeaderCardId = _selectedLeaderId.Value,
                    DeckCards = _cards.Select(kv => new DeckCardDto { cardStockId = kv.Key, quantity = kv.Value }).ToList()
                };

                await _decksService.SaveDeckAsync(dto);
                MessageBox.Show("Deck guardado correctamente.", "Exito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            var freshStocks = await _cardStockService.GetByIdsAsync(ids);
            var freshById = freshStocks.ToDictionary(x => x.Id, x => x);

            foreach (var id in ids)
            {
                if (!freshById.TryGetValue(id, out var stock))
                {
                    MessageBox.Show("Hay cartas del deck que ya no existen en stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!string.Equals(stock.type, "Leader", StringComparison.OrdinalIgnoreCase) && !IsCardAllowedByLeader(stock))
                {
                    MessageBox.Show(string.Format("La carta {0} no coincide con los colores del lider seleccionado.", stock.cardName), "Regla de color", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                int usedElsewhere = stock.usedCards - GetDbReserved(id);
                if (usedElsewhere < 0) usedElsewhere = 0;

                int need = GetEditorReserved(id);
                int available = stock.units - usedElsewhere - need;
                if (available < 0)
                {
                    MessageBox.Show(string.Format("No hay stock suficiente para guardar.\n\nCarta: {0}\nNecesitas: {1}\nDisponible real: {2}", stock.cardName, need, stock.units - usedElsewhere), "Stock insuficiente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (_cards.TryGetValue(id, out var q) && q > MAX_COPIES_PER_CARD)
                {
                    MessageBox.Show(string.Format("Carta {0} supera el maximo de {1} copias.", stock.cardName, MAX_COPIES_PER_CARD), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }

            if (TotalNonLeaderCardsInEditor() > MAX_DECK_CARDS)
            {
                MessageBox.Show(string.Format("El deck supera {0} cartas (sin contar el lider).", MAX_DECK_CARDS), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void RefreshLeaderRules()
        {
            _allowedLeaderColors = _selectedLeaderId.HasValue ? GetColorsForLeader(_selectedLeaderId.Value) : new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        private HashSet<string> GetColorsForLeader(Guid leaderId)
        {
            if (!_stockById.TryGetValue(leaderId, out var leader))
                return new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            return ParseCardColors(leader.color);
        }

        private HashSet<string> ParseCardColors(string colors)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(colors))
                return result;

            var normalized = colors
                .Replace(" y ", "/")
                .Replace("&", "/")
                .Replace(",", "/")
                .Replace("+", "/");

            foreach (var part in normalized.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var value = part.Trim();
                if (!string.IsNullOrWhiteSpace(value))
                    result.Add(value);
            }

            return result;
        }

        private bool IsCardAllowedByLeader(CardStock stock)
        {
            return IsCardAllowedByColors(stock, _allowedLeaderColors);
        }

        private bool IsCardAllowedByColors(CardStock stock, HashSet<string> allowedColors)
        {
            if (stock == null)
                return false;

            if (string.Equals(stock.type, "Leader", StringComparison.OrdinalIgnoreCase))
                return true;

            if (allowedColors == null || allowedColors.Count == 0)
                return false;

            var cardColors = ParseCardColors(stock.color);
            return cardColors.Count == 0 || cardColors.Any(allowedColors.Contains);
        }

        private string DescribeAllowedColors()
        {
            return DescribeColors(_allowedLeaderColors);
        }

        private string DescribeColors(IEnumerable<string> colors)
        {
            var list = (colors ?? Enumerable.Empty<string>()).Where(x => !string.IsNullOrWhiteSpace(x)).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            return list.Count == 0 ? "pendiente de lider" : string.Join(" / ", list);
        }

        private void SetLeaderSelectionMode(bool enabled)
        {
            _isLeaderSelectionMode = enabled;
            UpdateSectionTexts();
        }

        private void UpdateSectionTexts()
        {
            bool hasLeader = _selectedLeaderId.HasValue;
            lblLeadersTitle.Text = hasLeader ? "Cambiar lider" : "1. Elige tu lider";
            lblCatalogTitle.Text = hasLeader
                ? string.Format("2. Cartas disponibles para {0}", DescribeAllowedColors())
                : "2. Catalogo bloqueado hasta elegir lider";
        }

        private async Task<Image> LoadImageCachedAsync(string pathOrUrl)
        {
            if (string.IsNullOrWhiteSpace(pathOrUrl)) return null;
            if (_imgCache.TryGetValue(pathOrUrl, out var cached)) return cached;

            var img = await ImageUtils.TryLoadImageAsync(pathOrUrl);
            if (img != null)
                _imgCache[pathOrUrl] = img;
            return img;
        }
    }
}
