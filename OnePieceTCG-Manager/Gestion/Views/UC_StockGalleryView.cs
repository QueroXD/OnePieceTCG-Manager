using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
            flowPanel.Resize += (s, e) => ReflowCards();
        }

        public async Task LoadDataAsync(List<CardStock> data)
        {
            flowPanel.SuspendLayout();
            flowPanel.Controls.Clear();

            foreach (var card in data)
            {
                var cardPanel = await CrearPanelAsync(card);
                flowPanel.Controls.Add(cardPanel);
            }

            flowPanel.ResumeLayout(true);
            ReflowCards();
        }

        private async Task<Panel> CrearPanelAsync(CardStock card)
        {
            var panel = new Panel
            {
                Width = 220,
                Height = 330,
                BackColor = ModernUi.Surface,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                Tag = card
            };

            var pic = new PictureBox
            {
                Dock = DockStyle.Top,
                Height = 220,
                BackColor = ModernUi.SurfaceAlt,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            await ImageUtils.CargarImagenAsync(pic, card.cardImage);

            var lblName = new Label
            {
                Text = card.cardName,
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                Height = 46,
                Padding = new Padding(8, 8, 8, 0),
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = ModernUi.TextPrimary
            };

            var lblMeta = new Label
            {
                Text = string.Format("{0} | {1}", card.cardId, string.IsNullOrWhiteSpace(card.color) ? "Sin color" : card.color),
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 8.5F),
                Height = 24,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = ModernUi.TextMuted
            };

            var lblUnits = new Label
            {
                Text = string.Format("Stock {0} | Libres {1}", card.units, Math.Max(0, card.units - card.usedCards)),
                Dock = DockStyle.Bottom,
                Font = new Font("Segoe UI", 8.8F, FontStyle.Bold),
                ForeColor = Math.Max(0, card.units - card.usedCards) > 0 ? ModernUi.Success : ModernUi.Danger,
                Height = 32,
                TextAlign = ContentAlignment.MiddleCenter
            };

            panel.Controls.Add(lblUnits);
            panel.Controls.Add(lblMeta);
            panel.Controls.Add(lblName);
            panel.Controls.Add(pic);

            panel.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);
            pic.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);
            lblName.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);
            lblMeta.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);
            lblUnits.DoubleClick += (s, e) => CardDoubleClicked?.Invoke(card);

            return panel;
        }

        private void ReflowCards()
        {
            if (flowPanel.Controls.Count == 0)
                return;

            int availableWidth = Math.Max(320, flowPanel.ClientSize.Width - flowPanel.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth);
            int baseCardWidth = 220;
            int columns = Math.Max(1, availableWidth / (baseCardWidth + 20));
            int targetWidth = Math.Max(190, (availableWidth / columns) - 20);

            foreach (var panel in flowPanel.Controls.OfType<Panel>())
                panel.Width = targetWidth;
        }
    }
}
