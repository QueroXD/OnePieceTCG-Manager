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
    public class FrmDeckBrowser : Form
    {
        private readonly DecksService _decksService = new DecksService();
        private readonly CardStockService _cardStockService = new CardStockService();
        private readonly Dictionary<string, Image> _imageCache = new Dictionary<string, Image>(StringComparer.OrdinalIgnoreCase);
        private readonly string _currentUserCode;

        private TextBox _txtUserCode;
        private Button _btnLoad;
        private DataGridView _dgvDecks;
        private Label _lblHeader;
        private Label _lblDeckName;
        private Label _lblDeckMeta;
        private Label _lblReadOnly;
        private PictureBox _picLeader;
        private FlowLayoutPanel _flowCards;

        public FrmDeckBrowser(string currentUserCode = null)
        {
            _currentUserCode = currentUserCode ?? string.Empty;
            InitializeUi();
        }

        private void InitializeUi()
        {
            ModernUi.ApplyFormTheme(this);
            Text = "Explorar decks";
            WindowState = FormWindowState.Maximized;
            MinimumSize = new Size(1100, 700);

            var top = new Panel
            {
                Dock = DockStyle.Top,
                Height = 72,
                Padding = new Padding(16, 14, 16, 14),
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            _lblHeader = new Label
            {
                Text = "Explora decks de cualquier usuario en modo solo lectura",
                AutoSize = true,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary,
                Location = new Point(16, 18)
            };

            var lblPrompt = new Label
            {
                Text = "codUsu:",
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary,
                Location = new Point(540, 23)
            };

            _txtUserCode = new TextBox
            {
                Location = new Point(602, 19),
                Size = new Size(140, 28),
                Text = _currentUserCode
            };
            ModernUi.StyleInput(_txtUserCode);
            _txtUserCode.KeyDown += async (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.SuppressKeyPress = true;
                    await LoadDecksForUserAsync();
                }
            };

            _btnLoad = new Button
            {
                Text = "Cargar decks",
                Location = new Point(752, 17),
                Size = new Size(110, 32)
            };
            ModernUi.StyleButton(_btnLoad, ModernUi.Accent, Color.White);
            _btnLoad.Click += async (s, e) => await LoadDecksForUserAsync();

            top.Controls.Add(_lblHeader);
            top.Controls.Add(lblPrompt);
            top.Controls.Add(_txtUserCode);
            top.Controls.Add(_btnLoad);

            var split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                SplitterDistance = 430,
                BackColor = ModernUi.AppBack
            };

            split.Panel1.Padding = new Padding(12);
            split.Panel2.Padding = new Padding(0, 12, 12, 12);

            _dgvDecks = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                RowTemplate = { Height = 56 }
            };
            ModernUi.StyleDataGridView(_dgvDecks);
            _dgvDecks.SelectionChanged += async (s, e) => await LoadSelectedDeckAsync();
            _dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { Name = "Id", DataPropertyName = nameof(DeckRow.Id), Visible = false });
            _dgvDecks.Columns.Add(new DataGridViewImageColumn { HeaderText = "Líder", DataPropertyName = nameof(DeckRow.LeaderImage), Width = 72, ImageLayout = DataGridViewImageCellLayout.Zoom });
            _dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Deck", DataPropertyName = nameof(DeckRow.DeckName), AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            _dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Cartas", DataPropertyName = nameof(DeckRow.TotalCards), Width = 70 });
            _dgvDecks.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Actualizado", DataPropertyName = nameof(DeckRow.LastUpdated), Width = 130, DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM HH:mm" } });

            split.Panel1.Controls.Add(_dgvDecks);

            var detail = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(16)
            };

            _lblDeckName = new Label
            {
                Dock = DockStyle.Top,
                Height = 34,
                Text = "Selecciona un deck",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary
            };

            _lblDeckMeta = new Label
            {
                Dock = DockStyle.Top,
                Height = 42,
                Text = "Busca un codUsu para cargar decks compartidos.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ModernUi.TextMuted
            };

            _lblReadOnly = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Text = "Modo lectura: puedes revisar composición, líder y cantidades, pero no editar.",
                Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold),
                ForeColor = ModernUi.Warning
            };

            var leaderHost = new Panel
            {
                Dock = DockStyle.Top,
                Height = 170,
                Padding = new Padding(0, 8, 0, 12),
                BackColor = ModernUi.Surface
            };

            _picLeader = new PictureBox
            {
                Size = new Size(120, 150),
                Location = new Point(0, 6),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = ModernUi.SurfaceAlt
            };

            leaderHost.Controls.Add(_picLeader);

            var lblCardsTitle = new Label
            {
                Dock = DockStyle.Top,
                Height = 28,
                Text = "Lista del deck",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary
            };

            _flowCards = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ModernUi.AppBack,
                Padding = new Padding(0, 4, 0, 4)
            };

            detail.Controls.Add(_flowCards);
            detail.Controls.Add(lblCardsTitle);
            detail.Controls.Add(leaderHost);
            detail.Controls.Add(_lblReadOnly);
            detail.Controls.Add(_lblDeckMeta);
            detail.Controls.Add(_lblDeckName);
            split.Panel2.Controls.Add(detail);

            Controls.Add(split);
            Controls.Add(top);

            Shown += async (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(_txtUserCode.Text))
                    await LoadDecksForUserAsync();
            };
        }

        private async Task LoadDecksForUserAsync()
        {
            var codUsu = (_txtUserCode.Text ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(codUsu))
            {
                MessageBox.Show("Introduce un codUsu para buscar decks.", "Buscar decks", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                var decks = await _decksService.GetDecksByUserAsync(codUsu) ?? new List<DeckRow>();
                _dgvDecks.DataSource = decks;
                await LoadThumbnailsAsync(decks);

                if (decks.Count == 0)
                {
                    _lblDeckName.Text = "Sin decks para este usuario";
                    _lblDeckMeta.Text = "Prueba con otro codUsu o revisa si el usuario tiene decks guardados.";
                    _picLeader.Image = null;
                    _flowCards.Controls.Clear();
                    return;
                }

                if (_dgvDecks.Rows.Count > 0)
                    _dgvDecks.Rows[0].Selected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los decks:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadThumbnailsAsync(IEnumerable<DeckRow> decks)
        {
            foreach (var row in decks)
            {
                if (string.IsNullOrWhiteSpace(row.LeaderImageUrl))
                    continue;

                if (_imageCache.TryGetValue(row.LeaderImageUrl, out var cached))
                {
                    row.LeaderImage = cached;
                    continue;
                }

                var image = await ImageUtils.TryLoadImageAsync(row.LeaderImageUrl);
                if (image != null)
                {
                    _imageCache[row.LeaderImageUrl] = image;
                    row.LeaderImage = image;
                }
            }

            _dgvDecks.Invalidate();
        }

        private async Task LoadSelectedDeckAsync()
        {
            if (_dgvDecks.CurrentRow == null || !(_dgvDecks.CurrentRow.DataBoundItem is DeckRow row))
                return;

            try
            {
                var dto = await _decksService.GetDeckForEditAsync(row.Id);
                var ids = dto.DeckCards.Select(x => x.cardStockId).ToList();
                if (dto.LeaderCardId.HasValue)
                    ids.Add(dto.LeaderCardId.Value);

                var stock = await _cardStockService.GetByIdsAsync(ids.Distinct().ToList());
                var stockById = stock.ToDictionary(x => x.Id, x => x);

                _lblDeckName.Text = row.DeckName ?? "Deck sin nombre";
                _lblDeckMeta.Text = string.Format("Usuario {0} · Líder {1} · {2}/50 cartas · Actualizado {3}",
                    (_txtUserCode.Text ?? string.Empty).Trim(),
                    row.LeaderName ?? "Sin líder",
                    row.TotalCards,
                    row.LastUpdated.ToString("dd/MM/yyyy HH:mm"));

                _picLeader.Image = null;
                if (dto.LeaderCardId.HasValue && stockById.TryGetValue(dto.LeaderCardId.Value, out var leader))
                    _picLeader.Image = await LoadImageCachedAsync(leader.cardImage);

                _flowCards.SuspendLayout();
                _flowCards.Controls.Clear();

                foreach (var item in dto.DeckCards.OrderByDescending(x => x.quantity).ThenBy(x => stockById.ContainsKey(x.cardStockId) ? stockById[x.cardStockId].cardName : string.Empty))
                {
                    if (!stockById.TryGetValue(item.cardStockId, out var card))
                        continue;

                    _flowCards.Controls.Add(await CreateCardPanelAsync(card, item.quantity));
                }

                _flowCards.ResumeLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el detalle del deck:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<Control> CreateCardPanelAsync(CardStock card, int quantity)
        {
            var panel = new Panel
            {
                Width = 300,
                Height = 104,
                Margin = new Padding(0, 0, 10, 10),
                Padding = new Padding(10),
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var pic = new PictureBox
            {
                Location = new Point(10, 10),
                Size = new Size(56, 80),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = await LoadImageCachedAsync(card.cardImage)
            };

            var lblName = new Label
            {
                Location = new Point(76, 10),
                Size = new Size(160, 20),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Text = card.cardName
            };

            var lblMeta = new Label
            {
                Location = new Point(76, 34),
                Size = new Size(170, 38),
                Font = new Font("Segoe UI", 8.5F),
                ForeColor = ModernUi.TextMuted,
                Text = string.Format("{0} · {1}\nCoste {2} · Counter {3}", card.cardId, card.color, card.cost, card.counter)
            };

            var lblQty = new Label
            {
                Location = new Point(246, 10),
                Size = new Size(38, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = ModernUi.Navy,
                Text = quantity.ToString()
            };

            panel.Controls.Add(pic);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblMeta);
            panel.Controls.Add(lblQty);
            return panel;
        }

        private async Task<Image> LoadImageCachedAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            if (_imageCache.TryGetValue(url, out var image))
                return image;

            image = await ImageUtils.TryLoadImageAsync(url);
            if (image != null)
                _imageCache[url] = image;

            return image;
        }
    }
}
