namespace UI
{
    partial class IDDReanalysisHoldup
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
            this.MeasurementDateLabel = new System.Windows.Forms.Label();
            this.ItemIdLabel = new System.Windows.Forms.Label();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.StratumIdLabel = new System.Windows.Forms.Label();
            this.DeclaredMassLabel = new System.Windows.Forms.Label();
            this.NormConstLabel = new System.Windows.Forms.Label();
            this.NormConstErrorLabel = new System.Windows.Forms.Label();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.ItemIdComboBox = new System.Windows.Forms.ComboBox();
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.StratumIdComboBox = new System.Windows.Forms.ComboBox();
            this.DeclaredMassTextBox = new System.Windows.Forms.TextBox();
            this.NormConstTextBox = new System.Windows.Forms.TextBox();
            this.NormConstErrTextBox = new System.Windows.Forms.TextBox();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.CommentAtEndOfMeasurementCheckBox = new System.Windows.Forms.CheckBox();
            this.UseCurrentCalibrationParametersCheckBox = new System.Windows.Forms.CheckBox();
            this.ReplaceOriginalHoldupMeasurementCheckBox = new System.Windows.Forms.CheckBox();
            this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.BackgroundBtn = new System.Windows.Forms.Button();
            this.IsotopicsBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.MeasurementDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // MeasurementDateLabel
            // 
            this.MeasurementDateLabel.AutoSize = true;
            this.MeasurementDateLabel.Location = new System.Drawing.Point(97, 21);
            this.MeasurementDateLabel.Name = "MeasurementDateLabel";
            this.MeasurementDateLabel.Size = new System.Drawing.Size(95, 13);
            this.MeasurementDateLabel.TabIndex = 0;
            this.MeasurementDateLabel.Text = "Measurement date";
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.AutoSize = true;
            this.ItemIdLabel.Location = new System.Drawing.Point(154, 44);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(38, 13);
            this.ItemIdLabel.TabIndex = 1;
            this.ItemIdLabel.Text = "Item id";
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(125, 71);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 2;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // StratumIdLabel
            // 
            this.StratumIdLabel.AutoSize = true;
            this.StratumIdLabel.Location = new System.Drawing.Point(138, 98);
            this.StratumIdLabel.Name = "StratumIdLabel";
            this.StratumIdLabel.Size = new System.Drawing.Size(54, 13);
            this.StratumIdLabel.TabIndex = 3;
            this.StratumIdLabel.Text = "Stratum id";
            // 
            // DeclaredMassLabel
            // 
            this.DeclaredMassLabel.AutoSize = true;
            this.DeclaredMassLabel.Location = new System.Drawing.Point(100, 125);
            this.DeclaredMassLabel.Name = "DeclaredMassLabel";
            this.DeclaredMassLabel.Size = new System.Drawing.Size(92, 13);
            this.DeclaredMassLabel.TabIndex = 4;
            this.DeclaredMassLabel.Text = "Declared mass (g)";
            // 
            // NormConstLabel
            // 
            this.NormConstLabel.AutoSize = true;
            this.NormConstLabel.Location = new System.Drawing.Point(78, 151);
            this.NormConstLabel.Name = "NormConstLabel";
            this.NormConstLabel.Size = new System.Drawing.Size(114, 13);
            this.NormConstLabel.TabIndex = 5;
            this.NormConstLabel.Text = "Normalization constant";
            // 
            // NormConstErrorLabel
            // 
            this.NormConstErrorLabel.AutoSize = true;
            this.NormConstErrorLabel.Location = new System.Drawing.Point(54, 177);
            this.NormConstErrorLabel.Name = "NormConstErrorLabel";
            this.NormConstErrorLabel.Size = new System.Drawing.Size(138, 13);
            this.NormConstErrorLabel.TabIndex = 6;
            this.NormConstErrorLabel.Text = "Normalization constant error";
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(141, 203);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 7;
            this.CommentLabel.Text = "Comment";
            // 
            // ItemIdComboBox
            // 
            this.ItemIdComboBox.FormattingEnabled = true;
            this.ItemIdComboBox.Location = new System.Drawing.Point(198, 41);
            this.ItemIdComboBox.Name = "ItemIdComboBox";
            this.ItemIdComboBox.Size = new System.Drawing.Size(200, 21);
            this.ItemIdComboBox.TabIndex = 8;
            this.ItemIdComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemIdComboBox_SelectedIndexChanged);
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(198, 68);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(200, 21);
            this.MaterialTypeComboBox.TabIndex = 9;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // StratumIdComboBox
            // 
            this.StratumIdComboBox.FormattingEnabled = true;
            this.StratumIdComboBox.Location = new System.Drawing.Point(198, 95);
            this.StratumIdComboBox.Name = "StratumIdComboBox";
            this.StratumIdComboBox.Size = new System.Drawing.Size(200, 21);
            this.StratumIdComboBox.TabIndex = 10;
            this.StratumIdComboBox.SelectedIndexChanged += new System.EventHandler(this.StratumIdComboBox_SelectedIndexChanged);
            // 
            // DeclaredMassTextBox
            // 
            this.DeclaredMassTextBox.Location = new System.Drawing.Point(198, 122);
            this.DeclaredMassTextBox.Name = "DeclaredMassTextBox";
            this.DeclaredMassTextBox.Size = new System.Drawing.Size(100, 20);
            this.DeclaredMassTextBox.TabIndex = 12;
            this.DeclaredMassTextBox.TextChanged += new System.EventHandler(this.DeclaredMassTextBox_TextChanged);
            // 
            // NormConstTextBox
            // 
            this.NormConstTextBox.Location = new System.Drawing.Point(198, 148);
            this.NormConstTextBox.Name = "NormConstTextBox";
            this.NormConstTextBox.Size = new System.Drawing.Size(100, 20);
            this.NormConstTextBox.TabIndex = 13;
            this.NormConstTextBox.TextChanged += new System.EventHandler(this.NormConstTextBox_TextChanged);
            // 
            // NormConstErrTextBox
            // 
            this.NormConstErrTextBox.Location = new System.Drawing.Point(198, 174);
            this.NormConstErrTextBox.Name = "NormConstErrTextBox";
            this.NormConstErrTextBox.Size = new System.Drawing.Size(100, 20);
            this.NormConstErrTextBox.TabIndex = 14;
            this.NormConstErrTextBox.TextChanged += new System.EventHandler(this.NormConstErrTextBox_TextChanged);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(198, 200);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(291, 20);
            this.CommentTextBox.TabIndex = 15;
            this.CommentTextBox.TextChanged += new System.EventHandler(this.CommentTextBox_TextChanged);
            // 
            // CommentAtEndOfMeasurementCheckBox
            // 
            this.CommentAtEndOfMeasurementCheckBox.AutoSize = true;
            this.CommentAtEndOfMeasurementCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CommentAtEndOfMeasurementCheckBox.Location = new System.Drawing.Point(31, 226);
            this.CommentAtEndOfMeasurementCheckBox.Name = "CommentAtEndOfMeasurementCheckBox";
            this.CommentAtEndOfMeasurementCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndOfMeasurementCheckBox.TabIndex = 16;
            this.CommentAtEndOfMeasurementCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndOfMeasurementCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndOfMeasurementCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndOfMeasurementCheckBox_CheckedChanged);
            // 
            // UseCurrentCalibrationParametersCheckBox
            // 
            this.UseCurrentCalibrationParametersCheckBox.AutoSize = true;
            this.UseCurrentCalibrationParametersCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.UseCurrentCalibrationParametersCheckBox.Location = new System.Drawing.Point(25, 249);
            this.UseCurrentCalibrationParametersCheckBox.Name = "UseCurrentCalibrationParametersCheckBox";
            this.UseCurrentCalibrationParametersCheckBox.Size = new System.Drawing.Size(187, 17);
            this.UseCurrentCalibrationParametersCheckBox.TabIndex = 17;
            this.UseCurrentCalibrationParametersCheckBox.Text = "Use current calibration parameters";
            this.UseCurrentCalibrationParametersCheckBox.UseVisualStyleBackColor = true;
            this.UseCurrentCalibrationParametersCheckBox.CheckedChanged += new System.EventHandler(this.UseCurrentCalibrationParametersCheckBox_CheckedChanged);
            // 
            // ReplaceOriginalHoldupMeasurementCheckBox
            // 
            this.ReplaceOriginalHoldupMeasurementCheckBox.AutoSize = true;
            this.ReplaceOriginalHoldupMeasurementCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ReplaceOriginalHoldupMeasurementCheckBox.Location = new System.Drawing.Point(9, 272);
            this.ReplaceOriginalHoldupMeasurementCheckBox.Name = "ReplaceOriginalHoldupMeasurementCheckBox";
            this.ReplaceOriginalHoldupMeasurementCheckBox.Size = new System.Drawing.Size(203, 17);
            this.ReplaceOriginalHoldupMeasurementCheckBox.TabIndex = 18;
            this.ReplaceOriginalHoldupMeasurementCheckBox.Text = "Replace original holdup measurement";
            this.ReplaceOriginalHoldupMeasurementCheckBox.UseVisualStyleBackColor = true;
            this.ReplaceOriginalHoldupMeasurementCheckBox.CheckedChanged += new System.EventHandler(this.ReplaceOriginalHoldupMeasurementCheckBox_CheckedChanged);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(146, 295);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 19;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(132, 318);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 20;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // BackgroundBtn
            // 
            this.BackgroundBtn.Location = new System.Drawing.Point(414, 171);
            this.BackgroundBtn.Name = "BackgroundBtn";
            this.BackgroundBtn.Size = new System.Drawing.Size(75, 23);
            this.BackgroundBtn.TabIndex = 21;
            this.BackgroundBtn.Text = "Background...";
            this.BackgroundBtn.UseVisualStyleBackColor = true;
            this.BackgroundBtn.Click += new System.EventHandler(this.BackgroundBtn_Click);
            // 
            // IsotopicsBtn
            // 
            this.IsotopicsBtn.Location = new System.Drawing.Point(414, 142);
            this.IsotopicsBtn.Name = "IsotopicsBtn";
            this.IsotopicsBtn.Size = new System.Drawing.Size(75, 23);
            this.IsotopicsBtn.TabIndex = 22;
            this.IsotopicsBtn.Text = "Isotopics...";
            this.IsotopicsBtn.UseVisualStyleBackColor = true;
            this.IsotopicsBtn.Click += new System.EventHandler(this.IsotopicsBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(414, 73);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 23;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(414, 15);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 25;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(414, 44);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 26;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // MeasurementDateTimePicker
            // 
            this.MeasurementDateTimePicker.Location = new System.Drawing.Point(198, 15);
            this.MeasurementDateTimePicker.Name = "MeasurementDateTimePicker";
            this.MeasurementDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.MeasurementDateTimePicker.TabIndex = 27;
            this.MeasurementDateTimePicker.ValueChanged += new System.EventHandler(this.MeasurementDateTimePicker_ValueChanged);
            // 
            // IDDReanalysisHoldup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 350);
            this.Controls.Add(this.MeasurementDateTimePicker);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.IsotopicsBtn);
            this.Controls.Add(this.BackgroundBtn);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.QCTestsCheckBox);
            this.Controls.Add(this.ReplaceOriginalHoldupMeasurementCheckBox);
            this.Controls.Add(this.UseCurrentCalibrationParametersCheckBox);
            this.Controls.Add(this.CommentAtEndOfMeasurementCheckBox);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.NormConstErrTextBox);
            this.Controls.Add(this.NormConstTextBox);
            this.Controls.Add(this.DeclaredMassTextBox);
            this.Controls.Add(this.StratumIdComboBox);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Controls.Add(this.ItemIdComboBox);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.NormConstErrorLabel);
            this.Controls.Add(this.NormConstLabel);
            this.Controls.Add(this.DeclaredMassLabel);
            this.Controls.Add(this.StratumIdLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.ItemIdLabel);
            this.Controls.Add(this.MeasurementDateLabel);
            this.Name = "IDDReanalysisHoldup";
            this.Text = "Holdup Measurement Reanalysis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MeasurementDateLabel;
        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label StratumIdLabel;
        private System.Windows.Forms.Label DeclaredMassLabel;
        private System.Windows.Forms.Label NormConstLabel;
        private System.Windows.Forms.Label NormConstErrorLabel;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.ComboBox ItemIdComboBox;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox StratumIdComboBox;
        private System.Windows.Forms.TextBox DeclaredMassTextBox;
        private System.Windows.Forms.TextBox NormConstTextBox;
        private System.Windows.Forms.TextBox NormConstErrTextBox;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.CheckBox CommentAtEndOfMeasurementCheckBox;
        private System.Windows.Forms.CheckBox UseCurrentCalibrationParametersCheckBox;
        private System.Windows.Forms.CheckBox ReplaceOriginalHoldupMeasurementCheckBox;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.Button BackgroundBtn;
        private System.Windows.Forms.Button IsotopicsBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.DateTimePicker MeasurementDateTimePicker;
    }
}