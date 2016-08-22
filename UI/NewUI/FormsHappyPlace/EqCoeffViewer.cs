/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2016, International Atomic Energy Agency (IAEA), IAEA.org
   Authored by J. Longo

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Windows.Forms;
using AnalysisDefs;
namespace NewUI
{
	using N = NCC.CentralizedState;
	using Integ = NCC.IntegrationHelpers;
	using System.Collections.Generic;
	using System.Text.RegularExpressions;

	public partial class EqCoeffViewer : Form
	{

		public string Material;
		public AnalysisMethod AnalysisMethod;
		public INCCAnalysisParams.CurveEquation CurveEquation;
		private Detector det;

		public INCCAnalysisParams.CurveEquationVals Coefficients;

		internal class DataLoad
		{
			internal string label;
			internal double value;

			internal DataLoad(string s, double v)
			{
				label = s;
				value = v;
			}

			public string[] ToRow()
			{
				return new string[] { label, value.ToString("E")}; 
			}
		}

		List<DataLoad> disprows;
		Regex _reg;

		public EqCoeffViewer(INCCAnalysisParams.CurveEquationVals coeff, INCCAnalysisParams.CurveEquation eq)
		{
			InitializeComponent();
			det = Integ.GetCurrentAcquireDetector();
            Text += " for detector " + det.Id.DetectorName;
			disprows = new List<DataLoad>();
			CurveEquation = eq;
			Coefficients = coeff;
			BuildRep();
			BuildRows();
			BuildCurveCombo();
			_reg = new Regex("[1-9][0-9]*\\.?[0-9]*([Ee][+-]?[0-9]+)?");  // reg ex for number test

		}

		public void BuildRep()
		{
			disprows.Add(new DataLoad("a", Coefficients.a));
			disprows.Add(new DataLoad("b", Coefficients.b));
			disprows.Add(new DataLoad("c", Coefficients.c));
			disprows.Add(new DataLoad("d", Coefficients.d));
			disprows.Add(new DataLoad("variance a", Coefficients.var_a));
			disprows.Add(new DataLoad("variance b", Coefficients.var_b));
			disprows.Add(new DataLoad("variance c", Coefficients.var_c));
			disprows.Add(new DataLoad("variance d", Coefficients.var_d));
			disprows.Add(new DataLoad("covariance ab", Coefficients.covar(Coeff.a, Coeff.b)));
			disprows.Add(new DataLoad("covariance ac", Coefficients.covar(Coeff.a, Coeff.c)));
			disprows.Add(new DataLoad("covariance ad", Coefficients.covar(Coeff.a, Coeff.d)));
			disprows.Add(new DataLoad("covariance bc", Coefficients.covar(Coeff.b, Coeff.c)));
			disprows.Add(new DataLoad("covariance bd", Coefficients.covar(Coeff.b, Coeff.d)));
			disprows.Add(new DataLoad("covariance cd", Coefficients.covar(Coeff.c, Coeff.d)));
		}

		void BuildCurveCombo()
		{
			CurveType.Items.Clear();
			foreach (INCCAnalysisParams.CurveEquation cs in Enum.GetValues(typeof(INCCAnalysisParams.CurveEquation)))
			{
				CurveType.Items.Add(cs.ToDisplayString());
			}
			CurveType.Refresh();
            CurveType.SelectedIndex = (int)CurveEquation;
		}
		void BuildRows()
		{
			DataGridViewRowCollection rows = CoeffView.Rows;

			switch (CurveEquation)
			{
			case INCCAnalysisParams.CurveEquation.CUBIC:// a b c d
				foreach (DataLoad dl in disprows)
				{
					string[] a = dl.ToRow();
					int i = rows.Add(a);
					rows[i].Tag = dl.value; // save full double value, nor formatted string
					rows[i].Cells[0].ReadOnly = true;
				}
				break;
			case INCCAnalysisParams.CurveEquation.POWER: // a b
			case INCCAnalysisParams.CurveEquation.HOWARDS:// a b
			case INCCAnalysisParams.CurveEquation.EXPONENTIAL:// a b
				{
					string[] x = disprows[0].ToRow();
					int 
					i = rows.Add(x); rows[i].Cells[0].ReadOnly = true; rows[i].Tag = disprows[0].value; // a 
					x = disprows[1].ToRow();
					i = rows.Add(x); rows[i].Cells[0].ReadOnly = true; rows[i].Tag = disprows[1].value; // b  
					x = disprows[4].ToRow(); 
					i = rows.Add(x); rows[i].Cells[0].ReadOnly = true; rows[i].Tag = disprows[4].value; // var a
					x = disprows[5].ToRow();
					i = rows.Add(x); rows[i].Cells[0].ReadOnly = true; rows[i].Tag = disprows[5].value; // var b 
				}
				break;
			}
		}
		bool DetectChangeAndCopyIt()
		{
			DataGridViewRowCollection rows = CoeffView.Rows;
			bool change = false;
			int i = 0;
			switch (CurveEquation)
			{
			case INCCAnalysisParams.CurveEquation.CUBIC:// a b c d
				if (Coefficients.a != (double)rows[i].Tag)
					{ Coefficients.a = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.b != (double)rows[i].Tag)
					{ Coefficients.b = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.c != (double)rows[i].Tag)
					{ Coefficients.c = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.d != (double)rows[i].Tag)
					{ Coefficients.d = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.var_a != (double)rows[i].Tag)
					{ Coefficients.var_a = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.var_b != (double)rows[i].Tag)
					{ Coefficients.var_b = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.var_c != (double)rows[i].Tag)
					{ Coefficients.var_c = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.var_d != (double)rows[i].Tag)
					{ Coefficients.var_d = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.covar(Coeff.a, Coeff.b) != (double)rows[i].Tag)
					{ Coefficients.setcovar(Coeff.a, Coeff.b, (double)rows[i].Tag); change = true; }
				i++;
				if (Coefficients.covar(Coeff.a, Coeff.c) != (double)rows[i].Tag)
					{ Coefficients.setcovar(Coeff.a, Coeff.c, (double)rows[i].Tag); change = true; }
				i++;
				if (Coefficients.covar(Coeff.a, Coeff.d) != (double)rows[i].Tag)
					{ Coefficients.setcovar(Coeff.a, Coeff.d, (double)rows[i].Tag); change = true; }
				i++;
				if (Coefficients.covar(Coeff.a, Coeff.b) != (double)rows[i].Tag)
					{ Coefficients.setcovar(Coeff.a, Coeff.b, (double)rows[i].Tag); change = true; }
				i++;
				if (Coefficients.covar(Coeff.b, Coeff.c) != (double)rows[i].Tag)
					{ Coefficients.setcovar(Coeff.b, Coeff.c, (double)rows[i].Tag); change = true; }
				i++;
				if (Coefficients.covar(Coeff.b, Coeff.d) != (double)rows[i].Tag)
					{ Coefficients.setcovar(Coeff.b, Coeff.d, (double)rows[i].Tag); change = true; }
				i++;
				if (Coefficients.covar(Coeff.c, Coeff.d) != (double)rows[i].Tag)
					{ Coefficients.setcovar(Coeff.c, Coeff.d, (double)rows[i].Tag); change = true; }
				i++;
				break;
			case INCCAnalysisParams.CurveEquation.POWER: // a b
			case INCCAnalysisParams.CurveEquation.HOWARDS:// a b
			case INCCAnalysisParams.CurveEquation.EXPONENTIAL:// a b
				if (Coefficients.a != (double)rows[i].Tag)
					{ Coefficients.a = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.b != (double)rows[i].Tag)
					{ Coefficients.b = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.var_a != (double)rows[i].Tag)
					{ Coefficients.var_a = (double)rows[i].Tag; change = true; }
				i++;
				if (Coefficients.var_b != (double)rows[i].Tag)
					{ Coefficients.var_b = (double)rows[i].Tag; change = true; }
				i++;
				break;
			}

			return change;
		}

		private void OK_Click(object sender, EventArgs e)
		{
			DetectChangeAndCopyIt();
			DialogResult = DialogResult.OK;
			Close();
		}

		private void Cancel_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

		private void CoeffView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
               if (string.IsNullOrEmpty (e.FormattedValue.ToString()))
                {    
                    CoeffView.Rows[e.RowIndex].ErrorText = "Value must not be empty";
                    e.Cancel = true;
                }
                else
                {
                    if (!_reg.IsMatch(e.FormattedValue.ToString()))
                    {
                        CoeffView.Rows[e.RowIndex].ErrorText = "Value is not a floating point number";
                        e.Cancel = true;
                        double res = (double)CoeffView.Rows[e.RowIndex].Tag;
                        CoeffView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res.ToString("E6");
                        CoeffView.Rows[e.RowIndex].ErrorText = string.Empty;
                        CoeffView.RefreshEdit();
                    }
                    else
                    {
                        double res = 0;
                        double.TryParse(e.FormattedValue.ToString(),out res);
						CoeffView.Rows[e.RowIndex].Tag = res;
                        CoeffView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value= res.ToString ("E6");
                        CoeffView.RefreshEdit();
                    }
				}
            }
        }

        private void CoeffView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.   
            CoeffView.Rows[e.RowIndex].ErrorText = string.Empty;
        }

	}
}
