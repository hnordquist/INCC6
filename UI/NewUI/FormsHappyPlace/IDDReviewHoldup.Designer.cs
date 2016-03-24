namespace NewUI
{
    partial class IDDReviewHoldup
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
			this.InspectionNumberLabel = new System.Windows.Forms.Label();
			this.InspectionNumberComboBox = new System.Windows.Forms.ComboBox();
			this.OptionalResultsGroupBox = new System.Windows.Forms.GroupBox();
			this.IndividualCycleRateDataCheckBox = new System.Windows.Forms.CheckBox();
			this.IndividualCycleRawDataCheckBox = new System.Windows.Forms.CheckBox();
			this.IsotopicsCheckBox = new System.Windows.Forms.CheckBox();
			this.CalibrationParametersCheckBox = new System.Windows.Forms.CheckBox();
			this.DetectorParametersCheckBox = new System.Windows.Forms.CheckBox();
			this.PrintCheckBox = new System.Windows.Forms.CheckBox();
			this.OKBtn = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.OptionalResultsGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// InspectionNumberLabel
			// 
			this.InspectionNumberLabel.AutoSize = true;
			this.InspectionNumberLabel.Location = new System.Drawing.Point(28, 28);
			this.InspectionNumberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.InspectionNumberLabel.Name = "InspectionNumberLabel";
			this.InspectionNumberLabel.Size = new System.Drawing.Size(124, 17);
			this.InspectionNumberLabel.TabIndex = 0;
			this.InspectionNumberLabel.Text = "Inspection number";
			// 
			// InspectionNumberComboBox
			// 
			this.InspectionNumberComboBox.Enabled = false;
			this.InspectionNumberComboBox.FormattingEnabled = true;
			this.InspectionNumberComboBox.Location = new System.Drawing.Point(161, 25);
			this.InspectionNumberComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.InspectionNumberComboBox.Name = "InspectionNumberComboBox";
			this.InspectionNumberComboBox.Size = new System.Drawing.Size(219, 24);
			this.InspectionNumberComboBox.TabIndex = 1;
			this.InspectionNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.InspectionNumberComboBox_SelectedIndexChanged);
			// 
			// OptionalResultsGroupBox
			// 
			this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleRateDataCheckBox);
			this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleRawDataCheckBox);
			this.OptionalResultsGroupBox.Controls.Add(this.IsotopicsCheckBox);
			this.OptionalResultsGroupBox.Controls.Add(this.CalibrationParametersCheckBox);
			this.OptionalResultsGroupBox.Controls.Add(this.DetectorParametersCheckBox);
			this.OptionalResultsGroupBox.Location = new System.Drawing.Point(32, 70);
			this.OptionalResultsGroupBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OptionalResultsGroupBox.Name = "OptionalResultsGroupBox";
			this.OptionalResultsGroupBox.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OptionalResultsGroupBox.Size = new System.Drawing.Size(235, 188);
			this.OptionalResultsGroupBox.TabIndex = 2;
			this.OptionalResultsGroupBox.TabStop = false;
			this.OptionalResultsGroupBox.Text = "Optional results to display";
			// 
			// IndividualCycleRateDataCheckBox
			// 
			this.IndividualCycleRateDataCheckBox.AutoSize = true;
			this.IndividualCycleRateDataCheckBox.Location = new System.Drawing.Point(24, 148);
			this.IndividualCycleRateDataCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.IndividualCycleRateDataCheckBox.Name = "IndividualCycleRateDataCheckBox";
			this.IndividualCycleRateDataCheckBox.Size = new System.Drawing.Size(186, 21);
			this.IndividualCycleRateDataCheckBox.TabIndex = 4;
			this.IndividualCycleRateDataCheckBox.Text = "Individual cycle rate data";
			this.IndividualCycleRateDataCheckBox.UseVisualStyleBackColor = true;
			this.IndividualCycleRateDataCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleRateDataCheckBox_CheckedChanged);
			// 
			// IndividualCycleRawDataCheckBox
			// 
			this.IndividualCycleRawDataCheckBox.AutoSize = true;
			this.IndividualCycleRawDataCheckBox.Location = new System.Drawing.Point(24, 119);
			this.IndividualCycleRawDataCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.IndividualCycleRawDataCheckBox.Name = "IndividualCycleRawDataCheckBox";
			this.IndividualCycleRawDataCheckBox.Size = new System.Drawing.Size(183, 21);
			this.IndividualCycleRawDataCheckBox.TabIndex = 3;
			this.IndividualCycleRawDataCheckBox.Text = "Individual cycle raw data";
			this.IndividualCycleRawDataCheckBox.UseVisualStyleBackColor = true;
			this.IndividualCycleRawDataCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleRawDataCheckBox_CheckedChanged);
			// 
			// IsotopicsCheckBox
			// 
			this.IsotopicsCheckBox.AutoSize = true;
			this.IsotopicsCheckBox.Location = new System.Drawing.Point(24, 91);
			this.IsotopicsCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.IsotopicsCheckBox.Name = "IsotopicsCheckBox";
			this.IsotopicsCheckBox.Size = new System.Drawing.Size(85, 21);
			this.IsotopicsCheckBox.TabIndex = 2;
			this.IsotopicsCheckBox.Text = "Isotopics";
			this.IsotopicsCheckBox.UseVisualStyleBackColor = true;
			this.IsotopicsCheckBox.CheckedChanged += new System.EventHandler(this.IsotopicsCheckBox_CheckedChanged);
			// 
			// CalibrationParametersCheckBox
			// 
			this.CalibrationParametersCheckBox.AutoSize = true;
			this.CalibrationParametersCheckBox.Location = new System.Drawing.Point(24, 63);
			this.CalibrationParametersCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CalibrationParametersCheckBox.Name = "CalibrationParametersCheckBox";
			this.CalibrationParametersCheckBox.Size = new System.Drawing.Size(173, 21);
			this.CalibrationParametersCheckBox.TabIndex = 1;
			this.CalibrationParametersCheckBox.Text = "Calibration parameters";
			this.CalibrationParametersCheckBox.UseVisualStyleBackColor = true;
			this.CalibrationParametersCheckBox.CheckedChanged += new System.EventHandler(this.CalibrationParametersCheckBox_CheckedChanged);
			// 
			// DetectorParametersCheckBox
			// 
			this.DetectorParametersCheckBox.AutoSize = true;
			this.DetectorParametersCheckBox.Location = new System.Drawing.Point(24, 34);
			this.DetectorParametersCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.DetectorParametersCheckBox.Name = "DetectorParametersCheckBox";
			this.DetectorParametersCheckBox.Size = new System.Drawing.Size(160, 21);
			this.DetectorParametersCheckBox.TabIndex = 0;
			this.DetectorParametersCheckBox.Text = "Detector parameters";
			this.DetectorParametersCheckBox.UseVisualStyleBackColor = true;
			this.DetectorParametersCheckBox.CheckedChanged += new System.EventHandler(this.DetectorParametersCheckBox_CheckedChanged);
			// 
			// PrintCheckBox
			// 
			this.PrintCheckBox.AutoSize = true;
			this.PrintCheckBox.Location = new System.Drawing.Point(56, 282);
			this.PrintCheckBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.PrintCheckBox.Name = "PrintCheckBox";
			this.PrintCheckBox.Size = new System.Drawing.Size(59, 21);
			this.PrintCheckBox.TabIndex = 3;
			this.PrintCheckBox.Text = "Print";
			this.PrintCheckBox.UseVisualStyleBackColor = true;
			this.PrintCheckBox.CheckedChanged += new System.EventHandler(this.PrintCheckBox_CheckedChanged);
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(413, 22);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 4;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(413, 58);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 5;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(413, 94);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 6;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// IDDReviewHoldup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(536, 327);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.PrintCheckBox);
			this.Controls.Add(this.OptionalResultsGroupBox);
			this.Controls.Add(this.InspectionNumberComboBox);
			this.Controls.Add(this.InspectionNumberLabel);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "IDDReviewHoldup";
			this.Text = "Holdup Measurement Report";
			this.Load += new System.EventHandler(this.IDDReviewHoldup_Load);
			this.OptionalResultsGroupBox.ResumeLayout(false);
			this.OptionalResultsGroupBox.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InspectionNumberLabel;
        private System.Windows.Forms.ComboBox InspectionNumberComboBox;
        private System.Windows.Forms.GroupBox OptionalResultsGroupBox;
        private System.Windows.Forms.CheckBox IndividualCycleRateDataCheckBox;
        private System.Windows.Forms.CheckBox IndividualCycleRawDataCheckBox;
        private System.Windows.Forms.CheckBox IsotopicsCheckBox;
        private System.Windows.Forms.CheckBox CalibrationParametersCheckBox;
        private System.Windows.Forms.CheckBox DetectorParametersCheckBox;
        private System.Windows.Forms.CheckBox PrintCheckBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}