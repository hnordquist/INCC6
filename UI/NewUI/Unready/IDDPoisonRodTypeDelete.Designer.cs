namespace NewUI
{
    partial class IDDPoisonRodTypeDelete
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
            this.PoisonRodTypeLabel = new System.Windows.Forms.Label();
            this.PoisonRodTypeComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // PoisonRodTypeLabel
            // 
            this.PoisonRodTypeLabel.AutoSize = true;
            this.PoisonRodTypeLabel.Location = new System.Drawing.Point(12, 20);
            this.PoisonRodTypeLabel.Name = "PoisonRodTypeLabel";
            this.PoisonRodTypeLabel.Size = new System.Drawing.Size(80, 13);
            this.PoisonRodTypeLabel.TabIndex = 0;
            this.PoisonRodTypeLabel.Text = "Poison rod type";
            // 
            // PoisonRodTypeComboBox
            // 
            this.PoisonRodTypeComboBox.FormattingEnabled = true;
            this.PoisonRodTypeComboBox.Location = new System.Drawing.Point(98, 17);
            this.PoisonRodTypeComboBox.Name = "PoisonRodTypeComboBox";
            this.PoisonRodTypeComboBox.Size = new System.Drawing.Size(66, 21);
            this.PoisonRodTypeComboBox.TabIndex = 1;
            this.PoisonRodTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.PoisonRodTypeComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(201, 15);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(201, 44);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(201, 73);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 5;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDPoisonRodTypeDelete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 113);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PoisonRodTypeComboBox);
            this.Controls.Add(this.PoisonRodTypeLabel);
            this.Name = "IDDPoisonRodTypeDelete";
            this.Text = "Select a Poison Rod Type to Delete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label PoisonRodTypeLabel;
        private System.Windows.Forms.ComboBox PoisonRodTypeComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}