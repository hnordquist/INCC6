namespace NewUI
{
    partial class IDDSaveTransfer
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
            this.TypeOfTransferGroupBox = new System.Windows.Forms.GroupBox();
            this.AllDetectorsRadioButton = new System.Windows.Forms.RadioButton();
            this.CurrentDetectorRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.TypeOfTransferGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // TypeOfTransferGroupBox
            // 
            this.TypeOfTransferGroupBox.Controls.Add(this.AllDetectorsRadioButton);
            this.TypeOfTransferGroupBox.Controls.Add(this.CurrentDetectorRadioButton);
            this.TypeOfTransferGroupBox.Location = new System.Drawing.Point(12, 12);
            this.TypeOfTransferGroupBox.Name = "TypeOfTransferGroupBox";
            this.TypeOfTransferGroupBox.Size = new System.Drawing.Size(272, 71);
            this.TypeOfTransferGroupBox.TabIndex = 0;
            this.TypeOfTransferGroupBox.TabStop = false;
            this.TypeOfTransferGroupBox.Text = "Type of Transfer";
            // 
            // AllDetectorsRadioButton
            // 
            this.AllDetectorsRadioButton.AutoSize = true;
            this.AllDetectorsRadioButton.Location = new System.Drawing.Point(11, 42);
            this.AllDetectorsRadioButton.Name = "AllDetectorsRadioButton";
            this.AllDetectorsRadioButton.Size = new System.Drawing.Size(215, 17);
            this.AllDetectorsRadioButton.TabIndex = 1;
            this.AllDetectorsRadioButton.TabStop = true;
            this.AllDetectorsRadioButton.Text = "Save measurement data for all detectors";
            this.AllDetectorsRadioButton.UseVisualStyleBackColor = true;
            this.AllDetectorsRadioButton.CheckedChanged += new System.EventHandler(this.AllDetectorsRadioButton_CheckedChanged);
            // 
            // CurrentDetectorRadioButton
            // 
            this.CurrentDetectorRadioButton.AutoSize = true;
            this.CurrentDetectorRadioButton.Location = new System.Drawing.Point(11, 19);
            this.CurrentDetectorRadioButton.Name = "CurrentDetectorRadioButton";
            this.CurrentDetectorRadioButton.Size = new System.Drawing.Size(251, 17);
            this.CurrentDetectorRadioButton.TabIndex = 0;
            this.CurrentDetectorRadioButton.TabStop = true;
            this.CurrentDetectorRadioButton.Text = "Save measurement data for the current detector";
            this.CurrentDetectorRadioButton.UseVisualStyleBackColor = true;
            this.CurrentDetectorRadioButton.CheckedChanged += new System.EventHandler(this.CurrentDetectorRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(310, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(310, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(310, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDSaveTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 113);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.TypeOfTransferGroupBox);
            this.Name = "IDDSaveTransfer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Type of Transfer";
            this.TypeOfTransferGroupBox.ResumeLayout(false);
            this.TypeOfTransferGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox TypeOfTransferGroupBox;
        private System.Windows.Forms.RadioButton AllDetectorsRadioButton;
        private System.Windows.Forms.RadioButton CurrentDetectorRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}