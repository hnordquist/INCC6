namespace NewUI
{
    partial class IDDAcquirePrecision
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
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.CountTimeTextBox = new System.Windows.Forms.TextBox();
            this.NumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.MeasPrecisionTextBox = new System.Windows.Forms.TextBox();
            this.MinNumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.MaxNumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UseTriplesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseDoublesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseNumCyclesRadioButton = new System.Windows.Forms.RadioButton();
            this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.CommentAtEndCheckBox = new System.Windows.Forms.CheckBox();
            this.DataSourceComboBox = new System.Windows.Forms.ComboBox();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.CountTimeLabel = new System.Windows.Forms.Label();
            this.NumCyclesLabel = new System.Windows.Forms.Label();
            this.MeasPrecisionLabel = new System.Windows.Forms.Label();
            this.MinNumCyclesLabel = new System.Windows.Forms.Label();
            this.MaxNumCyclesLabel = new System.Windows.Forms.Label();
            this.DataSourceLabel = new System.Windows.Forms.Label();
            this.provider = new System.Windows.Forms.HelpProvider();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(152, 12);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(228, 20);
            this.CommentTextBox.TabIndex = 0;
            this.CommentTextBox.Leave += new System.EventHandler(this.CommentTextBox_Leave);
            // 
            // CountTimeTextBox
            // 
            this.CountTimeTextBox.Location = new System.Drawing.Point(152, 38);
            this.CountTimeTextBox.Name = "CountTimeTextBox";
            this.CountTimeTextBox.Size = new System.Drawing.Size(100, 20);
            this.CountTimeTextBox.TabIndex = 1;
            this.CountTimeTextBox.Leave += new System.EventHandler(this.CountTimeTextBox_Leave);
            // 
            // NumCyclesTextBox
            // 
            this.NumCyclesTextBox.Location = new System.Drawing.Point(152, 178);
            this.NumCyclesTextBox.Name = "NumCyclesTextBox";
            this.NumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumCyclesTextBox.TabIndex = 2;
            this.NumCyclesTextBox.Leave += new System.EventHandler(this.NumCyclesTextBox_Leave);
            // 
            // MeasPrecisionTextBox
            // 
            this.MeasPrecisionTextBox.Location = new System.Drawing.Point(152, 204);
            this.MeasPrecisionTextBox.Name = "MeasPrecisionTextBox";
            this.MeasPrecisionTextBox.Size = new System.Drawing.Size(100, 20);
            this.MeasPrecisionTextBox.TabIndex = 3;
            this.MeasPrecisionTextBox.Leave += new System.EventHandler(this.MeasPrecisionTextBox_Leave);
            // 
            // MinNumCyclesTextBox
            // 
            this.MinNumCyclesTextBox.Location = new System.Drawing.Point(152, 230);
            this.MinNumCyclesTextBox.Name = "MinNumCyclesTextBox";
            this.MinNumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinNumCyclesTextBox.TabIndex = 4;
            this.MinNumCyclesTextBox.Leave += new System.EventHandler(this.MinNumCyclesTextBox_Leave);
            // 
            // MaxNumCyclesTextBox
            // 
            this.MaxNumCyclesTextBox.Location = new System.Drawing.Point(152, 256);
            this.MaxNumCyclesTextBox.Name = "MaxNumCyclesTextBox";
            this.MaxNumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxNumCyclesTextBox.TabIndex = 5;
            this.MaxNumCyclesTextBox.Leave += new System.EventHandler(this.MaxNumCyclesTextBox_Leave);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.UseTriplesRadioButton);
            this.groupBox1.Controls.Add(this.UseDoublesRadioButton);
            this.groupBox1.Controls.Add(this.UseNumCyclesRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(152, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(228, 98);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // UseTriplesRadioButton
            // 
            this.UseTriplesRadioButton.AutoSize = true;
            this.UseTriplesRadioButton.Location = new System.Drawing.Point(14, 65);
            this.UseTriplesRadioButton.Name = "UseTriplesRadioButton";
            this.UseTriplesRadioButton.Size = new System.Drawing.Size(185, 17);
            this.UseTriplesRadioButton.TabIndex = 2;
            this.UseTriplesRadioButton.TabStop = true;
            this.UseTriplesRadioButton.Text = "Use triples measurement precision";
            this.UseTriplesRadioButton.UseVisualStyleBackColor = true;
            this.UseTriplesRadioButton.CheckedChanged += new System.EventHandler(this.UseTriplesRadioButton_CheckedChanged);
            // 
            // UseDoublesRadioButton
            // 
            this.UseDoublesRadioButton.AutoSize = true;
            this.UseDoublesRadioButton.Location = new System.Drawing.Point(14, 42);
            this.UseDoublesRadioButton.Name = "UseDoublesRadioButton";
            this.UseDoublesRadioButton.Size = new System.Drawing.Size(195, 17);
            this.UseDoublesRadioButton.TabIndex = 1;
            this.UseDoublesRadioButton.TabStop = true;
            this.UseDoublesRadioButton.Text = "Use doubles measurement precision";
            this.UseDoublesRadioButton.UseVisualStyleBackColor = true;
            this.UseDoublesRadioButton.CheckedChanged += new System.EventHandler(this.UseDblsRadioButton_CheckedChanged);
            // 
            // UseNumCyclesRadioButton
            // 
            this.UseNumCyclesRadioButton.AutoSize = true;
            this.UseNumCyclesRadioButton.Location = new System.Drawing.Point(14, 19);
            this.UseNumCyclesRadioButton.Name = "UseNumCyclesRadioButton";
            this.UseNumCyclesRadioButton.Size = new System.Drawing.Size(127, 17);
            this.UseNumCyclesRadioButton.TabIndex = 0;
            this.UseNumCyclesRadioButton.TabStop = true;
            this.UseNumCyclesRadioButton.Text = "Use number of cycles";
            this.UseNumCyclesRadioButton.UseVisualStyleBackColor = true;
            this.UseNumCyclesRadioButton.CheckedChanged += new System.EventHandler(this.UseNumCyclesRadioButton_CheckedChanged);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(152, 291);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 7;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(152, 314);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 8;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // CommentAtEndCheckBox
            // 
            this.CommentAtEndCheckBox.AutoSize = true;
            this.CommentAtEndCheckBox.Location = new System.Drawing.Point(152, 337);
            this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
            this.CommentAtEndCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndCheckBox.TabIndex = 9;
            this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndCheckBox_CheckedChanged);
            // 
            // DataSourceComboBox
            // 
            this.DataSourceComboBox.FormattingEnabled = true;
            this.DataSourceComboBox.Location = new System.Drawing.Point(152, 369);
            this.DataSourceComboBox.Name = "DataSourceComboBox";
            this.DataSourceComboBox.Size = new System.Drawing.Size(121, 21);
            this.DataSourceComboBox.TabIndex = 10;
            this.DataSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.DataSrcComboBox_SelectedIndexChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(418, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 11;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(418, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 12;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(418, 68);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 13;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(92, 15);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 14;
            this.CommentLabel.Text = "Comment";
            // 
            // CountTimeLabel
            // 
            this.CountTimeLabel.AutoSize = true;
            this.CountTimeLabel.Location = new System.Drawing.Point(55, 41);
            this.CountTimeLabel.Name = "CountTimeLabel";
            this.CountTimeLabel.Size = new System.Drawing.Size(88, 13);
            this.CountTimeLabel.TabIndex = 15;
            this.CountTimeLabel.Text = "Count time (secs)";
            // 
            // NumCyclesLabel
            // 
            this.NumCyclesLabel.AutoSize = true;
            this.NumCyclesLabel.Location = new System.Drawing.Point(54, 181);
            this.NumCyclesLabel.Name = "NumCyclesLabel";
            this.NumCyclesLabel.Size = new System.Drawing.Size(89, 13);
            this.NumCyclesLabel.TabIndex = 16;
            this.NumCyclesLabel.Text = "Number of cycles";
            // 
            // MeasPrecisionLabel
            // 
            this.MeasPrecisionLabel.AutoSize = true;
            this.MeasPrecisionLabel.Location = new System.Drawing.Point(7, 207);
            this.MeasPrecisionLabel.Name = "MeasPrecisionLabel";
            this.MeasPrecisionLabel.Size = new System.Drawing.Size(133, 13);
            this.MeasPrecisionLabel.TabIndex = 17;
            this.MeasPrecisionLabel.Text = "Measurement precision (%)";
            // 
            // MinNumCyclesLabel
            // 
            this.MinNumCyclesLabel.AutoSize = true;
            this.MinNumCyclesLabel.Location = new System.Drawing.Point(48, 233);
            this.MinNumCyclesLabel.Name = "MinNumCyclesLabel";
            this.MinNumCyclesLabel.Size = new System.Drawing.Size(95, 13);
            this.MinNumCyclesLabel.TabIndex = 18;
            this.MinNumCyclesLabel.Text = "Min number cycles";
            // 
            // MaxNumCyclesLabel
            // 
            this.MaxNumCyclesLabel.AutoSize = true;
            this.MaxNumCyclesLabel.Location = new System.Drawing.Point(45, 259);
            this.MaxNumCyclesLabel.Name = "MaxNumCyclesLabel";
            this.MaxNumCyclesLabel.Size = new System.Drawing.Size(98, 13);
            this.MaxNumCyclesLabel.TabIndex = 19;
            this.MaxNumCyclesLabel.Text = "Max number cycles";
            // 
            // DataSourceLabel
            // 
            this.DataSourceLabel.AutoSize = true;
            this.DataSourceLabel.Location = new System.Drawing.Point(78, 372);
            this.DataSourceLabel.Name = "DataSourceLabel";
            this.DataSourceLabel.Size = new System.Drawing.Size(65, 13);
            this.DataSourceLabel.TabIndex = 20;
            this.DataSourceLabel.Text = "Data source";
            // 
            // IDDAcquirePrecision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 403);
            this.Controls.Add(this.DataSourceLabel);
            this.Controls.Add(this.MaxNumCyclesLabel);
            this.Controls.Add(this.MinNumCyclesLabel);
            this.Controls.Add(this.MeasPrecisionLabel);
            this.Controls.Add(this.NumCyclesLabel);
            this.Controls.Add(this.CountTimeLabel);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.DataSourceComboBox);
            this.Controls.Add(this.CommentAtEndCheckBox);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.QCTestsCheckBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.MaxNumCyclesTextBox);
            this.Controls.Add(this.MinNumCyclesTextBox);
            this.Controls.Add(this.MeasPrecisionTextBox);
            this.Controls.Add(this.NumCyclesTextBox);
            this.Controls.Add(this.CountTimeTextBox);
            this.Controls.Add(this.CommentTextBox);
            this.Name = "IDDAcquirePrecision";
            this.Text = "Precision Measurement";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.TextBox CountTimeTextBox;
        private System.Windows.Forms.TextBox NumCyclesTextBox;
        private System.Windows.Forms.TextBox MeasPrecisionTextBox;
        private System.Windows.Forms.TextBox MinNumCyclesTextBox;
        private System.Windows.Forms.TextBox MaxNumCyclesTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton UseTriplesRadioButton;
        private System.Windows.Forms.RadioButton UseDoublesRadioButton;
        private System.Windows.Forms.RadioButton UseNumCyclesRadioButton;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndCheckBox;
        private System.Windows.Forms.ComboBox DataSourceComboBox;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.Label CountTimeLabel;
        private System.Windows.Forms.Label NumCyclesLabel;
        private System.Windows.Forms.Label MeasPrecisionLabel;
        private System.Windows.Forms.Label MinNumCyclesLabel;
        private System.Windows.Forms.Label MaxNumCyclesLabel;
        private System.Windows.Forms.Label DataSourceLabel;
        private System.Windows.Forms.HelpProvider provider;
    }
}