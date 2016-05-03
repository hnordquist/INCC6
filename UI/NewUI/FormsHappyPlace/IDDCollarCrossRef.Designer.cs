namespace NewUI
{
    partial class IDDCollarCrossRef
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
            this.ModeComboBox = new System.Windows.Forms.ComboBox();
            this.PoisonRodTypesComboBox = new System.Windows.Forms.ComboBox();
            this.PoisonAbsorptionFactorTextBox = new NewUI.NumericTextBox();
            this.RelativeDoublesRateTextBox = new NewUI.NumericTextBox();
            this.PrintBtn = new System.Windows.Forms.Button();
            this.NextBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.ModeLabel = new System.Windows.Forms.Label();
            this.ReferenceDateLabel = new System.Windows.Forms.Label();
            this.PoisonAbsorptionFactorLabel = new System.Windows.Forms.Label();
            this.RelativeDoublesRateLabel = new System.Windows.Forms.Label();
            this.ReferenceDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Items.AddRange(new object[] {
            "Thermal (no Cd)",
            "Fast (Cd)"});
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(103, 16);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(272, 21);
            this.MaterialTypeComboBox.TabIndex = 0;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // ModeComboBox
            // 
            this.ModeComboBox.FormattingEnabled = true;
            this.ModeComboBox.Items.AddRange(new object[] {
            "Thermal (no Cd)",
            "Fast (Cd)"});
            this.ModeComboBox.Location = new System.Drawing.Point(103, 56);
            this.ModeComboBox.Name = "ModeComboBox";
            this.ModeComboBox.Size = new System.Drawing.Size(200, 21);
            this.ModeComboBox.TabIndex = 1;
            this.ModeComboBox.SelectedIndexChanged += new System.EventHandler(this.ModeComboBox_SelectedIndexChanged);
            // 
            // PoisonRodTypesComboBox
            // 
            this.PoisonRodTypesComboBox.FormattingEnabled = true;
            this.PoisonRodTypesComboBox.Location = new System.Drawing.Point(466, 61);
            this.PoisonRodTypesComboBox.Name = "PoisonRodTypesComboBox";
            this.PoisonRodTypesComboBox.Size = new System.Drawing.Size(76, 21);
            this.PoisonRodTypesComboBox.TabIndex = 2;
            this.PoisonRodTypesComboBox.SelectedIndexChanged += new System.EventHandler(this.PoisonRodTypesComboBox_SelectedIndexChanged);
            // 
            // PoisonAbsorptionFactorTextBox
            // 
            this.PoisonAbsorptionFactorTextBox.Location = new System.Drawing.Point(466, 90);
            this.PoisonAbsorptionFactorTextBox.Max = 1.7976931348623157E+308D;
            this.PoisonAbsorptionFactorTextBox.Min = 0D;
            this.PoisonAbsorptionFactorTextBox.Name = "PoisonAbsorptionFactorTextBox";
            this.PoisonAbsorptionFactorTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.PoisonAbsorptionFactorTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.PoisonAbsorptionFactorTextBox.Size = new System.Drawing.Size(76, 20);
            this.PoisonAbsorptionFactorTextBox.Steps = -1D;
            this.PoisonAbsorptionFactorTextBox.TabIndex = 4;
            this.PoisonAbsorptionFactorTextBox.Text = "0.000000E+000";
            this.PoisonAbsorptionFactorTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.PoisonAbsorptionFactorTextBox.Value = 0D;
            this.PoisonAbsorptionFactorTextBox.TextChanged += new System.EventHandler(this.PoisonAbsorptionFactorTextBox_TextChanged);
            // 
            // RelativeDoublesRateTextBox
            // 
            this.RelativeDoublesRateTextBox.Location = new System.Drawing.Point(466, 120);
            this.RelativeDoublesRateTextBox.Max = 1.7976931348623157E+308D;
            this.RelativeDoublesRateTextBox.Min = 0D;
            this.RelativeDoublesRateTextBox.Name = "RelativeDoublesRateTextBox";
            this.RelativeDoublesRateTextBox.NumberFormat = NewUI.NumericTextBox.Formatter.E6;
            this.RelativeDoublesRateTextBox.NumStyles = ((System.Globalization.NumberStyles)(((((((System.Globalization.NumberStyles.AllowLeadingWhite | System.Globalization.NumberStyles.AllowTrailingWhite) 
            | System.Globalization.NumberStyles.AllowLeadingSign) 
            | System.Globalization.NumberStyles.AllowTrailingSign) 
            | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands) 
            | System.Globalization.NumberStyles.AllowExponent)));
            this.RelativeDoublesRateTextBox.Size = new System.Drawing.Size(76, 20);
            this.RelativeDoublesRateTextBox.Steps = -1D;
            this.RelativeDoublesRateTextBox.TabIndex = 5;
            this.RelativeDoublesRateTextBox.Text = "0.000000E+000";
            this.RelativeDoublesRateTextBox.ToValidate = NewUI.NumericTextBox.ValidateType.Double;
            this.RelativeDoublesRateTextBox.Value = 0D;
            this.RelativeDoublesRateTextBox.TextChanged += new System.EventHandler(this.RelativeDoublesRateTextBox_TextChanged);
            // 
            // PrintBtn
            // 
            this.PrintBtn.Location = new System.Drawing.Point(484, 14);
            this.PrintBtn.Name = "PrintBtn";
            this.PrintBtn.Size = new System.Drawing.Size(171, 23);
            this.PrintBtn.TabIndex = 6;
            this.PrintBtn.Text = "Print cross reference factors";
            this.PrintBtn.UseVisualStyleBackColor = true;
            this.PrintBtn.Click += new System.EventHandler(this.PrintBtn_Click);
            // 
            // NextBtn
            // 
            this.NextBtn.Location = new System.Drawing.Point(594, 59);
            this.NextBtn.Name = "NextBtn";
            this.NextBtn.Size = new System.Drawing.Size(75, 23);
            this.NextBtn.TabIndex = 7;
            this.NextBtn.Text = "Next";
            this.NextBtn.UseVisualStyleBackColor = true;
            this.NextBtn.Click += new System.EventHandler(this.NextBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(594, 88);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 8;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(594, 117);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 9;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(30, 19);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 10;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // ModeLabel
            // 
            this.ModeLabel.AutoSize = true;
            this.ModeLabel.Location = new System.Drawing.Point(63, 59);
            this.ModeLabel.Name = "ModeLabel";
            this.ModeLabel.Size = new System.Drawing.Size(34, 13);
            this.ModeLabel.TabIndex = 11;
            this.ModeLabel.Text = "Mode";
            // 
            // ReferenceDateLabel
            // 
            this.ReferenceDateLabel.AutoSize = true;
            this.ReferenceDateLabel.Location = new System.Drawing.Point(16, 88);
            this.ReferenceDateLabel.Name = "ReferenceDateLabel";
            this.ReferenceDateLabel.Size = new System.Drawing.Size(81, 13);
            this.ReferenceDateLabel.TabIndex = 12;
            this.ReferenceDateLabel.Text = "Reference date";
            // 
            // PoisonAbsorptionFactorLabel
            // 
            this.PoisonAbsorptionFactorLabel.AutoSize = true;
            this.PoisonAbsorptionFactorLabel.Location = new System.Drawing.Point(339, 93);
            this.PoisonAbsorptionFactorLabel.Name = "PoisonAbsorptionFactorLabel";
            this.PoisonAbsorptionFactorLabel.Size = new System.Drawing.Size(121, 13);
            this.PoisonAbsorptionFactorLabel.TabIndex = 13;
            this.PoisonAbsorptionFactorLabel.Text = "Poison absorption factor";
            // 
            // RelativeDoublesRateLabel
            // 
            this.RelativeDoublesRateLabel.AutoSize = true;
            this.RelativeDoublesRateLabel.Location = new System.Drawing.Point(331, 122);
            this.RelativeDoublesRateLabel.Name = "RelativeDoublesRateLabel";
            this.RelativeDoublesRateLabel.Size = new System.Drawing.Size(129, 13);
            this.RelativeDoublesRateLabel.TabIndex = 14;
            this.RelativeDoublesRateLabel.Text = "Relative doubles rate (K2)";
            // 
            // ReferenceDateTimePicker
            // 
            this.ReferenceDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.ReferenceDateTimePicker.Location = new System.Drawing.Point(103, 83);
            this.ReferenceDateTimePicker.Name = "ReferenceDateTimePicker";
            this.ReferenceDateTimePicker.Size = new System.Drawing.Size(105, 20);
            this.ReferenceDateTimePicker.TabIndex = 15;
            this.ReferenceDateTimePicker.ValueChanged += new System.EventHandler(this.ReferenceDateTimePicker_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(370, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Poison Rod Type";
            // 
            // IDDCollarCrossRef
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 158);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReferenceDateTimePicker);
            this.Controls.Add(this.RelativeDoublesRateLabel);
            this.Controls.Add(this.PoisonAbsorptionFactorLabel);
            this.Controls.Add(this.ReferenceDateLabel);
            this.Controls.Add(this.ModeLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.NextBtn);
            this.Controls.Add(this.PrintBtn);
            this.Controls.Add(this.RelativeDoublesRateTextBox);
            this.Controls.Add(this.PoisonAbsorptionFactorTextBox);
            this.Controls.Add(this.PoisonRodTypesComboBox);
            this.Controls.Add(this.ModeComboBox);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Name = "IDDCollarCrossRef";
            this.Text = "Cross Reference Factors";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HelpProvider provider = new System.Windows.Forms.HelpProvider();
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox ModeComboBox;
        private System.Windows.Forms.ComboBox PoisonRodTypesComboBox;
        private NumericTextBox PoisonAbsorptionFactorTextBox;
        private NumericTextBox RelativeDoublesRateTextBox;
        private System.Windows.Forms.Button PrintBtn;
        private System.Windows.Forms.Button NextBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label ModeLabel;
        private System.Windows.Forms.Label ReferenceDateLabel;
        private System.Windows.Forms.Label PoisonAbsorptionFactorLabel;
        private System.Windows.Forms.Label RelativeDoublesRateLabel;
        private System.Windows.Forms.DateTimePicker ReferenceDateTimePicker;
        private System.Windows.Forms.Label label1;
    }
}