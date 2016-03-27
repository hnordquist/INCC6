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
			System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("DetParms");
			System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("DetId");
			System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Detector", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
			System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Acquire Params");
			System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("Background Params");
			System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Isotopics");
			System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Normalization Params");
			System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Test Params");
			System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Mass + Error");
			System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("S, D, T");
			System.Windows.Forms.TreeNode treeNode11 = new System.Windows.Forms.TreeNode("Multiplicity Distributions");
			System.Windows.Forms.TreeNode treeNode12 = new System.Windows.Forms.TreeNode("LM Results");
			System.Windows.Forms.TreeNode treeNode13 = new System.Windows.Forms.TreeNode("Summaries", new System.Windows.Forms.TreeNode[] {
            treeNode9,
            treeNode10,
            treeNode11,
            treeNode12});
			System.Windows.Forms.TreeNode treeNode14 = new System.Windows.Forms.TreeNode("Identifier");
			System.Windows.Forms.TreeNode treeNode15 = new System.Windows.Forms.TreeNode("ResultsFiles");
			System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Stratum");
			System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Calibration Curve");
			System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Known Alpha");
			System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Known M");
			System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Multiplicity");
			System.Windows.Forms.TreeNode treeNode21 = new System.Windows.Forms.TreeNode("Add-a-Source");
			System.Windows.Forms.TreeNode treeNode22 = new System.Windows.Forms.TreeNode("Curium Ratio");
			System.Windows.Forms.TreeNode treeNode23 = new System.Windows.Forms.TreeNode("Truncated Multiplicity");
			System.Windows.Forms.TreeNode treeNode24 = new System.Windows.Forms.TreeNode("Active Calib Curve");
			System.Windows.Forms.TreeNode treeNode25 = new System.Windows.Forms.TreeNode("Collar");
			System.Windows.Forms.TreeNode treeNode26 = new System.Windows.Forms.TreeNode("Active Multiplicity");
			System.Windows.Forms.TreeNode treeNode27 = new System.Windows.Forms.TreeNode("Active/Passive");
			System.Windows.Forms.TreeNode treeNode28 = new System.Windows.Forms.TreeNode("Mass Analysis Methods", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18,
            treeNode19,
            treeNode20,
            treeNode21,
            treeNode22,
            treeNode23,
            treeNode24,
            treeNode25,
            treeNode26,
            treeNode27});
			System.Windows.Forms.TreeNode treeNode29 = new System.Windows.Forms.TreeNode("Measurement", new System.Windows.Forms.TreeNode[] {
            treeNode3,
            treeNode4,
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode13,
            treeNode14,
            treeNode15,
            treeNode16,
            treeNode28});
			this.InspectionNumComboBox = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.DefaultLastEnteredRadioBtn = new System.Windows.Forms.RadioButton();
			this.SortByDetectorRadioBtn = new System.Windows.Forms.RadioButton();
			this.DefaultCurrentRadioBtn = new System.Windows.Forms.RadioButton();
			this.SortByStratRadioBtn = new System.Windows.Forms.RadioButton();
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
			this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// InspectionNumComboBox
			// 
			this.InspectionNumComboBox.FormattingEnabled = true;
			this.InspectionNumComboBox.Location = new System.Drawing.Point(227, 20);
			this.InspectionNumComboBox.Margin = new System.Windows.Forms.Padding(4);
			this.InspectionNumComboBox.Name = "InspectionNumComboBox";
			this.InspectionNumComboBox.Size = new System.Drawing.Size(204, 24);
			this.InspectionNumComboBox.TabIndex = 0;
			this.InspectionNumComboBox.SelectedIndexChanged += new System.EventHandler(this.InspectionNumComboBox_SelectedIndexChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.DefaultLastEnteredRadioBtn);
			this.groupBox1.Controls.Add(this.SortByDetectorRadioBtn);
			this.groupBox1.Controls.Add(this.DefaultCurrentRadioBtn);
			this.groupBox1.Controls.Add(this.SortByStratRadioBtn);
			this.groupBox1.Location = new System.Drawing.Point(25, 48);
			this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
			this.groupBox1.Size = new System.Drawing.Size(409, 77);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			// 
			// DefaultLastEnteredRadioBtn
			// 
			this.DefaultLastEnteredRadioBtn.AutoSize = true;
			this.DefaultLastEnteredRadioBtn.Location = new System.Drawing.Point(176, 45);
			this.DefaultLastEnteredRadioBtn.Margin = new System.Windows.Forms.Padding(4);
			this.DefaultLastEnteredRadioBtn.Name = "DefaultLastEnteredRadioBtn";
			this.DefaultLastEnteredRadioBtn.Size = new System.Drawing.Size(178, 21);
			this.DefaultLastEnteredRadioBtn.TabIndex = 1;
			this.DefaultLastEnteredRadioBtn.TabStop = true;
			this.DefaultLastEnteredRadioBtn.Text = "Default to last date time";
			this.DefaultLastEnteredRadioBtn.UseVisualStyleBackColor = true;
			this.DefaultLastEnteredRadioBtn.CheckedChanged += new System.EventHandler(this.DefaultLastEnteredRadioBtn_CheckedChanged);
			// 
			// SortByDetectorRadioBtn
			// 
			this.SortByDetectorRadioBtn.AutoSize = true;
			this.SortByDetectorRadioBtn.Location = new System.Drawing.Point(17, 45);
			this.SortByDetectorRadioBtn.Margin = new System.Windows.Forms.Padding(4);
			this.SortByDetectorRadioBtn.Name = "SortByDetectorRadioBtn";
			this.SortByDetectorRadioBtn.Size = new System.Drawing.Size(145, 21);
			this.SortByDetectorRadioBtn.TabIndex = 1;
			this.SortByDetectorRadioBtn.TabStop = true;
			this.SortByDetectorRadioBtn.Text = "Sort by detector id";
			this.SortByDetectorRadioBtn.UseVisualStyleBackColor = true;
			this.SortByDetectorRadioBtn.CheckedChanged += new System.EventHandler(this.SortByDetectorRadioBtn_CheckedChanged);
			// 
			// DefaultCurrentRadioBtn
			// 
			this.DefaultCurrentRadioBtn.AutoSize = true;
			this.DefaultCurrentRadioBtn.Location = new System.Drawing.Point(176, 16);
			this.DefaultCurrentRadioBtn.Margin = new System.Windows.Forms.Padding(4);
			this.DefaultCurrentRadioBtn.Name = "DefaultCurrentRadioBtn";
			this.DefaultCurrentRadioBtn.Size = new System.Drawing.Size(229, 21);
			this.DefaultCurrentRadioBtn.TabIndex = 0;
			this.DefaultCurrentRadioBtn.TabStop = true;
			this.DefaultCurrentRadioBtn.Text = "Default to current end date time";
			this.DefaultCurrentRadioBtn.UseVisualStyleBackColor = true;
			this.DefaultCurrentRadioBtn.CheckedChanged += new System.EventHandler(this.DefaultCurrentRadioBtn_CheckedChanged);
			// 
			// SortByStratRadioBtn
			// 
			this.SortByStratRadioBtn.AutoSize = true;
			this.SortByStratRadioBtn.Location = new System.Drawing.Point(17, 16);
			this.SortByStratRadioBtn.Margin = new System.Windows.Forms.Padding(4);
			this.SortByStratRadioBtn.Name = "SortByStratRadioBtn";
			this.SortByStratRadioBtn.Size = new System.Drawing.Size(140, 21);
			this.SortByStratRadioBtn.TabIndex = 0;
			this.SortByStratRadioBtn.TabStop = true;
			this.SortByStratRadioBtn.Text = "Sort by stratum id";
			this.SortByStratRadioBtn.UseVisualStyleBackColor = true;
			this.SortByStratRadioBtn.CheckedChanged += new System.EventHandler(this.SortByStratRadioBtn_CheckedChanged);
			// 
			// PrintCheckBox
			// 
			this.PrintCheckBox.AutoSize = true;
			this.PrintCheckBox.Location = new System.Drawing.Point(727, 22);
			this.PrintCheckBox.Margin = new System.Windows.Forms.Padding(4);
			this.PrintCheckBox.Name = "PrintCheckBox";
			this.PrintCheckBox.Size = new System.Drawing.Size(59, 21);
			this.PrintCheckBox.TabIndex = 7;
			this.PrintCheckBox.Text = "Print";
			this.PrintCheckBox.UseVisualStyleBackColor = true;
			this.PrintCheckBox.CheckedChanged += new System.EventHandler(this.PrintCheckBox_CheckedChanged);
			// 
			// InspectionNumLabel
			// 
			this.InspectionNumLabel.AutoSize = true;
			this.InspectionNumLabel.Location = new System.Drawing.Point(93, 23);
			this.InspectionNumLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.InspectionNumLabel.Name = "InspectionNumLabel";
			this.InspectionNumLabel.Size = new System.Drawing.Size(124, 17);
			this.InspectionNumLabel.TabIndex = 8;
			this.InspectionNumLabel.Text = "Inspection number";
			// 
			// StartDateLabel
			// 
			this.StartDateLabel.AutoSize = true;
			this.StartDateLabel.Location = new System.Drawing.Point(466, 64);
			this.StartDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.StartDateLabel.Name = "StartDateLabel";
			this.StartDateLabel.Size = new System.Drawing.Size(38, 17);
			this.StartDateLabel.TabIndex = 9;
			this.StartDateLabel.Text = "Start";
			// 
			// EndDateLabel
			// 
			this.EndDateLabel.AutoSize = true;
			this.EndDateLabel.Location = new System.Drawing.Point(471, 94);
			this.EndDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.EndDateLabel.Name = "EndDateLabel";
			this.EndDateLabel.Size = new System.Drawing.Size(33, 17);
			this.EndDateLabel.TabIndex = 10;
			this.EndDateLabel.Text = "End";
			// 
			// OKBtn
			// 
			this.OKBtn.Location = new System.Drawing.Point(812, 12);
			this.OKBtn.Margin = new System.Windows.Forms.Padding(4);
			this.OKBtn.Name = "OKBtn";
			this.OKBtn.Size = new System.Drawing.Size(100, 28);
			this.OKBtn.TabIndex = 13;
			this.OKBtn.Text = "OK";
			this.OKBtn.UseVisualStyleBackColor = true;
			this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
			// 
			// CancelBtn
			// 
			this.CancelBtn.Location = new System.Drawing.Point(812, 48);
			this.CancelBtn.Margin = new System.Windows.Forms.Padding(4);
			this.CancelBtn.Name = "CancelBtn";
			this.CancelBtn.Size = new System.Drawing.Size(100, 28);
			this.CancelBtn.TabIndex = 14;
			this.CancelBtn.Text = "Cancel";
			this.CancelBtn.UseVisualStyleBackColor = true;
			this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
			// 
			// HelpBtn
			// 
			this.HelpBtn.Location = new System.Drawing.Point(812, 84);
			this.HelpBtn.Margin = new System.Windows.Forms.Padding(4);
			this.HelpBtn.Name = "HelpBtn";
			this.HelpBtn.Size = new System.Drawing.Size(100, 28);
			this.HelpBtn.TabIndex = 15;
			this.HelpBtn.Text = "Help";
			this.HelpBtn.UseVisualStyleBackColor = true;
			this.HelpBtn.Click += new System.EventHandler(this.HelpBtn_Click);
			// 
			// StartDateTimePicker
			// 
			this.StartDateTimePicker.Location = new System.Drawing.Point(521, 59);
			this.StartDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
			this.StartDateTimePicker.Name = "StartDateTimePicker";
			this.StartDateTimePicker.Size = new System.Drawing.Size(265, 22);
			this.StartDateTimePicker.TabIndex = 16;
			this.StartDateTimePicker.ValueChanged += new System.EventHandler(this.StartDateTimePicker_ValueChanged);
			// 
			// EndDateTimePicker
			// 
			this.EndDateTimePicker.Location = new System.Drawing.Point(522, 90);
			this.EndDateTimePicker.Margin = new System.Windows.Forms.Padding(4);
			this.EndDateTimePicker.Name = "EndDateTimePicker";
			this.EndDateTimePicker.Size = new System.Drawing.Size(265, 22);
			this.EndDateTimePicker.TabIndex = 17;
			this.EndDateTimePicker.ValueChanged += new System.EventHandler(this.EndDateTimePicker_ValueChanged);
			// 
			// treeView1
			// 
			this.treeView1.CheckBoxes = true;
			this.treeView1.Location = new System.Drawing.Point(1, 146);
			this.treeView1.Name = "treeView1";
			treeNode1.Name = "Detector Parameters";
			treeNode1.Text = "DetParms";
			treeNode2.Name = "Detector Identifier";
			treeNode2.Text = "DetId";
			treeNode3.Name = "Detector";
			treeNode3.Text = "Detector";
			treeNode4.Name = "Acquire";
			treeNode4.Text = "Acquire Params";
			treeNode4.ToolTipText = "Facility, Item Id, Comments, &c;";
			treeNode5.Name = "Background";
			treeNode5.Text = "Background Params";
			treeNode6.Name = "Isotopics";
			treeNode6.Text = "Isotopics";
			treeNode7.Name = "Normal";
			treeNode7.Text = "Normalization Params";
			treeNode8.Name = "Test";
			treeNode8.Text = "Test Params";
			treeNode9.Name = "Mass";
			treeNode9.Text = "Mass + Error";
			treeNode10.Name = "SDT";
			treeNode10.Text = "S, D, T";
			treeNode11.Name = "Mult";
			treeNode11.Text = "Multiplicity Distributions";
			treeNode12.Name = "LM";
			treeNode12.Text = "LM Results";
			treeNode13.Name = "Summaries";
			treeNode13.Text = "Summaries";
			treeNode14.Name = "MeasId";
			treeNode14.Text = "Identifier";
			treeNode15.Name = "Results Files";
			treeNode15.Text = "ResultsFiles";
			treeNode16.Name = "Stratum";
			treeNode16.Text = "Stratum";
			treeNode17.Name = "Calibration";
			treeNode17.Text = "Calibration Curve";
			treeNode18.Name = "KnownA";
			treeNode18.Text = "Known Alpha";
			treeNode19.Name = "KnownM";
			treeNode19.Text = "Known M";
			treeNode20.Name = "Multiplicity";
			treeNode20.Text = "Multiplicity";
			treeNode21.Name = "AddASource";
			treeNode21.Text = "Add-a-Source";
			treeNode22.Name = "Curium";
			treeNode22.Text = "Curium Ratio";
			treeNode23.Name = "Multiplicity";
			treeNode23.Text = "Truncated Multiplicity";
			treeNode24.Name = "ActiveCalib";
			treeNode24.Text = "Active Calib Curve";
			treeNode25.Name = "Collar";
			treeNode25.Text = "Collar";
			treeNode26.Name = "ActiveMult";
			treeNode26.Text = "Active Multiplicity";
			treeNode27.Name = "ActivePassive";
			treeNode27.Text = "Active/Passive";
			treeNode28.Name = "Methods";
			treeNode28.Text = "Mass Analysis Methods";
			treeNode29.Name = "Meas";
			treeNode29.Text = "Measurement";
			this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode29});
			this.treeView1.Size = new System.Drawing.Size(911, 505);
			this.treeView1.TabIndex = 18;
			this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// elementHost1
			// 
			this.elementHost1.Location = new System.Drawing.Point(662, 366);
			this.elementHost1.Name = "elementHost1";
			this.elementHost1.Size = new System.Drawing.Size(8, 8);
			this.elementHost1.TabIndex = 19;
			this.elementHost1.Text = "elementHost1";
			this.elementHost1.Child = null;
			// 
			// IDDAssaySummary
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(917, 663);
			this.Controls.Add(this.elementHost1);
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
			this.Controls.Add(this.InspectionNumComboBox);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "IDDAssaySummary";
			this.Text = "Verification Summary for All Detectors";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
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
		private System.Windows.Forms.Integration.ElementHost elementHost1;
	}
}