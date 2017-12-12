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
namespace UI
{

    using Integ = NCC.IntegrationHelpers;
    using NC = NCC.CentralizedState;
    public partial class IDDKnownMCal : Form
    {
        INCCAnalysisParams.known_m_rec kmr;
        MethodParamFormFields mp;

        protected void FieldFiller()
        {
            SponFissRateTextBox.Text = kmr.sf_rate.ToString("E6");
            SponFiss1stMomentTextBox.Text = kmr.vs1.ToString("E6");
            SponFiss2ndMomentTextBox.Text = kmr.vs2.ToString("E6");
            IndFiss1stMomentTextBox.Text = kmr.vi1.ToString("E6");
            IndFiss2ndMomentTextBox.Text = kmr.vi2.ToString("E6");
            LowerPuLimitTextBox.Text = kmr.lower_mass_limit.ToString("F6");
            UpperPuLimitTextBox.Text = kmr.upper_mass_limit.ToString("F6");
            BTextBox.Text = kmr.b.ToString("E6");
            CTextBox.Text = kmr.c.ToString("E6");
            SigmaXTextBox.Text = kmr.sigma_x.ToString("E6");
        }

        public IDDKnownMCal()
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.KnownM);
            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            Text += " for " + mp.det.Id.DetectorName;
            MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }
            MaterialTypeComboBox.SelectedItem = mp.acq.item_type;
        }
        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.acq.item_type = (string)((ComboBox)sender).SelectedItem;
            mp.ams = Integ.GetMethodSelections(mp.acq.detector_id, mp.acq.item_type); // unfinished, test and firm up
            if (mp.ams != null)
            {
                mp.imd = new INCCAnalysisParams.known_m_rec((INCCAnalysisParams.known_m_rec)mp.ams.GetMethodParameters(mp.am));
            }
            else
            {
                mp.imd = new INCCAnalysisParams.known_m_rec(); // not mapped, so make a new one
                mp.imd.modified = true;
            }
            kmr = (INCCAnalysisParams.known_m_rec)mp.imd;
            FieldFiller();
        }

        private void SponFissRateTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.sf_rate);
        }

        private void SponFiss1stMomentTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.vs1);
        }

        private void SponFiss2ndMomentTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.vs2);
        }

        private void IndFiss1stMomentTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.vi1);
        }

        private void IndFiss2ndMomentTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.vi2);
        }

        private void LowerPuLimitTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.lower_mass_limit);
        }

        private void UpperPuLimitTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.upper_mass_limit);
        }

        private void BTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.b);
        }

        private void CTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.c);
        }

        private void SigmaXTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave(((TextBox)sender), ref kmr.sigma_x);
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null, 0, 0, 0);
            List<NCCReporter.Row> rows = new List<NCCReporter.Row>();
            rows = kmr.ToLines(null);
            sec.AddRange(rows);

            string path = Path.GetTempFileName();
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter s = new StreamWriter(f);
            s.AutoFlush = true;
            foreach (NCCReporter.Row r in rows)
                s.WriteLine(r.ToLine(' '));
            f.Close();
            PrintForm pf = new PrintForm(path, Text);
            pf.ShowDialog();
            File.Delete(path);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            mp.Persist();
			Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
        }
    }
}
