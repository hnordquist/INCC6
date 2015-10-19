namespace NewUI
{
    partial class IDDShiftRegisterSetup
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
			this.ShiftRegisterTypeLabel = new System.Windows.Forms.Label();
			this.ShiftRegisterTypeComboBox = new System.Windows.Forms.ComboBox();
			this.ShiftRegisterSerialPortLabel = new System.Windows.Forms.Label();
			this.ShiftRegisterSerialPortComboBox = new System.Windows.Forms.ComboBox();
			this.PredelayLabel = new System.Windows.Forms.Label();
			this.GateLengthLabel = new System.Windows.Forms.Label();
			this.SecondGateLengthLabel = new System.Windows.Forms.Label();
			this.HighVoltageLabel = new System.Windows.Forms.Label();
			this.DieAwayTimeLabel = new System.Windows.Forms.Label();
			this.EfficiencyLabel = new System.Windows.Forms.Label();
			this.MultiplicityDeadtimeLabel = new System.Windows.Forms.Label();
			this.DeadtimeCoefficientALabel = new System.Windows.Forms.Label();
			this.DeadtimeCoefficientBLabel = new System.Windows.Forms.Label();
			this.DeadtimeCoefficientCLabel = new System.Windows.Forms.Label();
			this.DoublesGateFractionLabel = new System.Windows.Forms.Label();
			this.TriplesGateFractionLabel = new System.Windows.Forms.Label();
			this.OKButton = new System.Windows.Forms.Button();
			this.CancelBtn = new System.Windows.Forms.Button();
			this.HelpBtn = new System.Windows.Forms.Button();
			this.EditBtn = new System.Windows.Forms.Button();
			this.BaudCombo = new System.Windows.Forms.ComboBox();
			this.BawdyRateLabel = new System.Windows.Forms.Label();
			this.ShiftRegisterSerialPortComboBoxRefresh = new System.Windows.Forms.Button();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.LMFA = new System.Windows.Forms.CheckBox();
			this.TriplesGateFractionTextBox = new NewUI.NumericTextBox();
			this.DoublesGateFractionTextBox = new NewUI.NumericTextBox();
			this.DeadtimeCoefficientCTextBox = new NewUI.NumericTextBox();
			this.DeadtimeCoefficientBTextBox = new NewUI.NumericTextBox();
			this.DeadtimeCoefficientATextBox = new NewUI.NumericTextBox();
			this.MultiplicityDeadtimeTextBox = new NewUI.NumericTextBox();
			this.EfficiencyTextBox = new NewUI.NumericTextBox();
			this.DieAwayTimeTextBox = new NewUI.NumericTextBox();
			this.HighVoltageTextBox = new NewUI.NumericTextBox();
			this.SecondGateLengthTextBox = new NewUI.NumericTextBox();
			this.GateLengthTextBox = new NewUI.NumericTextBox();
			this.PredelayTextBox = new NewUI.NumericTextBox();
			this.SuspendLayout();
			// 
			// ShiftRegisterTypeLabel
			// 
			this.ShiftRegisterTypeLabel.AutoSize = true;
			this.ShiftRegisterTypeLabel.Location = new System.Drawing.Point(104, 30);
			this.ShiftRegisterTypeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ShiftRegisterTypeLabel.Name = "ShiftRegisterTypeLabel";
			this.ShiftRegisterTypeLabel.Size = new System.Drawing.Size(119, 17);
			this.ShiftRegisterTypeLabel.TabIndex = 0;
			this.ShiftRegisterTypeLabel.Text = "Shift register type";
			// 
			// ShiftRegisterTypeComboBox
			// 
			this.ShiftRegisterTypeComboBox.FormattingEnabled = true;
			this.ShiftRegisterTypeComboBox.Location = new System.Drawing.Point(229, 26);
			this.ShiftRegisterTypeComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ShiftRegisterTypeComboBox.Name = "ShiftRegisterTypeComboBox";
			this.ShiftRegisterTypeComboBox.Size = new System.Drawing.Size(160, 24);
			this.ShiftRegisterTypeComboBox.TabIndex = 1;
			this.ShiftRegisterTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ShiftRegisterTypeComboBox_SelectedIndexChanged);
			// 
			// ShiftRegisterSerialPortLabel
			// 
			this.ShiftRegisterSerialPortLabel.AutoSize = true;
			this.ShiftRegisterSerialPortLabel.Location = new System.Drawing.Point(519, 31);
			this.ShiftRegisterSerialPortLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.ShiftRegisterSerialPortLabel.Name = "ShiftRegisterSerialPortLabel";
			this.ShiftRegisterSerialPortLabel.Size = new System.Drawing.Size(155, 17);
			this.ShiftRegisterSerialPortLabel.TabIndex = 2;
			this.ShiftRegisterSerialPortLabel.Text = "Shift register serial port";
			// 
			// ShiftRegisterSerialPortComboBox
			// 
			this.ShiftRegisterSerialPortComboBox.FormattingEnabled = true;
			this.ShiftRegisterSerialPortComboBox.Location = new System.Drawing.Point(671, 26);
			this.ShiftRegisterSerialPortComboBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.ShiftRegisterSerialPortComboBox.Name = "ShiftRegisterSerialPortComboBox";
			this.ShiftRegisterSerialPortComboBox.Size = new System.Drawing.Size(160, 24);
			this.ShiftRegisterSerialPortComboBox.TabIndex = 3;
			this.ShiftRegisterSerialPortComboBox.SelectedIndexChanged += new System.EventHandler(this.ShiftRegisterSerialPortComboBox_SelectedIndexChanged);
			// 
			// PredelayLabel
			// 
			this.PredelayLabel.AutoSize = true;
			this.PredelayLabel.Location = new System.Drawing.Point(59, 106);
			this.PredelayLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.PredelayLabel.Name = "PredelayLabel";
			this.PredelayLabel.Size = new System.Drawing.Size(165, 17);
			this.PredelayLabel.TabIndex = 4;
			this.PredelayLabel.Text = "Predelay (microseconds)";
			// 
			// GateLengthLabel
			// 
			this.GateLengthLabel.AutoSize = true;
			this.GateLengthLabel.Location = new System.Drawing.Point(40, 139);
			this.GateLengthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.GateLengthLabel.Name = "GateLengthLabel";
			this.GateLengthLabel.Size = new System.Drawing.Size(183, 17);
			this.GateLengthLabel.TabIndex = 6;
			this.GateLengthLabel.Text = "Gate length (microseconds)";
			// 
			// SecondGateLengthLabel
			// 
			this.SecondGateLengthLabel.AutoSize = true;
			this.SecondGateLengthLabel.Location = new System.Drawing.Point(15, 175);
			this.SecondGateLengthLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.SecondGateLengthLabel.Name = "SecondGateLengthLabel";
			this.SecondGateLengthLabel.Size = new System.Drawing.Size(208, 17);
			this.SecondGateLengthLabel.TabIndex = 8;
			this.SecondGateLengthLabel.Text = "2nd gate length (microseconds)";
			// 
			// HighVoltageLabel
			// 
			this.HighVoltageLabel.AutoSize = true;
			this.HighVoltageLabel.Location = new System.Drawing.Point(132, 207);
			this.HighVoltageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.HighVoltageLabel.Name = "HighVoltageLabel";
			this.HighVoltageLabel.Size = new System.Drawing.Size(87, 17);
			this.HighVoltageLabel.TabIndex = 10;
			this.HighVoltageLabel.Text = "High voltage";
			// 
			// DieAwayTimeLabel
			// 
			this.DieAwayTimeLabel.AutoSize = true;
			this.DieAwayTimeLabel.Location = new System.Drawing.Point(25, 239);
			this.DieAwayTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DieAwayTimeLabel.Name = "DieAwayTimeLabel";
			this.DieAwayTimeLabel.Size = new System.Drawing.Size(196, 17);
			this.DieAwayTimeLabel.TabIndex = 12;
			this.DieAwayTimeLabel.Text = "Die away time (microseconds)";
			// 
			// EfficiencyLabel
			// 
			this.EfficiencyLabel.AutoSize = true;
			this.EfficiencyLabel.Location = new System.Drawing.Point(151, 271);
			this.EfficiencyLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.EfficiencyLabel.Name = "EfficiencyLabel";
			this.EfficiencyLabel.Size = new System.Drawing.Size(68, 17);
			this.EfficiencyLabel.TabIndex = 14;
			this.EfficiencyLabel.Text = "Efficiency";
			// 
			// MultiplicityDeadtimeLabel
			// 
			this.MultiplicityDeadtimeLabel.AutoSize = true;
			this.MultiplicityDeadtimeLabel.Location = new System.Drawing.Point(488, 106);
			this.MultiplicityDeadtimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.MultiplicityDeadtimeLabel.Name = "MultiplicityDeadtimeLabel";
			this.MultiplicityDeadtimeLabel.Size = new System.Drawing.Size(177, 17);
			this.MultiplicityDeadtimeLabel.TabIndex = 16;
			this.MultiplicityDeadtimeLabel.Text = "Multiplicity deadtime (1e-9)";
			// 
			// DeadtimeCoefficientALabel
			// 
			this.DeadtimeCoefficientALabel.AutoSize = true;
			this.DeadtimeCoefficientALabel.Location = new System.Drawing.Point(467, 138);
			this.DeadtimeCoefficientALabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DeadtimeCoefficientALabel.Name = "DeadtimeCoefficientALabel";
			this.DeadtimeCoefficientALabel.Size = new System.Drawing.Size(192, 17);
			this.DeadtimeCoefficientALabel.TabIndex = 18;
			this.DeadtimeCoefficientALabel.Text = "Deadtime coefficient A (1e-6)";
			// 
			// DeadtimeCoefficientBLabel
			// 
			this.DeadtimeCoefficientBLabel.AutoSize = true;
			this.DeadtimeCoefficientBLabel.Location = new System.Drawing.Point(467, 174);
			this.DeadtimeCoefficientBLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DeadtimeCoefficientBLabel.Name = "DeadtimeCoefficientBLabel";
			this.DeadtimeCoefficientBLabel.Size = new System.Drawing.Size(200, 17);
			this.DeadtimeCoefficientBLabel.TabIndex = 20;
			this.DeadtimeCoefficientBLabel.Text = "Deadtime coefficient B (1e-12)";
			// 
			// DeadtimeCoefficientCLabel
			// 
			this.DeadtimeCoefficientCLabel.AutoSize = true;
			this.DeadtimeCoefficientCLabel.Location = new System.Drawing.Point(471, 206);
			this.DeadtimeCoefficientCLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DeadtimeCoefficientCLabel.Name = "DeadtimeCoefficientCLabel";
			this.DeadtimeCoefficientCLabel.Size = new System.Drawing.Size(192, 17);
			this.DeadtimeCoefficientCLabel.TabIndex = 22;
			this.DeadtimeCoefficientCLabel.Text = "Deadtime coefficient C (1e-9)";
			// 
			// DoublesGateFractionLabel
			// 
			this.DoublesGateFractionLabel.AutoSize = true;
			this.DoublesGateFractionLabel.Location = new System.Drawing.Point(519, 238);
			this.DoublesGateFractionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.DoublesGateFractionLabel.Name = "DoublesGateFractionLabel";
			this.DoublesGateFractionLabel.Size = new System.Drawing.Size(143, 17);
			this.DoublesGateFractionLabel.TabIndex = 24;
			this.DoublesGateFractionLabel.Text = "Doubles gate fraction";
			// 
			// TriplesGateFractionLabel
			// 
			this.TriplesGateFractionLabel.AutoSize = true;
			this.TriplesGateFractionLabel.Location = new System.Drawing.Point(529, 270);
			this.TriplesGateFractionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.TriplesGateFractionLabel.Name = "TriplesGateFractionLabel";
			this.TriplesGateFractionLabel.Size = new System.Drawing.Size(134, 17);
			this.TriplesGateFractionLabel.TabIndex = 26;
			this.TriplesGateFractionLabel.Text = "Triples gate fraction";
			// 
			// OKButton
			// 
			this.OKButton.Location = new System.Drawing.Point(912, 23);
			this.OKButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.OKButton.Name = "OKButton";
			this.OKButton.Size = new System.Drawing.Size(100, 28);
			this.OKButton.TabIndex = 28;
			this.OKButton.Text = "OK";
			this.OKButton.UseVisualStyleBackColor = true;
			this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(912, 59);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 29;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(912, 95);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 30;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
			// 
			// EditBtn
			// 
			this.EditBtn.Enabled = false;
			this.EditBtn.Location = new System.Drawing.Point(636, 23);
			this.EditBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.EditBtn.Name = "EditBtn";
			this.EditBtn.Size = new System.Drawing.Size(196, 28);
			this.EditBtn.TabIndex = 31;
			this.EditBtn.Text = "Edit selected definition";
			this.EditBtn.UseVisualStyleBackColor = true;
			this.EditBtn.Click += new System.EventHandler(this.EditBtn_Click);
			// 
			// BaudCombo
			// 
			this.BaudCombo.FormattingEnabled = true;
			this.BaudCombo.Location = new System.Drawing.Point(229, 59);
			this.BaudCombo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.BaudCombo.Name = "BaudCombo";
			this.BaudCombo.Size = new System.Drawing.Size(160, 24);
			this.BaudCombo.TabIndex = 32;
			// 
			// BawdyRateLabel
			// 
			this.BawdyRateLabel.AutoSize = true;
			this.BawdyRateLabel.Location = new System.Drawing.Point(137, 65);
			this.BawdyRateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.BawdyRateLabel.Name = "BawdyRateLabel";
			this.BawdyRateLabel.Size = new System.Drawing.Size(70, 17);
			this.BawdyRateLabel.TabIndex = 33;
			this.BawdyRateLabel.Text = "Baud rate";
			// 
			// ShiftRegisterSerialPortComboBoxRefresh
			// 
			this.ShiftRegisterSerialPortComboBoxRefresh.Location = new System.Drawing.Point(840, 26);
			this.ShiftRegisterSerialPortComboBoxRefresh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.ShiftRegisterSerialPortComboBoxRefresh.Name = "ShiftRegisterSerialPortComboBoxRefresh";
			this.ShiftRegisterSerialPortComboBoxRefresh.Size = new System.Drawing.Size(25, 25);
			this.ShiftRegisterSerialPortComboBoxRefresh.TabIndex = 34;
			this.toolTip1.SetToolTip(this.ShiftRegisterSerialPortComboBoxRefresh, "Refresh the available Serial COM Port list");
			this.ShiftRegisterSerialPortComboBoxRefresh.UseVisualStyleBackColor = true;
			this.ShiftRegisterSerialPortComboBoxRefresh.Click += new System.EventHandler(this.Refre_Click);
			// 
			// LMFA
			// 
			this.LMFA.AutoSize = true;
			this.LMFA.Location = new System.Drawing.Point(406, 28);
			this.LMFA.Name = "LMFA";
			this.LMFA.Size = new System.Drawing.Size(87, 21);
			this.LMFA.TabIndex = 35;
			this.LMFA.Text = "Fast acc.";
			this.toolTip1.SetToolTip(this.LMFA, "Conventional multiplicity or \'fast accidentals\' ");
			this.LMFA.UseVisualStyleBackColor = true;
			this.LMFA.Visible = false;
			this.LMFA.CheckedChanged += new System.EventHandler(this.LMFA_CheckedChanged);
			// 
			// TriplesGateFractionTextBox
			// 
			this.TriplesGateFractionTextBox.Location = new System.Drawing.Point(671, 266);
			this.TriplesGateFractionTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.TriplesGateFractionTextBox.Max = 1.7976931348623157E+308D;
			this.TriplesGateFractionTextBox.Min = 0D;
			this.TriplesGateFractionTextBox.Name = "TriplesGateFractionTextBox";
			this.TriplesGateFractionTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.TriplesGateFractionTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.TriplesGateFractionTextBox.Size = new System.Drawing.Size(160, 22);
			this.TriplesGateFractionTextBox.Steps = -1D;
			this.TriplesGateFractionTextBox.TabIndex = 27;
			this.TriplesGateFractionTextBox.Text = "0.000000E+000";
			this.TriplesGateFractionTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.TriplesGateFractionTextBox.Value = 0D;
			// 
			// DoublesGateFractionTextBox
			// 
			this.DoublesGateFractionTextBox.Location = new System.Drawing.Point(671, 234);
			this.DoublesGateFractionTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.DoublesGateFractionTextBox.Max = 1.7976931348623157E+308D;
			this.DoublesGateFractionTextBox.Min = 0D;
			this.DoublesGateFractionTextBox.Name = "DoublesGateFractionTextBox";
			this.DoublesGateFractionTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.DoublesGateFractionTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.DoublesGateFractionTextBox.Size = new System.Drawing.Size(160, 22);
			this.DoublesGateFractionTextBox.Steps = -1D;
			this.DoublesGateFractionTextBox.TabIndex = 25;
			this.DoublesGateFractionTextBox.Text = "0.000000E+000";
			this.DoublesGateFractionTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.DoublesGateFractionTextBox.Value = 0D;
			// 
			// DeadtimeCoefficientCTextBox
			// 
			this.DeadtimeCoefficientCTextBox.Location = new System.Drawing.Point(671, 202);
			this.DeadtimeCoefficientCTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.DeadtimeCoefficientCTextBox.Max = 1.7976931348623157E+308D;
			this.DeadtimeCoefficientCTextBox.Min = 0D;
			this.DeadtimeCoefficientCTextBox.Name = "DeadtimeCoefficientCTextBox";
			this.DeadtimeCoefficientCTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.DeadtimeCoefficientCTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.DeadtimeCoefficientCTextBox.Size = new System.Drawing.Size(160, 22);
			this.DeadtimeCoefficientCTextBox.Steps = -1D;
			this.DeadtimeCoefficientCTextBox.TabIndex = 23;
			this.DeadtimeCoefficientCTextBox.Text = "0.000000E+000";
			this.DeadtimeCoefficientCTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.DeadtimeCoefficientCTextBox.Value = 0D;
			// 
			// DeadtimeCoefficientBTextBox
			// 
			this.DeadtimeCoefficientBTextBox.Location = new System.Drawing.Point(671, 170);
			this.DeadtimeCoefficientBTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.DeadtimeCoefficientBTextBox.Max = 1.7976931348623157E+308D;
			this.DeadtimeCoefficientBTextBox.Min = 0D;
			this.DeadtimeCoefficientBTextBox.Name = "DeadtimeCoefficientBTextBox";
			this.DeadtimeCoefficientBTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.DeadtimeCoefficientBTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.DeadtimeCoefficientBTextBox.Size = new System.Drawing.Size(160, 22);
			this.DeadtimeCoefficientBTextBox.Steps = -1D;
			this.DeadtimeCoefficientBTextBox.TabIndex = 21;
			this.DeadtimeCoefficientBTextBox.Text = "0.000000E+000";
			this.DeadtimeCoefficientBTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.DeadtimeCoefficientBTextBox.Value = 0D;
			// 
			// DeadtimeCoefficientATextBox
			// 
			this.DeadtimeCoefficientATextBox.Location = new System.Drawing.Point(671, 138);
			this.DeadtimeCoefficientATextBox.Margin = new System.Windows.Forms.Padding(4);
			this.DeadtimeCoefficientATextBox.Max = 1.7976931348623157E+308D;
			this.DeadtimeCoefficientATextBox.Min = 0D;
			this.DeadtimeCoefficientATextBox.Name = "DeadtimeCoefficientATextBox";
			this.DeadtimeCoefficientATextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.DeadtimeCoefficientATextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.DeadtimeCoefficientATextBox.Size = new System.Drawing.Size(160, 22);
			this.DeadtimeCoefficientATextBox.Steps = -1D;
			this.DeadtimeCoefficientATextBox.TabIndex = 19;
			this.DeadtimeCoefficientATextBox.Text = "0.000000E+000";
			this.DeadtimeCoefficientATextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.DeadtimeCoefficientATextBox.Value = 0D;
			// 
			// MultiplicityDeadtimeTextBox
			// 
			this.MultiplicityDeadtimeTextBox.Location = new System.Drawing.Point(671, 102);
			this.MultiplicityDeadtimeTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.MultiplicityDeadtimeTextBox.Max = 1.7976931348623157E+308D;
			this.MultiplicityDeadtimeTextBox.Min = 0D;
			this.MultiplicityDeadtimeTextBox.Name = "MultiplicityDeadtimeTextBox";
			this.MultiplicityDeadtimeTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.MultiplicityDeadtimeTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.MultiplicityDeadtimeTextBox.Size = new System.Drawing.Size(160, 22);
			this.MultiplicityDeadtimeTextBox.Steps = -1D;
			this.MultiplicityDeadtimeTextBox.TabIndex = 17;
			this.MultiplicityDeadtimeTextBox.Text = "0.000000E+000";
			this.MultiplicityDeadtimeTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.MultiplicityDeadtimeTextBox.Value = 0D;
			// 
			// EfficiencyTextBox
			// 
			this.EfficiencyTextBox.Location = new System.Drawing.Point(229, 267);
			this.EfficiencyTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.EfficiencyTextBox.Max = 1.7976931348623157E+308D;
			this.EfficiencyTextBox.Min = 0D;
			this.EfficiencyTextBox.Name = "EfficiencyTextBox";
			this.EfficiencyTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.EfficiencyTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.EfficiencyTextBox.Size = new System.Drawing.Size(160, 22);
			this.EfficiencyTextBox.Steps = -1D;
			this.EfficiencyTextBox.TabIndex = 15;
			this.EfficiencyTextBox.Text = "0.000000E+000";
			this.EfficiencyTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.EfficiencyTextBox.Value = 0D;
			// 
			// DieAwayTimeTextBox
			// 
			this.DieAwayTimeTextBox.Location = new System.Drawing.Point(229, 235);
			this.DieAwayTimeTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.DieAwayTimeTextBox.Max = 1.7976931348623157E+308D;
			this.DieAwayTimeTextBox.Min = 0D;
			this.DieAwayTimeTextBox.Name = "DieAwayTimeTextBox";
			this.DieAwayTimeTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.DieAwayTimeTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.DieAwayTimeTextBox.Size = new System.Drawing.Size(160, 22);
			this.DieAwayTimeTextBox.Steps = -1D;
			this.DieAwayTimeTextBox.TabIndex = 13;
			this.DieAwayTimeTextBox.Text = "0.000000E+000";
			this.DieAwayTimeTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.DieAwayTimeTextBox.Value = 0D;
			// 
			// HighVoltageTextBox
			// 
			this.HighVoltageTextBox.Location = new System.Drawing.Point(229, 203);
			this.HighVoltageTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.HighVoltageTextBox.Max = 1.7976931348623157E+308D;
			this.HighVoltageTextBox.Min = 0D;
			this.HighVoltageTextBox.Name = "HighVoltageTextBox";
			this.HighVoltageTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.HighVoltageTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.HighVoltageTextBox.Size = new System.Drawing.Size(160, 22);
			this.HighVoltageTextBox.Steps = -1D;
			this.HighVoltageTextBox.TabIndex = 11;
			this.HighVoltageTextBox.Text = "0.000000E+000";
			this.HighVoltageTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.HighVoltageTextBox.Value = 0D;
			// 
			// SecondGateLengthTextBox
			// 
			this.SecondGateLengthTextBox.Enabled = false;
			this.SecondGateLengthTextBox.Location = new System.Drawing.Point(229, 171);
			this.SecondGateLengthTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.SecondGateLengthTextBox.Max = 1.7976931348623157E+308D;
			this.SecondGateLengthTextBox.Min = 0D;
			this.SecondGateLengthTextBox.Name = "SecondGateLengthTextBox";
			this.SecondGateLengthTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.SecondGateLengthTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.SecondGateLengthTextBox.Size = new System.Drawing.Size(160, 22);
			this.SecondGateLengthTextBox.Steps = -1D;
			this.SecondGateLengthTextBox.TabIndex = 9;
			this.SecondGateLengthTextBox.Text = "0.000000E+000";
			this.SecondGateLengthTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.SecondGateLengthTextBox.Value = 0D;
			// 
			// GateLengthTextBox
			// 
			this.GateLengthTextBox.Location = new System.Drawing.Point(229, 139);
			this.GateLengthTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.GateLengthTextBox.Max = 1.7976931348623157E+308D;
			this.GateLengthTextBox.Min = 0D;
			this.GateLengthTextBox.Name = "GateLengthTextBox";
			this.GateLengthTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.GateLengthTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.GateLengthTextBox.Size = new System.Drawing.Size(160, 22);
			this.GateLengthTextBox.Steps = -1D;
			this.GateLengthTextBox.TabIndex = 7;
			this.GateLengthTextBox.Text = "0.000000E+000";
			this.GateLengthTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.GateLengthTextBox.Value = 0D;
			// 
			// PredelayTextBox
			// 
			this.PredelayTextBox.Location = new System.Drawing.Point(229, 102);
			this.PredelayTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.PredelayTextBox.Max = 1.7976931348623157E+308D;
			this.PredelayTextBox.Min = 0D;
			this.PredelayTextBox.Name = "PredelayTextBox";
			this.PredelayTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
			this.PredelayTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
			this.PredelayTextBox.Size = new System.Drawing.Size(160, 22);
			this.PredelayTextBox.Steps = -1D;
			this.PredelayTextBox.TabIndex = 5;
			this.PredelayTextBox.Text = "0.000000E+000";
			this.PredelayTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
			this.PredelayTextBox.Value = 0D;
			// 
			// IDDShiftRegisterSetup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1047, 320);
			this.Controls.Add(this.LMFA);
			this.Controls.Add(this.ShiftRegisterSerialPortComboBoxRefresh);
			this.Controls.Add(this.BawdyRateLabel);
			this.Controls.Add(this.BaudCombo);
			this.Controls.Add(this.HelpBtn);
			this.Controls.Add(this.CancelBtn);
			this.Controls.Add(this.OKButton);
			this.Controls.Add(this.TriplesGateFractionTextBox);
			this.Controls.Add(this.TriplesGateFractionLabel);
			this.Controls.Add(this.DoublesGateFractionTextBox);
			this.Controls.Add(this.DoublesGateFractionLabel);
			this.Controls.Add(this.DeadtimeCoefficientCTextBox);
			this.Controls.Add(this.DeadtimeCoefficientCLabel);
			this.Controls.Add(this.DeadtimeCoefficientBTextBox);
			this.Controls.Add(this.DeadtimeCoefficientBLabel);
			this.Controls.Add(this.DeadtimeCoefficientATextBox);
			this.Controls.Add(this.DeadtimeCoefficientALabel);
			this.Controls.Add(this.MultiplicityDeadtimeTextBox);
			this.Controls.Add(this.MultiplicityDeadtimeLabel);
			this.Controls.Add(this.EfficiencyTextBox);
			this.Controls.Add(this.EfficiencyLabel);
			this.Controls.Add(this.DieAwayTimeTextBox);
			this.Controls.Add(this.DieAwayTimeLabel);
			this.Controls.Add(this.HighVoltageTextBox);
			this.Controls.Add(this.HighVoltageLabel);
			this.Controls.Add(this.SecondGateLengthTextBox);
			this.Controls.Add(this.SecondGateLengthLabel);
			this.Controls.Add(this.GateLengthTextBox);
			this.Controls.Add(this.GateLengthLabel);
			this.Controls.Add(this.PredelayTextBox);
			this.Controls.Add(this.PredelayLabel);
			this.Controls.Add(this.ShiftRegisterSerialPortComboBox);
			this.Controls.Add(this.ShiftRegisterSerialPortLabel);
			this.Controls.Add(this.ShiftRegisterTypeComboBox);
			this.Controls.Add(this.ShiftRegisterTypeLabel);
			this.Controls.Add(this.EditBtn);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "IDDShiftRegisterSetup";
			this.Text = "Measurement Parameters Setup";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ShiftRegisterTypeLabel;
        private System.Windows.Forms.ComboBox ShiftRegisterTypeComboBox;
        private System.Windows.Forms.Label ShiftRegisterSerialPortLabel;
        private System.Windows.Forms.ComboBox ShiftRegisterSerialPortComboBox;
        private System.Windows.Forms.Label PredelayLabel;
        private NewUI.NumericTextBox PredelayTextBox;
        private System.Windows.Forms.Label GateLengthLabel;
        private NewUI.NumericTextBox GateLengthTextBox;
        private System.Windows.Forms.Label SecondGateLengthLabel;
        private NewUI.NumericTextBox SecondGateLengthTextBox;
        private System.Windows.Forms.Label HighVoltageLabel;
        private NewUI.NumericTextBox HighVoltageTextBox;
        private System.Windows.Forms.Label DieAwayTimeLabel;
        private NewUI.NumericTextBox DieAwayTimeTextBox;
        private System.Windows.Forms.Label EfficiencyLabel;
        private NewUI.NumericTextBox EfficiencyTextBox;
        private System.Windows.Forms.Label MultiplicityDeadtimeLabel;
        private NewUI.NumericTextBox MultiplicityDeadtimeTextBox;
        private System.Windows.Forms.Label DeadtimeCoefficientALabel;
        private NewUI.NumericTextBox DeadtimeCoefficientATextBox;
        private System.Windows.Forms.Label DeadtimeCoefficientBLabel;
        private NewUI.NumericTextBox DeadtimeCoefficientBTextBox;
        private System.Windows.Forms.Label DeadtimeCoefficientCLabel;
        private NewUI.NumericTextBox DeadtimeCoefficientCTextBox;
        private System.Windows.Forms.Label DoublesGateFractionLabel;
        private NewUI.NumericTextBox DoublesGateFractionTextBox;
        private System.Windows.Forms.Label TriplesGateFractionLabel;
        private NewUI.NumericTextBox TriplesGateFractionTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button EditBtn;
        private System.Windows.Forms.ComboBox BaudCombo;
        private System.Windows.Forms.Label BawdyRateLabel;
        private System.Windows.Forms.Button ShiftRegisterSerialPortComboBoxRefresh;
        private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.CheckBox LMFA;
    }
}