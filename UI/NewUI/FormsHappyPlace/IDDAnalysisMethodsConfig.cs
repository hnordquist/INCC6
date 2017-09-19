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
                CollarAmLiCheckBox.Checked = am.choices[(int)AnalysisMethod.Collar] && (acq.collar_mode == (int)CollarType.AmLiThermal || acq.collar_mode == (int)CollarType.AmLiFast);
                CollarCfCheckBox.Checked = am.choices[(int)AnalysisMethod.Collar] && (acq.collar_mode == (int)CollarType.CfThermal || acq.collar_mode == (int)CollarType.CfFast);
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
            collaractive();
        }

        private void collaractive()
        {
            // Modified 5/4/2017 -- Looks for the Collar check, then sets the AcquireParms to default of thermal for source type selectd. HN 5/4/2017
            string CollarName;
            if (CollarAmLiCheckBox.Checked == true)
            {
                CollarName = CollarAmLiCheckBox.Name;
                foreach (Control cb in this.Controls)
                {
                    if (cb is CheckBox)
                    {
                        if (cb.Name != CollarAmLiCheckBox.Name)
                        {
                            cb.Enabled = false;
                        }
                    }
                }
                am.Normal = AnalysisMethod.Collar;
                am.Backup = AnalysisMethod.None;
                am.Auxiliary = AnalysisMethod.None;
                acq.collar_mode = (int)AnalysisDefs.CollarType.AmLiThermal;
                am.modified = true;
            }
            else if (CollarCfCheckBox.Checked == true)
            {
                CollarName = CollarCfCheckBox.Name;
                foreach (Control cb in this.Controls)
                {
                    if (cb is CheckBox)
                    {
                        if (cb.Name != CollarCfCheckBox.Name)
                        {
                            cb.Enabled = false;
                        }
                    }
                }
                am.Normal = AnalysisMethod.Collar;
                am.Backup = AnalysisMethod.None;
                am.Auxiliary = AnalysisMethod.None;
                acq.collar_mode = (int)AnalysisDefs.CollarType.CfThermal;
                acq.modified = true;
                am.modified = true;
            }
            else
            {
                //Check this. hn 1/26/17
                bool anyChecked = false;
                CollarName = CollarAmLiCheckBox.Name;
                foreach (Control cb in this.Controls)
                {
                    if (cb is CheckBox)
                    {
                        if (cb.Name != CollarAmLiCheckBox.Name && cb.Name != CollarCfCheckBox.Name)
                        {
                            ((CheckBox)cb).Enabled = true;
                            anyChecked |= ((CheckBox)cb).Checked;
                        }
                    }
                }
                this.CollarAmLiCheckBox.Enabled = !anyChecked;
                this.CollarCfCheckBox.Enabled = !anyChecked;
                if (am.AnySelected())
                {
                    am.choices[0] = false;
                    am.choices[10] = false;
                    am.Normal = am.GetFirstSelected();
                    am.Backup = am.GetSecondSelected();
                    am.Auxiliary = am.GetThirdSelected();
                }
            }

        }
        // collar disabled if any other enabled

        private void PassiveCalCurveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.CalibrationCurve] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void KnownAlphaCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.KnownA] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void KnownMCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.KnownM] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void PassiveMultiplicityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.Multiplicity] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void AddASourceCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.AddASource] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void CuRatioCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.CuriumRatio] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void TruncatedMultCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.TruncatedMultiplicity] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void ActiveCalCurveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.Active] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void CollarCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.Collar] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
                  
        }

        private void ActiveMultCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.ActiveMultiplicity] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void ActivePassiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.ActivePassive] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            // First, check if selections changed.
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
                    NC.App.DB.UpdateAnalysisMethodSpecifics(original.selector.detectorid, original.selector.material);
                    original.modified = false;
                }
                else if (am.modified) //  or created new one
                {
                    NC.App.DB.UpdateAnalysisMethods(original.selector, am);
                    NC.App.DB.UpdateAnalysisMethodSpecifics(original.selector.detectorid, original.selector.material);
                    original.modified = false;
                }
            }

            // first, select priorities
            if (am.AnySelected())
            {
                IDDSelectPrimaryAM x = new IDDSelectPrimaryAM(am);
                if (x.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AnalysisMethods original2;
                    bool found2 = NC.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(am.selector, out original2);
                    if (found2)
                    {
                        if (original2.selector == null) // empty initial value, copy the selector here
                            original2.selector = new INCCSelector(am.selector);
                        if (!am.Equals(original2)) // an existing has changed,
                        {
                            // copy updated changes back to original on the map
                            original2.CopySettings(am);
                            NC.App.DB.UpdateAnalysisMethods(original2.selector, am);
                            NC.App.DB.UpdateAnalysisMethodSpecifics(original2.selector.detectorid, original.selector.material);
                            original2.modified = false;
                        }
                        else if (am.modified) //  or created new one
                        {
                            NC.App.DB.UpdateAnalysisMethods(original2.selector, am);
                            NC.App.DB.UpdateAnalysisMethodSpecifics(original2.selector.detectorid, original.selector.material);
                            original2.modified = false;
                        }

                    }
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

        private void CollarCfCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            am.choices[(int)AnalysisMethod.Collar] = ((CheckBox)sender).Checked;
            choke((CheckBox)sender);
        }
    }
}
