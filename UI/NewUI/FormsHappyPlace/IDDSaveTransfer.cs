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
using System.Collections.Generic;
using AnalysisDefs;
using NCCTransfer;
namespace NewUI
{
	using NC = NCC.CentralizedState;

	public partial class IDDSaveTransfer : Form
	{
		public IDDSaveTransfer()
		{
			InitializeComponent();
		}

		private void CurrentDetectorRadioButton_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void AllDetectorsRadioButton_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void OKBtn_Click(object sender, EventArgs e)
		{
			if (CurrentDetectorRadioButton.Checked)
			{
				IDDSaveCampaignId f = new IDDSaveCampaignId();
				DialogResult r = f.ShowDialog();
				if (r == DialogResult.OK)
				{
					string dest = UIIntegration.GetUsersFolder("Select Directory for Saving Measurement Data", string.Empty);
					if (string.IsNullOrEmpty(dest))
						return;
					IDDMeasurementList measlist = new IDDMeasurementList(); 
					measlist.Init(f.FilteredList,
							AssaySelector.MeasurementOption.unspecified,
							lmonly: false, goal: IDDMeasurementList.EndGoal.Transfer, inspnum: f.inspnum, detector: f.det);
					DialogResult dr = DialogResult.None;
					if (measlist.bGood)
						measlist.ShowDialog();
					dr = measlist.DialogResult;
					if (dr != DialogResult.OK)
						return;
					List<Measurement> mlist = measlist.GetSelectedMeas();
					foreach (Measurement m in mlist)
					{
						CycleList cl = NC.App.DB.GetCycles(f.det, m.MeasurementId, m.AcquireState.data_src); // APluralityOfMultiplicityAnalyzers: // URGENT: get all the cycles associated with each analyzer, restoring into the correct key->result pair
                        m.Cycles.AddRange(cl);
						// NEXT: m.CFCyles for AAS not used for INCC6 created measurements, only INCC5 transfer measurements have them m.Add(c, i);
						m.INCCAnalysisResults.TradResultsRec = NC.App.DB.ResultsRecFor(m.MeasurementId); 
						m.ReportRecalc(); // dev note: not strictly from INCC5, but based on usage complaint from LANL 
					}
					List<INCCTransferFile> itdl = INCCKnew.XFerFromMeasurements(mlist);
					foreach (INCCTransferFile itd in itdl)
					{
						itd.Save(dest);
					}
				}
			}
			else if (AllDetectorsRadioButton.Checked)
			{
				string dest = UIIntegration.GetUsersFolder("Select Directory for Saving Measurement Data", string.Empty);
				if (string.IsNullOrEmpty(dest))
					return;
				List<Detector> l = NC.App.DB.Detectors;
				foreach (Detector det in l)
				{
					List<Measurement> mlist = NC.App.DB.MeasurementsFor(det.Id.DetectorId);
					foreach (Measurement m in mlist)
					{
						CycleList cl = NC.App.DB.GetCycles(det, m.MeasurementId, m.AcquireState.data_src); // APluralityOfMultiplicityAnalyzers: // URGENT: get all the cycles associated with each analyzer, restoring into the correct key->result pair
                        m.Add(cl);
                        m.INCCAnalysisResults.TradResultsRec = NC.App.DB.ResultsRecFor(m.MeasurementId); 
					}
					List<INCCTransferFile> itdl = INCCKnew.XFerFromMeasurements(mlist);
					foreach (INCCTransferFile itd in itdl)
					{
						itd.Save(dest);
					}
				}
			}
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
