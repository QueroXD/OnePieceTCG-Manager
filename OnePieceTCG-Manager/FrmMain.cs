using OnePieceTCG_Manager.Decks;
using OnePieceTCG_Manager.Gestion;
using OnePieceTCG_Manager.Models;
using OnePieceTCG_Manager.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager
{
    public partial class FrmMain : Form
    {
        private readonly string _codUsu;
        private readonly string _userName;
        private readonly AppUpdateService _appUpdateService = new AppUpdateService();
        private DiscordRPC.DiscordRpcClient rpc;
        private bool _updateCheckStarted;

        public FrmMain(string codUsu, string userName)
        {
            InitializeComponent();
            _codUsu = codUsu;
            _userName = userName;
        }

        private async void FrmMain_Load(object sender, EventArgs e)
        {
            Text = string.Format("OPTCG Manager v{0} - {1}", GetCurrentVersion(), _userName);
            economiaToolStripMenuItem.Visible = false;
            InitDiscordRPC();

            if (!_updateCheckStarted)
            {
                _updateCheckStarted = true;
                await CheckForUpdatesAsync();
            }
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

        private async Task CheckForUpdatesAsync()
        {
            UpdateCheckResult result = await _appUpdateService.CheckForUpdatesAsync();
            if (!result.IsUpdateAvailable || result.Manifest == null)
                return;

            string message = string.Format(
                "Hay una nueva versión disponible.\n\nActual: {0}\nÚltima en main: {1}\n\n¿Quieres descargarla e instalarla ahora?",
                result.CurrentVersion,
                result.LatestVersion);

            DialogResult confirmation = MessageBox.Show(
                this,
                message,
                "Actualización disponible",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (confirmation != DialogResult.Yes)
                return;

            try
            {
                UseWaitCursor = true;
                Enabled = false;
                DownloadedUpdatePackage package = await _appUpdateService.DownloadUpdateAsync(result.Manifest);

                MessageBox.Show(
                    this,
                    "La aplicación se va a cerrar para instalar la actualización.\n\nAhora se abrirá una ventana del actualizador con el progreso.\nSi falla, puedes revisar estos logs:\n\n" +
                    _appUpdateService.GetLauncherLogPath() + "\n" +
                    _appUpdateService.GetUpdaterLogPath(),
                    "Instalando actualización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                _appUpdateService.LaunchUpdaterAndExit(package);
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    this,
                    "No se pudo completar la actualización automática.\n\n" + ex.Message,
                    "Error de actualización",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                Enabled = true;
                UseWaitCursor = false;
            }
        }

        private static Version GetCurrentVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version ?? new Version(1, 0, 0);
        }
    }
}
