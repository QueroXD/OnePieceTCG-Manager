namespace OnePieceTCG_Manager
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.ajustesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testConexiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.carpetaDeCartasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.añadirStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.estadisticasStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.economiaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ingresosEstimadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ingresosGanadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.misDecksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.explorarDecksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ajustesToolStripMenuItem,
            this.gestiónToolStripMenuItem,
            this.economiaToolStripMenuItem,
            this.decksToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menu.Size = new System.Drawing.Size(800, 28);
            this.menu.TabIndex = 0;
            this.menu.Text = "menu";
            // 
            // ajustesToolStripMenuItem
            // 
            this.ajustesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testConexiónToolStripMenuItem,
            this.carpetaDeCartasToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.ajustesToolStripMenuItem.Name = "ajustesToolStripMenuItem";
            this.ajustesToolStripMenuItem.Size = new System.Drawing.Size(70, 24);
            this.ajustesToolStripMenuItem.Text = "Ajustes";
            // 
            // testConexiónToolStripMenuItem
            // 
            this.testConexiónToolStripMenuItem.Name = "testConexiónToolStripMenuItem";
            this.testConexiónToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.testConexiónToolStripMenuItem.Text = "Test Conexión";
            // 
            // carpetaDeCartasToolStripMenuItem
            // 
            this.carpetaDeCartasToolStripMenuItem.Name = "carpetaDeCartasToolStripMenuItem";
            this.carpetaDeCartasToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.carpetaDeCartasToolStripMenuItem.Text = "Carpeta de cartas";
            this.carpetaDeCartasToolStripMenuItem.Click += new System.EventHandler(this.carpetaDeCartasToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(208, 26);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // gestiónToolStripMenuItem
            // 
            this.gestiónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verStockToolStripMenuItem,
            this.añadirStockToolStripMenuItem,
            this.estadisticasStockToolStripMenuItem});
            this.gestiónToolStripMenuItem.Name = "gestiónToolStripMenuItem";
            this.gestiónToolStripMenuItem.Size = new System.Drawing.Size(73, 24);
            this.gestiónToolStripMenuItem.Text = "Gestión";
            // 
            // verStockToolStripMenuItem
            // 
            this.verStockToolStripMenuItem.Name = "verStockToolStripMenuItem";
            this.verStockToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.verStockToolStripMenuItem.Text = "Ver Stock";
            this.verStockToolStripMenuItem.Click += new System.EventHandler(this.verStockToolStripMenuItem_Click);
            // 
            // añadirStockToolStripMenuItem
            // 
            this.añadirStockToolStripMenuItem.Name = "añadirStockToolStripMenuItem";
            this.añadirStockToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.añadirStockToolStripMenuItem.Text = "Añadir Stock";
            this.añadirStockToolStripMenuItem.Click += new System.EventHandler(this.añadirStockToolStripMenuItem_Click);
            // 
            // estadisticasStockToolStripMenuItem
            // 
            this.estadisticasStockToolStripMenuItem.Name = "estadisticasStockToolStripMenuItem";
            this.estadisticasStockToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.estadisticasStockToolStripMenuItem.Text = "Estadisticas Stock";
            this.estadisticasStockToolStripMenuItem.Visible = false;
            this.estadisticasStockToolStripMenuItem.Click += new System.EventHandler(this.estadisticasStockToolStripMenuItem_Click);
            // 
            // economiaToolStripMenuItem
            // 
            this.economiaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ingresosEstimadosToolStripMenuItem,
            this.ingresosGanadosToolStripMenuItem});
            this.economiaToolStripMenuItem.Name = "economiaToolStripMenuItem";
            this.economiaToolStripMenuItem.Size = new System.Drawing.Size(89, 24);
            this.economiaToolStripMenuItem.Text = "Economia";
            // 
            // ingresosEstimadosToolStripMenuItem
            // 
            this.ingresosEstimadosToolStripMenuItem.Name = "ingresosEstimadosToolStripMenuItem";
            this.ingresosEstimadosToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.ingresosEstimadosToolStripMenuItem.Text = "Ingresos estimados";
            // 
            // ingresosGanadosToolStripMenuItem
            // 
            this.ingresosGanadosToolStripMenuItem.Name = "ingresosGanadosToolStripMenuItem";
            this.ingresosGanadosToolStripMenuItem.Size = new System.Drawing.Size(219, 26);
            this.ingresosGanadosToolStripMenuItem.Text = "Ingresos Ganados";
            // 
            // decksToolStripMenuItem
            // 
            this.decksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.misDecksToolStripMenuItem,
            this.explorarDecksToolStripMenuItem});
            this.decksToolStripMenuItem.Name = "decksToolStripMenuItem";
            this.decksToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
            this.decksToolStripMenuItem.Text = "Decks";
            // 
            // misDecksToolStripMenuItem
            // 
            this.misDecksToolStripMenuItem.Name = "misDecksToolStripMenuItem";
            this.misDecksToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.misDecksToolStripMenuItem.Text = "Mis decks";
            this.misDecksToolStripMenuItem.Click += new System.EventHandler(this.misDecksToolStripMenuItem_Click);
            // 
            // explorarDecksToolStripMenuItem
            // 
            this.explorarDecksToolStripMenuItem.Name = "explorarDecksToolStripMenuItem";
            this.explorarDecksToolStripMenuItem.Size = new System.Drawing.Size(188, 26);
            this.explorarDecksToolStripMenuItem.Text = "Explorar decks";
            this.explorarDecksToolStripMenuItem.Click += new System.EventHandler(this.explorarDecksToolStripMenuItem_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "One Piece TCG - Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem ajustesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testConexiónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestiónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem añadirStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem estadisticasStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem economiaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ingresosEstimadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ingresosGanadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem carpetaDeCartasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem misDecksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem explorarDecksToolStripMenuItem;
    }
}
