using Newtonsoft.Json;
using OnePieceTCG_Manager.Data;
using OnePieceTCG_Manager.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        public string STQuery = "allSTCards/";

        private bool _modoModificacion = false;
        private string _currentCardId = null;

        public FrmAddStock()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Constructor para modo modificación o solo unidades.
        /// </summary>
        public FrmAddStock(string cardId, bool modoSoloUnidades = false)
        {
            InitializeComponent();
            _modoModificacion = true;
            _currentCardId = cardId;

            // Cargar datos desde la base local
            LoadCardFromDB(cardId);

            // Ajustar visibilidad de botones
            addButton.Visible = false;
            confirmButton.Visible = true;

            if (modoSoloUnidades)
            {
                // Desactivar todos los controles excepto unidades y Confirmar
                foreach (Control c in Controls)
                    if (c.Name != "inputCantidad" && c.Name != "confirmButton")
                        c.Enabled = false;
            }
        }

        private void FrmAddStock_Load(object sender, EventArgs e)
        {
            if (!_modoModificacion)
            {
                // En modo añadir: ocultar confirmButton
                confirmButton.Visible = false;
            }

            // Ocultar botones innecesarios
            delButton.Visible = false;
            infoButton.Visible = false;
        }

        // 🔹 Carga los datos de la carta desde la base de datos local
        private void LoadCardFromDB(string cardId)
        {
            try
            {
                using (var db = new OnePieceContext())
                {
                    var card = db.CardStock.Find(cardId);
                    if (card == null)
                    {
                        MessageBox.Show("No se encontró la carta en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Close();
                        return;
                    }

                    // Cargar datos en los inputs
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

                    // Cargar imagen si existe ruta o URL
                    if (!string.IsNullOrEmpty(card.cardId))
                    {
                        try
                        {
                            fotoCard.ImageLocation = card.cardImage;
                        }
                        catch
                        {
                            fotoCard.Image = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al cargar la carta desde la base de datos:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 Botón Buscar (solo modo añadir)
        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputCardID.Text))
            {
                MessageBox.Show("Por favor, ingrese un ID de carta válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string cardId = inputCardID.Text.Trim();
            await LoadCardDataFromApiAsync(cardId);
        }

        // 🔹 Consulta la API solo cuando añadimos cartas nuevas
        private async Task LoadCardDataFromApiAsync(string cardId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string url = $"{BaseURL}{CardQuery}{cardId}/";
                    HttpResponseMessage response = await client.GetAsync(url);

                    List<Card> cards = null;

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        cards = JsonConvert.DeserializeObject<List<Card>>(json);
                    }
                    else
                    {
                        string stUrl = $"{BaseURL}{STQuery}";
                        var stResponse = await client.GetAsync(stUrl);
                        stResponse.EnsureSuccessStatusCode();

                        string stJson = await stResponse.Content.ReadAsStringAsync();
                        var allSTCards = JsonConvert.DeserializeObject<List<Card>>(stJson);

                        cards = allSTCards
                            .Where(c => c.card_image_id.Equals(cardId, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }

                    if (cards == null || cards.Count == 0)
                    {
                        MessageBox.Show("No se encontró ninguna carta con ese ID.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    Card selectedCard = isAlter.Checked && cards.Count > 1 ? cards[1] : cards[0];

                    inputCardName.Text = selectedCard.card_name;
                    inputSet.Text = selectedCard.set_name;
                    inputRarity.Text = selectedCard.rarity;
                    inputColor.Text = selectedCard.card_color;
                    inputCardType.Text = selectedCard.card_type;
                    inputCardSubType.Text = selectedCard.sub_types;
                    inputAtributo.Text = selectedCard.attribute;
                    inputPower.Text = selectedCard.card_power;
                    inputCost.Text = selectedCard.card_cost;
                    inputLifes.Text = selectedCard.life ?? "-";
                    inputCounter.Text = selectedCard.counter_amount?.ToString() ?? "";
                    inputDescription.Text = selectedCard.card_text;

                    if (!string.IsNullOrEmpty(selectedCard.card_image))
                    {
                        fotoCard.ImageLocation = selectedCard.card_image;
                    }
                    else
                    {
                        fotoCard.Image = null;
                    }

                    using (var db = new OnePieceContext())
                    {
                        var existingCard = db.Set<CardStock>().Find(cardId);
                        if (existingCard != null)
                        {
                            inputCantidad.Value = existingCard.units;
                        }
                        else
                        {
                            inputCantidad.Value = 1;
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al consultar la API:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // 🔹 Añadir carta (modo alta)
        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(inputCardID.Text) || string.IsNullOrWhiteSpace(inputCardName.Text))
                {
                    MessageBox.Show("Primero busca una carta antes de añadirla al stock.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var db = new OnePieceContext())
                {
                    string cardId = inputCardID.Text.Trim();
                    var existingCard = db.Set<CardStock>().Find(cardId);

                    if (existingCard != null)
                    {
                        existingCard.units += existingCard.units - (int)inputCantidad.Value;
                        db.SaveChanges();
                        MessageBox.Show("✅ Se actualizó el número de unidades de la carta en el stock.", "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        var newCard = new CardStock
                        {
                            cardId = inputCardID.Text.Trim(),
                            cardName = inputCardName.Text,
                            rarity = inputRarity.Text,
                            type = inputCardType.Text,
                            subType = inputCardSubType.Text,
                            attribute = inputAtributo.Text,
                            color = inputColor.Text,
                            cost = int.TryParse(inputCost.Text, out int c) ? c : 0,
                            counter = int.TryParse(inputCounter.Text, out int cnt) ? cnt : 0,
                            power = int.TryParse(inputPower.Text, out int p) ? p : 0,
                            setDesc = inputSet.Text,
                            isAlter = isAlter.Checked,
                            description = inputDescription.Text,
                            units = (int)inputCantidad.Value,
                            cardImage = fotoCard.ImageLocation
                        };

                        db.Set<CardStock>().Add(newCard);
                        db.SaveChanges();

                        MessageBox.Show("✅ Carta añadida correctamente al stock.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al añadir la carta:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 🔹 Confirmar cambios (modo modificar)
        private void confirmButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new OnePieceContext())
                {
                    var card = db.CardStock.Find(_currentCardId);
                    if (card == null)
                    {
                        MessageBox.Show("No se encontró la carta a modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // Actualizar los datos
                    card.units = (int)inputCantidad.Value;
                    card.description = inputDescription.Text;
                    db.SaveChanges();

                    MessageBox.Show("✅ Carta actualizada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Error al actualizar la carta:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
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
            inputPower.Clear();
            inputSet.Clear();
            inputDescription.Clear();
            isAlter.Checked = false;
            inputCantidad.Value = 1;
            fotoCard.Image = null;
        }
    }

    public class Card
    {
        public double inventory_price { get; set; }
        public double market_price { get; set; }
        public string card_name { get; set; }
        public string set_name { get; set; }
        public string card_text { get; set; }
        public string set_id { get; set; }
        public string rarity { get; set; }
        public string card_set_id { get; set; }
        public string card_color { get; set; }
        public string card_type { get; set; }
        public string life { get; set; }
        public string card_cost { get; set; }
        public string card_power { get; set; }
        public string sub_types { get; set; }
        public int? counter_amount { get; set; }
        public string attribute { get; set; }
        public string card_image_id { get; set; }
        public string card_image { get; set; }
    }
}
