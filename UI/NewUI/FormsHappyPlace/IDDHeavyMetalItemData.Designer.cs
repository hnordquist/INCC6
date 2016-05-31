namespace NewUI
{
    partial class IDDHeavyMetalItemData
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
            this.DeclaredUMassLabel = new System.Windows.Forms.Label();
            this.LengthLabel = new System.Windows.Forms.Label();
            this.DeclaredUMassTextBox = new System.Windows.Forms.TextBox();
            this.LengthTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DeclaredUMassLabel
            // 
            this.DeclaredUMassLabel.AutoSize = true;
            this.DeclaredUMassLabel.Location = new System.Drawing.Point(20, 24);
            this.DeclaredUMassLabel.Name = "DeclaredUMassLabel";
            this.DeclaredUMassLabel.Size = new System.Drawing.Size(103, 13);
            this.DeclaredUMassLabel.TabIndex = 0;
            this.DeclaredUMassLabel.Text = "Declared U mass (g)";
            // 
            // LengthLabel
            // 
            this.LengthLabel.AutoSize = true;
            this.LengthLabel.Location = new System.Drawing.Point(60, 50);
            this.LengthLabel.Name = "LengthLabel";
            this.LengthLabel.Size = new System.Drawing.Size(63, 13);
            this.LengthLabel.TabIndex = 1;
            this.LengthLabel.Text = "Length (cm)";
            // 
            // DeclaredUMassTextBox
            // 
            this.DeclaredUMassTextBox.Location = new System.Drawing.Point(129, 21);
            this.DeclaredUMassTextBox.Name = "DeclaredUMassTextBox";
            this.DeclaredUMassTextBox.Size = new System.Drawing.Size(100, 20);
            this.DeclaredUMassTextBox.TabIndex = 2;
            this.DeclaredUMassTextBox.Leave += new System.EventHandler(this.DeclaredUMassTextBox_Leave);
            // 
            // LengthTextBox
            // 
            this.LengthTextBox.Location = new System.Drawing.Point(129, 47);
            this.LengthTextBox.Name = "LengthTextBox";
            this.LengthTextBox.Size = new System.Drawing.Size(100, 20);
            this.LengthTextBox.TabIndex = 3;
            this.LengthTextBox.Leave += new System.EventHandler(this.LengthTextBox_Leave);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(264, 19);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(264, 48);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(264, 77);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 6;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDHeavyMetalItemData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 114);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.LengthTextBox);
            this.Controls.Add(this.DeclaredUMassTextBox);
            this.Controls.Add(this.LengthLabel);
            this.Controls.Add(this.DeclaredUMassLabel);
            this.Name = "IDDHeavyMetalItemData";
            this.Text = "Enter Heavy Metal Item Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DeclaredUMassLabel;
        private System.Windows.Forms.Label LengthLabel;
        private System.Windows.Forms.TextBox DeclaredUMassTextBox;
        private System.Windows.Forms.TextBox LengthTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}