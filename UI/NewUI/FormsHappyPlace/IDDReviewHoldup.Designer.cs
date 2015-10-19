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
            this.CalibrationParamatersCheckBox = new System.Windows.Forms.CheckBox();
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
            this.InspectionNumberLabel.Location = new System.Drawing.Point(21, 23);
            this.InspectionNumberLabel.Name = "InspectionNumberLabel";
            this.InspectionNumberLabel.Size = new System.Drawing.Size(94, 13);
            this.InspectionNumberLabel.TabIndex = 0;
            this.InspectionNumberLabel.Text = "Inspection number";
            // 
            // InspectionNumberComboBox
            // 
            this.InspectionNumberComboBox.Enabled = false;
            this.InspectionNumberComboBox.FormattingEnabled = true;
            this.InspectionNumberComboBox.Location = new System.Drawing.Point(121, 20);
            this.InspectionNumberComboBox.Name = "InspectionNumberComboBox";
            this.InspectionNumberComboBox.Size = new System.Drawing.Size(165, 21);
            this.InspectionNumberComboBox.TabIndex = 1;
            this.InspectionNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.InspectionNumberComboBox_SelectedIndexChanged);
            // 
            // OptionalResultsGroupBox
            // 
            this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleRateDataCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleRawDataCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.IsotopicsCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.CalibrationParamatersCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.DetectorParametersCheckBox);
            this.OptionalResultsGroupBox.Location = new System.Drawing.Point(24, 57);
            this.OptionalResultsGroupBox.Name = "OptionalResultsGroupBox";
            this.OptionalResultsGroupBox.Size = new System.Drawing.Size(176, 153);
            this.OptionalResultsGroupBox.TabIndex = 2;
            this.OptionalResultsGroupBox.TabStop = false;
            this.OptionalResultsGroupBox.Text = "Optional results to display";
            // 
            // IndividualCycleRateDataCheckBox
            // 
            this.IndividualCycleRateDataCheckBox.AutoSize = true;
            this.IndividualCycleRateDataCheckBox.Enabled = false;
            this.IndividualCycleRateDataCheckBox.Location = new System.Drawing.Point(18, 120);
            this.IndividualCycleRateDataCheckBox.Name = "IndividualCycleRateDataCheckBox";
            this.IndividualCycleRateDataCheckBox.Size = new System.Drawing.Size(144, 17);
            this.IndividualCycleRateDataCheckBox.TabIndex = 4;
            this.IndividualCycleRateDataCheckBox.Text = "Individual cycle rate data";
            this.IndividualCycleRateDataCheckBox.UseVisualStyleBackColor = true;
            this.IndividualCycleRateDataCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleRateDataCheckBox_CheckedChanged);
            // 
            // IndividualCycleRawDataCheckBox
            // 
            this.IndividualCycleRawDataCheckBox.AutoSize = true;
            this.IndividualCycleRawDataCheckBox.Enabled = false;
            this.IndividualCycleRawDataCheckBox.Location = new System.Drawing.Point(18, 97);
            this.IndividualCycleRawDataCheckBox.Name = "IndividualCycleRawDataCheckBox";
            this.IndividualCycleRawDataCheckBox.Size = new System.Drawing.Size(143, 17);
            this.IndividualCycleRawDataCheckBox.TabIndex = 3;
            this.IndividualCycleRawDataCheckBox.Text = "Individual cycle raw data";
            this.IndividualCycleRawDataCheckBox.UseVisualStyleBackColor = true;
            this.IndividualCycleRawDataCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleRawDataCheckBox_CheckedChanged);
            // 
            // IsotopicsCheckBox
            // 
            this.IsotopicsCheckBox.AutoSize = true;
            this.IsotopicsCheckBox.Enabled = false;
            this.IsotopicsCheckBox.Location = new System.Drawing.Point(18, 74);
            this.IsotopicsCheckBox.Name = "IsotopicsCheckBox";
            this.IsotopicsCheckBox.Size = new System.Drawing.Size(68, 17);
            this.IsotopicsCheckBox.TabIndex = 2;
            this.IsotopicsCheckBox.Text = "Isotopics";
            this.IsotopicsCheckBox.UseVisualStyleBackColor = true;
            this.IsotopicsCheckBox.CheckedChanged += new System.EventHandler(this.IsotopicsCheckBox_CheckedChanged);
            // 
            // CalibrationParamatersCheckBox
            // 
            this.CalibrationParamatersCheckBox.AutoSize = true;
            this.CalibrationParamatersCheckBox.Enabled = false;
            this.CalibrationParamatersCheckBox.Location = new System.Drawing.Point(18, 51);
            this.CalibrationParamatersCheckBox.Name = "CalibrationParamatersCheckBox";
            this.CalibrationParamatersCheckBox.Size = new System.Drawing.Size(130, 17);
            this.CalibrationParamatersCheckBox.TabIndex = 1;
            this.CalibrationParamatersCheckBox.Text = "Calibration parameters";
            this.CalibrationParamatersCheckBox.UseVisualStyleBackColor = true;
            this.CalibrationParamatersCheckBox.CheckedChanged += new System.EventHandler(this.CalibrationParamatersCheckBox_CheckedChanged);
            // 
            // DetectorParametersCheckBox
            // 
            this.DetectorParametersCheckBox.AutoSize = true;
            this.DetectorParametersCheckBox.Enabled = false;
            this.DetectorParametersCheckBox.Location = new System.Drawing.Point(18, 28);
            this.DetectorParametersCheckBox.Name = "DetectorParametersCheckBox";
            this.DetectorParametersCheckBox.Size = new System.Drawing.Size(122, 17);
            this.DetectorParametersCheckBox.TabIndex = 0;
            this.DetectorParametersCheckBox.Text = "Detector parameters";
            this.DetectorParametersCheckBox.UseVisualStyleBackColor = true;
            this.DetectorParametersCheckBox.CheckedChanged += new System.EventHandler(this.DetectorParametersCheckBox_CheckedChanged);
            // 
            // PrintCheckBox
            // 
            this.PrintCheckBox.AutoSize = true;
            this.PrintCheckBox.Enabled = false;
            this.PrintCheckBox.Location = new System.Drawing.Point(42, 229);
            this.PrintCheckBox.Name = "PrintCheckBox";
            this.PrintCheckBox.Size = new System.Drawing.Size(47, 17);
            this.PrintCheckBox.TabIndex = 3;
            this.PrintCheckBox.Text = "Print";
            this.PrintCheckBox.UseVisualStyleBackColor = true;
            this.PrintCheckBox.CheckedChanged += new System.EventHandler(this.PrintCheckBox_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(310, 18);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 4;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(310, 47);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 5;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(310, 76);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 6;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDReviewHoldup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 266);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintCheckBox);
            this.Controls.Add(this.OptionalResultsGroupBox);
            this.Controls.Add(this.InspectionNumberComboBox);
            this.Controls.Add(this.InspectionNumberLabel);
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
        private System.Windows.Forms.CheckBox CalibrationParamatersCheckBox;
        private System.Windows.Forms.CheckBox DetectorParametersCheckBox;
        private System.Windows.Forms.CheckBox PrintCheckBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}