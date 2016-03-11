namespace NewUI
{
    partial class IDDRestoreInitialData
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
            this.DataTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.RestoreCalibrationparametersRadioButton = new System.Windows.Forms.RadioButton();
            this.RestoreDetectorParametersRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.RestoreFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.DataTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataTypeGroupBox
            // 
            this.DataTypeGroupBox.Controls.Add(this.RestoreCalibrationparametersRadioButton);
            this.DataTypeGroupBox.Controls.Add(this.RestoreDetectorParametersRadioButton);
            this.DataTypeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.DataTypeGroupBox.Name = "DataTypeGroupBox";
            this.DataTypeGroupBox.Size = new System.Drawing.Size(207, 87);
            this.DataTypeGroupBox.TabIndex = 0;
            this.DataTypeGroupBox.TabStop = false;
            this.DataTypeGroupBox.Text = "Type of Initial Data";
            // 
            // RestoreCalibrationparametersRadioButton
            // 
            this.RestoreCalibrationparametersRadioButton.AutoSize = true;
            this.RestoreCalibrationparametersRadioButton.Location = new System.Drawing.Point(17, 52);
            this.RestoreCalibrationparametersRadioButton.Name = "RestoreCalibrationparametersRadioButton";
            this.RestoreCalibrationparametersRadioButton.Size = new System.Drawing.Size(168, 17);
            this.RestoreCalibrationparametersRadioButton.TabIndex = 1;
            this.RestoreCalibrationparametersRadioButton.TabStop = true;
            this.RestoreCalibrationparametersRadioButton.Text = "Restore calibration parameters";
            this.RestoreCalibrationparametersRadioButton.UseVisualStyleBackColor = true;
            this.RestoreCalibrationparametersRadioButton.CheckedChanged += new System.EventHandler(this.RestoreCalibrationparametersRadioButton_CheckedChanged);
            // 
            // RestoreDetectorParametersRadioButton
            // 
            this.RestoreDetectorParametersRadioButton.AutoSize = true;
            this.RestoreDetectorParametersRadioButton.Location = new System.Drawing.Point(17, 29);
            this.RestoreDetectorParametersRadioButton.Name = "RestoreDetectorParametersRadioButton";
            this.RestoreDetectorParametersRadioButton.Size = new System.Drawing.Size(159, 17);
            this.RestoreDetectorParametersRadioButton.TabIndex = 0;
            this.RestoreDetectorParametersRadioButton.TabStop = true;
            this.RestoreDetectorParametersRadioButton.Text = "Restore detector parameters";
            this.RestoreDetectorParametersRadioButton.UseVisualStyleBackColor = true;
            this.RestoreDetectorParametersRadioButton.CheckedChanged += new System.EventHandler(this.RestoreDetectorParametersRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(240, 17);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(240, 46);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(240, 75);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // RestoreFileDialog
            // 
            this.RestoreFileDialog.CheckFileExists = false;
            this.RestoreFileDialog.FileName = "";
            // 
            // IDDRestoreInitialData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(332, 113);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DataTypeGroupBox);
            this.Name = "IDDRestoreInitialData";
            this.Text = "Select Type of Initial Data";
            this.DataTypeGroupBox.ResumeLayout(false);
            this.DataTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox DataTypeGroupBox;
        private System.Windows.Forms.RadioButton RestoreCalibrationparametersRadioButton;
        private System.Windows.Forms.RadioButton RestoreDetectorParametersRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.OpenFileDialog RestoreFileDialog;
    }
}