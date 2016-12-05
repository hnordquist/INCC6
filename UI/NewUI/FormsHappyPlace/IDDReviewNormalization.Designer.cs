namespace NewUI
{
    partial class IDDReviewNormalization
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
            this.OptionalResultsGroupBox = new System.Windows.Forms.GroupBox();
            this.IndividualCycleMultiplicityDistributionsCheckBox = new System.Windows.Forms.CheckBox();
            this.SummedMultiplicityDistributionsCheckBox = new System.Windows.Forms.CheckBox();
            this.SummedRawCoincidenceDataCheckBox = new System.Windows.Forms.CheckBox();
            this.IndividualCycleRateDataCheckBox = new System.Windows.Forms.CheckBox();
            this.IndividualCycleRawDataCheckBox = new System.Windows.Forms.CheckBox();
            this.DetectorParametersCheckBox = new System.Windows.Forms.CheckBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.PlotSinglesDoublesTriplesRadioButton = new System.Windows.Forms.RadioButton();
            this.DisplayResultsInTextRadioButton = new System.Windows.Forms.RadioButton();
            this.PrintTextCheckBox = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.OptionalResultsGroupBox.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // OptionalResultsGroupBox
            // 
            this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleMultiplicityDistributionsCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.SummedMultiplicityDistributionsCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.SummedRawCoincidenceDataCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleRateDataCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.IndividualCycleRawDataCheckBox);
            this.OptionalResultsGroupBox.Controls.Add(this.DetectorParametersCheckBox);
            this.OptionalResultsGroupBox.Location = new System.Drawing.Point(16, 12);
            this.OptionalResultsGroupBox.Name = "OptionalResultsGroupBox";
            this.OptionalResultsGroupBox.Size = new System.Drawing.Size(243, 175);
            this.OptionalResultsGroupBox.TabIndex = 0;
            this.OptionalResultsGroupBox.TabStop = false;
            this.OptionalResultsGroupBox.Text = "Optional results to display";
            // 
            // IndividualCycleMultiplicityDistributionsCheckBox
            // 
            this.IndividualCycleMultiplicityDistributionsCheckBox.AutoSize = true;
            this.IndividualCycleMultiplicityDistributionsCheckBox.Location = new System.Drawing.Point(15, 144);
            this.IndividualCycleMultiplicityDistributionsCheckBox.Name = "IndividualCycleMultiplicityDistributionsCheckBox";
            this.IndividualCycleMultiplicityDistributionsCheckBox.Size = new System.Drawing.Size(207, 17);
            this.IndividualCycleMultiplicityDistributionsCheckBox.TabIndex = 5;
            this.IndividualCycleMultiplicityDistributionsCheckBox.Text = "Individual cycle multiplicity distributions";
            this.toolTip1.SetToolTip(this.IndividualCycleMultiplicityDistributionsCheckBox, "Check this box to include individual cycle multiplicity distributions for reals p" +
        "lus accidentals and accidentals with the displayed results.");
            this.IndividualCycleMultiplicityDistributionsCheckBox.UseVisualStyleBackColor = true;
            this.IndividualCycleMultiplicityDistributionsCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleMultiplicityDistributionsCheckBox_CheckedChanged);
            // 
            // SummedMultiplicityDistributionsCheckBox
            // 
            this.SummedMultiplicityDistributionsCheckBox.AutoSize = true;
            this.SummedMultiplicityDistributionsCheckBox.Location = new System.Drawing.Point(15, 121);
            this.SummedMultiplicityDistributionsCheckBox.Name = "SummedMultiplicityDistributionsCheckBox";
            this.SummedMultiplicityDistributionsCheckBox.Size = new System.Drawing.Size(175, 17);
            this.SummedMultiplicityDistributionsCheckBox.TabIndex = 4;
            this.SummedMultiplicityDistributionsCheckBox.Text = "Summed multiplicity distributions";
            this.toolTip1.SetToolTip(this.SummedMultiplicityDistributionsCheckBox, "Check this box to include summed multiplicity distributions for reals plus accide" +
        "ntals and accidentals with the displayed results.");
            this.SummedMultiplicityDistributionsCheckBox.UseVisualStyleBackColor = true;
            this.SummedMultiplicityDistributionsCheckBox.CheckedChanged += new System.EventHandler(this.SummedMultiplicityDistributionsCheckBox_CheckedChanged);
            // 
            // SummedRawCoincidenceDataCheckBox
            // 
            this.SummedRawCoincidenceDataCheckBox.AutoSize = true;
            this.SummedRawCoincidenceDataCheckBox.Location = new System.Drawing.Point(15, 98);
            this.SummedRawCoincidenceDataCheckBox.Name = "SummedRawCoincidenceDataCheckBox";
            this.SummedRawCoincidenceDataCheckBox.Size = new System.Drawing.Size(172, 17);
            this.SummedRawCoincidenceDataCheckBox.TabIndex = 3;
            this.SummedRawCoincidenceDataCheckBox.Text = "Summed raw coincidence data";
            this.toolTip1.SetToolTip(this.SummedRawCoincidenceDataCheckBox, "Check this box to include summed shift register singles, reals plus accidentals, " +
        "accidentals and scalers with the displayed results.");
            this.SummedRawCoincidenceDataCheckBox.UseVisualStyleBackColor = true;
            this.SummedRawCoincidenceDataCheckBox.CheckedChanged += new System.EventHandler(this.SummedRawCoincidenceDataCheckBox_CheckedChanged);
            // 
            // IndividualCycleRateDataCheckBox
            // 
            this.IndividualCycleRateDataCheckBox.AutoSize = true;
            this.IndividualCycleRateDataCheckBox.Location = new System.Drawing.Point(15, 75);
            this.IndividualCycleRateDataCheckBox.Name = "IndividualCycleRateDataCheckBox";
            this.IndividualCycleRateDataCheckBox.Size = new System.Drawing.Size(144, 17);
            this.IndividualCycleRateDataCheckBox.TabIndex = 2;
            this.IndividualCycleRateDataCheckBox.Text = "Individual cycle rate data";
            this.toolTip1.SetToolTip(this.IndividualCycleRateDataCheckBox, "Check this box to include individual cycle singles, doubles, triples, masses (if " +
        "applicable) and QC test status with the displayed results.");
            this.IndividualCycleRateDataCheckBox.UseVisualStyleBackColor = true;
            this.IndividualCycleRateDataCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleRateDataCheckBox_CheckedChanged);
            // 
            // IndividualCycleRawDataCheckBox
            // 
            this.IndividualCycleRawDataCheckBox.AutoSize = true;
            this.IndividualCycleRawDataCheckBox.Location = new System.Drawing.Point(15, 52);
            this.IndividualCycleRawDataCheckBox.Name = "IndividualCycleRawDataCheckBox";
            this.IndividualCycleRawDataCheckBox.Size = new System.Drawing.Size(143, 17);
            this.IndividualCycleRawDataCheckBox.TabIndex = 1;
            this.IndividualCycleRawDataCheckBox.Text = "Individual cycle raw data";
            this.toolTip1.SetToolTip(this.IndividualCycleRawDataCheckBox, "Check this box to include individual cycle singles, reals plus accidentals, accid" +
        "entals, scalers and QC test status with the displayed results.");
            this.IndividualCycleRawDataCheckBox.UseVisualStyleBackColor = true;
            this.IndividualCycleRawDataCheckBox.CheckedChanged += new System.EventHandler(this.IndividualCycleRawDataCheckBox_CheckedChanged);
            // 
            // DetectorParametersCheckBox
            // 
            this.DetectorParametersCheckBox.AutoSize = true;
            this.DetectorParametersCheckBox.Location = new System.Drawing.Point(15, 29);
            this.DetectorParametersCheckBox.Name = "DetectorParametersCheckBox";
            this.DetectorParametersCheckBox.Size = new System.Drawing.Size(122, 17);
            this.DetectorParametersCheckBox.TabIndex = 0;
            this.DetectorParametersCheckBox.Text = "Detector parameters";
            this.toolTip1.SetToolTip(this.DetectorParametersCheckBox, "Check this box to include detector and shift register parameters with the display" +
        "ed results.");
            this.DetectorParametersCheckBox.UseVisualStyleBackColor = true;
            this.DetectorParametersCheckBox.CheckedChanged += new System.EventHandler(this.DetectorParametersCheckBox_CheckedChanged);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.PlotSinglesDoublesTriplesRadioButton);
            this.GroupBox1.Controls.Add(this.DisplayResultsInTextRadioButton);
            this.GroupBox1.Location = new System.Drawing.Point(16, 216);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(243, 74);
            this.GroupBox1.TabIndex = 1;
            this.GroupBox1.TabStop = false;
            // 
            // PlotSinglesDoublesTriplesRadioButton
            // 
            this.PlotSinglesDoublesTriplesRadioButton.AutoSize = true;
            this.PlotSinglesDoublesTriplesRadioButton.Location = new System.Drawing.Point(15, 42);
            this.PlotSinglesDoublesTriplesRadioButton.Name = "PlotSinglesDoublesTriplesRadioButton";
            this.PlotSinglesDoublesTriplesRadioButton.Size = new System.Drawing.Size(175, 17);
            this.PlotSinglesDoublesTriplesRadioButton.TabIndex = 1;
            this.PlotSinglesDoublesTriplesRadioButton.TabStop = true;
            this.PlotSinglesDoublesTriplesRadioButton.Text = "Plot singles, doubles, and triples";
            this.toolTip1.SetToolTip(this.PlotSinglesDoublesTriplesRadioButton, "Select this radio button if you want a graphical display of a measurement\'s singl" +
        "es, doubles and triples.");
            this.PlotSinglesDoublesTriplesRadioButton.UseVisualStyleBackColor = true;
            this.PlotSinglesDoublesTriplesRadioButton.CheckedChanged += new System.EventHandler(this.PlotSinglesDoublesTriplesRadioButton_CheckedChanged);
            // 
            // DisplayResultsInTextRadioButton
            // 
            this.DisplayResultsInTextRadioButton.AutoSize = true;
            this.DisplayResultsInTextRadioButton.Location = new System.Drawing.Point(15, 19);
            this.DisplayResultsInTextRadioButton.Name = "DisplayResultsInTextRadioButton";
            this.DisplayResultsInTextRadioButton.Size = new System.Drawing.Size(123, 17);
            this.DisplayResultsInTextRadioButton.TabIndex = 0;
            this.DisplayResultsInTextRadioButton.TabStop = true;
            this.DisplayResultsInTextRadioButton.Text = "Display results in text";
            this.toolTip1.SetToolTip(this.DisplayResultsInTextRadioButton, "Select this radio button if you want a text display of measurements. Use the chec" +
        "k boxes to select which results are displayed.");
            this.DisplayResultsInTextRadioButton.UseVisualStyleBackColor = true;
            this.DisplayResultsInTextRadioButton.CheckedChanged += new System.EventHandler(this.DisplayResultsInTextRadioButton_CheckedChanged);
            // 
            // PrintTextCheckBox
            // 
            this.PrintTextCheckBox.AutoSize = true;
            this.PrintTextCheckBox.Location = new System.Drawing.Point(31, 193);
            this.PrintTextCheckBox.Name = "PrintTextCheckBox";
            this.PrintTextCheckBox.Size = new System.Drawing.Size(67, 17);
            this.PrintTextCheckBox.TabIndex = 2;
            this.PrintTextCheckBox.Text = "Print text";
            this.PrintTextCheckBox.UseVisualStyleBackColor = true;
            this.PrintTextCheckBox.CheckedChanged += new System.EventHandler(this.PrintTextCheckBox_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(265, 19);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(265, 48);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(265, 77);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 5;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IDDReviewNormalization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 304);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintTextCheckBox);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.OptionalResultsGroupBox);
            this.Name = "IDDReviewNormalization";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Normalization Measurement Report";
            this.Load += new System.EventHandler(this.IDDReviewNormalization_Load);
            this.OptionalResultsGroupBox.ResumeLayout(false);
            this.OptionalResultsGroupBox.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox OptionalResultsGroupBox;
        private System.Windows.Forms.CheckBox IndividualCycleMultiplicityDistributionsCheckBox;
        private System.Windows.Forms.CheckBox SummedMultiplicityDistributionsCheckBox;
        private System.Windows.Forms.CheckBox SummedRawCoincidenceDataCheckBox;
        private System.Windows.Forms.CheckBox IndividualCycleRateDataCheckBox;
        private System.Windows.Forms.CheckBox IndividualCycleRawDataCheckBox;
        private System.Windows.Forms.CheckBox DetectorParametersCheckBox;
        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.RadioButton PlotSinglesDoublesTriplesRadioButton;
        private System.Windows.Forms.RadioButton DisplayResultsInTextRadioButton;
        private System.Windows.Forms.CheckBox PrintTextCheckBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}