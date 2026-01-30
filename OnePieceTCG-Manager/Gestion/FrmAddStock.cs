using OnePieceTCG_Manager.Data;
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
        private readonly OnePieceContext _db;

        private bool _modoModificacion;
        private string _cardId;
        private bool _isAlter;
        private string _cardImage;

        public FrmAddStock()
        {
            InitializeComponent();
            lblStatus.Visible = false;

            _db = new OnePieceContext();
            this.FormClosed += FrmAddStock_FormClosed;
        }

        public FrmAddStock(string cardId, bool isAlter, string cardImage, bool modoSoloUnidades = false)
            : this()
        {
            _modoModificacion = true;
            _cardId = cardId;
            _isAlter = isAlter;
            _cardImage = cardImage;

            LoadCardFromDB();

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
            _db?.Dispose();
        }

        private void LoadCardFromDB()
        {
            var card = _db.CardStock.FirstOrDefault(c =>
                c.cardId == _cardId &&
                c.isAlter == _isAlter &&
                c.cardImage == _cardImage);

            if (card == null)
            {
                MessageBox.Show("No se encontró la carta.");
                Close();
                return;
            }

            lblStatus.Visible = true;
            lblStatus.BackColor = System.Drawing.Color.Yellow;
            lblStatus.Text = "Unidades existentes: " + card.units;

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
            // ===== Obtener carta completa desde el servicio =====
            var card = await _cardService.GetCardAsync(cardId);
            if (card == null)
            {
                MessageBox.Show("No se encontró la carta en la API.");
                return;
            }

            // ===== Llenar campos =====
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

            // ===== Obtener imágenes =====
            List<string> images;
            if (isAlter.Checked)
            {
                images = await _cardService.GetAlterImagesAsync(cardId);
            }
            else
            {
                images = await _cardService.GetCardImagesAsync(cardId);
            }

            if (images.Count == 0)
            {
                MessageBox.Show("No hay imágenes disponibles");
                return;
            }

            // ===== Selección de alter si hay varias =====
            string selectedImage = images[0];
            if (isAlter.Checked && images.Count > 1)
            {
                using (FrmImageSelector selector = new FrmImageSelector(images))
                {
                    if (selector.ShowDialog() == DialogResult.OK)
                    {
                        selectedImage = selector.SelectedImage;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            fotoCard.ImageLocation = selectedImage;
            fotoCard.Load(selectedImage);

            // ===== Inventario =====
            CheckIfCardExists(cardId, isAlter.Checked, selectedImage);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string cardId = inputCardID.Text.Trim();
            string imagePath = fotoCard.ImageLocation;

            var existing = _db.CardStock.FirstOrDefault(c =>
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
                existing.lastUpdatedCardDate = DateTime.Now;
            }
            else
            {
                int cost = int.TryParse(inputCost.Text, out int c1) ? c1 : 0;
                int cnt = int.TryParse(inputCounter.Text, out int c2) ? c2 : 0;
                int pwr = int.TryParse(inputPower.Text, out int c3) ? c3 : 0;

                _db.CardStock.Add(new CardStock
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
                    isAlter = isAlter.Checked,
                    description = inputDescription.Text,
                    units = (int)inputCantidad.Value,
                    cardImage = imagePath,
                    usedCards = 0,
                    insertedCardDate = DateTime.Now,
                    lastUpdatedCardDate = DateTime.Now
                });
            }

            _db.SaveChanges();
            MessageBox.Show("Carta guardada correctamente");
            clearFrom();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            var card = _db.CardStock.FirstOrDefault(c =>
                c.cardId == _cardId &&
                c.isAlter == _isAlter &&
                c.cardImage == _cardImage);

            if (card == null)
                return;

            if (card.usedCards > inputCantidad.Value)
            {
                MessageBox.Show(
                    $"No puedes reducir las unidades por debajo de las cartas en uso ({card.usedCards}).",
                    "Stock en uso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            card.units = (int)inputCantidad.Value;
            card.description = inputDescription.Text;
            card.lastUpdatedCardDate = DateTime.Now;

            _db.SaveChanges();
            clearFrom();
        }

        private void CheckIfCardExists(string cardId, bool isAlter, string imagePath)
        {
            var existing = _db.CardStock.FirstOrDefault(c =>
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

        private void clearFrom() 
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
