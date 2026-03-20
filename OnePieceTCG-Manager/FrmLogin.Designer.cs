namespace OnePieceTCG_Manager
{
    partial class FrmLogin
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlCard;
        private System.Windows.Forms.Panel pnlHero;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel progressShell;
        private System.Windows.Forms.Panel progressFill;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox inputUsername;
        private System.Windows.Forms.TextBox inputPasswd;
        private System.Windows.Forms.Label lblPasswd;
        private System.Windows.Forms.Button btnLogin;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.pnlCard = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.inputPasswd = new System.Windows.Forms.TextBox();
            this.lblPasswd = new System.Windows.Forms.Label();
            this.inputUsername = new System.Windows.Forms.TextBox();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlHero = new System.Windows.Forms.Panel();
            this.lblPercent = new System.Windows.Forms.Label();
            this.progressShell = new System.Windows.Forms.Panel();
            this.progressFill = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlCard.SuspendLayout();
            this.pnlHero.SuspendLayout();
            this.progressShell.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCard
            // 
            this.pnlCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCard.Controls.Add(this.btnLogin);
            this.pnlCard.Controls.Add(this.inputPasswd);
            this.pnlCard.Controls.Add(this.lblPasswd);
            this.pnlCard.Controls.Add(this.inputUsername);
            this.pnlCard.Controls.Add(this.lblUsername);
            this.pnlCard.Controls.Add(this.lblSubtitle);
            this.pnlCard.Controls.Add(this.lblTitle);
            this.pnlCard.Controls.Add(this.pnlHero);
            this.pnlCard.Location = new System.Drawing.Point(36, 34);
            this.pnlCard.Name = "pnlCard";
            this.pnlCard.Size = new System.Drawing.Size(708, 396);
            this.pnlCard.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnLogin.Location = new System.Drawing.Point(374, 311);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(274, 42);
            this.btnLogin.TabIndex = 7;
            this.btnLogin.Text = "Entrar al manager";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // inputPasswd
            // 
            this.inputPasswd.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.inputPasswd.Location = new System.Drawing.Point(374, 252);
            this.inputPasswd.Name = "inputPasswd";
            this.inputPasswd.PasswordChar = '*';
            this.inputPasswd.Size = new System.Drawing.Size(274, 32);
            this.inputPasswd.TabIndex = 6;
            // 
            // lblPasswd
            // 
            this.lblPasswd.AutoSize = true;
            this.lblPasswd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblPasswd.Location = new System.Drawing.Point(370, 224);
            this.lblPasswd.Name = "lblPasswd";
            this.lblPasswd.Size = new System.Drawing.Size(98, 23);
            this.lblPasswd.TabIndex = 5;
            this.lblPasswd.Text = "Contrasena";
            // 
            // inputUsername
            // 
            this.inputUsername.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.inputUsername.Location = new System.Drawing.Point(374, 173);
            this.inputUsername.Name = "inputUsername";
            this.inputUsername.Size = new System.Drawing.Size(274, 32);
            this.inputUsername.TabIndex = 4;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblUsername.Location = new System.Drawing.Point(370, 145);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(73, 23);
            this.lblUsername.TabIndex = 3;
            this.lblUsername.Text = "Usuario";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Location = new System.Drawing.Point(371, 88);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(277, 42);
            this.lblSubtitle.TabIndex = 2;
            this.lblSubtitle.Text = "Accede a tu inventario, construccion de decks y gestion de stock desde una sola pantalla.";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(367, 41);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(160, 41);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Bienvenido";
            // 
            // pnlHero
            // 
            this.pnlHero.Controls.Add(this.lblPercent);
            this.pnlHero.Controls.Add(this.progressShell);
            this.pnlHero.Controls.Add(this.lblStatus);
            this.pnlHero.Controls.Add(this.pictureBox1);
            this.pnlHero.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlHero.Location = new System.Drawing.Point(0, 0);
            this.pnlHero.Name = "pnlHero";
            this.pnlHero.Size = new System.Drawing.Size(320, 394);
            this.pnlHero.TabIndex = 0;
            // 
            // lblPercent
            // 
            this.lblPercent.AutoSize = true;
            this.lblPercent.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPercent.ForeColor = System.Drawing.Color.White;
            this.lblPercent.Location = new System.Drawing.Point(253, 307);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(29, 20);
            this.lblPercent.TabIndex = 3;
            this.lblPercent.Text = "0%";
            // 
            // progressShell
            // 
            this.progressShell.Controls.Add(this.progressFill);
            this.progressShell.Location = new System.Drawing.Point(38, 332);
            this.progressShell.Name = "progressShell";
            this.progressShell.Size = new System.Drawing.Size(244, 10);
            this.progressShell.TabIndex = 2;
            // 
            // progressFill
            // 
            this.progressFill.Dock = System.Windows.Forms.DockStyle.Left;
            this.progressFill.Location = new System.Drawing.Point(0, 0);
            this.progressFill.Name = "progressFill";
            this.progressFill.Size = new System.Drawing.Size(0, 10);
            this.progressFill.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(38, 262);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(244, 42);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Preparando entorno";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OnePieceTCG_Manager.Properties.Resources.optcg_manager_png;
            this.pictureBox1.Location = new System.Drawing.Point(68, 63);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(180, 144);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 466);
            this.Controls.Add(this.pnlCard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OPTCG Manager";
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.pnlCard.ResumeLayout(false);
            this.pnlCard.PerformLayout();
            this.pnlHero.ResumeLayout(false);
            this.pnlHero.PerformLayout();
            this.progressShell.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
