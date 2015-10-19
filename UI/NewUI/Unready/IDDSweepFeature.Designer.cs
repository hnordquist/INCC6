namespace NewUI
{
    partial class IDDSweepFeature
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
            this.SweepLabel = new System.Windows.Forms.Label();
            this.SelectLocationButton = new System.Windows.Forms.Button();
            this.SelectLocationTextBox = new System.Windows.Forms.TextBox();
            this.SweepCheckBox = new System.Windows.Forms.CheckBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SweepLabel
            // 
            this.SweepLabel.AutoSize = true;
            this.SweepLabel.Location = new System.Drawing.Point(25, 20);
            this.SweepLabel.Name = "SweepLabel";
            this.SweepLabel.Size = new System.Drawing.Size(392, 13);
            this.SweepLabel.TabIndex = 0;
            this.SweepLabel.Text = "Sweep measurement results text report and binary \'transfer\' files into this locat" +
    "ion...";
            // 
            // SelectLocationButton
            // 
            this.SelectLocationButton.Location = new System.Drawing.Point(14, 45);
            this.SelectLocationButton.Name = "SelectLocationButton";
            this.SelectLocationButton.Size = new System.Drawing.Size(75, 23);
            this.SelectLocationButton.TabIndex = 1;
            this.SelectLocationButton.Text = "Select Location";
            this.SelectLocationButton.UseVisualStyleBackColor = true;
            this.SelectLocationButton.Click += new System.EventHandler(this.SelectLocationButton_Click);
            // 
            // SelectLocationTextBox
            // 
            this.SelectLocationTextBox.Location = new System.Drawing.Point(106, 47);
            this.SelectLocationTextBox.Name = "SelectLocationTextBox";
            this.SelectLocationTextBox.Size = new System.Drawing.Size(330, 20);
            this.SelectLocationTextBox.TabIndex = 2;
            this.SelectLocationTextBox.TextChanged += new System.EventHandler(this.SelectLocationTextBox_TextChanged);
            // 
            // SweepCheckBox
            // 
            this.SweepCheckBox.AutoSize = true;
            this.SweepCheckBox.Location = new System.Drawing.Point(106, 83);
            this.SweepCheckBox.Name = "SweepCheckBox";
            this.SweepCheckBox.Size = new System.Drawing.Size(142, 17);
            this.SweepCheckBox.TabIndex = 3;
            this.SweepCheckBox.Text = "Use The Sweep Feature";
            this.SweepCheckBox.UseVisualStyleBackColor = true;
            this.SweepCheckBox.CheckedChanged += new System.EventHandler(this.SweepCheckBox_CheckedChanged);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(187, 118);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(106, 118);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(268, 118);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 7;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDSweepFeature
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 154);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.SweepCheckBox);
            this.Controls.Add(this.SelectLocationTextBox);
            this.Controls.Add(this.SelectLocationButton);
            this.Controls.Add(this.SweepLabel);
            this.Name = "IDDSweepFeature";
            this.Text = "IDDSweepFeature";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SweepLabel;
        private System.Windows.Forms.Button SelectLocationButton;
        private System.Windows.Forms.TextBox SelectLocationTextBox;
        private System.Windows.Forms.CheckBox SweepCheckBox;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button HelpBtn;
    }
}