using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager.Utils
{
    public partial class FrmModal : Form
    {
        public string Enlace { get; set; }

        public FrmModal()
        {
            InitializeComponent();
        }

        private void FrmModal_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Enlace))
            {
                txtInput.Text = Enlace;
            }
        }

        private void btnAcceptar_Click(object sender, EventArgs e)
        {
            // Verifica si el texto ingresado no está vacío
            if (!string.IsNullOrWhiteSpace(txtInput.Text))
            {
                Enlace = txtInput.Text;  // Asigna el enlace a la propiedad
                this.DialogResult = DialogResult.OK;  // Indica que se aceptó la acción
                this.Close();  // Cierra el formulario
            }
            else
            {
                MessageBox.Show("Por favor, ingrese un enlace válido.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
