﻿namespace UI
{
    partial class IDDKnownAlphaCal
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IDDKnownAlphaCal));
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.LowerMassLimitTextBox = new UI.NumericTextBox();
            this.UpperMassLimitTextBox = new UI.NumericTextBox();
            this.AlphaWeightTextBox = new UI.NumericTextBox();
            this.RhoZeroTextBox = new UI.NumericTextBox();
            this.KTextBox = new UI.NumericTextBox();
            this.ConventionalATextBox = new UI.NumericTextBox();
            this.ConventionalBTextBox = new UI.NumericTextBox();
            this.VarianceATextBox = new UI.NumericTextBox();
            this.VarianceBTextBox = new UI.NumericTextBox();
            this.CovarianceABTextBox = new UI.NumericTextBox();
            this.SigmaXTextBox = new UI.NumericTextBox();
            this.HvyMetalRefTextBox = new UI.NumericTextBox();
            this.HvyMetalWeightingTextBox = new UI.NumericTextBox();
            this.LowerAlphaLimitTextBox = new UI.NumericTextBox();
            this.UpperAlphaLimitTextBox = new UI.NumericTextBox();
            this.MoistureATextBox = new UI.NumericTextBox();
            this.MoistureBTextBox = new UI.NumericTextBox();
            this.MoistureCTextBox = new UI.NumericTextBox();
            this.MoistureDTextBox = new UI.NumericTextBox();
            this.CurveTypeComboBox = new System.Windows.Forms.ComboBox();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.KnownAlphaTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.MoistureToDoublesRadioButton = new System.Windows.Forms.RadioButton();
            this.MoistureToDryAlphaRadioButton = new System.Windows.Forms.RadioButton();
            this.HeavyMetalRadioButton = new System.Windows.Forms.RadioButton();
            this.ConventionalRadioButton = new System.Windows.Forms.RadioButton();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.LowerPuLimitLabel = new System.Windows.Forms.Label();
            this.UpperPuLimitLabel = new System.Windows.Forms.Label();
            this.AlphaWeightLabel = new System.Windows.Forms.Label();
            this.RhoZeroLabel = new System.Windows.Forms.Label();
            this.KLabel = new System.Windows.Forms.Label();
            this.ConventionalALabel = new System.Windows.Forms.Label();
            this.ConventionalBLabel = new System.Windows.Forms.Label();
            this.VarianceALabel = new System.Windows.Forms.Label();
            this.VarianceBLabel = new System.Windows.Forms.Label();
            this.CovarianceABLabel = new System.Windows.Forms.Label();
            this.SigmaXLabel = new System.Windows.Forms.Label();
            this.HeavyMetalReferenceLabel = new System.Windows.Forms.Label();
            this.HeavyMetalWeightingLabel = new System.Windows.Forms.Label();
            this.LowerAlphaLimitLabel = new System.Windows.Forms.Label();
            this.UpperAlphaLimitLabel = new System.Windows.Forms.Label();
            this.CurveTypeLabel = new System.Windows.Forms.Label();
            this.MoistureALabel = new System.Windows.Forms.Label();
            this.MoistureBLabel = new System.Windows.Forms.Label();
            this.MoistureCLabel = new System.Windows.Forms.Label();
            this.MoistureDLabel = new System.Windows.Forms.Label();
            this.ConventionalParametersGroupBox = new System.Windows.Forms.GroupBox();
            this.HeavyMetalCorrectionGroupBox = new System.Windows.Forms.GroupBox();
            this.MoistureCorrectionGroupBox = new System.Windows.Forms.GroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.KnownAlphaTypeGroupBox.SuspendLayout();
            this.ConventionalParametersGroupBox.SuspendLayout();
            this.HeavyMetalCorrectionGroupBox.SuspendLayout();
            this.MoistureCorrectionGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(87, 16);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(217, 21);
            this.MaterialTypeComboBox.TabIndex = 0;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // LowerMassLimitTextBox
            // 
            this.LowerMassLimitTextBox.Location = new System.Drawing.Point(169, 29);
            this.LowerMassLimitTextBox.Max = 1.7976931348623157E+308D;
            this.LowerMassLimitTextBox.Min = 0D;
            this.LowerMassLimitTextBox.Name = "LowerMassLimitTextBox";
            this.LowerMassLimitTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.LowerMassLimitTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.LowerMassLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.LowerMassLimitTextBox.Steps = -1D;
            this.LowerMassLimitTextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.LowerMassLimitTextBox, "Enter the mass for Pu240 effective or U235 below which the calibration parameters" +
        " are invalid. A calculated mass below this value will have an out of calibration" +
        " range warning message.");
            this.LowerMassLimitTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.LowerMassLimitTextBox.Value = 0D;
            // 
            // UpperMassLimitTextBox
            // 
            this.UpperMassLimitTextBox.Location = new System.Drawing.Point(169, 55);
            this.UpperMassLimitTextBox.Max = 1.7976931348623157E+308D;
            this.UpperMassLimitTextBox.Min = 0D;
            this.UpperMassLimitTextBox.Name = "UpperMassLimitTextBox";
            this.UpperMassLimitTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.UpperMassLimitTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.UpperMassLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.UpperMassLimitTextBox.Steps = -1D;
            this.UpperMassLimitTextBox.TabIndex = 2;
            this.toolTip1.SetToolTip(this.UpperMassLimitTextBox, "Enter the mass for Pu240 effective or U235 above which the calibration parameters" +
        " are invalid. A calculated mass above this value will have an out of calibration" +
        " range warning message.");
            this.UpperMassLimitTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.UpperMassLimitTextBox.Value = 0D;
            // 
            // AlphaWeightTextBox
            // 
            this.AlphaWeightTextBox.Location = new System.Drawing.Point(169, 81);
            this.AlphaWeightTextBox.Max = 1.7976931348623157E+308D;
            this.AlphaWeightTextBox.Min = 0D;
            this.AlphaWeightTextBox.Name = "AlphaWeightTextBox";
            this.AlphaWeightTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.AlphaWeightTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.AlphaWeightTextBox.Size = new System.Drawing.Size(100, 20);
            this.AlphaWeightTextBox.Steps = -1D;
            this.AlphaWeightTextBox.TabIndex = 3;
            this.toolTip1.SetToolTip(this.AlphaWeightTextBox, resources.GetString("AlphaWeightTextBox.ToolTip"));
            this.AlphaWeightTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.AlphaWeightTextBox.Value = 0D;
            // 
            // RhoZeroTextBox
            // 
            this.RhoZeroTextBox.Location = new System.Drawing.Point(169, 107);
            this.RhoZeroTextBox.Max = 1.7976931348623157E+308D;
            this.RhoZeroTextBox.Min = 0D;
            this.RhoZeroTextBox.Name = "RhoZeroTextBox";
            this.RhoZeroTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.RhoZeroTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.RhoZeroTextBox.Size = new System.Drawing.Size(100, 20);
            this.RhoZeroTextBox.Steps = -1D;
            this.RhoZeroTextBox.TabIndex = 4;
            this.toolTip1.SetToolTip(this.RhoZeroTextBox, resources.GetString("RhoZeroTextBox.ToolTip"));
            this.RhoZeroTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.RhoZeroTextBox.Value = 0D;
            // 
            // KTextBox
            // 
            this.KTextBox.Location = new System.Drawing.Point(169, 133);
            this.KTextBox.Max = 1.7976931348623157E+308D;
            this.KTextBox.Min = 0D;
            this.KTextBox.Name = "KTextBox";
            this.KTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.KTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.KTextBox.Size = new System.Drawing.Size(100, 20);
            this.KTextBox.Steps = -1D;
            this.KTextBox.TabIndex = 5;
            this.toolTip1.SetToolTip(this.KTextBox, resources.GetString("KTextBox.ToolTip"));
            this.KTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.KTextBox.Value = 0D;
            // 
            // ConventionalATextBox
            // 
            this.ConventionalATextBox.Location = new System.Drawing.Point(169, 160);
            this.ConventionalATextBox.Max = 1.7976931348623157E+308D;
            this.ConventionalATextBox.Min = 0D;
            this.ConventionalATextBox.Name = "ConventionalATextBox";
            this.ConventionalATextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.ConventionalATextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.ConventionalATextBox.Size = new System.Drawing.Size(100, 20);
            this.ConventionalATextBox.Steps = -1D;
            this.ConventionalATextBox.TabIndex = 6;
            this.toolTip1.SetToolTip(this.ConventionalATextBox, resources.GetString("ConventionalATextBox.ToolTip"));
            this.ConventionalATextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.ConventionalATextBox.Value = 0D;
            // 
            // ConventionalBTextBox
            // 
            this.ConventionalBTextBox.Location = new System.Drawing.Point(169, 185);
            this.ConventionalBTextBox.Max = 1.7976931348623157E+308D;
            this.ConventionalBTextBox.Min = 0D;
            this.ConventionalBTextBox.Name = "ConventionalBTextBox";
            this.ConventionalBTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.ConventionalBTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.ConventionalBTextBox.Size = new System.Drawing.Size(100, 20);
            this.ConventionalBTextBox.Steps = -1D;
            this.ConventionalBTextBox.TabIndex = 7;
            this.toolTip1.SetToolTip(this.ConventionalBTextBox, resources.GetString("ConventionalBTextBox.ToolTip"));
            this.ConventionalBTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.ConventionalBTextBox.Value = 0D;
            // 
            // VarianceATextBox
            // 
            this.VarianceATextBox.Location = new System.Drawing.Point(169, 211);
            this.VarianceATextBox.Max = 1.7976931348623157E+308D;
            this.VarianceATextBox.Min = 0D;
            this.VarianceATextBox.Name = "VarianceATextBox";
            this.VarianceATextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.VarianceATextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.VarianceATextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceATextBox.Steps = -1D;
            this.VarianceATextBox.TabIndex = 8;
            this.toolTip1.SetToolTip(this.VarianceATextBox, "Variance a is the variance of the calibration parameter a above. It can be entere" +
        "d manually or obtained automatically from the Deming curve fitting option.");
            this.VarianceATextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.VarianceATextBox.Value = 0D;
            // 
            // VarianceBTextBox
            // 
            this.VarianceBTextBox.Location = new System.Drawing.Point(169, 237);
            this.VarianceBTextBox.Max = 1.7976931348623157E+308D;
            this.VarianceBTextBox.Min = 0D;
            this.VarianceBTextBox.Name = "VarianceBTextBox";
            this.VarianceBTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.VarianceBTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.VarianceBTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceBTextBox.Steps = -1D;
            this.VarianceBTextBox.TabIndex = 9;
            this.toolTip1.SetToolTip(this.VarianceBTextBox, "Variance b is the variance of the calibration parameter b above. It can be entere" +
        "d manually or obtained automatically from the Deming curve fitting option.");
            this.VarianceBTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.VarianceBTextBox.Value = 0D;
            // 
            // CovarianceABTextBox
            // 
            this.CovarianceABTextBox.Location = new System.Drawing.Point(169, 263);
            this.CovarianceABTextBox.Max = 1.7976931348623157E+308D;
            this.CovarianceABTextBox.Min = 0D;
            this.CovarianceABTextBox.Name = "CovarianceABTextBox";
            this.CovarianceABTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.CovarianceABTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CovarianceABTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceABTextBox.Steps = -1D;
            this.CovarianceABTextBox.TabIndex = 10;
            this.toolTip1.SetToolTip(this.CovarianceABTextBox, resources.GetString("CovarianceABTextBox.ToolTip"));
            this.CovarianceABTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.CovarianceABTextBox.Value = 0D;
            // 
            // SigmaXTextBox
            // 
            this.SigmaXTextBox.Location = new System.Drawing.Point(169, 289);
            this.SigmaXTextBox.Max = 1.7976931348623157E+308D;
            this.SigmaXTextBox.Min = 0D;
            this.SigmaXTextBox.Name = "SigmaXTextBox";
            this.SigmaXTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.SigmaXTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.SigmaXTextBox.Size = new System.Drawing.Size(100, 20);
            this.SigmaXTextBox.Steps = -1D;
            this.SigmaXTextBox.TabIndex = 11;
            this.toolTip1.SetToolTip(this.SigmaXTextBox, resources.GetString("SigmaXTextBox.ToolTip"));
            this.SigmaXTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.SigmaXTextBox.Value = 0D;
            // 
            // HvyMetalRefTextBox
            // 
            this.HvyMetalRefTextBox.Location = new System.Drawing.Point(174, 29);
            this.HvyMetalRefTextBox.Max = 1.7976931348623157E+308D;
            this.HvyMetalRefTextBox.Min = 0D;
            this.HvyMetalRefTextBox.Name = "HvyMetalRefTextBox";
            this.HvyMetalRefTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.HvyMetalRefTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.HvyMetalRefTextBox.Size = new System.Drawing.Size(100, 20);
            this.HvyMetalRefTextBox.Steps = -1D;
            this.HvyMetalRefTextBox.TabIndex = 12;
            this.toolTip1.SetToolTip(this.HvyMetalRefTextBox, resources.GetString("HvyMetalRefTextBox.ToolTip"));
            this.HvyMetalRefTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.HvyMetalRefTextBox.Value = 0D;
            // 
            // HvyMetalWeightingTextBox
            // 
            this.HvyMetalWeightingTextBox.Location = new System.Drawing.Point(174, 55);
            this.HvyMetalWeightingTextBox.Max = 1.7976931348623157E+308D;
            this.HvyMetalWeightingTextBox.Min = 0D;
            this.HvyMetalWeightingTextBox.Name = "HvyMetalWeightingTextBox";
            this.HvyMetalWeightingTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.HvyMetalWeightingTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.HvyMetalWeightingTextBox.Size = new System.Drawing.Size(100, 20);
            this.HvyMetalWeightingTextBox.Steps = -1D;
            this.HvyMetalWeightingTextBox.TabIndex = 13;
            this.toolTip1.SetToolTip(this.HvyMetalWeightingTextBox, resources.GetString("HvyMetalWeightingTextBox.ToolTip"));
            this.HvyMetalWeightingTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.HvyMetalWeightingTextBox.Value = 0D;
            // 
            // LowerAlphaLimitTextBox
            // 
            this.LowerAlphaLimitTextBox.Location = new System.Drawing.Point(185, 27);
            this.LowerAlphaLimitTextBox.Max = 1.7976931348623157E+308D;
            this.LowerAlphaLimitTextBox.Min = 0D;
            this.LowerAlphaLimitTextBox.Name = "LowerAlphaLimitTextBox";
            this.LowerAlphaLimitTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.LowerAlphaLimitTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.LowerAlphaLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.LowerAlphaLimitTextBox.Steps = -1D;
            this.LowerAlphaLimitTextBox.TabIndex = 14;
            this.toolTip1.SetToolTip(this.LowerAlphaLimitTextBox, resources.GetString("LowerAlphaLimitTextBox.ToolTip"));
            this.LowerAlphaLimitTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.LowerAlphaLimitTextBox.Value = 0D;
            // 
            // UpperAlphaLimitTextBox
            // 
            this.UpperAlphaLimitTextBox.Location = new System.Drawing.Point(185, 53);
            this.UpperAlphaLimitTextBox.Max = 1.7976931348623157E+308D;
            this.UpperAlphaLimitTextBox.Min = 0D;
            this.UpperAlphaLimitTextBox.Name = "UpperAlphaLimitTextBox";
            this.UpperAlphaLimitTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.UpperAlphaLimitTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.UpperAlphaLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.UpperAlphaLimitTextBox.Steps = -1D;
            this.UpperAlphaLimitTextBox.TabIndex = 15;
            this.toolTip1.SetToolTip(this.UpperAlphaLimitTextBox, resources.GetString("UpperAlphaLimitTextBox.ToolTip"));
            this.UpperAlphaLimitTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.UpperAlphaLimitTextBox.Value = 0D;
            // 
            // MoistureATextBox
            // 
            this.MoistureATextBox.Location = new System.Drawing.Point(185, 106);
            this.MoistureATextBox.Max = 1.7976931348623157E+308D;
            this.MoistureATextBox.Min = 0D;
            this.MoistureATextBox.Name = "MoistureATextBox";
            this.MoistureATextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.MoistureATextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.MoistureATextBox.Size = new System.Drawing.Size(100, 20);
            this.MoistureATextBox.Steps = -1D;
            this.MoistureATextBox.TabIndex = 16;
            this.toolTip1.SetToolTip(this.MoistureATextBox, resources.GetString("MoistureATextBox.ToolTip"));
            this.MoistureATextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.MoistureATextBox.Value = 0D;
            // 
            // MoistureBTextBox
            // 
            this.MoistureBTextBox.Location = new System.Drawing.Point(185, 132);
            this.MoistureBTextBox.Max = 1.7976931348623157E+308D;
            this.MoistureBTextBox.Min = 0D;
            this.MoistureBTextBox.Name = "MoistureBTextBox";
            this.MoistureBTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.MoistureBTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.MoistureBTextBox.Size = new System.Drawing.Size(100, 20);
            this.MoistureBTextBox.Steps = -1D;
            this.MoistureBTextBox.TabIndex = 17;
            this.toolTip1.SetToolTip(this.MoistureBTextBox, resources.GetString("MoistureBTextBox.ToolTip"));
            this.MoistureBTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.MoistureBTextBox.Value = 0D;
            // 
            // MoistureCTextBox
            // 
            this.MoistureCTextBox.Location = new System.Drawing.Point(185, 158);
            this.MoistureCTextBox.Max = 1.7976931348623157E+308D;
            this.MoistureCTextBox.Min = 0D;
            this.MoistureCTextBox.Name = "MoistureCTextBox";
            this.MoistureCTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.MoistureCTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.MoistureCTextBox.Size = new System.Drawing.Size(100, 20);
            this.MoistureCTextBox.Steps = -1D;
            this.MoistureCTextBox.TabIndex = 18;
            this.toolTip1.SetToolTip(this.MoistureCTextBox, resources.GetString("MoistureCTextBox.ToolTip"));
            this.MoistureCTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.MoistureCTextBox.Value = 0D;
            // 
            // MoistureDTextBox
            // 
            this.MoistureDTextBox.Location = new System.Drawing.Point(185, 184);
            this.MoistureDTextBox.Max = 1.7976931348623157E+308D;
            this.MoistureDTextBox.Min = 0D;
            this.MoistureDTextBox.Name = "MoistureDTextBox";
            this.MoistureDTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.MoistureDTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.MoistureDTextBox.Size = new System.Drawing.Size(100, 20);
            this.MoistureDTextBox.Steps = -1D;
            this.MoistureDTextBox.TabIndex = 19;
            this.toolTip1.SetToolTip(this.MoistureDTextBox, resources.GetString("MoistureDTextBox.ToolTip"));
            this.MoistureDTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.MoistureDTextBox.Value = 0D;
            // 
            // CurveTypeComboBox
            // 
            this.CurveTypeComboBox.FormattingEnabled = true;
            this.CurveTypeComboBox.Location = new System.Drawing.Point(185, 79);
            this.CurveTypeComboBox.Name = "CurveTypeComboBox";
            this.CurveTypeComboBox.Size = new System.Drawing.Size(100, 21);
            this.CurveTypeComboBox.TabIndex = 20;
            this.toolTip1.SetToolTip(this.CurveTypeComboBox, "Select the type of calibration curve to be used to calculate the dry alpha or mul" +
        "tiplication corrected doubles rate correction factor from the ring ratio.");
            this.CurveTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.CurveTypeComboBox_SelectedIndexChanged);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(792, 16);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(163, 23);
            this.PrintBtn.TabIndex = 21;
            this.PrintBtn.Text = "Print calibration parameters";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(880, 66);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 22;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(881, 95);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 23;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(880, 124);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 24;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // KnownAlphaTypeGroupBox
            // 
            this.KnownAlphaTypeGroupBox.Controls.Add(this.MoistureToDoublesRadioButton);
            this.KnownAlphaTypeGroupBox.Controls.Add(this.MoistureToDryAlphaRadioButton);
            this.KnownAlphaTypeGroupBox.Controls.Add(this.HeavyMetalRadioButton);
            this.KnownAlphaTypeGroupBox.Controls.Add(this.ConventionalRadioButton);
            this.KnownAlphaTypeGroupBox.Location = new System.Drawing.Point(324, 12);
            this.KnownAlphaTypeGroupBox.Name = "KnownAlphaTypeGroupBox";
            this.KnownAlphaTypeGroupBox.Size = new System.Drawing.Size(393, 135);
            this.KnownAlphaTypeGroupBox.TabIndex = 25;
            this.KnownAlphaTypeGroupBox.TabStop = false;
            this.KnownAlphaTypeGroupBox.Text = "Type of known alpha analysis";
            // 
            // MoistureToDoublesRadioButton
            // 
            this.MoistureToDoublesRadioButton.AutoSize = true;
            this.MoistureToDoublesRadioButton.Location = new System.Drawing.Point(21, 97);
            this.MoistureToDoublesRadioButton.Name = "MoistureToDoublesRadioButton";
            this.MoistureToDoublesRadioButton.Size = new System.Drawing.Size(336, 17);
            this.MoistureToDoublesRadioButton.TabIndex = 3;
            this.MoistureToDoublesRadioButton.TabStop = true;
            this.MoistureToDoublesRadioButton.Text = "Moisture correction applied to multiplication-corrected doubles rate";
            this.MoistureToDoublesRadioButton.UseVisualStyleBackColor = true;
            this.MoistureToDoublesRadioButton.CheckedChanged += new System.EventHandler(this.MoistureToDoublesRadioButton_CheckedChanged);
            // 
            // MoistureToDryAlphaRadioButton
            // 
            this.MoistureToDryAlphaRadioButton.AutoSize = true;
            this.MoistureToDryAlphaRadioButton.Location = new System.Drawing.Point(21, 74);
            this.MoistureToDryAlphaRadioButton.Name = "MoistureToDryAlphaRadioButton";
            this.MoistureToDryAlphaRadioButton.Size = new System.Drawing.Size(210, 17);
            this.MoistureToDryAlphaRadioButton.TabIndex = 2;
            this.MoistureToDryAlphaRadioButton.TabStop = true;
            this.MoistureToDryAlphaRadioButton.Text = "Moisture correction applied to dry alpha";
            this.MoistureToDryAlphaRadioButton.UseVisualStyleBackColor = true;
            this.MoistureToDryAlphaRadioButton.CheckedChanged += new System.EventHandler(this.MoistureToDryAlphaRadioButton_CheckedChanged);
            // 
            // HeavyMetalRadioButton
            // 
            this.HeavyMetalRadioButton.AutoSize = true;
            this.HeavyMetalRadioButton.Location = new System.Drawing.Point(21, 51);
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
            this.ConventionalRadioButton.Location = new System.Drawing.Point(21, 28);
            this.ConventionalRadioButton.Name = "ConventionalRadioButton";
            this.ConventionalRadioButton.Size = new System.Drawing.Size(151, 17);
            this.ConventionalRadioButton.TabIndex = 0;
            this.ConventionalRadioButton.TabStop = true;
            this.ConventionalRadioButton.Text = "Conventional known alpha";
            this.ConventionalRadioButton.UseVisualStyleBackColor = true;
            this.ConventionalRadioButton.CheckedChanged += new System.EventHandler(this.ConventionalRadioButton_CheckedChanged);
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(14, 21);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 26;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // LowerPuLimitLabel
            // 
            this.LowerPuLimitLabel.AutoSize = true;
            this.LowerPuLimitLabel.Location = new System.Drawing.Point(6, 32);
            this.LowerPuLimitLabel.Name = "LowerPuLimitLabel";
            this.LowerPuLimitLabel.Size = new System.Drawing.Size(138, 13);
            this.LowerPuLimitLabel.TabIndex = 28;
            this.LowerPuLimitLabel.Text = "Lower Pu240e mass limit (g)";
            // 
            // UpperPuLimitLabel
            // 
            this.UpperPuLimitLabel.AutoSize = true;
            this.UpperPuLimitLabel.Location = new System.Drawing.Point(6, 58);
            this.UpperPuLimitLabel.Name = "UpperPuLimitLabel";
            this.UpperPuLimitLabel.Size = new System.Drawing.Size(138, 13);
            this.UpperPuLimitLabel.TabIndex = 29;
            this.UpperPuLimitLabel.Text = "Upper Pu240e mass limit (g)";
            // 
            // AlphaWeightLabel
            // 
            this.AlphaWeightLabel.AutoSize = true;
            this.AlphaWeightLabel.Location = new System.Drawing.Point(76, 84);
            this.AlphaWeightLabel.Name = "AlphaWeightLabel";
            this.AlphaWeightLabel.Size = new System.Drawing.Size(68, 13);
            this.AlphaWeightLabel.TabIndex = 30;
            this.AlphaWeightLabel.Text = "Alpha weight";
            // 
            // RhoZeroLabel
            // 
            this.RhoZeroLabel.AutoSize = true;
            this.RhoZeroLabel.Location = new System.Drawing.Point(94, 110);
            this.RhoZeroLabel.Name = "RhoZeroLabel";
            this.RhoZeroLabel.Size = new System.Drawing.Size(50, 13);
            this.RhoZeroLabel.TabIndex = 31;
            this.RhoZeroLabel.Text = "Rho zero";
            // 
            // KLabel
            // 
            this.KLabel.AutoSize = true;
            this.KLabel.Location = new System.Drawing.Point(131, 136);
            this.KLabel.Name = "KLabel";
            this.KLabel.Size = new System.Drawing.Size(13, 13);
            this.KLabel.TabIndex = 32;
            this.KLabel.Text = "k";
            // 
            // ConventionalALabel
            // 
            this.ConventionalALabel.AutoSize = true;
            this.ConventionalALabel.Location = new System.Drawing.Point(131, 162);
            this.ConventionalALabel.Name = "ConventionalALabel";
            this.ConventionalALabel.Size = new System.Drawing.Size(13, 13);
            this.ConventionalALabel.TabIndex = 33;
            this.ConventionalALabel.Text = "a";
            // 
            // ConventionalBLabel
            // 
            this.ConventionalBLabel.AutoSize = true;
            this.ConventionalBLabel.Location = new System.Drawing.Point(131, 188);
            this.ConventionalBLabel.Name = "ConventionalBLabel";
            this.ConventionalBLabel.Size = new System.Drawing.Size(13, 13);
            this.ConventionalBLabel.TabIndex = 34;
            this.ConventionalBLabel.Text = "b";
            // 
            // VarianceALabel
            // 
            this.VarianceALabel.AutoSize = true;
            this.VarianceALabel.Location = new System.Drawing.Point(86, 214);
            this.VarianceALabel.Name = "VarianceALabel";
            this.VarianceALabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceALabel.TabIndex = 35;
            this.VarianceALabel.Text = "Variance a";
            // 
            // VarianceBLabel
            // 
            this.VarianceBLabel.AutoSize = true;
            this.VarianceBLabel.Location = new System.Drawing.Point(86, 240);
            this.VarianceBLabel.Name = "VarianceBLabel";
            this.VarianceBLabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceBLabel.TabIndex = 36;
            this.VarianceBLabel.Text = "Variance b";
            // 
            // CovarianceABLabel
            // 
            this.CovarianceABLabel.AutoSize = true;
            this.CovarianceABLabel.Location = new System.Drawing.Point(68, 266);
            this.CovarianceABLabel.Name = "CovarianceABLabel";
            this.CovarianceABLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceABLabel.TabIndex = 37;
            this.CovarianceABLabel.Text = "Covariance ab";
            // 
            // SigmaXLabel
            // 
            this.SigmaXLabel.AutoSize = true;
            this.SigmaXLabel.Location = new System.Drawing.Point(83, 292);
            this.SigmaXLabel.Name = "SigmaXLabel";
            this.SigmaXLabel.Size = new System.Drawing.Size(61, 13);
            this.SigmaXLabel.TabIndex = 38;
            this.SigmaXLabel.Text = "Sigma x (%)";
            // 
            // HeavyMetalReferenceLabel
            // 
            this.HeavyMetalReferenceLabel.AutoSize = true;
            this.HeavyMetalReferenceLabel.Location = new System.Drawing.Point(18, 32);
            this.HeavyMetalReferenceLabel.Name = "HeavyMetalReferenceLabel";
            this.HeavyMetalReferenceLabel.Size = new System.Drawing.Size(148, 13);
            this.HeavyMetalReferenceLabel.TabIndex = 39;
            this.HeavyMetalReferenceLabel.Text = "Heavy metal reference (g/cm)";
            // 
            // HeavyMetalWeightingLabel
            // 
            this.HeavyMetalWeightingLabel.AutoSize = true;
            this.HeavyMetalWeightingLabel.Location = new System.Drawing.Point(22, 58);
            this.HeavyMetalWeightingLabel.Name = "HeavyMetalWeightingLabel";
            this.HeavyMetalWeightingLabel.Size = new System.Drawing.Size(144, 13);
            this.HeavyMetalWeightingLabel.TabIndex = 40;
            this.HeavyMetalWeightingLabel.Text = "Heavy metal weighting factor";
            // 
            // LowerAlphaLimitLabel
            // 
            this.LowerAlphaLimitLabel.AutoSize = true;
            this.LowerAlphaLimitLabel.Location = new System.Drawing.Point(8, 30);
            this.LowerAlphaLimitLabel.Name = "LowerAlphaLimitLabel";
            this.LowerAlphaLimitLabel.Size = new System.Drawing.Size(165, 13);
            this.LowerAlphaLimitLabel.TabIndex = 43;
            this.LowerAlphaLimitLabel.Text = "Lower alpha correction factor limit";
            // 
            // UpperAlphaLimitLabel
            // 
            this.UpperAlphaLimitLabel.AutoSize = true;
            this.UpperAlphaLimitLabel.Location = new System.Drawing.Point(8, 56);
            this.UpperAlphaLimitLabel.Name = "UpperAlphaLimitLabel";
            this.UpperAlphaLimitLabel.Size = new System.Drawing.Size(165, 13);
            this.UpperAlphaLimitLabel.TabIndex = 44;
            this.UpperAlphaLimitLabel.Text = "Upper alpha correction factor limit";
            // 
            // CurveTypeLabel
            // 
            this.CurveTypeLabel.AutoSize = true;
            this.CurveTypeLabel.Location = new System.Drawing.Point(121, 82);
            this.CurveTypeLabel.Name = "CurveTypeLabel";
            this.CurveTypeLabel.Size = new System.Drawing.Size(58, 13);
            this.CurveTypeLabel.TabIndex = 45;
            this.CurveTypeLabel.Text = "Curve type";
            // 
            // MoistureALabel
            // 
            this.MoistureALabel.AutoSize = true;
            this.MoistureALabel.Location = new System.Drawing.Point(166, 109);
            this.MoistureALabel.Name = "MoistureALabel";
            this.MoistureALabel.Size = new System.Drawing.Size(13, 13);
            this.MoistureALabel.TabIndex = 46;
            this.MoistureALabel.Text = "a";
            // 
            // MoistureBLabel
            // 
            this.MoistureBLabel.AutoSize = true;
            this.MoistureBLabel.Location = new System.Drawing.Point(166, 135);
            this.MoistureBLabel.Name = "MoistureBLabel";
            this.MoistureBLabel.Size = new System.Drawing.Size(13, 13);
            this.MoistureBLabel.TabIndex = 47;
            this.MoistureBLabel.Text = "b";
            // 
            // MoistureCLabel
            // 
            this.MoistureCLabel.AutoSize = true;
            this.MoistureCLabel.Location = new System.Drawing.Point(166, 161);
            this.MoistureCLabel.Name = "MoistureCLabel";
            this.MoistureCLabel.Size = new System.Drawing.Size(13, 13);
            this.MoistureCLabel.TabIndex = 48;
            this.MoistureCLabel.Text = "c";
            // 
            // MoistureDLabel
            // 
            this.MoistureDLabel.AutoSize = true;
            this.MoistureDLabel.Location = new System.Drawing.Point(166, 187);
            this.MoistureDLabel.Name = "MoistureDLabel";
            this.MoistureDLabel.Size = new System.Drawing.Size(13, 13);
            this.MoistureDLabel.TabIndex = 49;
            this.MoistureDLabel.Text = "d";
            // 
            // ConventionalParametersGroupBox
            // 
            this.ConventionalParametersGroupBox.Controls.Add(this.SigmaXLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.CovarianceABLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.VarianceBLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.VarianceALabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.ConventionalBLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.ConventionalALabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.KLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.RhoZeroLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.AlphaWeightLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.UpperPuLimitLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.LowerPuLimitLabel);
            this.ConventionalParametersGroupBox.Controls.Add(this.SigmaXTextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.CovarianceABTextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.VarianceBTextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.VarianceATextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.ConventionalBTextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.ConventionalATextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.KTextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.RhoZeroTextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.AlphaWeightTextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.UpperMassLimitTextBox);
            this.ConventionalParametersGroupBox.Controls.Add(this.LowerMassLimitTextBox);
            this.ConventionalParametersGroupBox.Location = new System.Drawing.Point(17, 63);
            this.ConventionalParametersGroupBox.Name = "ConventionalParametersGroupBox";
            this.ConventionalParametersGroupBox.Size = new System.Drawing.Size(287, 330);
            this.ConventionalParametersGroupBox.TabIndex = 50;
            this.ConventionalParametersGroupBox.TabStop = false;
            this.ConventionalParametersGroupBox.Text = "Conventional known alpha parameters";
            // 
            // HeavyMetalCorrectionGroupBox
            // 
            this.HeavyMetalCorrectionGroupBox.Controls.Add(this.HeavyMetalWeightingLabel);
            this.HeavyMetalCorrectionGroupBox.Controls.Add(this.HeavyMetalReferenceLabel);
            this.HeavyMetalCorrectionGroupBox.Controls.Add(this.HvyMetalWeightingTextBox);
            this.HeavyMetalCorrectionGroupBox.Controls.Add(this.HvyMetalRefTextBox);
            this.HeavyMetalCorrectionGroupBox.Location = new System.Drawing.Point(324, 169);
            this.HeavyMetalCorrectionGroupBox.Name = "HeavyMetalCorrectionGroupBox";
            this.HeavyMetalCorrectionGroupBox.Size = new System.Drawing.Size(298, 224);
            this.HeavyMetalCorrectionGroupBox.TabIndex = 51;
            this.HeavyMetalCorrectionGroupBox.TabStop = false;
            this.HeavyMetalCorrectionGroupBox.Text = "Heavy metal correction parameters";
            // 
            // MoistureCorrectionGroupBox
            // 
            this.MoistureCorrectionGroupBox.Controls.Add(this.MoistureDLabel);
            this.MoistureCorrectionGroupBox.Controls.Add(this.MoistureCLabel);
            this.MoistureCorrectionGroupBox.Controls.Add(this.MoistureBLabel);
            this.MoistureCorrectionGroupBox.Controls.Add(this.MoistureALabel);
            this.MoistureCorrectionGroupBox.Controls.Add(this.CurveTypeLabel);
            this.MoistureCorrectionGroupBox.Controls.Add(this.UpperAlphaLimitLabel);
            this.MoistureCorrectionGroupBox.Controls.Add(this.LowerAlphaLimitLabel);
            this.MoistureCorrectionGroupBox.Controls.Add(this.CurveTypeComboBox);
            this.MoistureCorrectionGroupBox.Controls.Add(this.MoistureDTextBox);
            this.MoistureCorrectionGroupBox.Controls.Add(this.MoistureCTextBox);
            this.MoistureCorrectionGroupBox.Controls.Add(this.MoistureBTextBox);
            this.MoistureCorrectionGroupBox.Controls.Add(this.MoistureATextBox);
            this.MoistureCorrectionGroupBox.Controls.Add(this.UpperAlphaLimitTextBox);
            this.MoistureCorrectionGroupBox.Controls.Add(this.LowerAlphaLimitTextBox);
            this.MoistureCorrectionGroupBox.Location = new System.Drawing.Point(642, 169);
            this.MoistureCorrectionGroupBox.Name = "MoistureCorrectionGroupBox";
            this.MoistureCorrectionGroupBox.Size = new System.Drawing.Size(313, 224);
            this.MoistureCorrectionGroupBox.TabIndex = 52;
            this.MoistureCorrectionGroupBox.TabStop = false;
            this.MoistureCorrectionGroupBox.Text = "Moisture correction parameters";
            // 
            // IDDKnownAlphaCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 406);
            this.Controls.Add(this.MoistureCorrectionGroupBox);
            this.Controls.Add(this.HeavyMetalCorrectionGroupBox);
            this.Controls.Add(this.ConventionalParametersGroupBox);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.KnownAlphaTypeGroupBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Name = "IDDKnownAlphaCal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Known Alpha Calibration";
            this.KnownAlphaTypeGroupBox.ResumeLayout(false);
            this.KnownAlphaTypeGroupBox.PerformLayout();
            this.ConventionalParametersGroupBox.ResumeLayout(false);
            this.ConventionalParametersGroupBox.PerformLayout();
            this.HeavyMetalCorrectionGroupBox.ResumeLayout(false);
            this.HeavyMetalCorrectionGroupBox.PerformLayout();
            this.MoistureCorrectionGroupBox.ResumeLayout(false);
            this.MoistureCorrectionGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private UI.NumericTextBox UpperMassLimitTextBox;
        private UI.NumericTextBox LowerMassLimitTextBox;
        private UI.NumericTextBox AlphaWeightTextBox;
        private UI.NumericTextBox RhoZeroTextBox;
        private UI.NumericTextBox KTextBox;
        private UI.NumericTextBox ConventionalATextBox;
        private UI.NumericTextBox ConventionalBTextBox;
        private UI.NumericTextBox VarianceATextBox;
        private UI.NumericTextBox VarianceBTextBox;
        private UI.NumericTextBox CovarianceABTextBox;
        private UI.NumericTextBox SigmaXTextBox;
        private UI.NumericTextBox HvyMetalRefTextBox;
        private UI.NumericTextBox HvyMetalWeightingTextBox;
        private UI.NumericTextBox LowerAlphaLimitTextBox;
        private UI.NumericTextBox UpperAlphaLimitTextBox;
        private UI.NumericTextBox MoistureATextBox;
        private UI.NumericTextBox MoistureBTextBox;
        private UI.NumericTextBox MoistureCTextBox;
        private UI.NumericTextBox MoistureDTextBox;
        private System.Windows.Forms.ComboBox CurveTypeComboBox;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.GroupBox KnownAlphaTypeGroupBox;
        private System.Windows.Forms.RadioButton MoistureToDoublesRadioButton;
        private System.Windows.Forms.RadioButton MoistureToDryAlphaRadioButton;
        private System.Windows.Forms.RadioButton HeavyMetalRadioButton;
        private System.Windows.Forms.RadioButton ConventionalRadioButton;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label LowerPuLimitLabel;
        private System.Windows.Forms.Label UpperPuLimitLabel;
        private System.Windows.Forms.Label AlphaWeightLabel;
        private System.Windows.Forms.Label RhoZeroLabel;
        private System.Windows.Forms.Label KLabel;
        private System.Windows.Forms.Label ConventionalALabel;
        private System.Windows.Forms.Label ConventionalBLabel;
        private System.Windows.Forms.Label VarianceALabel;
        private System.Windows.Forms.Label VarianceBLabel;
        private System.Windows.Forms.Label CovarianceABLabel;
        private System.Windows.Forms.Label SigmaXLabel;
        private System.Windows.Forms.Label HeavyMetalReferenceLabel;
        private System.Windows.Forms.Label HeavyMetalWeightingLabel;
        private System.Windows.Forms.Label LowerAlphaLimitLabel;
        private System.Windows.Forms.Label UpperAlphaLimitLabel;
        private System.Windows.Forms.Label CurveTypeLabel;
        private System.Windows.Forms.Label MoistureALabel;
        private System.Windows.Forms.Label MoistureBLabel;
        private System.Windows.Forms.Label MoistureCLabel;
        private System.Windows.Forms.Label MoistureDLabel;
        private System.Windows.Forms.GroupBox ConventionalParametersGroupBox;
        private System.Windows.Forms.GroupBox HeavyMetalCorrectionGroupBox;
        private System.Windows.Forms.GroupBox MoistureCorrectionGroupBox;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}