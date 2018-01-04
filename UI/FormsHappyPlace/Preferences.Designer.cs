﻿namespace UI
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
            this.LogFileLoc = new System.Windows.Forms.TextBox();
            this.ResultsFileLoc = new System.Windows.Forms.TextBox();
            this.DataFileLoc = new System.Windows.Forms.TextBox();
            this.Use8Char = new System.Windows.Forms.CheckBox();
            this.UseINCC5Suffix = new System.Windows.Forms.CheckBox();
            this.Incc5IniFileLoc = new System.Windows.Forms.TextBox();
            this.UseINCC5Ini = new System.Windows.Forms.CheckBox();
            this.UserDocFolder = new System.Windows.Forms.CheckBox();
            this.FPPrec = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.EnableAuxRatioReportingCheckBox = new System.Windows.Forms.CheckBox();
            this.EnableSilentFolderCreationCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.logLoc = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.resultsLoc = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.dataLoc = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.incc5IniLoc = new System.Windows.Forms.Button();
            this.UseINCC5IniLocLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // WorkingDirLabel
            // 
            this.WorkingDirLabel.AutoSize = true;
            this.WorkingDirLabel.Location = new System.Drawing.Point(9, 15);
            this.WorkingDirLabel.Name = "WorkingDirLabel";
            this.WorkingDirLabel.Size = new System.Drawing.Size(76, 13);
            this.WorkingDirLabel.TabIndex = 0;
            this.WorkingDirLabel.Text = "Working folder";
            // 
            // WorkingDirTextBox
            // 
            this.WorkingDirTextBox.Location = new System.Drawing.Point(111, 12);
            this.WorkingDirTextBox.Name = "WorkingDirTextBox";
            this.WorkingDirTextBox.Size = new System.Drawing.Size(193, 20);
            this.WorkingDirTextBox.TabIndex = 1;
            this.toolTip1.SetToolTip(this.WorkingDirTextBox, "Specify base {file location} for all input and output files");
            this.WorkingDirTextBox.Leave += new System.EventHandler(this.WorkingDirTextBox_Leave);
            // 
            // WorkingDirInstructionsLabel
            // 
            this.WorkingDirInstructionsLabel.AutoSize = true;
            this.WorkingDirInstructionsLabel.Location = new System.Drawing.Point(18, 35);
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
            this.root.Location = new System.Drawing.Point(310, 10);
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
            this.DailyF0lder.Location = new System.Drawing.Point(392, 15);
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
            this.IsoFractionalDay.Location = new System.Drawing.Point(12, 162);
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
            this.PollTimer.Location = new System.Drawing.Point(218, 316);
            this.PollTimer.Name = "PollTimer";
            this.PollTimer.Size = new System.Drawing.Size(58, 20);
            this.PollTimer.TabIndex = 13;
            this.toolTip1.SetToolTip(this.PollTimer, "status poll timer fires every n milliseconds");
            this.PollTimer.Leave += new System.EventHandler(this.PollTimer_Leave);
            // 
            // PollPacket
            // 
            this.PollPacket.AcceptsReturn = true;
            this.PollPacket.Location = new System.Drawing.Point(482, 315);
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
            this.OverwriteImportedDefs.Location = new System.Drawing.Point(406, 184);
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
            this.Replay.Location = new System.Drawing.Point(12, 139);
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
            this.AutoOpenCheckBox.Location = new System.Drawing.Point(12, 184);
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
            this.RevFileGen.Location = new System.Drawing.Point(12, 208);
            this.RevFileGen.Name = "RevFileGen";
            this.RevFileGen.Size = new System.Drawing.Size(254, 17);
            this.RevFileGen.TabIndex = 39;
            this.RevFileGen.Text = "Include INCC5 Test data (aka Disk) file in results";
            this.toolTip1.SetToolTip(this.RevFileGen, "Creates a .dat INCC5 Test data file alongside any results output files.");
            this.RevFileGen.UseVisualStyleBackColor = true;
            this.RevFileGen.CheckedChanged += new System.EventHandler(this.RevFileGen_CheckedChanged);
            // 
            // LogFileLoc
            // 
            this.LogFileLoc.Location = new System.Drawing.Point(111, 54);
            this.LogFileLoc.Name = "LogFileLoc";
            this.LogFileLoc.Size = new System.Drawing.Size(193, 20);
            this.LogFileLoc.TabIndex = 41;
            this.toolTip1.SetToolTip(this.LogFileLoc, "Specify file location for log files. Overrides the root location and daily folder" +
        "");
            this.LogFileLoc.Leave += new System.EventHandler(this.LogFileLoc_Leave);
            // 
            // ResultsFileLoc
            // 
            this.ResultsFileLoc.Location = new System.Drawing.Point(111, 81);
            this.ResultsFileLoc.Name = "ResultsFileLoc";
            this.ResultsFileLoc.Size = new System.Drawing.Size(193, 20);
            this.ResultsFileLoc.TabIndex = 44;
            this.toolTip1.SetToolTip(this.ResultsFileLoc, "Specify results file location for all results files. Overrides the root location " +
        "and daily folder");
            this.ResultsFileLoc.Leave += new System.EventHandler(this.ResultsFileLoc_Leave);
            // 
            // DataFileLoc
            // 
            this.DataFileLoc.Location = new System.Drawing.Point(111, 107);
            this.DataFileLoc.Name = "DataFileLoc";
            this.DataFileLoc.Size = new System.Drawing.Size(193, 20);
            this.DataFileLoc.TabIndex = 47;
            this.toolTip1.SetToolTip(this.DataFileLoc, "Specify data file location for all output data files. Overrides the root location" +
        " and daily folder");
            this.DataFileLoc.Leave += new System.EventHandler(this.DataFileLoc_Leave);
            // 
            // Use8Char
            // 
            this.Use8Char.AutoSize = true;
            this.Use8Char.Location = new System.Drawing.Point(406, 208);
            this.Use8Char.Name = "Use8Char";
            this.Use8Char.Size = new System.Drawing.Size(198, 17);
            this.Use8Char.TabIndex = 49;
            this.Use8Char.Text = "Use 8 char INCC5 results file naming";
            this.toolTip1.SetToolTip(this.Use8Char, "Encoded date and time in an 8 char file name");
            this.Use8Char.UseVisualStyleBackColor = true;
            this.Use8Char.CheckedChanged += new System.EventHandler(this.Use8Char_CheckedChanged);
            // 
            // UseINCC5Suffix
            // 
            this.UseINCC5Suffix.AutoSize = true;
            this.UseINCC5Suffix.Location = new System.Drawing.Point(406, 232);
            this.UseINCC5Suffix.Name = "UseINCC5Suffix";
            this.UseINCC5Suffix.Size = new System.Drawing.Size(166, 17);
            this.UseINCC5Suffix.TabIndex = 50;
            this.UseINCC5Suffix.Text = "Use INCC5 results file suffixes";
            this.toolTip1.SetToolTip(this.UseINCC5Suffix, "Text result suffixes: .RTS, .BKG, .INS, .NOR, .PRE, .VER, .CAL, .HUP");
            this.UseINCC5Suffix.UseVisualStyleBackColor = true;
            this.UseINCC5Suffix.CheckedChanged += new System.EventHandler(this.UseINCC5Suffix_CheckedChanged);
            // 
            // Incc5IniFileLoc
            // 
            this.Incc5IniFileLoc.Location = new System.Drawing.Point(124, 262);
            this.Incc5IniFileLoc.Name = "Incc5IniFileLoc";
            this.Incc5IniFileLoc.Size = new System.Drawing.Size(180, 20);
            this.Incc5IniFileLoc.TabIndex = 52;
            this.toolTip1.SetToolTip(this.Incc5IniFileLoc, "Specify data file location for all output data files. Overrides the root location" +
        " and daily folder");
            // 
            // UseINCC5Ini
            // 
            this.UseINCC5Ini.AutoSize = true;
            this.UseINCC5Ini.Location = new System.Drawing.Point(12, 238);
            this.UseINCC5Ini.Name = "UseINCC5Ini";
            this.UseINCC5Ini.Size = new System.Drawing.Size(171, 17);
            this.UseINCC5Ini.TabIndex = 54;
            this.UseINCC5Ini.Text = "Use paths in INCC5 incc.ini file";
            this.toolTip1.SetToolTip(this.UseINCC5Ini, "Use the path entries in an INCC5 incc.ini file. Apparently this is a thing");
            this.UseINCC5Ini.UseVisualStyleBackColor = true;
            this.UseINCC5Ini.CheckedChanged += new System.EventHandler(this.UseINCC5Ini_CheckedChanged);
            // 
            // UserDocFolder
            // 
            this.UserDocFolder.AutoSize = true;
            this.UserDocFolder.Location = new System.Drawing.Point(406, 255);
            this.UserDocFolder.Name = "UserDocFolder";
            this.UserDocFolder.Size = new System.Drawing.Size(177, 17);
            this.UserDocFolder.TabIndex = 55;
            this.UserDocFolder.Text = "User doc folder for default paths";
            this.toolTip1.SetToolTip(this.UserDocFolder, "Enable to use user Documents folder, disable to use user AppData\\Local\\Temp folde" +
        "r ");
            this.UserDocFolder.UseVisualStyleBackColor = true;
            this.UserDocFolder.CheckedChanged += new System.EventHandler(this.UserDocFolder_CheckedChanged);
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
            this.FPPrec.Location = new System.Drawing.Point(310, 290);
            this.FPPrec.Name = "FPPrec";
            this.FPPrec.Size = new System.Drawing.Size(46, 21);
            this.FPPrec.TabIndex = 9;
            this.FPPrec.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 293);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Precison of floating point results mass reporting output";
            // 
            // EnableAuxRatioReportingCheckBox
            // 
            this.EnableAuxRatioReportingCheckBox.AutoSize = true;
            this.EnableAuxRatioReportingCheckBox.Location = new System.Drawing.Point(406, 162);
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
            this.EnableSilentFolderCreationCheckBox.Location = new System.Drawing.Point(406, 139);
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
            this.label2.Location = new System.Drawing.Point(19, 319);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(188, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Status poll timer interval in milliseconds";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(289, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(182, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Status poll interval per MB processed";
            // 
            // logLoc
            // 
            this.logLoc.Location = new System.Drawing.Point(310, 52);
            this.logLoc.Name = "logLoc";
            this.logLoc.Size = new System.Drawing.Size(75, 23);
            this.logLoc.TabIndex = 42;
            this.logLoc.Text = "Select...";
            this.logLoc.UseVisualStyleBackColor = true;
            this.logLoc.Click += new System.EventHandler(this.logLoc_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Log file folder";
            // 
            // resultsLoc
            // 
            this.resultsLoc.Location = new System.Drawing.Point(310, 79);
            this.resultsLoc.Name = "resultsLoc";
            this.resultsLoc.Size = new System.Drawing.Size(75, 23);
            this.resultsLoc.TabIndex = 45;
            this.resultsLoc.Text = "Select...";
            this.resultsLoc.UseVisualStyleBackColor = true;
            this.resultsLoc.Click += new System.EventHandler(this.resultsLoc_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(87, 13);
            this.label5.TabIndex = 43;
            this.label5.Text = "Results file folder";
            // 
            // dataLoc
            // 
            this.dataLoc.Location = new System.Drawing.Point(310, 105);
            this.dataLoc.Name = "dataLoc";
            this.dataLoc.Size = new System.Drawing.Size(75, 23);
            this.dataLoc.TabIndex = 48;
            this.dataLoc.Text = "Select...";
            this.dataLoc.UseVisualStyleBackColor = true;
            this.dataLoc.Click += new System.EventHandler(this.dataLoc_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 46;
            this.label6.Text = "Data file folder";
            // 
            // incc5IniLoc
            // 
            this.incc5IniLoc.Location = new System.Drawing.Point(310, 259);
            this.incc5IniLoc.Name = "incc5IniLoc";
            this.incc5IniLoc.Size = new System.Drawing.Size(75, 23);
            this.incc5IniLoc.TabIndex = 53;
            this.incc5IniLoc.Text = "Select...";
            this.incc5IniLoc.UseVisualStyleBackColor = true;
            this.incc5IniLoc.Click += new System.EventHandler(this.incc5IniLoc_Click);
            // 
            // UseINCC5IniLocLabel
            // 
            this.UseINCC5IniLocLabel.AutoSize = true;
            this.UseINCC5IniLocLabel.Enabled = false;
            this.UseINCC5IniLocLabel.Location = new System.Drawing.Point(4, 264);
            this.UseINCC5IniLocLabel.Name = "UseINCC5IniLocLabel";
            this.UseINCC5IniLocLabel.Size = new System.Drawing.Size(103, 13);
            this.UseINCC5IniLocLabel.TabIndex = 51;
            this.UseINCC5IniLocLabel.Text = "INCC5 incc.ini folder";
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 338);
            this.Controls.Add(this.UserDocFolder);
            this.Controls.Add(this.UseINCC5Ini);
            this.Controls.Add(this.incc5IniLoc);
            this.Controls.Add(this.Incc5IniFileLoc);
            this.Controls.Add(this.UseINCC5IniLocLabel);
            this.Controls.Add(this.UseINCC5Suffix);
            this.Controls.Add(this.Use8Char);
            this.Controls.Add(this.dataLoc);
            this.Controls.Add(this.DataFileLoc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.resultsLoc);
            this.Controls.Add(this.ResultsFileLoc);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.logLoc);
            this.Controls.Add(this.LogFileLoc);
            this.Controls.Add(this.label4);
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
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
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
		private System.Windows.Forms.Button logLoc;
		private System.Windows.Forms.TextBox LogFileLoc;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button resultsLoc;
		private System.Windows.Forms.TextBox ResultsFileLoc;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button dataLoc;
		private System.Windows.Forms.TextBox DataFileLoc;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox Use8Char;
		private System.Windows.Forms.CheckBox UseINCC5Suffix;
		private System.Windows.Forms.Button incc5IniLoc;
		private System.Windows.Forms.TextBox Incc5IniFileLoc;
		private System.Windows.Forms.Label UseINCC5IniLocLabel;
		private System.Windows.Forms.CheckBox UseINCC5Ini;
        private System.Windows.Forms.CheckBox UserDocFolder;
    }
}