using OnePieceTCG_Manager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Gestion
{
    public partial class FrmImageSelector : Form
    {
        public string SelectedImage { get; private set; }

        public FrmImageSelector(List<string> images)
        {
            InitializeComponent();
            ApplyModernLayout();
            listImages.DataSource = images;
            if (images != null && images.Count > 0)
                listImages.SelectedIndex = 0;
        }

        private void ApplyModernLayout()
        {
            ModernUi.ApplyFormTheme(this);
            BackColor = ModernUi.AppBack;
            listImages.BackColor = ModernUi.Surface;
            listImages.ForeColor = ModernUi.TextPrimary;
            listImages.BorderStyle = BorderStyle.None;
            picturePreview.BackColor = ModernUi.SurfaceAlt;
            ModernUi.StyleButton(btnAceptar, ModernUi.Accent, Color.White);
            ModernUi.StyleOutlineButton(btnCancelar);
            pnlList.BackColor = ModernUi.Surface;
            pnlPreview.BackColor = ModernUi.Surface;
            lblTitle.ForeColor = ModernUi.TextPrimary;
            lblHelp.ForeColor = ModernUi.TextMuted;
        }

        private void listImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listImages.SelectedItem != null)
                picturePreview.Load(listImages.SelectedItem.ToString());
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (listImages.SelectedItem == null)
                return;

            SelectedImage = listImages.SelectedItem.ToString();
            DialogResult = DialogResult.OK;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void listImages_Format(object sender, ListControlConvertEventArgs e)
        {
            var items = listImages.Items.Cast<object>().ToList();
            int index = items.IndexOf(e.ListItem);
            e.Value = string.Format("Imagen {0}", index + 1);
        }
    }
}
