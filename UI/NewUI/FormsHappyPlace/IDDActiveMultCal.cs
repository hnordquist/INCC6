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
using System.IO;
using System.Windows.Forms;
using AnalysisDefs;

namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    public partial class IDDActiveMultCal : Form
    {
        INCCAnalysisParams.active_mult_rec amr = new INCCAnalysisParams.active_mult_rec();
        MethodParamFormFields mp;
        protected void FieldFiller(INCCAnalysisParams.active_mult_rec amr)
        {
            Thermal1stMomentTextBox.Text = amr.vt1.ToString("E6");
            Thermal2ndMomentTextBox.Text = amr.vt2.ToString("E6");
            Thermal3rdMomentTextBox.Text = amr.vt3.ToString("E6");
            Fast1stMomentTextBox.Text = amr.vf1.ToString("E6");
            Fast2ndMomentTextBox.Text = amr.vf2.ToString("E6");
            Fast3rdMomentTextBox.Text = amr.vf3.ToString("E6");
        }

        public IDDActiveMultCal()
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.ActiveMultiplicity);

            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;
            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);

        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.acq.item_type = (string)((ComboBox)sender).SelectedItem;
            mp.ams = Integ.GetMethodSelections(mp.acq.detector_id, mp.acq.item_type); // unfinished, test and firm up
            if (mp.ams != null)
            {
                if (mp.ams.GetMethodParameters(mp.am) != null)
                    mp.imd = new INCCAnalysisParams.active_mult_rec((INCCAnalysisParams.active_mult_rec)mp.ams.GetMethodParameters(mp.am));
                else
                    mp.imd = new INCCAnalysisParams.active_mult_rec();
            }
            else
            {
                mp.imd = new INCCAnalysisParams.active_mult_rec(); // not mapped, so make a new one
                mp.imd.modified = true;
            }
            amr = (INCCAnalysisParams.active_mult_rec)mp.imd;
            FieldFiller(amr);
        }

        private void Thermal1stMomentTextBox_Leave(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref amr.vt1);
        }

        private void Thermal2ndMomentTextBox_Leave(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref amr.vt2);
        }

        private void Thermal3rdMomentTextBox_Leave(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref amr.vt3);
        }

        private void Fast1stMomentTextBox_Leave(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref amr.vf1);
        }

        private void Fast2ndMomentTextBox_Leave(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref amr.vf2);
        }

        private void Fast3rdMomentTextBox_Leave(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref amr.vf3);
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null, 0, 0, 0);
            List<NCCReporter.Row> rows = new List<NCCReporter.Row>();
            rows = amr.ToLines(null);
            sec.AddRange(rows);

            string path = System.IO.Path.GetTempFileName();
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter s = new StreamWriter(f);
            s.AutoFlush = true;
            foreach (NCCReporter.Row r in rows)
                s.WriteLine(r.ToLine(' '));
            f.Close();
            PrintForm pf = new PrintForm(path, this.Text + DateTime.Now.ToString(" yyyy/MM/dd HH:mm:ss"));
            pf.ShowDialog();
            File.Delete(path);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            mp.Persist();
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }
    }
}
