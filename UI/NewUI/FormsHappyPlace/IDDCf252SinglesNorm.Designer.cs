namespace NewUI
{
    partial class IDDCf252SinglesNorm
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
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CfSourceIdTextBox = new System.Windows.Forms.TextBox();
            this.NormConstTextBox = new NewUI.NumericTextBox();
            this.NormConstErrTextBox = new NewUI.NumericTextBox();
            this.RefSinglesRateTextBox = new NewUI.NumericTextBox();
            this.RefSinglesRateErrorTextBox = new NewUI.NumericTextBox();
            this.PrecisionLimitTextBox = new NewUI.NumericTextBox();
            this.AccLimitStdDevTextBox = new NewUI.NumericTextBox();
            this.AccLimitPercentTextBox = new NewUI.NumericTextBox();
            this.CfSourceIdLabel = new System.Windows.Forms.Label();
            this.NormConstLabel = new System.Windows.Forms.Label();
            this.NormConstErrLabel = new System.Windows.Forms.Label();
            this.RefSinglesRateLabel = new System.Windows.Forms.Label();
            this.RefSinglesRateErrorLabel = new System.Windows.Forms.Label();
            this.RefSinglesDateLabel = new System.Windows.Forms.Label();
            this.PrecisionLimitLabel = new System.Windows.Forms.Label();
            this.AccLimitStdDevLabel = new System.Windows.Forms.Label();
            this.AccLimitPercentLabel = new System.Windows.Forms.Label();
            this.RefSinglesDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(408, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 0;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(408, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 1;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(408, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 2;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // CfSourceIdTextBox
            // 
            this.CfSourceIdTextBox.Location = new System.Drawing.Point(192, 12);
            this.CfSourceIdTextBox.Name = "CfSourceIdTextBox";
            this.CfSourceIdTextBox.Size = new System.Drawing.Size(200, 20);
            this.CfSourceIdTextBox.TabIndex = 3;
            this.CfSourceIdTextBox.Leave += new System.EventHandler(this.CfSourceIdTextBox_Leave);
            // 
            // NormConstTextBox
            // 
            this.NormConstTextBox.Location = new System.Drawing.Point(192, 38);
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
            this.NormConstTextBox.TabIndex = 4;
            this.NormConstTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.NormConstTextBox.Value = 0D;
            // 
            // NormConstErrTextBox
            // 
            this.NormConstErrTextBox.Location = new System.Drawing.Point(192, 64);
            this.NormConstErrTextBox.Max = 1.7976931348623157E+308D;
            this.NormConstErrTextBox.Min = 0D;
            this.NormConstErrTextBox.Name = "NormConstErrTextBox";
            this.NormConstErrTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.NormConstErrTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.NormConstErrTextBox.Size = new System.Drawing.Size(200, 20);
            this.NormConstErrTextBox.Steps = -1D;
            this.NormConstErrTextBox.TabIndex = 5;
            this.NormConstErrTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.NormConstErrTextBox.Value = 0D;
            // 
            // RefSinglesRateTextBox
            // 
            this.RefSinglesRateTextBox.Location = new System.Drawing.Point(192, 90);
            this.RefSinglesRateTextBox.Max = 1.7976931348623157E+308D;
            this.RefSinglesRateTextBox.Min = 0D;
            this.RefSinglesRateTextBox.Name = "RefSinglesRateTextBox";
            this.RefSinglesRateTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.RefSinglesRateTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.RefSinglesRateTextBox.Size = new System.Drawing.Size(200, 20);
            this.RefSinglesRateTextBox.Steps = -1D;
            this.RefSinglesRateTextBox.TabIndex = 6;
            this.RefSinglesRateTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.RefSinglesRateTextBox.Value = 0D;
            // 
            // RefSinglesRateErrorTextBox
            // 
            this.RefSinglesRateErrorTextBox.Location = new System.Drawing.Point(192, 116);
            this.RefSinglesRateErrorTextBox.Max = 1.7976931348623157E+308D;
            this.RefSinglesRateErrorTextBox.Min = 0D;
            this.RefSinglesRateErrorTextBox.Name = "RefSinglesRateErrorTextBox";
            this.RefSinglesRateErrorTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.RefSinglesRateErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.RefSinglesRateErrorTextBox.Size = new System.Drawing.Size(200, 20);
            this.RefSinglesRateErrorTextBox.Steps = -1D;
            this.RefSinglesRateErrorTextBox.TabIndex = 7;
            this.RefSinglesRateErrorTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.RefSinglesRateErrorTextBox.Value = 0D;
            // 
            // PrecisionLimitTextBox
            // 
            this.PrecisionLimitTextBox.Location = new System.Drawing.Point(192, 168);
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
            this.PrecisionLimitTextBox.TabIndex = 9;
            this.PrecisionLimitTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.PrecisionLimitTextBox.Value = 0D;
            // 
            // AccLimitStdDevTextBox
            // 
            this.AccLimitStdDevTextBox.Location = new System.Drawing.Point(192, 194);
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
            this.AccLimitStdDevTextBox.TabIndex = 10;
            this.AccLimitStdDevTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.AccLimitStdDevTextBox.Value = 0D;
            // 
            // AccLimitPercentTextBox
            // 
            this.AccLimitPercentTextBox.Location = new System.Drawing.Point(192, 220);
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
            this.AccLimitPercentTextBox.TabIndex = 11;
            this.AccLimitPercentTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.AccLimitPercentTextBox.Value = 0D;
            // 
            // CfSourceIdLabel
            // 
            this.CfSourceIdLabel.AutoSize = true;
            this.CfSourceIdLabel.Location = new System.Drawing.Point(96, 15);
            this.CfSourceIdLabel.Name = "CfSourceIdLabel";
            this.CfSourceIdLabel.Size = new System.Drawing.Size(81, 13);
            this.CfSourceIdLabel.TabIndex = 13;
            this.CfSourceIdLabel.Text = "Cf252 source id";
            // 
            // NormConstLabel
            // 
            this.NormConstLabel.AutoSize = true;
            this.NormConstLabel.Location = new System.Drawing.Point(63, 41);
            this.NormConstLabel.Name = "NormConstLabel";
            this.NormConstLabel.Size = new System.Drawing.Size(114, 13);
            this.NormConstLabel.TabIndex = 14;
            this.NormConstLabel.Text = "Normalization constant";
            // 
            // NormConstErrLabel
            // 
            this.NormConstErrLabel.AutoSize = true;
            this.NormConstErrLabel.Location = new System.Drawing.Point(39, 67);
            this.NormConstErrLabel.Name = "NormConstErrLabel";
            this.NormConstErrLabel.Size = new System.Drawing.Size(138, 13);
            this.NormConstErrLabel.TabIndex = 15;
            this.NormConstErrLabel.Text = "Normalization constant error";
            // 
            // RefSinglesRateLabel
            // 
            this.RefSinglesRateLabel.AutoSize = true;
            this.RefSinglesRateLabel.Location = new System.Drawing.Point(38, 93);
            this.RefSinglesRateLabel.Name = "RefSinglesRateLabel";
            this.RefSinglesRateLabel.Size = new System.Drawing.Size(139, 13);
            this.RefSinglesRateLabel.TabIndex = 16;
            this.RefSinglesRateLabel.Text = "Cf252 reference singles rate";
            // 
            // RefSinglesRateErrorLabel
            // 
            this.RefSinglesRateErrorLabel.AutoSize = true;
            this.RefSinglesRateErrorLabel.Location = new System.Drawing.Point(14, 119);
            this.RefSinglesRateErrorLabel.Name = "RefSinglesRateErrorLabel";
            this.RefSinglesRateErrorLabel.Size = new System.Drawing.Size(163, 13);
            this.RefSinglesRateErrorLabel.TabIndex = 17;
            this.RefSinglesRateErrorLabel.Text = "Cf252 reference singles rate error";
            // 
            // RefSinglesDateLabel
            // 
            this.RefSinglesDateLabel.AutoSize = true;
            this.RefSinglesDateLabel.Location = new System.Drawing.Point(35, 145);
            this.RefSinglesDateLabel.Name = "RefSinglesDateLabel";
            this.RefSinglesDateLabel.Size = new System.Drawing.Size(142, 13);
            this.RefSinglesDateLabel.TabIndex = 18;
            this.RefSinglesDateLabel.Text = "Cf252 reference singles date";
            // 
            // PrecisionLimitLabel
            // 
            this.PrecisionLimitLabel.AutoSize = true;
            this.PrecisionLimitLabel.Location = new System.Drawing.Point(90, 171);
            this.PrecisionLimitLabel.Name = "PrecisionLimitLabel";
            this.PrecisionLimitLabel.Size = new System.Drawing.Size(87, 13);
            this.PrecisionLimitLabel.TabIndex = 19;
            this.PrecisionLimitLabel.Text = "Precision limit (%)";
            // 
            // AccLimitStdDevLabel
            // 
            this.AccLimitStdDevLabel.AutoSize = true;
            this.AccLimitStdDevLabel.Location = new System.Drawing.Point(48, 197);
            this.AccLimitStdDevLabel.Name = "AccLimitStdDevLabel";
            this.AccLimitStdDevLabel.Size = new System.Drawing.Size(129, 13);
            this.AccLimitStdDevLabel.TabIndex = 20;
            this.AccLimitStdDevLabel.Text = "Acceptance limit (std dev)";
            // 
            // AccLimitPercentLabel
            // 
            this.AccLimitPercentLabel.AutoSize = true;
            this.AccLimitPercentLabel.Location = new System.Drawing.Point(75, 223);
            this.AccLimitPercentLabel.Name = "AccLimitPercentLabel";
            this.AccLimitPercentLabel.Size = new System.Drawing.Size(102, 13);
            this.AccLimitPercentLabel.TabIndex = 21;
            this.AccLimitPercentLabel.Text = "Acceptance limit (%)";
            // 
            // RefSinglesDateTimePicker
            // 
            this.RefSinglesDateTimePicker.Location = new System.Drawing.Point(192, 142);
            this.RefSinglesDateTimePicker.Name = "RefSinglesDateTimePicker";
            this.RefSinglesDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.RefSinglesDateTimePicker.TabIndex = 22;
            this.RefSinglesDateTimePicker.ValueChanged += new System.EventHandler(this.RefSinglesDateTimePicker_ValueChanged);
            // 
            // IDDCf252SinglesNorm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 257);
            this.Controls.Add(this.RefSinglesDateTimePicker);
            this.Controls.Add(this.AccLimitPercentLabel);
            this.Controls.Add(this.AccLimitStdDevLabel);
            this.Controls.Add(this.PrecisionLimitLabel);
            this.Controls.Add(this.RefSinglesDateLabel);
            this.Controls.Add(this.RefSinglesRateErrorLabel);
            this.Controls.Add(this.RefSinglesRateLabel);
            this.Controls.Add(this.NormConstErrLabel);
            this.Controls.Add(this.NormConstLabel);
            this.Controls.Add(this.CfSourceIdLabel);
            this.Controls.Add(this.AccLimitPercentTextBox);
            this.Controls.Add(this.AccLimitStdDevTextBox);
            this.Controls.Add(this.PrecisionLimitTextBox);
            this.Controls.Add(this.RefSinglesRateErrorTextBox);
            this.Controls.Add(this.RefSinglesRateTextBox);
            this.Controls.Add(this.NormConstErrTextBox);
            this.Controls.Add(this.NormConstTextBox);
            this.Controls.Add(this.CfSourceIdTextBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDCf252SinglesNorm";
            this.Text = "Cf252 Normalization Setup";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.TextBox CfSourceIdTextBox;
        private NumericTextBox NormConstTextBox;
        private NumericTextBox NormConstErrTextBox;
        private NumericTextBox RefSinglesRateTextBox;
        private NumericTextBox RefSinglesRateErrorTextBox;
        private NumericTextBox PrecisionLimitTextBox;
        private NumericTextBox AccLimitStdDevTextBox;
        private NumericTextBox AccLimitPercentTextBox;
        private System.Windows.Forms.Label CfSourceIdLabel;
        private System.Windows.Forms.Label NormConstLabel;
        private System.Windows.Forms.Label NormConstErrLabel;
        private System.Windows.Forms.Label RefSinglesRateLabel;
        private System.Windows.Forms.Label RefSinglesRateErrorLabel;
        private System.Windows.Forms.Label RefSinglesDateLabel;
        private System.Windows.Forms.Label PrecisionLimitLabel;
        private System.Windows.Forms.Label AccLimitStdDevLabel;
        private System.Windows.Forms.Label AccLimitPercentLabel;
        private System.Windows.Forms.DateTimePicker RefSinglesDateTimePicker;
    }
}