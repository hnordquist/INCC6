/*
Copyright (c) 2014, Los Alamos National Security, LLC
All rights reserved.
Copyright 2014. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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
using System.Linq;
using System.Windows.Forms;
using AnalysisDefs;
namespace NewUI
{
    using NC = NCC.CentralizedState;

    public partial class IDDStratumLimits : Form
    {
        //MethodParamFormFields mp;
        List<INCCDB.StratumDescriptor> sl;

        public IDDStratumLimits()
        {
            InitializeComponent();
            sl = NC.App.DB.StrataList();
            StrataView.Rows.Clear();

            foreach (var stratum in sl)
            {
                string [] row = new string [4]{stratum.Desc.Name,stratum.Stratum.bias_uncertainty.ToString("F4"), stratum.Stratum.random_uncertainty.ToString("F4"), stratum.Stratum.systematic_uncertainty.ToString("F4") };
                StrataView.Rows.Add(row);
            }
            this.Refresh();
            if (StrataView.Rows.Count <= 0)
                MessageBox.Show("There are no strata defined in the database.", "WARNING");
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            foreach (var stratum in sl)
            {
                if (stratum.Stratum.modified)
                {
                    AcquireParameters acq = NC.App.DB.LastAcquire();
                    string curdet = acq.detector_id;
                    NC.App.DB.UpdateStratum(stratum.Desc, stratum.Stratum);
                    NC.App.DB.AssociateStratum(NC.App.DB.Detectors.Find(d => string.Compare(d.Id.DetectorName, curdet, true) == 0), stratum.Desc, stratum.Stratum); // associates it with the detector
                }
            }
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void StrataView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;

            if (row >= 0)
            {
                if (row >= sl.Count) // Is a new stratum
                {
                    INCCDB.Descriptor candidate = new INCCDB.Descriptor(StrataView[0, row].Value.ToString(), StrataView[0, row].Value.ToString());
                    Stratum st = new Stratum();
                    INCCDB.StratumDescriptor newst = new INCCDB.StratumDescriptor(candidate, st);
                    AcquireParameters acq = NC.App.DB.LastAcquire();
                    string curdet = acq.detector_id;
                    if (!String.IsNullOrEmpty(StrataView[0, row].Value.ToString()))
                    {
                        NC.App.DB.StrataList().Add(newst);
                        NC.App.DB.AssociateStratum(NC.App.DB.Detectors.Find(di => string.Compare(di.Id.DetectorName, curdet, true) == 0), candidate, st);
                    }
                    sl.Add(newst);
                }
                else
                {
                    INCCDB.StratumDescriptor changed = sl.ElementAt(row);
                    double d;
                    switch (col)
                    {
                        case 1:
                            Double.TryParse((StrataView[col, row]).Value.ToString(), out d);
                            changed.Stratum.bias_uncertainty = d;
                            changed.Stratum.modified = true; 
                            StrataView[col,row].Value = d.ToString("F4");
                            break;
                        case 2:
                            Double.TryParse((StrataView[col, row]).Value.ToString(), out d);
                            changed.Stratum.random_uncertainty = d;
                            changed.Stratum.modified = true;
                            StrataView[col, row].Value = d.ToString("F4");
                            break;
                        case 3:
                            Double.TryParse((StrataView[col, row]).Value.ToString(), out d);
                            changed.Stratum.systematic_uncertainty = d;
                            StrataView[col, row].Value = d.ToString("F4");
                            changed.Stratum.modified = true;
                            break;
                    }
                }
            }
        }
    }
}
