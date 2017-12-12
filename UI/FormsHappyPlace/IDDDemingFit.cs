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
using AnalysisDefs;

namespace UI
{
	using Integ = NCC.IntegrationHelpers;
	using N = NCC.CentralizedState;

    public partial class IDDDemingFit : Form
    {

		private Detector det;
        private AcquireParameters ap;
		private IDDCurveType ict;
        public IDDDemingFit()
        {
            InitializeComponent();
            Integ.GetCurrentAcquireDetectorPair(ref ap, ref det);
            Text += " for detector " + det.Id.DetectorName;
			ict = new IDDCurveType();
            RefreshMaterialComboBox();
			RefreshMethodComboBox();
		}

		public void RefreshMaterialComboBox()
        {
            MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in N.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }
            MaterialTypeComboBox.Refresh();
			if (MaterialTypeComboBox.Items.Count > 0)
				MaterialTypeComboBox.SelectedItem = ap.item_type;
		}
 		 public void RefreshMethodComboBox()
        {
            AnalysisMethodComboBox.Items.Clear();
                AnalysisMethodComboBox.Items.Add(new jigglypuff(AnalysisMethod.CalibrationCurve));                
                AnalysisMethodComboBox.Items.Add(new jigglypuff(AnalysisMethod.KnownA)); 
				if (VerificationCalDataRadioButton.Checked)
				{               
					AnalysisMethodComboBox.Items.Add(new jigglypuff(AnalysisMethod.AddASource));                
					AnalysisMethodComboBox.Items.Add(new jigglypuff(AnalysisMethod.Active));
				}
            AnalysisMethodComboBox.Refresh();
			AnalysisMethodComboBox.SelectedIndex = 0;
		}         

        private void VerificationCalDataRadioButton_CheckedChanged(object sender, EventArgs e)
        {
			RefreshMethodComboBox();
        }


        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (MaterialTypeComboBox.SelectedItem != null)
				ict.Material = (string)MaterialTypeComboBox.SelectedItem;
        }

        private void AnalysisMethodComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (AnalysisMethodComboBox.SelectedItem != null)
			{
				jigglypuff j = (jigglypuff)AnalysisMethodComboBox.SelectedItem;
				ict.AnalysisMethod = j.a;
			}
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
			DialogResult = DialogResult.OK;
			if (string.Compare(ap.item_type, (string)MaterialTypeComboBox.SelectedItem, true) != 0)  // mtl type changed on the way out
            {
				ap.item_type = (string)MaterialTypeComboBox.SelectedItem;
                INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, ap.item_type, DateTime.Now);
                ap.MeasDateTime = sel.TimeStamp; ap.lm.TimeStamp = sel.TimeStamp;
                N.App.DB.AddAcquireParams(sel, ap);  // it's a new one, not the existing one modified
            }
			Close();
        }

		public void CurveTypeSelectionStep()
		{
			if (CalcDataList == null)
				CalcDataList = new CalibrationCurveList();  // one-time init upon first use.
			ict.CalcDataList = CalcDataList;// pre-load last best result, not maybe the best approach ...
			ict.ShowDialog();
			CalcDataList = ict.CalcDataList; // preserve the last list generated by any processing
		}
        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

		public class jigglypuff
		{
			public jigglypuff(AnalysisMethod am)
			{
				a = am;
				s = a.FullName();
			}
			public AnalysisMethod a;
			public string s;
			override public string ToString()
			{
				return s;
			}

		}

		static public CalibrationCurveList CalcDataList;

    }
}
