namespace NewUI
{
    partial class IDDSaveCampaignId
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
            this.InspectionNumLabel = new System.Windows.Forms.Label();
            this.InspectionNumComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InspectionNumLabel
            // 
            this.InspectionNumLabel.AutoSize = true;
            this.InspectionNumLabel.Location = new System.Drawing.Point(12, 18);
            this.InspectionNumLabel.Name = "InspectionNumLabel";
            this.InspectionNumLabel.Size = new System.Drawing.Size(94, 13);
            this.InspectionNumLabel.TabIndex = 0;
            this.InspectionNumLabel.Text = "Inspection number";
            // 
            // InspectionNumComboBox
            // 
            this.InspectionNumComboBox.FormattingEnabled = true;
            this.InspectionNumComboBox.Location = new System.Drawing.Point(112, 15);
            this.InspectionNumComboBox.Name = "InspectionNumComboBox";
            this.InspectionNumComboBox.Size = new System.Drawing.Size(162, 21);
            this.InspectionNumComboBox.TabIndex = 1;
            this.InspectionNumComboBox.SelectedIndexChanged += new System.EventHandler(this.InspectionNumComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(298, 13);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(298, 42);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(298, 71);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDSaveCampaignId
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 108);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.InspectionNumComboBox);
            this.Controls.Add(this.InspectionNumLabel);
            this.Name = "IDDSaveCampaignId";
            this.Text = "Select Inspection";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InspectionNumLabel;
        private System.Windows.Forms.ComboBox InspectionNumComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}