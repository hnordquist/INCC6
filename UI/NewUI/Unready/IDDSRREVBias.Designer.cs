namespace NewUI
{
    partial class IDDSRREVBias
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
            this.MeasurementDateTimeLabel = new System.Windows.Forms.Label();
            this.MeasurementDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.CommentAtEndOfMeasurementCheckBox = new System.Windows.Forms.CheckBox();
            this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
            this.SourceIdLabel = new System.Windows.Forms.Label();
            this.SourceIdLabelToBeChanged = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CancelAllImportingBtn = new System.Windows.Forms.Button();
            this.BackgroundBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // MeasurementDateTimeLabel
            // 
            this.MeasurementDateTimeLabel.AutoSize = true;
            this.MeasurementDateTimeLabel.Location = new System.Drawing.Point(36, 20);
            this.MeasurementDateTimeLabel.Name = "MeasurementDateTimeLabel";
            this.MeasurementDateTimeLabel.Size = new System.Drawing.Size(138, 13);
            this.MeasurementDateTimeLabel.TabIndex = 0;
            this.MeasurementDateTimeLabel.Text = "Measurement date and time";
            // 
            // MeasurementDateTimePicker
            // 
            this.MeasurementDateTimePicker.Location = new System.Drawing.Point(180, 14);
            this.MeasurementDateTimePicker.Name = "MeasurementDateTimePicker";
            this.MeasurementDateTimePicker.Size = new System.Drawing.Size(259, 20);
            this.MeasurementDateTimePicker.TabIndex = 1;
            this.MeasurementDateTimePicker.ValueChanged += new System.EventHandler(this.MeasurementDateTimePicker_ValueChanged);
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(123, 43);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 2;
            this.CommentLabel.Text = "Comment";
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(180, 40);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(259, 20);
            this.CommentTextBox.TabIndex = 3;
            this.CommentTextBox.TextChanged += new System.EventHandler(this.CommentTextBox_TextChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(112, 108);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 4;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // CommentAtEndOfMeasurementCheckBox
            // 
            this.CommentAtEndOfMeasurementCheckBox.AutoSize = true;
            this.CommentAtEndOfMeasurementCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CommentAtEndOfMeasurementCheckBox.Location = new System.Drawing.Point(11, 141);
            this.CommentAtEndOfMeasurementCheckBox.Name = "CommentAtEndOfMeasurementCheckBox";
            this.CommentAtEndOfMeasurementCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndOfMeasurementCheckBox.TabIndex = 5;
            this.CommentAtEndOfMeasurementCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndOfMeasurementCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndOfMeasurementCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndOfMeasurementCheckBox_CheckedChanged);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(126, 75);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 6;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // SourceIdLabel
            // 
            this.SourceIdLabel.AutoSize = true;
            this.SourceIdLabel.Location = new System.Drawing.Point(122, 176);
            this.SourceIdLabel.Name = "SourceIdLabel";
            this.SourceIdLabel.Size = new System.Drawing.Size(52, 13);
            this.SourceIdLabel.TabIndex = 7;
            this.SourceIdLabel.Text = "Source id";
            // 
            // SourceIdLabelToBeChanged
            // 
            this.SourceIdLabelToBeChanged.AutoSize = true;
            this.SourceIdLabelToBeChanged.Location = new System.Drawing.Point(180, 176);
            this.SourceIdLabelToBeChanged.Name = "SourceIdLabelToBeChanged";
            this.SourceIdLabelToBeChanged.Size = new System.Drawing.Size(27, 13);
            this.SourceIdLabelToBeChanged.TabIndex = 8;
            this.SourceIdLabelToBeChanged.Text = "xxxx";
            this.SourceIdLabelToBeChanged.Click += new System.EventHandler(this.SourceIdLabelToBeChanged_Click);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(463, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 9;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(463, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 10;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(463, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 11;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // CancelAllImportingBtn
            // 
            this.CancelAllImportingBtn.Location = new System.Drawing.Point(425, 116);
            this.CancelAllImportingBtn.Name = "CancelAllImportingBtn";
            this.CancelAllImportingBtn.Size = new System.Drawing.Size(113, 23);
            this.CancelAllImportingBtn.TabIndex = 12;
            this.CancelAllImportingBtn.Text = "Cancel all importing";
            this.CancelAllImportingBtn.UseVisualStyleBackColor = true;
            this.CancelAllImportingBtn.Click += new System.EventHandler(this.CancelAllImportingBtn_Click);
            // 
            // BackgroundBtn
            // 
            this.BackgroundBtn.Location = new System.Drawing.Point(425, 166);
            this.BackgroundBtn.Name = "BackgroundBtn";
            this.BackgroundBtn.Size = new System.Drawing.Size(113, 23);
            this.BackgroundBtn.TabIndex = 13;
            this.BackgroundBtn.Text = "Background...";
            this.BackgroundBtn.UseVisualStyleBackColor = true;
            this.BackgroundBtn.Click += new System.EventHandler(this.BackgroundBtn_Click);
            // 
            // IDDSRREVBias
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(552, 208);
            this.Controls.Add(this.BackgroundBtn);
            this.Controls.Add(this.CancelAllImportingBtn);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.SourceIdLabelToBeChanged);
            this.Controls.Add(this.SourceIdLabel);
            this.Controls.Add(this.QCTestsCheckBox);
            this.Controls.Add(this.CommentAtEndOfMeasurementCheckBox);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.MeasurementDateTimePicker);
            this.Controls.Add(this.MeasurementDateTimeLabel);
            this.Name = "IDDSRREVBias";
            this.Text = "IDDSRREVBias";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label MeasurementDateTimeLabel;
        private System.Windows.Forms.DateTimePicker MeasurementDateTimePicker;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndOfMeasurementCheckBox;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.Label SourceIdLabel;
        private System.Windows.Forms.Label SourceIdLabelToBeChanged;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Button CancelAllImportingBtn;
        private System.Windows.Forms.Button BackgroundBtn;
    }
}