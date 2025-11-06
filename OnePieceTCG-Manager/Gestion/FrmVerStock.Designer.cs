namespace OnePieceTCG_Manager.Gestion
{
    partial class FrmVerStock
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.ToolStrip toolStripMain;
        private System.Windows.Forms.ToolStripButton toolListView;
        private System.Windows.Forms.ToolStripButton toolGalleryView;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel lblMode;
        private System.Windows.Forms.Panel panelContainer;

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
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolListView = new System.Windows.Forms.ToolStripButton();
            this.toolGalleryView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblMode = new System.Windows.Forms.ToolStripLabel();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.toolStripMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolListView,
            this.toolGalleryView,
            this.toolStripSeparator1,
            this.lblMode});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Padding = new System.Windows.Forms.Padding(5, 2, 5, 2);
            this.toolStripMain.Size = new System.Drawing.Size(1000, 31);
            this.toolStripMain.TabIndex = 0;
            this.toolStripMain.Text = "toolStrip1";
            // 
            // toolListView
            // 
            this.toolListView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolListView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolListView.Name = "toolListView";
            this.toolListView.Size = new System.Drawing.Size(72, 24);
            this.toolListView.Text = "📋 Lista";
            this.toolListView.Click += new System.EventHandler(this.toolListView_Click);
            // 
            // toolGalleryView
            // 
            this.toolGalleryView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolGalleryView.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.toolGalleryView.Name = "toolGalleryView";
            this.toolGalleryView.Size = new System.Drawing.Size(88, 24);
            this.toolGalleryView.Text = "🖼️ Galería";
            this.toolGalleryView.Click += new System.EventHandler(this.toolGalleryView_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // lblMode
            // 
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(86, 24);
            this.lblMode.Text = "Modo: Lista";
            // 
            // panelContainer
            // 
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 31);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(1000, 619);
            this.panelContainer.TabIndex = 1;
            // 
            // FrmVerStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.toolStripMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrmVerStock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ver Stock";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmVerStock_Load);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
