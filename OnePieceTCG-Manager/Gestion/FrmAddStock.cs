using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion
{
    public partial class FrmAddStock : Form
    {
        public string BaseURL = "https://optcgapi.com/api/sets/card/";

        public FrmAddStock()
        {
            InitializeComponent();
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
                    string url = $"{BaseURL}{cardId}/";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();

                    string json = await response.Content.ReadAsStringAsync();

                    var cards = JsonConvert.DeserializeObject<List<Card>>(json);
                    if (cards == null || cards.Count == 0)
                    {
                        MessageBox.Show("No se encontró ninguna carta con ese ID.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    // Si el checkbox de alternativo está marcado, tomamos la segunda carta (si existe)
                    Card selectedCard = isAlter.Checked && cards.Count > 1 ? cards[1] : cards[0];

                    // Muestra los valores en los inputs
                    inputCardName.Text = selectedCard.card_name;
                    inputSet.Text = selectedCard.set_name;
                    inputRarity.Text = selectedCard.rarity;
                    inputColor.Text = selectedCard.card_color;
                    inputCardType.Text = selectedCard.card_type;
                    inputCardSubType.Text = selectedCard.sub_types;
                    inputPower.Text = selectedCard.card_power;
                    inputCost.Text = selectedCard.card_cost;
                    inputLifes.Text = selectedCard.life ?? "-";
                    //inputAttribute.Text = selectedCard.attribute;
                    inputCounter.Text = selectedCard.counter_amount?.ToString() ?? "";
                    inputDescription.Text = selectedCard.card_text;
                    //inputMarketPrice.Text = selectedCard.market_price.ToString("0.00");
                    //inputInventoryPrice.Text = selectedCard.inventory_price.ToString("0.00");
                    //inputSetId.Text = selectedCard.set_id;
                    //inputCardSetId.Text = selectedCard.card_set_id;

                    // Cargar la imagen en el PictureBox
                    using (var imgStream = await client.GetStreamAsync(selectedCard.card_image))
                    {
                        fotoCard.Image = Image.FromStream(imgStream);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al consultar la API:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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
