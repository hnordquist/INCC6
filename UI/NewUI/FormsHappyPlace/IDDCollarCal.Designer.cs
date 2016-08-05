namespace NewUI
{
    partial class IDDCollarCal
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
            this.CurveTypeComboBox = new System.Windows.Forms.ComboBox();
            this.NextBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CurveTypeLabel = new System.Windows.Forms.Label();
            this.ALabel = new System.Windows.Forms.Label();
            this.BLabel = new System.Windows.Forms.Label();
            this.CLabel = new System.Windows.Forms.Label();
            this.DLabel = new System.Windows.Forms.Label();
            this.VarALabel = new System.Windows.Forms.Label();
            this.VarBLabel = new System.Windows.Forms.Label();
            this.VarClabel = new System.Windows.Forms.Label();
            this.VarDLabel = new System.Windows.Forms.Label();
            this.LowerMassLimitLabel = new System.Windows.Forms.Label();
            this.UpperMassLimitLabel = new System.Windows.Forms.Label();
            this.CovarABLabel = new System.Windows.Forms.Label();
            this.CovarACLabel = new System.Windows.Forms.Label();
            this.CovarADLabel = new System.Windows.Forms.Label();
            this.CovarBCLabel = new System.Windows.Forms.Label();
            this.CovarBDLabel = new System.Windows.Forms.Label();
            this.CovarCDLabel = new System.Windows.Forms.Label();
            this.SigmaXLabel = new System.Windows.Forms.Label();
            this.CovarianceCDTextBox = new NewUI.NumericTextBox();
            this.SigmaXTextBox = new NewUI.NumericTextBox();
            this.CovarianceBDTextBox = new NewUI.NumericTextBox();
            this.CovarianceBCTextBox = new NewUI.NumericTextBox();
            this.CovarianceADTextBox = new NewUI.NumericTextBox();
            this.CovarianceACTextBox = new NewUI.NumericTextBox();
            this.CovarianceABTextBox = new NewUI.NumericTextBox();
            this.UpperMassLimitTextBox = new NewUI.NumericTextBox();
            this.LowerMassLimitTextBox = new NewUI.NumericTextBox();
            this.VarianceDTextBox = new NewUI.NumericTextBox();
            this.VarianceCTextBox = new NewUI.NumericTextBox();
            this.VarianceBTextBox = new NewUI.NumericTextBox();
            this.VarianceATextBox = new NewUI.NumericTextBox();
            this.DTextBox = new NewUI.NumericTextBox();
            this.CTextBox = new NewUI.NumericTextBox();
            this.BTextBox = new NewUI.NumericTextBox();
            this.ATextBox = new NewUI.NumericTextBox();
            this.BackBtn = new System.Windows.Forms.Button();
            this.provider = new System.Windows.Forms.HelpProvider();
            this.SuspendLayout();
            // 
            // CurveTypeComboBox
            // 
            this.CurveTypeComboBox.FormattingEnabled = true;
            this.CurveTypeComboBox.Location = new System.Drawing.Point(74, 15);
            this.CurveTypeComboBox.Name = "CurveTypeComboBox";
            this.CurveTypeComboBox.Size = new System.Drawing.Size(223, 21);
            this.CurveTypeComboBox.TabIndex = 0;
            this.CurveTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.CurveTypeComboBox_SelectedIndexChanged);
            // 
            // NextBtn
            // 
            this.NextBtn.Location = new System.Drawing.Point(606, 56);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(75, 23);
            this.NextBtn.TabIndex = 20;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(606, 85);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 21;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(606, 114);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 22;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // CurveTypeLabel
            // 
            this.CurveTypeLabel.AutoSize = true;
            this.CurveTypeLabel.Location = new System.Drawing.Point(12, 18);
            this.CurveTypeLabel.Name = "CurveTypeLabel";
            this.CurveTypeLabel.Size = new System.Drawing.Size(58, 13);
            this.CurveTypeLabel.TabIndex = 23;
            this.CurveTypeLabel.Text = "Curve type";
            // 
            // ALabel
            // 
            this.ALabel.AutoSize = true;
            this.ALabel.Location = new System.Drawing.Point(25, 91);
            this.ALabel.Name = "ALabel";
            this.ALabel.Size = new System.Drawing.Size(13, 13);
            this.ALabel.TabIndex = 24;
            this.ALabel.Text = "a";
            // 
            // BLabel
            // 
            this.BLabel.AutoSize = true;
            this.BLabel.Location = new System.Drawing.Point(25, 117);
            this.BLabel.Name = "BLabel";
            this.BLabel.Size = new System.Drawing.Size(13, 13);
            this.BLabel.TabIndex = 25;
            this.BLabel.Text = "b";
            // 
            // CLabel
            // 
            this.CLabel.AutoSize = true;
            this.CLabel.Location = new System.Drawing.Point(25, 143);
            this.CLabel.Name = "CLabel";
            this.CLabel.Size = new System.Drawing.Size(13, 13);
            this.CLabel.TabIndex = 26;
            this.CLabel.Text = "c";
            // 
            // DLabel
            // 
            this.DLabel.AutoSize = true;
            this.DLabel.Location = new System.Drawing.Point(25, 169);
            this.DLabel.Name = "DLabel";
            this.DLabel.Size = new System.Drawing.Size(13, 13);
            this.DLabel.TabIndex = 27;
            this.DLabel.Text = "d";
            // 
            // VarALabel
            // 
            this.VarALabel.AutoSize = true;
            this.VarALabel.Location = new System.Drawing.Point(190, 91);
            this.VarALabel.Name = "VarALabel";
            this.VarALabel.Size = new System.Drawing.Size(58, 13);
            this.VarALabel.TabIndex = 28;
            this.VarALabel.Text = "Variance a";
            // 
            // VarBLabel
            // 
            this.VarBLabel.AutoSize = true;
            this.VarBLabel.Location = new System.Drawing.Point(190, 117);
            this.VarBLabel.Name = "VarBLabel";
            this.VarBLabel.Size = new System.Drawing.Size(58, 13);
            this.VarBLabel.TabIndex = 29;
            this.VarBLabel.Text = "Variance b";
            // 
            // VarClabel
            // 
            this.VarClabel.AutoSize = true;
            this.VarClabel.Location = new System.Drawing.Point(190, 143);
            this.VarClabel.Name = "VarClabel";
            this.VarClabel.Size = new System.Drawing.Size(58, 13);
            this.VarClabel.TabIndex = 30;
            this.VarClabel.Text = "Variance c";
            // 
            // VarDLabel
            // 
            this.VarDLabel.AutoSize = true;
            this.VarDLabel.Location = new System.Drawing.Point(190, 169);
            this.VarDLabel.Name = "VarDLabel";
            this.VarDLabel.Size = new System.Drawing.Size(58, 13);
            this.VarDLabel.TabIndex = 31;
            this.VarDLabel.Text = "Variance d";
            // 
            // LowerMassLimitLabel
            // 
            this.LowerMassLimitLabel.AutoSize = true;
            this.LowerMassLimitLabel.Location = new System.Drawing.Point(331, 18);
            this.LowerMassLimitLabel.Name = "LowerMassLimitLabel";
            this.LowerMassLimitLabel.Size = new System.Drawing.Size(127, 13);
            this.LowerMassLimitLabel.TabIndex = 32;
            this.LowerMassLimitLabel.Text = "Lower U235 mass limit (g)";
            // 
            // UpperMassLimitLabel
            // 
            this.UpperMassLimitLabel.AutoSize = true;
            this.UpperMassLimitLabel.Location = new System.Drawing.Point(331, 44);
            this.UpperMassLimitLabel.Name = "UpperMassLimitLabel";
            this.UpperMassLimitLabel.Size = new System.Drawing.Size(127, 13);
            this.UpperMassLimitLabel.TabIndex = 33;
            this.UpperMassLimitLabel.Text = "Upper U235 mass limit (g)";
            // 
            // CovarABLabel
            // 
            this.CovarABLabel.AutoSize = true;
            this.CovarABLabel.Location = new System.Drawing.Point(388, 91);
            this.CovarABLabel.Name = "CovarABLabel";
            this.CovarABLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarABLabel.TabIndex = 34;
            this.CovarABLabel.Text = "Covariance ab";
            // 
            // CovarACLabel
            // 
            this.CovarACLabel.AutoSize = true;
            this.CovarACLabel.Location = new System.Drawing.Point(388, 117);
            this.CovarACLabel.Name = "CovarACLabel";
            this.CovarACLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarACLabel.TabIndex = 35;
            this.CovarACLabel.Text = "Covariance ac";
            // 
            // CovarADLabel
            // 
            this.CovarADLabel.AutoSize = true;
            this.CovarADLabel.Location = new System.Drawing.Point(388, 143);
            this.CovarADLabel.Name = "CovarADLabel";
            this.CovarADLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarADLabel.TabIndex = 36;
            this.CovarADLabel.Text = "Covariance ad";
            // 
            // CovarBCLabel
            // 
            this.CovarBCLabel.AutoSize = true;
            this.CovarBCLabel.Location = new System.Drawing.Point(388, 169);
            this.CovarBCLabel.Name = "CovarBCLabel";
            this.CovarBCLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarBCLabel.TabIndex = 37;
            this.CovarBCLabel.Text = "Covariance bc";
            // 
            // CovarBDLabel
            // 
            this.CovarBDLabel.AutoSize = true;
            this.CovarBDLabel.Location = new System.Drawing.Point(388, 195);
            this.CovarBDLabel.Name = "CovarBDLabel";
            this.CovarBDLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarBDLabel.TabIndex = 38;
            this.CovarBDLabel.Text = "Covariance bd";
            // 
            // CovarCDLabel
            // 
            this.CovarCDLabel.AutoSize = true;
            this.CovarCDLabel.Location = new System.Drawing.Point(388, 221);
            this.CovarCDLabel.Name = "CovarCDLabel";
            this.CovarCDLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarCDLabel.TabIndex = 39;
            this.CovarCDLabel.Text = "Covariance cd";
            // 
            // SigmaXLabel
            // 
            this.SigmaXLabel.AutoSize = true;
            this.SigmaXLabel.Location = new System.Drawing.Point(403, 247);
            this.SigmaXLabel.Name = "SigmaXLabel";
            this.SigmaXLabel.Size = new System.Drawing.Size(61, 13);
            this.SigmaXLabel.TabIndex = 40;
            this.SigmaXLabel.Text = "Sigma x (%)";
            // 
            // CovarianceCDTextBox
            // 
            this.CovarianceCDTextBox.Location = new System.Drawing.Point(470, 218);
            this.CovarianceCDTextBox.Max = 1.7976931348623157E+308D;
            this.CovarianceCDTextBox.Min = 0D;
            this.CovarianceCDTextBox.Name = "CovarianceCDTextBox";
            this.CovarianceCDTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.CovarianceCDTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CovarianceCDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceCDTextBox.Steps = -1D;
            this.CovarianceCDTextBox.TabIndex = 16;
            this.CovarianceCDTextBox.Text = "0.000000E+000";
            this.CovarianceCDTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.CovarianceCDTextBox.Value = 0D;
            // 
            // SigmaXTextBox
            // 
            this.SigmaXTextBox.Location = new System.Drawing.Point(470, 244);
            this.SigmaXTextBox.Max = 1.7976931348623157E+308D;
            this.SigmaXTextBox.Min = 0D;
            this.SigmaXTextBox.Name = "SigmaXTextBox";
            this.SigmaXTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.SigmaXTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.SigmaXTextBox.Size = new System.Drawing.Size(100, 20);
            this.SigmaXTextBox.Steps = -1D;
            this.SigmaXTextBox.TabIndex = 17;
            this.SigmaXTextBox.Text = "0.000000E+000";
            this.SigmaXTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.SigmaXTextBox.Value = 0D;
            // 
            // CovarianceBDTextBox
            // 
            this.CovarianceBDTextBox.Location = new System.Drawing.Point(470, 192);
            this.CovarianceBDTextBox.Max = 1.7976931348623157E+308D;
            this.CovarianceBDTextBox.Min = 0D;
            this.CovarianceBDTextBox.Name = "CovarianceBDTextBox";
            this.CovarianceBDTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.CovarianceBDTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CovarianceBDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceBDTextBox.Steps = -1D;
            this.CovarianceBDTextBox.TabIndex = 15;
            this.CovarianceBDTextBox.Text = "0.000000E+000";
            this.CovarianceBDTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.CovarianceBDTextBox.Value = 0D;
            // 
            // CovarianceBCTextBox
            // 
            this.CovarianceBCTextBox.Location = new System.Drawing.Point(470, 166);
            this.CovarianceBCTextBox.Max = 1.7976931348623157E+308D;
            this.CovarianceBCTextBox.Min = 0D;
            this.CovarianceBCTextBox.Name = "CovarianceBCTextBox";
            this.CovarianceBCTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.CovarianceBCTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CovarianceBCTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceBCTextBox.Steps = -1D;
            this.CovarianceBCTextBox.TabIndex = 14;
            this.CovarianceBCTextBox.Text = "0.000000E+000";
            this.CovarianceBCTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.CovarianceBCTextBox.Value = 0D;
            // 
            // CovarianceADTextBox
            // 
            this.CovarianceADTextBox.Location = new System.Drawing.Point(470, 140);
            this.CovarianceADTextBox.Max = 1.7976931348623157E+308D;
            this.CovarianceADTextBox.Min = 0D;
            this.CovarianceADTextBox.Name = "CovarianceADTextBox";
            this.CovarianceADTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.CovarianceADTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CovarianceADTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceADTextBox.Steps = -1D;
            this.CovarianceADTextBox.TabIndex = 13;
            this.CovarianceADTextBox.Text = "0.000000E+000";
            this.CovarianceADTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.CovarianceADTextBox.Value = 0D;
            // 
            // CovarianceACTextBox
            // 
            this.CovarianceACTextBox.Location = new System.Drawing.Point(470, 114);
            this.CovarianceACTextBox.Max = 1.7976931348623157E+308D;
            this.CovarianceACTextBox.Min = 0D;
            this.CovarianceACTextBox.Name = "CovarianceACTextBox";
            this.CovarianceACTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.CovarianceACTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CovarianceACTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceACTextBox.Steps = -1D;
            this.CovarianceACTextBox.TabIndex = 12;
            this.CovarianceACTextBox.Text = "0.000000E+000";
            this.CovarianceACTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.CovarianceACTextBox.Value = 0D;
            // 
            // CovarianceABTextBox
            // 
            this.CovarianceABTextBox.Location = new System.Drawing.Point(470, 88);
            this.CovarianceABTextBox.Max = 1.7976931348623157E+308D;
            this.CovarianceABTextBox.Min = 0D;
            this.CovarianceABTextBox.Name = "CovarianceABTextBox";
            this.CovarianceABTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.CovarianceABTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CovarianceABTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceABTextBox.Steps = -1D;
            this.CovarianceABTextBox.TabIndex = 11;
            this.CovarianceABTextBox.Text = "0.000000E+000";
            this.CovarianceABTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.CovarianceABTextBox.Value = 0D;
            // 
            // UpperMassLimitTextBox
            // 
            this.UpperMassLimitTextBox.Location = new System.Drawing.Point(470, 41);
            this.UpperMassLimitTextBox.Max = 1.7976931348623157E+308D;
            this.UpperMassLimitTextBox.Min = 0D;
            this.UpperMassLimitTextBox.Name = "UpperMassLimitTextBox";
            this.UpperMassLimitTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.F3;
            this.UpperMassLimitTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.UpperMassLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.UpperMassLimitTextBox.Steps = -1D;
            this.UpperMassLimitTextBox.TabIndex = 10;
            this.UpperMassLimitTextBox.Text = "0.000";
            this.UpperMassLimitTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.UpperMassLimitTextBox.Value = 0D;
            // 
            // LowerMassLimitTextBox
            // 
            this.LowerMassLimitTextBox.Location = new System.Drawing.Point(470, 15);
            this.LowerMassLimitTextBox.Max = 1.7976931348623157E+308D;
            this.LowerMassLimitTextBox.Min = 0D;
            this.LowerMassLimitTextBox.Name = "LowerMassLimitTextBox";
            this.LowerMassLimitTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.F3;
            this.LowerMassLimitTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.LowerMassLimitTextBox.Size = new System.Drawing.Size(100, 20);
            this.LowerMassLimitTextBox.Steps = -1D;
            this.LowerMassLimitTextBox.TabIndex = 9;
            this.LowerMassLimitTextBox.Text = "0.000";
            this.LowerMassLimitTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.LowerMassLimitTextBox.Value = 0D;
            // 
            // VarianceDTextBox
            // 
            this.VarianceDTextBox.Location = new System.Drawing.Point(254, 166);
            this.VarianceDTextBox.Max = 1.7976931348623157E+308D;
            this.VarianceDTextBox.Min = 0D;
            this.VarianceDTextBox.Name = "VarianceDTextBox";
            this.VarianceDTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.VarianceDTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.VarianceDTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceDTextBox.Steps = -1D;
            this.VarianceDTextBox.TabIndex = 8;
            this.VarianceDTextBox.Text = "0.000000E+000";
            this.VarianceDTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.VarianceDTextBox.Value = 0D;
            // 
            // VarianceCTextBox
            // 
            this.VarianceCTextBox.Location = new System.Drawing.Point(254, 140);
            this.VarianceCTextBox.Max = 1.7976931348623157E+308D;
            this.VarianceCTextBox.Min = 0D;
            this.VarianceCTextBox.Name = "VarianceCTextBox";
            this.VarianceCTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.VarianceCTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.VarianceCTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceCTextBox.Steps = -1D;
            this.VarianceCTextBox.TabIndex = 6;
            this.VarianceCTextBox.Text = "0.000000E+000";
            this.VarianceCTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.VarianceCTextBox.Value = 0D;
            // 
            // VarianceBTextBox
            // 
            this.VarianceBTextBox.Location = new System.Drawing.Point(254, 114);
            this.VarianceBTextBox.Max = 1.7976931348623157E+308D;
            this.VarianceBTextBox.Min = 0D;
            this.VarianceBTextBox.Name = "VarianceBTextBox";
            this.VarianceBTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.VarianceBTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.VarianceBTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceBTextBox.Steps = -1D;
            this.VarianceBTextBox.TabIndex = 4;
            this.VarianceBTextBox.Text = "0.000000E+000";
            this.VarianceBTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.VarianceBTextBox.Value = 0D;
            // 
            // VarianceATextBox
            // 
            this.VarianceATextBox.Location = new System.Drawing.Point(254, 88);
            this.VarianceATextBox.Max = 1.7976931348623157E+308D;
            this.VarianceATextBox.Min = 0D;
            this.VarianceATextBox.Name = "VarianceATextBox";
            this.VarianceATextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.VarianceATextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.VarianceATextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceATextBox.Steps = -1D;
            this.VarianceATextBox.TabIndex = 2;
            this.VarianceATextBox.Text = "0.000000E+000";
            this.VarianceATextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.VarianceATextBox.Value = 0D;
            // 
            // DTextBox
            // 
            this.DTextBox.Location = new System.Drawing.Point(44, 166);
            this.DTextBox.Max = 1.7976931348623157E+308D;
            this.DTextBox.Min = 0D;
            this.DTextBox.Name = "DTextBox";
            this.DTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.DTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.DTextBox.Size = new System.Drawing.Size(100, 20);
            this.DTextBox.Steps = -1D;
            this.DTextBox.TabIndex = 7;
            this.DTextBox.Text = "0.000000E+000";
            this.DTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.DTextBox.Value = 0D;
            // 
            // CTextBox
            // 
            this.CTextBox.Location = new System.Drawing.Point(44, 140);
            this.CTextBox.Max = 1.7976931348623157E+308D;
            this.CTextBox.Min = 0D;
            this.CTextBox.Name = "CTextBox";
            this.CTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.CTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CTextBox.Size = new System.Drawing.Size(100, 20);
            this.CTextBox.Steps = -1D;
            this.CTextBox.TabIndex = 5;
            this.CTextBox.Text = "0.000000E+000";
            this.CTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.CTextBox.Value = 0D;
            // 
            // BTextBox
            // 
            this.BTextBox.Location = new System.Drawing.Point(44, 114);
            this.BTextBox.Max = 1.7976931348623157E+308D;
            this.BTextBox.Min = 0D;
            this.BTextBox.Name = "BTextBox";
            this.BTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.BTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.BTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTextBox.Steps = -1D;
            this.BTextBox.TabIndex = 3;
            this.BTextBox.Text = "0.000000E+000";
            this.BTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.BTextBox.Value = 0D;
            // 
            // ATextBox
            // 
            this.ATextBox.Location = new System.Drawing.Point(44, 88);
            this.ATextBox.Max = 1.7976931348623157E+308D;
            this.ATextBox.Min = 0D;
            this.ATextBox.Name = "ATextBox";
            this.ATextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.ATextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.ATextBox.Size = new System.Drawing.Size(100, 20);
            this.ATextBox.Steps = -1D;
            this.ATextBox.TabIndex = 1;
            this.ATextBox.Text = "0.000000E+000";
            this.ATextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.ATextBox.Value = 0D;
            // 
            // BackBtn
            // 
            this.BackBtn.Location = new System.Drawing.Point(606, 27);
            this.BackBtn.Name = "BackBtn";
            this.BackBtn.Size = new System.Drawing.Size(75, 23);
            this.BackBtn.TabIndex = 41;
            this.BackBtn.Text = "Back";
            this.BackBtn.UseVisualStyleBackColor = true;
            this.BackBtn.Click += new System.EventHandler(this.BackBtn_Click);
            // 
            // IDDCollarCal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 279);
            this.Controls.Add(this.BackBtn);
            this.Controls.Add(this.SigmaXLabel);
            this.Controls.Add(this.CovarCDLabel);
            this.Controls.Add(this.CovarBDLabel);
            this.Controls.Add(this.CovarBCLabel);
            this.Controls.Add(this.CovarADLabel);
            this.Controls.Add(this.CovarACLabel);
            this.Controls.Add(this.CovarABLabel);
            this.Controls.Add(this.UpperMassLimitLabel);
            this.Controls.Add(this.LowerMassLimitLabel);
            this.Controls.Add(this.VarDLabel);
            this.Controls.Add(this.VarClabel);
            this.Controls.Add(this.VarBLabel);
            this.Controls.Add(this.VarALabel);
            this.Controls.Add(this.DLabel);
            this.Controls.Add(this.CLabel);
            this.Controls.Add(this.BLabel);
            this.Controls.Add(this.ALabel);
            this.Controls.Add(this.CurveTypeLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.CovarianceCDTextBox);
            this.Controls.Add(this.SigmaXTextBox);
            this.Controls.Add(this.CovarianceBDTextBox);
            this.Controls.Add(this.CovarianceBCTextBox);
            this.Controls.Add(this.CovarianceADTextBox);
            this.Controls.Add(this.CovarianceACTextBox);
            this.Controls.Add(this.CovarianceABTextBox);
            this.Controls.Add(this.UpperMassLimitTextBox);
            this.Controls.Add(this.LowerMassLimitTextBox);
            this.Controls.Add(this.VarianceDTextBox);
            this.Controls.Add(this.VarianceCTextBox);
            this.Controls.Add(this.VarianceBTextBox);
            this.Controls.Add(this.VarianceATextBox);
            this.Controls.Add(this.DTextBox);
            this.Controls.Add(this.CTextBox);
            this.Controls.Add(this.BTextBox);
            this.Controls.Add(this.ATextBox);
            this.Controls.Add(this.CurveTypeComboBox);
            this.Name = "IDDCollarCal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Collar Calibration";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CurveTypeComboBox;
        private NumericTextBox ATextBox;
        private NumericTextBox BTextBox;
        private NumericTextBox CTextBox;
        private NumericTextBox DTextBox;
        private NumericTextBox VarianceATextBox;
        private NumericTextBox VarianceBTextBox;
        private NumericTextBox VarianceCTextBox;
        private NumericTextBox VarianceDTextBox;
        private NumericTextBox LowerMassLimitTextBox;
        private NumericTextBox UpperMassLimitTextBox;
        private NumericTextBox CovarianceABTextBox;
        private NumericTextBox CovarianceACTextBox;
        private NumericTextBox CovarianceADTextBox;
        private NumericTextBox CovarianceBCTextBox;
        private NumericTextBox CovarianceBDTextBox;
        private NumericTextBox SigmaXTextBox;
        private NumericTextBox CovarianceCDTextBox;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label CurveTypeLabel;
        private System.Windows.Forms.Label ALabel;
        private System.Windows.Forms.Label BLabel;
        private System.Windows.Forms.Label CLabel;
        private System.Windows.Forms.Label DLabel;
        private System.Windows.Forms.Label VarALabel;
        private System.Windows.Forms.Label VarBLabel;
        private System.Windows.Forms.Label VarClabel;
        private System.Windows.Forms.Label VarDLabel;
        private System.Windows.Forms.Label LowerMassLimitLabel;
        private System.Windows.Forms.Label UpperMassLimitLabel;
        private System.Windows.Forms.Label CovarABLabel;
        private System.Windows.Forms.Label CovarACLabel;
        private System.Windows.Forms.Label CovarADLabel;
        private System.Windows.Forms.Label CovarBCLabel;
        private System.Windows.Forms.Label CovarBDLabel;
        private System.Windows.Forms.Label CovarCDLabel;
        private System.Windows.Forms.Label SigmaXLabel;
        private System.Windows.Forms.Button BackBtn;
        private System.Windows.Forms.HelpProvider provider;
    }
}