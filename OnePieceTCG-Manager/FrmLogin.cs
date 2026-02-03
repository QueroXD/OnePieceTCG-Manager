using OnePieceTCG_Manager.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnePieceTCG_Manager
{
    public partial class FrmLogin : Form
    {
        private readonly UsuariosService _usuariosService = new UsuariosService();

        public string LoggedUserCodUsu { get; private set; }
        public string LoggedUserName { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();
        }

        private async void FrmLogin_Load(object sender, EventArgs e)
        {
            await AutoLoginAsync();
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

            try
            {
                var usuario = await _usuariosService.LoginAsync(userName, password);

                if (usuario != null)
                {
                    LoggedUserCodUsu = usuario.CodUsu;
                    LoggedUserName = usuario.UserName;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("❌ Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error conectando con la API:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                    btnLogin.PerformClick();
                }
            }
            catch
            {
                // Ignoramos errores de autologin
            }
        }
    }
}
