using System;
using System.Net;
using System.Windows.Forms;

namespace OnePieceTCG_Manager
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FrmLogin frmLogin = new FrmLogin();

            if (frmLogin.ShowDialog() == DialogResult.OK)
            {
                Application.Run(
                    new FrmMain(
                        frmLogin.LoggedUserCodUsu,
                        frmLogin.LoggedUserName
                    )
                );
            }
            else
            {
                Application.Exit();
            }
        }
    }
}
