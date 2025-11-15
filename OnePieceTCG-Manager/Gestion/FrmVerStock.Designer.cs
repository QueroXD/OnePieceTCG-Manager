using System.Windows.Forms;

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

        // NUEVO: panel de filtros
        private System.Windows.Forms.Panel panelFilters;
        private System.Windows.Forms.ComboBox cbColor;
        private System.Windows.Forms.ComboBox cbType;
        private System.Windows.Forms.ComboBox cbSubType;
        private System.Windows.Forms.ComboBox cbOrder;
        private System.Windows.Forms.Button btnApplyFilters;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.toolStripMain = new System.Windows.Forms.ToolStrip();
            this.toolListView = new System.Windows.Forms.ToolStripButton();
            this.toolGalleryView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.lblMode = new System.Windows.Forms.ToolStripLabel();
            this.panelFilters = new System.Windows.Forms.Panel();
            this.cbColor = new System.Windows.Forms.ComboBox();
            this.cbType = new System.Windows.Forms.ComboBox();
            this.cbSubType = new System.Windows.Forms.ComboBox();
            this.cbOrder = new System.Windows.Forms.ComboBox();
            this.btnApplyFilters = new System.Windows.Forms.Button();
            this.panelContainer = new System.Windows.Forms.Panel();
            this.lblOrden = new System.Windows.Forms.Label();
            this.lblColor = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.lblSubtype = new System.Windows.Forms.Label();
            this.toolStripMain.SuspendLayout();
            this.panelFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMain
            // 
            this.toolStripMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStripMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolListView,
            this.toolGalleryView,
            this.toolStripSeparator1,
            this.lblMode});
            this.toolStripMain.Location = new System.Drawing.Point(0, 0);
            this.toolStripMain.Name = "toolStripMain";
            this.toolStripMain.Size = new System.Drawing.Size(874, 25);
            this.toolStripMain.TabIndex = 2;
            // 
            // toolListView
            // 
            this.toolListView.Name = "toolListView";
            this.toolListView.Size = new System.Drawing.Size(50, 22);
            this.toolListView.Text = "📋 Lista";
            this.toolListView.Click += new System.EventHandler(this.toolListView_Click);
            // 
            // toolGalleryView
            // 
            this.toolGalleryView.Name = "toolGalleryView";
            this.toolGalleryView.Size = new System.Drawing.Size(62, 22);
            this.toolGalleryView.Text = "🖼️ Galería";
            this.toolGalleryView.Click += new System.EventHandler(this.toolGalleryView_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // lblMode
            // 
            this.lblMode.Name = "lblMode";
            this.lblMode.Size = new System.Drawing.Size(0, 22);
            // 
            // panelFilters
            // 
            this.panelFilters.Controls.Add(this.btnApplyFilters);
            this.panelFilters.Controls.Add(this.cbOrder);
            this.panelFilters.Controls.Add(this.cbType);
            this.panelFilters.Controls.Add(this.cbSubType);
            this.panelFilters.Controls.Add(this.lblSubtype);
            this.panelFilters.Controls.Add(this.lblTipo);
            this.panelFilters.Controls.Add(this.lblColor);
            this.panelFilters.Controls.Add(this.lblOrden);
            this.panelFilters.Controls.Add(this.cbColor);
            this.panelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilters.Location = new System.Drawing.Point(0, 25);
            this.panelFilters.Name = "panelFilters";
            this.panelFilters.Padding = new System.Windows.Forms.Padding(5);
            this.panelFilters.Size = new System.Drawing.Size(874, 26);
            this.panelFilters.TabIndex = 1;
            // 
            // cbColor
            // 
            this.cbColor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbColor.Location = new System.Drawing.Point(661, 2);
            this.cbColor.Name = "cbColor";
            this.cbColor.Size = new System.Drawing.Size(120, 21);
            this.cbColor.TabIndex = 0;
            // 
            // cbType
            // 
            this.cbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbType.Location = new System.Drawing.Point(297, 2);
            this.cbType.Name = "cbType";
            this.cbType.Size = new System.Drawing.Size(120, 21);
            this.cbType.TabIndex = 1;
            // 
            // cbSubType
            // 
            this.cbSubType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSubType.Location = new System.Drawing.Point(470, 2);
            this.cbSubType.Name = "cbSubType";
            this.cbSubType.Size = new System.Drawing.Size(150, 21);
            this.cbSubType.TabIndex = 2;
            // 
            // cbOrder
            // 
            this.cbOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrder.Location = new System.Drawing.Point(72, 2);
            this.cbOrder.Name = "cbOrder";
            this.cbOrder.Size = new System.Drawing.Size(150, 21);
            this.cbOrder.TabIndex = 3;
            // 
            // btnApplyFilters
            // 
            this.btnApplyFilters.Location = new System.Drawing.Point(790, 2);
            this.btnApplyFilters.Name = "btnApplyFilters";
            this.btnApplyFilters.Size = new System.Drawing.Size(79, 22);
            this.btnApplyFilters.TabIndex = 4;
            this.btnApplyFilters.Text = "Aplicar filtros";
            // 
            // panelContainer
            // 
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 51);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(874, 210);
            this.panelContainer.TabIndex = 0;
            // 
            // lblOrden
            // 
            this.lblOrden.AutoSize = true;
            this.lblOrden.Location = new System.Drawing.Point(8, 5);
            this.lblOrden.Name = "lblOrden";
            this.lblOrden.Size = new System.Drawing.Size(66, 13);
            this.lblOrden.TabIndex = 5;
            this.lblOrden.Text = "Ordenar por:";
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(625, 5);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(34, 13);
            this.lblColor.TabIndex = 6;
            this.lblColor.Text = "Color:";
            // 
            // lblTipo
            // 
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(225, 5);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(73, 13);
            this.lblTipo.TabIndex = 7;
            this.lblTipo.Text = "Tipo de carta:";
            // 
            // lblSubtype
            // 
            this.lblSubtype.AutoSize = true;
            this.lblSubtype.Location = new System.Drawing.Point(422, 4);
            this.lblSubtype.Name = "lblSubtype";
            this.lblSubtype.Size = new System.Drawing.Size(50, 13);
            this.lblSubtype.TabIndex = 8;
            this.lblSubtype.Text = "SubTipo:";
            // 
            // FrmVerStock
            // 
            this.ClientSize = new System.Drawing.Size(874, 261);
            this.Controls.Add(this.panelContainer);
            this.Controls.Add(this.panelFilters);
            this.Controls.Add(this.toolStripMain);
            this.Name = "FrmVerStock";
            this.Text = "Ver Stock";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmVerStock_Load);
            this.toolStripMain.ResumeLayout(false);
            this.toolStripMain.PerformLayout();
            this.panelFilters.ResumeLayout(false);
            this.panelFilters.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Label lblSubtype;
        private Label lblTipo;
        private Label lblColor;
        private Label lblOrden;
    }
}
