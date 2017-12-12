namespace UI
{
    partial class IDDCollarAcquire
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
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CalculateResultsRadioButton = new System.Windows.Forms.RadioButton();
            this.ActiveMeasurementRadioButton = new System.Windows.Forms.RadioButton();
            this.PassiveMeasurementRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.FromDB = new System.Windows.Forms.Button();
            this.PassiveDoublesErrorLabel = new System.Windows.Forms.Label();
            this.PassiveSinglesErrorLabel = new System.Windows.Forms.Label();
            this.PassiveDoublesLabel = new System.Windows.Forms.Label();
            this.PassiveSinglesLabel = new System.Windows.Forms.Label();
            this.PassiveDoublesErrorTextBox = new UI.NumericTextBox();
            this.PassiveSinglesErrorTextBox = new UI.NumericTextBox();
            this.PassiveDoublesTextBox = new UI.NumericTextBox();
            this.PassiveSinglesTextBox = new UI.NumericTextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ActiveDoublesErrorLabel = new System.Windows.Forms.Label();
            this.ActiveSinglesErrorLabel = new System.Windows.Forms.Label();
            this.ActiveDoublesLabel = new System.Windows.Forms.Label();
            this.ActiveSinglesLabel = new System.Windows.Forms.Label();
            this.ActiveDoublesTextBox = new UI.NumericTextBox();
            this.ActiveDoublesErrorTextBox = new UI.NumericTextBox();
            this.ActiveSinglesErrorTextBox = new UI.NumericTextBox();
            this.ActiveSinglesTextBox = new UI.NumericTextBox();
            this.ModeLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // ModeComboBox
            // 
            this.ModeComboBox.FormattingEnabled = true;
            this.ModeComboBox.Items.AddRange(new object[] {
            "Thermal (no Cd)",
            "Fast (Cd)"});
            this.ModeComboBox.Location = new System.Drawing.Point(49, 24);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(132, 21);
            this.ModeComboBox.TabIndex = 0;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(786, 17);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 1;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(786, 46);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 2;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(786, 75);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 3;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CalculateResultsRadioButton);
            this.groupBox1.Controls.Add(this.ActiveMeasurementRadioButton);
            this.groupBox1.Controls.Add(this.PassiveMeasurementRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(169, 112);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            // 
            // CalculateResultsRadioButton
            // 
            this.CalculateResultsRadioButton.AutoSize = true;
            this.CalculateResultsRadioButton.Location = new System.Drawing.Point(20, 74);
            this.CalculateResultsRadioButton.Name = "CalculateResultsRadioButton";
            this.CalculateResultsRadioButton.Size = new System.Drawing.Size(102, 17);
            this.CalculateResultsRadioButton.TabIndex = 2;
            this.CalculateResultsRadioButton.TabStop = true;
            this.CalculateResultsRadioButton.Text = "Calculate results";
            this.CalculateResultsRadioButton.UseVisualStyleBackColor = true;
            // 
            // ActiveMeasurementRadioButton
            // 
            this.ActiveMeasurementRadioButton.AutoSize = true;
            this.ActiveMeasurementRadioButton.Location = new System.Drawing.Point(20, 51);
            this.ActiveMeasurementRadioButton.Name = "ActiveMeasurementRadioButton";
            this.ActiveMeasurementRadioButton.Size = new System.Drawing.Size(121, 17);
            this.ActiveMeasurementRadioButton.TabIndex = 1;
            this.ActiveMeasurementRadioButton.TabStop = true;
            this.ActiveMeasurementRadioButton.Text = "Active measurement";
            this.ActiveMeasurementRadioButton.UseVisualStyleBackColor = true;
            // 
            // PassiveMeasurementRadioButton
            // 
            this.PassiveMeasurementRadioButton.AutoSize = true;
            this.PassiveMeasurementRadioButton.Location = new System.Drawing.Point(20, 28);
            this.PassiveMeasurementRadioButton.Name = "PassiveMeasurementRadioButton";
            this.PassiveMeasurementRadioButton.Size = new System.Drawing.Size(128, 17);
            this.PassiveMeasurementRadioButton.TabIndex = 0;
            this.PassiveMeasurementRadioButton.TabStop = true;
            this.PassiveMeasurementRadioButton.Text = "Passive measurement";
            this.PassiveMeasurementRadioButton.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.FromDB);
            this.groupBox2.Controls.Add(this.PassiveDoublesErrorLabel);
            this.groupBox2.Controls.Add(this.PassiveSinglesErrorLabel);
            this.groupBox2.Controls.Add(this.PassiveDoublesLabel);
            this.groupBox2.Controls.Add(this.PassiveSinglesLabel);
            this.groupBox2.Controls.Add(this.PassiveDoublesErrorTextBox);
            this.groupBox2.Controls.Add(this.PassiveSinglesErrorTextBox);
            this.groupBox2.Controls.Add(this.PassiveDoublesTextBox);
            this.groupBox2.Controls.Add(this.PassiveSinglesTextBox);
            this.groupBox2.Location = new System.Drawing.Point(206, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(574, 91);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            // 
            // FromDB
            // 
            this.FromDB.Location = new System.Drawing.Point(479, 31);
            this.FromDB.Name = "FromDB";
            this.FromDB.Size = new System.Drawing.Size(75, 35);
            this.FromDB.TabIndex = 8;
            this.FromDB.Text = "Select From DB";
            this.FromDB.UseVisualStyleBackColor = true;
            this.FromDB.Click += new System.EventHandler(this.FromDB_Click);
            // 
            // PassiveDoublesErrorLabel
            // 
            this.PassiveDoublesErrorLabel.AutoSize = true;
            this.PassiveDoublesErrorLabel.Location = new System.Drawing.Point(248, 53);
            this.PassiveDoublesErrorLabel.Name = "PassiveDoublesErrorLabel";
            this.PassiveDoublesErrorLabel.Size = new System.Drawing.Size(108, 13);
            this.PassiveDoublesErrorLabel.TabIndex = 7;
            this.PassiveDoublesErrorLabel.Text = "Passive doubles error";
            // 
            // PassiveSinglesErrorLabel
            // 
            this.PassiveSinglesErrorLabel.AutoSize = true;
            this.PassiveSinglesErrorLabel.Location = new System.Drawing.Point(253, 27);
            this.PassiveSinglesErrorLabel.Name = "PassiveSinglesErrorLabel";
            this.PassiveSinglesErrorLabel.Size = new System.Drawing.Size(103, 13);
            this.PassiveSinglesErrorLabel.TabIndex = 6;
            this.PassiveSinglesErrorLabel.Text = "Passive singles error";
            // 
            // PassiveDoublesLabel
            // 
            this.PassiveDoublesLabel.AutoSize = true;
            this.PassiveDoublesLabel.Location = new System.Drawing.Point(14, 53);
            this.PassiveDoublesLabel.Name = "PassiveDoublesLabel";
            this.PassiveDoublesLabel.Size = new System.Drawing.Size(105, 13);
            this.PassiveDoublesLabel.TabIndex = 5;
            this.PassiveDoublesLabel.Text = "Passive doubles rate";
            // 
            // PassiveSinglesLabel
            // 
            this.PassiveSinglesLabel.AutoSize = true;
            this.PassiveSinglesLabel.Location = new System.Drawing.Point(19, 27);
            this.PassiveSinglesLabel.Name = "PassiveSinglesLabel";
            this.PassiveSinglesLabel.Size = new System.Drawing.Size(100, 13);
            this.PassiveSinglesLabel.TabIndex = 4;
            this.PassiveSinglesLabel.Text = "Passive singles rate";
            // 
            // PassiveDoublesErrorTextBox
            // 
            this.PassiveDoublesErrorTextBox.Location = new System.Drawing.Point(362, 50);
            this.PassiveDoublesErrorTextBox.Max = 1.7976931348623157E+308D;
            this.PassiveDoublesErrorTextBox.Min = 0D;
            this.PassiveDoublesErrorTextBox.Name = "PassiveDoublesErrorTextBox";
            this.PassiveDoublesErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.PassiveDoublesErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.PassiveDoublesErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.PassiveDoublesErrorTextBox.Steps = -1D;
            this.PassiveDoublesErrorTextBox.TabIndex = 3;
            this.PassiveDoublesErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.PassiveDoublesErrorTextBox.Value = 0D;
            // 
            // PassiveSinglesErrorTextBox
            // 
            this.PassiveSinglesErrorTextBox.Location = new System.Drawing.Point(362, 24);
            this.PassiveSinglesErrorTextBox.Max = 1.7976931348623157E+308D;
            this.PassiveSinglesErrorTextBox.Min = 0D;
            this.PassiveSinglesErrorTextBox.Name = "PassiveSinglesErrorTextBox";
            this.PassiveSinglesErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.PassiveSinglesErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.PassiveSinglesErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.PassiveSinglesErrorTextBox.Steps = -1D;
            this.PassiveSinglesErrorTextBox.TabIndex = 2;
            this.PassiveSinglesErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.PassiveSinglesErrorTextBox.Value = 0D;
            // 
            // PassiveDoublesTextBox
            // 
            this.PassiveDoublesTextBox.Location = new System.Drawing.Point(125, 50);
            this.PassiveDoublesTextBox.Max = 1.7976931348623157E+308D;
            this.PassiveDoublesTextBox.Min = 0D;
            this.PassiveDoublesTextBox.Name = "PassiveDoublesTextBox";
            this.PassiveDoublesTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.PassiveDoublesTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.PassiveDoublesTextBox.Size = new System.Drawing.Size(100, 20);
            this.PassiveDoublesTextBox.Steps = -1D;
            this.PassiveDoublesTextBox.TabIndex = 1;
            this.PassiveDoublesTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.PassiveDoublesTextBox.Value = 0D;
            // 
            // PassiveSinglesTextBox
            // 
            this.PassiveSinglesTextBox.Location = new System.Drawing.Point(125, 24);
            this.PassiveSinglesTextBox.Max = 1.7976931348623157E+308D;
            this.PassiveSinglesTextBox.Min = 0D;
            this.PassiveSinglesTextBox.Name = "PassiveSinglesTextBox";
            this.PassiveSinglesTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.PassiveSinglesTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.PassiveSinglesTextBox.Size = new System.Drawing.Size(100, 20);
            this.PassiveSinglesTextBox.Steps = -1D;
            this.PassiveSinglesTextBox.TabIndex = 0;
            this.PassiveSinglesTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.PassiveSinglesTextBox.Value = 0D;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ActiveDoublesErrorLabel);
            this.groupBox3.Controls.Add(this.ActiveSinglesErrorLabel);
            this.groupBox3.Controls.Add(this.ActiveDoublesLabel);
            this.groupBox3.Controls.Add(this.ActiveSinglesLabel);
            this.groupBox3.Controls.Add(this.ActiveDoublesTextBox);
            this.groupBox3.Controls.Add(this.ActiveDoublesErrorTextBox);
            this.groupBox3.Controls.Add(this.ActiveSinglesErrorTextBox);
            this.groupBox3.Controls.Add(this.ActiveSinglesTextBox);
            this.groupBox3.Location = new System.Drawing.Point(206, 112);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(574, 87);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            // 
            // ActiveDoublesErrorLabel
            // 
            this.ActiveDoublesErrorLabel.AutoSize = true;
            this.ActiveDoublesErrorLabel.Location = new System.Drawing.Point(255, 52);
            this.ActiveDoublesErrorLabel.Name = "ActiveDoublesErrorLabel";
            this.ActiveDoublesErrorLabel.Size = new System.Drawing.Size(101, 13);
            this.ActiveDoublesErrorLabel.TabIndex = 8;
            this.ActiveDoublesErrorLabel.Text = "Active doubles error";
            // 
            // ActiveSinglesErrorLabel
            // 
            this.ActiveSinglesErrorLabel.AutoSize = true;
            this.ActiveSinglesErrorLabel.Location = new System.Drawing.Point(260, 26);
            this.ActiveSinglesErrorLabel.Name = "ActiveSinglesErrorLabel";
            this.ActiveSinglesErrorLabel.Size = new System.Drawing.Size(96, 13);
            this.ActiveSinglesErrorLabel.TabIndex = 7;
            this.ActiveSinglesErrorLabel.Text = "Active singles error";
            // 
            // ActiveDoublesLabel
            // 
            this.ActiveDoublesLabel.AutoSize = true;
            this.ActiveDoublesLabel.Location = new System.Drawing.Point(21, 52);
            this.ActiveDoublesLabel.Name = "ActiveDoublesLabel";
            this.ActiveDoublesLabel.Size = new System.Drawing.Size(98, 13);
            this.ActiveDoublesLabel.TabIndex = 6;
            this.ActiveDoublesLabel.Text = "Active doubles rate";
            // 
            // ActiveSinglesLabel
            // 
            this.ActiveSinglesLabel.AutoSize = true;
            this.ActiveSinglesLabel.Location = new System.Drawing.Point(26, 26);
            this.ActiveSinglesLabel.Name = "ActiveSinglesLabel";
            this.ActiveSinglesLabel.Size = new System.Drawing.Size(93, 13);
            this.ActiveSinglesLabel.TabIndex = 5;
            this.ActiveSinglesLabel.Text = "Active singles rate";
            // 
            // ActiveDoublesTextBox
            // 
            this.ActiveDoublesTextBox.Location = new System.Drawing.Point(125, 49);
            this.ActiveDoublesTextBox.Max = 1.7976931348623157E+308D;
            this.ActiveDoublesTextBox.Min = 0D;
            this.ActiveDoublesTextBox.Name = "ActiveDoublesTextBox";
            this.ActiveDoublesTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.ActiveDoublesTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.ActiveDoublesTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActiveDoublesTextBox.Steps = -1D;
            this.ActiveDoublesTextBox.TabIndex = 4;
            this.ActiveDoublesTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.ActiveDoublesTextBox.Value = 0D;
            // 
            // ActiveDoublesErrorTextBox
            // 
            this.ActiveDoublesErrorTextBox.Location = new System.Drawing.Point(362, 49);
            this.ActiveDoublesErrorTextBox.Max = 1.7976931348623157E+308D;
            this.ActiveDoublesErrorTextBox.Min = 0D;
            this.ActiveDoublesErrorTextBox.Name = "ActiveDoublesErrorTextBox";
            this.ActiveDoublesErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.ActiveDoublesErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.ActiveDoublesErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActiveDoublesErrorTextBox.Steps = -1D;
            this.ActiveDoublesErrorTextBox.TabIndex = 3;
            this.ActiveDoublesErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.ActiveDoublesErrorTextBox.Value = 0D;
            // 
            // ActiveSinglesErrorTextBox
            // 
            this.ActiveSinglesErrorTextBox.Location = new System.Drawing.Point(362, 23);
            this.ActiveSinglesErrorTextBox.Max = 1.7976931348623157E+308D;
            this.ActiveSinglesErrorTextBox.Min = 0D;
            this.ActiveSinglesErrorTextBox.Name = "ActiveSinglesErrorTextBox";
            this.ActiveSinglesErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.ActiveSinglesErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.ActiveSinglesErrorTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActiveSinglesErrorTextBox.Steps = -1D;
            this.ActiveSinglesErrorTextBox.TabIndex = 2;
            this.ActiveSinglesErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.ActiveSinglesErrorTextBox.Value = 0D;
            // 
            // ActiveSinglesTextBox
            // 
            this.ActiveSinglesTextBox.Location = new System.Drawing.Point(125, 23);
            this.ActiveSinglesTextBox.Max = 1.7976931348623157E+308D;
            this.ActiveSinglesTextBox.Min = 0D;
            this.ActiveSinglesTextBox.Name = "ActiveSinglesTextBox";
            this.ActiveSinglesTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.ActiveSinglesTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.ActiveSinglesTextBox.Size = new System.Drawing.Size(100, 20);
            this.ActiveSinglesTextBox.Steps = -1D;
            this.ActiveSinglesTextBox.TabIndex = 0;
            this.ActiveSinglesTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.ActiveSinglesTextBox.Value = 0D;
            // 
            // ModeLabel
            // 
            this.ModeLabel.AutoSize = true;
            this.ModeLabel.Location = new System.Drawing.Point(9, 27);
            this.ModeLabel.Name = "ModeLabel";
            this.ModeLabel.Size = new System.Drawing.Size(34, 13);
            this.ModeLabel.TabIndex = 7;
            this.ModeLabel.Text = "Mode";
            // 
            // IDDCollarAcquire
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 257);
            this.Controls.Add(this.ModeLabel);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.ModeComboBox);
            this.Name = "IDDCollarAcquire";
            this.Text = "Collar Verification";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ModeComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton CalculateResultsRadioButton;
        private System.Windows.Forms.RadioButton ActiveMeasurementRadioButton;
        private System.Windows.Forms.RadioButton PassiveMeasurementRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label PassiveDoublesErrorLabel;
        private System.Windows.Forms.Label PassiveSinglesErrorLabel;
        private System.Windows.Forms.Label PassiveDoublesLabel;
        private System.Windows.Forms.Label PassiveSinglesLabel;
        private NumericTextBox PassiveDoublesErrorTextBox;
        private NumericTextBox PassiveSinglesErrorTextBox;
        private NumericTextBox PassiveDoublesTextBox;
        private NumericTextBox PassiveSinglesTextBox;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label ActiveDoublesErrorLabel;
        private System.Windows.Forms.Label ActiveSinglesErrorLabel;
        private System.Windows.Forms.Label ActiveDoublesLabel;
        private System.Windows.Forms.Label ActiveSinglesLabel;
        private NumericTextBox ActiveDoublesTextBox;
        private NumericTextBox ActiveDoublesErrorTextBox;
        private NumericTextBox ActiveSinglesErrorTextBox;
        private NumericTextBox ActiveSinglesTextBox;
        private System.Windows.Forms.Label ModeLabel;
        private System.Windows.Forms.Button FromDB;
    }
}