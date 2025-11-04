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
        FrmAddStock frmAddStock = new FrmAddStock();

        public FrmMain()
        {
            InitializeComponent();
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

    }
}
