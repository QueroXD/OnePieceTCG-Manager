using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion
{
    public partial class FrmImageSelector : Form
    {
        public string SelectedImage { get; private set; }

        public FrmImageSelector(List<string> images)
        {
            InitializeComponent();
            listImages.DataSource = images;
        }

        private void listImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listImages.SelectedItem != null)
            {
                picturePreview.Load(listImages.SelectedItem.ToString());
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (listImages.SelectedItem == null)
                return;

            SelectedImage = listImages.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }
    }
}
