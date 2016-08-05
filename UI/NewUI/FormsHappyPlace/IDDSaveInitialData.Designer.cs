namespace NewUI
{
    partial class IDDSaveInitialData
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
            this.TypeGroupBox = new System.Windows.Forms.GroupBox();
            this.CalibrationAllRadioButton = new System.Windows.Forms.RadioButton();
            this.CalibrationCurrentRadioButton = new System.Windows.Forms.RadioButton();
            this.DetectorAllRadioButton = new System.Windows.Forms.RadioButton();
            this.DetectorCurrentRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.TypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // TypeGroupBox
            // 
            this.TypeGroupBox.Controls.Add(this.CalibrationAllRadioButton);
            this.TypeGroupBox.Controls.Add(this.CalibrationCurrentRadioButton);
            this.TypeGroupBox.Controls.Add(this.DetectorAllRadioButton);
            this.TypeGroupBox.Controls.Add(this.DetectorCurrentRadioButton);
            this.TypeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.TypeGroupBox.Name = "TypeGroupBox";
            this.TypeGroupBox.Size = new System.Drawing.Size(297, 120);
            this.TypeGroupBox.TabIndex = 0;
            this.TypeGroupBox.TabStop = false;
            this.TypeGroupBox.Text = "Type of Initial Data";
            // 
            // CalibrationAllRadioButton
            // 
            this.CalibrationAllRadioButton.AutoSize = true;
            this.CalibrationAllRadioButton.Location = new System.Drawing.Point(17, 88);
            this.CalibrationAllRadioButton.Name = "CalibrationAllRadioButton";
            this.CalibrationAllRadioButton.Size = new System.Drawing.Size(231, 17);
            this.CalibrationAllRadioButton.TabIndex = 3;
            this.CalibrationAllRadioButton.TabStop = true;
            this.CalibrationAllRadioButton.Text = "Save calibration parameters for all detectors";
            this.CalibrationAllRadioButton.UseVisualStyleBackColor = true;
            this.CalibrationAllRadioButton.CheckedChanged += new System.EventHandler(this.CalibrationAllRadioButton_CheckedChanged);
            // 
            // CalibrationCurrentRadioButton
            // 
            this.CalibrationCurrentRadioButton.AutoSize = true;
            this.CalibrationCurrentRadioButton.Location = new System.Drawing.Point(17, 65);
            this.CalibrationCurrentRadioButton.Name = "CalibrationCurrentRadioButton";
            this.CalibrationCurrentRadioButton.Size = new System.Drawing.Size(267, 17);
            this.CalibrationCurrentRadioButton.TabIndex = 2;
            this.CalibrationCurrentRadioButton.TabStop = true;
            this.CalibrationCurrentRadioButton.Text = "Save calibration parameters for the current detector";
            this.CalibrationCurrentRadioButton.UseVisualStyleBackColor = true;
            this.CalibrationCurrentRadioButton.CheckedChanged += new System.EventHandler(this.CalibrationCurrentRadioButton_CheckedChanged);
            // 
            // DetectorAllRadioButton
            // 
            this.DetectorAllRadioButton.AutoSize = true;
            this.DetectorAllRadioButton.Location = new System.Drawing.Point(17, 42);
            this.DetectorAllRadioButton.Name = "DetectorAllRadioButton";
            this.DetectorAllRadioButton.Size = new System.Drawing.Size(222, 17);
            this.DetectorAllRadioButton.TabIndex = 1;
            this.DetectorAllRadioButton.TabStop = true;
            this.DetectorAllRadioButton.Text = "Save detector parameters for all detectors";
            this.DetectorAllRadioButton.UseVisualStyleBackColor = true;
            this.DetectorAllRadioButton.CheckedChanged += new System.EventHandler(this.DetectorAllRadioButton_CheckedChanged);
            // 
            // DetectorCurrentRadioButton
            // 
            this.DetectorCurrentRadioButton.AutoSize = true;
            this.DetectorCurrentRadioButton.Location = new System.Drawing.Point(17, 19);
            this.DetectorCurrentRadioButton.Name = "DetectorCurrentRadioButton";
            this.DetectorCurrentRadioButton.Size = new System.Drawing.Size(258, 17);
            this.DetectorCurrentRadioButton.TabIndex = 0;
            this.DetectorCurrentRadioButton.TabStop = true;
            this.DetectorCurrentRadioButton.Text = "Save detector parameters for the current detector";
            this.DetectorCurrentRadioButton.UseVisualStyleBackColor = true;
            this.DetectorCurrentRadioButton.CheckedChanged += new System.EventHandler(this.DetectorCurrentRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(336, 25);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(336, 54);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(336, 83);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDSaveInitialData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 143);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.TypeGroupBox);
            this.Name = "IDDSaveInitialData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Type of Initial Data";
            this.TypeGroupBox.ResumeLayout(false);
            this.TypeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox TypeGroupBox;
        private System.Windows.Forms.RadioButton CalibrationAllRadioButton;
        private System.Windows.Forms.RadioButton CalibrationCurrentRadioButton;
        private System.Windows.Forms.RadioButton DetectorAllRadioButton;
        private System.Windows.Forms.RadioButton DetectorCurrentRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}