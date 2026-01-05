using OnePieceTCG_Manager.Data;
using System;
using System.Linq;
using System.Windows.Forms;

namespace OnePieceTCG_Manager
{
    public partial class FrmLogin : Form
    {
        public string LoggedUserCodUsu { get; private set; }
        public string LoggedUserName { get; private set; }

        public FrmLogin()
        {
            InitializeComponent();
        }
        private void FrmLogin_Load(object sender, EventArgs e)
        {
            AutoLogin();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = inputUsername.Text.Trim();
            string password = inputPasswd.Text.Trim();

            // Validación básica
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Por favor, introduce usuario y contraseña.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var db = new OnePieceContext())
                {
                    // Buscar usuario en la base de datos
                    var user = db.Usuarios.FirstOrDefault(u => u.userName == userName && u.passwd == password);

                    if (user != null)
                    {
                        LoggedUserCodUsu = user.codUsu;
                        LoggedUserName = user.userName;
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("❌ Usuario o contraseña incorrectos", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void AutoLogin()
        {
            string currentHostname = System.Environment.MachineName;
            try
            {
                using (var db = new OnePieceContext())
                {
                    var user = db.Usuarios.FirstOrDefault(u => u.hostname == currentHostname);
                    if (user != null)
                    {
                        inputUsername.Text = user.userName;
                        inputPasswd.Text = user.passwd;
                        btnLogin.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
