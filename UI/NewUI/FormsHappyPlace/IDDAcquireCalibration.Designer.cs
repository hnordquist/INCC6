namespace NewUI
{
    partial class IDDAcquireCalibration
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
            this.ItemIdComboBox = new System.Windows.Forms.ComboBox();
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.DataSourceComboBox = new System.Windows.Forms.ComboBox();
            this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.CommentAtEndCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UseTriplesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseDoublesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseNumCyclesRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.IsotopicsBtn = new System.Windows.Forms.Button();
            this.CompositeIsotopicsBtn = new System.Windows.Forms.Button();
            this.Pu240eCoeffBtn = new System.Windows.Forms.Button();
            this.ItemIdLabel = new System.Windows.Forms.Label();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.CountTimeLabel = new System.Windows.Forms.Label();
            this.DeclaredMassLabel = new System.Windows.Forms.Label();
            this.NumCyclesLabel = new System.Windows.Forms.Label();
            this.MeasPrecisionLabel = new System.Windows.Forms.Label();
            this.MinNumCyclesLabel = new System.Windows.Forms.Label();
            this.MaxNumCyclesLabel = new System.Windows.Forms.Label();
            this.DataSourceLabel = new System.Windows.Forms.Label();
            this.MaxNumCyclesTextBox = new NewUI.NumericTextBox();
            this.MinNumCyclesTextBox = new NewUI.NumericTextBox();
            this.MeasPrecisionTextBox = new NewUI.NumericTextBox();
            this.NumCyclesTextBox = new NewUI.NumericTextBox();
            this.DeclaredMassTextBox = new NewUI.NumericTextBox();
            this.CountTimeTextBox = new NewUI.NumericTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemIdComboBox
            // 
            this.ItemIdComboBox.FormattingEnabled = true;
            this.ItemIdComboBox.Location = new System.Drawing.Point(157, 14);
            this.ItemIdComboBox.Name = "ItemIdComboBox";
            this.ItemIdComboBox.Size = new System.Drawing.Size(121, 21);
            this.ItemIdComboBox.TabIndex = 0;
            this.ItemIdComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemIdComboBox_SelectedIndexChanged);
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(157, 41);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.MaterialTypeComboBox.TabIndex = 1;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(157, 94);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(100, 20);
            this.CommentTextBox.TabIndex = 2;
            this.CommentTextBox.Leave += new System.EventHandler(this.CommentTextBox_Leave);
            // 
            // DataSourceComboBox
            // 
            this.DataSourceComboBox.FormattingEnabled = true;
            this.DataSourceComboBox.Location = new System.Drawing.Point(384, 332);
            this.DataSourceComboBox.Name = "DataSourceComboBox";
            this.DataSourceComboBox.Size = new System.Drawing.Size(140, 21);
            this.DataSourceComboBox.TabIndex = 9;
            this.DataSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.DataSourceComboBox_SelectedIndexChanged);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(157, 354);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 10;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(157, 377);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 11;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // CommentAtEndCheckBox
            // 
            this.CommentAtEndCheckBox.AutoSize = true;
            this.CommentAtEndCheckBox.Location = new System.Drawing.Point(157, 400);
            this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
            this.CommentAtEndCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndCheckBox.TabIndex = 12;
            this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndCheckBox_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UseTriplesRadioButton);
            this.groupBox1.Controls.Add(this.UseDoublesRadioButton);
            this.groupBox1.Controls.Add(this.UseNumCyclesRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(157, 146);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 98);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            // 
            // UseTriplesRadioButton
            // 
            this.UseTriplesRadioButton.AutoSize = true;
            this.UseTriplesRadioButton.Location = new System.Drawing.Point(27, 65);
            this.UseTriplesRadioButton.Name = "UseTriplesRadioButton";
            this.UseTriplesRadioButton.Size = new System.Drawing.Size(185, 17);
            this.UseTriplesRadioButton.TabIndex = 2;
            this.UseTriplesRadioButton.TabStop = true;
            this.UseTriplesRadioButton.Text = "Use triples measurement precision";
            this.UseTriplesRadioButton.UseVisualStyleBackColor = true;
            this.UseTriplesRadioButton.CheckedChanged += new System.EventHandler(this.UseTriplesRadioButton_CheckedChanged);
            // 
            // UseDoublesRadioButton
            // 
            this.UseDoublesRadioButton.AutoSize = true;
            this.UseDoublesRadioButton.Location = new System.Drawing.Point(27, 42);
            this.UseDoublesRadioButton.Name = "UseDoublesRadioButton";
            this.UseDoublesRadioButton.Size = new System.Drawing.Size(195, 17);
            this.UseDoublesRadioButton.TabIndex = 1;
            this.UseDoublesRadioButton.TabStop = true;
            this.UseDoublesRadioButton.Text = "Use doubles measurement precision";
            this.UseDoublesRadioButton.UseVisualStyleBackColor = true;
            this.UseDoublesRadioButton.CheckedChanged += new System.EventHandler(this.UseDoublesRadioButton_CheckedChanged);
            // 
            // UseNumCyclesRadioButton
            // 
            this.UseNumCyclesRadioButton.AutoSize = true;
            this.UseNumCyclesRadioButton.Location = new System.Drawing.Point(27, 19);
            this.UseNumCyclesRadioButton.Name = "UseNumCyclesRadioButton";
            this.UseNumCyclesRadioButton.Size = new System.Drawing.Size(127, 17);
            this.UseNumCyclesRadioButton.TabIndex = 0;
            this.UseNumCyclesRadioButton.TabStop = true;
            this.UseNumCyclesRadioButton.Text = "Use number of cycles";
            this.UseNumCyclesRadioButton.UseVisualStyleBackColor = true;
            this.UseNumCyclesRadioButton.CheckedChanged += new System.EventHandler(this.UseNumCyclesRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(449, 12);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 14;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(449, 41);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 15;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(449, 70);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 16;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IsotopicsBtn
            // 
            this.IsotopicsBtn.Location = new System.Drawing.Point(384, 274);
            this.IsotopicsBtn.Name = "IsotopicsBtn";
            this.IsotopicsBtn.Size = new System.Drawing.Size(140, 23);
            this.IsotopicsBtn.TabIndex = 17;
            this.IsotopicsBtn.Text = "Isotopics...";
            this.IsotopicsBtn.UseVisualStyleBackColor = true;
            this.IsotopicsBtn.Click += new System.EventHandler(this.IsotopicsBtn_Click);
            // 
            // CompositeIsotopicsBtn
            // 
            this.CompositeIsotopicsBtn.Enabled = false;
            this.CompositeIsotopicsBtn.Location = new System.Drawing.Point(384, 303);
            this.CompositeIsotopicsBtn.Name = "CompositeIsotopicsBtn";
            this.CompositeIsotopicsBtn.Size = new System.Drawing.Size(140, 23);
            this.CompositeIsotopicsBtn.TabIndex = 18;
            this.CompositeIsotopicsBtn.Text = "Composite Isotopics...";
            this.CompositeIsotopicsBtn.UseVisualStyleBackColor = true;
            this.CompositeIsotopicsBtn.Click += new System.EventHandler(this.CompositeIsotopicsBtn_Click);
            // 
            // Pu240eCoeffBtn
            // 
            this.Pu240eCoeffBtn.Enabled = false;
            this.Pu240eCoeffBtn.Location = new System.Drawing.Point(384, 359);
            this.Pu240eCoeffBtn.Name = "Pu240eCoeffBtn";
            this.Pu240eCoeffBtn.Size = new System.Drawing.Size(140, 23);
            this.Pu240eCoeffBtn.TabIndex = 19;
            this.Pu240eCoeffBtn.Text = "Pu240e Coeff...";
            this.Pu240eCoeffBtn.UseVisualStyleBackColor = true;
            this.Pu240eCoeffBtn.Click += new System.EventHandler(this.Pu240eCoeffBtn_Click);
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.AutoSize = true;
            this.ItemIdLabel.Location = new System.Drawing.Point(106, 17);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(38, 13);
            this.ItemIdLabel.TabIndex = 20;
            this.ItemIdLabel.Text = "Item id";
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(77, 44);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 21;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(93, 97);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 22;
            this.CommentLabel.Text = "Comment";
            // 
            // CountTimeLabel
            // 
            this.CountTimeLabel.AutoSize = true;
            this.CountTimeLabel.Location = new System.Drawing.Point(56, 123);
            this.CountTimeLabel.Name = "CountTimeLabel";
            this.CountTimeLabel.Size = new System.Drawing.Size(88, 13);
            this.CountTimeLabel.TabIndex = 23;
            this.CountTimeLabel.Text = "Count time (secs)";
            // 
            // DeclaredMassLabel
            // 
            this.DeclaredMassLabel.AutoSize = true;
            this.DeclaredMassLabel.Location = new System.Drawing.Point(52, 71);
            this.DeclaredMassLabel.Name = "DeclaredMassLabel";
            this.DeclaredMassLabel.Size = new System.Drawing.Size(92, 13);
            this.DeclaredMassLabel.TabIndex = 24;
            this.DeclaredMassLabel.Text = "Declared mass (g)";
            // 
            // NumCyclesLabel
            // 
            this.NumCyclesLabel.AutoSize = true;
            this.NumCyclesLabel.Location = new System.Drawing.Point(55, 253);
            this.NumCyclesLabel.Name = "NumCyclesLabel";
            this.NumCyclesLabel.Size = new System.Drawing.Size(89, 13);
            this.NumCyclesLabel.TabIndex = 25;
            this.NumCyclesLabel.Text = "Number of cycles";
            // 
            // MeasPrecisionLabel
            // 
            this.MeasPrecisionLabel.AutoSize = true;
            this.MeasPrecisionLabel.Location = new System.Drawing.Point(11, 279);
            this.MeasPrecisionLabel.Name = "MeasPrecisionLabel";
            this.MeasPrecisionLabel.Size = new System.Drawing.Size(133, 13);
            this.MeasPrecisionLabel.TabIndex = 26;
            this.MeasPrecisionLabel.Text = "Measurement precision (%)";
            // 
            // MinNumCyclesLabel
            // 
            this.MinNumCyclesLabel.AutoSize = true;
            this.MinNumCyclesLabel.Location = new System.Drawing.Point(49, 305);
            this.MinNumCyclesLabel.Name = "MinNumCyclesLabel";
            this.MinNumCyclesLabel.Size = new System.Drawing.Size(95, 13);
            this.MinNumCyclesLabel.TabIndex = 27;
            this.MinNumCyclesLabel.Text = "Min number cycles";
            // 
            // MaxNumCyclesLabel
            // 
            this.MaxNumCyclesLabel.AutoSize = true;
            this.MaxNumCyclesLabel.Location = new System.Drawing.Point(46, 331);
            this.MaxNumCyclesLabel.Name = "MaxNumCyclesLabel";
            this.MaxNumCyclesLabel.Size = new System.Drawing.Size(98, 13);
            this.MaxNumCyclesLabel.TabIndex = 28;
            this.MaxNumCyclesLabel.Text = "Max number cycles";
            // 
            // DataSourceLabel
            // 
            this.DataSourceLabel.AutoSize = true;
            this.DataSourceLabel.Location = new System.Drawing.Point(311, 335);
            this.DataSourceLabel.Name = "DataSourceLabel";
            this.DataSourceLabel.Size = new System.Drawing.Size(67, 13);
            this.DataSourceLabel.TabIndex = 29;
            this.DataSourceLabel.Text = "Data Source";
            // 
            // MaxNumCyclesTextBox
            // 
            this.MaxNumCyclesTextBox.Location = new System.Drawing.Point(157, 328);
            this.MaxNumCyclesTextBox.Max = 1.7976931348623157E+308D;
            this.MaxNumCyclesTextBox.Min = 0D;
            this.MaxNumCyclesTextBox.Name = "MaxNumCyclesTextBox";
            this.MaxNumCyclesTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.MaxNumCyclesTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.MaxNumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxNumCyclesTextBox.Steps = -1D;
            this.MaxNumCyclesTextBox.TabIndex = 8;
            this.MaxNumCyclesTextBox.Text = "0.000000E+000";
            this.MaxNumCyclesTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.MaxNumCyclesTextBox.Value = 0D;
            // 
            // MinNumCyclesTextBox
            // 
            this.MinNumCyclesTextBox.Location = new System.Drawing.Point(157, 302);
            this.MinNumCyclesTextBox.Max = 1.7976931348623157E+308D;
            this.MinNumCyclesTextBox.Min = 0D;
            this.MinNumCyclesTextBox.Name = "MinNumCyclesTextBox";
            this.MinNumCyclesTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.MinNumCyclesTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.MinNumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinNumCyclesTextBox.Steps = -1D;
            this.MinNumCyclesTextBox.TabIndex = 7;
            this.MinNumCyclesTextBox.Text = "0.000000E+000";
            this.MinNumCyclesTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.MinNumCyclesTextBox.Value = 0D;
            // 
            // MeasPrecisionTextBox
            // 
            this.MeasPrecisionTextBox.Location = new System.Drawing.Point(157, 276);
            this.MeasPrecisionTextBox.Max = 1.7976931348623157E+308D;
            this.MeasPrecisionTextBox.Min = 0D;
            this.MeasPrecisionTextBox.Name = "MeasPrecisionTextBox";
            this.MeasPrecisionTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.MeasPrecisionTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.MeasPrecisionTextBox.Size = new System.Drawing.Size(100, 20);
            this.MeasPrecisionTextBox.Steps = -1D;
            this.MeasPrecisionTextBox.TabIndex = 6;
            this.MeasPrecisionTextBox.Text = "0.000000E+000";
            this.MeasPrecisionTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.MeasPrecisionTextBox.Value = 0D;
            // 
            // NumCyclesTextBox
            // 
            this.NumCyclesTextBox.Location = new System.Drawing.Point(157, 250);
            this.NumCyclesTextBox.Max = 1.7976931348623157E+308D;
            this.NumCyclesTextBox.Min = 0D;
            this.NumCyclesTextBox.Name = "NumCyclesTextBox";
            this.NumCyclesTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.NumCyclesTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.NumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumCyclesTextBox.Steps = -1D;
            this.NumCyclesTextBox.TabIndex = 5;
            this.NumCyclesTextBox.Text = "0.000000E+000";
            this.NumCyclesTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.NumCyclesTextBox.Value = 0D;
            // 
            // DeclaredMassTextBox
            // 
            this.DeclaredMassTextBox.Location = new System.Drawing.Point(157, 68);
            this.DeclaredMassTextBox.Max = 1.7976931348623157E+308D;
            this.DeclaredMassTextBox.Min = 0D;
            this.DeclaredMassTextBox.Name = "DeclaredMassTextBox";
            this.DeclaredMassTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.DeclaredMassTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.DeclaredMassTextBox.Size = new System.Drawing.Size(100, 20);
            this.DeclaredMassTextBox.Steps = -1D;
            this.DeclaredMassTextBox.TabIndex = 4;
            this.DeclaredMassTextBox.Text = "0.000000E+000";
            this.DeclaredMassTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.DeclaredMassTextBox.Value = 0D;
            // 
            // CountTimeTextBox
            // 
            this.CountTimeTextBox.Location = new System.Drawing.Point(157, 120);
            this.CountTimeTextBox.Max = 1.7976931348623157E+308D;
            this.CountTimeTextBox.Min = 0D;
            this.CountTimeTextBox.Name = "CountTimeTextBox";
            this.CountTimeTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.CountTimeTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.CountTimeTextBox.Size = new System.Drawing.Size(100, 20);
            this.CountTimeTextBox.Steps = -1D;
            this.CountTimeTextBox.TabIndex = 3;
            this.CountTimeTextBox.Text = "0.000000E+000";
            this.CountTimeTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.CountTimeTextBox.Value = 0D;
            // 
            // IDDAcquireCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 431);
            this.Controls.Add(this.DataSourceLabel);
            this.Controls.Add(this.MaxNumCyclesLabel);
            this.Controls.Add(this.MinNumCyclesLabel);
            this.Controls.Add(this.MeasPrecisionLabel);
            this.Controls.Add(this.NumCyclesLabel);
            this.Controls.Add(this.DeclaredMassLabel);
            this.Controls.Add(this.CountTimeLabel);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.ItemIdLabel);
            this.Controls.Add(this.Pu240eCoeffBtn);
            this.Controls.Add(this.CompositeIsotopicsBtn);
            this.Controls.Add(this.IsotopicsBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CommentAtEndCheckBox);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.QCTestsCheckBox);
            this.Controls.Add(this.DataSourceComboBox);
            this.Controls.Add(this.MaxNumCyclesTextBox);
            this.Controls.Add(this.MinNumCyclesTextBox);
            this.Controls.Add(this.MeasPrecisionTextBox);
            this.Controls.Add(this.NumCyclesTextBox);
            this.Controls.Add(this.DeclaredMassTextBox);
            this.Controls.Add(this.CountTimeTextBox);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Controls.Add(this.ItemIdComboBox);
            this.Name = "IDDAcquireCalibration";
            this.Text = "Calibration Measurement";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ItemIdComboBox;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.TextBox CommentTextBox;
        private NumericTextBox CountTimeTextBox;
        private NumericTextBox DeclaredMassTextBox;
        private NumericTextBox NumCyclesTextBox;
        private NumericTextBox MeasPrecisionTextBox;
        private NumericTextBox MinNumCyclesTextBox;
        private NumericTextBox MaxNumCyclesTextBox;
        private System.Windows.Forms.ComboBox DataSourceComboBox;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton UseTriplesRadioButton;
        private System.Windows.Forms.RadioButton UseDoublesRadioButton;
        private System.Windows.Forms.RadioButton UseNumCyclesRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button IsotopicsBtn;
        private System.Windows.Forms.Button CompositeIsotopicsBtn;
        private System.Windows.Forms.Button Pu240eCoeffBtn;
        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.Label CountTimeLabel;
        private System.Windows.Forms.Label DeclaredMassLabel;
        private System.Windows.Forms.Label NumCyclesLabel;
        private System.Windows.Forms.Label MeasPrecisionLabel;
        private System.Windows.Forms.Label MinNumCyclesLabel;
        private System.Windows.Forms.Label MaxNumCyclesLabel;
        private System.Windows.Forms.Label DataSourceLabel;
    }
}