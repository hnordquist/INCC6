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
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AnalysisDefs;
namespace UI
{
	using Integ = NCC.IntegrationHelpers;

	public partial class IDDK5CollarItemData : Form
    {
        INCCAnalysisParams.collar_combined_rec col;
        bool modified;
        MethodParamFormFields mp;
        double total = 1;
        double totalerr = 0;

        public IDDK5CollarItemData(bool suppressBackButton = false)
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.Collar);
            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            INCCAnalysisParams.collar_combined_rec inDB;

            if (mp.HasMethod)
            {
                mp.imd = new INCCAnalysisParams.collar_combined_rec((INCCAnalysisParams.collar_combined_rec)mp.ams.GetMethodParameters(mp.am));
                inDB = (INCCAnalysisParams.collar_combined_rec)mp.imd;
            }

            K5TextBox.ToValidate = NumericTextBox.ValidateType.Double;
            K5TextBox.NumberFormat = NumericTextBox.Formatter.E3;
            K5ErrorTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            K5ErrorTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            K5TextBox.Value = col.collar.sample_corr_fact.v;
            K5ErrorTextBox.Value = col.collar.sample_corr_fact.err;

            for (int i = 0; i < INCCAnalysisParams.MAX_COLLAR_K5_PARAMETERS; i++)
            {
                if (col.k5.k5[i] != null)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    dataGridView1.Rows.Add(new object[] { col.k5.k5_checkbox[i], col.k5.k5_label[i], col.k5.k5[i].v, col.k5.k5[i].err });
                }
            }
            k5TotalTextBox.Value = total;
            k5TotalErrTextBox.Value = totalerr;

        }
        public IDDK5CollarItemData(INCCAnalysisParams.collar_combined_rec c, bool mod)
        {
            InitializeComponent();
            col = c;
            modified = mod;
            K5TextBox.ToValidate = NumericTextBox.ValidateType.Double;
            K5TextBox.NumberFormat = NumericTextBox.Formatter.E3;
            K5ErrorTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            K5ErrorTextBox.NumberFormat = NumericTextBox.Formatter.E3;
            K5TextBox.Value = col.collar.sample_corr_fact.v;
            K5ErrorTextBox.Value = col.collar.sample_corr_fact.err;

            for (int i = 0; i < INCCAnalysisParams.MAX_COLLAR_K5_PARAMETERS; i ++)
            {
                if (col.k5.k5[i] != null)
                {
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);
                    dataGridView1.Rows.Add(new object[] { col.k5.k5_checkbox[i], col.k5.k5_label[i], col.k5.k5[i].v, col.k5.k5[i].err });
                }
            }
            k5TotalTextBox.Value = total;
            k5TotalErrTextBox.Value = totalerr;

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                string display = string.Empty;
                DataGridViewCell cell =
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                switch (e.ColumnIndex)
                {
                    case 0:
                        display = display = "Check this box to include this row in the product of K5\r\n" +
                            "correction factors.";
                        cell.ToolTipText = display;
                        break;
                    case 1:
                        display = "Type in the name for this K5 correction factor.";
                        cell.ToolTipText = display;
                        break;
                    case 2:
                        display = "K5 correction factor to be included in the product of all enabled K5\r\n" +
                            "correction factors.";
                        cell.ToolTipText = display;
                        break;
                    case 3:
                        display = "K5 correction factor absolute error to be combined in quadrature with\r\n" +
                            "all enabled K5 correction factor errors, and then multiplied by the product of\r\n" +
                            "all enabed K5 correction factors.";
                        cell.ToolTipText = display;
                        break;
                }
            }
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            NCCReporter.Section sec = new NCCReporter.Section(null, 0, 0, 0);
            List<string> rows = new List<string>();
            //Addrows

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

        private void FinishBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
            SaveParamsToDb();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
            IDDCollarCal calib = new IDDCollarCal(col, modified);
            calib.StartPosition = FormStartPosition.CenterScreen;
            calib.Show();
            Close();
        }

        private void SaveParamsToDb ()
        {
            mp = new MethodParamFormFields(AnalysisMethod.Collar);
            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            INCCAnalysisParams.collar_combined_rec inDB;

            if (!string.IsNullOrEmpty(col.k5.k5_item_type) &&  // not null
				string.Compare(col.k5.k5_item_type, mp.acq.item_type, true) != 0)  // and not the same string
            {
                modified = true;
            }
            if (modified)
            {
                if (mp.HasMethod)
                {
                    mp.imd = new INCCAnalysisParams.collar_combined_rec((INCCAnalysisParams.collar_combined_rec)mp.ams.GetMethodParameters(mp.am));
                    inDB = (INCCAnalysisParams.collar_combined_rec)mp.imd;
                    inDB.GenParamList();
					if (!string.IsNullOrEmpty(col.k5.k5_item_type))
						mp.acq.item_type = col.k5.k5_item_type;
                }
                else
                {
                    mp.imd = new INCCAnalysisParams.collar_combined_rec(); // not mapped, so make a new one
                    mp.imd.modified = true;
                    inDB = (INCCAnalysisParams.collar_combined_rec)mp.imd;
                    inDB.GenParamList();
					if (!string.IsNullOrEmpty(col.k5.k5_item_type))
						mp.acq.item_type = col.k5.k5_item_type;
				}

                col.k5.CopyTo(inDB.k5);
                inDB.k5.modified = true;
                mp.Persist();
            }
        }
        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > 1)// Numeric column and editable
            {
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    dataGridView1.Rows[e.RowIndex].ErrorText = "Value must not be empty.";
                    e.Cancel = true;
                }
                else
                {
                    Regex reg = new Regex("[1-9][0-9]*\\.?[0-9]*([Ee][+-]?[0-9]+)?");  // valid for point-based (e.g 1.0, not 1,0 or 1 0) numbering systems
                    if (!reg.IsMatch(e.FormattedValue.ToString()))
                    {
                        dataGridView1.Rows[e.RowIndex].ErrorText = "Value is not a floating point number.";
                        e.Cancel = true;
                        double res = 0;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res.ToString("E3");
                        dataGridView1.Rows[e.RowIndex].ErrorText = string.Empty;
                        dataGridView1.RefreshEdit();
                    }
                    else
                    {
                        double res = 0;
                        double.TryParse(e.FormattedValue.ToString(), out res);
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res.ToString("E3");
                        dataGridView1.RefreshEdit();
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void StoreChanges ()
        {
            double temp = 0;

            for (int i = 0; i < INCCAnalysisParams.MAX_COLLAR_K5_PARAMETERS; i ++)
            {
                if ((bool)dataGridView1.Rows[i].Cells[0].Value != col.k5.k5_checkbox[i])
                {
                    modified = true;
                    col.k5.k5_checkbox[i] = (bool)dataGridView1.Rows[i].Cells[0].Value;
                }

                if ((string)dataGridView1.Rows[i].Cells[1].Value != col.k5.k5_label[i])
                {
                    modified = true;
                    col.k5.k5_label[i] = (string) dataGridView1.Rows[i].Cells[1].Value;
                }

                double.TryParse (dataGridView1.Rows[i].Cells[2].Value.ToString(), out temp);
                if ( temp!= col.k5.k5[i].v)
                {
                    modified = true;
                    col.k5.k5[i].v = temp;
                }

                double.TryParse(dataGridView1.Rows[i].Cells[3].Value.ToString(), out temp);
                if (temp != col.k5.k5[i].err)
                {
                    modified = true;
                    col.k5.k5[i].err = temp; 
                }
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
            if (modified)
            {
                if (MessageBox.Show("Are you sure you want to abandon your changes and exit?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    Close();
            }
            else
                Close();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            if (col != null)
            {
                col.collar.sample_corr_fact.v = 1;
                col.collar.sample_corr_fact.err = 0;
                bool check = false;
                double temp = 0;
                double temp1 = 0;
                int numChecks = 0;
                for (int i = 0; i < INCCAnalysisParams.MAX_COLLAR_K5_PARAMETERS; i++)
                {
                    //Checkbox
                    check = (bool)dataGridView1.Rows[i].Cells[0].Value;
                    if (check)
                    {
                        double.TryParse(dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString(), out temp);
                        col.collar.sample_corr_fact.v *= temp;
                        double.TryParse(dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(), out temp1);
                        col.collar.sample_corr_fact.err += (temp != 0)?(temp1 / temp) * (temp1 / temp):0;
                        numChecks++;
                    }
                }
                col.collar.sample_corr_fact.err = col.collar.sample_corr_fact.v * Math.Sqrt(col.collar.sample_corr_fact.err) != Double.NaN?col.collar.sample_corr_fact.v * Math.Sqrt(col.collar.sample_corr_fact.err):0 ;

                if (numChecks == 0)
                {
                    col.collar.sample_corr_fact.v = 0;
                    col.collar.sample_corr_fact.err = 0;
                }
                K5TextBox.Value = col.collar.sample_corr_fact.v;
                K5ErrorTextBox.Value = col.collar.sample_corr_fact.err;
            }
        }

        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            dataGridView1.EndEdit();
        }

        private void HelpBtn_Click_1(object sender, EventArgs e)
        {
            Help.ShowHelp(null, ".\\inccuser.chm", HelpNavigator.Topic, "/WordDocuments/collar.htm");
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            total = 1;
            totalerr = 0;
            double temp = 0;
            double temp1 = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if ((bool)row.Cells[0].Value)
                {
                    double.TryParse(row.Cells[2].Value.ToString(), out temp);
                    total *= temp;
                    double.TryParse(row.Cells[3].Value.ToString(), out temp1);
                    totalerr +=
                        (temp1 / temp) *
                        (temp1 / temp);
                }

            }
            k5TotalTextBox.Value = total;
            k5TotalErrTextBox.Value = totalerr;
        }
    }
}
