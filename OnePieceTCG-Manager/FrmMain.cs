using OnePieceTCG_Manager.Decks;
using OnePieceTCG_Manager.Gestion;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace OnePieceTCG_Manager
{
    public partial class FrmMain : Form
    {
        private readonly string _codUsu;
        private readonly string _userName;
        private DiscordRPC.DiscordRpcClient rpc;

        public FrmMain(string codUsu, string userName)
        {
            InitializeComponent();
            _codUsu = codUsu;
            _userName = userName;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            Text = $"OPTCG Manager - {_userName}";
            economiaToolStripMenuItem.Visible = false;
            InitDiscordRPC();
        }

        private void añadirStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmAddStock>().FirstOrDefault();
            if (frm == null)
            {
                frm = new FrmAddStock { MdiParent = this, WindowState = FormWindowState.Maximized };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }

            UpdateRPC("Añadiendo stock");
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

            UpdateRPC("Viendo el inventario");
        }

        private void estadisticasStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmKpiStats>().FirstOrDefault();
            if (frm == null)
            {
                frm = new FrmKpiStats { MdiParent = this };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }

            UpdateRPC("Estadisticas de stock");
        }
        private void misDecksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmMyDecks>().FirstOrDefault();
            if (frm == null)
            {
                frm = new FrmMyDecks(_codUsu) { MdiParent = this };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }

            UpdateRPC("Mis Decks");
        }

        private void explorarDecksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmDeckBrowser>().FirstOrDefault();
            if (frm == null)
            {
                frm = new FrmDeckBrowser(_codUsu) { MdiParent = this };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }

            UpdateRPC("Explorando decks");
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void carpetaDeCartasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", @"\\192.168.1.50\OnePieceTCG");
        }

        private void InitDiscordRPC()
        {
            try
            {
                rpc = new DiscordRPC.DiscordRpcClient("1439634178235826257");
                rpc.Initialize();
                rpc.SetPresence(new DiscordRPC.RichPresence
                {
                    Details = "En el menú principal",
                    State = "OPTCG Manager - By Quero",
                    Timestamps = DiscordRPC.Timestamps.Now
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error iniciando Discord RPC: " + ex.Message);
            }
        }

        private void UpdateRPC(string details)
        {
            if (rpc != null && rpc.IsInitialized)
                rpc.UpdateDetails(details);
        }
    }
}


