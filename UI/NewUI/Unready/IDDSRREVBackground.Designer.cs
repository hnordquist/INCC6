namespace NewUI
{
    partial class IDDSRREVBackground
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
            this.CommentLabel = new System.Windows.Forms.Label();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
            this.CommentAtEndCheckBox = new System.Windows.Forms.CheckBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.CancelAllImportingBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.OKBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MeasurementDateTimePickerLabel
            // 
            this.MeasurementDateTimePickerLabel.AutoSize = true;
            this.MeasurementDateTimePickerLabel.Location = new System.Drawing.Point(43, 24);
            this.MeasurementDateTimePickerLabel.Name = "MeasurementDateTimePickerLabel";
            this.MeasurementDateTimePickerLabel.Size = new System.Drawing.Size(138, 13);
            this.MeasurementDateTimePickerLabel.TabIndex = 0;
            this.MeasurementDateTimePickerLabel.Text = "Measurement date and time";
            // 
            // MeasurementDateTimePicker
            // 
            this.MeasurementDateTimePicker.Location = new System.Drawing.Point(187, 18);
            this.MeasurementDateTimePicker.Name = "MeasurementDateTimePicker";
            this.MeasurementDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.MeasurementDateTimePicker.TabIndex = 1;
            this.MeasurementDateTimePicker.ValueChanged += new System.EventHandler(this.MeasurementDateTimePicker_ValueChanged);
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(130, 50);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 2;
            this.CommentLabel.Text = "Comment";
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(187, 47);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(331, 20);
            this.CommentTextBox.TabIndex = 3;
            this.CommentTextBox.TextChanged += new System.EventHandler(this.CommentTextBox_TextChanged);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(133, 78);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 4;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // CommentAtEndCheckBox
            // 
            this.CommentAtEndCheckBox.AutoSize = true;
            this.CommentAtEndCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CommentAtEndCheckBox.Location = new System.Drawing.Point(18, 124);
            this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
            this.CommentAtEndCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndCheckBox.TabIndex = 5;
            this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndCheckBox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(119, 101);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 6;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(560, 43);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 9;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // CancelAllImportingBtn
            // 
            this.CancelAllImportingBtn.Location = new System.Drawing.Point(468, 116);
            this.CancelAllImportingBtn.Name = "CancelAllImportingBtn";
            this.CancelAllImportingBtn.Size = new System.Drawing.Size(167, 23);
            this.CancelAllImportingBtn.TabIndex = 10;
            this.CancelAllImportingBtn.Text = "Cancel all importing";
            this.CancelAllImportingBtn.UseVisualStyleBackColor = true;
            this.CancelAllImportingBtn.Click += new System.EventHandler(this.CancelAllImportingBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(560, 72);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 11;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(560, 14);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 12;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // IDDSRREVBackground
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(655, 157);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelAllImportingBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.CommentAtEndCheckBox);
            this.Controls.Add(this.QCTestsCheckBox);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.MeasurementDateTimePicker);
            this.Controls.Add(this.MeasurementDateTimePickerLabel);
            this.Name = "IDDSRREVBackground";
            this.Text = "Background Measurement from Radiation Review";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MeasurementDateTimePickerLabel;
        private System.Windows.Forms.DateTimePicker MeasurementDateTimePicker;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button CancelAllImportingBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button OKBtn;
    }
}