namespace NewUI
{
    partial class IDDPoisonRodTypeAdd
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
            this.CurrentPoisonRodTypesLabel = new System.Windows.Forms.Label();
            this.PoisonRodTypeLabel = new System.Windows.Forms.Label();
            this.DefaultPoisonAbsorptionLabel = new System.Windows.Forms.Label();
            this.CurrentPoisonRodTypesComboBox = new System.Windows.Forms.ComboBox();
            this.PoisonRodTypeTextBox = new System.Windows.Forms.TextBox();
            this.DefaultPoisonAbsorptionTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentPoisonRodTypesLabel
            // 
            this.CurrentPoisonRodTypesLabel.AutoSize = true;
            this.CurrentPoisonRodTypesLabel.Location = new System.Drawing.Point(12, 19);
            this.CurrentPoisonRodTypesLabel.Name = "CurrentPoisonRodTypesLabel";
            this.CurrentPoisonRodTypesLabel.Size = new System.Drawing.Size(121, 13);
            this.CurrentPoisonRodTypesLabel.TabIndex = 0;
            this.CurrentPoisonRodTypesLabel.Text = "Current poison rod types";
            // 
            // PoisonRodTypeLabel
            // 
            this.PoisonRodTypeLabel.AutoSize = true;
            this.PoisonRodTypeLabel.Location = new System.Drawing.Point(53, 49);
            this.PoisonRodTypeLabel.Name = "PoisonRodTypeLabel";
            this.PoisonRodTypeLabel.Size = new System.Drawing.Size(80, 13);
            this.PoisonRodTypeLabel.TabIndex = 1;
            this.PoisonRodTypeLabel.Text = "Poison rod type";
            // 
            // DefaultPoisonAbsorptionLabel
            // 
            this.DefaultPoisonAbsorptionLabel.AutoSize = true;
            this.DefaultPoisonAbsorptionLabel.Location = new System.Drawing.Point(6, 78);
            this.DefaultPoisonAbsorptionLabel.Name = "DefaultPoisonAbsorptionLabel";
            this.DefaultPoisonAbsorptionLabel.Size = new System.Drawing.Size(127, 13);
            this.DefaultPoisonAbsorptionLabel.TabIndex = 2;
            this.DefaultPoisonAbsorptionLabel.Text = "Default poison absorption";
            // 
            // CurrentPoisonRodTypesComboBox
            // 
            this.CurrentPoisonRodTypesComboBox.FormattingEnabled = true;
            this.CurrentPoisonRodTypesComboBox.Location = new System.Drawing.Point(139, 16);
            this.CurrentPoisonRodTypesComboBox.Name = "CurrentPoisonRodTypesComboBox";
            this.CurrentPoisonRodTypesComboBox.Size = new System.Drawing.Size(85, 21);
            this.CurrentPoisonRodTypesComboBox.TabIndex = 3;
            this.CurrentPoisonRodTypesComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrentPoisonRodTypesComboBox_SelectedIndexChanged);
            // 
            // PoisonRodTypeTextBox
            // 
            this.PoisonRodTypeTextBox.Location = new System.Drawing.Point(139, 46);
            this.PoisonRodTypeTextBox.Name = "PoisonRodTypeTextBox";
            this.PoisonRodTypeTextBox.Size = new System.Drawing.Size(85, 20);
            this.PoisonRodTypeTextBox.TabIndex = 4;
            this.PoisonRodTypeTextBox.Leave += new System.EventHandler(this.PoisonRodTypeTextBox_Leave);
            // 
            // DefaultPoisonAbsorptionTextBox
            // 
            this.DefaultPoisonAbsorptionTextBox.Location = new System.Drawing.Point(139, 75);
            this.DefaultPoisonAbsorptionTextBox.Name = "DefaultPoisonAbsorptionTextBox";
            this.DefaultPoisonAbsorptionTextBox.Size = new System.Drawing.Size(85, 20);
            this.DefaultPoisonAbsorptionTextBox.TabIndex = 5;
            this.DefaultPoisonAbsorptionTextBox.Leave += new System.EventHandler(this.DefaultPoisonAbsorptionTextBox_Leave);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(252, 14);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 6;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(252, 43);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(252, 72);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDPoisonRodTypeAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 109);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DefaultPoisonAbsorptionTextBox);
            this.Controls.Add(this.PoisonRodTypeTextBox);
            this.Controls.Add(this.CurrentPoisonRodTypesComboBox);
            this.Controls.Add(this.DefaultPoisonAbsorptionLabel);
            this.Controls.Add(this.PoisonRodTypeLabel);
            this.Controls.Add(this.CurrentPoisonRodTypesLabel);
            this.Name = "IDDPoisonRodTypeAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Poison Rod Type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentPoisonRodTypesLabel;
        private System.Windows.Forms.Label PoisonRodTypeLabel;
        private System.Windows.Forms.Label DefaultPoisonAbsorptionLabel;
        private System.Windows.Forms.ComboBox CurrentPoisonRodTypesComboBox;
        private System.Windows.Forms.TextBox PoisonRodTypeTextBox;
        private System.Windows.Forms.TextBox DefaultPoisonAbsorptionTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}