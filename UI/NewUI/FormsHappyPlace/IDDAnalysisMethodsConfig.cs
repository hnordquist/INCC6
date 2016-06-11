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

namespace NewUI
{

    using Integ = NCC.IntegrationHelpers;
    using NC = NCC.CentralizedState;
    public partial class IDDAnalysisMethodsConfig : Form
    {
        Detector det;
        AcquireParameters acq = null;
        AnalysisMethods am = null;
        public IDDAnalysisMethodsConfig()
        {
            InitializeComponent();

            Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
            this.Text += " for " + det.Id.DetectorName;
            this.MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }
            MaterialTypeComboBox.SelectedItem = acq.item_type;
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            acq.item_type = (string)((ComboBox)sender).SelectedItem;
            INCCSelector sel = new INCCSelector(acq.detector_id, acq.item_type);
            AnalysisMethods lam;
            bool found = NC.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(sel, out lam);
            if (found)
            {
                am = new AnalysisMethods(sel);
                am.CopySettings(lam);
                ActivePassiveCheckBox.Checked = am.choices[(int)AnalysisMethod.ActivePassive];
                ActiveCalCurveCheckBox.Checked = am.choices[(int)AnalysisMethod.Active];
                ActiveMultCheckBox.Checked = am.choices[(int)AnalysisMethod.ActiveMultiplicity];
                CollarCheckBox.Checked = am.choices[(int)AnalysisMethod.Collar];
                PassiveCalCurveCheckBox.Checked = am.choices[(int)AnalysisMethod.CalibrationCurve];
                KnownAlphaCheckBox.Checked = am.choices[(int)AnalysisMethod.KnownA];
                KnownMCheckBox.Checked = am.choices[(int)AnalysisMethod.KnownM];
                PassiveMultiplicityCheckBox.Checked = am.choices[(int)AnalysisMethod.Multiplicity];
                AddASourceCheckBox.Checked = am.choices[(int)AnalysisMethod.AddASource];
                CuRatioCheckBox.Checked = am.choices[(int)AnalysisMethod.CuriumRatio];
                TruncatedMultCheckBox.Checked = am.choices[(int)AnalysisMethod.TruncatedMultiplicity];
                collaractive();
            }
            else
            {
                // create an AM marked None and insert in Map here, set modified flag
                am = new AnalysisMethods(sel);
                NC.App.DB.DetectorMaterialAnalysisMethods.Add(sel, am);
                am.modified = true;
            }
        }

        private void choke(CheckBox cb)
        {
            if (cb.Checked)
            {
                CollarCheckBox.Checked = false;
            }
            collaractive();
        }

        private void collaractive()
        {
            if (CollarCheckBox.Checked == true)
            {
                string CollarName = CollarCheckBox.Name;
                foreach (Control cb in this.Controls)
                {
                    if (cb is CheckBox)
                    {
                        if (cb.Name != CollarCheckBox.Name)
                        {
                            cb.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                bool allUnchecked = true;
                string CollarName = CollarCheckBox.Name;
                foreach (Control cb in this.Controls)
                {
                    if (cb is CheckBox)
                    {
                        if (cb.Name != CollarCheckBox.Name)
                        {
                            allUnchecked = ((CheckBox)cb).Checked == false;
                        }
                    }
                }
                this.CollarCheckBox.Enabled = allUnchecked;
            }
        }
        // collar disabled if any other enabled

        private void PassiveCalCurveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.CalibrationCurve] = ((CheckBox)sender).Checked;
        }

        private void KnownAlphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.KnownA] = ((CheckBox)sender).Checked;
        }

        private void KnownMCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.KnownM] = ((CheckBox)sender).Checked;
        }

        private void PassiveMultiplicityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.Multiplicity] = ((CheckBox)sender).Checked;
        }

        private void AddASourceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.AddASource] = ((CheckBox)sender).Checked;
        }

        private void CuRatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.CuriumRatio] = ((CheckBox)sender).Checked;
        }

        private void TruncatedMultCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.TruncatedMultiplicity] = ((CheckBox)sender).Checked;
        }

        private void ActiveCalCurveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.Active] = ((CheckBox)sender).Checked;
        }

        private void CollarCheckBox_CheckedChanged(object sender, EventArgs e)
        {
               ActivePassiveCheckBox.Enabled = !((CheckBox)sender).Checked;
                ActiveCalCurveCheckBox.Enabled = !((CheckBox)sender).Checked;
                ActiveMultCheckBox.Enabled = !((CheckBox)sender).Checked;
                PassiveCalCurveCheckBox.Enabled = !((CheckBox)sender).Checked;
                KnownAlphaCheckBox.Enabled = !((CheckBox)sender).Checked;
                KnownMCheckBox.Enabled = !((CheckBox)sender).Checked;
                PassiveMultiplicityCheckBox.Enabled = !((CheckBox)sender).Checked;
                AddASourceCheckBox.Enabled = !((CheckBox)sender).Checked;
                CuRatioCheckBox.Enabled = !((CheckBox)sender).Checked;
                TruncatedMultCheckBox.Enabled = !((CheckBox)sender).Checked;
                CollarCheckBox.Enabled = ((CheckBox)sender).Checked;
                am.choices.Initialize(); //  reset all the rest
                am.choices[(int)AnalysisMethod.Collar] = ((CheckBox)sender).Checked;  
        }

        private void ActiveMultCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.ActiveMultiplicity] = ((CheckBox)sender).Checked;
        }

        private void ActivePassiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            choke((CheckBox)sender);
            am.choices[(int)AnalysisMethod.ActivePassive] = ((CheckBox)sender).Checked;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            // first, select priorities
            IDDSelectPrimaryAM x = new IDDSelectPrimaryAM(am);
            if (x.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                AnalysisMethods original;
                bool found = NC.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(am.selector, out original);
                if (found)
                {
					if (original.selector == null) // empty initial value, copy the selector here
						original.selector = new INCCSelector(am.selector);
                    if (!am.Equals(original)) // an existing has changed,
                    {
                        // copy updated changes back to original on the map
                        original.CopySettings(am);
                        NC.App.DB.UpdateAnalysisMethods(original.selector, am);
                        NC.App.DB.UpdateAnalysisMethodSpecifics(original.selector.detectorid,original.selector.material);
                        original.modified = false;
                    }
                    else if (am.modified) //  or created new one
                    {
                        NC.App.DB.UpdateAnalysisMethods(original.selector, am);
                        NC.App.DB.UpdateAnalysisMethodSpecifics(original.selector.detectorid, original.selector.material);
                        original.modified = false;
                    }

                    this.Close();
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
    }
}
