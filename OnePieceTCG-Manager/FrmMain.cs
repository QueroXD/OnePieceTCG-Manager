using OnePieceTCG_Manager.Gestion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            economiaToolStripMenuItem.Visible = false; // Ocultar el menú de economía por ahora
        }

        private void añadirStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmAddStock>().FirstOrDefault();

            if (frm == null)
            {
                frm = new FrmAddStock { MdiParent = this };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }
        }

        private void testConexiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Probar la conexión a la base de datos
            using (var context = new Data.OnePieceContext())
            {
                try
                {
                    context.Database.Connection.Open();
                    MessageBox.Show("Conexión exitosa a la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar a la base de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void verStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmVerStock>().FirstOrDefault();

            if (frm == null)
            {
                frm = new FrmVerStock { MdiParent = this };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
