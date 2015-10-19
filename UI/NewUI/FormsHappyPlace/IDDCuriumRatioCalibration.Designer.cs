namespace NewUI
{
    partial class IDDCuriumRatioCalibration
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
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.CurveTypeComboBox = new System.Windows.Forms.ComboBox();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UseAddASourceRadioButton = new System.Windows.Forms.RadioButton();
            this.UseDoublesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseSinglesRadioButton = new System.Windows.Forms.RadioButton();
            this.LowerPuMassLimitTextBox = new System.Windows.Forms.TextBox();
            this.UpperPuMassLimitTextBox = new System.Windows.Forms.TextBox();
            this.ATextBox = new System.Windows.Forms.TextBox();
            this.BTextBox = new System.Windows.Forms.TextBox();
            this.CTextBox = new System.Windows.Forms.TextBox();
            this.DTextBox = new System.Windows.Forms.TextBox();
            this.VarianceATextBox = new System.Windows.Forms.TextBox();
            this.VarianceBTextBox = new System.Windows.Forms.TextBox();
            this.VarianceCTextBox = new System.Windows.Forms.TextBox();
            this.VarianceDTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceABTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceACTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceADTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceBDTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceCDTextBox = new System.Windows.Forms.TextBox();
            this.SigmaXTextBox = new System.Windows.Forms.TextBox();
            this.CovarianceBCTextBox = new System.Windows.Forms.TextBox();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.CurveTypeLabel = new System.Windows.Forms.Label();
            this.LowerPuMassLimitLabel = new System.Windows.Forms.Label();
            this.UpperPuMassLimitLabel = new System.Windows.Forms.Label();
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
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(94, 20);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(240, 21);
            this.MaterialTypeComboBox.TabIndex = 0;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // CurveTypeComboBox
            // 
            this.CurveTypeComboBox.FormattingEnabled = true;
            this.CurveTypeComboBox.Location = new System.Drawing.Point(82, 184);
            this.CurveTypeComboBox.Name = "CurveTypeComboBox";
            this.CurveTypeComboBox.Size = new System.Drawing.Size(252, 21);
            this.CurveTypeComboBox.TabIndex = 1;
            this.CurveTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.CurveTypeComboBox_SelectedIndexChanged);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(399, 18);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(178, 23);
            this.PrintBtn.TabIndex = 2;
            this.PrintBtn.Text = "Print calibration parameters";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(604, 18);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(604, 47);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(604, 76);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 5;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UseAddASourceRadioButton);
            this.groupBox1.Controls.Add(this.UseDoublesRadioButton);
            this.groupBox1.Controls.Add(this.UseSinglesRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(24, 51);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(310, 107);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // UseAddASourceRadioButton
            // 
            this.UseAddASourceRadioButton.AutoSize = true;
            this.UseAddASourceRadioButton.Location = new System.Drawing.Point(15, 70);
            this.UseAddASourceRadioButton.Name = "UseAddASourceRadioButton";
            this.UseAddASourceRadioButton.Size = new System.Drawing.Size(282, 17);
            this.UseAddASourceRadioButton.TabIndex = 2;
            this.UseAddASourceRadioButton.TabStop = true;
            this.UseAddASourceRadioButton.Text = "Use add-a-source corrected doubles to calculate mass";
            this.UseAddASourceRadioButton.UseVisualStyleBackColor = true;
            this.UseAddASourceRadioButton.CheckedChanged += new System.EventHandler(this.UseAddASourceRadioButton_CheckedChanged);
            // 
            // UseDoublesRadioButton
            // 
            this.UseDoublesRadioButton.AutoSize = true;
            this.UseDoublesRadioButton.Location = new System.Drawing.Point(15, 47);
            this.UseDoublesRadioButton.Name = "UseDoublesRadioButton";
            this.UseDoublesRadioButton.Size = new System.Drawing.Size(169, 17);
            this.UseDoublesRadioButton.TabIndex = 1;
            this.UseDoublesRadioButton.TabStop = true;
            this.UseDoublesRadioButton.Text = "Use doubles to calculate mass";
            this.UseDoublesRadioButton.UseVisualStyleBackColor = true;
            this.UseDoublesRadioButton.CheckedChanged += new System.EventHandler(this.UseDoublesRadioButton_CheckedChanged);
            // 
            // UseSinglesRadioButton
            // 
            this.UseSinglesRadioButton.AutoSize = true;
            this.UseSinglesRadioButton.Location = new System.Drawing.Point(15, 24);
            this.UseSinglesRadioButton.Name = "UseSinglesRadioButton";
            this.UseSinglesRadioButton.Size = new System.Drawing.Size(164, 17);
            this.UseSinglesRadioButton.TabIndex = 0;
            this.UseSinglesRadioButton.TabStop = true;
            this.UseSinglesRadioButton.Text = "Use singles to calculate mass";
            this.UseSinglesRadioButton.UseVisualStyleBackColor = true;
            this.UseSinglesRadioButton.CheckedChanged += new System.EventHandler(this.UseSinglesRadioButton_CheckedChanged);
            // 
            // LowerPuMassLimitTextBox
            // 
            this.LowerPuMassLimitTextBox.Location = new System.Drawing.Point(508, 72);
            this.LowerPuMassLimitTextBox.Name = "LowerPuMassLimitTextBox";
            this.LowerPuMassLimitTextBox.Size = new System.Drawing.Size(69, 20);
            this.LowerPuMassLimitTextBox.TabIndex = 7;
            this.LowerPuMassLimitTextBox.TextChanged += new System.EventHandler(this.LowerPuMassLimitTextBox_TextChanged);
            // 
            // UpperPuMassLimitTextBox
            // 
            this.UpperPuMassLimitTextBox.Location = new System.Drawing.Point(508, 98);
            this.UpperPuMassLimitTextBox.Name = "UpperPuMassLimitTextBox";
            this.UpperPuMassLimitTextBox.Size = new System.Drawing.Size(69, 20);
            this.UpperPuMassLimitTextBox.TabIndex = 8;
            this.UpperPuMassLimitTextBox.TextChanged += new System.EventHandler(this.UpperPuMassLimitTextBox_TextChanged);
            // 
            // ATextBox
            // 
            this.ATextBox.Location = new System.Drawing.Point(40, 231);
            this.ATextBox.Name = "ATextBox";
            this.ATextBox.Size = new System.Drawing.Size(100, 20);
            this.ATextBox.TabIndex = 9;
            this.ATextBox.TextChanged += new System.EventHandler(this.ATextBox_TextChanged);
            // 
            // BTextBox
            // 
            this.BTextBox.Location = new System.Drawing.Point(40, 257);
            this.BTextBox.Name = "BTextBox";
            this.BTextBox.Size = new System.Drawing.Size(100, 20);
            this.BTextBox.TabIndex = 10;
            this.BTextBox.TextChanged += new System.EventHandler(this.BTextBox_TextChanged);
            // 
            // CTextBox
            // 
            this.CTextBox.Location = new System.Drawing.Point(40, 283);
            this.CTextBox.Name = "CTextBox";
            this.CTextBox.Size = new System.Drawing.Size(100, 20);
            this.CTextBox.TabIndex = 11;
            this.CTextBox.TextChanged += new System.EventHandler(this.CTextBox_TextChanged);
            // 
            // DTextBox
            // 
            this.DTextBox.Location = new System.Drawing.Point(40, 309);
            this.DTextBox.Name = "DTextBox";
            this.DTextBox.Size = new System.Drawing.Size(100, 20);
            this.DTextBox.TabIndex = 12;
            this.DTextBox.TextChanged += new System.EventHandler(this.DTextBox_TextChanged);
            // 
            // VarianceATextBox
            // 
            this.VarianceATextBox.Location = new System.Drawing.Point(247, 231);
            this.VarianceATextBox.Name = "VarianceATextBox";
            this.VarianceATextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceATextBox.TabIndex = 13;
            this.VarianceATextBox.TextChanged += new System.EventHandler(this.VarianceATextBox_TextChanged);
            // 
            // VarianceBTextBox
            // 
            this.VarianceBTextBox.Location = new System.Drawing.Point(247, 257);
            this.VarianceBTextBox.Name = "VarianceBTextBox";
            this.VarianceBTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceBTextBox.TabIndex = 14;
            this.VarianceBTextBox.TextChanged += new System.EventHandler(this.VarianceBTextBox_TextChanged);
            // 
            // VarianceCTextBox
            // 
            this.VarianceCTextBox.Location = new System.Drawing.Point(247, 283);
            this.VarianceCTextBox.Name = "VarianceCTextBox";
            this.VarianceCTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceCTextBox.TabIndex = 15;
            this.VarianceCTextBox.TextChanged += new System.EventHandler(this.VarianceCTextBox_TextChanged);
            // 
            // VarianceDTextBox
            // 
            this.VarianceDTextBox.Location = new System.Drawing.Point(246, 309);
            this.VarianceDTextBox.Name = "VarianceDTextBox";
            this.VarianceDTextBox.Size = new System.Drawing.Size(100, 20);
            this.VarianceDTextBox.TabIndex = 16;
            this.VarianceDTextBox.TextChanged += new System.EventHandler(this.VarianceDTextBox_TextChanged);
            // 
            // CovarianceABTextBox
            // 
            this.CovarianceABTextBox.Location = new System.Drawing.Point(477, 231);
            this.CovarianceABTextBox.Name = "CovarianceABTextBox";
            this.CovarianceABTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceABTextBox.TabIndex = 17;
            this.CovarianceABTextBox.TextChanged += new System.EventHandler(this.CovarianceABTextBox_TextChanged);
            // 
            // CovarianceACTextBox
            // 
            this.CovarianceACTextBox.Location = new System.Drawing.Point(477, 257);
            this.CovarianceACTextBox.Name = "CovarianceACTextBox";
            this.CovarianceACTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceACTextBox.TabIndex = 18;
            this.CovarianceACTextBox.TextChanged += new System.EventHandler(this.CovarianceACTextBox_TextChanged);
            // 
            // CovarianceADTextBox
            // 
            this.CovarianceADTextBox.Location = new System.Drawing.Point(477, 283);
            this.CovarianceADTextBox.Name = "CovarianceADTextBox";
            this.CovarianceADTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceADTextBox.TabIndex = 19;
            this.CovarianceADTextBox.TextChanged += new System.EventHandler(this.CovarianceADTextBox_TextChanged);
            // 
            // CovarianceBDTextBox
            // 
            this.CovarianceBDTextBox.Location = new System.Drawing.Point(477, 335);
            this.CovarianceBDTextBox.Name = "CovarianceBDTextBox";
            this.CovarianceBDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceBDTextBox.TabIndex = 21;
            this.CovarianceBDTextBox.TextChanged += new System.EventHandler(this.CovarianceBDTextBox_TextChanged);
            // 
            // CovarianceCDTextBox
            // 
            this.CovarianceCDTextBox.Location = new System.Drawing.Point(477, 361);
            this.CovarianceCDTextBox.Name = "CovarianceCDTextBox";
            this.CovarianceCDTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceCDTextBox.TabIndex = 22;
            this.CovarianceCDTextBox.TextChanged += new System.EventHandler(this.CovarianceCDTextBox_TextChanged);
            // 
            // SigmaXTextBox
            // 
            this.SigmaXTextBox.Location = new System.Drawing.Point(477, 387);
            this.SigmaXTextBox.Name = "SigmaXTextBox";
            this.SigmaXTextBox.Size = new System.Drawing.Size(100, 20);
            this.SigmaXTextBox.TabIndex = 23;
            this.SigmaXTextBox.TextChanged += new System.EventHandler(this.SigmaXTextBox_TextChanged);
            // 
            // CovarianceBCTextBox
            // 
            this.CovarianceBCTextBox.Location = new System.Drawing.Point(477, 309);
            this.CovarianceBCTextBox.Name = "CovarianceBCTextBox";
            this.CovarianceBCTextBox.Size = new System.Drawing.Size(100, 20);
            this.CovarianceBCTextBox.TabIndex = 24;
            this.CovarianceBCTextBox.TextChanged += new System.EventHandler(this.CovarianceBCTextBox_TextChanged);
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(21, 23);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 25;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // CurveTypeLabel
            // 
            this.CurveTypeLabel.AutoSize = true;
            this.CurveTypeLabel.Location = new System.Drawing.Point(21, 187);
            this.CurveTypeLabel.Name = "CurveTypeLabel";
            this.CurveTypeLabel.Size = new System.Drawing.Size(58, 13);
            this.CurveTypeLabel.TabIndex = 26;
            this.CurveTypeLabel.Text = "Curve type";
            // 
            // LowerPuMassLimitLabel
            // 
            this.LowerPuMassLimitLabel.AutoSize = true;
            this.LowerPuMassLimitLabel.Location = new System.Drawing.Point(352, 75);
            this.LowerPuMassLimitLabel.Name = "LowerPuMassLimitLabel";
            this.LowerPuMassLimitLabel.Size = new System.Drawing.Size(141, 13);
            this.LowerPuMassLimitLabel.TabIndex = 27;
            this.LowerPuMassLimitLabel.Text = "Lower Pu240e mass limig (g)";
            // 
            // UpperPuMassLimitLabel
            // 
            this.UpperPuMassLimitLabel.AutoSize = true;
            this.UpperPuMassLimitLabel.Location = new System.Drawing.Point(355, 101);
            this.UpperPuMassLimitLabel.Name = "UpperPuMassLimitLabel";
            this.UpperPuMassLimitLabel.Size = new System.Drawing.Size(138, 13);
            this.UpperPuMassLimitLabel.TabIndex = 28;
            this.UpperPuMassLimitLabel.Text = "Upper Pu240e mass limit (g)";
            // 
            // ALabel
            // 
            this.ALabel.AutoSize = true;
            this.ALabel.Location = new System.Drawing.Point(21, 234);
            this.ALabel.Name = "ALabel";
            this.ALabel.Size = new System.Drawing.Size(13, 13);
            this.ALabel.TabIndex = 29;
            this.ALabel.Text = "a";
            // 
            // BLabel
            // 
            this.BLabel.AutoSize = true;
            this.BLabel.Location = new System.Drawing.Point(21, 260);
            this.BLabel.Name = "BLabel";
            this.BLabel.Size = new System.Drawing.Size(13, 13);
            this.BLabel.TabIndex = 30;
            this.BLabel.Text = "b";
            // 
            // CLabel
            // 
            this.CLabel.AutoSize = true;
            this.CLabel.Location = new System.Drawing.Point(21, 286);
            this.CLabel.Name = "CLabel";
            this.CLabel.Size = new System.Drawing.Size(13, 13);
            this.CLabel.TabIndex = 31;
            this.CLabel.Text = "c";
            // 
            // DLabel
            // 
            this.DLabel.AutoSize = true;
            this.DLabel.Location = new System.Drawing.Point(21, 312);
            this.DLabel.Name = "DLabel";
            this.DLabel.Size = new System.Drawing.Size(13, 13);
            this.DLabel.TabIndex = 32;
            this.DLabel.Text = "d";
            // 
            // VarianceALabel
            // 
            this.VarianceALabel.AutoSize = true;
            this.VarianceALabel.Location = new System.Drawing.Point(183, 234);
            this.VarianceALabel.Name = "VarianceALabel";
            this.VarianceALabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceALabel.TabIndex = 33;
            this.VarianceALabel.Text = "Variance a";
            // 
            // VarianceBLabel
            // 
            this.VarianceBLabel.AutoSize = true;
            this.VarianceBLabel.Location = new System.Drawing.Point(183, 260);
            this.VarianceBLabel.Name = "VarianceBLabel";
            this.VarianceBLabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceBLabel.TabIndex = 34;
            this.VarianceBLabel.Text = "Variance b";
            // 
            // VarianceCLabel
            // 
            this.VarianceCLabel.AutoSize = true;
            this.VarianceCLabel.Location = new System.Drawing.Point(183, 286);
            this.VarianceCLabel.Name = "VarianceCLabel";
            this.VarianceCLabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceCLabel.TabIndex = 35;
            this.VarianceCLabel.Text = "Variance c";
            // 
            // VarianceDLabel
            // 
            this.VarianceDLabel.AutoSize = true;
            this.VarianceDLabel.Location = new System.Drawing.Point(182, 312);
            this.VarianceDLabel.Name = "VarianceDLabel";
            this.VarianceDLabel.Size = new System.Drawing.Size(58, 13);
            this.VarianceDLabel.TabIndex = 36;
            this.VarianceDLabel.Text = "Variance d";
            // 
            // CovarianceABLabel
            // 
            this.CovarianceABLabel.AutoSize = true;
            this.CovarianceABLabel.Location = new System.Drawing.Point(395, 234);
            this.CovarianceABLabel.Name = "CovarianceABLabel";
            this.CovarianceABLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceABLabel.TabIndex = 37;
            this.CovarianceABLabel.Text = "Covariance ab";
            // 
            // CovarianceACLabel
            // 
            this.CovarianceACLabel.AutoSize = true;
            this.CovarianceACLabel.Location = new System.Drawing.Point(395, 260);
            this.CovarianceACLabel.Name = "CovarianceACLabel";
            this.CovarianceACLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceACLabel.TabIndex = 38;
            this.CovarianceACLabel.Text = "Covariance ac";
            // 
            // CovarianceADLabel
            // 
            this.CovarianceADLabel.AutoSize = true;
            this.CovarianceADLabel.Location = new System.Drawing.Point(395, 286);
            this.CovarianceADLabel.Name = "CovarianceADLabel";
            this.CovarianceADLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceADLabel.TabIndex = 39;
            this.CovarianceADLabel.Text = "Covariance ad";
            // 
            // CovarianceBCLabel
            // 
            this.CovarianceBCLabel.AutoSize = true;
            this.CovarianceBCLabel.Location = new System.Drawing.Point(395, 312);
            this.CovarianceBCLabel.Name = "CovarianceBCLabel";
            this.CovarianceBCLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceBCLabel.TabIndex = 40;
            this.CovarianceBCLabel.Text = "Covariance bc";
            // 
            // CovarianceBDLabel
            // 
            this.CovarianceBDLabel.AutoSize = true;
            this.CovarianceBDLabel.Location = new System.Drawing.Point(395, 338);
            this.CovarianceBDLabel.Name = "CovarianceBDLabel";
            this.CovarianceBDLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceBDLabel.TabIndex = 41;
            this.CovarianceBDLabel.Text = "Covariance bd";
            // 
            // CovarianceCDLabel
            // 
            this.CovarianceCDLabel.AutoSize = true;
            this.CovarianceCDLabel.Location = new System.Drawing.Point(395, 364);
            this.CovarianceCDLabel.Name = "CovarianceCDLabel";
            this.CovarianceCDLabel.Size = new System.Drawing.Size(76, 13);
            this.CovarianceCDLabel.TabIndex = 42;
            this.CovarianceCDLabel.Text = "Covariance cd";
            // 
            // SigmaXLabel
            // 
            this.SigmaXLabel.AutoSize = true;
            this.SigmaXLabel.Location = new System.Drawing.Point(410, 390);
            this.SigmaXLabel.Name = "SigmaXLabel";
            this.SigmaXLabel.Size = new System.Drawing.Size(61, 13);
            this.SigmaXLabel.TabIndex = 43;
            this.SigmaXLabel.Text = "Sigma x (%)";
            // 
            // IDDCuriumRatioCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 424);
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
            this.Controls.Add(this.UpperPuMassLimitLabel);
            this.Controls.Add(this.LowerPuMassLimitLabel);
            this.Controls.Add(this.CurveTypeLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.CovarianceBCTextBox);
            this.Controls.Add(this.SigmaXTextBox);
            this.Controls.Add(this.CovarianceCDTextBox);
            this.Controls.Add(this.CovarianceBDTextBox);
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
            this.Controls.Add(this.UpperPuMassLimitTextBox);
            this.Controls.Add(this.LowerPuMassLimitTextBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.CurveTypeComboBox);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Name = "IDDCuriumRatioCalibration";
            this.Text = "Curium Ratio Calibration";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox CurveTypeComboBox;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton UseAddASourceRadioButton;
        private System.Windows.Forms.RadioButton UseDoublesRadioButton;
        private System.Windows.Forms.RadioButton UseSinglesRadioButton;
        private System.Windows.Forms.TextBox LowerPuMassLimitTextBox;
        private System.Windows.Forms.TextBox UpperPuMassLimitTextBox;
        private System.Windows.Forms.TextBox ATextBox;
        private System.Windows.Forms.TextBox BTextBox;
        private System.Windows.Forms.TextBox CTextBox;
        private System.Windows.Forms.TextBox DTextBox;
        private System.Windows.Forms.TextBox VarianceATextBox;
        private System.Windows.Forms.TextBox VarianceBTextBox;
        private System.Windows.Forms.TextBox VarianceCTextBox;
        private System.Windows.Forms.TextBox VarianceDTextBox;
        private System.Windows.Forms.TextBox CovarianceABTextBox;
        private System.Windows.Forms.TextBox CovarianceACTextBox;
        private System.Windows.Forms.TextBox CovarianceADTextBox;
        private System.Windows.Forms.TextBox CovarianceBDTextBox;
        private System.Windows.Forms.TextBox CovarianceCDTextBox;
        private System.Windows.Forms.TextBox SigmaXTextBox;
        private System.Windows.Forms.TextBox CovarianceBCTextBox;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label CurveTypeLabel;
        private System.Windows.Forms.Label LowerPuMassLimitLabel;
        private System.Windows.Forms.Label UpperPuMassLimitLabel;
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
    }
}