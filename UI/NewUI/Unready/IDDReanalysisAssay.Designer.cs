namespace NewUI
{
    partial class IDDReanalysisAssay
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
            this.NormalizationConstantLabel = new System.Windows.Forms.Label();
            this.NormConstErrLabel = new System.Windows.Forms.Label();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.DeclaredMassTextBox = new System.Windows.Forms.TextBox();
            this.NormalizationConstantTextBox = new System.Windows.Forms.TextBox();
            this.NormConstErrTextBox = new System.Windows.Forms.TextBox();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.InventoryChangeCodeLabel = new System.Windows.Forms.Label();
            this.IOCodeLabel = new System.Windows.Forms.Label();
            this.ItemIdComboBox = new System.Windows.Forms.ComboBox();
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.StratumIdComboBox = new System.Windows.Forms.ComboBox();
            this.InventoryChangeCodeComboBox = new System.Windows.Forms.ComboBox();
            this.IOCodeComboBox = new System.Windows.Forms.ComboBox();
            this.CommentAtEndOfMeasurementCheckBox = new System.Windows.Forms.CheckBox();
            this.UseCurrentCalibrationParamatersCheckBox = new System.Windows.Forms.CheckBox();
            this.ReplaceOriginalVerificationMeasurementCheckBox = new System.Windows.Forms.CheckBox();
            this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.IsotopicsBtn = new System.Windows.Forms.Button();
            this.BackgroundBtn = new System.Windows.Forms.Button();
            this.Pu240eCoeffBtn = new System.Windows.Forms.Button();
            this.MeasurementDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // MeasurementDateLabel
            // 
            this.MeasurementDateLabel.AutoSize = true;
            this.MeasurementDateLabel.Location = new System.Drawing.Point(123, 20);
            this.MeasurementDateLabel.Name = "MeasurementDateLabel";
            this.MeasurementDateLabel.Size = new System.Drawing.Size(95, 13);
            this.MeasurementDateLabel.TabIndex = 0;
            this.MeasurementDateLabel.Text = "Measurement date";
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.AutoSize = true;
            this.ItemIdLabel.Location = new System.Drawing.Point(180, 43);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(38, 13);
            this.ItemIdLabel.TabIndex = 1;
            this.ItemIdLabel.Text = "Item id";
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(151, 70);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 2;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // StratumIdLabel
            // 
            this.StratumIdLabel.AutoSize = true;
            this.StratumIdLabel.Location = new System.Drawing.Point(164, 97);
            this.StratumIdLabel.Name = "StratumIdLabel";
            this.StratumIdLabel.Size = new System.Drawing.Size(54, 13);
            this.StratumIdLabel.TabIndex = 3;
            this.StratumIdLabel.Text = "Stratum id";
            // 
            // DeclaredMassLabel
            // 
            this.DeclaredMassLabel.AutoSize = true;
            this.DeclaredMassLabel.Location = new System.Drawing.Point(126, 124);
            this.DeclaredMassLabel.Name = "DeclaredMassLabel";
            this.DeclaredMassLabel.Size = new System.Drawing.Size(92, 13);
            this.DeclaredMassLabel.TabIndex = 4;
            this.DeclaredMassLabel.Text = "Declared mass (g)";
            // 
            // NormalizationConstantLabel
            // 
            this.NormalizationConstantLabel.AutoSize = true;
            this.NormalizationConstantLabel.Location = new System.Drawing.Point(104, 150);
            this.NormalizationConstantLabel.Name = "NormalizationConstantLabel";
            this.NormalizationConstantLabel.Size = new System.Drawing.Size(114, 13);
            this.NormalizationConstantLabel.TabIndex = 5;
            this.NormalizationConstantLabel.Text = "Normalization constant";
            // 
            // NormConstErrLabel
            // 
            this.NormConstErrLabel.AutoSize = true;
            this.NormConstErrLabel.Location = new System.Drawing.Point(80, 176);
            this.NormConstErrLabel.Name = "NormConstErrLabel";
            this.NormConstErrLabel.Size = new System.Drawing.Size(138, 13);
            this.NormConstErrLabel.TabIndex = 6;
            this.NormConstErrLabel.Text = "Normalization constant error";
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(167, 202);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 7;
            this.CommentLabel.Text = "Comment";
            // 
            // DeclaredMassTextBox
            // 
            this.DeclaredMassTextBox.Location = new System.Drawing.Point(224, 121);
            this.DeclaredMassTextBox.Name = "DeclaredMassTextBox";
            this.DeclaredMassTextBox.Size = new System.Drawing.Size(100, 20);
            this.DeclaredMassTextBox.TabIndex = 9;
            this.DeclaredMassTextBox.TextChanged += new System.EventHandler(this.DeclaredMassTextBox_TextChanged);
            // 
            // NormalizationConstantTextBox
            // 
            this.NormalizationConstantTextBox.Location = new System.Drawing.Point(224, 147);
            this.NormalizationConstantTextBox.Name = "NormalizationConstantTextBox";
            this.NormalizationConstantTextBox.Size = new System.Drawing.Size(100, 20);
            this.NormalizationConstantTextBox.TabIndex = 10;
            this.NormalizationConstantTextBox.TextChanged += new System.EventHandler(this.NormalizationConstantTextBox_TextChanged);
            // 
            // NormConstErrTextBox
            // 
            this.NormConstErrTextBox.Location = new System.Drawing.Point(224, 173);
            this.NormConstErrTextBox.Name = "NormConstErrTextBox";
            this.NormConstErrTextBox.Size = new System.Drawing.Size(100, 20);
            this.NormConstErrTextBox.TabIndex = 11;
            this.NormConstErrTextBox.TextChanged += new System.EventHandler(this.NormConstErrTextBox_TextChanged);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(224, 199);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(100, 20);
            this.CommentTextBox.TabIndex = 12;
            this.CommentTextBox.TextChanged += new System.EventHandler(this.CommentTextBox_TextChanged);
            // 
            // InventoryChangeCodeLabel
            // 
            this.InventoryChangeCodeLabel.AutoSize = true;
            this.InventoryChangeCodeLabel.Location = new System.Drawing.Point(470, 20);
            this.InventoryChangeCodeLabel.Name = "InventoryChangeCodeLabel";
            this.InventoryChangeCodeLabel.Size = new System.Drawing.Size(117, 13);
            this.InventoryChangeCodeLabel.TabIndex = 13;
            this.InventoryChangeCodeLabel.Text = "Inventory change code";
            // 
            // IOCodeLabel
            // 
            this.IOCodeLabel.AutoSize = true;
            this.IOCodeLabel.Location = new System.Drawing.Point(537, 47);
            this.IOCodeLabel.Name = "IOCodeLabel";
            this.IOCodeLabel.Size = new System.Drawing.Size(50, 13);
            this.IOCodeLabel.TabIndex = 14;
            this.IOCodeLabel.Text = "I/O code";
            // 
            // ItemIdComboBox
            // 
            this.ItemIdComboBox.FormattingEnabled = true;
            this.ItemIdComboBox.Location = new System.Drawing.Point(224, 40);
            this.ItemIdComboBox.Name = "ItemIdComboBox";
            this.ItemIdComboBox.Size = new System.Drawing.Size(200, 21);
            this.ItemIdComboBox.TabIndex = 15;
            this.ItemIdComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemIdComboBox_SelectedIndexChanged);
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(224, 67);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(200, 21);
            this.MaterialTypeComboBox.TabIndex = 16;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // StratumIdComboBox
            // 
            this.StratumIdComboBox.FormattingEnabled = true;
            this.StratumIdComboBox.Location = new System.Drawing.Point(224, 94);
            this.StratumIdComboBox.Name = "StratumIdComboBox";
            this.StratumIdComboBox.Size = new System.Drawing.Size(200, 21);
            this.StratumIdComboBox.TabIndex = 17;
            this.StratumIdComboBox.SelectedIndexChanged += new System.EventHandler(this.StratumIdComboBox_SelectedIndexChanged);
            // 
            // InventoryChangeCodeComboBox
            // 
            this.InventoryChangeCodeComboBox.FormattingEnabled = true;
            this.InventoryChangeCodeComboBox.Location = new System.Drawing.Point(593, 17);
            this.InventoryChangeCodeComboBox.Name = "InventoryChangeCodeComboBox";
            this.InventoryChangeCodeComboBox.Size = new System.Drawing.Size(61, 21);
            this.InventoryChangeCodeComboBox.TabIndex = 18;
            this.InventoryChangeCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.InventoryChangeCodeComboBox_SelectedIndexChanged);
            // 
            // IOCodeComboBox
            // 
            this.IOCodeComboBox.FormattingEnabled = true;
            this.IOCodeComboBox.Location = new System.Drawing.Point(593, 44);
            this.IOCodeComboBox.Name = "IOCodeComboBox";
            this.IOCodeComboBox.Size = new System.Drawing.Size(61, 21);
            this.IOCodeComboBox.TabIndex = 19;
            this.IOCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IOCodeComboBox_SelectedIndexChanged);
            // 
            // CommentAtEndOfMeasurementCheckBox
            // 
            this.CommentAtEndOfMeasurementCheckBox.AutoSize = true;
            this.CommentAtEndOfMeasurementCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CommentAtEndOfMeasurementCheckBox.Location = new System.Drawing.Point(57, 225);
            this.CommentAtEndOfMeasurementCheckBox.Name = "CommentAtEndOfMeasurementCheckBox";
            this.CommentAtEndOfMeasurementCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndOfMeasurementCheckBox.TabIndex = 20;
            this.CommentAtEndOfMeasurementCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndOfMeasurementCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndOfMeasurementCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndOfMeasurementCheckBox_CheckedChanged);
            // 
            // UseCurrentCalibrationParamatersCheckBox
            // 
            this.UseCurrentCalibrationParamatersCheckBox.AutoSize = true;
            this.UseCurrentCalibrationParamatersCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.UseCurrentCalibrationParamatersCheckBox.Location = new System.Drawing.Point(51, 248);
            this.UseCurrentCalibrationParamatersCheckBox.Name = "UseCurrentCalibrationParamatersCheckBox";
            this.UseCurrentCalibrationParamatersCheckBox.Size = new System.Drawing.Size(187, 17);
            this.UseCurrentCalibrationParamatersCheckBox.TabIndex = 21;
            this.UseCurrentCalibrationParamatersCheckBox.Text = "Use current calibration parameters";
            this.UseCurrentCalibrationParamatersCheckBox.UseVisualStyleBackColor = true;
            this.UseCurrentCalibrationParamatersCheckBox.CheckedChanged += new System.EventHandler(this.UseCurrentCalibrationParamatersCheckBox_CheckedChanged);
            // 
            // ReplaceOriginalVerificationMeasurementCheckBox
            // 
            this.ReplaceOriginalVerificationMeasurementCheckBox.AutoSize = true;
            this.ReplaceOriginalVerificationMeasurementCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ReplaceOriginalVerificationMeasurementCheckBox.Location = new System.Drawing.Point(16, 271);
            this.ReplaceOriginalVerificationMeasurementCheckBox.Name = "ReplaceOriginalVerificationMeasurementCheckBox";
            this.ReplaceOriginalVerificationMeasurementCheckBox.Size = new System.Drawing.Size(222, 17);
            this.ReplaceOriginalVerificationMeasurementCheckBox.TabIndex = 22;
            this.ReplaceOriginalVerificationMeasurementCheckBox.Text = "Replace original verification measurement";
            this.ReplaceOriginalVerificationMeasurementCheckBox.UseVisualStyleBackColor = true;
            this.ReplaceOriginalVerificationMeasurementCheckBox.CheckedChanged += new System.EventHandler(this.ReplaceOriginalVerificationMeasurementCheckBox_CheckedChanged);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(172, 294);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 23;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(158, 317);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 24;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(682, 15);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 25;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(682, 44);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 26;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(682, 73);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 27;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // IsotopicsBtn
            // 
            this.IsotopicsBtn.Location = new System.Drawing.Point(473, 141);
            this.IsotopicsBtn.Name = "IsotopicsBtn";
            this.IsotopicsBtn.Size = new System.Drawing.Size(75, 23);
            this.IsotopicsBtn.TabIndex = 28;
            this.IsotopicsBtn.Text = "Isotopics...";
            this.IsotopicsBtn.UseVisualStyleBackColor = true;
            this.IsotopicsBtn.Click += new System.EventHandler(this.IsotopicsBtn_Click);
            // 
            // BackgroundBtn
            // 
            this.BackgroundBtn.Location = new System.Drawing.Point(473, 170);
            this.BackgroundBtn.Name = "BackgroundBtn";
            this.BackgroundBtn.Size = new System.Drawing.Size(75, 23);
            this.BackgroundBtn.TabIndex = 29;
            this.BackgroundBtn.Text = "Background...";
            this.BackgroundBtn.UseVisualStyleBackColor = true;
            this.BackgroundBtn.Click += new System.EventHandler(this.BackgroundBtn_Click);
            // 
            // Pu240eCoeffBtn
            // 
            this.Pu240eCoeffBtn.Location = new System.Drawing.Point(473, 199);
            this.Pu240eCoeffBtn.Name = "Pu240eCoeffBtn";
            this.Pu240eCoeffBtn.Size = new System.Drawing.Size(75, 23);
            this.Pu240eCoeffBtn.TabIndex = 30;
            this.Pu240eCoeffBtn.Text = "Pu240e Coeff...";
            this.Pu240eCoeffBtn.UseVisualStyleBackColor = true;
            this.Pu240eCoeffBtn.Click += new System.EventHandler(this.Pu240eCoeffBtn_Click);
            // 
            // MeasurementDateTimePicker
            // 
            this.MeasurementDateTimePicker.Location = new System.Drawing.Point(224, 14);
            this.MeasurementDateTimePicker.Name = "MeasurementDateTimePicker";
            this.MeasurementDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.MeasurementDateTimePicker.TabIndex = 31;
            this.MeasurementDateTimePicker.ValueChanged += new System.EventHandler(this.MeasurementDateTimePicker_ValueChanged);
            // 
            // IDDReanalysisAssay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 344);
            this.Controls.Add(this.MeasurementDateTimePicker);
            this.Controls.Add(this.Pu240eCoeffBtn);
            this.Controls.Add(this.BackgroundBtn);
            this.Controls.Add(this.IsotopicsBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.QCTestsCheckBox);
            this.Controls.Add(this.ReplaceOriginalVerificationMeasurementCheckBox);
            this.Controls.Add(this.UseCurrentCalibrationParamatersCheckBox);
            this.Controls.Add(this.CommentAtEndOfMeasurementCheckBox);
            this.Controls.Add(this.IOCodeComboBox);
            this.Controls.Add(this.InventoryChangeCodeComboBox);
            this.Controls.Add(this.StratumIdComboBox);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Controls.Add(this.ItemIdComboBox);
            this.Controls.Add(this.IOCodeLabel);
            this.Controls.Add(this.InventoryChangeCodeLabel);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.NormConstErrTextBox);
            this.Controls.Add(this.NormalizationConstantTextBox);
            this.Controls.Add(this.DeclaredMassTextBox);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.NormConstErrLabel);
            this.Controls.Add(this.NormalizationConstantLabel);
            this.Controls.Add(this.DeclaredMassLabel);
            this.Controls.Add(this.StratumIdLabel);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.ItemIdLabel);
            this.Controls.Add(this.MeasurementDateLabel);
            this.Name = "IDDReanalysisAssay";
            this.Text = "Verification Measurement Reanalysis";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MeasurementDateLabel;
        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.Label StratumIdLabel;
        private System.Windows.Forms.Label DeclaredMassLabel;
        private System.Windows.Forms.Label NormalizationConstantLabel;
        private System.Windows.Forms.Label NormConstErrLabel;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.TextBox DeclaredMassTextBox;
        private System.Windows.Forms.TextBox NormalizationConstantTextBox;
        private System.Windows.Forms.TextBox NormConstErrTextBox;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.Label InventoryChangeCodeLabel;
        private System.Windows.Forms.Label IOCodeLabel;
        private System.Windows.Forms.ComboBox ItemIdComboBox;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.ComboBox StratumIdComboBox;
        private System.Windows.Forms.ComboBox InventoryChangeCodeComboBox;
        private System.Windows.Forms.ComboBox IOCodeComboBox;
        private System.Windows.Forms.CheckBox CommentAtEndOfMeasurementCheckBox;
        private System.Windows.Forms.CheckBox UseCurrentCalibrationParamatersCheckBox;
        private System.Windows.Forms.CheckBox ReplaceOriginalVerificationMeasurementCheckBox;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button IsotopicsBtn;
        private System.Windows.Forms.Button BackgroundBtn;
        private System.Windows.Forms.Button Pu240eCoeffBtn;
        private System.Windows.Forms.DateTimePicker MeasurementDateTimePicker;
    }
}