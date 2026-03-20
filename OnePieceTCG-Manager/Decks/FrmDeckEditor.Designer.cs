namespace OnePieceTCG_Manager.Decks
{
    partial class FrmDeckEditor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tlpRoot;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDeckName;
        private System.Windows.Forms.TextBox txtDeckName;

        private System.Windows.Forms.SplitContainer splitMain;

        // Left: Deck
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlLeader;
        private System.Windows.Forms.PictureBox picLeader;
        private System.Windows.Forms.Label lblLeaderName;
        private System.Windows.Forms.Button btnClearLeader;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Panel pnlDeckCards;
        private System.Windows.Forms.Label lblDeckCardsTitle;

        // Right: Catalog
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.CheckBox chkShowNoStock;
        private System.Windows.Forms.Button btnClearFilters;

        private System.Windows.Forms.Label lblLeadersTitle;
        private System.Windows.Forms.FlowLayoutPanel flowLeaders;

        private System.Windows.Forms.Label lblCatalogTitle;
        private System.Windows.Forms.FlowLayoutPanel flowCatalog;

        // Bottom buttons
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tlpRoot = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblDeckName = new System.Windows.Forms.Label();
            this.txtDeckName = new System.Windows.Forms.TextBox();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlDeckCards = new System.Windows.Forms.Panel();
            this.lblDeckCardsTitle = new System.Windows.Forms.Label();
            this.pnlLeader = new System.Windows.Forms.Panel();
            this.picLeader = new System.Windows.Forms.PictureBox();
            this.lblLeaderName = new System.Windows.Forms.Label();
            this.btnClearLeader = new System.Windows.Forms.Button();
            this.lblTotal = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.flowCatalog = new System.Windows.Forms.FlowLayoutPanel();
            this.lblCatalogTitle = new System.Windows.Forms.Label();
            this.flowLeaders = new System.Windows.Forms.FlowLayoutPanel();
            this.lblLeadersTitle = new System.Windows.Forms.Label();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.chkShowNoStock = new System.Windows.Forms.CheckBox();
            this.btnClearFilters = new System.Windows.Forms.Button();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tlpRoot.SuspendLayout();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlLeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLeader)).BeginInit();
            this.pnlRight.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRoot
            // 
            this.tlpRoot.ColumnCount = 1;
            this.tlpRoot.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRoot.Controls.Add(this.pnlTop, 0, 0);
            this.tlpRoot.Controls.Add(this.splitMain, 0, 1);
            this.tlpRoot.Controls.Add(this.pnlBottom, 0, 2);
            this.tlpRoot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRoot.Location = new System.Drawing.Point(0, 0);
            this.tlpRoot.Name = "tlpRoot";
            this.tlpRoot.RowCount = 3;
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRoot.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.tlpRoot.Size = new System.Drawing.Size(1346, 653);
            this.tlpRoot.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Controls.Add(this.lblDeckName);
            this.pnlTop.Controls.Add(this.txtDeckName);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(3, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(12);
            this.pnlTop.Size = new System.Drawing.Size(1340, 58);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(165, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Editor de Deck";
            // 
            // lblDeckName
            // 
            this.lblDeckName.AutoSize = true;
            this.lblDeckName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDeckName.Location = new System.Drawing.Point(220, 20);
            this.lblDeckName.Name = "lblDeckName";
            this.lblDeckName.Size = new System.Drawing.Size(81, 23);
            this.lblDeckName.TabIndex = 1;
            this.lblDeckName.Text = "Nombre:";
            // 
            // txtDeckName
            // 
            this.txtDeckName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtDeckName.Location = new System.Drawing.Point(310, 17);
            this.txtDeckName.Name = "txtDeckName";
            this.txtDeckName.Size = new System.Drawing.Size(520, 30);
            this.txtDeckName.TabIndex = 2;
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(3, 67);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.pnlLeft);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.pnlRight);
            this.splitMain.Size = new System.Drawing.Size(1340, 519);
            this.splitMain.SplitterDistance = 627;
            this.splitMain.TabIndex = 1;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.pnlDeckCards);
            this.pnlLeft.Controls.Add(this.lblDeckCardsTitle);
            this.pnlLeft.Controls.Add(this.pnlLeader);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(12);
            this.pnlLeft.Size = new System.Drawing.Size(627, 519);
            this.pnlLeft.TabIndex = 0;
            // 
            // pnlDeckCards
            // 
            this.pnlDeckCards.AutoScroll = true;
            this.pnlDeckCards.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlDeckCards.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDeckCards.Location = new System.Drawing.Point(12, 186);
            this.pnlDeckCards.Name = "pnlDeckCards";
            this.pnlDeckCards.Padding = new System.Windows.Forms.Padding(0, 8, 0, 8);
            this.pnlDeckCards.Size = new System.Drawing.Size(603, 321);
            this.pnlDeckCards.TabIndex = 0;
            // 
            // lblDeckCardsTitle
            // 
            this.lblDeckCardsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDeckCardsTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDeckCardsTitle.Location = new System.Drawing.Point(12, 152);
            this.lblDeckCardsTitle.Name = "lblDeckCardsTitle";
            this.lblDeckCardsTitle.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblDeckCardsTitle.Size = new System.Drawing.Size(603, 34);
            this.lblDeckCardsTitle.TabIndex = 1;
            this.lblDeckCardsTitle.Text = "Cartas del Deck";
            // 
            // pnlLeader
            // 
            this.pnlLeader.BackColor = System.Drawing.Color.White;
            this.pnlLeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlLeader.Controls.Add(this.picLeader);
            this.pnlLeader.Controls.Add(this.lblLeaderName);
            this.pnlLeader.Controls.Add(this.btnClearLeader);
            this.pnlLeader.Controls.Add(this.lblTotal);
            this.pnlLeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLeader.Location = new System.Drawing.Point(12, 12);
            this.pnlLeader.Name = "pnlLeader";
            this.pnlLeader.Padding = new System.Windows.Forms.Padding(10);
            this.pnlLeader.Size = new System.Drawing.Size(603, 140);
            this.pnlLeader.TabIndex = 2;
            // 
            // picLeader
            // 
            this.picLeader.Location = new System.Drawing.Point(10, 10);
            this.picLeader.Name = "picLeader";
            this.picLeader.Size = new System.Drawing.Size(90, 120);
            this.picLeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLeader.TabIndex = 0;
            this.picLeader.TabStop = false;
            // 
            // lblLeaderName
            // 
            this.lblLeaderName.AutoEllipsis = true;
            this.lblLeaderName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLeaderName.Location = new System.Drawing.Point(110, 18);
            this.lblLeaderName.Name = "lblLeaderName";
            this.lblLeaderName.Size = new System.Drawing.Size(380, 26);
            this.lblLeaderName.TabIndex = 1;
            this.lblLeaderName.Text = "Sin líder seleccionado";
            // 
            // btnClearLeader
            // 
            this.btnClearLeader.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearLeader.Location = new System.Drawing.Point(110, 60);
            this.btnClearLeader.Name = "btnClearLeader";
            this.btnClearLeader.Size = new System.Drawing.Size(110, 30);
            this.btnClearLeader.TabIndex = 2;
            this.btnClearLeader.Text = "Cambiar lider";
            this.btnClearLeader.Click += new System.EventHandler(this.btnClearLeader_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotal.Location = new System.Drawing.Point(110, 100);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(253, 23);
            this.lblTotal.TabIndex = 3;
            this.lblTotal.Text = "Cartas: 0 / 50 (Líder aparte: 1)";
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.flowCatalog);
            this.pnlRight.Controls.Add(this.lblCatalogTitle);
            this.pnlRight.Controls.Add(this.flowLeaders);
            this.pnlRight.Controls.Add(this.lblLeadersTitle);
            this.pnlRight.Controls.Add(this.pnlFilters);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Location = new System.Drawing.Point(0, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Padding = new System.Windows.Forms.Padding(12);
            this.pnlRight.Size = new System.Drawing.Size(709, 519);
            this.pnlRight.TabIndex = 0;
            // 
            // flowCatalog
            // 
            this.flowCatalog.AutoScroll = true;
            this.flowCatalog.BackColor = System.Drawing.Color.White;
            this.flowCatalog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowCatalog.Location = new System.Drawing.Point(12, 363);
            this.flowCatalog.Name = "flowCatalog";
            this.flowCatalog.Size = new System.Drawing.Size(685, 144);
            this.flowCatalog.TabIndex = 0;
            // 
            // lblCatalogTitle
            // 
            this.lblCatalogTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCatalogTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCatalogTitle.Location = new System.Drawing.Point(12, 329);
            this.lblCatalogTitle.Name = "lblCatalogTitle";
            this.lblCatalogTitle.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblCatalogTitle.Size = new System.Drawing.Size(685, 34);
            this.lblCatalogTitle.TabIndex = 1;
            this.lblCatalogTitle.Text = "2. Catalogo bloqueado hasta elegir lider";
            // 
            // flowLeaders
            // 
            this.flowLeaders.AutoScroll = true;
            this.flowLeaders.BackColor = System.Drawing.Color.White;
            this.flowLeaders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLeaders.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLeaders.Location = new System.Drawing.Point(12, 110);
            this.flowLeaders.Name = "flowLeaders";
            this.flowLeaders.Size = new System.Drawing.Size(685, 219);
            this.flowLeaders.TabIndex = 2;
            this.flowLeaders.WrapContents = false;
            // 
            // lblLeadersTitle
            // 
            this.lblLeadersTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLeadersTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblLeadersTitle.Location = new System.Drawing.Point(12, 76);
            this.lblLeadersTitle.Name = "lblLeadersTitle";
            this.lblLeadersTitle.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.lblLeadersTitle.Size = new System.Drawing.Size(685, 34);
            this.lblLeadersTitle.TabIndex = 3;
            this.lblLeadersTitle.Text = "1. Elige tu lider";
            // 
            // pnlFilters
            // 
            this.pnlFilters.BackColor = System.Drawing.Color.White;
            this.pnlFilters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlFilters.Controls.Add(this.lblSearch);
            this.pnlFilters.Controls.Add(this.txtSearch);
            this.pnlFilters.Controls.Add(this.chkShowNoStock);
            this.pnlFilters.Controls.Add(this.btnClearFilters);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(12, 12);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Padding = new System.Windows.Forms.Padding(10);
            this.pnlFilters.Size = new System.Drawing.Size(685, 64);
            this.pnlFilters.TabIndex = 4;
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSearch.Location = new System.Drawing.Point(10, 18);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(68, 23);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Buscar:";
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(83, 14);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(299, 30);
            this.txtSearch.TabIndex = 1;
            // 
            // chkShowNoStock
            // 
            this.chkShowNoStock.AutoSize = true;
            this.chkShowNoStock.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.chkShowNoStock.Location = new System.Drawing.Point(416, 16);
            this.chkShowNoStock.Name = "chkShowNoStock";
            this.chkShowNoStock.Size = new System.Drawing.Size(132, 27);
            this.chkShowNoStock.TabIndex = 2;
            this.chkShowNoStock.Text = "Ver sin stock";
            // 
            // btnClearFilters
            // 
            this.btnClearFilters.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnClearFilters.Location = new System.Drawing.Point(584, 14);
            this.btnClearFilters.Name = "btnClearFilters";
            this.btnClearFilters.Size = new System.Drawing.Size(90, 32);
            this.btnClearFilters.TabIndex = 3;
            this.btnClearFilters.Text = "Limpiar";
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.White;
            this.pnlBottom.Controls.Add(this.btnSave);
            this.pnlBottom.Controls.Add(this.btnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(3, 592);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(12);
            this.pnlBottom.Size = new System.Drawing.Size(1340, 58);
            this.pnlBottom.TabIndex = 2;
            this.pnlBottom.Resize += new System.EventHandler(this.pnlBottom_Resize);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(1140, -42);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 38);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Guardar";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Location = new System.Drawing.Point(1140, -42);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 38);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancelar";
            // 
            // FrmDeckEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1346, 653);
            this.Controls.Add(this.tlpRoot);
            this.MinimumSize = new System.Drawing.Size(900, 620);
            this.Name = "FrmDeckEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Editor de Deck";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tlpRoot.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeader.ResumeLayout(false);
            this.pnlLeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLeader)).EndInit();
            this.pnlRight.ResumeLayout(false);
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}

