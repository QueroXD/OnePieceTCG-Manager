using DiscordRPC;
using OnePieceTCG_Manager.Gestion;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace OnePieceTCG_Manager
{
    public partial class FrmMain : Form
    {
        private DiscordRpcClient rpc;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            economiaToolStripMenuItem.Visible = false; // Ocultar el menú de economía por ahora

            InitDiscordRPC();  // 🔥 Inicializamos el Rich Presence
        }

        // -----------------------------------------
        // 🔵 Inicialización Rich Presence
        // -----------------------------------------
        private void InitDiscordRPC()
        {
            try
            {
                rpc = new DiscordRpcClient("1439634178235826257");

                rpc.Initialize();

                rpc.SetPresence(new RichPresence()
                {
                    Details = "En el menú principal",
                    State = "OPTCG Manager - By Quero",
                    //Assets = new Assets()
                    //{
                    //    LargeImageKey = "icon_large",
                    //    LargeImageText = "One Piece TCG",
                    //    SmallImageKey = "icon_small",
                    //    SmallImageText = "OPTCG Manager"
                    //},
                    Timestamps = Timestamps.Now
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error iniciando Discord RPC: " + ex.Message);
            }
        }

        // -----------------------------------------
        // 🔵 Actualizar Presence según lo que hace el usuario
        // -----------------------------------------
        private void UpdateRPC(string details, string state = "OPTCG Manager")
        {
            if (rpc != null && rpc.IsInitialized)
            {
                rpc.UpdateDetails(details);
            }
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

            UpdateRPC("Añadiendo stock");   // 🔥 Actualiza Rich Presence
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

            UpdateRPC("Viendo el inventario");  // 🔥
        }

        private void testConexiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var context = new Data.OnePieceContext())
            {
                try
                {
                    context.Database.Connection.Open();
                    MessageBox.Show("Conexión exitosa a la base de datos.",
                                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    UpdateRPC("Probando conexión a la base de datos");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar: {ex.Message}",
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    UpdateRPC("Error al probar conexión");
                }
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
