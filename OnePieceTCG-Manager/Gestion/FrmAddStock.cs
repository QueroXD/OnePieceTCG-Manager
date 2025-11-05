using Newtonsoft.Json;
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

        public FrmAddStock()
        {
            InitializeComponent();
        }

        private void FrmAddStock_Load(object sender, EventArgs e)
        {
            // Ocultar botones de Eliminar e Informacion
            delButton.Visible = false;
            infoButton.Visible = false;
            confirmButton.Visible = false;
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(inputCardID.Text))
            {
                MessageBox.Show("Por favor, ingrese un ID de carta válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string cardId = inputCardID.Text.Trim();
            await LoadCardDataAsync(cardId);
        }

        private async Task LoadCardDataAsync(string cardId)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    // 1️⃣ — Buscar primero en sets/card/{cardId}/
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
                        // 2️⃣ — Si no lo encuentra (por ejemplo, 404), buscar en allSTCards/
                        string stUrl = $"{BaseURL}{STQuery}";
                        var stResponse = await client.GetAsync(stUrl);
                        stResponse.EnsureSuccessStatusCode();

                        string stJson = await stResponse.Content.ReadAsStringAsync();
                        var allSTCards = JsonConvert.DeserializeObject<List<Card>>(stJson);

                        // Filtra por card_image_id (exact match)
                        cards = allSTCards
                            .Where(c => c.card_image_id.Equals(cardId, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }

                    // Si no se encontró nada en ninguno de los dos
                    if (cards == null || cards.Count == 0)
                    {
                        MessageBox.Show("No se encontró ninguna carta con ese ID.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // 3️⃣ — Selecciona la carta (normal o alternate)
                    Card selectedCard = isAlter.Checked && cards.Count > 1 ? cards[1] : cards[0];

                    // 4️⃣ — Mostrar valores en los inputs
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

                    // 5️⃣ — Cargar imagen
                    if (!string.IsNullOrEmpty(selectedCard.card_image))
                    {
                        using (var imgStream = await client.GetStreamAsync(selectedCard.card_image))
                        {
                            fotoCard.Image = Image.FromStream(imgStream);
                        }
                    }
                    else
                    {
                        fotoCard.Image = null;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al consultar la API:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar campos básicos
                if (string.IsNullOrWhiteSpace(inputCardID.Text) || string.IsNullOrWhiteSpace(inputCardName.Text))
                {
                    MessageBox.Show("Primero busca una carta antes de añadirla al stock.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var db = new Data.OnePieceContext())
                {
                    string cardId = inputCardID.Text.Trim();

                    // Buscar si la carta ya existe
                    var existingCard = db.Set<Models.CardStock>().Find(cardId);

                    if (existingCard != null)
                    {
                        // Si ya existe, incrementar unidades
                        existingCard.units += (int)inputCantidad.Value;
                        db.SaveChanges();
                        MessageBox.Show("✅ Se actualizó el número de unidades de la carta en el stock.", "Actualizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // Crear nueva instancia de CardStock
                        var newCard = new Models.CardStock
                        {
                            cardId = inputCardID.Text.Trim(),
                            cardName = inputCardName.Text,
                            rarity = inputRarity.Text,
                            type = inputCardType.Text,
                            subType = inputCardSubType.Text,
                            attribute = inputAtributo?.Text ?? "", // por si no existe el input
                            color = inputColor.Text,
                            cost = int.TryParse(inputCost.Text, out int c) ? c : 0,
                            counter = int.TryParse(inputCounter.Text, out int cnt) ? cnt : 0,
                            power = int.TryParse(inputPower.Text, out int p) ? p : 0,
                            setDesc = inputSet.Text,
                            isAlter = isAlter.Checked,
                            description = inputDescription.Text,
                            units = (int)inputCantidad.Value
                        };

                        // Guardar en base de datos
                        db.Set<Models.CardStock>().Add(newCard);
                        db.SaveChanges();

                        MessageBox.Show("✅ Carta añadida correctamente al stock.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

                // Opcional: limpiar los campos del formulario tras guardar
                ClearForm();
            }
            catch (Exception ex)
            {
                string inner = ex.InnerException?.Message ?? "(sin detalles)";
                MessageBox.Show($"❌ Error al añadir la carta al stock:\n{ex.Message}\n\nDetalles: {inner}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public string date_scraped { get; set; }
        public string card_image_id { get; set; }
        public string card_image { get; set; }
    }
}
