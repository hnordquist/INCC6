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
namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
 	using N = NCC.CentralizedState;
   public partial class IDDReviewHoldup : Form
    {
        public IDDReviewHoldup()
        {
            InitializeComponent();
			Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
			FieldFiller();
			this.Text += " for Detector " + det.Id.DetectorId;
		}

		AcquireParameters acq;
		Detector det;
		public void FieldFiller()
        {
			PrintCheckBox.Checked = acq.print;
 			DetectorParametersCheckBox.Checked = acq.review.DetectorParameters;
            IsotopicsCheckBox.Checked = acq.review.Isotopics;
            IndividualCycleRawDataCheckBox.Checked = acq.review.RawCycleData;
            IndividualCycleRateDataCheckBox.Checked = acq.review.RateCycleData;
        }
		void SaveAcquireState()
		{
			acq.review.DetectorParameters = DetectorParametersCheckBox.Checked;
			acq.review.CalibrationParameters = CalibrationParametersCheckBox.Checked;
			acq.review.Isotopics = IsotopicsCheckBox.Checked;
			acq.review.RawCycleData = IndividualCycleRawDataCheckBox.Checked;
			acq.review.RateCycleData = IndividualCycleRateDataCheckBox.Checked;
			INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det,acq.item_type, acq.MeasDateTime);
			N.App.DB.ReplaceAcquireParams(sel, acq);
		}
        private void OKBtn_Click(object sender, EventArgs e)
        {
            IDDMeasurementList measlist = new IDDMeasurementList(
                AssaySelector.MeasurementOption.holdup,
                alltypes: false, goal: IDDMeasurementList.EndGoal.Report, detector: det);
            if (measlist.bGood)
                measlist.ShowDialog();
			SaveAcquireState();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
			Help.ShowHelp(null, ".\\inccuser.chm"/*, HelpNavigator.Topic, "/WordDocuments/selectpu240ecoefficients.htm"*/);
        }

		private void InspectionNumberComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
		private void DetectorParametersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.review.DetectorParameters = ((CheckBox)sender).Checked;
        }
		
		private void CalibrationParametersCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.review.CalibrationParameters = ((CheckBox)sender).Checked;
        }

		private void IsotopicsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.review.Isotopics = ((CheckBox)sender).Checked;
        }
        private void IndividualCycleRawDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.review.RawCycleData = ((CheckBox)sender).Checked;
        }

        private void IndividualCycleRateDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.review.RateCycleData = ((CheckBox)sender).Checked;
        }

        private void SummedRawCoincidenceDataCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.review.SummedRawCoincData = ((CheckBox)sender).Checked;
        }

        private void SummedMultiplicityDistributionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.review.SummedMultiplicityDistributions = ((CheckBox)sender).Checked;
        }

        private void IndividualCycleMultiplicityDistributionsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.review.MultiplicityDistributions = ((CheckBox)sender).Checked;
        }

        private void PrintCheckBox_CheckedChanged(object sender, EventArgs e)
        {
			acq.print = ((CheckBox)sender).Checked;
        }

        private void IDDReviewHoldup_Load(object sender, EventArgs e)
        {
        }




    }
}
