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
using System;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using AnalysisDefs;

namespace NewUI
{
    //This whole file looks like it shouldn't work at all. hn 10.17.2017
    using NC = NCC.CentralizedState;
    
    public partial class IDDAcquireDBMeas : Form
    {
        AcquireHandlers ah;
        public MeasId measurementId;

        public IDDAcquireDBMeas(AcquireHandlers AH)
        {
            ah = AH;
            InitializeComponent();            
            LoadMeasurementsFromDB();
        }

        private void MeasurementDateTimePicker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            ListViewItem lvi = null;
            if (listView1.SelectedItems.Count > 0)
            { 
                lvi = listView1.SelectedItems[0];
                measurementId = ((Measurement)lvi.Tag).MeasurementId;
                DialogResult = DialogResult.OK;
            }
            else
                DialogResult = DialogResult.Ignore;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void LoadMeasurementsFromDB()
        {
            // get the list of measurement Ids (I did this two-step ingest to reduce memory use prior to an analysis: only the selected measurements are fully restored from the DB)
            //List<MeasId> list = NC.App.DB.MeasurementIds(ah.det.Id.DetectorName, ah.mo.PrintName());
            List<Measurement> mlist = NC.App.DB.MeasurementsFor(ah.det.Id.DetectorName);
            int measurecount = mlist.Count;
            foreach (Measurement m in mlist)
            {
                int CycleCount = NC.App.DB.GetCycleCount(m.MeasurementId);
                string fname = Path.GetFileName(m.MeasurementId.FileName);
                string ItemWithNumber = string.IsNullOrEmpty(m.AcquireState.item_id) ? "Empty" : m.AcquireState.item_id;
                if (fname.Contains("_"))
                //Lameness alert to display subsequent reanalysis number...... hn 9.21.2015
                    ItemWithNumber += "("+fname.Substring(fname.IndexOf('_')+1, 2)+")";
                ListViewItem lvi = new ListViewItem(new string[] {
                    m.MeasOption.PrintName(),
                    string.IsNullOrEmpty(m.AcquireState.ItemId.stratum) ? "Empty" : m.AcquireState.ItemId.stratum,
                    m.AcquireState.item_id,
                    m.MeasDate.ToString("yyMMdd HH:mm:ss"),
                    fname, CycleCount.ToString(), m.AcquireState.comment
                    });
                ListViewItem lvii = listView1.Items.Add(lvi);
                lvii.Tag = m;
				if (string.IsNullOrEmpty(fname))
					lvii.ToolTipText = "(" + m.MeasurementId.UniqueId.ToString() + ") No file name available";
				else
					lvii.ToolTipText = "(" + m.MeasurementId.UniqueId.ToString() + ") " + fname;
			}
            
            //Add also any rates only measurements. No reason they can't be used here.
            /*mlist = NC.App.DB.MeasurementIds(ah.det.Id.DetectorName, "Rates");
            NC.App.Logger(NCCReporter.LMLoggers.AppSection.App).
                TraceEvent(NCCReporter.LogLevels.Info, 87654,
                measurecount+list.Count + " " + ah.mo.PrintName() + " measurements available");
            foreach (MeasId m in list)
            {
                string ItemWithNumber = string.IsNullOrEmpty(m.Item.item) ? "Empty" : m.Item.item;
                if (Path.GetFileName(m.FileName).Contains("_"))
                    //Lameness alert to display subsequent reanalysis number...... hn 9.21.2015
                    ItemWithNumber += "(" + Path.GetFileName(m.FileName).Substring(Path.GetFileName(m.FileName).IndexOf('_') + 1, 2) + ")";
                ListViewItem lvi = new ListViewItem(new string[] {
                    ItemWithNumber,
                    string.IsNullOrEmpty(m.Item.stratum) ? "Empty" : m.Item.stratum,
                    m.MeasDateTime.ToString("yy.MM.dd"), m.MeasDateTime.ToString("HH:mm:ss") });
                ListViewItem lvii = listView1.Items.Add(lvi);
                lvii.Tag = m;
            }*/

        }

        public bool HasItems()
        {
            return listView1.Items.Count > 0;
        }

        public bool HasAMeasId()
        {
            return measurementId != null;
        }
    }
}
