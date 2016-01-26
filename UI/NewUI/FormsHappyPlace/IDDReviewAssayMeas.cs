﻿/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.  
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE. If software is modified to produce derivative works, 
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
using NCCReporter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    using NC = NCC.CentralizedState;
    public partial class IDDReviewAssayMeas : Form
    {
        private List<Measurement> mlist;
        protected LMLoggers.LognLM ctrllog;

        public IDDReviewAssayMeas(string filter = "Verification")
        {
            InitializeComponent();
            this.Text = "Verification Measurements";
            Detector det = Integ.GetCurrentAcquireDetector();
            mlist = NC.App.DB.MeasurementsFor(det,filter);
            mlist.RemoveAll(EmptyFile);
            ctrllog = NC.App.Loggers.Logger(LMLoggers.AppSection.Control);
            foreach (Measurement m in mlist)
            {
                ListViewItem lvi = new ListViewItem(new string[] { String.IsNullOrEmpty(m.MeasurementId.Item.item) ? "Empty" : m.MeasurementId.Item.item, String.IsNullOrEmpty(m.AcquireState.stratum_id.Name) ? "Empty" : m.AcquireState.stratum_id.Name, m.MeasOption.ToString(), m.MeasDate.DateTime.ToString("MM.dd.yy"), m.MeasDate.DateTime.ToString("HH:mm:ss") });
                listView1.Items.Add(lvi);
            }
            if (mlist.Count == 0)
            {
                MessageBox.Show ("There are no Verification Measurements found in the database.","WARNING");
                this.DialogResult = DialogResult.Abort;
            }
        }

        public static bool EmptyFile(Measurement m)
        {
            return m.ResultsFiles.Count <= 0 || String.IsNullOrEmpty(m.ResultsFiles.PrimaryINCC5Filename.Path);
        }
        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            ShowResults();
        }
        private void ShowResults()
        {
            string notepadPath = System.IO.Path.Combine(Environment.SystemDirectory, "notepad.exe");
            foreach (ListViewItem lvi in listView1.Items)
            {
                if (lvi.Selected)
                {
                    if (System.IO.File.Exists(notepadPath))
                    {
                        if (mlist[lvi.Index].ResultsFiles.Count > 0 && !String.IsNullOrEmpty(mlist[lvi.Index].ResultsFiles.PrimaryINCC5Filename.Path))
                            System.Diagnostics.Process.Start(notepadPath, mlist[lvi.Index].ResultsFiles.PrimaryINCC5Filename.Path);
                        else
                            ctrllog.TraceEvent(LogLevels.Error, 22222, "The file path did not exist or the filename was blank.");
                    }
                    lvi.Selected = false;
                }
            }
        }
        private void PrintBtn_Click(object sender, EventArgs e)
        {
            string path = System.IO.Path.GetTempFileName();
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter s = new StreamWriter(f);
            s.AutoFlush = true;
            s.Write(String.Format("{0,20}\t\t{1,20}\t\t{2,20}\t\t{3,20}\t\t{4,20}\r\n", "Item id", "Stratum id", "Measurement Type", "Measurement Date", "Measurement Time"));
            foreach (ListViewItem lvi in listView1.Items)
            {
                s.Write(String.Format("{0,20}\t\t{1,20}\t\t{2,20}\t\t{3,20}\t\t{4,20}\r\n", lvi.SubItems[0].Text, lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text, lvi.SubItems[4].Text));
            }
            f.Close();
            PrintForm pf = new PrintForm(path, this.Text, "Print List");
            pf.ShowDialog();
            File.Delete(path);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            ShowResults();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm"/*, HelpNavigator.Topic, "/WordDocuments/selectpu240ecoefficients.htm"*/);
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowResults();
        }

    }
}
