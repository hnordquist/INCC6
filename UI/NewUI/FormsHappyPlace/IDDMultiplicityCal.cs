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
    public partial class IDDMultiplicityCal : Form
    {
        MethodParamFormFields mp;
        INCCAnalysisParams.multiplicity_rec mult;
        public IDDMultiplicityCal()
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.Multiplicity);

            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;

            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);
            //mp.RefreshCurveEqComboBox(CurveTypeComboBox);
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null, 0, 0, 0);
            List<NCCReporter.Row> rows = new List<NCCReporter.Row>();
            rows = mult.ToLines(null);
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

        public void RadioRadio()
        {
            switch (mult.solve_efficiency)
            {
                case INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT:
                    ConventionalMultRadioButton.Checked = true;
                    break;
                case INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT_WEIGHTED:
                    WeightedMultRadioButton.Checked = true;
                    break;
                case INCCAnalysisParams.MultChoice.MULT_DUAL_ENERGY_MODEL:
                    DualEnergyModelRadioButton.Checked = true;
                    break;
                case INCCAnalysisParams.MultChoice.MULT_KNOWN_ALPHA:
                    KnownAlphaRadioButton.Checked = true;
                    break;
                case INCCAnalysisParams.MultChoice.MULT_SOLVE_EFFICIENCY:
                    SolveForEfficiencyRadioButton.Checked = true;
                    break;
            }

        }

        public void FieldValorizer(TextBox t, ref double tgt)
        {
            double d = tgt;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified) { tgt = d;  mp.imd.modified = true; }
            t.Text = tgt.ToString("E6");
        }

        public void FieldFiller()
        {
            SpontaneousFissionRateTextBox.Text = mult.sf_rate.ToString("E6");
            SponFiss1stMomentTextBox.Text = mult.vs1.ToString("E6");
            SponFiss2ndMomentTextBox.Text = mult.vs2.ToString("E6");
            SponFiss3rdMomentTextBox.Text = mult.vs3.ToString("E6");
            IndFiss1stMomentTextBox.Text = mult.vi1.ToString("E6");
            IndFiss2ndMomentTextBox.Text = mult.vi2.ToString("E6");
            IndFiss3rdMomentTextBox.Text = mult.vi3.ToString("E6");
            ATextBox.Text = mult.a.ToString("E6");
            BTextBox.Text = mult.b.ToString("E6");
            CTextBox.Text = mult.c.ToString("E6");

            AlphaWeightTextBox.Text = mult.alpha_weight.ToString("E6");
            EfficiencyCorrectionFactorTextBox.Text = mult.multEffCorFactor.ToString("E6");
            SigmaXTextBox.Text = mult.sigma_x.ToString("E6");
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            mp.SelectMaterialType((ComboBox)sender);
            if (mp.HasMethod)
            {
                mp.imd = new INCCAnalysisParams.multiplicity_rec((INCCAnalysisParams.multiplicity_rec)mp.ams.GetMethodParameters(mp.am));
            }
            else
            {
                mp.imd = new INCCAnalysisParams.multiplicity_rec(); // not mapped, so make a new one
                mp.imd.modified = true;
            }
            mult = (INCCAnalysisParams.multiplicity_rec)mp.imd;
            FieldFiller();
            RadioRadio();
        }

        private void SpontaneousFissionRateTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer(((TextBox)sender), ref mult.sf_rate);
        }

        private void SponFiss1stMomentTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.vs1);
        }

        private void SponFiss2ndMomentTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.vs2);
        }

        private void SponFiss3rdMomentTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.vs3);
        }

        private void IndFiss1stMomentTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.vi1);
        }

        private void IndFiss2ndMomentTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.vi2);
        }

        private void IndFiss3rdMomentTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.vi3);
        }

        private void AlphaWeightTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.alpha_weight);
        }

        private void EfficiencyCorrectionFactorTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.multEffCorFactor);
        }

        private void ATextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.a);
        }

        private void BTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.b);
        }

        private void CTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.c);
        }

        private void SigmaXTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref mult.sigma_x);
        }

        private void ARadioButton_CheckedChanged(object sender, EventArgs e)
        {
			if (ConventionalMultRadioButton.Checked)
				mult.solve_efficiency = INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT;
			else if (WeightedMultRadioButton.Checked)
				mult.solve_efficiency = INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT_WEIGHTED;
			else if (DualEnergyModelRadioButton.Checked)
				mult.solve_efficiency = INCCAnalysisParams.MultChoice.MULT_DUAL_ENERGY_MODEL;
			else if (KnownAlphaRadioButton.Checked)
				mult.solve_efficiency = INCCAnalysisParams.MultChoice.MULT_KNOWN_ALPHA;
			else if (SolveForEfficiencyRadioButton.Checked)
				mult.solve_efficiency = INCCAnalysisParams.MultChoice.MULT_SOLVE_EFFICIENCY;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            // radio change check
            if (!mp.imd.modified)
            {
                INCCAnalysisParams.multiplicity_rec chump = (INCCAnalysisParams.multiplicity_rec)mp.ams.GetMethodParameters(mp.am);
                mp.imd.modified = (chump != null && chump.solve_efficiency != mult.solve_efficiency);
            }
            switch (mult.solve_efficiency)
            {
                case INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT:
                    break;
                case INCCAnalysisParams.MultChoice.CONVENTIONAL_MULT_WEIGHTED:
                    break;
                case INCCAnalysisParams.MultChoice.MULT_DUAL_ENERGY_MODEL:
					new IDDDualEnergyMult().ShowDialog();
                    break;
                case INCCAnalysisParams.MultChoice.MULT_KNOWN_ALPHA:
                    break;
                case INCCAnalysisParams.MultChoice.MULT_SOLVE_EFFICIENCY:
                    break;
            }
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
