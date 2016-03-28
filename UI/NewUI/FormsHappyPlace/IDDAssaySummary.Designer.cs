namespace NewUI
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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Identifier");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Detector Parameters");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Detector Identifier");
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Detector", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4});
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Mass + Error");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Summaries");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Background Params");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Isotopics");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Normalization Params");
            System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Test Params");
            System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("Stratum");
            System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Results Files");
            System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Calibration Curve");
            System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("Known Alpha");
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Known M");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Multiplicity");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Add-a-Source");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Curium Ratio");
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Truncated Multiplicity");
            System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Active Calib Curve");
            System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Collar");
            System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Active Multiplicity");
            System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Active/Passive");
            System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Mass Analysis Methods", new System.Windows.Forms.TreeNode[] {
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22,
            treeNode23,
            treeNode24});
            System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("LM Results");
            System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Measurement", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12,
            treeNode13,
            treeNode25,
            treeNode26});
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
            this.treeView1.Margin = new System.Windows.Forms.Padding(2);
            this.treeView1.Name = "treeView1";
            treeNode1.Name = "AcquireParams";
            treeNode1.Text = "Acquire Params";
            treeNode1.ToolTipText = "Facility, Item Id, Comments, &c;";
            treeNode2.Name = "Identifier";
            treeNode2.Text = "Identifier";
            treeNode3.Name = "DetParams";
            treeNode3.Text = "Detector Parameters";
            treeNode4.Name = "DetId";
            treeNode4.Text = "Detector Identifier";
            treeNode5.Name = "Detector";
            treeNode5.Text = "Detector";
            treeNode6.Name = "MassAndError";
            treeNode6.Text = "Mass + Error";
            treeNode7.Name = "Summaries";
            treeNode7.Text = "Summaries";
            treeNode8.Name = "BackgroundParams";
            treeNode8.Text = "Background Params";
            treeNode9.Name = "Isotopics";
            treeNode9.Text = "Isotopics";
            treeNode10.Name = "NormalizationParams";
            treeNode10.Text = "Normalization Params";
            treeNode11.Name = "TestParams";
            treeNode11.Text = "Test Params";
            treeNode12.Name = "Stratum";
            treeNode12.Text = "Stratum";
            treeNode13.Name = "ResultsFiles";
            treeNode13.Text = "Results Files";
            treeNode14.Name = "CalibrationCurve";
            treeNode14.Text = "Calibration Curve";
            treeNode15.Name = "KnownAlpha";
            treeNode15.Text = "Known Alpha";
            treeNode16.Name = "KnownM";
            treeNode16.Text = "Known M";
            treeNode17.Name = "Multiplicity";
            treeNode17.Text = "Multiplicity";
            treeNode18.Name = "AddASource";
            treeNode18.Text = "Add-a-Source";
            treeNode19.Name = "CuriumRatio";
            treeNode19.Text = "Curium Ratio";
            treeNode20.Name = "TruncatedMultiplicity";
            treeNode20.Text = "Truncated Multiplicity";
            treeNode21.Name = "ActiveCalibCurve";
            treeNode21.Text = "Active Calib Curve";
            treeNode22.Name = "Collar";
            treeNode22.Text = "Collar";
            treeNode23.Name = "ActiveMultiplicity";
            treeNode23.Text = "Active Multiplicity";
            treeNode24.Name = "ActivePassive";
            treeNode24.Text = "Active/Passive";
            treeNode25.Name = "MassAnalysisMethods";
            treeNode25.Text = "Mass Analysis Methods";
            treeNode26.Name = "LMResults";
            treeNode26.Text = "LM Results";
            treeNode27.Name = "Measurement";
            treeNode27.Text = "Measurement";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode27});
            this.treeView1.Size = new System.Drawing.Size(418, 325);
            this.treeView1.TabIndex = 18;
            this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
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