﻿/*
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
using System.Collections.Generic;
using AnalysisDefs;
namespace NewUI
{

    using Integ = NCC.IntegrationHelpers;
	using N = NCC.CentralizedState;

    public partial class IDDReviewAll : Form
    {

		bool bLMOnly = false;
        public bool bGood = false;
        public IDDReviewAll(bool LMOnly)
        {
            InitializeComponent();
			bLMOnly = LMOnly;
			Integ.GetCurrentAcquireDetectorPair(ref acq, ref det);
            if (!det.ListMode && LMOnly)
            {
                MessageBox.Show(det.Id.DetectorId + " is not a List Mode detector", "Well, hello there");
                return;
            }
            bGood = true;
            FieldFiller();
            if (LMOnly)
                Text = "List Mode Measurements for Detector " + det.Id.DetectorId;
            else
                Text += " for Detector " + det.Id.DetectorId;
			mlist = N.App.DB.IndexedResultsFor(det.Id.DetectorId, string.Empty, "All");
			LoadInspNumCombo();
            DisplayResultsInTextRadioButton.Checked = true;
        }
        private List<INCCDB.IndexedResults> mlist;
        AcquireParameters acq;
		Detector det;
		void LoadInspNumCombo()
		{
			SortedSet<string> set = new SortedSet<string>();
			InspectionNumberComboBox.Items.Add("All");
			InspectionNumberComboBox.SelectedItem = "All";
			foreach(INCCDB.IndexedResults ir in mlist)
			{
				if (!string.IsNullOrEmpty(ir.Campaign))
					set.Add(ir.Campaign);
			}
			foreach(string si in set)
			{
				InspectionNumberComboBox.Items.Add(si);
			}
		}
		public void FieldFiller()
        {
 			DetectorParametersCheckBox.Checked = acq.review.DetectorParameters;
            CalibrationParametersCheckBox.Checked = acq.review.CalibrationParameters;
            IsotopicsCheckBox.Checked = acq.review.Isotopics;
            IndividualCycleRawDataCheckBox.Checked = acq.review.RawCycleData;
            IndividualCycleRateDateCheckBox.Checked = acq.review.RateCycleData;
            SummedRawCoincidenceDataCheckBox.Checked = acq.review.SummedRawCoincData;
            SummedMultiplicityDistributionsCheckBox.Checked = acq.review.SummedMultiplicityDistributions;
            IndividualCycleMultiplicityDistributionsCheckBox.Checked = acq.review.MultiplicityDistributions;
        }
        void FieldEnabler()
        {
            bool enable = DisplayResultsInTextRadioButton.Checked;
            DetectorParametersCheckBox.Enabled = enable;
            CalibrationParametersCheckBox.Enabled = enable;
            IsotopicsCheckBox.Enabled = enable;
            IndividualCycleRawDataCheckBox.Enabled = enable;
            IndividualCycleRateDateCheckBox.Enabled = enable;
            SummedRawCoincidenceDataCheckBox.Enabled = enable;
            SummedMultiplicityDistributionsCheckBox.Enabled = enable;
            IndividualCycleMultiplicityDistributionsCheckBox.Enabled = enable;
        }
        private void OKBtn_Click(object sender, EventArgs e)
        {
			List<INCCDB.IndexedResults> list = null;
			string inspnum = string.Copy(InspectionNumberComboBox.SelectedItem.ToString());
			if (string.Compare(inspnum,"All", true) == 0)
			{
				inspnum = "";
				list = mlist;
			}
			else
				list = mlist.FindAll(ir => (string.Compare(inspnum,ir.Campaign, true) == 0));
            SaveAcquireState();
            IDDMeasurementList measlist = new IDDMeasurementList();
			measlist.TextReport = DisplayResultsInTextRadioButton.Checked; 
            measlist.Init(list,
                    AssaySelector.MeasurementOption.unspecified,
                    lmonly: bLMOnly, goal: IDDMeasurementList.EndGoal.Report, inspnum: inspnum, detector: det);
            measlist.Sections = acq.review;
            if (measlist.bGood)
                measlist.ShowDialog();
			if (!DisplayResultsInTextRadioButton.Checked)
			{
				List<Measurement> mlist = measlist.GetSelectedMeas();
				foreach (Measurement m in mlist)
				{
					new PlotSDTChart(m).Show();
				}
			}
        }

		void SaveAcquireState()
		{
			acq.review.DetectorParameters = DetectorParametersCheckBox.Checked;
			acq.review.CalibrationParameters = CalibrationParametersCheckBox.Checked;
			acq.review.Isotopics = IsotopicsCheckBox.Checked;
			acq.review.RawCycleData = IndividualCycleRawDataCheckBox.Checked;
			acq.review.RateCycleData = IndividualCycleRateDateCheckBox.Checked;
			acq.review.SummedRawCoincData = SummedRawCoincidenceDataCheckBox.Checked;
			acq.review.SummedMultiplicityDistributions = SummedMultiplicityDistributionsCheckBox.Checked;
			acq.review.MultiplicityDistributions = IndividualCycleMultiplicityDistributionsCheckBox.Checked;
			INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det,acq.item_type, acq.MeasDateTime);
			N.App.DB.ReplaceAcquireParams(sel, acq);
		}

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
			Help.ShowHelp(null, ".\\inccuser.chm"/*, HelpNavigator.Topic, "/WordDocuments/selectpu240ecoefficients.htm"*/);
        }

        private void IDDReviewAll_Load(object sender, EventArgs e)
        {
            ToolTip disclaimer = new ToolTip();
            disclaimer.AutoPopDelay = 5000;
            disclaimer.InitialDelay = 1000;
            disclaimer.ReshowDelay = 2000;
            disclaimer.ShowAlways = true;
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

        private void IndividualCycleRateDateCheckBox_CheckedChanged(object sender, EventArgs e)
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

        private void PlotSinglesDoublesTriplesRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            FieldEnabler();
        }

        private void DisplayResultsInTextRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            FieldEnabler();
        }
    }
}
