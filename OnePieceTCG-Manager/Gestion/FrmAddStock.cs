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
    public partial class FrmAddStock : Form
    {
        private readonly OnePieceApiCardService _cardService = new OnePieceApiCardService();
        private readonly CardStockService _stockService = new CardStockService();

        private bool _modoModificacion;
        private bool _modoSoloUnidades;
        private string _cardId;
        private bool _isAlter;
        private string _cardImage;

        private Button _btnPrimaryAction;
        private Button _btnSecondaryAction;
        private Panel _canvas;

        public FrmAddStock()
        {
            InitializeComponent();
            InitializeModernLayout();
            lblStatus.Visible = false;
            FormClosed += FrmAddStock_FormClosed;
        }

        public FrmAddStock(string cardId, bool isAlter, string cardImage, bool modoSoloUnidades = false)
            : this()
        {
            _modoModificacion = true;
            _modoSoloUnidades = modoSoloUnidades;
            _cardId = cardId;
            _isAlter = isAlter;
            _cardImage = cardImage;

            if (modoSoloUnidades)
                Text = "Detalles de la carta";
        }

        private void InitializeModernLayout()
        {
            ModernUi.ApplyFormTheme(this);
            BackColor = ModernUi.AppBack;
            WindowState = FormWindowState.Maximized;
            AutoScroll = true;

            addButton.Visible = false;
            confirmButton.Visible = false;
            delButton.Visible = false;
            infoButton.Visible = false;

            foreach (Control control in Controls.Cast<Control>().ToList())
                control.Parent = null;

            var viewport = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                BackColor = ModernUi.AppBack,
                Padding = new Padding(24)
            };

            _canvas = new Panel
            {
                Width = 1160,
                Height = 760,
                BackColor = ModernUi.AppBack
            };

            var shell = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = ModernUi.AppBack
            };
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));

            var leftCard = BuildLeftCard();
            var rightCard = BuildRightCard();
            shell.Controls.Add(leftCard, 0, 0);
            shell.Controls.Add(rightCard, 1, 0);
            _canvas.Controls.Add(shell);
            viewport.Controls.Add(_canvas);
            Controls.Add(viewport);

            viewport.Resize += (s, e) => CenterCanvas(viewport);
            Load += (s, e) => CenterCanvas(viewport);
        }

        private Panel BuildLeftCard()
        {
            var host = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(0, 0, 12, 0),
                BackColor = ModernUi.AppBack
            };

            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 5,
                BackColor = ModernUi.Surface
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 320F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            var header = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Anadir stock",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = ModernUi.TextPrimary,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var subtitle = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Busca una carta por ID, revisa sus datos y actualiza el inventario con un flujo unificado.",
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = ModernUi.TextMuted,
                TextAlign = ContentAlignment.MiddleLeft
            };

            var searchRow = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 5,
                RowCount = 1,
                BackColor = ModernUi.Surface
            };
            searchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
            searchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            searchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            searchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            searchRow.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10F));

            lblCardID.Text = "Card ID";
            lblCardID.TextAlign = ContentAlignment.MiddleLeft;
            lblCardID.Dock = DockStyle.Fill;
            inputCardID.Dock = DockStyle.Fill;
            inputCardID.Margin = new Padding(0, 0, 12, 0);
            btnSearch.Dock = DockStyle.Fill;
            btnSearch.Text = "Buscar";
            isAlter.Dock = DockStyle.Fill;
            isAlter.Text = "Version alter";
            isAlter.TextAlign = ContentAlignment.MiddleCenter;

            searchRow.Controls.Add(lblCardID, 0, 0);
            searchRow.Controls.Add(inputCardID, 1, 0);
            searchRow.Controls.Add(btnSearch, 2, 0);
            searchRow.Controls.Add(isAlter, 3, 0);

            var detailsGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 6,
                BackColor = ModernUi.Surface
            };
            detailsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            detailsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            detailsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            detailsGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            for (int i = 0; i < 6; i++)
                detailsGrid.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));

            ConfigureField(lblCardName, inputCardName, "Nombre");
            ConfigureField(lblRarity, inputRarity, "Rareza");
            ConfigureField(lblCardType, inputCardType, "Tipo");
            ConfigureField(lblCost, inputCost, "Coste");
            ConfigureField(lblCardSubType, inputCardSubType, "Subtipo");
            ConfigureField(lblLifes, inputLifes, "Vida");
            ConfigureField(lblColor, inputColor, "Color");
            ConfigureField(lblPower, inputPower, "Power");
            ConfigureField(lblSet, inputSet, "Set");
            ConfigureField(label1, inputCounter, "Counter");
            ConfigureField(lblAtributo, inputAtributo, "Atributo");

            detailsGrid.Controls.Add(lblCardName, 0, 0);
            detailsGrid.Controls.Add(inputCardName, 1, 0);
            detailsGrid.Controls.Add(lblRarity, 2, 0);
            detailsGrid.Controls.Add(inputRarity, 3, 0);
            detailsGrid.Controls.Add(lblCardType, 0, 1);
            detailsGrid.Controls.Add(inputCardType, 1, 1);
            detailsGrid.Controls.Add(lblCost, 2, 1);
            detailsGrid.Controls.Add(inputCost, 3, 1);
            detailsGrid.Controls.Add(lblCardSubType, 0, 2);
            detailsGrid.Controls.Add(inputCardSubType, 1, 2);
            detailsGrid.SetColumnSpan(inputCardSubType, 3);
            detailsGrid.Controls.Add(lblColor, 0, 3);
            detailsGrid.Controls.Add(inputColor, 1, 3);
            detailsGrid.Controls.Add(lblPower, 2, 3);
            detailsGrid.Controls.Add(inputPower, 3, 3);
            detailsGrid.Controls.Add(lblSet, 0, 4);
            detailsGrid.Controls.Add(inputSet, 1, 4);
            detailsGrid.Controls.Add(label1, 2, 4);
            detailsGrid.Controls.Add(inputCounter, 3, 4);
            detailsGrid.Controls.Add(lblAtributo, 0, 5);
            detailsGrid.Controls.Add(inputAtributo, 1, 5);
            detailsGrid.Controls.Add(lblLifes, 2, 5);
            detailsGrid.Controls.Add(inputLifes, 3, 5);

            var descriptionPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = ModernUi.Surface
            };

            lblDescription.Text = "Descripcion";
            lblDescription.Dock = DockStyle.Top;
            lblDescription.Height = 28;
            lblDescription.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblDescription.ForeColor = ModernUi.TextPrimary;
            inputDescription.Dock = DockStyle.Fill;
            inputDescription.BorderStyle = BorderStyle.FixedSingle;
            inputDescription.Font = new Font("Segoe UI", 9.5F);

            descriptionPanel.Controls.Add(inputDescription);
            descriptionPanel.Controls.Add(lblDescription);

            root.Controls.Add(header, 0, 0);
            root.Controls.Add(subtitle, 0, 1);
            root.Controls.Add(searchRow, 0, 2);
            root.Controls.Add(detailsGrid, 0, 3);
            root.Controls.Add(descriptionPanel, 0, 4);

            card.Controls.Add(root);
            host.Controls.Add(card);
            return host;
        }

        private Panel BuildRightCard()
        {
            var host = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12, 0, 0, 0),
                BackColor = ModernUi.AppBack
            };

            var card = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = ModernUi.Surface,
                BorderStyle = BorderStyle.FixedSingle
            };

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 6,
                BackColor = ModernUi.Surface
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 380F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 56F));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            lblStatus.Dock = DockStyle.Fill;
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblStatus.ForeColor = ModernUi.Success;

            fotoCard.Dock = DockStyle.Fill;
            fotoCard.BackColor = ModernUi.SurfaceAlt;
            fotoCard.SizeMode = PictureBoxSizeMode.Zoom;

            lblCantidad.Text = "Unidades en stock";
            lblCantidad.Dock = DockStyle.Fill;
            lblCantidad.TextAlign = ContentAlignment.MiddleLeft;
            lblCantidad.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblCantidad.ForeColor = ModernUi.TextPrimary;

            var qtyPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = ModernUi.Surface
            };
            qtyPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            qtyPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 0F));
            inputCantidad.Dock = DockStyle.Left;
            inputCantidad.Width = 140;
            inputCantidad.Maximum = 9999;
            qtyPanel.Controls.Add(inputCantidad, 0, 0);

            _btnPrimaryAction = new Button
            {
                Dock = DockStyle.Fill,
                Height = 42,
                Text = _modoModificacion ? "Guardar cambios" : "Anadir al inventario"
            };
            ModernUi.StyleButton(_btnPrimaryAction, ModernUi.Accent, Color.White);
            _btnPrimaryAction.Click += (s, e) =>
            {
                if (_modoModificacion) confirmButton_Click(s, e);
                else addButton_Click(s, e);
            };

            _btnSecondaryAction = new Button
            {
                Dock = DockStyle.Fill,
                Height = 42,
                Text = _modoModificacion ? "Cerrar" : "Limpiar formulario"
            };
            ModernUi.StyleOutlineButton(_btnSecondaryAction);
            _btnSecondaryAction.Click += (s, e) =>
            {
                if (_modoModificacion) Close();
                else clearForm();
            };

            root.Controls.Add(lblStatus, 0, 0);
            root.Controls.Add(fotoCard, 0, 1);
            root.Controls.Add(lblCantidad, 0, 2);
            root.Controls.Add(qtyPanel, 0, 3);
            root.Controls.Add(_btnPrimaryAction, 0, 4);
            root.Controls.Add(_btnSecondaryAction, 0, 5);

            card.Controls.Add(root);
            host.Controls.Add(card);
            return host;
        }

        private void ConfigureField(Label label, Control input, string text)
        {
            label.Text = text;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            label.ForeColor = ModernUi.TextPrimary;

            input.Dock = DockStyle.Fill;
            input.Margin = new Padding(0, 6, 12, 6);
            if (!(input is RichTextBox))
                ModernUi.StyleInput(input);
        }

        private void CenterCanvas(Control viewport)
        {
            if (_canvas == null)
                return;

            int targetWidth = Math.Min(1160, Math.Max(920, viewport.ClientSize.Width - 48));
            _canvas.Width = targetWidth;
            _canvas.Left = Math.Max(0, (viewport.ClientSize.Width - _canvas.Width) / 2);
            _canvas.Top = 12;
        }

        private async void FrmAddStock_Load(object sender, EventArgs e)
        {
            _btnPrimaryAction.Text = _modoModificacion ? "Guardar cambios" : "Anadir al inventario";
            _btnSecondaryAction.Text = _modoModificacion ? "Cerrar" : "Limpiar formulario";

            if (_modoModificacion)
            {
                await LoadCardFromApiStockAsync();
                if (_modoSoloUnidades)
                    SetFieldEnabledState();
            }
        }

        private void SetFieldEnabledState()
        {
            var editable = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                inputCantidad.Name
            };

            foreach (var control in GetAllControls(this))
            {
                if (control == _btnPrimaryAction || control == _btnSecondaryAction || control == inputCantidad)
                    continue;

                if (control is TextBox || control is RichTextBox || control is CheckBox || control is Button)
                    control.Enabled = editable.Contains(control.Name);
            }
        }

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                yield return control;
                foreach (var child in GetAllControls(control))
                    yield return child;
            }
        }

        private void FrmAddStock_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private async Task LoadCardFromApiStockAsync()
        {
            var card = await _stockService.GetByKeyAsync(_cardId, _isAlter, _cardImage);

            if (card == null)
            {
                MessageBox.Show("No se encontro la carta en el stock.");
                Close();
                return;
            }

            SetStatusMessage(string.Format("Stock actual: {0} unidades", card.units), ModernUi.Warning);

            inputCardID.Text = card.cardId;
            inputCardName.Text = card.cardName;
            inputRarity.Text = card.rarity;
            inputCardType.Text = card.type;
            inputCardSubType.Text = card.subType;
            inputAtributo.Text = card.attribute;
            inputColor.Text = card.color;
            inputCost.Text = card.cost.ToString();
            inputCounter.Text = card.counter.ToString();
            inputLifes.Text = card.life;
            inputPower.Text = card.power.ToString();
            inputSet.Text = card.setDesc;
            inputDescription.Text = card.description;
            inputCantidad.Value = card.units;
            isAlter.Checked = card.isAlter;

            fotoCard.Load(card.cardImage);
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadCardDataFromApiAsync(inputCardID.Text.Trim());
        }

        private async Task LoadCardDataFromApiAsync(string cardId)
        {
            var card = await _cardService.GetCardAsync(cardId);
            if (card == null)
            {
                MessageBox.Show("No se encontro la carta en la API.");
                return;
            }

            inputCardName.Text = card.card_name;
            inputSet.Text = card.set_name;
            inputRarity.Text = card.rarity;
            inputColor.Text = card.card_color;
            inputCardType.Text = card.card_type;
            inputCardSubType.Text = card.sub_types;
            inputAtributo.Text = card.attribute;
            inputPower.Text = card.card_power;
            inputCost.Text = card.card_cost;
            inputLifes.Text = card.life;
            inputCounter.Text = card.counter_amount.HasValue ? card.counter_amount.Value.ToString() : string.Empty;
            inputDescription.Text = card.card_text;

            List<string> images = isAlter.Checked
                ? await _cardService.GetAlterImagesAsync(cardId)
                : await _cardService.GetCardImagesAsync(cardId);

            if (images.Count == 0)
            {
                MessageBox.Show("No hay imagenes disponibles.");
                return;
            }

            string selectedImage = images[0];
            if (isAlter.Checked && images.Count > 1)
            {
                using (var selector = new FrmImageSelector(images))
                {
                    if (selector.ShowDialog() == DialogResult.OK)
                        selectedImage = selector.SelectedImage;
                    else
                        return;
                }
            }

            fotoCard.ImageLocation = selectedImage;
            fotoCard.Load(selectedImage);

            await CheckIfCardExistsAsync(cardId, isAlter.Checked, selectedImage);
        }

        private async void addButton_Click(object sender, EventArgs e)
        {
            string cardId = inputCardID.Text.Trim();
            string imagePath = fotoCard.ImageLocation;

            var allCards = await _stockService.GetAllAsync();
            var existing = allCards.FirstOrDefault(c => c.cardId == cardId && c.isAlter == isAlter.Checked && c.cardImage == imagePath);

            if (existing != null)
            {
                if (existing.usedCards > inputCantidad.Value)
                {
                    MessageBox.Show(string.Format("No puedes reducir las unidades por debajo de las cartas en uso ({0}).", existing.usedCards), "Stock en uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                existing.units = (int)inputCantidad.Value;
                await _stockService.UpdateUnitsAsync(existing.Id, existing.units);
            }
            else
            {
                int cost = int.TryParse(inputCost.Text, out int c1) ? c1 : 0;
                int cnt = int.TryParse(inputCounter.Text, out int c2) ? c2 : 0;
                int pwr = int.TryParse(inputPower.Text, out int c3) ? c3 : 0;

                var stockCard = new CardStock
                {
                    cardId = cardId,
                    cardName = inputCardName.Text,
                    rarity = inputRarity.Text,
                    type = inputCardType.Text,
                    subType = inputCardSubType.Text,
                    attribute = inputAtributo.Text,
                    color = inputColor.Text,
                    cost = cost,
                    counter = cnt,
                    power = pwr,
                    setDesc = inputSet.Text,
                    description = inputDescription.Text,
                    units = (int)inputCantidad.Value,
                    cardImage = imagePath,
                    isAlter = isAlter.Checked,
                    usedCards = 0
                };

                await _stockService.CreateAsync(stockCard);
            }

            MessageBox.Show("Carta guardada correctamente");
            clearForm();
        }

        private async void confirmButton_Click(object sender, EventArgs e)
        {
            var allCards = await _stockService.GetAllAsync();
            var existing = allCards.FirstOrDefault(c => c.cardId == _cardId && c.isAlter == _isAlter && c.cardImage == _cardImage);

            if (existing == null)
                return;

            if (existing.usedCards > inputCantidad.Value)
            {
                MessageBox.Show(string.Format("No puedes reducir las unidades por debajo de las cartas en uso ({0}).", existing.usedCards), "Stock en uso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            existing.units = (int)inputCantidad.Value;
            existing.description = inputDescription.Text;

            await _stockService.UpdateAsync(existing.Id, existing);

            DialogResult = DialogResult.OK;
            Close();
        }

        private async Task CheckIfCardExistsAsync(string cardId, bool alter, string imagePath)
        {
            var existing = await _stockService.GetByKeyAsync(cardId, alter, imagePath);

            if (existing != null)
            {
                SetStatusMessage(string.Format("Ya en inventario: {0} unidades", existing.units), ModernUi.Warning);
                inputCantidad.Value = existing.units;
            }
            else
            {
                SetStatusMessage("Carta nueva para el inventario", ModernUi.Success);
                inputCantidad.Value = 0;
            }
        }

        private void SetStatusMessage(string text, Color color)
        {
            lblStatus.Visible = true;
            lblStatus.ForeColor = color;
            lblStatus.Text = text;
        }

        private void clearForm()
        {
            inputCardID.Clear();
            inputCardName.Clear();
            inputRarity.Clear();
            inputCardType.Clear();
            inputCardSubType.Clear();
            inputAtributo.Clear();
            inputColor.Clear();
            inputCost.Clear();
            inputCounter.Clear();
            inputLifes.Clear();
            inputPower.Clear();
            inputSet.Clear();
            inputDescription.Clear();
            inputCantidad.Value = 0;
            isAlter.Checked = false;
            fotoCard.Image = null;
            fotoCard.ImageLocation = null;
            lblStatus.Visible = false;
        }
    }
}


