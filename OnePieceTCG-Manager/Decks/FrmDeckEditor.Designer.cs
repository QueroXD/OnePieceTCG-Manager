namespace OnePieceTCG_Manager.Decks
{
    partial class FrmDeckEditor
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TextBox txtDeckName;
        private System.Windows.Forms.Label lblDeckName;
        private System.Windows.Forms.Label lblTotalCards;

        private System.Windows.Forms.FlowLayoutPanel flowStock;
        private System.Windows.Forms.FlowLayoutPanel flowLeaders;

        private System.Windows.Forms.DataGridView dgvDeck;

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.txtDeckName = new System.Windows.Forms.TextBox();
            this.lblDeckName = new System.Windows.Forms.Label();
            this.lblTotalCards = new System.Windows.Forms.Label();

            this.flowStock = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLeaders = new System.Windows.Forms.FlowLayoutPanel();

            this.dgvDeck = new System.Windows.Forms.DataGridView();

            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvDeck)).BeginInit();
            this.SuspendLayout();

            /* =======================
               Label y TextBox Nombre del Deck
               ======================= */
            this.lblDeckName.AutoSize = true;
            this.lblDeckName.Location = new System.Drawing.Point(20, 15);
            this.lblDeckName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDeckName.Text = "Nombre del Deck";

            this.txtDeckName.Location = new System.Drawing.Point(120, 12);
            this.txtDeckName.Size = new System.Drawing.Size(300, 23);
            this.txtDeckName.Font = new System.Drawing.Font("Segoe UI", 9F);

            /* =======================
               Label Total de Cartas
               ======================= */
            this.lblTotalCards.AutoSize = true;
            this.lblTotalCards.Location = new System.Drawing.Point(440, 15);
            this.lblTotalCards.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblTotalCards.Text = "Total cartas: 0 / 50";

            /* =======================
               FlowLayoutPanel Líderes (arriba)
               ======================= */
            this.flowLeaders.Location = new System.Drawing.Point(20, 45);
            this.flowLeaders.Size = new System.Drawing.Size(400, 120);
            this.flowLeaders.AutoScroll = true;
            this.flowLeaders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowLeaders.WrapContents = true;
            this.flowLeaders.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;

            /* =======================
               FlowLayoutPanel Stock (abajo)
               ======================= */
            this.flowStock.Location = new System.Drawing.Point(20, 175);
            this.flowStock.Size = new System.Drawing.Size(400, 320);
            this.flowStock.AutoScroll = true;
            this.flowStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flowStock.WrapContents = true;
            this.flowStock.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;

            /* =======================
               DataGridView Deck (derecha)
               ======================= */
            this.dgvDeck.Location = new System.Drawing.Point(440, 45);
            this.dgvDeck.Size = new System.Drawing.Size(340, 450);
            this.dgvDeck.AllowUserToAddRows = false;
            this.dgvDeck.AllowUserToDeleteRows = false;
            this.dgvDeck.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeck.MultiSelect = false;
            this.dgvDeck.ReadOnly = true;
            this.dgvDeck.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;

            /* =======================
               Botones Guardar / Cancelar
               ======================= */
            this.btnSave.Location = new System.Drawing.Point(600, 510);
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.Text = "Guardar";
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.Location = new System.Drawing.Point(690, 510);
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            /* =======================
               Form
               ======================= */
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 560);
            this.Controls.Add(this.txtDeckName);
            this.Controls.Add(this.lblDeckName);
            this.Controls.Add(this.lblTotalCards);

            this.Controls.Add(this.flowLeaders);
            this.Controls.Add(this.flowStock);

            this.Controls.Add(this.dgvDeck);

            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCancel);

            this.Text = "Editor de Decks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            ((System.ComponentModel.ISupportInitialize)(this.dgvDeck)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}
