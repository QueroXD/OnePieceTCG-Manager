using OnePieceTCG_Manager.Gestion.Views;
using System;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion
{
    public partial class FrmVerStock : Form
    {
        public FrmVerStock()
        {
            InitializeComponent();
        }

        private void FrmVerStock_Load(object sender, EventArgs e)
        {
            ShowListView();
        }

        private void toolListView_Click(object sender, EventArgs e)
        {
            lblMode.Text = "Modo: Lista";
            ShowListView();
        }

        private void toolGalleryView_Click(object sender, EventArgs e)
        {
            lblMode.Text = "Modo: Galería";
            ShowGalleryView();
        }

        private void ShowListView()
        {
            panelContainer.Controls.Clear();
            var listView = new UC_StockListView();
            listView.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(listView);
        }

        private void ShowGalleryView()
        {
            panelContainer.Controls.Clear();
            var galleryView = new UC_StockGalleryView();
            galleryView.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(galleryView);
        }
    }
}
