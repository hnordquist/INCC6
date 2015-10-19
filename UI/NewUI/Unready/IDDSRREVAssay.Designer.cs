namespace NewUI
{
    partial class IDDSRREVAssay
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
            this.MeasurementDateTimePickerLabel = new System.Windows.Forms.Label();
            this.MeasurementDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.MBALabel = new System.Windows.Forms.Label();
            this.MBAComboBox = new System.Windows.Forms.ComboBox();
            this.ItemIdLabel = new System.Windows.Forms.Label();
            this.ItemIdComboBox = new System.Windows.Forms.ComboBox();
            this.StratumIdLabel = new System.Windows.Forms.Label();
            this.StratumIdComboBox = new System.Windows.Forms.ComboBox();
            this.MaterialTypeLabel = new System.Windows.Forms.Label();
            this.MaterialTypeComboBox = new System.Windows.Forms.ComboBox();
            this.DeclaredMassLabel = new System.Windows.Forms.Label();
            this.DeclaredMassTextBox = new System.Windows.Forms.TextBox();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.CommentAtEndCheckBox = new System.Windows.Forms.CheckBox();
            this.InventoryChangeCodeLabel = new System.Windows.Forms.Label();
            this.InventoryChangeCodeComboBox = new System.Windows.Forms.ComboBox();
            this.IOCodeLabel = new System.Windows.Forms.Label();
            this.IOCodeComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.BackgroundBtn = new System.Windows.Forms.Button();
            this.CompositeIsotpicsBtn = new System.Windows.Forms.Button();
            this.IsotopicsBtn = new System.Windows.Forms.Button();
            this.CancelAllImportingBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MeasurementDateTimePickerLabel
            // 
            this.MeasurementDateTimePickerLabel.AutoSize = true;
            this.MeasurementDateTimePickerLabel.Location = new System.Drawing.Point(38, 14);
            this.MeasurementDateTimePickerLabel.Name = "MeasurementDateTimePickerLabel";
            this.MeasurementDateTimePickerLabel.Size = new System.Drawing.Size(138, 13);
            this.MeasurementDateTimePickerLabel.TabIndex = 0;
            this.MeasurementDateTimePickerLabel.Text = "Measurement date and time";
            // 
            // MeasurementDateTimePicker
            // 
            this.MeasurementDateTimePicker.Location = new System.Drawing.Point(180, 12);
            this.MeasurementDateTimePicker.Name = "MeasurementDateTimePicker";
            this.MeasurementDateTimePicker.Size = new System.Drawing.Size(232, 20);
            this.MeasurementDateTimePicker.TabIndex = 1;
            this.MeasurementDateTimePicker.ValueChanged += new System.EventHandler(this.MeasurementDateTimePicker_ValueChanged);
            // 
            // MBALabel
            // 
            this.MBALabel.AutoSize = true;
            this.MBALabel.Location = new System.Drawing.Point(144, 41);
            this.MBALabel.Name = "MBALabel";
            this.MBALabel.Size = new System.Drawing.Size(30, 13);
            this.MBALabel.TabIndex = 2;
            this.MBALabel.Text = "MBA";
            // 
            // MBAComboBox
            // 
            this.MBAComboBox.FormattingEnabled = true;
            this.MBAComboBox.Location = new System.Drawing.Point(180, 38);
            this.MBAComboBox.Name = "MBAComboBox";
            this.MBAComboBox.Size = new System.Drawing.Size(121, 21);
            this.MBAComboBox.TabIndex = 3;
            this.MBAComboBox.SelectedIndexChanged += new System.EventHandler(this.MBAComboBox_SelectedIndexChanged);
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.AutoSize = true;
            this.ItemIdLabel.Location = new System.Drawing.Point(136, 68);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(38, 13);
            this.ItemIdLabel.TabIndex = 4;
            this.ItemIdLabel.Text = "Item id";
            // 
            // ItemIdComboBox
            // 
            this.ItemIdComboBox.FormattingEnabled = true;
            this.ItemIdComboBox.Location = new System.Drawing.Point(180, 65);
            this.ItemIdComboBox.Name = "ItemIdComboBox";
            this.ItemIdComboBox.Size = new System.Drawing.Size(121, 21);
            this.ItemIdComboBox.TabIndex = 5;
            this.ItemIdComboBox.SelectedIndexChanged += new System.EventHandler(this.ItemIdComboBox_SelectedIndexChanged);
            // 
            // StratumIdLabel
            // 
            this.StratumIdLabel.AutoSize = true;
            this.StratumIdLabel.Location = new System.Drawing.Point(120, 95);
            this.StratumIdLabel.Name = "StratumIdLabel";
            this.StratumIdLabel.Size = new System.Drawing.Size(54, 13);
            this.StratumIdLabel.TabIndex = 6;
            this.StratumIdLabel.Text = "Stratum id";
            // 
            // StratumIdComboBox
            // 
            this.StratumIdComboBox.FormattingEnabled = true;
            this.StratumIdComboBox.Location = new System.Drawing.Point(180, 92);
            this.StratumIdComboBox.Name = "StratumIdComboBox";
            this.StratumIdComboBox.Size = new System.Drawing.Size(121, 21);
            this.StratumIdComboBox.TabIndex = 7;
            this.StratumIdComboBox.SelectedIndexChanged += new System.EventHandler(this.StratumIdComboBox_SelectedIndexChanged);
            // 
            // MaterialTypeLabel
            // 
            this.MaterialTypeLabel.AutoSize = true;
            this.MaterialTypeLabel.Location = new System.Drawing.Point(107, 122);
            this.MaterialTypeLabel.Name = "MaterialTypeLabel";
            this.MaterialTypeLabel.Size = new System.Drawing.Size(67, 13);
            this.MaterialTypeLabel.TabIndex = 8;
            this.MaterialTypeLabel.Text = "Material type";
            // 
            // MaterialTypeComboBox
            // 
            this.MaterialTypeComboBox.FormattingEnabled = true;
            this.MaterialTypeComboBox.Location = new System.Drawing.Point(180, 119);
            this.MaterialTypeComboBox.Name = "MaterialTypeComboBox";
            this.MaterialTypeComboBox.Size = new System.Drawing.Size(139, 21);
            this.MaterialTypeComboBox.TabIndex = 9;
            this.MaterialTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.MaterialTypeComboBox_SelectedIndexChanged);
            // 
            // DeclaredMassLabel
            // 
            this.DeclaredMassLabel.AutoSize = true;
            this.DeclaredMassLabel.Location = new System.Drawing.Point(82, 149);
            this.DeclaredMassLabel.Name = "DeclaredMassLabel";
            this.DeclaredMassLabel.Size = new System.Drawing.Size(92, 13);
            this.DeclaredMassLabel.TabIndex = 10;
            this.DeclaredMassLabel.Text = "Declared mass (g)";
            // 
            // DeclaredMassTextBox
            // 
            this.DeclaredMassTextBox.Location = new System.Drawing.Point(180, 146);
            this.DeclaredMassTextBox.Name = "DeclaredMassTextBox";
            this.DeclaredMassTextBox.Size = new System.Drawing.Size(100, 20);
            this.DeclaredMassTextBox.TabIndex = 11;
            this.DeclaredMassTextBox.TextChanged += new System.EventHandler(this.DeclaredMassTextBox_TextChanged);
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(123, 175);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 12;
            this.CommentLabel.Text = "Comment";
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(180, 172);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(100, 20);
            this.CommentTextBox.TabIndex = 13;
            this.CommentTextBox.TextChanged += new System.EventHandler(this.CommentTextBox_TextChanged);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(128, 202);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 14;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(114, 225);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 15;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // CommentAtEndCheckBox
            // 
            this.CommentAtEndCheckBox.AutoSize = true;
            this.CommentAtEndCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CommentAtEndCheckBox.Location = new System.Drawing.Point(13, 248);
            this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
            this.CommentAtEndCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndCheckBox.TabIndex = 16;
            this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndCheckBox_CheckedChanged);
            // 
            // InventoryChangeCodeLabel
            // 
            this.InventoryChangeCodeLabel.AutoSize = true;
            this.InventoryChangeCodeLabel.Location = new System.Drawing.Point(356, 68);
            this.InventoryChangeCodeLabel.Name = "InventoryChangeCodeLabel";
            this.InventoryChangeCodeLabel.Size = new System.Drawing.Size(117, 13);
            this.InventoryChangeCodeLabel.TabIndex = 17;
            this.InventoryChangeCodeLabel.Text = "Inventory change code";
            // 
            // InventoryChangeCodeComboBox
            // 
            this.InventoryChangeCodeComboBox.FormattingEnabled = true;
            this.InventoryChangeCodeComboBox.Location = new System.Drawing.Point(479, 65);
            this.InventoryChangeCodeComboBox.Name = "InventoryChangeCodeComboBox";
            this.InventoryChangeCodeComboBox.Size = new System.Drawing.Size(62, 21);
            this.InventoryChangeCodeComboBox.TabIndex = 18;
            this.InventoryChangeCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.InventoryChangeCodeComboBox_SelectedIndexChanged);
            // 
            // IOCodeLabel
            // 
            this.IOCodeLabel.AutoSize = true;
            this.IOCodeLabel.Location = new System.Drawing.Point(423, 95);
            this.IOCodeLabel.Name = "IOCodeLabel";
            this.IOCodeLabel.Size = new System.Drawing.Size(50, 13);
            this.IOCodeLabel.TabIndex = 19;
            this.IOCodeLabel.Text = "I/O code";
            // 
            // IOCodeComboBox
            // 
            this.IOCodeComboBox.FormattingEnabled = true;
            this.IOCodeComboBox.Location = new System.Drawing.Point(479, 92);
            this.IOCodeComboBox.Name = "IOCodeComboBox";
            this.IOCodeComboBox.Size = new System.Drawing.Size(62, 21);
            this.IOCodeComboBox.TabIndex = 20;
            this.IOCodeComboBox.SelectedIndexChanged += new System.EventHandler(this.IOCodeComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(559, 9);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 21;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(559, 38);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 22;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(559, 67);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 23;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // BackgroundBtn
            // 
            this.BackgroundBtn.Location = new System.Drawing.Point(512, 244);
            this.BackgroundBtn.Name = "BackgroundBtn";
            this.BackgroundBtn.Size = new System.Drawing.Size(122, 23);
            this.BackgroundBtn.TabIndex = 24;
            this.BackgroundBtn.Text = "Background...";
            this.BackgroundBtn.UseVisualStyleBackColor = true;
            this.BackgroundBtn.Click += new System.EventHandler(this.BackgroundBtn_Click);
            // 
            // CompositeIsotpicsBtn
            // 
            this.CompositeIsotpicsBtn.Location = new System.Drawing.Point(512, 215);
            this.CompositeIsotpicsBtn.Name = "CompositeIsotpicsBtn";
            this.CompositeIsotpicsBtn.Size = new System.Drawing.Size(123, 23);
            this.CompositeIsotpicsBtn.TabIndex = 25;
            this.CompositeIsotpicsBtn.Text = "Composite Isotopics...";
            this.CompositeIsotpicsBtn.UseVisualStyleBackColor = true;
            this.CompositeIsotpicsBtn.Click += new System.EventHandler(this.CompositeIsotpicsBtn_Click);
            // 
            // IsotopicsBtn
            // 
            this.IsotopicsBtn.Location = new System.Drawing.Point(512, 186);
            this.IsotopicsBtn.Name = "IsotopicsBtn";
            this.IsotopicsBtn.Size = new System.Drawing.Size(123, 23);
            this.IsotopicsBtn.TabIndex = 26;
            this.IsotopicsBtn.Text = "Isotopics...";
            this.IsotopicsBtn.UseVisualStyleBackColor = true;
            this.IsotopicsBtn.Click += new System.EventHandler(this.IsotopicsBtn_Click);
            // 
            // CancelAllImportingBtn
            // 
            this.CancelAllImportingBtn.Location = new System.Drawing.Point(511, 157);
            this.CancelAllImportingBtn.Name = "CancelAllImportingBtn";
            this.CancelAllImportingBtn.Size = new System.Drawing.Size(123, 23);
            this.CancelAllImportingBtn.TabIndex = 27;
            this.CancelAllImportingBtn.Text = "Cancel all importing";
            this.CancelAllImportingBtn.UseVisualStyleBackColor = true;
            this.CancelAllImportingBtn.Click += new System.EventHandler(this.CancelAllImportingBtn_Click);
            // 
            // IDDSRREVAssay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 283);
            this.Controls.Add(this.CancelAllImportingBtn);
            this.Controls.Add(this.IsotopicsBtn);
            this.Controls.Add(this.CompositeIsotpicsBtn);
            this.Controls.Add(this.BackgroundBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.IOCodeComboBox);
            this.Controls.Add(this.IOCodeLabel);
            this.Controls.Add(this.InventoryChangeCodeComboBox);
            this.Controls.Add(this.InventoryChangeCodeLabel);
            this.Controls.Add(this.CommentAtEndCheckBox);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.QCTestsCheckBox);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.DeclaredMassTextBox);
            this.Controls.Add(this.DeclaredMassLabel);
            this.Controls.Add(this.MaterialTypeComboBox);
            this.Controls.Add(this.MaterialTypeLabel);
            this.Controls.Add(this.StratumIdComboBox);
            this.Controls.Add(this.StratumIdLabel);
            this.Controls.Add(this.ItemIdComboBox);
            this.Controls.Add(this.ItemIdLabel);
            this.Controls.Add(this.MBAComboBox);
            this.Controls.Add(this.MBALabel);
            this.Controls.Add(this.MeasurementDateTimePicker);
            this.Controls.Add(this.MeasurementDateTimePickerLabel);
            this.Name = "IDDSRREVAssay";
            this.Text = "Verification Measurement from Radiation Review";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MeasurementDateTimePickerLabel;
        private System.Windows.Forms.DateTimePicker MeasurementDateTimePicker;
        private System.Windows.Forms.Label MBALabel;
        private System.Windows.Forms.ComboBox MBAComboBox;
        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.ComboBox ItemIdComboBox;
        private System.Windows.Forms.Label StratumIdLabel;
        private System.Windows.Forms.ComboBox StratumIdComboBox;
        private System.Windows.Forms.Label MaterialTypeLabel;
        private System.Windows.Forms.ComboBox MaterialTypeComboBox;
        private System.Windows.Forms.Label DeclaredMassLabel;
        private System.Windows.Forms.TextBox DeclaredMassTextBox;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndCheckBox;
        private System.Windows.Forms.Label InventoryChangeCodeLabel;
        private System.Windows.Forms.ComboBox InventoryChangeCodeComboBox;
        private System.Windows.Forms.Label IOCodeLabel;
        private System.Windows.Forms.ComboBox IOCodeComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button BackgroundBtn;
        private System.Windows.Forms.Button CompositeIsotpicsBtn;
        private System.Windows.Forms.Button IsotopicsBtn;
        private System.Windows.Forms.Button CancelAllImportingBtn;
    }
}