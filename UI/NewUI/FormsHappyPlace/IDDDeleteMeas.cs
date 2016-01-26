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
using System.Collections.Generic;
using System.Windows.Forms;
using AnalysisDefs;
namespace NewUI
{
    public partial class IDDDeleteMeas : Form
    {
        ResultsRecs results;
        Dictionary<string, List<INCCResults.results_rec>> recs;

        public IDDDeleteMeas()
        {
            InitializeComponent();
            LoadMeasurementList();
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            DB.Measurements meas = new DB.Measurements ();
            bool success = true;
            foreach (ListViewItem lvi in MeasurementView.Items)
            {
                if (lvi.Selected)
                {
                    DateTime dt;
                    DateTime.TryParseExact (lvi.SubItems[4].Text + lvi.SubItems[5].Text,"yyyy/MM/ddHH:mm:ss",null,System.Globalization.DateTimeStyles.None, out dt);
                    success = meas.Delete((long)lvi.Tag) || success;
                    LoadMeasurementList();
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }
        private void LoadMeasurementList()
        {
            MeasurementView.Items.Clear();
            results = new ResultsRecs();
            recs = results.GetMap();
            foreach (var key in recs.Keys)
            {
                List<INCCResults.results_rec> list = new List<INCCResults.results_rec>();
                if (recs.TryGetValue(key, out list))
                    foreach (INCCResults.results_rec rr in list)
                    {
                        ListViewItem lvi = new ListViewItem(new string[6] { rr.acq.detector_id.ToString(), rr.meas_option.PrintName(), rr.acq.item_id.ToString(),
                            rr.acq.stratum_id.ToString() == String.Empty ? "Default" : rr.acq.stratum_id.ToString(), rr.acq.MeasDateTime.ToString("yyyy/MM/dd"), rr.acq.MeasDateTime.ToString("HH:mm:ss") });
                        lvi.Name = rr.acq.MeasDateTime.ToString();
                        MeasurementView.Items.Add(lvi);
						lvi.Tag = rr.MeasId;
                    }
            }
            this.Refresh();
            if (MeasurementView.Items.Count <= 0)
                MessageBox.Show("There are no measurements in the database.", "WARNING");
        }
    }
}
