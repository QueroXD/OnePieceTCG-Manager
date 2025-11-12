using OnePieceTCG_Manager.Data;
using OnePieceTCG_Manager.Utils;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion.Views
{
    public partial class UC_StockGalleryView : UserControl
    {
        public UC_StockGalleryView()
        {
            InitializeComponent();
            _ = LoadGalleryAsync(); // no bloquea la UI
        }

        private async Task LoadGalleryAsync()
        {
            flowPanel.Controls.Clear();

            using (var db = new OnePieceContext())
            {
                var stockList = db.CardStock.ToList();

                foreach (var card in stockList)
                {
                    var panel = new Panel
                    {
                        Width = 180,
                        Height = 250,
                        Margin = new Padding(10),
                        BorderStyle = BorderStyle.FixedSingle,
                        BackColor = Color.White
                    };

                    PictureBox pic = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Dock = DockStyle.Top,
                        Height = 180
                    };

                    // 🔹 carga asíncrona (rápida para PNG/JPG, decodifica WebP solo si es necesario)
                    await ImageUtils.CargarImagenAsync(pic, card.cardImage);

                    Label lblName = new Label
                    {
                        Text = card.cardName,
                        Dock = DockStyle.Top,
                        Font = new Font("Segoe UI", 9, FontStyle.Bold),
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    Label lblUnits = new Label
                    {
                        Text = $"x{card.units}",
                        Dock = DockStyle.Bottom,
                        Font = new Font("Segoe UI", 9, FontStyle.Regular),
                        ForeColor = Color.DarkGreen,
                        TextAlign = ContentAlignment.MiddleCenter
                    };

                    panel.Controls.Add(lblUnits);
                    panel.Controls.Add(lblName);
                    panel.Controls.Add(pic);

                    // Click en cualquier parte de la card
                    panel.Click += (s, e) => OpenEditForm(card.cardId);
                    pic.Click += (s, e) => OpenEditForm(card.cardId);
                    lblName.Click += (s, e) => OpenEditForm(card.cardId);
                    lblUnits.Click += (s, e) => OpenEditForm(card.cardId);

                    flowPanel.Controls.Add(panel);
                }
            }
        }

        private void OpenEditForm(string cardId)
        {
            var frm = new FrmAddStock(cardId, modoSoloUnidades: true);
            frm.ShowDialog();
            _ = LoadGalleryAsync(); // refrescar sin bloquear
        }
    }
}
