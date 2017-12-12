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
using System.Windows.Forms;
using AnalysisDefs;
namespace UI
{
    using NC = NCC.CentralizedState;
    using Integ = NCC.IntegrationHelpers;
    using System.Collections.Generic;
    using System.IO;

    public partial class IDDTruncatedMultCalibration : Form
    {
        INCCAnalysisParams.truncated_mult_rec tm;
        MethodParamFormFields mp;
        public void FieldFiller()
        {
            ATextBox.Text = tm.a.ToString("E6");
            BTextBox.Text = tm.b.ToString("E6");
            if (tm.known_eff && !tm.solve_eff)
                KnownEfficiencyRadioButton.Checked = true;
            if (tm.solve_eff && !tm.known_eff)
                SolveForEfficiencyRadioButton.Checked = true;
            if (tm.solve_eff && tm.known_eff)
                BothEfficiencyMethodsRadioButton.Checked = true;
        }
        public IDDTruncatedMultCalibration()
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.TruncatedMultiplicity);
            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;
            this.MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }
            MaterialTypeComboBox.SelectedItem = mp.acq.item_type;
            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);
            FieldFiller();
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.SelectMaterialType((ComboBox)sender);
            mp.ams = Integ.GetMethodSelections(mp.acq.detector_id, mp.acq.item_type); // unfinished, test and firm up
            if (mp.HasMethod)
            {
                mp.imd = new INCCAnalysisParams.truncated_mult_rec((INCCAnalysisParams.truncated_mult_rec)mp.ams.GetMethodParameters(mp.am));
            }
            else
            {
                mp.imd = new INCCAnalysisParams.truncated_mult_rec(); // not mapped, so make a new one
                mp.imd.modified = true;
            }
            tm = (INCCAnalysisParams.truncated_mult_rec)mp.imd;

        }

        private void ATextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void BTextBox_TextChanged(object sender, EventArgs e)
        {
            mp.DblTextBox_Leave((TextBox)sender, ref tm.b);
        }

        private void KnownEfficiencyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (tm.known_eff != KnownEfficiencyRadioButton.Checked)
            {
                tm.known_eff = KnownEfficiencyRadioButton.Checked;
                tm.modified = true;
            }
        }

        private void SolveForEfficiencyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (tm.solve_eff != SolveForEfficiencyRadioButton.Checked)
            {
                tm.solve_eff = SolveForEfficiencyRadioButton.Checked;
                tm.modified = true;
            }
        }

        private void BothEfficiencyMethodsRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (!(tm.solve_eff && tm.known_eff) && ((RadioButton)sender).Checked)
            {
                tm.known_eff = tm.solve_eff = true;
                tm.modified = true;
            }
        }

        private void PrintCalParametersButton_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null, 0, 0, 0);
            List<NCCReporter.Row> rows = new List<NCCReporter.Row>();
            rows = tm.ToLines(null);
            sec.AddRange(rows);

            string path = System.IO.Path.GetTempFileName();
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter s = new StreamWriter(f);
            s.AutoFlush = true;
            foreach (NCCReporter.Row r in rows)
                s.WriteLine(r.ToLine(' '));
            f.Close();
            PrintForm pf = new PrintForm(path, this.Text);
            pf.ShowDialog();
            File.Delete(path);
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            mp.Persist();
            this.Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {

        }
    }
}
