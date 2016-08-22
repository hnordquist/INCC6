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
	using N = NCC.CentralizedState;
	using Integ = NCC.IntegrationHelpers;

	public partial class IDDCurveType : Form
	{

		public string Material;
		public AnalysisMethod AnalysisMethod;
		public INCCAnalysisParams.CurveEquation CurveEquation;
		private Detector det;
		public IDDCurveType()
		{
			InitializeComponent();
			det = Integ.GetCurrentAcquireDetector();
			Text += " for detector " + det.Id.DetectorName;
			CurveTypeComboBox.Items.Clear();
			foreach (INCCAnalysisParams.CurveEquation cs in Enum.GetValues(typeof(INCCAnalysisParams.CurveEquation)))
			{
				CurveTypeComboBox.Items.Add(cs.ToDisplayString());
			}
			CurveTypeComboBox.Refresh();
            CurveTypeComboBox.SelectedIndex = 0;
		}

		private void OKBtn_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.OK;
			CurveEquation = (INCCAnalysisParams.CurveEquation)CurveTypeComboBox.SelectedIndex;
			Close();
			IDDDemingFitSelect measlist = new IDDDemingFitSelect();
			measlist.CurveEquation = CurveEquation;
			measlist.AnalysisMethod = AnalysisMethod;
			measlist.Material = Material;
			measlist.Init(det.Id.DetectorId);
            if (measlist.bGood)
                measlist.ShowDialog();
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void HelpBtn_Click(object sender, EventArgs e)
		{

		}

		private void button1_Click(object sender, EventArgs e)
		{
			// dialog that opens a DMR file, and loads the resulting values into a coeff instance
			OpenFileDialog aDlg =  new OpenFileDialog();
			aDlg.CheckFileExists = true;
			aDlg.FileName = "Deming.dmr";
			aDlg.Filter = "DMR files (*.dmr)|*.dmr|All files (*.*)|*.*";
            aDlg.DefaultExt = ".dmr";
            aDlg.InitialDirectory = N.App.AppContext.FileInput;
            aDlg.Title = "Select a Deming results file";
            aDlg.Multiselect = false;
            aDlg.RestoreDirectory = true;
			DialogResult c645 = aDlg.ShowDialog();
            if (c645 == DialogResult.OK)
			{
				NCCFile.CoefficientFile onefile = new NCCFile.CoefficientFile();
				string path = System.IO.Path.GetFullPath(aDlg.FileName);
				onefile.Process(path);
				CurveEquation = (INCCAnalysisParams.CurveEquation)CurveTypeComboBox.SelectedIndex;
				EqCoeffViewer c = new EqCoeffViewer(onefile.Coefficients, CurveEquation);
				c.Material = Material;
				c.AnalysisMethod = AnalysisMethod;
				if (c.ShowDialog() == DialogResult.OK)
					ApplyCoefficients(c.Coefficients);  // apply Coefficients to the current selected analysis method
			}
		}

		static void CopyCoefficients(INCCAnalysisParams.CurveEquationVals src, INCCAnalysisParams.CurveEquationVals tgt)
		{
			tgt.a = src.a;
			tgt.b = src.b;
			tgt.c = src.c;
			tgt.d = src.d;
			tgt.var_a = src.var_a;
			tgt.var_b = src.var_b;
			tgt.var_c = src.var_c;
			tgt.var_d = src.var_d;
			tgt.setcovar(Coeff.a, Coeff.b, src.covar(Coeff.a, Coeff.b));
			tgt.setcovar(Coeff.a, Coeff.c, src.covar(Coeff.a, Coeff.c));
			tgt.setcovar(Coeff.a, Coeff.d, src.covar(Coeff.a, Coeff.d));
			tgt.setcovar(Coeff.b, Coeff.c, src.covar(Coeff.b, Coeff.c));
			tgt.setcovar(Coeff.b, Coeff.d, src.covar(Coeff.b, Coeff.d));
			tgt.setcovar(Coeff.c, Coeff.d, src.covar(Coeff.c, Coeff.d));
		}

		void ApplyCoefficients(INCCAnalysisParams.CurveEquationVals coeff)
		{
			INCCSelector sel = new INCCSelector(det.Id.DetectorId, Material);
			AnalysisMethods lam;
			bool found = N.App.DB.DetectorMaterialAnalysisMethods.TryGetValue(sel, out lam);
			if (!found)
			{
				lam = new AnalysisMethods(sel);
			}
			if (!lam.HasMethod(AnalysisMethod))
			{
				MessageBox.Show(string.Format("{0} method not specified for detector {1} and material {2}", 
									AnalysisMethod.FullName(), det.Id.DetectorId, Material),
					"Coefficient File Ingester", MessageBoxButtons.OK);
				return;
			}

			INCCAnalysisParams.INCCMethodDescriptor imd = lam.GetMethodParameters(AnalysisMethod);
			switch (AnalysisMethod)
			{
			case AnalysisMethod.CalibrationCurve:
				INCCAnalysisParams.cal_curve_rec c = (INCCAnalysisParams.cal_curve_rec)imd;
				CopyCoefficients(coeff, c.cev);
				c.cev.cal_curve_equation = CurveEquation;
				break;
			case AnalysisMethod.KnownA:
				INCCAnalysisParams.known_alpha_rec ka = (INCCAnalysisParams.known_alpha_rec)imd;
				CopyCoefficients(coeff, ka.cev);
				ka.cev.cal_curve_equation = CurveEquation;
				break;
			case AnalysisMethod.AddASource:
				INCCAnalysisParams.add_a_source_rec aas = (INCCAnalysisParams.add_a_source_rec)imd;
				CopyCoefficients(coeff, aas.cev);
				aas.cev.cal_curve_equation = CurveEquation;
				break;
			case AnalysisMethod.Active:
				INCCAnalysisParams.active_rec ac = (INCCAnalysisParams.active_rec)imd;
				CopyCoefficients(coeff, ac.cev);
				ac.cev.cal_curve_equation = CurveEquation;
				break;
			}
			imd.modified = true;
			// ok save it now
			N.App.DB.UpdateAnalysisMethod(sel, lam);  // flush changes on internal map to the DB
		}
	}
}
