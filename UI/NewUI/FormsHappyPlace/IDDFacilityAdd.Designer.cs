namespace NewUI
{
    partial class IDDFacilityAdd
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
            this.CurrentFacilitiesLabel = new System.Windows.Forms.Label();
            this.CurrentFacilitiesComboBox = new System.Windows.Forms.ComboBox();
            this.FacilityLabel = new System.Windows.Forms.Label();
            this.FacilityTextBox = new System.Windows.Forms.TextBox();
            this.FacilityDescriptionLabel = new System.Windows.Forms.Label();
            this.FacilityDescriptionTextBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CurrentFacilitiesLabel
            // 
            this.CurrentFacilitiesLabel.AutoSize = true;
            this.CurrentFacilitiesLabel.Location = new System.Drawing.Point(23, 17);
            this.CurrentFacilitiesLabel.Name = "CurrentFacilitiesLabel";
            this.CurrentFacilitiesLabel.Size = new System.Drawing.Size(84, 13);
            this.CurrentFacilitiesLabel.TabIndex = 0;
            this.CurrentFacilitiesLabel.Text = "Current Facilities";
            // 
            // CurrentFacilitiesComboBox
            // 
            this.CurrentFacilitiesComboBox.FormattingEnabled = true;
            this.CurrentFacilitiesComboBox.Location = new System.Drawing.Point(113, 14);
            this.CurrentFacilitiesComboBox.Name = "CurrentFacilitiesComboBox";
            this.CurrentFacilitiesComboBox.Size = new System.Drawing.Size(225, 21);
            this.CurrentFacilitiesComboBox.TabIndex = 1;
            this.CurrentFacilitiesComboBox.SelectedIndexChanged += new System.EventHandler(this.CurrentFacilitiesComboBox_SelectedIndexChanged);
            // 
            // FacilityLabel
            // 
            this.FacilityLabel.AutoSize = true;
            this.FacilityLabel.Location = new System.Drawing.Point(68, 93);
            this.FacilityLabel.Name = "FacilityLabel";
            this.FacilityLabel.Size = new System.Drawing.Size(39, 13);
            this.FacilityLabel.TabIndex = 2;
            this.FacilityLabel.Text = "Facility";
            // 
            // FacilityTextBox
            // 
            this.FacilityTextBox.Location = new System.Drawing.Point(113, 90);
            this.FacilityTextBox.Name = "FacilityTextBox";
            this.FacilityTextBox.Size = new System.Drawing.Size(225, 20);
            this.FacilityTextBox.TabIndex = 3;
            // 
            // FacilityDescriptionLabel
            // 
            this.FacilityDescriptionLabel.AutoSize = true;
            this.FacilityDescriptionLabel.Location = new System.Drawing.Point(14, 121);
            this.FacilityDescriptionLabel.Name = "FacilityDescriptionLabel";
            this.FacilityDescriptionLabel.Size = new System.Drawing.Size(93, 13);
            this.FacilityDescriptionLabel.TabIndex = 4;
            this.FacilityDescriptionLabel.Text = "Facility description";
            // 
            // FacilityDescriptionTextBox
            // 
            this.FacilityDescriptionTextBox.Location = new System.Drawing.Point(113, 118);
            this.FacilityDescriptionTextBox.Name = "FacilityDescriptionTextBox";
            this.FacilityDescriptionTextBox.Size = new System.Drawing.Size(225, 20);
            this.FacilityDescriptionTextBox.TabIndex = 5;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(383, 12);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 6;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(383, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 7;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(383, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 8;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDFacilityAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 157);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.FacilityDescriptionTextBox);
            this.Controls.Add(this.FacilityDescriptionLabel);
            this.Controls.Add(this.FacilityTextBox);
            this.Controls.Add(this.FacilityLabel);
            this.Controls.Add(this.CurrentFacilitiesComboBox);
            this.Controls.Add(this.CurrentFacilitiesLabel);
            this.Name = "IDDFacilityAdd";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add a New Facility";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentFacilitiesLabel;
        private System.Windows.Forms.ComboBox CurrentFacilitiesComboBox;
        private System.Windows.Forms.Label FacilityLabel;
        private System.Windows.Forms.TextBox FacilityTextBox;
        private System.Windows.Forms.Label FacilityDescriptionLabel;
        private System.Windows.Forms.TextBox FacilityDescriptionTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}