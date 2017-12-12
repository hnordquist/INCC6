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
using System.Collections.Generic;
using System.Windows.Forms;
using AnalysisDefs;
using NCCTransfer;

namespace UI
{
	using Integ = NCC.IntegrationHelpers;
	using NC = NCC.CentralizedState;

	public partial class IDDSaveInitialData : Form
	{
		public IDDSaveInitialData()
		{
			InitializeComponent();
		}

		private void DetectorCurrentRadioButton_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void DetectorAllRadioButton_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void CalibrationCurrentRadioButton_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void CalibrationAllRadioButton_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void OKBtn_Click(object sender, EventArgs e)
		{
			string dest = UIIntegration.GetUsersFolder("Select Destination", string.Empty);
			if (string.IsNullOrEmpty(dest))
				return;
			if (DetectorCurrentRadioButton.Checked)
			{
				List<Detector> l = new List<Detector>();
				l.Add(Integ.GetCurrentAcquireDetector());
				INCCInitialDataDetectorFile iddf = INCCKnew.FromDetectors(l);
				iddf.Save(dest);
			} else if (DetectorAllRadioButton.Checked)
			{
				List<Detector> l = NC.App.DB.Detectors;
				INCCInitialDataDetectorFile iddf = INCCKnew.FromDetectors(l);
				iddf.Save(dest);
			} else if (CalibrationCurrentRadioButton.Checked)
			{
				List<Detector> l = new List<Detector>();
				l.Add(Integ.GetCurrentAcquireDetector());
				List<INCCInitialDataCalibrationFile> lidcf = INCCKnew.CalibFromDetectors(l);
				if (lidcf.Count > 0)
					if (!lidcf[0].Save(dest))
                        NC.App.AppLogger.TraceEvent(NCCReporter.LogLevels.Warning, 33154, "No calibration parameters for " + lidcf[0].Name);

			} else if (CalibrationAllRadioButton.Checked)
			{
				List<Detector> l = NC.App.DB.Detectors;
				List<INCCInitialDataCalibrationFile> lidcf = INCCKnew.CalibFromDetectors(l);
				foreach (INCCInitialDataCalibrationFile idcf in lidcf)
				{
					if (!idcf.Save(dest))
                        NC.App.AppLogger.TraceEvent(NCCReporter.LogLevels.Warning, 33154, "No calibration parameters for " + idcf.Name);
				}
			}
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
