namespace OnePieceTCG_Manager.Decks
{
    partial class FrmMyDecks
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvDecks;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvDecks = new System.Windows.Forms.DataGridView();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvDecks)).BeginInit();
            this.SuspendLayout();

            // dgvDecks
            this.dgvDecks.Location = new System.Drawing.Point(12, 12);
            this.dgvDecks.Size = new System.Drawing.Size(600, 350);
            this.dgvDecks.ReadOnly = true;
            this.dgvDecks.AllowUserToAddRows = false;
            this.dgvDecks.AllowUserToDeleteRows = false;
            this.dgvDecks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDecks.MultiSelect = false;

            // btnCreate
            this.btnCreate.Location = new System.Drawing.Point(630, 20);
            this.btnCreate.Size = new System.Drawing.Size(120, 30);
            this.btnCreate.Text = "Crear Deck";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);

            // btnEdit
            this.btnEdit.Location = new System.Drawing.Point(630, 60);
            this.btnEdit.Size = new System.Drawing.Size(120, 30);
            this.btnEdit.Text = "Editar Deck";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(630, 100);
            this.btnDelete.Size = new System.Drawing.Size(120, 30);
            this.btnDelete.Text = "Borrar Deck";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);

            // FrmMyDecks
            this.ClientSize = new System.Drawing.Size(770, 380);
            this.Controls.Add(this.dgvDecks);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnDelete);
            this.Text = "Mis Decks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;

            ((System.ComponentModel.ISupportInitialize)(this.dgvDecks)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
