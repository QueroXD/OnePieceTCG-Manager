using OnePieceTCG_Manager.Services;
using OnePieceTCG_Manager.Utils;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager
{
    public partial class FrmLogin : Form
    {
        private readonly UsuariosService _usuariosService = new UsuariosService();
        private readonly Timer _bootTimer = new Timer();
        private int _bootTarget;
        private int _bootValue;
        private Func<Task> _bootCompletionAction;

        public string LoggedUserCodUsu { get; private set; }
        public string LoggedUserName { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();
            ApplyModernLayout();
            _bootTimer.Interval = 18;
            _bootTimer.Tick += BootTimer_Tick;
        }

        private void ApplyModernLayout()
        {
            ModernUi.ApplyFormTheme(this);
            BackColor = Color.FromArgb(242, 245, 248);
            pnlCard.BackColor = ModernUi.Surface;
            pnlHero.BackColor = ModernUi.Navy;
            lblTitle.ForeColor = ModernUi.TextPrimary;
            lblSubtitle.ForeColor = ModernUi.TextMuted;
            lblStatus.ForeColor = Color.FromArgb(218, 226, 240);
            lblUsername.ForeColor = ModernUi.TextPrimary;
            lblPasswd.ForeColor = ModernUi.TextPrimary;
            ModernUi.StyleInput(inputUsername);
            ModernUi.StyleInput(inputPasswd);
            ModernUi.StyleButton(btnLogin, ModernUi.Accent, Color.White);
            progressShell.BackColor = Color.FromArgb(57, 73, 102);
            progressFill.BackColor = ModernUi.Accent;
            pictureBox1.BackColor = Color.Transparent;
        }

        private async void FrmLogin_Load(object sender, EventArgs e)
        {
            await RunBootSequenceAsync("Inicializando cliente", 78, async () =>
            {
                await AutoLoginAsync();
            });
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = inputUsername.Text.Trim();
            string password = inputPasswd.Text.Trim();

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, introduce usuario y contraseña.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            await RunBootSequenceAsync("Cargando coleccion y servicios", 100, async () =>
            {
                try
                {
                    var usuario = await _usuariosService.LoginAsync(userName, password);

                    if (usuario != null)
                    {
                        LoggedUserCodUsu = usuario.CodUsu;
                        LoggedUserName = usuario.UserName;
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                    else
                    {
                        lblStatus.Text = "Credenciales incorrectas. Revisa los datos y prueba de nuevo.";
                        MessageBox.Show("Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    lblStatus.Text = "No se pudo conectar con el servicio de acceso.";
                    MessageBox.Show(string.Format("Error conectando con la API:\n{0}", ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnLogin.Enabled = true;
                }
            });
        }

        private async Task AutoLoginAsync()
        {
            string hostname = Environment.MachineName;
            try
            {
                var usuario = await _usuariosService.AutoLoginAsync(hostname);
                if (usuario != null)
                {
                    inputUsername.Text = usuario.UserName;
                    inputPasswd.Text = usuario.Passwd;
                    lblStatus.Text = "Equipo reconocido. Accediendo automaticamente...";
                    btnLogin.PerformClick();
                }
                else
                {
                    lblStatus.Text = "Introduce tus credenciales para continuar.";
                }
            }
            catch
            {
                lblStatus.Text = "Listo para iniciar sesion.";
            }
        }

        private async Task RunBootSequenceAsync(string status, int target, Func<Task> onCompleted)
        {
            lblStatus.Text = status;
            _bootCompletionAction = onCompleted;
            _bootTarget = target;
            if (_bootValue > _bootTarget)
                _bootValue = 0;

            if (!_bootTimer.Enabled)
                _bootTimer.Start();

            while (_bootTimer.Enabled)
                await Task.Delay(15);
        }

        private async void BootTimer_Tick(object sender, EventArgs e)
        {
            int step = _bootValue < 70 ? 4 : 2;
            _bootValue = Math.Min(_bootTarget, _bootValue + step);
            progressFill.Width = (int)(progressShell.Width * (_bootValue / 100.0));
            lblPercent.Text = string.Format("{0}%", _bootValue);

            if (_bootValue >= _bootTarget)
            {
                _bootTimer.Stop();
                var next = _bootCompletionAction;
                _bootCompletionAction = null;
                if (next != null)
                    await next();
            }
        }
    }
}
