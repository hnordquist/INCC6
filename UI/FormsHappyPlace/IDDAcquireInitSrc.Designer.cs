namespace UI
{
    partial class IDDAcquireInitSrc
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.AmLiNormRadioButton = new System.Windows.Forms.RadioButton();
            this.Cf252SinglesNormRadioButton = new System.Windows.Forms.RadioButton();
            this.Cf252DblsNormRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.UseTriplesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseDoublesRadioButton = new System.Windows.Forms.RadioButton();
            this.UseNumCyclesRadioButton = new System.Windows.Forms.RadioButton();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.SourceIdTextBox = new System.Windows.Forms.TextBox();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.CountTimeTextBox = new System.Windows.Forms.TextBox();
            this.NumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.MeasPrecisionTextBox = new System.Windows.Forms.TextBox();
            this.MinNumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.MaxNumCyclesTextBox = new System.Windows.Forms.TextBox();
            this.DistanceToMoveTextBox = new System.Windows.Forms.TextBox();
            this.QCTestsCheckBox = new System.Windows.Forms.CheckBox();
            this.PrintResultsCheckBox = new System.Windows.Forms.CheckBox();
            this.CommentAtEndCheckBox = new System.Windows.Forms.CheckBox();
            this.UseAddASourceCheckBox = new System.Windows.Forms.CheckBox();
            this.DataSourceComboBox = new System.Windows.Forms.ComboBox();
            this.SourceIdLabel = new System.Windows.Forms.Label();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.CountTimeLabel = new System.Windows.Forms.Label();
            this.NumCyclesLabel = new System.Windows.Forms.Label();
            this.MeasPrecisionLabel = new System.Windows.Forms.Label();
            this.MinNumCyclesLabel = new System.Windows.Forms.Label();
            this.MaxNumCyclesLabel = new System.Windows.Forms.Label();
            this.DataSourceLabel = new System.Windows.Forms.Label();
            this.DistanceToMoveLabel = new System.Windows.Forms.Label();
            this.provider = new System.Windows.Forms.HelpProvider();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.AmLiNormRadioButton);
            this.groupBox1.Controls.Add(this.Cf252SinglesNormRadioButton);
            this.groupBox1.Controls.Add(this.Cf252DblsNormRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(157, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(313, 95);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // AmLiNormRadioButton
            // 
            this.AmLiNormRadioButton.AutoSize = true;
            this.AmLiNormRadioButton.Location = new System.Drawing.Point(19, 65);
            this.AmLiNormRadioButton.Name = "AmLiNormRadioButton";
            this.AmLiNormRadioButton.Size = new System.Drawing.Size(204, 17);
            this.AmLiNormRadioButton.TabIndex = 2;
            this.AmLiNormRadioButton.TabStop = true;
            this.AmLiNormRadioButton.Text = "Use AmLi source for normalization test";
            this.AmLiNormRadioButton.UseVisualStyleBackColor = true;
            this.AmLiNormRadioButton.CheckedChanged += new System.EventHandler(this.AmLiNormRadioButton_CheckedChanged);
            // 
            // Cf252SinglesNormRadioButton
            // 
            this.Cf252SinglesNormRadioButton.AutoSize = true;
            this.Cf252SinglesNormRadioButton.Location = new System.Drawing.Point(19, 42);
            this.Cf252SinglesNormRadioButton.Name = "Cf252SinglesNormRadioButton";
            this.Cf252SinglesNormRadioButton.Size = new System.Drawing.Size(265, 17);
            this.Cf252SinglesNormRadioButton.TabIndex = 1;
            this.Cf252SinglesNormRadioButton.TabStop = true;
            this.Cf252SinglesNormRadioButton.Text = "Use Cf252 source singles rate for normalization test";
            this.Cf252SinglesNormRadioButton.UseVisualStyleBackColor = true;
            this.Cf252SinglesNormRadioButton.CheckedChanged += new System.EventHandler(this.Cf252SinglesNormRadioButton_CheckedChanged);
            // 
            // Cf252DblsNormRadioButton
            // 
            this.Cf252DblsNormRadioButton.AutoSize = true;
            this.Cf252DblsNormRadioButton.Location = new System.Drawing.Point(19, 19);
            this.Cf252DblsNormRadioButton.Name = "Cf252DblsNormRadioButton";
            this.Cf252DblsNormRadioButton.Size = new System.Drawing.Size(270, 17);
            this.Cf252DblsNormRadioButton.TabIndex = 0;
            this.Cf252DblsNormRadioButton.TabStop = true;
            this.Cf252DblsNormRadioButton.Text = "Use Cf252 source doubles rate for normalization test";
            this.Cf252DblsNormRadioButton.UseVisualStyleBackColor = true;
            this.Cf252DblsNormRadioButton.CheckedChanged += new System.EventHandler(this.Cf252DblsNormRadioButton_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.UseTriplesRadioButton);
            this.groupBox2.Controls.Add(this.UseDoublesRadioButton);
            this.groupBox2.Controls.Add(this.UseNumCyclesRadioButton);
            this.groupBox2.Location = new System.Drawing.Point(157, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(299, 97);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // UseTriplesRadioButton
            // 
            this.UseTriplesRadioButton.AutoSize = true;
            this.UseTriplesRadioButton.Location = new System.Drawing.Point(19, 65);
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
            this.UseDoublesRadioButton.Location = new System.Drawing.Point(19, 42);
            this.UseDoublesRadioButton.Name = "UseDoublesRadioButton";
            this.UseDoublesRadioButton.Size = new System.Drawing.Size(195, 17);
            this.UseDoublesRadioButton.TabIndex = 1;
            this.UseDoublesRadioButton.TabStop = true;
            this.UseDoublesRadioButton.Text = "Use doubles measurement precision";
            this.UseDoublesRadioButton.UseVisualStyleBackColor = true;
            this.UseDoublesRadioButton.CheckedChanged += new System.EventHandler(this.UseDoublesRadioButton_CheckedChanged);
            // 
            // UseNumCyclesRadioButton
            // 
            this.UseNumCyclesRadioButton.AutoSize = true;
            this.UseNumCyclesRadioButton.Location = new System.Drawing.Point(19, 19);
            this.UseNumCyclesRadioButton.Name = "UseNumCyclesRadioButton";
            this.UseNumCyclesRadioButton.Size = new System.Drawing.Size(127, 17);
            this.UseNumCyclesRadioButton.TabIndex = 0;
            this.UseNumCyclesRadioButton.TabStop = true;
            this.UseNumCyclesRadioButton.Text = "Use number of cycles";
            this.UseNumCyclesRadioButton.UseVisualStyleBackColor = true;
            this.UseNumCyclesRadioButton.CheckedChanged += new System.EventHandler(this.UseNumCyclesRadioButton_CheckedChanged);
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(482, 23);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 2;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(482, 52);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 3;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(482, 81);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 4;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // SourceIdTextBox
            // 
            this.SourceIdTextBox.Location = new System.Drawing.Point(157, 113);
            this.SourceIdTextBox.Name = "SourceIdTextBox";
            this.SourceIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.SourceIdTextBox.TabIndex = 5;
            this.SourceIdTextBox.Leave += new System.EventHandler(this.SourceIdTextBox_Leave);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(157, 139);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.Size = new System.Drawing.Size(299, 20);
            this.CommentTextBox.TabIndex = 6;
            this.CommentTextBox.Leave += new System.EventHandler(this.CommentTextBox_Leave);
            // 
            // CountTimeTextBox
            // 
            this.CountTimeTextBox.Location = new System.Drawing.Point(157, 165);
            this.CountTimeTextBox.Name = "CountTimeTextBox";
            this.CountTimeTextBox.Size = new System.Drawing.Size(100, 20);
            this.CountTimeTextBox.TabIndex = 7;
            this.CountTimeTextBox.Leave += new System.EventHandler(this.CountTimeTextBox_Leave);
            // 
            // NumCyclesTextBox
            // 
            this.NumCyclesTextBox.Location = new System.Drawing.Point(157, 294);
            this.NumCyclesTextBox.Name = "NumCyclesTextBox";
            this.NumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.NumCyclesTextBox.TabIndex = 8;
            this.NumCyclesTextBox.Leave += new System.EventHandler(this.NumCyclesTextBox_Leave);
            // 
            // MeasPrecisionTextBox
            // 
            this.MeasPrecisionTextBox.Location = new System.Drawing.Point(157, 320);
            this.MeasPrecisionTextBox.Name = "MeasPrecisionTextBox";
            this.MeasPrecisionTextBox.Size = new System.Drawing.Size(100, 20);
            this.MeasPrecisionTextBox.TabIndex = 9;
            this.MeasPrecisionTextBox.Leave += new System.EventHandler(this.MeasPrecisionTextBox_Leave);
            // 
            // MinNumCyclesTextBox
            // 
            this.MinNumCyclesTextBox.Location = new System.Drawing.Point(157, 346);
            this.MinNumCyclesTextBox.Name = "MinNumCyclesTextBox";
            this.MinNumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.MinNumCyclesTextBox.TabIndex = 10;
            this.MinNumCyclesTextBox.Leave += new System.EventHandler(this.MinNumCyclesTextBox_Leave);
            // 
            // MaxNumCyclesTextBox
            // 
            this.MaxNumCyclesTextBox.Location = new System.Drawing.Point(157, 372);
            this.MaxNumCyclesTextBox.Name = "MaxNumCyclesTextBox";
            this.MaxNumCyclesTextBox.Size = new System.Drawing.Size(100, 20);
            this.MaxNumCyclesTextBox.TabIndex = 11;
            this.MaxNumCyclesTextBox.Leave += new System.EventHandler(this.MaxNumCyclesTextBox_Leave);
            // 
            // DistanceToMoveTextBox
            // 
            this.DistanceToMoveTextBox.Location = new System.Drawing.Point(470, 436);
            this.DistanceToMoveTextBox.Name = "DistanceToMoveTextBox";
            this.DistanceToMoveTextBox.Size = new System.Drawing.Size(100, 20);
            this.DistanceToMoveTextBox.TabIndex = 12;
            this.DistanceToMoveTextBox.Leave += new System.EventHandler(this.DistanceToMoveTextBox_Leave);
            // 
            // QCTestsCheckBox
            // 
            this.QCTestsCheckBox.AutoSize = true;
            this.QCTestsCheckBox.Location = new System.Drawing.Point(471, 210);
            this.QCTestsCheckBox.Name = "QCTestsCheckBox";
            this.QCTestsCheckBox.Size = new System.Drawing.Size(66, 17);
            this.QCTestsCheckBox.TabIndex = 13;
            this.QCTestsCheckBox.Text = "QC tests";
            this.QCTestsCheckBox.UseVisualStyleBackColor = true;
            this.QCTestsCheckBox.CheckedChanged += new System.EventHandler(this.QCTestsCheckBox_CheckedChanged);
            // 
            // PrintResultsCheckBox
            // 
            this.PrintResultsCheckBox.AutoSize = true;
            this.PrintResultsCheckBox.Location = new System.Drawing.Point(471, 233);
            this.PrintResultsCheckBox.Name = "PrintResultsCheckBox";
            this.PrintResultsCheckBox.Size = new System.Drawing.Size(80, 17);
            this.PrintResultsCheckBox.TabIndex = 14;
            this.PrintResultsCheckBox.Text = "Print results";
            this.PrintResultsCheckBox.UseVisualStyleBackColor = true;
            this.PrintResultsCheckBox.CheckedChanged += new System.EventHandler(this.PrintResultsCheckBox_CheckedChanged);
            // 
            // CommentAtEndCheckBox
            // 
            this.CommentAtEndCheckBox.AutoSize = true;
            this.CommentAtEndCheckBox.Location = new System.Drawing.Point(471, 256);
            this.CommentAtEndCheckBox.Name = "CommentAtEndCheckBox";
            this.CommentAtEndCheckBox.Size = new System.Drawing.Size(181, 17);
            this.CommentAtEndCheckBox.TabIndex = 15;
            this.CommentAtEndCheckBox.Text = "Comment at end of measurement";
            this.CommentAtEndCheckBox.UseVisualStyleBackColor = true;
            this.CommentAtEndCheckBox.CheckedChanged += new System.EventHandler(this.CommentAtEndCheckBox_CheckedChanged);
            // 
            // UseAddASourceCheckBox
            // 
            this.UseAddASourceCheckBox.AutoSize = true;
            this.UseAddASourceCheckBox.Location = new System.Drawing.Point(157, 398);
            this.UseAddASourceCheckBox.Name = "UseAddASourceCheckBox";
            this.UseAddASourceCheckBox.Size = new System.Drawing.Size(346, 17);
            this.UseAddASourceCheckBox.TabIndex = 16;
            this.UseAddASourceCheckBox.Text = "Use add-a-source Cf252 source for normalization test measurements";
            this.UseAddASourceCheckBox.UseVisualStyleBackColor = true;
            this.UseAddASourceCheckBox.CheckedChanged += new System.EventHandler(this.UseAddASourceCheckBox_CheckedChanged);
            // 
            // DataSourceComboBox
            // 
            this.DataSourceComboBox.FormattingEnabled = true;
            this.DataSourceComboBox.Location = new System.Drawing.Point(471, 320);
            this.DataSourceComboBox.Name = "DataSourceComboBox";
            this.DataSourceComboBox.Size = new System.Drawing.Size(121, 21);
            this.DataSourceComboBox.TabIndex = 17;
            this.DataSourceComboBox.SelectedIndexChanged += new System.EventHandler(this.DataSourceComboBox_SelectedIndexChanged);
            // 
            // SourceIdLabel
            // 
            this.SourceIdLabel.AutoSize = true;
            this.SourceIdLabel.Location = new System.Drawing.Point(93, 116);
            this.SourceIdLabel.Name = "SourceIdLabel";
            this.SourceIdLabel.Size = new System.Drawing.Size(52, 13);
            this.SourceIdLabel.TabIndex = 18;
            this.SourceIdLabel.Text = "Source id";
            // 
            // CommentLabel
            // 
            this.CommentLabel.AutoSize = true;
            this.CommentLabel.Location = new System.Drawing.Point(94, 142);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(51, 13);
            this.CommentLabel.TabIndex = 19;
            this.CommentLabel.Text = "Comment";
            // 
            // CountTimeLabel
            // 
            this.CountTimeLabel.AutoSize = true;
            this.CountTimeLabel.Location = new System.Drawing.Point(57, 168);
            this.CountTimeLabel.Name = "CountTimeLabel";
            this.CountTimeLabel.Size = new System.Drawing.Size(88, 13);
            this.CountTimeLabel.TabIndex = 20;
            this.CountTimeLabel.Text = "Count time (secs)";
            // 
            // NumCyclesLabel
            // 
            this.NumCyclesLabel.AutoSize = true;
            this.NumCyclesLabel.Location = new System.Drawing.Point(55, 297);
            this.NumCyclesLabel.Name = "NumCyclesLabel";
            this.NumCyclesLabel.Size = new System.Drawing.Size(89, 13);
            this.NumCyclesLabel.TabIndex = 21;
            this.NumCyclesLabel.Text = "Number of cycles";
            // 
            // MeasPrecisionLabel
            // 
            this.MeasPrecisionLabel.AutoSize = true;
            this.MeasPrecisionLabel.Location = new System.Drawing.Point(10, 323);
            this.MeasPrecisionLabel.Name = "MeasPrecisionLabel";
            this.MeasPrecisionLabel.Size = new System.Drawing.Size(133, 13);
            this.MeasPrecisionLabel.TabIndex = 22;
            this.MeasPrecisionLabel.Text = "Measurement precision (%)";
            // 
            // MinNumCyclesLabel
            // 
            this.MinNumCyclesLabel.AutoSize = true;
            this.MinNumCyclesLabel.Location = new System.Drawing.Point(49, 349);
            this.MinNumCyclesLabel.Name = "MinNumCyclesLabel";
            this.MinNumCyclesLabel.Size = new System.Drawing.Size(95, 13);
            this.MinNumCyclesLabel.TabIndex = 23;
            this.MinNumCyclesLabel.Text = "Min number cycles";
            // 
            // MaxNumCyclesLabel
            // 
            this.MaxNumCyclesLabel.AutoSize = true;
            this.MaxNumCyclesLabel.Location = new System.Drawing.Point(46, 375);
            this.MaxNumCyclesLabel.Name = "MaxNumCyclesLabel";
            this.MaxNumCyclesLabel.Size = new System.Drawing.Size(98, 13);
            this.MaxNumCyclesLabel.TabIndex = 24;
            this.MaxNumCyclesLabel.Text = "Max number cycles";
            // 
            // DataSourceLabel
            // 
            this.DataSourceLabel.AutoSize = true;
            this.DataSourceLabel.Location = new System.Drawing.Point(400, 323);
            this.DataSourceLabel.Name = "DataSourceLabel";
            this.DataSourceLabel.Size = new System.Drawing.Size(65, 13);
            this.DataSourceLabel.TabIndex = 25;
            this.DataSourceLabel.Text = "Data source";
            // 
            // DistanceToMoveLabel
            // 
            this.DistanceToMoveLabel.AutoSize = true;
            this.DistanceToMoveLabel.Location = new System.Drawing.Point(206, 434);
            this.DistanceToMoveLabel.MaximumSize = new System.Drawing.Size(260, 0);
            this.DistanceToMoveLabel.Name = "DistanceToMoveLabel";
            this.DistanceToMoveLabel.Size = new System.Drawing.Size(259, 26);
            this.DistanceToMoveLabel.TabIndex = 26;
            this.DistanceToMoveLabel.Text = "Distance to move Cf252 source from home to position for normalization test measur" +
    "ements (inches)";
            // 
            // IDDAcquireInitSrc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 481);
            this.Controls.Add(this.DistanceToMoveLabel);
            this.Controls.Add(this.DataSourceLabel);
            this.Controls.Add(this.MaxNumCyclesLabel);
            this.Controls.Add(this.MinNumCyclesLabel);
            this.Controls.Add(this.MeasPrecisionLabel);
            this.Controls.Add(this.NumCyclesLabel);
            this.Controls.Add(this.CountTimeLabel);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.SourceIdLabel);
            this.Controls.Add(this.DataSourceComboBox);
            this.Controls.Add(this.UseAddASourceCheckBox);
            this.Controls.Add(this.CommentAtEndCheckBox);
            this.Controls.Add(this.PrintResultsCheckBox);
            this.Controls.Add(this.QCTestsCheckBox);
            this.Controls.Add(this.DistanceToMoveTextBox);
            this.Controls.Add(this.MaxNumCyclesTextBox);
            this.Controls.Add(this.MinNumCyclesTextBox);
            this.Controls.Add(this.MeasPrecisionTextBox);
            this.Controls.Add(this.NumCyclesTextBox);
            this.Controls.Add(this.CountTimeTextBox);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.SourceIdTextBox);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "IDDAcquireInitSrc";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Initial Source Measurement";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton AmLiNormRadioButton;
        private System.Windows.Forms.RadioButton Cf252SinglesNormRadioButton;
        private System.Windows.Forms.RadioButton Cf252DblsNormRadioButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton UseTriplesRadioButton;
        private System.Windows.Forms.RadioButton UseDoublesRadioButton;
        private System.Windows.Forms.RadioButton UseNumCyclesRadioButton;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.TextBox SourceIdTextBox;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.TextBox CountTimeTextBox;
        private System.Windows.Forms.TextBox NumCyclesTextBox;
        private System.Windows.Forms.TextBox MeasPrecisionTextBox;
        private System.Windows.Forms.TextBox MinNumCyclesTextBox;
        private System.Windows.Forms.TextBox MaxNumCyclesTextBox;
        private System.Windows.Forms.TextBox DistanceToMoveTextBox;
        private System.Windows.Forms.CheckBox QCTestsCheckBox;
        private System.Windows.Forms.CheckBox PrintResultsCheckBox;
        private System.Windows.Forms.CheckBox CommentAtEndCheckBox;
        private System.Windows.Forms.CheckBox UseAddASourceCheckBox;
        private System.Windows.Forms.ComboBox DataSourceComboBox;
        private System.Windows.Forms.Label SourceIdLabel;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.Label CountTimeLabel;
        private System.Windows.Forms.Label NumCyclesLabel;
        private System.Windows.Forms.Label MeasPrecisionLabel;
        private System.Windows.Forms.Label MinNumCyclesLabel;
        private System.Windows.Forms.Label MaxNumCyclesLabel;
        private System.Windows.Forms.Label DataSourceLabel;
        private System.Windows.Forms.Label DistanceToMoveLabel;
        private System.Windows.Forms.HelpProvider provider;
    }
}