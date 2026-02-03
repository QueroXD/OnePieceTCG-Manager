using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Services;
using System;
using System.Collections.Generic;
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
        private string _cardId;
        private bool _isAlter;
        private string _cardImage;

        public FrmAddStock()
        {
            InitializeComponent();
            lblStatus.Visible = false;

            this.FormClosed += FrmAddStock_FormClosed;
        }

        public FrmAddStock(string cardId, bool isAlter, string cardImage, bool modoSoloUnidades = false)
            : this()
        {
            _modoModificacion = true;
            _cardId = cardId;
            _isAlter = isAlter;
            _cardImage = cardImage;

            LoadCardFromApiStockAsync().Wait();

            addButton.Visible = false;
            confirmButton.Visible = true;

            if (modoSoloUnidades)
            {
                FrmAddStock.ActiveForm.Text = "Detalles de la carta";
                foreach (Control c in Controls)
                {
                    if (c.Name != "inputCantidad" &&
                        c.Name != "confirmButton")
                        c.Enabled = false;
                }
            }
        }

        private void FrmAddStock_FormClosed(object sender, FormClosedEventArgs e)
        {
            // nada que cerrar ahora, todo es API
        }

        private async Task LoadCardFromApiStockAsync()
        {
            var allCards = await _stockService.GetAllAsync();
            var card = allCards.FirstOrDefault(c =>
                c.cardId == _cardId &&
                c.isAlter == _isAlter &&
                c.cardImage == _cardImage);

            if (card == null)
            {
                MessageBox.Show("No se encontró la carta en el stock.");
                Close();
                return;
            }

            lblStatus.Visible = true;
            lblStatus.BackColor = System.Drawing.Color.Yellow;
            lblStatus.Text = "Unidades existentes: " + card.units;

            // llenar campos
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
                MessageBox.Show("No se encontró la carta en la API.");
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
            inputCounter.Text = card.counter_amount.HasValue
                ? card.counter_amount.Value.ToString()
                : "";
            inputDescription.Text = card.card_text;

            List<string> images;
            if (isAlter.Checked)
                images = await _cardService.GetAlterImagesAsync(cardId);
            else
                images = await _cardService.GetCardImagesAsync(cardId);

            if (images.Count == 0)
            {
                MessageBox.Show("No hay imágenes disponibles");
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

            CheckIfCardExists(cardId, isAlter.Checked, selectedImage);
        }

        private async void addButton_Click(object sender, EventArgs e)
        {
            string cardId = inputCardID.Text.Trim();
            string imagePath = fotoCard.ImageLocation;

            var allCards = await _stockService.GetAllAsync();
            var existing = allCards.FirstOrDefault(c =>
                c.cardId == cardId &&
                c.isAlter == isAlter.Checked &&
                c.cardImage == imagePath);

            if (existing != null)
            {
                if (existing.usedCards > inputCantidad.Value)
                {
                    MessageBox.Show(
                        $"No puedes reducir las unidades por debajo de las cartas en uso ({existing.usedCards}).",
                        "Stock en uso",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
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

                var card = new CardStock
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

                await _stockService.CreateAsync(card);
            }

            MessageBox.Show("Carta guardada correctamente");
            clearForm();
        }

        private async void confirmButton_Click(object sender, EventArgs e)
        {
            var allCards = await _stockService.GetAllAsync();
            var existing = allCards.FirstOrDefault(c =>
                c.cardId == _cardId &&
                c.isAlter == _isAlter &&
                c.cardImage == _cardImage);

            if (existing == null) return;

            if (existing.usedCards > inputCantidad.Value)
            {
                MessageBox.Show(
                    $"No puedes reducir las unidades por debajo de las cartas en uso ({existing.usedCards}).",
                    "Stock en uso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            existing.units = (int)inputCantidad.Value;
            existing.description = inputDescription.Text;

            await _stockService.UpdateAsync(existing.Id, existing);

            clearForm();
        }

        private void CheckIfCardExists(string cardId, bool isAlter, string imagePath)
        {
            var existingTask = _stockService.GetAllAsync();
            existingTask.Wait();
            var existing = existingTask.Result.FirstOrDefault(c =>
                c.cardId == cardId &&
                c.isAlter == isAlter &&
                c.cardImage == imagePath);

            if (existing != null)
            {
                lblStatus.Visible = true;
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = $"Ya en el inventario: {existing.units}";
                inputCantidad.Value = existing.units;
            }
            else
            {
                lblStatus.Visible = true;
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Text = $"NEW";
                inputCantidad.Value = 0;
            }
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
            lblStatus.Visible = false;
        }
    }
}
