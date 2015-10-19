namespace NewUI
{
    partial class IDDReviewAll
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
            this.IndividualCycleMultiplicityDistributionsCheckBox = new System.Windows.Forms.CheckBox();
            this.SummedMultiplicityDistributionsCheckBox = new System.Windows.Forms.CheckBox();
            this.SummedRawCoincidenceDataCheckBox = new System.Windows.Forms.CheckBox();
            this.IndividualCycleRateDateCheckBox = new System.Windows.Forms.CheckBox();
            this.IndividualCycleRawDataCheckBox = new System.Windows.Forms.CheckBox();
            this.IsotopicsCheckBox = new System.Windows.Forms.CheckBox();
            this.CalibrationParametersCheckBox = new System.Windows.Forms.CheckBox();
            this.DetectorParametersCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PlotSinglesDoublesTriplesRadioButton = new System.Windows.Forms.RadioButton();
            this.DisplayResultsInTextRadioButton = new System.Windows.Forms.RadioButton();
            this.PrintTextCheckBox = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.OptionalResultsGroupBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // InspectionNumberLabel
            // 
            this.InspectionNumberLabel.AutoSize = true;
            this.InspectionNumberLabel.Location = new System.Drawing.Point(23, 19);
            this.InspectionNumberLabel.Name = "InspectionNumberLabel";
            this.InspectionNumberLabel.Size = new System.Drawing.Size(94, 13);
            this.InspectionNumberLabel.TabIndex = 0;
            this.InspectionNumberLabel.Text = "Inspection number";
            // 
            // InspectionNumberComboBox
            // 
            this.InspectionNumberComboBox.Enabled = false;
            this.InspectionNumberComboBox.FormattingEnabled = true;
            this.InspectionNumberComboBox.Location = new System.Drawing.Point(128, 16);
            this.InspectionNumberComboBox.Name = "InspectionNumberComboBox";
            this.InspectionNumberComboBox.Size = new System.Drawing.Size(135, 21);
            this.InspectionNumberComboBox.TabIndex = 1;
            this.InspectionNumberComboBox.SelectedIndexChanged += new System.EventHandler(this.InspectionNumberComboBox_SelectedIndexChanged);
            // 
            // OptionalResultsGroupBox
            // 
            this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleMultiplicityDistributionsCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.SummedMultiplicityDistributionsCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.SummedRawCoincidenceDataCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleRateDateCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleRawDataCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.IsotopicsCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.CalibrationParametersCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.DetectorParametersCheckBox);
            this.OptionalResultsGroupBox.Location = new System.Drawing.Point(26, 58);
            this.OptionalResultsGroupBox.Name = "OptionalResultsGroupBox";
            this.OptionalResultsGroupBox.Size = new System.Drawing.Size(237, 224);
            this.OptionalResultsGroupBox.TabIndex = 2;
            this.OptionalResultsGroupBox.TabStop = false;
            this.OptionalResultsGroupBox.Text = "Optional results to display";
            // 
            // IndividualCycleMultiplicityDistributionsCheckBox
            // 
            this.IndividualCycleMultiplicityDistributionsCheckBox.AutoSize = true;
            this.IndividualCycleMultiplicityDistributionsCheckBox.Enabled = false;
            this.IndividualCycleMultiplicityDistributionsCheckBox.Location = new System.Drawing.Point(16, 191);
            this.IndividualCycleMultiplicityDistributionsCheckBox.Name = "IndividualCycleMultiplicityDistributionsCheckBox";
            this.IndividualCycleMultiplicityDistributionsCheckBox.Size = new System.Drawing.Size(207, 17);
            this.IndividualCycleMultiplicityDistributionsCheckBox.TabIndex = 7;
            this.IndividualCycleMultiplicityDistributionsCheckBox.Text = "Individual cycle multiplicity distributions";
            this.IndividualCycleMultiplicityDistributionsCheckBox.UseVisualStyleBackColor = true;
            this.IndividualCycleMultiplicityDistributionsCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleMultiplicityDistributionsCheckBox_CheckedChanged);
            // 
            // SummedMultiplicityDistributionsCheckBox
            // 
            this.SummedMultiplicityDistributionsCheckBox.AutoSize = true;
            this.SummedMultiplicityDistributionsCheckBox.Enabled = false;
            this.SummedMultiplicityDistributionsCheckBox.Location = new System.Drawing.Point(16, 168);
            this.SummedMultiplicityDistributionsCheckBox.Name = "SummedMultiplicityDistributionsCheckBox";
            this.SummedMultiplicityDistributionsCheckBox.Size = new System.Drawing.Size(175, 17);
            this.SummedMultiplicityDistributionsCheckBox.TabIndex = 6;
            this.SummedMultiplicityDistributionsCheckBox.Text = "Summed multiplicity distributions";
            this.SummedMultiplicityDistributionsCheckBox.UseVisualStyleBackColor = true;
            this.SummedMultiplicityDistributionsCheckBox.CheckedChanged += new System.EventHandler(this.SummedMultiplicityDistributionsCheckBox_CheckedChanged);
            // 
            // SummedRawCoincidenceDataCheckBox
            // 
            this.SummedRawCoincidenceDataCheckBox.AutoSize = true;
            this.SummedRawCoincidenceDataCheckBox.Enabled = false;
            this.SummedRawCoincidenceDataCheckBox.Location = new System.Drawing.Point(16, 145);
            this.SummedRawCoincidenceDataCheckBox.Name = "SummedRawCoincidenceDataCheckBox";
            this.SummedRawCoincidenceDataCheckBox.Size = new System.Drawing.Size(172, 17);
            this.SummedRawCoincidenceDataCheckBox.TabIndex = 5;
            this.SummedRawCoincidenceDataCheckBox.Text = "Summed raw coincidence data";
            this.SummedRawCoincidenceDataCheckBox.UseVisualStyleBackColor = true;
            this.SummedRawCoincidenceDataCheckBox.CheckedChanged += new System.EventHandler(this.SummedRawCoincidenceDataCheckBox_CheckedChanged);
            // 
            // IndividualCycleRateDateCheckBox
            // 
            this.IndividualCycleRateDateCheckBox.AutoSize = true;
            this.IndividualCycleRateDateCheckBox.Enabled = false;
            this.IndividualCycleRateDateCheckBox.Location = new System.Drawing.Point(16, 122);
            this.IndividualCycleRateDateCheckBox.Name = "IndividualCycleRateDateCheckBox";
            this.IndividualCycleRateDateCheckBox.Size = new System.Drawing.Size(144, 17);
            this.IndividualCycleRateDateCheckBox.TabIndex = 4;
            this.IndividualCycleRateDateCheckBox.Text = "Individual cycle rate data";
            this.IndividualCycleRateDateCheckBox.UseVisualStyleBackColor = true;
            this.IndividualCycleRateDateCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleRateDateCheckBox_CheckedChanged);
            // 
            // IndividualCycleRawDataCheckBox
            // 
            this.IndividualCycleRawDataCheckBox.AutoSize = true;
            this.IndividualCycleRawDataCheckBox.Enabled = false;
            this.IndividualCycleRawDataCheckBox.Location = new System.Drawing.Point(16, 99);
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
            this.IsotopicsCheckBox.Location = new System.Drawing.Point(16, 76);
            this.IsotopicsCheckBox.Name = "IsotopicsCheckBox";
            this.IsotopicsCheckBox.Size = new System.Drawing.Size(68, 17);
            this.IsotopicsCheckBox.TabIndex = 2;
            this.IsotopicsCheckBox.Text = "Isotopics";
            this.IsotopicsCheckBox.UseVisualStyleBackColor = true;
            this.IsotopicsCheckBox.CheckedChanged += new System.EventHandler(this.IsotopicsCheckBox_CheckedChanged);
            // 
            // CalibrationParametersCheckBox
            // 
            this.CalibrationParametersCheckBox.AutoSize = true;
            this.CalibrationParametersCheckBox.Enabled = false;
            this.CalibrationParametersCheckBox.Location = new System.Drawing.Point(16, 53);
            this.CalibrationParametersCheckBox.Name = "CalibrationParametersCheckBox";
            this.CalibrationParametersCheckBox.Size = new System.Drawing.Size(130, 17);
            this.CalibrationParametersCheckBox.TabIndex = 1;
            this.CalibrationParametersCheckBox.Text = "Calibration parameters";
            this.CalibrationParametersCheckBox.UseVisualStyleBackColor = true;
            this.CalibrationParametersCheckBox.CheckedChanged += new System.EventHandler(this.CalibrationParametersCheckBox_CheckedChanged);
            // 
            // DetectorParametersCheckBox
            // 
            this.DetectorParametersCheckBox.AutoSize = true;
            this.DetectorParametersCheckBox.Enabled = false;
            this.DetectorParametersCheckBox.Location = new System.Drawing.Point(16, 30);
            this.DetectorParametersCheckBox.Name = "DetectorParametersCheckBox";
            this.DetectorParametersCheckBox.Size = new System.Drawing.Size(122, 17);
            this.DetectorParametersCheckBox.TabIndex = 0;
            this.DetectorParametersCheckBox.Text = "Detector parameters";
            this.DetectorParametersCheckBox.UseVisualStyleBackColor = true;
            this.DetectorParametersCheckBox.CheckedChanged += new System.EventHandler(this.DetectorParametersCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.PlotSinglesDoublesTriplesRadioButton);
            this.groupBox1.Controls.Add(this.DisplayResultsInTextRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(26, 331);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(237, 78);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            // 
            // PlotSinglesDoublesTriplesRadioButton
            // 
            this.PlotSinglesDoublesTriplesRadioButton.AutoSize = true;
            this.PlotSinglesDoublesTriplesRadioButton.Enabled = false;
            this.PlotSinglesDoublesTriplesRadioButton.Location = new System.Drawing.Point(16, 42);
            this.PlotSinglesDoublesTriplesRadioButton.Name = "PlotSinglesDoublesTriplesRadioButton";
            this.PlotSinglesDoublesTriplesRadioButton.Size = new System.Drawing.Size(175, 17);
            this.PlotSinglesDoublesTriplesRadioButton.TabIndex = 1;
            this.PlotSinglesDoublesTriplesRadioButton.TabStop = true;
            this.PlotSinglesDoublesTriplesRadioButton.Text = "Plot singles, doubles, and triples";
            this.PlotSinglesDoublesTriplesRadioButton.UseVisualStyleBackColor = true;
            this.PlotSinglesDoublesTriplesRadioButton.CheckedChanged += new System.EventHandler(this.PlotSinglesDoublesTriplesRadioButton_CheckedChanged);
            // 
            // DisplayResultsInTextRadioButton
            // 
            this.DisplayResultsInTextRadioButton.AutoSize = true;
            this.DisplayResultsInTextRadioButton.Enabled = false;
            this.DisplayResultsInTextRadioButton.Location = new System.Drawing.Point(16, 19);
            this.DisplayResultsInTextRadioButton.Name = "DisplayResultsInTextRadioButton";
            this.DisplayResultsInTextRadioButton.Size = new System.Drawing.Size(123, 17);
            this.DisplayResultsInTextRadioButton.TabIndex = 0;
            this.DisplayResultsInTextRadioButton.TabStop = true;
            this.DisplayResultsInTextRadioButton.Text = "Display results in text";
            this.DisplayResultsInTextRadioButton.UseVisualStyleBackColor = true;
            this.DisplayResultsInTextRadioButton.CheckedChanged += new System.EventHandler(this.DisplayResultsInTextRadioButton_CheckedChanged);
            // 
            // PrintTextCheckBox
            // 
            this.PrintTextCheckBox.AutoSize = true;
            this.PrintTextCheckBox.Enabled = false;
            this.PrintTextCheckBox.Location = new System.Drawing.Point(42, 299);
            this.PrintTextCheckBox.Name = "PrintTextCheckBox";
            this.PrintTextCheckBox.Size = new System.Drawing.Size(67, 17);
            this.PrintTextCheckBox.TabIndex = 4;
            this.PrintTextCheckBox.Text = "Print text";
            this.PrintTextCheckBox.UseVisualStyleBackColor = true;
            this.PrintTextCheckBox.CheckedChanged += new System.EventHandler(this.PrintTextCheckBox_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(285, 14);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 5;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(285, 43);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 6;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(285, 72);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 7;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDReviewAll
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 437);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintTextCheckBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.OptionalResultsGroupBox);
            this.Controls.Add(this.InspectionNumberComboBox);
            this.Controls.Add(this.InspectionNumberLabel);
            this.Name = "IDDReviewAll";
            this.Text = "All Measurements Report";
            this.Load += new System.EventHandler(this.IDDReviewAll_Load);
            this.OptionalResultsGroupBox.ResumeLayout(false);
            this.OptionalResultsGroupBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InspectionNumberLabel;
        private System.Windows.Forms.ComboBox InspectionNumberComboBox;
        private System.Windows.Forms.GroupBox OptionalResultsGroupBox;
        private System.Windows.Forms.CheckBox IndividualCycleMultiplicityDistributionsCheckBox;
        private System.Windows.Forms.CheckBox SummedMultiplicityDistributionsCheckBox;
        private System.Windows.Forms.CheckBox SummedRawCoincidenceDataCheckBox;
        private System.Windows.Forms.CheckBox IndividualCycleRateDateCheckBox;
        private System.Windows.Forms.CheckBox IndividualCycleRawDataCheckBox;
        private System.Windows.Forms.CheckBox IsotopicsCheckBox;
        private System.Windows.Forms.CheckBox CalibrationParametersCheckBox;
        private System.Windows.Forms.CheckBox DetectorParametersCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton PlotSinglesDoublesTriplesRadioButton;
        private System.Windows.Forms.RadioButton DisplayResultsInTextRadioButton;
        private System.Windows.Forms.CheckBox PrintTextCheckBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
    }
}