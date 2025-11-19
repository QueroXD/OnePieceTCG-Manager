using Newtonsoft.Json;
using OnePieceTCG_Manager.Data;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace OnePieceTCG_Manager.Gestion
{
    public partial class FrmAddStock : Form
    {
        public string BaseURL = "https://optcgapi.com/api/";
        public string CardQuery = "sets/card/";
        public string STQuery = "allSTCards/";
        public string PromoQuery = "promos/card/";

        private bool _modoModificacion = false;
        private string _currentCardId = null;

        public FrmAddStock()
        {
            InitializeComponent();
            lblStatus.Visible = false;
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
            lblStatus.Visible = false;

            if (modoSoloUnidades)
            {
                // Desactivar todos los controles excepto unidades y Confirmar
                foreach (Control c in Controls)
                    if (c.Name != "inputCantidad" &&
                        c.Name != "confirmButton" &&
                        c.Name != "fotoCard")
                    {
                        c.Enabled = false;
                    }
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

                    // Cargar imagen si existe
                    if (!string.IsNullOrEmpty(card.cardImage))
                    {
                        try
                        {
                            string ext = Path.GetExtension(card.cardImage)?.ToLowerInvariant();
                            if (ext == ".webp")
                            {
                                _ = ImageUtils.CargarImagenAsync(fotoCard, card.cardImage);
                            }
                            else
                            {
                                fotoCard.LoadAsync(card.cardImage);
                            }
                        }
                        catch
                        {
                            fotoCard.Image = null;
                        }
                    }
                    else
                    {
                        fotoCard.Image = null;
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
                    List<Card> cards = null;

                    // ===================================================
                    // 1️⃣ PRIMERA QUERY → PROMOS
                    // ===================================================
                    string promoUrl = $"{BaseURL}{PromoQuery}{cardId}/";
                    HttpResponseMessage promoResponse = await client.GetAsync(promoUrl);

                    if (promoResponse.IsSuccessStatusCode)
                    {
                        string promoJson = await promoResponse.Content.ReadAsStringAsync();
                        cards = JsonConvert.DeserializeObject<List<Card>>(promoJson);

                        if (cards != null && cards.Count > 0)
                            goto PROCESS_RESULT;
                    }

                    // ===================================================
                    // 2️⃣ SEGUNDA QUERY → SETS/CARD
                    // ===================================================
                    string setsUrl = $"{BaseURL}{CardQuery}{cardId}/";
                    HttpResponseMessage setsResponse = await client.GetAsync(setsUrl);

                    if (setsResponse.IsSuccessStatusCode)
                    {
                        string setsJson = await setsResponse.Content.ReadAsStringAsync();
                        cards = JsonConvert.DeserializeObject<List<Card>>(setsJson);

                        if (cards != null && cards.Count > 0)
                            goto PROCESS_RESULT;
                    }

                    // ===================================================
                    // 3️⃣ TERCERA QUERY → ALL ST CARDS (filtrando)
                    // ===================================================
                    string stUrl = $"{BaseURL}{STQuery}";
                    var stResponse = await client.GetAsync(stUrl);
                    stResponse.EnsureSuccessStatusCode();

                    string stJson = await stResponse.Content.ReadAsStringAsync();
                    var allSTCards = JsonConvert.DeserializeObject<List<Card>>(stJson);

                    cards = allSTCards
                        .Where(c => c.card_image_id.Equals(cardId, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    if (cards == null || cards.Count == 0)
                    {
                        MessageBox.Show("No se encontró ninguna carta con ese ID.",
                                        "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                PROCESS_RESULT:

                    // ===================================================
                    // Elegir versión normal o alternativa
                    // ===================================================
                    Card selectedCard = isAlter.Checked && cards.Count > 1 ? cards[1] : cards[0];

                    // ===================================================
                    // Rellenar los inputs
                    // ===================================================
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

                    // ===================================================
                    // Carga imagen con fallback WebP → JPG/PNG y fallback final a OP sitio oficial
                    // ===================================================
                    string imageUrl = selectedCard.card_image;

                    // 1) Si viene null, fallback directo al sitio oficial
                    if (string.IsNullOrEmpty(imageUrl))
                    {
                        imageUrl = $"https://en.onepiece-cardgame.com/images/cardlist/card/{cardId}.png";
                    }

                    bool loaded = false;

                    // 2) Intentar WebP mediante ImageUtils si aplica
                    string ext = Path.GetExtension(imageUrl)?.ToLowerInvariant();
                    if (ext == ".webp")
                    {
                        try
                        {
                            await ImageUtils.CargarImagenAsync(fotoCard, imageUrl);
                            loaded = true;
                        }
                        catch { }
                    }

                    // 3) Si no se cargó, intentar carga normal (PNG/JPG)
                    if (!loaded)
                    {
                        try
                        {
                            fotoCard.Load(imageUrl);
                            loaded = true;
                        }
                        catch { }
                    }

                    // 4) Último fallback: sitio oficial, solo si aún no se cargó y no venía ya usando ese URL
                    if (!loaded && !imageUrl.Contains("onepiece-cardgame.com"))
                    {
                        string fallbackOfficial = $"https://en.onepiece-cardgame.com/images/cardlist/card/{cardId}.png";
                        try
                        {
                            fotoCard.Load(fallbackOfficial);
                            loaded = true;
                        }
                        catch
                        {
                            fotoCard.Image = null;
                        }
                    }



                    // ===================================================
                    // Mostrar unidades si ya existe en DB
                    // ===================================================
                    using (var db = new OnePieceContext())
                    {
                        var existingCard = db.Set<CardStock>().Find(cardId);
                        if (existingCard != null)
                        {
                            lblStatus.Text = "Ya en inventario: " + existingCard.units;
                            lblStatus.ForeColor = Color.Red;
                            lblStatus.Visible = true;

                            inputCantidad.Value = existingCard.units;
                        }
                        else
                        {
                            lblStatus.Text = "New";
                            lblStatus.ForeColor = Color.Green;
                            lblStatus.Visible = true;

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
                        existingCard.units = (int)inputCantidad.Value;
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

        private void fotoCard_DoubleClick(object sender, EventArgs e)
        {
            // Mostrar el formulario para introducir el enlace
            FrmModal enlaceForm = new FrmModal();

            // ⭐ NUEVO → Si ya hay imagen cargada, mostrar su link en el input
            if (!string.IsNullOrEmpty(fotoCard.ImageLocation))
            {
                enlaceForm.Enlace = fotoCard.ImageLocation;
            }

            if (enlaceForm.ShowDialog() == DialogResult.OK)
            {
                // Si el enlace es válido, cargamos la imagen en el PictureBox
                string enlace = enlaceForm.Enlace;
                ImageUtils.CargarImagen(fotoCard, enlace);
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
        public string card_image_id { get; set; }
        public string card_image { get; set; }
    }
}
