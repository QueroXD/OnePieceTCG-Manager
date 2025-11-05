namespace OnePieceTCG_Manager
{
    partial class FrmMain
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.menu = new System.Windows.Forms.MenuStrip();
            this.ajustesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testConexiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gestiónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.añadirStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.economiaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ingresosEstimadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ingresosGanadosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // menu
            // 
            this.menu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ajustesToolStripMenuItem,
            this.gestiónToolStripMenuItem,
            this.economiaToolStripMenuItem});
            this.menu.Location = new System.Drawing.Point(0, 0);
            this.menu.Name = "menu";
            this.menu.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menu.Size = new System.Drawing.Size(600, 24);
            this.menu.TabIndex = 0;
            this.menu.Text = "menu";
            // 
            // ajustesToolStripMenuItem
            // 
            this.ajustesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testConexiónToolStripMenuItem,
            this.salirToolStripMenuItem});
            this.ajustesToolStripMenuItem.Name = "ajustesToolStripMenuItem";
            this.ajustesToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.ajustesToolStripMenuItem.Text = "Ajustes";
            // 
            // testConexiónToolStripMenuItem
            // 
            this.testConexiónToolStripMenuItem.Name = "testConexiónToolStripMenuItem";
            this.testConexiónToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.testConexiónToolStripMenuItem.Text = "Test Conexión";
            this.testConexiónToolStripMenuItem.Click += new System.EventHandler(this.testConexiónToolStripMenuItem_Click);
            // 
            // salirToolStripMenuItem
            // 
            this.salirToolStripMenuItem.Name = "salirToolStripMenuItem";
            this.salirToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.salirToolStripMenuItem.Text = "Salir";
            this.salirToolStripMenuItem.Click += new System.EventHandler(this.salirToolStripMenuItem_Click);
            // 
            // gestiónToolStripMenuItem
            // 
            this.gestiónToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verStockToolStripMenuItem,
            this.añadirStockToolStripMenuItem});
            this.gestiónToolStripMenuItem.Name = "gestiónToolStripMenuItem";
            this.gestiónToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.gestiónToolStripMenuItem.Text = "Gestión";
            // 
            // verStockToolStripMenuItem
            // 
            this.verStockToolStripMenuItem.Name = "verStockToolStripMenuItem";
            this.verStockToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.verStockToolStripMenuItem.Text = "Ver Stock";
            this.verStockToolStripMenuItem.Click += new System.EventHandler(this.verStockToolStripMenuItem_Click);
            // 
            // añadirStockToolStripMenuItem
            // 
            this.añadirStockToolStripMenuItem.Name = "añadirStockToolStripMenuItem";
            this.añadirStockToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.añadirStockToolStripMenuItem.Text = "Añadir Stock";
            this.añadirStockToolStripMenuItem.Click += new System.EventHandler(this.añadirStockToolStripMenuItem_Click);
            // 
            // economiaToolStripMenuItem
            // 
            this.economiaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ingresosEstimadosToolStripMenuItem,
            this.ingresosGanadosToolStripMenuItem});
            this.economiaToolStripMenuItem.Name = "economiaToolStripMenuItem";
            this.economiaToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.economiaToolStripMenuItem.Text = "Economia";
            // 
            // ingresosEstimadosToolStripMenuItem
            // 
            this.ingresosEstimadosToolStripMenuItem.Name = "ingresosEstimadosToolStripMenuItem";
            this.ingresosEstimadosToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ingresosEstimadosToolStripMenuItem.Text = "Ingresos estimados";
            // 
            // ingresosGanadosToolStripMenuItem
            // 
            this.ingresosGanadosToolStripMenuItem.Name = "ingresosGanadosToolStripMenuItem";
            this.ingresosGanadosToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.ingresosGanadosToolStripMenuItem.Text = "Ingresos Ganados";
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 366);
            this.Controls.Add(this.menu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menu;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "One Piece TCG - Manager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menu;
        private System.Windows.Forms.ToolStripMenuItem ajustesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testConexiónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gestiónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem añadirStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem economiaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ingresosEstimadosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ingresosGanadosToolStripMenuItem;
    }
}

