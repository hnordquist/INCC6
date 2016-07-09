/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE.  If software is modified to produce derivative works, 
such modified software should be clearly marked, so as not to confuse it with the version available from LANL.

Additionally, redistribution and use in source and binary forms, with or without modification, are permitted provided 
that the following conditions are met:
•	Redistributions of source code must retain the above copyright notice, this list of conditions and the following 
disclaimer. 
•	Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following 
disclaimer in the documentation and/or other materials provided with the distribution. 
•	Neither the name of Los Alamos National Security, LLC, Los Alamos National Laboratory, LANL, the U.S. Government, 
nor the names of its contributors may be used to endorse or promote products derived from this software without specific 
prior written permission. 
THIS SOFTWARE IS PROVIDED BY LOS ALAMOS NATIONAL SECURITY, LLC AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED 
WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR 
PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL LOS ALAMOS NATIONAL SECURITY, LLC OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY 
THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using AnalysisDefs;
using NCCConfig;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace NewUI
{
    
    using NC = NCC.CentralizedState;
    public partial class Preferences : Form
    {
        Dictionary<NCCFlags, object> maybe;
        public Preferences()
        {

            maybe = new Dictionary<NCCFlags, object>();
            maybe.Add(NCCFlags.auxRatioReport, NC.App.AppContext.AuxRatioReport);
            maybe.Add(NCCFlags.autoCreateMissing, NC.App.AppContext.AutoCreateMissing);
            maybe.Add(NCCFlags.root, NC.App.AppContext.RootLoc);
            maybe.Add(NCCFlags.logFileLoc, NC.App.AppContext.LogFilePath);
            maybe.Add(NCCFlags.resultsFileLoc, NC.App.AppContext.ResultsFilePath);
            maybe.Add(NCCFlags.fileinput, NC.App.AppContext.FileInput);
            maybe.Add(NCCFlags.dailyRootPath, NC.App.AppContext.DailyRootPath);
            maybe.Add(NCCFlags.fpPrec, NC.App.AppContext.FPPrecision);
            maybe.Add(NCCFlags.INCCParity, NC.App.AppContext.INCCParity);
            maybe.Add(NCCFlags.replay, NC.App.AppContext.Replay);
            maybe.Add(NCCFlags.opStatusPktInterval, NC.App.AppContext.StatusPacketCount);
            maybe.Add(NCCFlags.opStatusTimeInterval, NC.App.AppContext.StatusTimerMilliseconds);
            maybe.Add(NCCFlags.overwriteImportedDefs, NC.App.AppContext.OverwriteImportedDefs);
            maybe.Add(NCCFlags.openResults, NC.App.AppContext.OpenResults);
            maybe.Add(NCCFlags.gen5TestDataFile, NC.App.AppContext.CreateINCC5TestDataFile);    
            maybe.Add(NCCFlags.results8Char, NC.App.AppContext.Results8Char);    
            maybe.Add(NCCFlags.assayTypeSuffix, NC.App.AppContext.AssayTypeSuffix);    
            maybe.Add(NCCFlags.serveremulation, NC.App.AppContext.UseINCC5Ini);    
            maybe.Add(NCCFlags.emulatorapp, NC.App.AppContext.INCC5IniLoc);    

            InitializeComponent();

            WorkingDirTextBox.Tag = root.Tag = NCCFlags.root;
            LogFileLoc.Tag = logLoc.Tag = NCCFlags.logFileLoc;
            ResultsFileLoc.Tag = resultsLoc.Tag = NCCFlags.resultsFileLoc;
            DataFileLoc.Tag = dataLoc.Tag = NCCFlags.fileinput;
            DailyF0lder.Tag = NCCFlags.dailyRootPath;
            IsoFractionalDay.Tag = NCCFlags.INCCParity;
			RevFileGen.Tag = NCCFlags.gen5TestDataFile;
            PollTimer.Tag = NCCFlags.opStatusTimeInterval;
            PollPacket.Tag = NCCFlags.opStatusPktInterval;
            Replay.Tag = NCCFlags.replay;
            FPPrec.Tag = NCCFlags.fpPrec;
            EnableAuxRatioReportingCheckBox.Tag = NCCFlags.auxRatioReport;
            EnableSilentFolderCreationCheckBox.Tag = NCCFlags.autoCreateMissing;
            OverwriteImportedDefs.Tag = NCCFlags.overwriteImportedDefs;
            AutoOpenCheckBox.Tag = NCCFlags.openResults;
			Use8Char.Tag = NCCFlags.results8Char;
			UseINCC5Suffix.Tag = NCCFlags.assayTypeSuffix;
			UseINCC5Ini.Tag = NCCFlags.serveremulation;
			Incc5IniFileLoc.Tag = NCCFlags.emulatorapp;

            DailyF0lder.Checked = NC.App.AppContext.DailyRootPath;
            WorkingDirTextBox.Text = NC.App.AppContext.RootLoc;
            LogFileLoc.Text = NC.App.AppContext.LogFilePath;
            ResultsFileLoc.Text = NC.App.AppContext.ResultsFilePath;
            DataFileLoc.Text = NC.App.AppContext.FileInput;
            FPPrec.SelectedItem = NC.App.AppContext.FPPrecision.ToString();
            EnableAuxRatioReportingCheckBox.Checked = NC.App.AppContext.AuxRatioReport;
            EnableSilentFolderCreationCheckBox.Checked = NC.App.AppContext.AutoCreateMissing;
            OverwriteImportedDefs.Checked = NC.App.AppContext.OverwriteImportedDefs;
            AutoOpenCheckBox.Checked = NC.App.AppContext.OpenResults;
            IsoFractionalDay.Checked = NC.App.AppContext.INCCParity;
            RevFileGen.Checked = NC.App.AppContext.CreateINCC5TestDataFile;
            Replay.Checked = NC.App.AppContext.Replay;
            PollPacket.Text = NC.App.AppContext.StatusPacketCount.ToString();
            PollTimer.Text = NC.App.AppContext.StatusTimerMilliseconds.ToString();
			Use8Char.Checked = NC.App.AppContext.Results8Char;
			UseINCC5Suffix.Checked = NC.App.AppContext.AssayTypeSuffix;
			Use8Char.Text = "Use INCC5 results file naming (" + AnalysisDefs.MethodResultsReport.EightCharConvert(DateTimeOffset.Now) + ")";
			UseINCC5Ini.Checked = NC.App.AppContext.UseINCC5Ini;
			Incc5IniFileLoc.Text = NC.App.AppContext.INCC5IniLoc;

			// hide these if the relevant flag is not set
			UseINCC5IniLocLabel.Visible =  NC.App.AppContext.UseINCC5Ini;
			Incc5IniFileLoc.Visible =  NC.App.AppContext.UseINCC5Ini;
			incc5IniLoc.Visible = NC.App.AppContext.UseINCC5Ini;

            bool lmmm = false;
            Detector det = NCC.IntegrationHelpers.GetCurrentAcquireDetector();
            lmmm = (det.Id.SRType == DetectorDefs.InstrType.LMMM);
            PollPacket.Visible = lmmm;
            PollTimer.Visible = lmmm;
            label2.Visible = lmmm;
            label3.Visible = lmmm;
        }
        private void OKBtn_Click(object sender, EventArgs e)
        {
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.auxRatioReport] != NC.App.AppContext.AuxRatioReport);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.autoCreateMissing] !=NC.App.AppContext.AutoCreateMissing);
            NC.App.AppContext.modified |= ((string)maybe[NCCFlags.root] !=NC.App.AppContext.RootLoc);
            NC.App.AppContext.modified |= ((string)maybe[NCCFlags.logFileLoc] !=NC.App.AppContext.LogFilePath);
            NC.App.AppContext.modified |= ((string)maybe[NCCFlags.resultsFileLoc] !=NC.App.AppContext.ResultsFilePath);
            NC.App.AppContext.modified |= ((string)maybe[NCCFlags.fileinput] !=NC.App.AppContext.FileInput);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.dailyRootPath] !=NC.App.AppContext.DailyRootPath);
            NC.App.AppContext.modified |= ((UInt16)maybe[NCCFlags.fpPrec] !=NC.App.AppContext.FPPrecision);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.INCCParity] !=NC.App.AppContext.INCCParity);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.replay] !=NC.App.AppContext.Replay);
            NC.App.AppContext.modified |= ((UInt32)maybe[NCCFlags.opStatusPktInterval] != NC.App.AppContext.StatusPacketCount);
            NC.App.AppContext.modified |= ((UInt32)maybe[NCCFlags.opStatusTimeInterval] != NC.App.AppContext.StatusTimerMilliseconds);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.overwriteImportedDefs] != NC.App.AppContext.OverwriteImportedDefs);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.openResults] != NC.App.AppContext.OpenResults);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.gen5TestDataFile] != NC.App.AppContext.CreateINCC5TestDataFile);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.results8Char] != NC.App.AppContext.Results8Char);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.assayTypeSuffix] != NC.App.AppContext.AssayTypeSuffix);
            NC.App.AppContext.modified |= ((string)maybe[NCCFlags.emulatorapp] != NC.App.AppContext.INCC5IniLoc);
            NC.App.AppContext.modified |= ((bool)maybe[NCCFlags.serveremulation] != NC.App.AppContext.UseINCC5Ini);
            if (!NC.App.AppContext.modified)  // nothing 
            {
                Close();
                return;
            }
			// copy any changes back to the context
            NC.App.AppContext.AuxRatioReport = (bool)maybe[NCCFlags.auxRatioReport];
            NC.App.AppContext.AutoCreateMissing = (bool)maybe[NCCFlags.autoCreateMissing];
            NC.App.AppContext.RootPath = (string)maybe[NCCFlags.root];
            NC.App.AppContext.LogFilePath = (string)maybe[NCCFlags.logFileLoc];
            NC.App.AppContext.ResultsFilePath = (string)maybe[NCCFlags.resultsFileLoc];
            NC.App.AppContext.FileInput = (string)maybe[NCCFlags.fileinput];
            NC.App.AppContext.DailyRootPath = (bool)maybe[NCCFlags.dailyRootPath];
            NC.App.AppContext.FPPrecision = (UInt16)maybe[NCCFlags.fpPrec];
            NC.App.AppContext.INCCParity = (bool)maybe[NCCFlags.INCCParity];
            NC.App.AppContext.Replay = (bool)maybe[NCCFlags.replay];
            NC.App.AppContext.StatusPacketCount = (UInt32)maybe[NCCFlags.opStatusPktInterval];
            NC.App.AppContext.StatusTimerMilliseconds = (UInt32)maybe[NCCFlags.opStatusTimeInterval];
            NC.App.AppContext.OverwriteImportedDefs = (bool)maybe[NCCFlags.overwriteImportedDefs];
            NC.App.AppContext.OpenResults = (bool)maybe[NCCFlags.openResults];
            NC.App.AppContext.CreateINCC5TestDataFile = (bool)maybe[NCCFlags.gen5TestDataFile];
			NC.App.AppContext.Results8Char = (bool)maybe[NCCFlags.results8Char];
			NC.App.AppContext.AssayTypeSuffix = (bool)maybe[NCCFlags.assayTypeSuffix];
			NC.App.AppContext.INCC5IniLoc = (string)maybe[NCCFlags.emulatorapp];
            NC.App.AppContext.UseINCC5Ini = (bool)maybe[NCCFlags.serveremulation];
            NC.App.LMBD.UpdateLMINCCAppContext(); // out to the DB with you!
            Close();
        }

        // possible to append daily path to the daily path, so do not do that.
        private void root_Click(object sender, EventArgs e)
        {
            string str = UIIntegration.GetUsersFolder("Working folder (the current directory) location", (string)maybe[(NCCFlags)((Control)sender).Tag]);

            //Adding date to this path is sticking.  Need to just display the root location. hn 10-2
            //Checking for extra date presence to skip it, to keep the daily path override of the overall root path in play, to fix  hn 10-2 bug,

            if (!String.IsNullOrEmpty(str))
            {
                maybe[(NCCFlags)((Control)sender).Tag] = str;
                if ((bool)maybe[NCCFlags.dailyRootPath])
                {
                    string part = DateTime.Now.ToString("yyyy-MMdd");
                    if (!str.EndsWith(part))  // it's not the current day
                    {
                        Match m = Regex.Match(str, "\\d{4}-\\d{4}$");
                        if (m.Success)  // it is a pattern match
                        {
                            // so strip and replace with current date
                            string s = str.Remove(m.Index);
                            s = System.IO.Path.Combine(s, part);
                            WorkingDirTextBox.Text = s;
                        }
                        else 
                            WorkingDirTextBox.Text = System.IO.Path.Combine(str, part);
                    }
                    else
                        WorkingDirTextBox.Text = System.IO.Path.GetFullPath(str);
                }
                else
                    WorkingDirTextBox.Text = System.IO.Path.GetFullPath(str);
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ushort hoph = (ushort)maybe[(NCCFlags)((Control)sender).Tag];
            string s = (string)((ComboBox)sender).SelectedItem;
            Format.ToNN(s, ref hoph);
            maybe[(NCCFlags)((Control)sender).Tag] = hoph;
        }

        private void EnableSilentFolderCreationCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
        }

        private void EnableAuxRatioReportingCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
        }

        private void DailyF0lder_CheckedChanged(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
        }

        private void Replay_CheckedChanged(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
        }

        private void IsoFractionalDay_CheckedChanged(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
        }
        
        private void WorkingDirTextBox_Leave(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((TextBox)sender).Text;
        }

        private void OverwriteImportedDefs_CheckedChanged(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
        }

        private void AutoOpenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
        }

        private void PollTimer_Leave(object sender, EventArgs e)
        {
            UInt32 groovearmada = (UInt32)maybe[(NCCFlags)((Control)sender).Tag];
            string s = ((TextBox)sender).Text;
            Format.ToUInt(s, ref groovearmada);
            ((Control)sender).Text = groovearmada.ToString();
            maybe[(NCCFlags)((Control)sender).Tag] = groovearmada;
        }

        private void PollPacket_Leave(object sender, EventArgs e)
        {
            UInt32 zero7 = (UInt32)maybe[(NCCFlags)((Control)sender).Tag];
            string s = ((TextBox)sender).Text;
            Format.ToUInt(s, ref zero7);
            ((Control)sender).Text = zero7.ToString();
            maybe[(NCCFlags)((Control)sender).Tag] = zero7;
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RevFileGen_CheckedChanged(object sender, EventArgs e)
        {
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
        }

		private void logLoc_Click(object sender, EventArgs e)
		{
            string str = UIIntegration.GetUsersFolder("Log file folder location", (string)maybe[(NCCFlags)((Control)sender).Tag]);
            if (!String.IsNullOrEmpty(str))
            {
                maybe[(NCCFlags)((Control)sender).Tag] = str;
				LogFileLoc.Text = str;
				if (!string.IsNullOrEmpty(str))
					LogFileLoc.Text = System.IO.Path.GetFullPath(str);
            }

		}

		private void resultsLoc_Click(object sender, EventArgs e)
		{
            string str = UIIntegration.GetUsersFolder("Results file folder location", (string)maybe[(NCCFlags)((Control)sender).Tag]);
            if (!String.IsNullOrEmpty(str))
            {
                maybe[(NCCFlags)((Control)sender).Tag] = str;
				ResultsFileLoc.Text = str;
				if (!string.IsNullOrEmpty(str))
					ResultsFileLoc.Text = System.IO.Path.GetFullPath(str);
            }
		}

		private void dataLoc_Click(object sender, EventArgs e)
		{
            string str = UIIntegration.GetUsersFolder("Data file folder location", (string)maybe[(NCCFlags)((Control)sender).Tag]);
            if (!String.IsNullOrEmpty(str))
            {
                maybe[(NCCFlags)((Control)sender).Tag] = str;
				DataFileLoc.Text = str;
				if (!string.IsNullOrEmpty(str))
					DataFileLoc.Text = System.IO.Path.GetFullPath(str);
            }
		}

		private void LogFileLoc_Leave(object sender, EventArgs e)
		{
			string str = ((TextBox)sender).Text;
			if (0 != string.Compare(str, (string)maybe[NCCFlags.logFileLoc], true))
			{
				maybe[NCCFlags.logFileLoc] = str;
				LogFileLoc.Text = str;
				if (!string.IsNullOrEmpty(str))
					LogFileLoc.Text = System.IO.Path.GetFullPath(str);
			}
		}

		private void ResultsFileLoc_Leave(object sender, EventArgs e)
		{
			string str = ((TextBox)sender).Text;
			if (0 != string.Compare(str, (string)maybe[NCCFlags.resultsFileLoc], true))
			{
				maybe[NCCFlags.resultsFileLoc] = str;
				ResultsFileLoc.Text = str;
				if (!string.IsNullOrEmpty(str))
					ResultsFileLoc.Text = System.IO.Path.GetFullPath(str);
			}
		}

		private void DataFileLoc_Leave(object sender, EventArgs e)
		{
			string str = ((TextBox)sender).Text;
			if (0 != string.Compare(str, (string)maybe[NCCFlags.fileinput], true))
			{
				maybe[NCCFlags.fileinput] = str;
				DataFileLoc.Text = str;
				if (!string.IsNullOrEmpty(str))
					DataFileLoc.Text = System.IO.Path.GetFullPath(str);
			}
		}

		private void Use8Char_CheckedChanged(object sender, EventArgs e)
		{
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;

		}

		private void UseINCC5Suffix_CheckedChanged(object sender, EventArgs e)
		{
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
		}

		private void incc5IniLoc_Click(object sender, EventArgs e)
		{
			string str = UIIntegration.GetUsersFolder("Data file folder location", (string)maybe[(NCCFlags)((Control)sender).Tag]);
            if (!string.IsNullOrEmpty(str))
            {
                maybe[(NCCFlags)((Control)sender).Tag] = str;
				DataFileLoc.Text = str;
				if (!string.IsNullOrEmpty(str))
					incc5IniLoc.Text = System.IO.Path.GetFullPath(str);
            }
		}

		private void UseINCC5Ini_CheckedChanged(object sender, EventArgs e)
		{
            maybe[(NCCFlags)((Control)sender).Tag] = ((CheckBox)sender).Checked;
			UseINCC5IniLocLabel.Visible = (bool) maybe[(NCCFlags)((Control)sender).Tag];
			Incc5IniFileLoc.Visible = (bool) maybe[(NCCFlags)((Control)sender).Tag];
			incc5IniLoc.Visible = (bool) maybe[(NCCFlags)((Control)sender).Tag];
		}
	}
}
