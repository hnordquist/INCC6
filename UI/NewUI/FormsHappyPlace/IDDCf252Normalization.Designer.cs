namespace NewUI
{
    partial class IDDCf252Normalization
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
            this.SourceIdTextBox = new System.Windows.Forms.TextBox();
            this.NormConstTextBox = new NewUI.NumericTextBox();
            this.NormConstErrorTextBox = new NewUI.NumericTextBox();
            this.RefDoublesRateTextBox = new NewUI.NumericTextBox();
            this.RefDoublesRateErrorTextBox = new NewUI.NumericTextBox();
            this.PrecisionLimitTextBox = new NewUI.NumericTextBox();
            this.AccLimitStdDevTextBox = new NewUI.NumericTextBox();
            this.AccLimitPercentTextBox = new NewUI.NumericTextBox();
            this.MovementDistanceTextBox = new NewUI.NumericTextBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.AddASourceCheckBox = new System.Windows.Forms.CheckBox();
            this.SourceIdLabel = new System.Windows.Forms.Label();
            this.NormConstLabel = new System.Windows.Forms.Label();
            this.NormConstErrorLabel = new System.Windows.Forms.Label();
            this.RefDoublesRateLabel = new System.Windows.Forms.Label();
            this.RefDoublesRateErrorLabel = new System.Windows.Forms.Label();
            this.RefDoublesDateLabel = new System.Windows.Forms.Label();
            this.PrecisionLimitLabel = new System.Windows.Forms.Label();
            this.AccLimitStdDevLabel = new System.Windows.Forms.Label();
            this.AccLimitPercentLabel = new System.Windows.Forms.Label();
            this.MovementDistanceLabel = new System.Windows.Forms.Label();
            this.RefDoublesDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // SourceIdTextBox
            // 
            this.SourceIdTextBox.Location = new System.Drawing.Point(196, 12);
            this.SourceIdTextBox.Name = "SourceIdTextBox";
            this.SourceIdTextBox.Size = new System.Drawing.Size(200, 20);
            this.SourceIdTextBox.TabIndex = 0;
            this.SourceIdTextBox.Leave += new System.EventHandler(this.SourceIdTextBox_Leave);
            // 
            // NormConstTextBox
            // 
            this.NormConstTextBox.Location = new System.Drawing.Point(196, 38);
            this.NormConstTextBox.Max = 1.7976931348623157E+308D;
            this.NormConstTextBox.Min = 0D;
            this.NormConstTextBox.Name = "NormConstTextBox";
            this.NormConstTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.NormConstTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.NormConstTextBox.Size = new System.Drawing.Size(200, 20);
            this.NormConstTextBox.Steps = -1D;
            this.NormConstTextBox.TabIndex = 1;
            this.NormConstTextBox.Text = "0.000000E+000";
            this.NormConstTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.NormConstTextBox.Value = 0D;
            // 
            // NormConstErrorTextBox
            // 
            this.NormConstErrorTextBox.Location = new System.Drawing.Point(196, 64);
            this.NormConstErrorTextBox.Max = 1.7976931348623157E+308D;
            this.NormConstErrorTextBox.Min = 0D;
            this.NormConstErrorTextBox.Name = "NormConstErrorTextBox";
            this.NormConstErrorTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.NormConstErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.NormConstErrorTextBox.Size = new System.Drawing.Size(200, 20);
            this.NormConstErrorTextBox.Steps = -1D;
            this.NormConstErrorTextBox.TabIndex = 2;
            this.NormConstErrorTextBox.Text = "0.000000E+000";
            this.NormConstErrorTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.NormConstErrorTextBox.Value = 0D;
            // 
            // RefDoublesRateTextBox
            // 
            this.RefDoublesRateTextBox.Location = new System.Drawing.Point(196, 90);
            this.RefDoublesRateTextBox.Max = 1.7976931348623157E+308D;
            this.RefDoublesRateTextBox.Min = 0D;
            this.RefDoublesRateTextBox.Name = "RefDoublesRateTextBox";
            this.RefDoublesRateTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.RefDoublesRateTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.RefDoublesRateTextBox.Size = new System.Drawing.Size(200, 20);
            this.RefDoublesRateTextBox.Steps = -1D;
            this.RefDoublesRateTextBox.TabIndex = 3;
            this.RefDoublesRateTextBox.Text = "0.000000E+000";
            this.RefDoublesRateTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.RefDoublesRateTextBox.Value = 0D;
            // 
            // RefDoublesRateErrorTextBox
            // 
            this.RefDoublesRateErrorTextBox.Location = new System.Drawing.Point(196, 116);
            this.RefDoublesRateErrorTextBox.Max = 1.7976931348623157E+308D;
            this.RefDoublesRateErrorTextBox.Min = 0D;
            this.RefDoublesRateErrorTextBox.Name = "RefDoublesRateErrorTextBox";
            this.RefDoublesRateErrorTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.RefDoublesRateErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.RefDoublesRateErrorTextBox.Size = new System.Drawing.Size(200, 20);
            this.RefDoublesRateErrorTextBox.Steps = -1D;
            this.RefDoublesRateErrorTextBox.TabIndex = 4;
            this.RefDoublesRateErrorTextBox.Text = "0.000000E+000";
            this.RefDoublesRateErrorTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.RefDoublesRateErrorTextBox.Value = 0D;
            // 
            // PrecisionLimitTextBox
            // 
            this.PrecisionLimitTextBox.Location = new System.Drawing.Point(196, 168);
            this.PrecisionLimitTextBox.Max = 1.7976931348623157E+308D;
            this.PrecisionLimitTextBox.Min = 0D;
            this.PrecisionLimitTextBox.Name = "PrecisionLimitTextBox";
            this.PrecisionLimitTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.PrecisionLimitTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.PrecisionLimitTextBox.Size = new System.Drawing.Size(200, 20);
            this.PrecisionLimitTextBox.Steps = -1D;
            this.PrecisionLimitTextBox.TabIndex = 6;
            this.PrecisionLimitTextBox.Text = "0.000000E+000";
            this.PrecisionLimitTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.PrecisionLimitTextBox.Value = 0D;
            // 
            // AccLimitStdDevTextBox
            // 
            this.AccLimitStdDevTextBox.Location = new System.Drawing.Point(196, 194);
            this.AccLimitStdDevTextBox.Max = 1.7976931348623157E+308D;
            this.AccLimitStdDevTextBox.Min = 0D;
            this.AccLimitStdDevTextBox.Name = "AccLimitStdDevTextBox";
            this.AccLimitStdDevTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.AccLimitStdDevTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.AccLimitStdDevTextBox.Size = new System.Drawing.Size(200, 20);
            this.AccLimitStdDevTextBox.Steps = -1D;
            this.AccLimitStdDevTextBox.TabIndex = 7;
            this.AccLimitStdDevTextBox.Text = "0.000000E+000";
            this.AccLimitStdDevTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.AccLimitStdDevTextBox.Value = 0D;
            // 
            // AccLimitPercentTextBox
            // 
            this.AccLimitPercentTextBox.Location = new System.Drawing.Point(196, 220);
            this.AccLimitPercentTextBox.Max = 1.7976931348623157E+308D;
            this.AccLimitPercentTextBox.Min = 0D;
            this.AccLimitPercentTextBox.Name = "AccLimitPercentTextBox";
            this.AccLimitPercentTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.AccLimitPercentTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.AccLimitPercentTextBox.Size = new System.Drawing.Size(200, 20);
            this.AccLimitPercentTextBox.Steps = -1D;
            this.AccLimitPercentTextBox.TabIndex = 8;
            this.AccLimitPercentTextBox.Text = "0.000000E+000";
            this.AccLimitPercentTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.AccLimitPercentTextBox.Value = 0D;
            // 
            // MovementDistanceTextBox
            // 
            this.MovementDistanceTextBox.Location = new System.Drawing.Point(285, 297);
            this.MovementDistanceTextBox.Max = 1.7976931348623157E+308D;
            this.MovementDistanceTextBox.Min = 0D;
            this.MovementDistanceTextBox.Name = "MovementDistanceTextBox";
            this.MovementDistanceTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.MovementDistanceTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.MovementDistanceTextBox.Size = new System.Drawing.Size(100, 20);
            this.MovementDistanceTextBox.Steps = -1D;
            this.MovementDistanceTextBox.TabIndex = 10;
            this.MovementDistanceTextBox.Text = "0.000000E+000";
            this.MovementDistanceTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.MovementDistanceTextBox.Value = 0D;
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(411, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 11;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(411, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 12;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(411, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 13;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // AddASourceCheckBox
            // 
            this.AddASourceCheckBox.AutoSize = true;
            this.AddASourceCheckBox.Location = new System.Drawing.Point(39, 262);
            this.AddASourceCheckBox.MaximumSize = new System.Drawing.Size(500, 0);
            this.AddASourceCheckBox.Name = "AddASourceCheckBox";
            this.AddASourceCheckBox.Size = new System.Drawing.Size(346, 17);
            this.AddASourceCheckBox.TabIndex = 14;
            this.AddASourceCheckBox.Text = "Use add-a-source Cf252 source for normalization test measurements";
            this.AddASourceCheckBox.UseVisualStyleBackColor = true;
            this.AddASourceCheckBox.CheckedChanged += new System.EventHandler(this.AddASourceCheckBox_CheckedChanged);
            // 
            // SourceIdLabel
            // 
            this.SourceIdLabel.AutoSize = true;
            this.SourceIdLabel.Location = new System.Drawing.Point(98, 15);
            this.SourceIdLabel.Name = "SourceIdLabel";
            this.SourceIdLabel.Size = new System.Drawing.Size(81, 13);
            this.SourceIdLabel.TabIndex = 15;
            this.SourceIdLabel.Text = "Cf252 source id";
            // 
            // NormConstLabel
            // 
            this.NormConstLabel.AutoSize = true;
            this.NormConstLabel.Location = new System.Drawing.Point(65, 41);
            this.NormConstLabel.Name = "NormConstLabel";
            this.NormConstLabel.Size = new System.Drawing.Size(114, 13);
            this.NormConstLabel.TabIndex = 16;
            this.NormConstLabel.Text = "Normalization constant";
            // 
            // NormConstErrorLabel
            // 
            this.NormConstErrorLabel.AutoSize = true;
            this.NormConstErrorLabel.Location = new System.Drawing.Point(41, 67);
            this.NormConstErrorLabel.Name = "NormConstErrorLabel";
            this.NormConstErrorLabel.Size = new System.Drawing.Size(138, 13);
            this.NormConstErrorLabel.TabIndex = 17;
            this.NormConstErrorLabel.Text = "Normalization constant error";
            // 
            // RefDoublesRateLabel
            // 
            this.RefDoublesRateLabel.AutoSize = true;
            this.RefDoublesRateLabel.Location = new System.Drawing.Point(35, 93);
            this.RefDoublesRateLabel.Name = "RefDoublesRateLabel";
            this.RefDoublesRateLabel.Size = new System.Drawing.Size(144, 13);
            this.RefDoublesRateLabel.TabIndex = 18;
            this.RefDoublesRateLabel.Text = "Cf252 reference doubles rate";
            // 
            // RefDoublesRateErrorLabel
            // 
            this.RefDoublesRateErrorLabel.AutoSize = true;
            this.RefDoublesRateErrorLabel.Location = new System.Drawing.Point(11, 119);
            this.RefDoublesRateErrorLabel.Name = "RefDoublesRateErrorLabel";
            this.RefDoublesRateErrorLabel.Size = new System.Drawing.Size(168, 13);
            this.RefDoublesRateErrorLabel.TabIndex = 19;
            this.RefDoublesRateErrorLabel.Text = "Cf252 reference doubles rate error";
            // 
            // RefDoublesDateLabel
            // 
            this.RefDoublesDateLabel.AutoSize = true;
            this.RefDoublesDateLabel.Location = new System.Drawing.Point(32, 145);
            this.RefDoublesDateLabel.Name = "RefDoublesDateLabel";
            this.RefDoublesDateLabel.Size = new System.Drawing.Size(147, 13);
            this.RefDoublesDateLabel.TabIndex = 20;
            this.RefDoublesDateLabel.Text = "Cf252 reference doubles date";
            // 
            // PrecisionLimitLabel
            // 
            this.PrecisionLimitLabel.AutoSize = true;
            this.PrecisionLimitLabel.Location = new System.Drawing.Point(92, 171);
            this.PrecisionLimitLabel.Name = "PrecisionLimitLabel";
            this.PrecisionLimitLabel.Size = new System.Drawing.Size(87, 13);
            this.PrecisionLimitLabel.TabIndex = 21;
            this.PrecisionLimitLabel.Text = "Precision limit (%)";
            // 
            // AccLimitStdDevLabel
            // 
            this.AccLimitStdDevLabel.AutoSize = true;
            this.AccLimitStdDevLabel.Location = new System.Drawing.Point(50, 197);
            this.AccLimitStdDevLabel.Name = "AccLimitStdDevLabel";
            this.AccLimitStdDevLabel.Size = new System.Drawing.Size(129, 13);
            this.AccLimitStdDevLabel.TabIndex = 22;
            this.AccLimitStdDevLabel.Text = "Acceptance limit (std dev)";
            // 
            // AccLimitPercentLabel
            // 
            this.AccLimitPercentLabel.AutoSize = true;
            this.AccLimitPercentLabel.Location = new System.Drawing.Point(77, 223);
            this.AccLimitPercentLabel.Name = "AccLimitPercentLabel";
            this.AccLimitPercentLabel.Size = new System.Drawing.Size(102, 13);
            this.AccLimitPercentLabel.TabIndex = 23;
            this.AccLimitPercentLabel.Text = "Acceptance limit (%)";
            // 
            // MovementDistanceLabel
            // 
            this.MovementDistanceLabel.AutoSize = true;
            this.MovementDistanceLabel.Location = new System.Drawing.Point(20, 300);
            this.MovementDistanceLabel.MaximumSize = new System.Drawing.Size(260, 0);
            this.MovementDistanceLabel.Name = "MovementDistanceLabel";
            this.MovementDistanceLabel.Size = new System.Drawing.Size(259, 26);
            this.MovementDistanceLabel.TabIndex = 24;
            this.MovementDistanceLabel.Text = "Distance to move Cf252 source from home to position for normalization test measur" +
    "ements (inches)";
            // 
            // RefDoublesDateTimePicker
            // 
            this.RefDoublesDateTimePicker.Location = new System.Drawing.Point(196, 142);
            this.RefDoublesDateTimePicker.Name = "RefDoublesDateTimePicker";
            this.RefDoublesDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.RefDoublesDateTimePicker.TabIndex = 25;
            this.RefDoublesDateTimePicker.ValueChanged += new System.EventHandler(this.RefDoublesDateTimePicker_ValueChanged);
            // 
            // IDDCf252Normalization
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 345);
            this.Controls.Add(this.RefDoublesDateTimePicker);
            this.Controls.Add(this.MovementDistanceLabel);
            this.Controls.Add(this.AccLimitPercentLabel);
            this.Controls.Add(this.AccLimitStdDevLabel);
            this.Controls.Add(this.PrecisionLimitLabel);
            this.Controls.Add(this.RefDoublesDateLabel);
            this.Controls.Add(this.RefDoublesRateErrorLabel);
            this.Controls.Add(this.RefDoublesRateLabel);
            this.Controls.Add(this.NormConstErrorLabel);
            this.Controls.Add(this.NormConstLabel);
            this.Controls.Add(this.SourceIdLabel);
            this.Controls.Add(this.AddASourceCheckBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.MovementDistanceTextBox);
            this.Controls.Add(this.AccLimitPercentTextBox);
            this.Controls.Add(this.AccLimitStdDevTextBox);
            this.Controls.Add(this.PrecisionLimitTextBox);
            this.Controls.Add(this.RefDoublesRateErrorTextBox);
            this.Controls.Add(this.RefDoublesRateTextBox);
            this.Controls.Add(this.NormConstErrorTextBox);
            this.Controls.Add(this.NormConstTextBox);
            this.Controls.Add(this.SourceIdTextBox);
            this.Name = "IDDCf252Normalization";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Cf252 Normalization Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SourceIdTextBox;
        private NumericTextBox NormConstTextBox;
        private NumericTextBox NormConstErrorTextBox;
        private NumericTextBox RefDoublesRateTextBox;
        private NumericTextBox RefDoublesRateErrorTextBox;
        private NumericTextBox PrecisionLimitTextBox;
        private NumericTextBox AccLimitStdDevTextBox;
        private NumericTextBox AccLimitPercentTextBox;
        private NumericTextBox MovementDistanceTextBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.CheckBox AddASourceCheckBox;
        private System.Windows.Forms.Label SourceIdLabel;
        private System.Windows.Forms.Label NormConstLabel;
        private System.Windows.Forms.Label NormConstErrorLabel;
        private System.Windows.Forms.Label RefDoublesRateLabel;
        private System.Windows.Forms.Label RefDoublesRateErrorLabel;
        private System.Windows.Forms.Label RefDoublesDateLabel;
        private System.Windows.Forms.Label PrecisionLimitLabel;
        private System.Windows.Forms.Label AccLimitStdDevLabel;
        private System.Windows.Forms.Label AccLimitPercentLabel;
        private System.Windows.Forms.Label MovementDistanceLabel;
        private System.Windows.Forms.DateTimePicker RefDoublesDateTimePicker;
    }
}