namespace NewUI
{
    partial class IDDAcquireRatesOnly
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
            this.ItemIdLabel = new System.Windows.Forms.Label();
            this.ItemIdTextBox = new System.Windows.Forms.TextBox();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.CountTimeLabel = new System.Windows.Forms.Label();
            this.CountTimeTextBox = new System.Windows.Forms.TextBox();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.UseTriplesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseDoublesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseNumCyclesRadioButton = new System.Windows.Forms.RadioButton();
            this.NumCyclesLabel = new System.Windows.Forms.Label();
            this.NumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.MeasurementPrecisionLabel = new System.Windows.Forms.Label();
            this.MeasPrecisionTextBox = new System.Windows.Forms.TextBox();
            this.MinNumCyclesLabel = new System.Windows.Forms.Label();
            this.MinNumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.MaxNumCyclesLabel = new System.Windows.Forms.Label();
            this.MaxNumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.QCTestsCheckbox = new System.Windows.Forms.CheckBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.CommentAtEndCheckBox = new System.Windows.Forms.CheckBox();
            this.DataSourceLabel = new System.Windows.Forms.Label();
            this.DataSourceComboBox = new System.Windows.Forms.ComboBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.provider = new System.Windows.Forms.HelpProvider();
            this.GroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ItemIdLabel
            // 
            this.ItemIdLabel.AutoSize = true;
            this.ItemIdLabel.Location = new System.Drawing.Point(74, 21);
            this.ItemIdLabel.Name = "ItemIdLabel";
            this.ItemIdLabel.Size = new System.Drawing.Size(38, 13);
            this.ItemIdLabel.TabIndex = 0;
            this.ItemIdLabel.Text = "Item id";
            // 
            // ItemIdTextBox
            // 
            this.ItemIdTextBox.Location = new System.Drawing.Point(118, 18);
            this.ItemIdTextBox.Name = "ItemIdTextBox";
            this.ItemIdTextBox.Size = new System.Drawing.Size(243, 20);
            this.ItemIdTextBox.TabIndex = 1;
            this.ItemIdTextBox.Leave += new System.EventHandler(this.ItemIdTextBox_Leave);
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(61, 57);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 2;
            this.CommentLabel.Text = "Comment";
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(118, 54);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(243, 20);
            this.CommentTextBox.TabIndex = 3;
            this.CommentTextBox.Leave += new System.EventHandler(this.CommentTextBox_Leave);
            // 
            // CountTimeLabel
            // 
            this.CountTimeLabel.AutoSize = true;
            this.CountTimeLabel.Location = new System.Drawing.Point(24, 95);
            this.CountTimeLabel.Name = "CountTimeLabel";
            this.CountTimeLabel.Size = new System.Drawing.Size(88, 13);
            this.CountTimeLabel.TabIndex = 4;
            this.CountTimeLabel.Text = "Count time (secs)";
            // 
            // CountTimeTextBox
            // 
            this.CountTimeTextBox.Location = new System.Drawing.Point(118, 92);
            this.CountTimeTextBox.Name = "CountTimeTextBox";
            this.CountTimeTextBox.Size = new System.Drawing.Size(100, 20);
            this.CountTimeTextBox.TabIndex = 5;
            this.CountTimeTextBox.Leave += new System.EventHandler(this.CountTimeTextBox_Leave);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.UseTriplesRadioButton);
            this.GroupBox1.Controls.Add(this.UseDoublesRadioButton);
            this.GroupBox1.Controls.Add(this.UseNumCyclesRadioButton);
            this.GroupBox1.Location = new System.Drawing.Point(118, 127);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(243, 101);
            this.GroupBox1.TabIndex = 7;
            this.GroupBox1.TabStop = false;
            // 
            // UseTriplesRadioButton
            // 
            this.UseTriplesRadioButton.AutoSize = true;
            this.UseTriplesRadioButton.Location = new System.Drawing.Point(20, 65);
            this.UseTriplesRadioButton.Name = "UseTriplesRadioButton";
            this.UseTriplesRadioButton.Size = new System.Drawing.Size(185, 17);
            this.UseTriplesRadioButton.TabIndex = 2;
            this.UseTriplesRadioButton.TabStop = true;
            this.UseTriplesRadioButton.Text = "Use triples measurement precision";
            this.UseTriplesRadioButton.UseVisualStyleBackColor = true;
            this.UseTriplesRadioButton.CheckedChanged += new System.EventHandler(this.TriplesMeasurementPrecisionRadioButton_CheckedChanged);
            // 
            // UseDoublesRadioButton
            // 
            this.UseDoublesRadioButton.AutoSize = true;
            this.UseDoublesRadioButton.Location = new System.Drawing.Point(20, 42);
            this.UseDoublesRadioButton.Name = "UseDoublesRadioButton";
            this.UseDoublesRadioButton.Size = new System.Drawing.Size(195, 17);
            this.UseDoublesRadioButton.TabIndex = 1;
            this.UseDoublesRadioButton.TabStop = true;
            this.UseDoublesRadioButton.Text = "Use doubles measurement precision";
            this.UseDoublesRadioButton.UseVisualStyleBackColor = true;
            this.UseDoublesRadioButton.CheckedChanged += new System.EventHandler(this.DoublesMeasurementPrecisionRadioButton_CheckedChanged);
            // 
            // UseNumCyclesRadioButton
            // 
            this.UseNumCyclesRadioButton.AutoSize = true;
            this.UseNumCyclesRadioButton.Location = new System.Drawing.Point(20, 19);
            this.UseNumCyclesRadioButton.Name = "UseNumCyclesRadioButton";
            this.UseNumCyclesRadioButton.Size = new System.Drawing.Size(127, 17);
            this.UseNumCyclesRadioButton.TabIndex = 0;
            this.UseNumCyclesRadioButton.TabStop = true;
            this.UseNumCyclesRadioButton.Text = "Use number of cycles";
            this.UseNumCyclesRadioButton.UseVisualStyleBackColor = true;
            this.UseNumCyclesRadioButton.CheckedChanged += new System.EventHandler(this.NumberOfCyclesRadioButton_CheckedChanged);
            // 
            // NumCyclesLabel
            // 
            this.NumCyclesLabel.AutoSize = true;
            this.NumCyclesLabel.Location = new System.Drawing.Point(26, 251);
            this.NumCyclesLabel.Name = "NumCyclesLabel";
            this.NumCyclesLabel.Size = new System.Drawing.Size(89, 13);
            this.NumCyclesLabel.TabIndex = 8;
            this.NumCyclesLabel.Text = "Number of cycles";
            // 
            // NumCyclesTextBox
            // 
            this.NumCyclesTextBox.Location = new System.Drawing.Point(118, 248);
            this.NumCyclesTextBox.Name = "NumCyclesTextBox";
            this.NumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumCyclesTextBox.TabIndex = 9;
            this.NumCyclesTextBox.Leave += new System.EventHandler(this.NumCyclesTextBox_Leave);
            // 
            // MeasurementPrecisionLabel
            // 
            this.MeasurementPrecisionLabel.AutoSize = true;
            this.MeasurementPrecisionLabel.Location = new System.Drawing.Point(38, 284);
            this.MeasurementPrecisionLabel.MaximumSize = new System.Drawing.Size(100, 0);
            this.MeasurementPrecisionLabel.Name = "MeasurementPrecisionLabel";
            this.MeasurementPrecisionLabel.Size = new System.Drawing.Size(74, 26);
            this.MeasurementPrecisionLabel.TabIndex = 10;
            this.MeasurementPrecisionLabel.Text = "Measurement precision (%)";
            // 
            // MeasPrecisionTextBox
            // 
            this.MeasPrecisionTextBox.Location = new System.Drawing.Point(118, 284);
            this.MeasPrecisionTextBox.Name = "MeasPrecisionTextBox";
            this.MeasPrecisionTextBox.Size = new System.Drawing.Size(100, 20);
            this.MeasPrecisionTextBox.TabIndex = 11;
            this.MeasPrecisionTextBox.Leave += new System.EventHandler(this.MeasurementPrecisionTextBox_Leave);
            // 
            // MinNumCyclesLabel
            // 
            this.MinNumCyclesLabel.AutoSize = true;
            this.MinNumCyclesLabel.Location = new System.Drawing.Point(17, 323);
            this.MinNumCyclesLabel.Name = "MinNumCyclesLabel";
            this.MinNumCyclesLabel.Size = new System.Drawing.Size(95, 13);
            this.MinNumCyclesLabel.TabIndex = 12;
            this.MinNumCyclesLabel.Text = "Min number cycles";
            // 
            // MinNumCyclesTextBox
            // 
            this.MinNumCyclesTextBox.Location = new System.Drawing.Point(118, 320);
            this.MinNumCyclesTextBox.Name = "MinNumCyclesTextBox";
            this.MinNumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinNumCyclesTextBox.TabIndex = 13;
            this.MinNumCyclesTextBox.Leave += new System.EventHandler(this.MinNumCyclesTextBox_Leave);
            // 
            // MaxNumCyclesLabel
            // 
            this.MaxNumCyclesLabel.AutoSize = true;
            this.MaxNumCyclesLabel.Location = new System.Drawing.Point(14, 359);
            this.MaxNumCyclesLabel.Name = "MaxNumCyclesLabel";
            this.MaxNumCyclesLabel.Size = new System.Drawing.Size(98, 13);
            this.MaxNumCyclesLabel.TabIndex = 14;
            this.MaxNumCyclesLabel.Text = "Max number cycles";
            // 
            // MaxNumCyclesTextBox
            // 
            this.MaxNumCyclesTextBox.Location = new System.Drawing.Point(118, 356);
            this.MaxNumCyclesTextBox.Name = "MaxNumCyclesTextBox";
            this.MaxNumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxNumCyclesTextBox.TabIndex = 15;
            this.MaxNumCyclesTextBox.Leave += new System.EventHandler(this.MaxNumCyclesTextBox_Leave);
            // 
            // QCTestsCheckbox
            // 
            this.QCTestsCheckbox.AutoSize = true;
            this.QCTestsCheckbox.Location = new System.Drawing.Point(118, 392);
            this.QCTestsCheckbox.Name = "QCTestsCheckbox";
            this.QCTestsCheckbox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckbox.TabIndex = 16;
            this.QCTestsCheckbox.Text = "QC tests";
            this.QCTestsCheckbox.UseVisualStyleBackColor = true;
            this.QCTestsCheckbox.CheckedChanged += new System.EventHandler(this.QCTestsCheckbox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(118, 425);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 17;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckbox_CheckedChanged);
            // 
            // CommentAtEndCheckBox
            // 
            this.CommentAtEndCheckBox.AutoSize = true;
            this.CommentAtEndCheckBox.Location = new System.Drawing.Point(118, 458);
            this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
            this.CommentAtEndCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndCheckBox.TabIndex = 18;
            this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentCheckbox_CheckedChanged);
            // 
            // DataSourceLabel
            // 
            this.DataSourceLabel.AutoSize = true;
            this.DataSourceLabel.Location = new System.Drawing.Point(47, 494);
            this.DataSourceLabel.Name = "DataSourceLabel";
            this.DataSourceLabel.Size = new System.Drawing.Size(65, 13);
            this.DataSourceLabel.TabIndex = 19;
            this.DataSourceLabel.Text = "Data source";
            // 
            // DataSourceComboBox
            // 
            this.DataSourceComboBox.FormattingEnabled = true;
            this.DataSourceComboBox.Location = new System.Drawing.Point(118, 491);
            this.DataSourceComboBox.Name = "DataSourceComboBox";
            this.DataSourceComboBox.Size = new System.Drawing.Size(121, 21);
            this.DataSourceComboBox.TabIndex = 20;
            this.DataSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.DataSourceComboBox_SelectedIndexChanged);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(397, 16);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 21;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(397, 45);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 22;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(397, 74);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 23;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpButton_Click);
            // 
            // IDDAcquireRatesOnly
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 532);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DataSourceComboBox);
            this.Controls.Add(this.DataSourceLabel);
            this.Controls.Add(this.CommentAtEndCheckBox);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.QCTestsCheckbox);
            this.Controls.Add(this.MaxNumCyclesTextBox);
            this.Controls.Add(this.MaxNumCyclesLabel);
            this.Controls.Add(this.MinNumCyclesTextBox);
            this.Controls.Add(this.MinNumCyclesLabel);
            this.Controls.Add(this.MeasPrecisionTextBox);
            this.Controls.Add(this.MeasurementPrecisionLabel);
            this.Controls.Add(this.NumCyclesTextBox);
            this.Controls.Add(this.NumCyclesLabel);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.CountTimeTextBox);
            this.Controls.Add(this.CountTimeLabel);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.ItemIdTextBox);
            this.Controls.Add(this.ItemIdLabel);
            this.Name = "IDDAcquireRatesOnly";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Rates Only Measurement";
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ItemIdLabel;
        private System.Windows.Forms.TextBox ItemIdTextBox;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.Label CountTimeLabel;
        private System.Windows.Forms.TextBox CountTimeTextBox;
        private System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.RadioButton UseTriplesRadioButton;
        private System.Windows.Forms.RadioButton UseDoublesRadioButton;
        private System.Windows.Forms.RadioButton UseNumCyclesRadioButton;
        private System.Windows.Forms.Label NumCyclesLabel;
        private System.Windows.Forms.TextBox NumCyclesTextBox;
        private System.Windows.Forms.Label MeasurementPrecisionLabel;
        private System.Windows.Forms.TextBox MeasPrecisionTextBox;
        private System.Windows.Forms.Label MinNumCyclesLabel;
        private System.Windows.Forms.TextBox MinNumCyclesTextBox;
        private System.Windows.Forms.Label MaxNumCyclesLabel;
        private System.Windows.Forms.TextBox MaxNumCyclesTextBox;
        private System.Windows.Forms.CheckBox QCTestsCheckbox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndCheckBox;
        private System.Windows.Forms.Label DataSourceLabel;
        private System.Windows.Forms.ComboBox DataSourceComboBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.HelpProvider provider;
    }
}