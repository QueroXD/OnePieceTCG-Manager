using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion.Views
{
    public partial class UC_StockGalleryView : UserControl
    {
        public event Action<CardStock> CardDoubleClicked;

        public UC_StockGalleryView()
        {
            InitializeComponent();
        }

        public async Task LoadDataAsync(List<CardStock> data)
        {
            flowPanel.Controls.Clear();

            foreach (var card in data)
            {
                var cardPanel = await CrearPanelAsync(card);
                flowPanel.Controls.Add(cardPanel);
            }
        }

        private async Task<Panel> CrearPanelAsync(CardStock card)
        {
            var panel = new Panel
            {
                Width = 180,
                Height = 250,
                BackColor = Color.White,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = card
            };

            var pic = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 180,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            await ImageUtils.CargarImagenAsync(pic, card.cardImage);

            var lblName = new Label
            {
                Text = card.cardName,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblUnits = new Label
            {
                Text = $"x{card.units}",
                Dock = DockStyle.Bottom,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.DarkGreen,
                Height = 25,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblUnits);
            panel.Controls.Add(lblName);
            panel.Controls.Add(pic);

            panel.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);
            pic.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);
            lblName.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);
            lblUnits.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);

            return panel;
        }
    }
}
