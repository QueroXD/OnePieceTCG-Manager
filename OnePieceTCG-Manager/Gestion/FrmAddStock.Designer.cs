namespace OnePieceTCG_Manager.Gestion
{
    partial class FrmAddStock
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fotoCard = new System.Windows.Forms.PictureBox();
            this.lblCardID = new System.Windows.Forms.Label();
            this.inputCardID = new System.Windows.Forms.TextBox();
            this.inputCardName = new System.Windows.Forms.TextBox();
            this.lblCardName = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.isAlter = new System.Windows.Forms.CheckBox();
            this.inputCardType = new System.Windows.Forms.TextBox();
            this.lblCardType = new System.Windows.Forms.Label();
            this.inputRarity = new System.Windows.Forms.TextBox();
            this.lblRarity = new System.Windows.Forms.Label();
            this.inputCost = new System.Windows.Forms.TextBox();
            this.lblCost = new System.Windows.Forms.Label();
            this.inputLifes = new System.Windows.Forms.TextBox();
            this.lblLifes = new System.Windows.Forms.Label();
            this.inputCardSubType = new System.Windows.Forms.TextBox();
            this.lblCardSubType = new System.Windows.Forms.Label();
            this.inputColor = new System.Windows.Forms.TextBox();
            this.lblColor = new System.Windows.Forms.Label();
            this.inputPower = new System.Windows.Forms.TextBox();
            this.lblPower = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.inputCounter = new System.Windows.Forms.TextBox();
            this.inputDescription = new System.Windows.Forms.RichTextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fotoCard)).BeginInit();
            this.SuspendLayout();
            // 
            // fotoCard
            // 
            this.fotoCard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fotoCard.Location = new System.Drawing.Point(634, 13);
            this.fotoCard.Margin = new System.Windows.Forms.Padding(5);
            this.fotoCard.Name = "fotoCard";
            this.fotoCard.Size = new System.Drawing.Size(300, 419);
            this.fotoCard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.fotoCard.TabIndex = 0;
            this.fotoCard.TabStop = false;
            // 
            // lblCardID
            // 
            this.lblCardID.AutoSize = true;
            this.lblCardID.Location = new System.Drawing.Point(14, 36);
            this.lblCardID.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCardID.Name = "lblCardID";
            this.lblCardID.Size = new System.Drawing.Size(105, 25);
            this.lblCardID.TabIndex = 1;
            this.lblCardID.Text = "Card ID: ";
            // 
            // inputCardID
            // 
            this.inputCardID.Location = new System.Drawing.Point(113, 35);
            this.inputCardID.Name = "inputCardID";
            this.inputCardID.Size = new System.Drawing.Size(103, 32);
            this.inputCardID.TabIndex = 2;
            // 
            // inputCardName
            // 
            this.inputCardName.Location = new System.Drawing.Point(175, 73);
            this.inputCardName.Name = "inputCardName";
            this.inputCardName.Size = new System.Drawing.Size(173, 32);
            this.inputCardName.TabIndex = 4;
            // 
            // lblCardName
            // 
            this.lblCardName.AutoSize = true;
            this.lblCardName.Location = new System.Drawing.Point(14, 73);
            this.lblCardName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCardName.Name = "lblCardName";
            this.lblCardName.Size = new System.Drawing.Size(140, 25);
            this.lblCardName.TabIndex = 3;
            this.lblCardName.Text = "Card Name: ";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(222, 35);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(113, 32);
            this.btnSearch.TabIndex = 5;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // isAlter
            // 
            this.isAlter.AutoSize = true;
            this.isAlter.Location = new System.Drawing.Point(354, 38);
            this.isAlter.Name = "isAlter";
            this.isAlter.Size = new System.Drawing.Size(113, 29);
            this.isAlter.TabIndex = 6;
            this.isAlter.Text = "Es Alter";
            this.isAlter.UseVisualStyleBackColor = true;
            // 
            // inputCardType
            // 
            this.inputCardType.Location = new System.Drawing.Point(175, 111);
            this.inputCardType.Name = "inputCardType";
            this.inputCardType.Size = new System.Drawing.Size(173, 32);
            this.inputCardType.TabIndex = 8;
            // 
            // lblCardType
            // 
            this.lblCardType.AutoSize = true;
            this.lblCardType.Location = new System.Drawing.Point(14, 111);
            this.lblCardType.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCardType.Name = "lblCardType";
            this.lblCardType.Size = new System.Drawing.Size(128, 25);
            this.lblCardType.TabIndex = 7;
            this.lblCardType.Text = "Card Type: ";
            // 
            // inputRarity
            // 
            this.inputRarity.Location = new System.Drawing.Point(443, 73);
            this.inputRarity.Name = "inputRarity";
            this.inputRarity.Size = new System.Drawing.Size(42, 32);
            this.inputRarity.TabIndex = 14;
            // 
            // lblRarity
            // 
            this.lblRarity.AutoSize = true;
            this.lblRarity.Location = new System.Drawing.Point(361, 73);
            this.lblRarity.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblRarity.Name = "lblRarity";
            this.lblRarity.Size = new System.Drawing.Size(89, 25);
            this.lblRarity.TabIndex = 13;
            this.lblRarity.Text = "Rarity: ";
            // 
            // inputCost
            // 
            this.inputCost.Location = new System.Drawing.Point(443, 111);
            this.inputCost.Name = "inputCost";
            this.inputCost.Size = new System.Drawing.Size(42, 32);
            this.inputCost.TabIndex = 16;
            // 
            // lblCost
            // 
            this.lblCost.AutoSize = true;
            this.lblCost.Location = new System.Drawing.Point(361, 111);
            this.lblCost.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(72, 25);
            this.lblCost.TabIndex = 15;
            this.lblCost.Text = "Cost: ";
            // 
            // inputLifes
            // 
            this.inputLifes.Location = new System.Drawing.Point(557, 75);
            this.inputLifes.Name = "inputLifes";
            this.inputLifes.Size = new System.Drawing.Size(42, 32);
            this.inputLifes.TabIndex = 18;
            // 
            // lblLifes
            // 
            this.lblLifes.AutoSize = true;
            this.lblLifes.Location = new System.Drawing.Point(491, 75);
            this.lblLifes.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblLifes.Name = "lblLifes";
            this.lblLifes.Size = new System.Drawing.Size(74, 25);
            this.lblLifes.TabIndex = 17;
            this.lblLifes.Text = "Lifes: ";
            // 
            // inputCardSubType
            // 
            this.inputCardSubType.Location = new System.Drawing.Point(175, 150);
            this.inputCardSubType.Name = "inputCardSubType";
            this.inputCardSubType.Size = new System.Drawing.Size(424, 32);
            this.inputCardSubType.TabIndex = 20;
            // 
            // lblCardSubType
            // 
            this.lblCardSubType.AutoSize = true;
            this.lblCardSubType.Location = new System.Drawing.Point(14, 150);
            this.lblCardSubType.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCardSubType.Name = "lblCardSubType";
            this.lblCardSubType.Size = new System.Drawing.Size(167, 25);
            this.lblCardSubType.TabIndex = 19;
            this.lblCardSubType.Text = "Card SubType: ";
            // 
            // inputColor
            // 
            this.inputColor.Location = new System.Drawing.Point(86, 186);
            this.inputColor.Name = "inputColor";
            this.inputColor.Size = new System.Drawing.Size(116, 32);
            this.inputColor.TabIndex = 22;
            // 
            // lblColor
            // 
            this.lblColor.AutoSize = true;
            this.lblColor.Location = new System.Drawing.Point(14, 187);
            this.lblColor.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblColor.Name = "lblColor";
            this.lblColor.Size = new System.Drawing.Size(81, 25);
            this.lblColor.TabIndex = 21;
            this.lblColor.Text = "Color: ";
            // 
            // inputPower
            // 
            this.inputPower.Location = new System.Drawing.Point(290, 188);
            this.inputPower.Name = "inputPower";
            this.inputPower.Size = new System.Drawing.Size(82, 32);
            this.inputPower.TabIndex = 24;
            this.inputPower.Text = "12000";
            // 
            // lblPower
            // 
            this.lblPower.AutoSize = true;
            this.lblPower.Location = new System.Drawing.Point(210, 189);
            this.lblPower.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPower.Name = "lblPower";
            this.lblPower.Size = new System.Drawing.Size(89, 25);
            this.lblPower.TabIndex = 23;
            this.lblPower.Text = "Power: ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(376, 191);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 25);
            this.label1.TabIndex = 25;
            this.label1.Text = "Counter: ";
            // 
            // inputCounter
            // 
            this.inputCounter.Location = new System.Drawing.Point(476, 190);
            this.inputCounter.Name = "inputCounter";
            this.inputCounter.Size = new System.Drawing.Size(82, 32);
            this.inputCounter.TabIndex = 26;
            this.inputCounter.Text = "2000";
            // 
            // inputDescription
            // 
            this.inputDescription.Location = new System.Drawing.Point(23, 262);
            this.inputDescription.Name = "inputDescription";
            this.inputDescription.Size = new System.Drawing.Size(597, 169);
            this.inputDescription.TabIndex = 27;
            this.inputDescription.Text = "";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(18, 234);
            this.lblDescription.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(141, 25);
            this.lblDescription.TabIndex = 28;
            this.lblDescription.Text = "Description: ";
            // 
            // FrmAddStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(940, 446);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.inputDescription);
            this.Controls.Add(this.inputCounter);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.inputPower);
            this.Controls.Add(this.lblPower);
            this.Controls.Add(this.inputColor);
            this.Controls.Add(this.lblColor);
            this.Controls.Add(this.inputCardSubType);
            this.Controls.Add(this.lblCardSubType);
            this.Controls.Add(this.inputLifes);
            this.Controls.Add(this.lblLifes);
            this.Controls.Add(this.inputCost);
            this.Controls.Add(this.lblCost);
            this.Controls.Add(this.inputRarity);
            this.Controls.Add(this.lblRarity);
            this.Controls.Add(this.inputCardType);
            this.Controls.Add(this.lblCardType);
            this.Controls.Add(this.isAlter);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.inputCardName);
            this.Controls.Add(this.lblCardName);
            this.Controls.Add(this.inputCardID);
            this.Controls.Add(this.lblCardID);
            this.Controls.Add(this.fotoCard);
            this.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FrmAddStock";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Añadir Stock";
            ((System.ComponentModel.ISupportInitialize)(this.fotoCard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox fotoCard;
        private System.Windows.Forms.Label lblCardID;
        private System.Windows.Forms.TextBox inputCardID;
        private System.Windows.Forms.TextBox inputCardName;
        private System.Windows.Forms.Label lblCardName;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.CheckBox isAlter;
        private System.Windows.Forms.TextBox inputCardType;
        private System.Windows.Forms.Label lblCardType;
        private System.Windows.Forms.TextBox inputRarity;
        private System.Windows.Forms.Label lblRarity;
        private System.Windows.Forms.TextBox inputCost;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.TextBox inputLifes;
        private System.Windows.Forms.Label lblLifes;
        private System.Windows.Forms.TextBox inputCardSubType;
        private System.Windows.Forms.Label lblCardSubType;
        private System.Windows.Forms.TextBox inputColor;
        private System.Windows.Forms.Label lblColor;
        private System.Windows.Forms.TextBox inputPower;
        private System.Windows.Forms.Label lblPower;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox inputCounter;
        private System.Windows.Forms.RichTextBox inputDescription;
        private System.Windows.Forms.Label lblDescription;
    }
}