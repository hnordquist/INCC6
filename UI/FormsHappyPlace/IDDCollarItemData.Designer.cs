namespace UI
{
    partial class IDDCollarItemData
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
            this.PoisonRodTypeComboBox = new System.Windows.Forms.ComboBox();
            this.LengthTextBox = new UI.NumericTextBox();
            this.LengthErrorTextBox = new UI.NumericTextBox();
            this.TotalU235TextBox = new UI.NumericTextBox();
            this.TotalU235ErrorTextBox = new UI.NumericTextBox();
            this.TotalU238TextBox = new UI.NumericTextBox();
            this.TotalRodsTextBox = new UI.NumericTextBox();
            this.PoisonTextBox = new UI.NumericTextBox();
            this.PoisonErrorTextBox = new UI.NumericTextBox();
            this.TotalU238ErrorTextBox = new UI.NumericTextBox();
            this.TotalPoisonRodsTextBox = new UI.NumericTextBox();
            this.PoisonRodTypeLabel = new System.Windows.Forms.Label();
            this.LengthLabel = new System.Windows.Forms.Label();
            this.LengthErrorLabel = new System.Windows.Forms.Label();
            this.TotalU235Label = new System.Windows.Forms.Label();
            this.TotalU235ErrorLabel = new System.Windows.Forms.Label();
            this.TotalU238Label = new System.Windows.Forms.Label();
            this.TotalU238ErrorLabel = new System.Windows.Forms.Label();
            this.TotalRodsLabel = new System.Windows.Forms.Label();
            this.TotalPoisonRodsLabel = new System.Windows.Forms.Label();
            this.PoisonLabel = new System.Windows.Forms.Label();
            this.PoisonErrorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(253, 14);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 11;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(253, 43);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 12;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(253, 72);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 13;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // PoisonRodTypeComboBox
            // 
            this.PoisonRodTypeComboBox.FormattingEnabled = true;
            this.PoisonRodTypeComboBox.Location = new System.Drawing.Point(111, 16);
            this.PoisonRodTypeComboBox.Name = "PoisonRodTypeComboBox";
            this.PoisonRodTypeComboBox.Size = new System.Drawing.Size(121, 21);
            this.PoisonRodTypeComboBox.TabIndex = 0;
            this.PoisonRodTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.PoisonRodTypeComboBox_SelectedIndexChanged);
            // 
            // LengthTextBox
            // 
            this.LengthTextBox.Location = new System.Drawing.Point(111, 51);
            this.LengthTextBox.Max = 1.7976931348623157E+308D;
            this.LengthTextBox.Min = 0D;
            this.LengthTextBox.Name = "LengthTextBox";
            this.LengthTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.LengthTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.LengthTextBox.Size = new System.Drawing.Size(121, 20);
            this.LengthTextBox.Steps = -1D;
            this.LengthTextBox.TabIndex = 1;
            this.LengthTextBox.Text = "0.000000E+000";
            this.LengthTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.LengthTextBox.Value = 0D;
            // 
            // LengthErrorTextBox
            // 
            this.LengthErrorTextBox.Location = new System.Drawing.Point(111, 77);
            this.LengthErrorTextBox.Max = 1.7976931348623157E+308D;
            this.LengthErrorTextBox.Min = 0D;
            this.LengthErrorTextBox.Name = "LengthErrorTextBox";
            this.LengthErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.LengthErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.LengthErrorTextBox.Size = new System.Drawing.Size(121, 20);
            this.LengthErrorTextBox.Steps = -1D;
            this.LengthErrorTextBox.TabIndex = 2;
            this.LengthErrorTextBox.Text = "0.000000E+000";
            this.LengthErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.LengthErrorTextBox.Value = 0D;
            // 
            // TotalU235TextBox
            // 
            this.TotalU235TextBox.Location = new System.Drawing.Point(111, 103);
            this.TotalU235TextBox.Max = 1.7976931348623157E+308D;
            this.TotalU235TextBox.Min = 0D;
            this.TotalU235TextBox.Name = "TotalU235TextBox";
            this.TotalU235TextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.TotalU235TextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.TotalU235TextBox.Size = new System.Drawing.Size(121, 20);
            this.TotalU235TextBox.Steps = -1D;
            this.TotalU235TextBox.TabIndex = 3;
            this.TotalU235TextBox.Text = "0.000000E+000";
            this.TotalU235TextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.TotalU235TextBox.Value = 0D;
            // 
            // TotalU235ErrorTextBox
            // 
            this.TotalU235ErrorTextBox.Location = new System.Drawing.Point(111, 129);
            this.TotalU235ErrorTextBox.Max = 1.7976931348623157E+308D;
            this.TotalU235ErrorTextBox.Min = 0D;
            this.TotalU235ErrorTextBox.Name = "TotalU235ErrorTextBox";
            this.TotalU235ErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.TotalU235ErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.TotalU235ErrorTextBox.Size = new System.Drawing.Size(121, 20);
            this.TotalU235ErrorTextBox.Steps = -1D;
            this.TotalU235ErrorTextBox.TabIndex = 4;
            this.TotalU235ErrorTextBox.Text = "0.000000E+000";
            this.TotalU235ErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.TotalU235ErrorTextBox.Value = 0D;
            // 
            // TotalU238TextBox
            // 
            this.TotalU238TextBox.Location = new System.Drawing.Point(111, 155);
            this.TotalU238TextBox.Max = 1.7976931348623157E+308D;
            this.TotalU238TextBox.Min = 0D;
            this.TotalU238TextBox.Name = "TotalU238TextBox";
            this.TotalU238TextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.TotalU238TextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.TotalU238TextBox.Size = new System.Drawing.Size(121, 20);
            this.TotalU238TextBox.Steps = -1D;
            this.TotalU238TextBox.TabIndex = 5;
            this.TotalU238TextBox.Text = "0.000000E+000";
            this.TotalU238TextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.TotalU238TextBox.Value = 0D;
            // 
            // TotalRodsTextBox
            // 
            this.TotalRodsTextBox.Location = new System.Drawing.Point(111, 207);
            this.TotalRodsTextBox.Max = 1.7976931348623157E+308D;
            this.TotalRodsTextBox.Min = 0D;
            this.TotalRodsTextBox.Name = "TotalRodsTextBox";
            this.TotalRodsTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.TotalRodsTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.TotalRodsTextBox.Size = new System.Drawing.Size(121, 20);
            this.TotalRodsTextBox.Steps = -1D;
            this.TotalRodsTextBox.TabIndex = 7;
            this.TotalRodsTextBox.Text = "0.000000E+000";
            this.TotalRodsTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.TotalRodsTextBox.Value = 0D;
            // 
            // PoisonTextBox
            // 
            this.PoisonTextBox.Location = new System.Drawing.Point(111, 259);
            this.PoisonTextBox.Max = 1.7976931348623157E+308D;
            this.PoisonTextBox.Min = 0D;
            this.PoisonTextBox.Name = "PoisonTextBox";
            this.PoisonTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.PoisonTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.PoisonTextBox.Size = new System.Drawing.Size(121, 20);
            this.PoisonTextBox.Steps = -1D;
            this.PoisonTextBox.TabIndex = 9;
            this.PoisonTextBox.Text = "0.000000E+000";
            this.PoisonTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.PoisonTextBox.Value = 0D;
            // 
            // PoisonErrorTextBox
            // 
            this.PoisonErrorTextBox.Location = new System.Drawing.Point(111, 285);
            this.PoisonErrorTextBox.Max = 1.7976931348623157E+308D;
            this.PoisonErrorTextBox.Min = 0D;
            this.PoisonErrorTextBox.Name = "PoisonErrorTextBox";
            this.PoisonErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.PoisonErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.PoisonErrorTextBox.Size = new System.Drawing.Size(121, 20);
            this.PoisonErrorTextBox.Steps = -1D;
            this.PoisonErrorTextBox.TabIndex = 10;
            this.PoisonErrorTextBox.Text = "0.000000E+000";
            this.PoisonErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.PoisonErrorTextBox.Value = 0D;
            // 
            // TotalU238ErrorTextBox
            // 
            this.TotalU238ErrorTextBox.Location = new System.Drawing.Point(111, 181);
            this.TotalU238ErrorTextBox.Max = 1.7976931348623157E+308D;
            this.TotalU238ErrorTextBox.Min = 0D;
            this.TotalU238ErrorTextBox.Name = "TotalU238ErrorTextBox";
            this.TotalU238ErrorTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.TotalU238ErrorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.TotalU238ErrorTextBox.Size = new System.Drawing.Size(121, 20);
            this.TotalU238ErrorTextBox.Steps = -1D;
            this.TotalU238ErrorTextBox.TabIndex = 6;
            this.TotalU238ErrorTextBox.Text = "0.000000E+000";
            this.TotalU238ErrorTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.TotalU238ErrorTextBox.Value = 0D;
            // 
            // TotalPoisonRodsTextBox
            // 
            this.TotalPoisonRodsTextBox.Location = new System.Drawing.Point(111, 233);
            this.TotalPoisonRodsTextBox.Max = 1.7976931348623157E+308D;
            this.TotalPoisonRodsTextBox.Min = 0D;
            this.TotalPoisonRodsTextBox.Name = "TotalPoisonRodsTextBox";
            this.TotalPoisonRodsTextBox.NumberFormat = UI.NumericTextBox.Formatter.E6;
            this.TotalPoisonRodsTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.TotalPoisonRodsTextBox.Size = new System.Drawing.Size(121, 20);
            this.TotalPoisonRodsTextBox.Steps = -1D;
            this.TotalPoisonRodsTextBox.TabIndex = 8;
            this.TotalPoisonRodsTextBox.Text = "0.000000E+000";
            this.TotalPoisonRodsTextBox.ToValidate = UI.NumericTextBox.ValidateType.Double;
            this.TotalPoisonRodsTextBox.Value = 0D;
            // 
            // PoisonRodTypeLabel
            // 
            this.PoisonRodTypeLabel.AutoSize = true;
            this.PoisonRodTypeLabel.Location = new System.Drawing.Point(25, 19);
            this.PoisonRodTypeLabel.Name = "PoisonRodTypeLabel";
            this.PoisonRodTypeLabel.Size = new System.Drawing.Size(80, 13);
            this.PoisonRodTypeLabel.TabIndex = 17;
            this.PoisonRodTypeLabel.Text = "Poison rod type";
            // 
            // LengthLabel
            // 
            this.LengthLabel.AutoSize = true;
            this.LengthLabel.Location = new System.Drawing.Point(42, 53);
            this.LengthLabel.Name = "LengthLabel";
            this.LengthLabel.Size = new System.Drawing.Size(63, 13);
            this.LengthLabel.TabIndex = 18;
            this.LengthLabel.Text = "Length (cm)";
            // 
            // LengthErrorLabel
            // 
            this.LengthErrorLabel.AutoSize = true;
            this.LengthErrorLabel.Location = new System.Drawing.Point(41, 80);
            this.LengthErrorLabel.Name = "LengthErrorLabel";
            this.LengthErrorLabel.Size = new System.Drawing.Size(64, 13);
            this.LengthErrorLabel.TabIndex = 19;
            this.LengthErrorLabel.Text = "Length error";
            // 
            // TotalU235Label
            // 
            this.TotalU235Label.AutoSize = true;
            this.TotalU235Label.Location = new System.Drawing.Point(30, 106);
            this.TotalU235Label.Name = "TotalU235Label";
            this.TotalU235Label.Size = new System.Drawing.Size(75, 13);
            this.TotalU235Label.TabIndex = 20;
            this.TotalU235Label.Text = "Total U235 (g)";
            // 
            // TotalU235ErrorLabel
            // 
            this.TotalU235ErrorLabel.AutoSize = true;
            this.TotalU235ErrorLabel.Location = new System.Drawing.Point(21, 132);
            this.TotalU235ErrorLabel.Name = "TotalU235ErrorLabel";
            this.TotalU235ErrorLabel.Size = new System.Drawing.Size(84, 13);
            this.TotalU235ErrorLabel.TabIndex = 21;
            this.TotalU235ErrorLabel.Text = "Total U235 error";
            // 
            // TotalU238Label
            // 
            this.TotalU238Label.AutoSize = true;
            this.TotalU238Label.Location = new System.Drawing.Point(30, 158);
            this.TotalU238Label.Name = "TotalU238Label";
            this.TotalU238Label.Size = new System.Drawing.Size(75, 13);
            this.TotalU238Label.TabIndex = 22;
            this.TotalU238Label.Text = "Total U238 (g)";
            // 
            // TotalU238ErrorLabel
            // 
            this.TotalU238ErrorLabel.AutoSize = true;
            this.TotalU238ErrorLabel.Location = new System.Drawing.Point(21, 184);
            this.TotalU238ErrorLabel.Name = "TotalU238ErrorLabel";
            this.TotalU238ErrorLabel.Size = new System.Drawing.Size(84, 13);
            this.TotalU238ErrorLabel.TabIndex = 23;
            this.TotalU238ErrorLabel.Text = "Total U238 error";
            // 
            // TotalRodsLabel
            // 
            this.TotalRodsLabel.AutoSize = true;
            this.TotalRodsLabel.Location = new System.Drawing.Point(51, 210);
            this.TotalRodsLabel.Name = "TotalRodsLabel";
            this.TotalRodsLabel.Size = new System.Drawing.Size(54, 13);
            this.TotalRodsLabel.TabIndex = 24;
            this.TotalRodsLabel.Text = "Total rods";
            // 
            // TotalPoisonRodsLabel
            // 
            this.TotalPoisonRodsLabel.AutoSize = true;
            this.TotalPoisonRodsLabel.Location = new System.Drawing.Point(17, 236);
            this.TotalPoisonRodsLabel.Name = "TotalPoisonRodsLabel";
            this.TotalPoisonRodsLabel.Size = new System.Drawing.Size(88, 13);
            this.TotalPoisonRodsLabel.TabIndex = 25;
            this.TotalPoisonRodsLabel.Text = "Total poison rods";
            // 
            // PoisonLabel
            // 
            this.PoisonLabel.AutoSize = true;
            this.PoisonLabel.Location = new System.Drawing.Point(55, 262);
            this.PoisonLabel.Name = "PoisonLabel";
            this.PoisonLabel.Size = new System.Drawing.Size(50, 13);
            this.PoisonLabel.TabIndex = 26;
            this.PoisonLabel.Text = "Poison %";
            // 
            // PoisonErrorLabel
            // 
            this.PoisonErrorLabel.AutoSize = true;
            this.PoisonErrorLabel.Location = new System.Drawing.Point(31, 288);
            this.PoisonErrorLabel.Name = "PoisonErrorLabel";
            this.PoisonErrorLabel.Size = new System.Drawing.Size(74, 13);
            this.PoisonErrorLabel.TabIndex = 27;
            this.PoisonErrorLabel.Text = "Poison % error";
            // 
            // IDDCollarItemData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 318);
            this.Controls.Add(this.PoisonErrorLabel);
            this.Controls.Add(this.PoisonLabel);
            this.Controls.Add(this.TotalPoisonRodsLabel);
            this.Controls.Add(this.TotalRodsLabel);
            this.Controls.Add(this.TotalU238ErrorLabel);
            this.Controls.Add(this.TotalU238Label);
            this.Controls.Add(this.TotalU235ErrorLabel);
            this.Controls.Add(this.TotalU235Label);
            this.Controls.Add(this.LengthErrorLabel);
            this.Controls.Add(this.LengthLabel);
            this.Controls.Add(this.PoisonRodTypeLabel);
            this.Controls.Add(this.TotalPoisonRodsTextBox);
            this.Controls.Add(this.TotalU238ErrorTextBox);
            this.Controls.Add(this.PoisonErrorTextBox);
            this.Controls.Add(this.PoisonTextBox);
            this.Controls.Add(this.TotalRodsTextBox);
            this.Controls.Add(this.TotalU238TextBox);
            this.Controls.Add(this.TotalU235ErrorTextBox);
            this.Controls.Add(this.TotalU235TextBox);
            this.Controls.Add(this.LengthErrorTextBox);
            this.Controls.Add(this.LengthTextBox);
            this.Controls.Add(this.PoisonRodTypeComboBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Name = "IDDCollarItemData";
            this.Text = "Enter Collar Item Data";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.ComboBox PoisonRodTypeComboBox;
        private NumericTextBox LengthTextBox;
        private NumericTextBox LengthErrorTextBox;
        private NumericTextBox TotalU235TextBox;
        private NumericTextBox TotalU235ErrorTextBox;
        private NumericTextBox TotalU238TextBox;
        private NumericTextBox TotalRodsTextBox;
        private NumericTextBox PoisonTextBox;
        private NumericTextBox PoisonErrorTextBox;
        private NumericTextBox TotalU238ErrorTextBox;
        private NumericTextBox TotalPoisonRodsTextBox;
        private System.Windows.Forms.Label PoisonRodTypeLabel;
        private System.Windows.Forms.Label LengthLabel;
        private System.Windows.Forms.Label LengthErrorLabel;
        private System.Windows.Forms.Label TotalU235Label;
        private System.Windows.Forms.Label TotalU235ErrorLabel;
        private System.Windows.Forms.Label TotalU238Label;
        private System.Windows.Forms.Label TotalU238ErrorLabel;
        private System.Windows.Forms.Label TotalRodsLabel;
        private System.Windows.Forms.Label TotalPoisonRodsLabel;
        private System.Windows.Forms.Label PoisonLabel;
        private System.Windows.Forms.Label PoisonErrorLabel;
    }
}