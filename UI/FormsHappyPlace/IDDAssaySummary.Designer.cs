namespace UI
{
    partial class IDDAssaySummary
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Acquire Params");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Detector Parameters");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Normalization Params");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Background Params");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Summaries");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Calibration Curve");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Known Alpha");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Known M");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Multiplicity");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Add-a-Source");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Curium Ratio");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Truncated Multiplicity");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Active Calib Curve");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Collar");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Active Multiplicity");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Active/Passive");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Mass Analysis Methods", new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode14,
            treeNode15,
            treeNode16});
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Comments");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("LM Results");
            this.InspectionNumComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.SortByDetectorRadioBtn = new System.Windows.Forms.RadioButton();
            this.SortByStratRadioBtn = new System.Windows.Forms.RadioButton();
            this.DefaultLastEnteredRadioBtn = new System.Windows.Forms.RadioButton();
            this.DefaultCurrentRadioBtn = new System.Windows.Forms.RadioButton();
            this.PrintCheckBox = new System.Windows.Forms.CheckBox();
            this.InspectionNumLabel = new System.Windows.Forms.Label();
            this.StartDateLabel = new System.Windows.Forms.Label();
            this.EndDateLabel = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.HelpBtn = new System.Windows.Forms.Button();
            this.StartDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.EndDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // InspectionNumComboBox
            // 
            this.InspectionNumComboBox.FormattingEnabled = true;
            this.InspectionNumComboBox.Location = new System.Drawing.Point(186, 8);
            this.InspectionNumComboBox.Name = "InspectionNumComboBox";
            this.InspectionNumComboBox.Size = new System.Drawing.Size(154, 21);
            this.InspectionNumComboBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.SortByDetectorRadioBtn);
            this.groupBox1.Controls.Add(this.SortByStratRadioBtn);
            this.groupBox1.Location = new System.Drawing.Point(4, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(145, 63);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // SortByDetectorRadioBtn
            // 
            this.SortByDetectorRadioBtn.AutoSize = true;
            this.SortByDetectorRadioBtn.Location = new System.Drawing.Point(13, 37);
            this.SortByDetectorRadioBtn.Name = "SortByDetectorRadioBtn";
            this.SortByDetectorRadioBtn.Size = new System.Drawing.Size(111, 17);
            this.SortByDetectorRadioBtn.TabIndex = 1;
            this.SortByDetectorRadioBtn.TabStop = true;
            this.SortByDetectorRadioBtn.Text = "Sort by detector id";
            this.SortByDetectorRadioBtn.UseVisualStyleBackColor = true;
            // 
            // SortByStratRadioBtn
            // 
            this.SortByStratRadioBtn.AutoSize = true;
            this.SortByStratRadioBtn.Location = new System.Drawing.Point(13, 13);
            this.SortByStratRadioBtn.Name = "SortByStratRadioBtn";
            this.SortByStratRadioBtn.Size = new System.Drawing.Size(106, 17);
            this.SortByStratRadioBtn.TabIndex = 0;
            this.SortByStratRadioBtn.TabStop = true;
            this.SortByStratRadioBtn.Text = "Sort by stratum id";
            this.SortByStratRadioBtn.UseVisualStyleBackColor = true;
            this.SortByStratRadioBtn.CheckedChanged += new System.EventHandler(this.SortByStratRadioBtn_CheckedChanged);
            // 
            // DefaultLastEnteredRadioBtn
            // 
            this.DefaultLastEnteredRadioBtn.AutoSize = true;
            this.DefaultLastEnteredRadioBtn.Location = new System.Drawing.Point(6, 37);
            this.DefaultLastEnteredRadioBtn.Name = "DefaultLastEnteredRadioBtn";
            this.DefaultLastEnteredRadioBtn.Size = new System.Drawing.Size(136, 17);
            this.DefaultLastEnteredRadioBtn.TabIndex = 1;
            this.DefaultLastEnteredRadioBtn.TabStop = true;
            this.DefaultLastEnteredRadioBtn.Text = "Default to last date time";
            this.DefaultLastEnteredRadioBtn.UseVisualStyleBackColor = true;
            // 
            // DefaultCurrentRadioBtn
            // 
            this.DefaultCurrentRadioBtn.AutoSize = true;
            this.DefaultCurrentRadioBtn.Location = new System.Drawing.Point(6, 13);
            this.DefaultCurrentRadioBtn.Name = "DefaultCurrentRadioBtn";
            this.DefaultCurrentRadioBtn.Size = new System.Drawing.Size(174, 17);
            this.DefaultCurrentRadioBtn.TabIndex = 0;
            this.DefaultCurrentRadioBtn.TabStop = true;
            this.DefaultCurrentRadioBtn.Text = "Default to current end date time";
            this.DefaultCurrentRadioBtn.UseVisualStyleBackColor = true;
            this.DefaultCurrentRadioBtn.CheckedChanged += new System.EventHandler(this.DefaultCurrentRadioBtn_CheckedChanged);
            // 
            // PrintCheckBox
            // 
            this.PrintCheckBox.AutoSize = true;
            this.PrintCheckBox.Location = new System.Drawing.Point(76, 111);
            this.PrintCheckBox.Name = "PrintCheckBox";
            this.PrintCheckBox.Size = new System.Drawing.Size(47, 17);
            this.PrintCheckBox.TabIndex = 7;
            this.PrintCheckBox.Text = "Print";
            this.PrintCheckBox.UseVisualStyleBackColor = true;
            this.PrintCheckBox.CheckedChanged += new System.EventHandler(this.PrintCheckBox_CheckedChanged);
            // 
            // InspectionNumLabel
            // 
            this.InspectionNumLabel.AutoSize = true;
            this.InspectionNumLabel.Location = new System.Drawing.Point(86, 11);
            this.InspectionNumLabel.Name = "InspectionNumLabel";
            this.InspectionNumLabel.Size = new System.Drawing.Size(94, 13);
            this.InspectionNumLabel.TabIndex = 8;
            this.InspectionNumLabel.Text = "Inspection number";
            // 
            // StartDateLabel
            // 
            this.StartDateLabel.AutoSize = true;
            this.StartDateLabel.Location = new System.Drawing.Point(178, 100);
            this.StartDateLabel.Name = "StartDateLabel";
            this.StartDateLabel.Size = new System.Drawing.Size(29, 13);
            this.StartDateLabel.TabIndex = 9;
            this.StartDateLabel.Text = "Start";
            // 
            // EndDateLabel
            // 
            this.EndDateLabel.AutoSize = true;
            this.EndDateLabel.Location = new System.Drawing.Point(181, 124);
            this.EndDateLabel.Name = "EndDateLabel";
            this.EndDateLabel.Size = new System.Drawing.Size(26, 13);
            this.EndDateLabel.TabIndex = 10;
            this.EndDateLabel.Text = "End";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(346, 9);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 13;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(346, 38);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 14;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // HelpBtn
            // 
            this.HelpBtn.Location = new System.Drawing.Point(346, 67);
            this.HelpBtn.Name = "HelpBtn";
            this.HelpBtn.Size = new System.Drawing.Size(75, 23);
            this.HelpBtn.TabIndex = 15;
            this.HelpBtn.Text = "Help";
            this.HelpBtn.UseVisualStyleBackColor = true;
            this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
            // 
            // StartDateTimePicker
            // 
            this.StartDateTimePicker.Location = new System.Drawing.Point(219, 96);
            this.StartDateTimePicker.Name = "StartDateTimePicker";
            this.StartDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.StartDateTimePicker.TabIndex = 16;
            // 
            // EndDateTimePicker
            // 
            this.EndDateTimePicker.Location = new System.Drawing.Point(220, 121);
            this.EndDateTimePicker.Name = "EndDateTimePicker";
            this.EndDateTimePicker.Size = new System.Drawing.Size(200, 20);
            this.EndDateTimePicker.TabIndex = 17;
            // 
            // treeView1
            // 
            this.treeView1.CheckBoxes = true;
            this.treeView1.Location = new System.Drawing.Point(4, 147);
            this.treeView1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "AcquireParams";
            treeNode1.Tag = false;
            treeNode1.Text = "Acquire Params";
            treeNode1.ToolTipText = "Facility, Item Id, Comments, &c;";
            treeNode2.Name = "DetectorParams";
            treeNode2.Tag = false;
            treeNode2.Text = "Detector Parameters";
            treeNode3.Name = "NormalizationParams";
            treeNode3.Tag = false;
            treeNode3.Text = "Normalization Params";
            treeNode4.Name = "BackgroundParams";
            treeNode4.Tag = false;
            treeNode4.Text = "Background Params";
            treeNode5.Name = "Summaries";
            treeNode5.Tag = false;
            treeNode5.Text = "Summaries";
            treeNode6.Name = "CalibrationCurve";
            treeNode6.Tag = true;
            treeNode6.Text = "Calibration Curve";
            treeNode7.Name = "KnownAlpha";
            treeNode7.Tag = true;
            treeNode7.Text = "Known Alpha";
            treeNode8.Name = "KnownM";
            treeNode8.Tag = true;
            treeNode8.Text = "Known M";
            treeNode9.Name = "Multiplicity";
            treeNode9.Tag = true;
            treeNode9.Text = "Multiplicity";
            treeNode10.Name = "AddASource";
            treeNode10.Tag = true;
            treeNode10.Text = "Add-a-Source";
            treeNode11.Name = "CuriumRatio";
            treeNode11.Tag = true;
            treeNode11.Text = "Curium Ratio";
            treeNode12.Name = "TruncatedMultiplicity";
            treeNode12.Tag = true;
            treeNode12.Text = "Truncated Multiplicity";
            treeNode13.Name = "ActiveCalibCurve";
            treeNode13.Tag = true;
            treeNode13.Text = "Active Calib Curve";
            treeNode14.Name = "Collar";
            treeNode14.Tag = true;
            treeNode14.Text = "Collar";
            treeNode15.Name = "ActiveMultiplicity";
            treeNode15.Tag = true;
            treeNode15.Text = "Active Multiplicity";
            treeNode16.Name = "ActivePassive";
            treeNode16.Tag = true;
            treeNode16.Text = "Active/Passive";
            treeNode17.Name = "MassAnalysisMethods";
            treeNode17.Tag = false;
            treeNode17.Text = "Mass Analysis Methods";
            treeNode18.Name = "Comments";
            treeNode18.Tag = false;
            treeNode18.Text = "Comments";
            treeNode19.Name = "LMResults";
            treeNode19.Tag = false;
            treeNode19.Text = "LM Results";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode17,
            treeNode18,
            treeNode19});
            this.treeView1.Size = new System.Drawing.Size(418, 325);
            this.treeView1.TabIndex = 18;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.DefaultCurrentRadioBtn);
            this.groupBox2.Controls.Add(this.DefaultLastEnteredRadioBtn);
            this.groupBox2.Location = new System.Drawing.Point(155, 27);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(186, 63);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // IDDAssaySummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 473);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.EndDateTimePicker);
            this.Controls.Add(this.StartDateTimePicker);
            this.Controls.Add(this.HelpBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.EndDateLabel);
            this.Controls.Add(this.StartDateLabel);
            this.Controls.Add(this.InspectionNumLabel);
            this.Controls.Add(this.PrintCheckBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.InspectionNumComboBox);
            this.Name = "IDDAssaySummary";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Summary for All Detectors";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox InspectionNumComboBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton SortByDetectorRadioBtn;
        private System.Windows.Forms.RadioButton SortByStratRadioBtn;
        private System.Windows.Forms.RadioButton DefaultLastEnteredRadioBtn;
        private System.Windows.Forms.RadioButton DefaultCurrentRadioBtn;
        private System.Windows.Forms.CheckBox PrintCheckBox;
        private System.Windows.Forms.Label InspectionNumLabel;
        private System.Windows.Forms.Label StartDateLabel;
        private System.Windows.Forms.Label EndDateLabel;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button HelpBtn;
        private System.Windows.Forms.DateTimePicker StartDateTimePicker;
        private System.Windows.Forms.DateTimePicker EndDateTimePicker;
		private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}