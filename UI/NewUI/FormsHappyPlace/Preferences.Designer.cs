namespace NewUI
{
    partial class Preferences
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
            this.components = new System.ComponentModel.Container();
            this.WorkingDirLabel = new System.Windows.Forms.Label();
            this.WorkingDirTextBox = new System.Windows.Forms.TextBox();
            this.WorkingDirInstructionsLabel = new System.Windows.Forms.Label();
            this.OKBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.root = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.DailyF0lder = new System.Windows.Forms.CheckBox();
            this.IsoFractionalDay = new System.Windows.Forms.CheckBox();
            this.PollTimer = new System.Windows.Forms.TextBox();
            this.PollPacket = new System.Windows.Forms.TextBox();
            this.OverwriteImportedDefs = new System.Windows.Forms.CheckBox();
            this.Replay = new System.Windows.Forms.CheckBox();
            this.AutoOpenCheckBox = new System.Windows.Forms.CheckBox();
            this.RevFileGen = new System.Windows.Forms.CheckBox();
            this.FPPrec = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.EnableAuxRatioReportingCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableSilentFolderCreationCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // WorkingDirLabel
            // 
            this.WorkingDirLabel.AutoSize = true;
            this.WorkingDirLabel.Location = new System.Drawing.Point(9, 15);
            this.WorkingDirLabel.Name = "WorkingDirLabel";
            this.WorkingDirLabel.Size = new System.Drawing.Size(90, 13);
            this.WorkingDirLabel.TabIndex = 0;
            this.WorkingDirLabel.Text = "Working directory";
            // 
            // WorkingDirTextBox
            // 
            this.WorkingDirTextBox.Location = new System.Drawing.Point(105, 12);
            this.WorkingDirTextBox.Name = "WorkingDirTextBox";
            this.WorkingDirTextBox.Size = new System.Drawing.Size(193, 20);
            this.WorkingDirTextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.WorkingDirTextBox, "Specify base {file location} for all input and output files");
            this.WorkingDirTextBox.Leave += new System.EventHandler(this.WorkingDirTextBox_Leave);
            // 
            // WorkingDirInstructionsLabel
            // 
            this.WorkingDirInstructionsLabel.AutoSize = true;
            this.WorkingDirInstructionsLabel.Location = new System.Drawing.Point(12, 35);
            this.WorkingDirInstructionsLabel.Name = "WorkingDirInstructionsLabel";
            this.WorkingDirInstructionsLabel.Size = new System.Drawing.Size(288, 13);
            this.WorkingDirInstructionsLabel.TabIndex = 2;
            this.WorkingDirInstructionsLabel.Text = "(prepend all relative paths for input and output files with this)";
            // 
            // OKBtn
            // 
            this.OKBtn.Location = new System.Drawing.Point(482, 10);
            this.OKBtn.Name = "OKBtn";
            this.OKBtn.Size = new System.Drawing.Size(75, 23);
            this.OKBtn.TabIndex = 3;
            this.OKBtn.Text = "OK";
            this.OKBtn.UseVisualStyleBackColor = true;
            this.OKBtn.Click += new System.EventHandler(this.OKBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.Location = new System.Drawing.Point(482, 39);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 4;
            this.CancelBtn.Text = "Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            this.CancelBtn.Click += new System.EventHandler(this.CancelBtn_Click);
            // 
            // root
            // 
            this.root.Location = new System.Drawing.Point(304, 10);
            this.root.Name = "root";
            this.root.Size = new System.Drawing.Size(75, 23);
            this.root.TabIndex = 5;
            this.root.Text = "Select...";
            this.root.UseVisualStyleBackColor = true;
            this.root.Click += new System.EventHandler(this.root_Click);
            // 
            // DailyF0lder
            // 
            this.DailyF0lder.AutoSize = true;
            this.DailyF0lder.Location = new System.Drawing.Point(386, 15);
            this.DailyF0lder.Name = "DailyF0lder";
            this.DailyF0lder.Size = new System.Drawing.Size(78, 17);
            this.DailyF0lder.TabIndex = 6;
            this.DailyF0lder.Text = "Daily folder";
            this.toolTip1.SetToolTip(this.DailyF0lder, "append a daily root file folder name to the current root file folder specificatio" +
        "n");
            this.DailyF0lder.UseVisualStyleBackColor = true;
            this.DailyF0lder.CheckedChanged += new System.EventHandler(this.DailyF0lder_CheckedChanged);
            // 
            // IsoFractionalDay
            // 
            this.IsoFractionalDay.AutoSize = true;
            this.IsoFractionalDay.Location = new System.Drawing.Point(12, 100);
            this.IsoFractionalDay.Name = "IsoFractionalDay";
            this.IsoFractionalDay.Size = new System.Drawing.Size(237, 17);
            this.IsoFractionalDay.TabIndex = 8;
            this.IsoFractionalDay.Text = "Use decay calculations rounded to each day";
            this.toolTip1.SetToolTip(this.IsoFractionalDay, "No fractional days. Use INCC isotopics whole-day decay constraint granularity");
            this.IsoFractionalDay.UseVisualStyleBackColor = true;
            this.IsoFractionalDay.CheckedChanged += new System.EventHandler(this.IsoFractionalDay_CheckedChanged);
            // 
            // PollTimer
            // 
            this.PollTimer.Location = new System.Drawing.Point(218, 209);
            this.PollTimer.Name = "PollTimer";
            this.PollTimer.Size = new System.Drawing.Size(58, 20);
            this.PollTimer.TabIndex = 13;
            this.toolTip1.SetToolTip(this.PollTimer, "status poll timer fires every n milliseconds");
            this.PollTimer.Leave += new System.EventHandler(this.PollTimer_Leave);
            // 
            // PollPacket
            // 
            this.PollPacket.AcceptsReturn = true;
            this.PollPacket.Location = new System.Drawing.Point(482, 208);
            this.PollPacket.Name = "PollPacket";
            this.PollPacket.Size = new System.Drawing.Size(58, 20);
            this.PollPacket.TabIndex = 15;
            this.toolTip1.SetToolTip(this.PollPacket, "every 1MB, (128*8192 KB), socket packet receipts, status retrieved from analyzers" +
        "");
            this.PollPacket.Leave += new System.EventHandler(this.PollPacket_Leave);
            // 
            // OverwriteImportedDefs
            // 
            this.OverwriteImportedDefs.AutoSize = true;
            this.OverwriteImportedDefs.Location = new System.Drawing.Point(363, 123);
            this.OverwriteImportedDefs.Name = "OverwriteImportedDefs";
            this.OverwriteImportedDefs.Size = new System.Drawing.Size(159, 17);
            this.OverwriteImportedDefs.TabIndex = 17;
            this.OverwriteImportedDefs.Text = "Replace imported definitions";
            this.toolTip1.SetToolTip(this.OverwriteImportedDefs, "Replace or skip imported or transferred detector and calibration parameters");
            this.OverwriteImportedDefs.UseVisualStyleBackColor = true;
            this.OverwriteImportedDefs.CheckedChanged += new System.EventHandler(this.OverwriteImportedDefs_CheckedChanged);
            // 
            // Replay
            // 
            this.Replay.AutoSize = true;
            this.Replay.Location = new System.Drawing.Point(12, 77);
            this.Replay.Name = "Replay";
            this.Replay.Size = new System.Drawing.Size(169, 17);
            this.Replay.TabIndex = 7;
            this.Replay.Text = "Recalc transfer measurements";
            this.toolTip1.SetToolTip(this.Replay, "Calculate INCC5 transfer measurement data. Unset preserves data without recalcula" +
        "ting.");
            this.Replay.UseVisualStyleBackColor = true;
            this.Replay.CheckedChanged += new System.EventHandler(this.Replay_CheckedChanged);
            // 
            // AutoOpenCheckBox
            // 
            this.AutoOpenCheckBox.AutoSize = true;
            this.AutoOpenCheckBox.Location = new System.Drawing.Point(12, 123);
            this.AutoOpenCheckBox.Name = "AutoOpenCheckBox";
            this.AutoOpenCheckBox.Size = new System.Drawing.Size(310, 17);
            this.AutoOpenCheckBox.TabIndex = 38;
            this.AutoOpenCheckBox.Text = "Automatically open INCC measurement results when finished";
            this.toolTip1.SetToolTip(this.AutoOpenCheckBox, "Open results files in notepad (and Excel when available)");
            this.AutoOpenCheckBox.UseVisualStyleBackColor = true;
            this.AutoOpenCheckBox.CheckedChanged += new System.EventHandler(this.AutoOpenCheckBox_CheckedChanged);
            // 
            // RevFileGen
            // 
            this.RevFileGen.AutoSize = true;
            this.RevFileGen.Location = new System.Drawing.Point(12, 146);
            this.RevFileGen.Name = "RevFileGen";
            this.RevFileGen.Size = new System.Drawing.Size(269, 17);
            this.RevFileGen.TabIndex = 39;
            this.RevFileGen.Text = "Include INCC5 Test data (aka Disk) file in results";
            this.toolTip1.SetToolTip(this.RevFileGen, "Creates a .dat INCC5 Test data file alongside any results output files.");
            this.RevFileGen.UseVisualStyleBackColor = true;
            this.RevFileGen.CheckedChanged += new System.EventHandler(this.RevFileGen_CheckedChanged);
            // 
            // FPPrec
            // 
            this.FPPrec.FormattingEnabled = true;
            this.FPPrec.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18"});
            this.FPPrec.Location = new System.Drawing.Point(389, 180);
            this.FPPrec.Name = "FPPrec";
            this.FPPrec.Size = new System.Drawing.Size(46, 21);
            this.FPPrec.TabIndex = 9;
            this.FPPrec.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(102, 184);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Precison of floating point results mass reporting output";
            // 
            // EnableAuxRatioReportingCheckBox
            // 
            this.EnableAuxRatioReportingCheckBox.AutoSize = true;
            this.EnableAuxRatioReportingCheckBox.Location = new System.Drawing.Point(363, 100);
            this.EnableAuxRatioReportingCheckBox.Name = "EnableAuxRatioReportingCheckBox";
            this.EnableAuxRatioReportingCheckBox.Size = new System.Drawing.Size(147, 17);
            this.EnableAuxRatioReportingCheckBox.TabIndex = 12;
            this.EnableAuxRatioReportingCheckBox.Text = "Enable Aux ratio reporting";
            this.EnableAuxRatioReportingCheckBox.UseVisualStyleBackColor = true;
            this.EnableAuxRatioReportingCheckBox.CheckedChanged += new System.EventHandler(this.EnableAuxRatioReportingCheckBox_CheckedChanged);
            // 
            // EnableSilentFolderCreationCheckBox
            // 
            this.EnableSilentFolderCreationCheckBox.AutoSize = true;
            this.EnableSilentFolderCreationCheckBox.Location = new System.Drawing.Point(363, 77);
            this.EnableSilentFolderCreationCheckBox.Name = "EnableSilentFolderCreationCheckBox";
            this.EnableSilentFolderCreationCheckBox.Size = new System.Drawing.Size(193, 17);
            this.EnableSilentFolderCreationCheckBox.TabIndex = 11;
            this.EnableSilentFolderCreationCheckBox.Text = "Enable silent missing folder creation";
            this.EnableSilentFolderCreationCheckBox.UseVisualStyleBackColor = true;
            this.EnableSilentFolderCreationCheckBox.CheckedChanged += new System.EventHandler(this.EnableSilentFolderCreationCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 212);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Status poll timer interval in milliseconds";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(289, 211);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Status poll interval per MB processed";
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(568, 241);
            this.Controls.Add(this.RevFileGen);
            this.Controls.Add(this.AutoOpenCheckBox);
            this.Controls.Add(this.OverwriteImportedDefs);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PollPacket);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PollTimer);
            this.Controls.Add(this.EnableAuxRatioReportingCheckBox);
            this.Controls.Add(this.EnableSilentFolderCreationCheckBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FPPrec);
            this.Controls.Add(this.IsoFractionalDay);
            this.Controls.Add(this.Replay);
            this.Controls.Add(this.DailyF0lder);
            this.Controls.Add(this.root);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OKBtn);
            this.Controls.Add(this.WorkingDirInstructionsLabel);
            this.Controls.Add(this.WorkingDirTextBox);
            this.Controls.Add(this.WorkingDirLabel);
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label WorkingDirLabel;
        private System.Windows.Forms.TextBox WorkingDirTextBox;
        private System.Windows.Forms.Label WorkingDirInstructionsLabel;
        private System.Windows.Forms.Button OKBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button root;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox DailyF0lder;
        private System.Windows.Forms.CheckBox Replay;
        private System.Windows.Forms.CheckBox IsoFractionalDay;
        private System.Windows.Forms.ComboBox FPPrec;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox EnableAuxRatioReportingCheckBox;
        private System.Windows.Forms.CheckBox EnableSilentFolderCreationCheckBox;
        private System.Windows.Forms.TextBox PollTimer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PollPacket;
        private System.Windows.Forms.CheckBox OverwriteImportedDefs;
        private System.Windows.Forms.CheckBox AutoOpenCheckBox;
        private System.Windows.Forms.CheckBox RevFileGen;
    }
}