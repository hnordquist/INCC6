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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AnalysisDefs;
namespace UI
{
	using Integ = NCC.IntegrationHelpers;
    public partial class IDDDualEnergyMult : Form
    {
        public IDDDualEnergyMult()
        {
            InitializeComponent();
			InnerTextBox.Text = "0.0001"; // default
			OuterTextBox.Text = "0.0001"; // default
			_reg = new Regex("[1-9][0-9]*\\.?[0-9]*([Ee][+-]?[0-9]+)?");  // reg ex for number test
			det = Integ.GetCurrentAcquireDetector();
            Text += " for detector " + det.Id.DetectorName;
        }

		private Detector det;
		Regex _reg;

        private void PrintBtn_Click(object sender, EventArgs e)
        {

        }

        private void OKBtn_Click(object sender, EventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

		private void GridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
			if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
			{
				GridView.Rows[e.RowIndex].ErrorText = "Value must not be empty";
				e.Cancel = true;
			} else
			{
				if (!_reg.IsMatch(e.FormattedValue.ToString()))
				{
					GridView.Rows[e.RowIndex].ErrorText = "Value is not a floating point number";
					e.Cancel = true;
					double res = (double)GridView.Rows[e.RowIndex].Tag;
					if (e.ColumnIndex == 0)
						GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res.ToString("F2");
					else
						GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res.ToString("F3");
					GridView.Rows[e.RowIndex].ErrorText = string.Empty;
					GridView.RefreshEdit();
				} else
        {
					double res = 0;
					double.TryParse(e.FormattedValue.ToString(), out res);
					GridView.Rows[e.RowIndex].Tag = res;
					if (e.ColumnIndex == 0)
						GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res.ToString("F2");
					else
						GridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res.ToString("F3");
					GridView.RefreshEdit();
				}
			}
        }

        private void GridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.   
            GridView.Rows[e.RowIndex].ErrorText = string.Empty;
        }

    }
}
