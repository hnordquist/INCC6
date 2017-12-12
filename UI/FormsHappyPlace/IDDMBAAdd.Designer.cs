namespace UI
{
    partial class IDDMBAAdd
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
            this.CurrentMBALabel = new System.Windows.Forms.Label();
            this.CurrentMBAComboBox = new System.Windows.Forms.ComboBox();
            this.MBALabel = new System.Windows.Forms.Label();
            this.MBATextBox = new System.Windows.Forms.TextBox();
            this.MBADescriptionLabel = new System.Windows.Forms.Label();
            this.MBADescriptionTextBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentMBALabel
            // 
            this.CurrentMBALabel.AutoSize = true;
            this.CurrentMBALabel.Location = new System.Drawing.Point(37, 20);
            this.CurrentMBALabel.Name = "CurrentMBALabel";
            this.CurrentMBALabel.Size = new System.Drawing.Size(150, 13);
            this.CurrentMBALabel.TabIndex = 0;
            this.CurrentMBALabel.Text = "Current material balance areas";
            // 
            // CurrentMBAComboBox
            // 
            this.CurrentMBAComboBox.FormattingEnabled = true;
            this.CurrentMBAComboBox.Location = new System.Drawing.Point(193, 17);
            this.CurrentMBAComboBox.Name = "CurrentMBAComboBox";
            this.CurrentMBAComboBox.Size = new System.Drawing.Size(260, 21);
            this.CurrentMBAComboBox.TabIndex = 1;
            this.CurrentMBAComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrentMBAComboBox_SelectedIndexChanged);
            // 
            // MBALabel
            // 
            this.MBALabel.AutoSize = true;
            this.MBALabel.Location = new System.Drawing.Point(76, 92);
            this.MBALabel.Name = "MBALabel";
            this.MBALabel.Size = new System.Drawing.Size(111, 13);
            this.MBALabel.TabIndex = 2;
            this.MBALabel.Text = "Material Balance Area";
            // 
            // MBATextBox
            // 
            this.MBATextBox.Location = new System.Drawing.Point(193, 89);
            this.MBATextBox.Name = "MBATextBox";
            this.MBATextBox.Size = new System.Drawing.Size(260, 20);
            this.MBATextBox.TabIndex = 3;
            this.MBATextBox.TextChanged += new System.EventHandler(this.MBATextBox_TextChanged);
            // 
            // MBADescriptionLabel
            // 
            this.MBADescriptionLabel.AutoSize = true;
            this.MBADescriptionLabel.Location = new System.Drawing.Point(24, 131);
            this.MBADescriptionLabel.Name = "MBADescriptionLabel";
            this.MBADescriptionLabel.Size = new System.Drawing.Size(163, 13);
            this.MBADescriptionLabel.TabIndex = 4;
            this.MBADescriptionLabel.Text = "Material balance area description";
            // 
            // MBADescriptionTextBox
            // 
            this.MBADescriptionTextBox.Location = new System.Drawing.Point(193, 128);
            this.MBADescriptionTextBox.Name = "MBADescriptionTextBox";
            this.MBADescriptionTextBox.Size = new System.Drawing.Size(260, 20);
            this.MBADescriptionTextBox.TabIndex = 5;
            this.MBADescriptionTextBox.TextChanged += new System.EventHandler(this.MBADescriptionTextBox_TextChanged);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(499, 15);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(499, 44);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(499, 73);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDMBAAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(601, 175);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.MBADescriptionTextBox);
            this.Controls.Add(this.MBADescriptionLabel);
            this.Controls.Add(this.MBATextBox);
            this.Controls.Add(this.MBALabel);
            this.Controls.Add(this.CurrentMBAComboBox);
            this.Controls.Add(this.CurrentMBALabel);
            this.Name = "IDDMBAAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add a New Material Balance Area";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentMBALabel;
        private System.Windows.Forms.ComboBox CurrentMBAComboBox;
        private System.Windows.Forms.Label MBALabel;
        private System.Windows.Forms.TextBox MBATextBox;
        private System.Windows.Forms.Label MBADescriptionLabel;
        private System.Windows.Forms.TextBox MBADescriptionTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}