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
			this.InspectionNumberLabel.Location = new System.Drawing.Point(31, 23);
			this.InspectionNumberLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.InspectionNumberLabel.Name = "InspectionNumberLabel";
			this.InspectionNumberLabel.Size = new System.Drawing.Size(124, 17);
			this.InspectionNumberLabel.TabIndex = 0;
			this.InspectionNumberLabel.Text = "Inspection number";
			// 
			// InspectionNumberComboBox
			// 
			this.InspectionNumberComboBox.FormattingEnabled = true;
			this.InspectionNumberComboBox.Location = new System.Drawing.Point(171, 20);
			this.InspectionNumberComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.InspectionNumberComboBox.Name = "InspectionNumberComboBox";
			this.InspectionNumberComboBox.Size = new System.Drawing.Size(179, 24);
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
			this.OptionalResultsGroupBox.Location = new System.Drawing.Point(35, 71);
			this.OptionalResultsGroupBox.Margin = new System.Windows.Forms.Padding(4);
			this.OptionalResultsGroupBox.Name = "OptionalResultsGroupBox";
			this.OptionalResultsGroupBox.Padding = new System.Windows.Forms.Padding(4);
			this.OptionalResultsGroupBox.Size = new System.Drawing.Size(316, 276);
			this.OptionalResultsGroupBox.TabIndex = 2;
			this.OptionalResultsGroupBox.TabStop = false;
			this.OptionalResultsGroupBox.Text = "Optional results to display";
			// 
			// IndividualCycleMultiplicityDistributionsCheckBox
			// 
			this.IndividualCycleMultiplicityDistributionsCheckBox.AutoSize = true;
			this.IndividualCycleMultiplicityDistributionsCheckBox.Location = new System.Drawing.Point(21, 235);
			this.IndividualCycleMultiplicityDistributionsCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.IndividualCycleMultiplicityDistributionsCheckBox.Name = "IndividualCycleMultiplicityDistributionsCheckBox";
			this.IndividualCycleMultiplicityDistributionsCheckBox.Size = new System.Drawing.Size(273, 21);
			this.IndividualCycleMultiplicityDistributionsCheckBox.TabIndex = 7;
			this.IndividualCycleMultiplicityDistributionsCheckBox.Text = "Individual cycle multiplicity distributions";
			this.IndividualCycleMultiplicityDistributionsCheckBox.UseVisualStyleBackColor = true;
			this.IndividualCycleMultiplicityDistributionsCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleMultiplicityDistributionsCheckBox_CheckedChanged);
			// 
			// SummedMultiplicityDistributionsCheckBox
			// 
			this.SummedMultiplicityDistributionsCheckBox.AutoSize = true;
			this.SummedMultiplicityDistributionsCheckBox.Location = new System.Drawing.Point(21, 207);
			this.SummedMultiplicityDistributionsCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.SummedMultiplicityDistributionsCheckBox.Name = "SummedMultiplicityDistributionsCheckBox";
			this.SummedMultiplicityDistributionsCheckBox.Size = new System.Drawing.Size(233, 21);
			this.SummedMultiplicityDistributionsCheckBox.TabIndex = 6;
			this.SummedMultiplicityDistributionsCheckBox.Text = "Summed multiplicity distributions";
			this.SummedMultiplicityDistributionsCheckBox.UseVisualStyleBackColor = true;
			this.SummedMultiplicityDistributionsCheckBox.CheckedChanged += new System.EventHandler(this.SummedMultiplicityDistributionsCheckBox_CheckedChanged);
			// 
			// SummedRawCoincidenceDataCheckBox
			// 
			this.SummedRawCoincidenceDataCheckBox.AutoSize = true;
			this.SummedRawCoincidenceDataCheckBox.Location = new System.Drawing.Point(21, 178);
			this.SummedRawCoincidenceDataCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.SummedRawCoincidenceDataCheckBox.Name = "SummedRawCoincidenceDataCheckBox";
			this.SummedRawCoincidenceDataCheckBox.Size = new System.Drawing.Size(222, 21);
			this.SummedRawCoincidenceDataCheckBox.TabIndex = 5;
			this.SummedRawCoincidenceDataCheckBox.Text = "Summed raw coincidence data";
			this.SummedRawCoincidenceDataCheckBox.UseVisualStyleBackColor = true;
			this.SummedRawCoincidenceDataCheckBox.CheckedChanged += new System.EventHandler(this.SummedRawCoincidenceDataCheckBox_CheckedChanged);
			// 
			// IndividualCycleRateDateCheckBox
			// 
			this.IndividualCycleRateDateCheckBox.AutoSize = true;
			this.IndividualCycleRateDateCheckBox.Location = new System.Drawing.Point(21, 150);
			this.IndividualCycleRateDateCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.IndividualCycleRateDateCheckBox.Name = "IndividualCycleRateDateCheckBox";
			this.IndividualCycleRateDateCheckBox.Size = new System.Drawing.Size(186, 21);
			this.IndividualCycleRateDateCheckBox.TabIndex = 4;
			this.IndividualCycleRateDateCheckBox.Text = "Individual cycle rate data";
			this.IndividualCycleRateDateCheckBox.UseVisualStyleBackColor = true;
			this.IndividualCycleRateDateCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleRateDateCheckBox_CheckedChanged);
			// 
			// IndividualCycleRawDataCheckBox
			// 
			this.IndividualCycleRawDataCheckBox.AutoSize = true;
			this.IndividualCycleRawDataCheckBox.Location = new System.Drawing.Point(21, 122);
			this.IndividualCycleRawDataCheckBox.Margin = new System.Windows.Forms.Padding(4);
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
			this.IsotopicsCheckBox.Location = new System.Drawing.Point(21, 94);
			this.IsotopicsCheckBox.Margin = new System.Windows.Forms.Padding(4);
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
			this.CalibrationParametersCheckBox.Location = new System.Drawing.Point(21, 65);
			this.CalibrationParametersCheckBox.Margin = new System.Windows.Forms.Padding(4);
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
			this.DetectorParametersCheckBox.Location = new System.Drawing.Point(21, 37);
			this.DetectorParametersCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.DetectorParametersCheckBox.Name = "DetectorParametersCheckBox";
			this.DetectorParametersCheckBox.Size = new System.Drawing.Size(160, 21);
			this.DetectorParametersCheckBox.TabIndex = 0;
			this.DetectorParametersCheckBox.Text = "Detector parameters";
			this.DetectorParametersCheckBox.UseVisualStyleBackColor = true;
			this.DetectorParametersCheckBox.CheckedChanged += new System.EventHandler(this.DetectorParametersCheckBox_CheckedChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.PlotSinglesDoublesTriplesRadioButton);
			this.groupBox1.Controls.Add(this.DisplayResultsInTextRadioButton);
			this.groupBox1.Location = new System.Drawing.Point(35, 407);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox1.Size = new System.Drawing.Size(316, 96);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			// 
			// PlotSinglesDoublesTriplesRadioButton
			// 
			this.PlotSinglesDoublesTriplesRadioButton.AutoSize = true;
			this.PlotSinglesDoublesTriplesRadioButton.Enabled = false;
			this.PlotSinglesDoublesTriplesRadioButton.Location = new System.Drawing.Point(21, 52);
			this.PlotSinglesDoublesTriplesRadioButton.Margin = new System.Windows.Forms.Padding(4);
			this.PlotSinglesDoublesTriplesRadioButton.Name = "PlotSinglesDoublesTriplesRadioButton";
			this.PlotSinglesDoublesTriplesRadioButton.Size = new System.Drawing.Size(233, 21);
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
			this.DisplayResultsInTextRadioButton.Location = new System.Drawing.Point(21, 23);
			this.DisplayResultsInTextRadioButton.Margin = new System.Windows.Forms.Padding(4);
			this.DisplayResultsInTextRadioButton.Name = "DisplayResultsInTextRadioButton";
			this.DisplayResultsInTextRadioButton.Size = new System.Drawing.Size(162, 21);
			this.DisplayResultsInTextRadioButton.TabIndex = 0;
			this.DisplayResultsInTextRadioButton.TabStop = true;
			this.DisplayResultsInTextRadioButton.Text = "Display results in text";
			this.DisplayResultsInTextRadioButton.UseVisualStyleBackColor = true;
			this.DisplayResultsInTextRadioButton.CheckedChanged += new System.EventHandler(this.DisplayResultsInTextRadioButton_CheckedChanged);
			// 
			// PrintTextCheckBox
			// 
			this.PrintTextCheckBox.AutoSize = true;
			this.PrintTextCheckBox.Location = new System.Drawing.Point(56, 368);
			this.PrintTextCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.PrintTextCheckBox.Name = "PrintTextCheckBox";
			this.PrintTextCheckBox.Size = new System.Drawing.Size(85, 21);
			this.PrintTextCheckBox.TabIndex = 4;
			this.PrintTextCheckBox.Text = "Print text";
			this.PrintTextCheckBox.UseVisualStyleBackColor = true;
			this.PrintTextCheckBox.CheckedChanged += new System.EventHandler(this.PrintTextCheckBox_CheckedChanged);
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(380, 17);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 5;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(380, 53);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 6;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(380, 89);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 7;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// IDDReviewAll
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(519, 538);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKBtn);
			this.Controls.Add(this.PrintTextCheckBox);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.OptionalResultsGroupBox);
			this.Controls.Add(this.InspectionNumberComboBox);
			this.Controls.Add(this.InspectionNumberLabel);
			this.Margin = new System.Windows.Forms.Padding(4);
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