using OnePieceTCG_Manager.Decks;
using OnePieceTCG_Manager.Gestion;
using OnePieceTCG_Manager.Services;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace OnePieceTCG_Manager
{
    public partial class FrmMain : Form
    {
        private readonly string _codUsu;
        private readonly string _userName;

        public FrmMain(string codUsu, string userName)
        {
            InitializeComponent();
            _codUsu = codUsu;
            _userName = userName;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.Text = $"OPTCG Manager - {_userName}";
            economiaToolStripMenuItem.Visible = false; // ocultar por ahora

            InitDiscordRPC();
        }

        // -----------------------------------------
        // Abrir FrmAddStock usando API
        // -----------------------------------------
        private void añadirStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmAddStock>().FirstOrDefault();
            if (frm == null)
            {
                frm = new FrmAddStock()
                {
                    MdiParent = this
                };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }
            UpdateRPC("Añadiendo stock");
        }

        // -----------------------------------------
        // Abrir FrmVerStock usando API
        // -----------------------------------------
        private void verStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmVerStock>().FirstOrDefault();
            if (frm == null)
            {
                frm = new FrmVerStock()
                {
                    MdiParent = this
                };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }
            UpdateRPC("Viendo el inventario");
        }

        // -----------------------------------------
        // Abrir FrmMyDecks usando API
        // -----------------------------------------
        private void decksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = Application.OpenForms.OfType<FrmMyDecks>().FirstOrDefault();
            if (frm == null)
            {
                frm = new FrmMyDecks(_codUsu)
                {
                    MdiParent = this
                };
                frm.Show();
            }
            else
            {
                frm.BringToFront();
            }
            UpdateRPC("Mis Decks");
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void carpetaDeCartasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("explorer.exe", @"\\192.168.1.50\OnePieceTCG");
        }

        // 🔵 Rich Presence (igual)
        private DiscordRPC.DiscordRpcClient rpc;
        private void InitDiscordRPC()
        {
            try
            {
                rpc = new DiscordRPC.DiscordRpcClient("1439634178235826257");
                rpc.Initialize();
                rpc.SetPresence(new DiscordRPC.RichPresence()
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

        private void UpdateRPC(string details, string state = "OPTCG Manager")
        {
            if (rpc != null && rpc.IsInitialized)
                rpc.UpdateDetails(details);
        }
    }
}
