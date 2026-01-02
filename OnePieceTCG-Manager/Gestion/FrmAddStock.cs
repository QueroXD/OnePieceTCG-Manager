using Newtonsoft.Json;
using OnePieceTCG_Manager.Data;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion
{
    public partial class FrmAddStock : Form
    {
        public string BaseURL = "https://optcgapi.com/api/";
        public string CardQuery = "sets/card/";

        private OnePieceContext _db;

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
            if (_db != null)
                _db.Dispose();
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
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response =
                    await client.GetAsync(BaseURL + CardQuery + cardId + "/");

                if (!response.IsSuccessStatusCode)
                    return;

                var cards = JsonConvert.DeserializeObject<List<CardApi>>(
                    await response.Content.ReadAsStringAsync());

                if (cards == null || cards.Count == 0)
                    return;

                CardApi c = cards[0];

                inputCardName.Text = c.card_name;
                inputSet.Text = c.set_name;
                inputRarity.Text = c.rarity;
                inputColor.Text = c.card_color;
                inputCardType.Text = c.card_type;
                inputCardSubType.Text = c.sub_types;
                inputAtributo.Text = c.attribute;
                inputPower.Text = c.card_power;
                inputCost.Text = c.card_cost;
                inputCounter.Text = c.counter_amount.HasValue
                    ? c.counter_amount.Value.ToString()
                    : "";
                inputDescription.Text = c.card_text;

                var images = NasCardImageService.GetImagesByCardId(cardId);

                if (images.Count == 0)
                {
                    MessageBox.Show("No hay imágenes en el NAS");
                    return;
                }

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
                            // usuario canceló → no tocamos imagen
                            return;
                        }
                    }
                }

                fotoCard.ImageLocation = selectedImage;
                fotoCard.Load(selectedImage);
                CheckIfCardExists(cardId, isAlter.Checked, selectedImage);
            }
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
                existing.units = (int)inputCantidad.Value;
            }
            else
            {
                int cost;
                int cnt;
                int pwr;

                int.TryParse(inputCost.Text, out cost);
                int.TryParse(inputCounter.Text, out cnt);
                int.TryParse(inputPower.Text, out pwr);

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
                    cardImage = imagePath
                });
            }

            _db.SaveChanges();
            MessageBox.Show("Carta guardada correctamente");
            Close();
        }

        private void confirmButton_Click(object sender, EventArgs e)
        {
            var card = _db.CardStock.FirstOrDefault(c =>
                c.cardId == _cardId &&
                c.isAlter == _isAlter &&
                c.cardImage == _cardImage);

            if (card == null)
                return;

            card.units = (int)inputCantidad.Value;
            card.description = inputDescription.Text;

            _db.SaveChanges();
            Close();
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

    }
}
