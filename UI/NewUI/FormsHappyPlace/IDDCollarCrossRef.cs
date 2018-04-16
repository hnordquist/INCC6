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
using System.Collections.Generic;
using System.IO;

namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    
    public partial class IDDCollarCrossRef : Form
    {
        MethodParamFormFields mp;
        INCCAnalysisParams.collar_combined_rec col;
        List<poison_rod_type_rec> poison;
        bool modified;

        public IDDCollarCrossRef(int mode, bool mod, INCCAnalysisParams.collar_combined_rec c = null)
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.CollarAmLi);

            RelativeDoublesRateTextBox.ToValidate = NumericTextBox.ValidateType.Float;
            RelativeDoublesRateTextBox.NumberFormat = NumericTextBox.Formatter.F3;

            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;

            modified = mod;
            mp.RefreshMatTypeComboBox(MaterialTypeComboBox);
			mp.SelectMaterialType(MaterialTypeComboBox);
            if (mp.HasMethod && c == null)
            {
                mp.imd = new INCCAnalysisParams.collar_combined_rec((INCCAnalysisParams.collar_combined_rec)mp.ams.GetMethodParameters(mp.am));
                col = (INCCAnalysisParams.collar_combined_rec)mp.imd;
                if (mode != -1)
                {
                    col.collar.collar_mode = mode;
                    col.modified = true;
                    col.collar_det.collar_mode = mode;
                    col.collar_det.modified = true;
                    col.k5.k5_mode = mode;
                    col.k5.modified = true;
                }
            }
            else if (mp.HasMethod && c != null)
            {
                col = c;
            }
            else
            {
                mp.imd = new INCCAnalysisParams.collar_combined_rec(mp.acq.collar_mode); // not mapped, so make a new one
                col = (INCCAnalysisParams.collar_combined_rec)mp.imd;
                modified = true;
            }

            CrossReferenceFieldFiller(col);
            this.TopMost = true;
        }

        public void CrossReferenceFieldFiller(INCCAnalysisParams.collar_combined_rec col)
        {
            MaterialTypeComboBox.SelectedIndex = MaterialTypeComboBox.FindStringExact(mp.acq.item_type)>=0?MaterialTypeComboBox.FindStringExact(mp.acq.item_type):0;
            ModeComboBox.SelectedIndex = col.collar_det.collar_mode;
            RelativeDoublesRateTextBox.Value = col.collar_det.relative_doubles_rate;
            ReferenceDateTimePicker.Value = col.collar_det.reference_date;
            poison = NCC.CentralizedState.App.DB.PoisonRods.GetList();
            SetHelp();
        }

        public void SetHelp ()
        {
            // Tell the HelpProvider what controls to provide help for, and
            // what the help string is.
            ToolTip t1  = new ToolTip();
            t1.IsBalloon = true;
            ToolTip t2 = new ToolTip();
            t2.IsBalloon = true;
            ToolTip t3 = new ToolTip();
            t3.IsBalloon = true;
            ToolTip t4 = new ToolTip();
            t4.IsBalloon = true;
            ToolTip t5 = new ToolTip();
            t5.IsBalloon = true;
            provider.SetShowHelp(this.MaterialTypeComboBox, true);
            String s1 = "Type of material to be measured. This determines which calibration\r\n" +
                        "and which analysis methods will be used to calculate results";
            provider.SetHelpString(this.MaterialTypeComboBox,s1);
            t1.SetToolTip (MaterialTypeComboBox,s1);

            s1 =        "Select the mode of operation for the collar. If the cadmium (Cd)\r\n" +
                        "liners are not used, the collar is in the thermal (no Cd) mode;\r\n" +
                        "otherwise it is in the fast (Cd) mode.";
            provider.SetShowHelp(this.ModeComboBox, true);
            provider.SetHelpString(this.ModeComboBox, s1);
            t2.SetToolTip(ModeComboBox, s1);

            provider.SetShowHelp (this.ReferenceDateTimePicker,true);
            s1 =        "This date is used to compensate for the decay of MRC-95 between the \r\n" +
                         "measurement date in this collar and the LANL reference collar. This date\r\n" +
                         "is October 17, 1989 for all the collars in LA-11965-MS.";
            provider.SetHelpString (this.ReferenceDateTimePicker, s1);
            t3.SetToolTip(ReferenceDateTimePicker, s1);

            provider.SetShowHelp (this.RelativeDoublesRateTextBox, true);
            s1 =        "Enter the doubles rate for this collar relative to that for the LANL reference\r\n" +
                        "collar (LANL-3 for PWR, LANL-4 for BWR) for the same AmLi source and \r\n" +
                        "fuel assembly.This is the k2 factor in the LA-11965-MS, Tables XII-XV.";
            provider.SetHelpString (this.RelativeDoublesRateTextBox,s1);
            t5.SetToolTip(RelativeDoublesRateTextBox, s1);
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mp.acq.item_type != MaterialTypeComboBox.Text)
            {
                mp.acq.item_type = MaterialTypeComboBox.Text;
                modified = true;
				mp.SelectMaterialType((ComboBox)sender);
            }
        }

        private void ModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (col.collar.collar_mode != ModeComboBox.SelectedIndex)
            {
                // copied three times as in INCC5, keeping it the same
                col.collar.collar_mode = ModeComboBox.SelectedIndex;
                col.collar_det.collar_mode = col.collar.collar_mode;
                col.k5.k5_mode = col.collar.collar_mode;
                modified = true;
            }
        }

        private void ReferenceDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!col.collar_det.reference_date.Equals(((DateTimePicker)sender).Value.Date))
            {
                col.collar_det.reference_date = ReferenceDateTimePicker.Value.Date;
                modified = true;
            }
        }

        private void RelativeDoublesRateTextBox_TextChanged(object sender, EventArgs e)
        {
            if (col.collar_det.relative_doubles_rate != RelativeDoublesRateTextBox.Value)
            {
                col.collar_det.relative_doubles_rate = RelativeDoublesRateTextBox.Value;
                modified = true;
            }
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null, 0, 0, 0);
            List<string> rows = new List<string>();
            rows.Add ("Collar Cross Reference Factors for:");
            rows.Add("Material Type " + MaterialTypeComboBox.Text);
            rows.Add("Mode " + ModeComboBox.Text);
            rows.Add("Detector " + mp.det.Id.DetectorName);
            rows.Add("Current Date " + DateTime.Now.ToShortDateString() + ' ' + DateTime.Now.ToShortTimeString());
            rows.Add("");
            rows.Add("Reference Date " + ReferenceDateTimePicker.Value.ToShortDateString());
            rows.Add("Relative doubles rate (k2) " + RelativeDoublesRateTextBox.Text);

            string path = System.IO.Path.GetTempFileName();
            FileStream f = new FileStream(path, FileMode.OpenOrCreate);
            StreamWriter s = new StreamWriter(f);
            s.AutoFlush = true;
            foreach (string r in rows)
                s.WriteLine(r);
            f.Close();
            PrintForm pf = new PrintForm(path, this.Text);
            pf.ShowDialog();
            File.Delete(path);
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            //store changes?
            IDDCollarCal cal = new IDDCollarCal(col,modified);
            cal.StartPosition = FormStartPosition.CenterScreen;
            cal.Show();
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (MessageBox.Show("Are you sure you want to abandon your changes and exit?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    this.Close();
            }
            else
                this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/collar.htm");
        }


    }
}
