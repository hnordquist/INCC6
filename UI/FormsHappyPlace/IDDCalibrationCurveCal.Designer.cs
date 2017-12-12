namespace UI
{
    partial class IDDCalibrationCurveCal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDCalibrationCurveCal));
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.CurveTypeComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DoublesForMassRadioButton = new System.Windows.Forms.RadioButton();
            this.SinglesForMassRadioButton = new System.Windows.Forms.RadioButton();
            this.AnalysisTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.PassiveUraniumRadioButton = new System.Windows.Forms.RadioButton();
            this.HeavyMetalRadioButton = new System.Windows.Forms.RadioButton();
            this.ConventionalRadioButton = new System.Windows.Forms.RadioButton();
            this.ATextBox = new NumericTextBox();
            this.BTextBox = new NumericTextBox();
            this.CTextBox = new NumericTextBox();
            this.DTextBox = new NumericTextBox();
            this.VarianceATextBox = new NumericTextBox();
            this.VarianceBTextBox = new NumericTextBox();
            this.VarianceCTextBox = new NumericTextBox();
            this.VarianceDTextBox = new NumericTextBox();
            this.CovarianceABTextBox = new NumericTextBox();
            this.CovarianceACTextBox = new NumericTextBox();
            this.CovarianceADTextBox = new NumericTextBox();
            this.CovarianceBCTextBox = new NumericTextBox();
            this.CovarianceBDTextBox = new NumericTextBox(); 
            this.CovarianceCDTextBox = new NumericTextBox();
            this.SigmaXTextBox = new NumericTextBox();
            this.HvyMetalRefTextBox = new NumericTextBox();
            this.HvyMetalWeightingTextBox = new NumericTextBox();
            this.U235PercentTextBox = new NumericTextBox();
            this.LowerMassLimitTextBox = new NumericTextBox();
            this.UpperMassLimitTextBox = new NumericTextBox();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.CurveTypeLabel = new System.Windows.Forms.Label();
            this.ALabel = new System.Windows.Forms.Label();
            this.BLabel = new System.Windows.Forms.Label();
            this.CLabel = new System.Windows.Forms.Label();
            this.DLabel = new System.Windows.Forms.Label();
            this.VarianceALabel = new System.Windows.Forms.Label();
            this.VarianceBLabel = new System.Windows.Forms.Label();
            this.VarianceCLabel = new System.Windows.Forms.Label();
            this.VarianceDLabel = new System.Windows.Forms.Label();
            this.CovarianceABLabel = new System.Windows.Forms.Label();
            this.CovarianceACLabel = new System.Windows.Forms.Label();
            this.CovarianceADLabel = new System.Windows.Forms.Label();
            this.CovarianceBCLabel = new System.Windows.Forms.Label();
            this.CovarianceBDLabel = new System.Windows.Forms.Label();
            this.CovarianceCDLabel = new System.Windows.Forms.Label();
            this.SigmaXLabel = new System.Windows.Forms.Label();
            this.PassCollarMeasLabel = new System.Windows.Forms.Label();
            this.HvyMetalRefLabel = new System.Windows.Forms.Label();
            this.HvyMetalWeightingLabel = new System.Windows.Forms.Label();
            this.PassiveUraniumMeasLabel = new System.Windows.Forms.Label();
            this.U235PercentLabel = new System.Windows.Forms.Label();
            this.LowerMassLimitLabel = new System.Windows.Forms.Label();
            this.UpperMassLimitLabel = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.AnalysisTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(83, 12);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(157, 21);
            this.MaterialTypeComboBox.TabIndex = 0;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // CurveTypeComboBox
            // 
            this.CurveTypeComboBox.FormattingEnabled = true;
            this.CurveTypeComboBox.Location = new System.Drawing.Point(73, 123);
            this.CurveTypeComboBox.Name = "CurveTypeComboBox";
            this.CurveTypeComboBox.Size = new System.Drawing.Size(167, 21);
            this.CurveTypeComboBox.TabIndex = 1;
            this.CurveTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.CurveTypeComboBox_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.DoublesForMassRadioButton);
            this.groupBox1.Controls.Add(this.SinglesForMassRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 73);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // DoublesForMassRadioButton
            // 
            this.DoublesForMassRadioButton.AutoSize = true;
            this.DoublesForMassRadioButton.Location = new System.Drawing.Point(18, 42);
            this.DoublesForMassRadioButton.Name = "DoublesForMassRadioButton";
            this.DoublesForMassRadioButton.Size = new System.Drawing.Size(190, 17);
            this.DoublesForMassRadioButton.TabIndex = 1;
            this.DoublesForMassRadioButton.TabStop = true;
            this.DoublesForMassRadioButton.Text = "Use doubles rate to calculate mass";
            this.DoublesForMassRadioButton.UseVisualStyleBackColor = true;
            this.DoublesForMassRadioButton.CheckedChanged += new System.EventHandler(this.DoublesForMassRadioButton_CheckedChanged);
            // 
            // SinglesForMassRadioButton
            // 
            this.SinglesForMassRadioButton.AutoSize = true;
            this.SinglesForMassRadioButton.Location = new System.Drawing.Point(18, 19);
            this.SinglesForMassRadioButton.Name = "SinglesForMassRadioButton";
            this.SinglesForMassRadioButton.Size = new System.Drawing.Size(185, 17);
            this.SinglesForMassRadioButton.TabIndex = 0;
            this.SinglesForMassRadioButton.TabStop = true;
            this.SinglesForMassRadioButton.Text = "Use singles rate to calculate mass";
            this.SinglesForMassRadioButton.UseVisualStyleBackColor = true;
            this.SinglesForMassRadioButton.CheckedChanged += new System.EventHandler(this.SinglesForMassRadioButton_CheckedChanged);
            // 
            // AnalysisTypeGroupBox
            // 
            this.AnalysisTypeGroupBox.Controls.Add(this.PassiveUraniumRadioButton);
            this.AnalysisTypeGroupBox.Controls.Add(this.HeavyMetalRadioButton);
            this.AnalysisTypeGroupBox.Controls.Add(this.ConventionalRadioButton);
            this.AnalysisTypeGroupBox.Location = new System.Drawing.Point(259, 12);
            this.AnalysisTypeGroupBox.Name = "AnalysisTypeGroupBox";
            this.AnalysisTypeGroupBox.Size = new System.Drawing.Size(249, 100);
            this.AnalysisTypeGroupBox.TabIndex = 3;
            this.AnalysisTypeGroupBox.TabStop = false;
            this.AnalysisTypeGroupBox.Text = "Type of passive calibration curve analysis";
            // 
            // PassiveUraniumRadioButton
            // 
            this.PassiveUraniumRadioButton.AutoSize = true;
            this.PassiveUraniumRadioButton.Location = new System.Drawing.Point(22, 69);
            this.PassiveUraniumRadioButton.Name = "PassiveUraniumRadioButton";
            this.PassiveUraniumRadioButton.Size = new System.Drawing.Size(168, 17);
            this.PassiveUraniumRadioButton.TabIndex = 2;
            this.PassiveUraniumRadioButton.TabStop = true;
            this.PassiveUraniumRadioButton.Text = "Passive uranium measurement";
            this.PassiveUraniumRadioButton.UseVisualStyleBackColor = true;
            this.PassiveUraniumRadioButton.CheckedChanged += new System.EventHandler(this.PassiveUraniumRadioButton_CheckedChanged);
            // 
            // HeavyMetalRadioButton
            // 
            this.HeavyMetalRadioButton.AutoSize = true;
            this.HeavyMetalRadioButton.Location = new System.Drawing.Point(22, 46);
            this.HeavyMetalRadioButton.Name = "HeavyMetalRadioButton";
            this.HeavyMetalRadioButton.Size = new System.Drawing.Size(134, 17);
            this.HeavyMetalRadioButton.TabIndex = 1;
            this.HeavyMetalRadioButton.TabStop = true;
            this.HeavyMetalRadioButton.Text = "Heavy metal correction";
            this.HeavyMetalRadioButton.UseVisualStyleBackColor = true;
            this.HeavyMetalRadioButton.CheckedChanged += new System.EventHandler(this.HeavyMetalRadioButton_CheckedChanged);
            // 
            // ConventionalRadioButton
            // 
            this.ConventionalRadioButton.AutoSize = true;
            this.ConventionalRadioButton.Location = new System.Drawing.Point(22, 23);
            this.ConventionalRadioButton.Name = "ConventionalRadioButton";
            this.ConventionalRadioButton.Size = new System.Drawing.Size(207, 17);
            this.ConventionalRadioButton.TabIndex = 0;
            this.ConventionalRadioButton.TabStop = true;
            this.ConventionalRadioButton.Text = "Conventional passive calibration curve";
            this.ConventionalRadioButton.UseVisualStyleBackColor = true;
            this.ConventionalRadioButton.CheckedChanged += new System.EventHandler(this.ConventionalRadioButton_CheckedChanged);
            // 
            // ATextBox
            // 
            this.ATextBox.Location = new System.Drawing.Point(30, 159);
            this.ATextBox.Name = "ATextBox";
            this.ATextBox.Size = new System.Drawing.Size(100, 20);
            this.ATextBox.TabIndex = 4;
            this.ATextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.ATextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.ATextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // BTextBox
            // 
            this.BTextBox.Location = new System.Drawing.Point(30, 185);
            this.BTextBox.Name = "BTextBox";
            this.BTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTextBox.TabIndex = 5;
            this.BTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.BTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.BTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // CTextBox
            // 
            this.CTextBox.Location = new System.Drawing.Point(30, 211);
            this.CTextBox.Name = "CTextBox";
            this.CTextBox.Size = new System.Drawing.Size(100, 20);
            this.CTextBox.TabIndex = 6;
            this.CTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.CTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.CTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // DTextBox
            // 
            this.DTextBox.Location = new System.Drawing.Point(30, 237);
            this.DTextBox.Name = "DTextBox";
            this.DTextBox.Size = new System.Drawing.Size(100, 20);
            this.DTextBox.TabIndex = 7;
            this.DTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.DTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.DTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // VarianceATextBox
            // 
            this.VarianceATextBox.Location = new System.Drawing.Point(237, 159);
            this.VarianceATextBox.Name = "VarianceATextBox";
            this.VarianceATextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceATextBox.TabIndex = 8;
            this.VarianceATextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.VarianceATextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.VarianceATextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // VarianceBTextBox
            // 
            this.VarianceBTextBox.Location = new System.Drawing.Point(237, 185);
            this.VarianceBTextBox.Name = "VarianceBTextBox";
            this.VarianceBTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceBTextBox.TabIndex = 9;
            this.VarianceBTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.VarianceBTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.VarianceBTextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // VarianceCTextBox
            // 
            this.VarianceCTextBox.Location = new System.Drawing.Point(237, 211);
            this.VarianceCTextBox.Name = "VarianceCTextBox";
            this.VarianceCTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceCTextBox.TabIndex = 10;
            this.VarianceCTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.VarianceCTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.VarianceCTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // VarianceDTextBox
            // 
            this.VarianceDTextBox.Location = new System.Drawing.Point(237, 237);
            this.VarianceDTextBox.Name = "VarianceDTextBox";
            this.VarianceDTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceDTextBox.TabIndex = 11;
            this.VarianceDTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.VarianceDTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.VarianceDTextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // CovarianceABTextBox
            // 
            this.CovarianceABTextBox.Location = new System.Drawing.Point(465, 159);
            this.CovarianceABTextBox.Name = "CovarianceABTextBox";
            this.CovarianceABTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceABTextBox.TabIndex = 12;
            this.CovarianceABTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.CovarianceABTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.CovarianceABTextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // CovarianceACTextBox
            // 
            this.CovarianceACTextBox.Location = new System.Drawing.Point(465, 185);
            this.CovarianceACTextBox.Name = "CovarianceACTextBox";
            this.CovarianceACTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceACTextBox.TabIndex = 13;
            this.CovarianceACTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.CovarianceACTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.CovarianceACTextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // CovarianceADTextBox
            // 
            this.CovarianceADTextBox.Location = new System.Drawing.Point(465, 211);
            this.CovarianceADTextBox.Name = "CovarianceADTextBox";
            this.CovarianceADTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceADTextBox.TabIndex = 14;
            this.CovarianceADTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.CovarianceADTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.CovarianceADTextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // CovarianceBCTextBox
            // 
            this.CovarianceBCTextBox.Location = new System.Drawing.Point(465, 237);
            this.CovarianceBCTextBox.Name = "CovarianceBCTextBox";
            this.CovarianceBCTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceBCTextBox.TabIndex = 15;
            this.CovarianceBCTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.CovarianceBCTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.CovarianceBCTextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // CovarianceBDTextBox
            // 
            this.CovarianceBDTextBox.Location = new System.Drawing.Point(465, 263);
            this.CovarianceBDTextBox.Name = "CovarianceBDTextBox";
            this.CovarianceBDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceBDTextBox.TabIndex = 16;
            this.CovarianceBDTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.CovarianceBDTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.CovarianceBDTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // CovarianceCDTextBox
            // 
            this.CovarianceCDTextBox.Location = new System.Drawing.Point(465, 289);
            this.CovarianceCDTextBox.Name = "CovarianceCDTextBox";
            this.CovarianceCDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceCDTextBox.TabIndex = 17;
            this.CovarianceCDTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.CovarianceCDTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.CovarianceCDTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // SigmaXTextBox
            // 
            this.SigmaXTextBox.Location = new System.Drawing.Point(465, 315);
            this.SigmaXTextBox.Name = "SigmaXTextBox";
            this.SigmaXTextBox.Size = new System.Drawing.Size(100, 20);
            this.SigmaXTextBox.TabIndex = 18;
            this.SigmaXTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.SigmaXTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.SigmaXTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // HvyMetalRefTextBox
            // 
            this.HvyMetalRefTextBox.Location = new System.Drawing.Point(777, 185);
            this.HvyMetalRefTextBox.Name = "HvyMetalRefTextBox";
            this.HvyMetalRefTextBox.Size = new System.Drawing.Size(100, 20);
            this.HvyMetalRefTextBox.TabIndex = 19;
            this.toolTip1.SetToolTip(this.HvyMetalRefTextBox, resources.GetString("HvyMetalRefTextBox.ToolTip"));
            this.HvyMetalRefTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.HvyMetalRefTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.HvyMetalRefTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // HvyMetalWeightingTextBox
            // 
            this.HvyMetalWeightingTextBox.Location = new System.Drawing.Point(777, 211);
            this.HvyMetalWeightingTextBox.Name = "HvyMetalWeightingTextBox";
            this.HvyMetalWeightingTextBox.Size = new System.Drawing.Size(100, 20);
            this.HvyMetalWeightingTextBox.TabIndex = 20;
            this.toolTip1.SetToolTip(this.HvyMetalWeightingTextBox, resources.GetString("HvyMetalWeightingTextBox.ToolTip"));
            this.HvyMetalWeightingTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.HvyMetalWeightingTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.HvyMetalWeightingTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            // 
            // U235PercentTextBox
            // 
            this.U235PercentTextBox.Location = new System.Drawing.Point(777, 315);
            this.U235PercentTextBox.Name = "U235PercentTextBox";
            this.U235PercentTextBox.Size = new System.Drawing.Size(100, 20);
            this.U235PercentTextBox.TabIndex = 21;
            this.toolTip1.SetToolTip(this.U235PercentTextBox, resources.GetString("U235PercentTextBox.ToolTip"));
            this.U235PercentTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.U235PercentTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.U235PercentTextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // LowerMassLimitTextBox
            // 
            this.LowerMassLimitTextBox.Location = new System.Drawing.Point(677, 62);
            this.LowerMassLimitTextBox.Name = "LowerMassLimitTextBox";
            this.LowerMassLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.LowerMassLimitTextBox.TabIndex = 22;
            this.LowerMassLimitTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.LowerMassLimitTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.LowerMassLimitTextBox.NumberFormat = NumericTextBox.Formatter.E3;

            // 
            // UpperMassLimitTextBox
            // 
            this.UpperMassLimitTextBox.Location = new System.Drawing.Point(677, 88);
            this.UpperMassLimitTextBox.Name = "UpperMassLimitTextBox";
            this.UpperMassLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.UpperMassLimitTextBox.TabIndex = 23;
            this.UpperMassLimitTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            this.UpperMassLimitTextBox.NumStyles = System.Globalization.NumberStyles.AllowExponent;
            this.UpperMassLimitTextBox.NumberFormat = NumericTextBox.Formatter.E3;


            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(614, 15);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(163, 23);
            this.PrintBtn.TabIndex = 24;
            this.PrintBtn.Text = "Print calibration parameters";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(802, 15);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 25;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(802, 44);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 26;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(802, 73);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 27;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(10, 15);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 28;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // CurveTypeLabel
            // 
            this.CurveTypeLabel.AutoSize = true;
            this.CurveTypeLabel.Location = new System.Drawing.Point(9, 126);
            this.CurveTypeLabel.Name = "CurveTypeLabel";
            this.CurveTypeLabel.Size = new System.Drawing.Size(58, 13);
            this.CurveTypeLabel.TabIndex = 29;
            this.CurveTypeLabel.Text = "Curve type";
            // 
            // ALabel
            // 
            this.ALabel.AutoSize = true;
            this.ALabel.Location = new System.Drawing.Point(11, 162);
            this.ALabel.Name = "ALabel";
            this.ALabel.Size = new System.Drawing.Size(13, 13);
            this.ALabel.TabIndex = 30;
            this.ALabel.Text = "a";
            // 
            // BLabel
            // 
            this.BLabel.AutoSize = true;
            this.BLabel.Location = new System.Drawing.Point(11, 188);
            this.BLabel.Name = "BLabel";
            this.BLabel.Size = new System.Drawing.Size(13, 13);
            this.BLabel.TabIndex = 31;
            this.BLabel.Text = "b";
            // 
            // CLabel
            // 
            this.CLabel.AutoSize = true;
            this.CLabel.Location = new System.Drawing.Point(11, 214);
            this.CLabel.Name = "CLabel";
            this.CLabel.Size = new System.Drawing.Size(13, 13);
            this.CLabel.TabIndex = 32;
            this.CLabel.Text = "c";
            // 
            // DLabel
            // 
            this.DLabel.AutoSize = true;
            this.DLabel.Location = new System.Drawing.Point(11, 240);
            this.DLabel.Name = "DLabel";
            this.DLabel.Size = new System.Drawing.Size(13, 13);
            this.DLabel.TabIndex = 33;
            this.DLabel.Text = "d";
            // 
            // VarianceALabel
            // 
            this.VarianceALabel.AutoSize = true;
            this.VarianceALabel.Location = new System.Drawing.Point(173, 162);
            this.VarianceALabel.Name = "VarianceALabel";
            this.VarianceALabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceALabel.TabIndex = 34;
            this.VarianceALabel.Text = "Variance a";
            // 
            // VarianceBLabel
            // 
            this.VarianceBLabel.AutoSize = true;
            this.VarianceBLabel.Location = new System.Drawing.Point(173, 188);
            this.VarianceBLabel.Name = "VarianceBLabel";
            this.VarianceBLabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceBLabel.TabIndex = 35;
            this.VarianceBLabel.Text = "Variance b";
            // 
            // VarianceCLabel
            // 
            this.VarianceCLabel.AutoSize = true;
            this.VarianceCLabel.Location = new System.Drawing.Point(173, 214);
            this.VarianceCLabel.Name = "VarianceCLabel";
            this.VarianceCLabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceCLabel.TabIndex = 36;
            this.VarianceCLabel.Text = "Variance c";
            // 
            // VarianceDLabel
            // 
            this.VarianceDLabel.AutoSize = true;
            this.VarianceDLabel.Location = new System.Drawing.Point(173, 239);
            this.VarianceDLabel.Name = "VarianceDLabel";
            this.VarianceDLabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceDLabel.TabIndex = 37;
            this.VarianceDLabel.Text = "Variance d";
            // 
            // CovarianceABLabel
            // 
            this.CovarianceABLabel.AutoSize = true;
            this.CovarianceABLabel.Location = new System.Drawing.Point(383, 162);
            this.CovarianceABLabel.Name = "CovarianceABLabel";
            this.CovarianceABLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceABLabel.TabIndex = 38;
            this.CovarianceABLabel.Text = "Covariance ab";
            // 
            // CovarianceACLabel
            // 
            this.CovarianceACLabel.AutoSize = true;
            this.CovarianceACLabel.Location = new System.Drawing.Point(383, 188);
            this.CovarianceACLabel.Name = "CovarianceACLabel";
            this.CovarianceACLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceACLabel.TabIndex = 39;
            this.CovarianceACLabel.Text = "Covariance ac";
            // 
            // CovarianceADLabel
            // 
            this.CovarianceADLabel.AutoSize = true;
            this.CovarianceADLabel.Location = new System.Drawing.Point(383, 214);
            this.CovarianceADLabel.Name = "CovarianceADLabel";
            this.CovarianceADLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceADLabel.TabIndex = 40;
            this.CovarianceADLabel.Text = "Covariance ad";
            // 
            // CovarianceBCLabel
            // 
            this.CovarianceBCLabel.AutoSize = true;
            this.CovarianceBCLabel.Location = new System.Drawing.Point(383, 240);
            this.CovarianceBCLabel.Name = "CovarianceBCLabel";
            this.CovarianceBCLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceBCLabel.TabIndex = 41;
            this.CovarianceBCLabel.Text = "Covariance bc";
            // 
            // CovarianceBDLabel
            // 
            this.CovarianceBDLabel.AutoSize = true;
            this.CovarianceBDLabel.Location = new System.Drawing.Point(383, 266);
            this.CovarianceBDLabel.Name = "CovarianceBDLabel";
            this.CovarianceBDLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceBDLabel.TabIndex = 42;
            this.CovarianceBDLabel.Text = "Covariance bd";
            // 
            // CovarianceCDLabel
            // 
            this.CovarianceCDLabel.AutoSize = true;
            this.CovarianceCDLabel.Location = new System.Drawing.Point(383, 292);
            this.CovarianceCDLabel.Name = "CovarianceCDLabel";
            this.CovarianceCDLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceCDLabel.TabIndex = 43;
            this.CovarianceCDLabel.Text = "Covariance cd";
            // 
            // SigmaXLabel
            // 
            this.SigmaXLabel.AutoSize = true;
            this.SigmaXLabel.Location = new System.Drawing.Point(398, 318);
            this.SigmaXLabel.Name = "SigmaXLabel";
            this.SigmaXLabel.Size = new System.Drawing.Size(61, 13);
            this.SigmaXLabel.TabIndex = 44;
            this.SigmaXLabel.Text = "Sigma x (%)";
            // 
            // PassCollarMeasLabel
            // 
            this.PassCollarMeasLabel.AutoSize = true;
            this.PassCollarMeasLabel.Location = new System.Drawing.Point(739, 162);
            this.PassCollarMeasLabel.Name = "PassCollarMeasLabel";
            this.PassCollarMeasLabel.Size = new System.Drawing.Size(138, 13);
            this.PassCollarMeasLabel.TabIndex = 45;
            this.PassCollarMeasLabel.Text = "Passive collar measurement";
            // 
            // HvyMetalRefLabel
            // 
            this.HvyMetalRefLabel.AutoSize = true;
            this.HvyMetalRefLabel.Location = new System.Drawing.Point(623, 188);
            this.HvyMetalRefLabel.Name = "HvyMetalRefLabel";
            this.HvyMetalRefLabel.Size = new System.Drawing.Size(148, 13);
            this.HvyMetalRefLabel.TabIndex = 46;
            this.HvyMetalRefLabel.Text = "Heavy metal reference (g/cm)";
            // 
            // HvyMetalWeightingLabel
            // 
            this.HvyMetalWeightingLabel.AutoSize = true;
            this.HvyMetalWeightingLabel.Location = new System.Drawing.Point(627, 214);
            this.HvyMetalWeightingLabel.Name = "HvyMetalWeightingLabel";
            this.HvyMetalWeightingLabel.Size = new System.Drawing.Size(144, 13);
            this.HvyMetalWeightingLabel.TabIndex = 47;
            this.HvyMetalWeightingLabel.Text = "Heavy metal weighting factor";
            // 
            // PassiveUraniumMeasLabel
            // 
            this.PassiveUraniumMeasLabel.AutoSize = true;
            this.PassiveUraniumMeasLabel.Location = new System.Drawing.Point(732, 292);
            this.PassiveUraniumMeasLabel.Name = "PassiveUraniumMeasLabel";
            this.PassiveUraniumMeasLabel.Size = new System.Drawing.Size(150, 13);
            this.PassiveUraniumMeasLabel.TabIndex = 48;
            this.PassiveUraniumMeasLabel.Text = "Passive uranium measurement";
            // 
            // U235PercentLabel
            // 
            this.U235PercentLabel.AutoSize = true;
            this.U235PercentLabel.Location = new System.Drawing.Point(727, 318);
            this.U235PercentLabel.Name = "U235PercentLabel";
            this.U235PercentLabel.Size = new System.Drawing.Size(44, 13);
            this.U235PercentLabel.TabIndex = 49;
            this.U235PercentLabel.Text = "% U235";
            // 
            // LowerMassLimitLabel
            // 
            this.LowerMassLimitLabel.AutoSize = true;
            this.LowerMassLimitLabel.Location = new System.Drawing.Point(525, 65);
            this.LowerMassLimitLabel.Name = "LowerMassLimitLabel";
            this.LowerMassLimitLabel.Size = new System.Drawing.Size(138, 13);
            this.LowerMassLimitLabel.TabIndex = 50;
            this.LowerMassLimitLabel.Text = "Lower Pu240e mass limit (g)";
            // 
            // UpperMassLimitLabel
            // 
            this.UpperMassLimitLabel.AutoSize = true;
            this.UpperMassLimitLabel.Location = new System.Drawing.Point(525, 91);
            this.UpperMassLimitLabel.Name = "UpperMassLimitLabel";
            this.UpperMassLimitLabel.Size = new System.Drawing.Size(138, 13);
            this.UpperMassLimitLabel.TabIndex = 51;
            this.UpperMassLimitLabel.Text = "Upper Pu240e mass limit (g)";
            // 
            // IDDCalibrationCurveCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(894, 354);
            this.Controls.Add(this.UpperMassLimitLabel);
            this.Controls.Add(this.LowerMassLimitLabel);
            this.Controls.Add(this.U235PercentLabel);
            this.Controls.Add(this.PassiveUraniumMeasLabel);
            this.Controls.Add(this.HvyMetalWeightingLabel);
            this.Controls.Add(this.HvyMetalRefLabel);
            this.Controls.Add(this.PassCollarMeasLabel);
            this.Controls.Add(this.SigmaXLabel);
            this.Controls.Add(this.CovarianceCDLabel);
            this.Controls.Add(this.CovarianceBDLabel);
            this.Controls.Add(this.CovarianceBCLabel);
            this.Controls.Add(this.CovarianceADLabel);
            this.Controls.Add(this.CovarianceACLabel);
            this.Controls.Add(this.CovarianceABLabel);
            this.Controls.Add(this.VarianceDLabel);
            this.Controls.Add(this.VarianceCLabel);
            this.Controls.Add(this.VarianceBLabel);
            this.Controls.Add(this.VarianceALabel);
            this.Controls.Add(this.DLabel);
            this.Controls.Add(this.CLabel);
            this.Controls.Add(this.BLabel);
            this.Controls.Add(this.ALabel);
            this.Controls.Add(this.CurveTypeLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.UpperMassLimitTextBox);
            this.Controls.Add(this.LowerMassLimitTextBox);
            this.Controls.Add(this.U235PercentTextBox);
            this.Controls.Add(this.HvyMetalWeightingTextBox);
            this.Controls.Add(this.HvyMetalRefTextBox);
            this.Controls.Add(this.SigmaXTextBox);
            this.Controls.Add(this.CovarianceCDTextBox);
            this.Controls.Add(this.CovarianceBDTextBox);
            this.Controls.Add(this.CovarianceBCTextBox);
            this.Controls.Add(this.CovarianceADTextBox);
            this.Controls.Add(this.CovarianceACTextBox);
            this.Controls.Add(this.CovarianceABTextBox);
            this.Controls.Add(this.VarianceDTextBox);
            this.Controls.Add(this.VarianceCTextBox);
            this.Controls.Add(this.VarianceBTextBox);
            this.Controls.Add(this.VarianceATextBox);
            this.Controls.Add(this.DTextBox);
            this.Controls.Add(this.CTextBox);
            this.Controls.Add(this.BTextBox);
            this.Controls.Add(this.ATextBox);
            this.Controls.Add(this.AnalysisTypeGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CurveTypeComboBox);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Name = "IDDCalibrationCurveCal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Passive Calibration Curve Calibration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.AnalysisTypeGroupBox.ResumeLayout(false);
            this.AnalysisTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox CurveTypeComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton DoublesForMassRadioButton;
        private System.Windows.Forms.RadioButton SinglesForMassRadioButton;
        private System.Windows.Forms.GroupBox AnalysisTypeGroupBox;
        private System.Windows.Forms.RadioButton PassiveUraniumRadioButton;
        private System.Windows.Forms.RadioButton HeavyMetalRadioButton;
        private System.Windows.Forms.RadioButton ConventionalRadioButton;
        private NumericTextBox ATextBox;
        private NumericTextBox BTextBox;
        private NumericTextBox CTextBox;
        private NumericTextBox DTextBox;
        private NumericTextBox VarianceATextBox;
        private NumericTextBox VarianceBTextBox;
        private NumericTextBox VarianceCTextBox;
        private NumericTextBox VarianceDTextBox;
        private NumericTextBox CovarianceABTextBox;
        private NumericTextBox CovarianceACTextBox;
        private NumericTextBox CovarianceADTextBox;
        private NumericTextBox CovarianceBCTextBox;
        private NumericTextBox CovarianceBDTextBox;
        private NumericTextBox CovarianceCDTextBox;
        private NumericTextBox SigmaXTextBox;
        private NumericTextBox HvyMetalRefTextBox;
        private NumericTextBox HvyMetalWeightingTextBox;
        private NumericTextBox U235PercentTextBox;
        private NumericTextBox LowerMassLimitTextBox;
        private NumericTextBox UpperMassLimitTextBox;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label CurveTypeLabel;
        private System.Windows.Forms.Label ALabel;
        private System.Windows.Forms.Label BLabel;
        private System.Windows.Forms.Label CLabel;
        private System.Windows.Forms.Label DLabel;
        private System.Windows.Forms.Label VarianceALabel;
        private System.Windows.Forms.Label VarianceBLabel;
        private System.Windows.Forms.Label VarianceCLabel;
        private System.Windows.Forms.Label VarianceDLabel;
        private System.Windows.Forms.Label CovarianceABLabel;
        private System.Windows.Forms.Label CovarianceACLabel;
        private System.Windows.Forms.Label CovarianceADLabel;
        private System.Windows.Forms.Label CovarianceBCLabel;
        private System.Windows.Forms.Label CovarianceBDLabel;
        private System.Windows.Forms.Label CovarianceCDLabel;
        private System.Windows.Forms.Label SigmaXLabel;
        private System.Windows.Forms.Label PassCollarMeasLabel;
        private System.Windows.Forms.Label HvyMetalRefLabel;
        private System.Windows.Forms.Label HvyMetalWeightingLabel;
        private System.Windows.Forms.Label PassiveUraniumMeasLabel;
        private System.Windows.Forms.Label U235PercentLabel;
        private System.Windows.Forms.Label LowerMassLimitLabel;
        private System.Windows.Forms.Label UpperMassLimitLabel;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}