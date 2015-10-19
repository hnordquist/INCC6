namespace NewUI
{
    partial class IDDDetectorDelete
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
            this.DetectorIdLabel = new System.Windows.Forms.Label();
            this.DetectorIdComboBox = new System.Windows.Forms.ComboBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DetectorIdLabel
            // 
            this.DetectorIdLabel.AutoSize = true;
            this.DetectorIdLabel.Location = new System.Drawing.Point(11, 17);
            this.DetectorIdLabel.Name = "DetectorIdLabel";
            this.DetectorIdLabel.Size = new System.Drawing.Size(59, 13);
            this.DetectorIdLabel.TabIndex = 0;
            this.DetectorIdLabel.Text = "Detector id";
            // 
            // DetectorIdComboBox
            // 
            this.DetectorIdComboBox.FormattingEnabled = true;
            this.DetectorIdComboBox.Location = new System.Drawing.Point(76, 14);
            this.DetectorIdComboBox.Name = "DetectorIdComboBox";
            this.DetectorIdComboBox.Size = new System.Drawing.Size(151, 21);
            this.DetectorIdComboBox.TabIndex = 1;
            this.DetectorIdComboBox.SelectedIndexChanged += new System.EventHandler(this.DetectorIdComboBox_SelectedIndexChanged);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(246, 14);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(246, 43);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(246, 72);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDDetectorDelete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 116);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DetectorIdComboBox);
            this.Controls.Add(this.DetectorIdLabel);
            this.Name = "IDDDetectorDelete";
            this.Text = "Select a Detector to Delete";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label DetectorIdLabel;
        private System.Windows.Forms.ComboBox DetectorIdComboBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}