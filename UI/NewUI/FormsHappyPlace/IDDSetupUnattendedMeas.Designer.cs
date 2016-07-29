namespace NewUI
{
    partial class IDDSetupUnattendedMeas
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDSetupUnattendedMeas));
            this.MaxTimeLabel = new System.Windows.Forms.Label();
            this.MaxTimeTextBox = new System.Windows.Forms.TextBox();
            this.AutoImportCheckBox = new System.Windows.Forms.CheckBox();
            this.DoublesThresholdLabel = new System.Windows.Forms.Label();
            this.DoublesThresholdTextBox = new System.Windows.Forms.TextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // MaxTimeLabel
            // 
            this.MaxTimeLabel.AutoSize = true;
            this.MaxTimeLabel.Location = new System.Drawing.Point(75, 15);
            this.MaxTimeLabel.Name = "MaxTimeLabel";
            this.MaxTimeLabel.Size = new System.Drawing.Size(236, 13);
            this.MaxTimeLabel.TabIndex = 0;
            this.MaxTimeLabel.Text = "Maximum time for finding a data match (seconds)";
            // 
            // MaxTimeTextBox
            // 
            this.MaxTimeTextBox.Location = new System.Drawing.Point(317, 12);
            this.MaxTimeTextBox.Name = "MaxTimeTextBox";
            this.MaxTimeTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxTimeTextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.MaxTimeTextBox, resources.GetString("MaxTimeTextBox.ToolTip"));
            this.MaxTimeTextBox.Leave += new System.EventHandler(this.MaxTimeTextBox_Leave);
            // 
            // AutoImportCheckBox
            // 
            this.AutoImportCheckBox.AutoSize = true;
            this.AutoImportCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AutoImportCheckBox.Location = new System.Drawing.Point(9, 38);
            this.AutoImportCheckBox.Name = "AutoImportCheckBox";
            this.AutoImportCheckBox.Size = new System.Drawing.Size(322, 17);
            this.AutoImportCheckBox.TabIndex = 2;
            this.AutoImportCheckBox.Text = "Automatic import (no acquire dialog box for each measurement)";
            this.toolTip1.SetToolTip(this.AutoImportCheckBox, "Check this box if, when importing unattended measurement data, you\r\nwant an acqui" +
        "re dialog box to be displayed only if item data has not\r\nalready been entered fo" +
        "r a measurement.\r\n");
            this.AutoImportCheckBox.UseVisualStyleBackColor = true;
            this.AutoImportCheckBox.CheckedChanged += new System.EventHandler(this.AutoImportCheckBox_CheckedChanged);
            // 
            // DoublesThresholdLabel
            // 
            this.DoublesThresholdLabel.AutoSize = true;
            this.DoublesThresholdLabel.Location = new System.Drawing.Point(74, 64);
            this.DoublesThresholdLabel.Name = "DoublesThresholdLabel";
            this.DoublesThresholdLabel.Size = new System.Drawing.Size(237, 13);
            this.DoublesThresholdLabel.TabIndex = 3;
            this.DoublesThresholdLabel.Text = "Add-a-source doubles threshold (counts/second)";
            // 
            // DoublesThresholdTextBox
            // 
            this.DoublesThresholdTextBox.Location = new System.Drawing.Point(317, 61);
            this.DoublesThresholdTextBox.Name = "DoublesThresholdTextBox";
            this.DoublesThresholdTextBox.Size = new System.Drawing.Size(100, 20);
            this.DoublesThresholdTextBox.TabIndex = 4;
            this.toolTip1.SetToolTip(this.DoublesThresholdTextBox, resources.GetString("DoublesThresholdTextBox.ToolTip"));
            this.DoublesThresholdTextBox.Leave += new System.EventHandler(this.DoublesThresholdTextBox_Leave);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(465, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(465, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(465, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 7;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDSetupUnattendedMeas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 102);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DoublesThresholdTextBox);
            this.Controls.Add(this.DoublesThresholdLabel);
            this.Controls.Add(this.AutoImportCheckBox);
            this.Controls.Add(this.MaxTimeTextBox);
            this.Controls.Add(this.MaxTimeLabel);
            this.Name = "IDDSetupUnattendedMeas";
            this.Text = "Unattended Measurement Parameters";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MaxTimeLabel;
        private System.Windows.Forms.TextBox MaxTimeTextBox;
        private System.Windows.Forms.CheckBox AutoImportCheckBox;
        private System.Windows.Forms.Label DoublesThresholdLabel;
        private System.Windows.Forms.TextBox DoublesThresholdTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
		private System.Windows.Forms.ToolTip toolTip1;
	}
}