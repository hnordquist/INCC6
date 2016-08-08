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
using System.Windows.Forms;
using AnalysisDefs;
using System.Text.RegularExpressions;

namespace NewUI
{
    using Integ = NCC.IntegrationHelpers;
    public partial class IDDCorrectionFactors : Form
    {
        MethodParamFormFields mp;
        INCCAnalysisParams.collar_combined_rec col;
        bool modified;
        List<string> list = new List<string>();
        List<poison_rod_type_rec> poison;

        public IDDCorrectionFactors(INCCAnalysisParams.collar_combined_rec c, bool mod)
        {
            InitializeComponent();
            mp = new MethodParamFormFields(AnalysisMethod.Collar);
            modified = mod;
            Integ.GetCurrentAcquireDetectorPair(ref mp.acq, ref mp.det);
            this.Text += " for " + mp.det.Id.DetectorName;
            
            col = c;
            MaterialLabel.Text = mp.acq.item_type;
            ModeLabel.Text = col.collar.collar_mode ? "Fast(Cd)" : "Thermal (no Cd)";
            DetectorLabel.Text = mp.det.Id.DetectorName;

            ATextBox.NumberFormat = NumericTextBox.Formatter.E6;
            ATextBox.ToValidate = NumericTextBox.ValidateType.Double;
            AErrorTextBox.NumberFormat = NumericTextBox.Formatter.E6;
            AErrorTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            BTextBox.NumberFormat = NumericTextBox.Formatter.E6;
            BTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            BErrorTextBox.NumberFormat = NumericTextBox.Formatter.E6;
            BErrorTextBox.ToValidate = NumericTextBox.ValidateType.Double;
            NumRodsTextBox.NumberFormat = NumericTextBox.Formatter.NONE;
            NumRodsTextBox.ToValidate = NumericTextBox.ValidateType.Integer;

            FillForm();
            this.TopMost = true;
        }

        private void FillForm()
        {
            ATextBox.Value = col.collar.u_mass_corr_fact_a.v;
            AErrorTextBox.Value = col.collar.u_mass_corr_fact_a.err;
            BTextBox.Value = col.collar.u_mass_corr_fact_b.v;
            BErrorTextBox.Value  = col.collar.u_mass_corr_fact_b.err;
            NumRodsTextBox.Value = col.collar.number_calib_rods;
            DataGridViewRow types = new DataGridViewRow();
            poison = NCC.CentralizedState.App.DB.PoisonRods.GetList();

            for (int i = 0; i < poison.Count; i++)
            {
                list.Add(poison[i].rod_type);
            }

            for (int j = 0; j < list.Count; j++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                DataGridCell combo = new DataGridCell();
                combo.ColumnNumber = 0;
                combo.RowNumber = j;
                DataGridViewComboBoxCell cel = new DataGridViewComboBoxCell();
                foreach (string s in list)
                {
                    cel.Items.Add(s);
                }
                row.Cells[0] = cel;
                row.Cells[0].Value = list[j];
                row.Cells[1].ValueType = typeof(double);
                row.Cells[1].Value = col.collar.poison_absorption_fact[j];
                row.Cells[2].ValueType = typeof(double);
                row.Cells[2].Value = col.collar.poison_rod_a[j].v;
                row.Cells[3].ValueType = typeof(double);
                row.Cells[3].Value = col.collar.poison_rod_a[j].err;
                row.Cells[4].ValueType = typeof(double);
                row.Cells[4].Value = col.collar.poison_rod_b[j].v;
                row.Cells[5].ValueType = typeof(double);
                row.Cells[5].Value = col.collar.poison_rod_b[j].err;
                row.Cells[6].ValueType = typeof(double);
                row.Cells[6].Value = col.collar.poison_rod_c[j].v;
                row.Cells[7].ValueType = typeof(double);
                row.Cells[7].Value = col.collar.poison_rod_c[j].err;
                dataGridView1.Rows.Add(row);
            }
            SetHelp();
        }

        private void SetHelp ()
        {
            ToolTip t1 = new ToolTip();
            t1.IsBalloon = true;
            String display = "The number of rods in the fuel assembly used to determine the\r\n" +
                            "calibration constants. For the calibrations in LA-11965-MS the number\r\n" +
                            "is 204 for PWR and 76 for BWR fuel.";
            provider.SetShowHelp(this.NumRodsTextBox, true);
            provider.SetHelpString(this.NumRodsTextBox, display);
            t1.SetToolTip(this.NumRodsTextBox, display);

            display = "Enter the coefficient a for the uranium mass correctionf actor k4 = 1 +\r\n" +
                "a(b - u); see LA-11965-MS, p. 30. In LA-11965-MS a = 3.89E-004 for PWR fuel and\r\n" +
                "7.24E-004 for BWR fuel.";
            provider.SetShowHelp(this.ATextBox, true);
            provider.SetHelpString(this.ATextBox, display);
            t1.SetToolTip(this.ATextBox, display);

            display = "Enter the error of coefficient a. If the error is not known, enter the\r\n" +
                "default values of 0.076E-004 for PWR fuel and 0.12E-004 for BWR fuel.\r\n";
            provider.SetShowHelp(this.AErrorTextBox, true);
            provider.SetHelpString(this.AErrorTextBox, display);
            t1.SetToolTip(this.AErrorTextBox, display);

            display = "Enter the coefficient b for the uranium mass correctionf actor k4 = 1 +\r\n" +
                "a(b - u); see LA-11965-MS, p. 30. In LA-11965-MS b = 1215 for PWR fuel and\r\n" +
                "453 for BWR fuel.";
            provider.SetShowHelp(this.BTextBox, true);
            provider.SetHelpString(this.BTextBox, display);
            t1.SetToolTip(this.BTextBox, display);

            display = "Enter the error of coefficient b. If the error is not known, enter the\r\n" +
                "default value of 0.001 * b.\r\n";
            provider.SetShowHelp(this.BErrorTextBox, true);
            provider.SetHelpString(this.BErrorTextBox, display);
            t1.SetToolTip(this.BErrorTextBox, display);
        }

        private void NextBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
            IDDK5CollarItemData k5 = new IDDK5CollarItemData(col, modified);
            k5.Show();
            this.Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
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
            //TODO: I don't see collar help??
            System.Windows.Forms.Help.ShowHelp(null, ".\\inccuser.chm");
        }

        private void BackBtn_Click(object sender, EventArgs e)
        {
            StoreChanges();
            IDDCollarCal calib = new IDDCollarCal(col, modified);
            calib.Show();
            this.Close();
        }

        private void StoreChanges()
        {
            if (col.collar.u_mass_corr_fact_a.v != ATextBox.Value)
            {
                modified = true;
                col.collar.u_mass_corr_fact_a.v = ATextBox.Value;
            }

            if (col.collar.u_mass_corr_fact_a.err != AErrorTextBox.Value)
            {
                modified = true;
                col.collar.u_mass_corr_fact_a.err = AErrorTextBox.Value;
            }

            if (col.collar.u_mass_corr_fact_b.v != BTextBox.Value)
            {
                modified = true;
                col.collar.u_mass_corr_fact_b.v = BTextBox.Value;
            }

            if (col.collar.u_mass_corr_fact_b.err != BErrorTextBox.Value)
            {
                modified = true;
                col.collar.u_mass_corr_fact_b.err = BErrorTextBox.Value;
            }
            
            if (col.collar.number_calib_rods != NumRodsTextBox.Value)
            {
                modified = true;
                col.collar.number_calib_rods = (int)NumRodsTextBox.Value;
            }

            for (int i = 0; i < list.Count; i ++)
            {
                double temp = 0;
                Double.TryParse(dataGridView1.Rows[i].Cells[2].Value.ToString(), out temp);
                if (col.collar.poison_rod_a[i].v != temp)
                {
                    modified = true;
                    col.collar.poison_rod_a[i].v = temp;
                }
                Double.TryParse(dataGridView1.Rows[i].Cells[3].Value.ToString(), out temp);
                if (col.collar.poison_rod_a[i].err != temp)
                {
                    modified = true;
                    col.collar.poison_rod_a[i].err = temp;
                }
                Double.TryParse(dataGridView1.Rows[i].Cells[4].Value.ToString(), out temp);
                if (col.collar.poison_rod_b[i].v != temp)
                {
                    modified = true;
                    col.collar.poison_rod_b[i].v = temp;
                }
                Double.TryParse(dataGridView1.Rows[i].Cells[5].Value.ToString(), out temp);
                if (col.collar.poison_rod_b[i].err != temp)
                {
                    modified = true;
                    col.collar.poison_rod_b[i].err = temp;
                }
                Double.TryParse(dataGridView1.Rows[i].Cells[6].Value.ToString(), out temp);
                if (col.collar.poison_rod_c[i].v != temp)
                {
                    modified = true;
                    col.collar.poison_rod_c[i].v = temp;
                }
                Double.TryParse(dataGridView1.Rows[i].Cells[7].Value.ToString(), out temp);
                if (col.collar.poison_rod_c[i].err != temp)
                {
                    modified = true;
                    col.collar.poison_rod_c[i].err = temp;
                }
            }
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > 1)// Numeric column and editable
            {
                if (String.IsNullOrEmpty (e.FormattedValue.ToString()))
                {    
                    dataGridView1.Rows[e.RowIndex].ErrorText =
                    "Value must not be empty.";
                    e.Cancel = true;
                }
                else
                {
                    Regex reg = new Regex("[1-9][0-9]*\\.?[0-9]*([Ee][+-]?[0-9]+)?");
                    if (!reg.IsMatch(e.FormattedValue.ToString()))
                    {
                        dataGridView1.Rows[e.RowIndex].ErrorText = "Value is not a floating point number.";
                        e.Cancel = true;
                        double res = 0;
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = res.ToString("E3");
                        dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
                        this.dataGridView1.RefreshEdit();
                    }
                    else
                    {
                        double res = 0;
                        Double.TryParse(e.FormattedValue.ToString(),out res);
                        dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value= res.ToString ("E3");
                        this.dataGridView1.RefreshEdit();
                    }
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                String display = String.Empty;
                DataGridViewCell cell =
                        this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                switch (e.ColumnIndex)
                {
                    case 0:
                        display = "The poison rod absorption factor is lambda in the poison correction\r\n" +
                        "for gadolinium rods select 'G'. To add a new poison rod type, go to\r\n" +
                        "Maintain | Poison Rody Type Add/Delete.";
                        cell.ToolTipText = display;
                        break;
                    case 1:
                        display = "The poison rod absorption factor is lambda in the poison correction\r\n" +
                            "factor k3 = 1 + a * n * N0 / N * [1 - exp(-lambda * Gd)] * (b - c * E); see\r\n" +
                            "LA-11965-MS, pp 25-30. For gadolinium the recommended factor is 0.647.";
                        cell.ToolTipText = display;
                        break;
                    case 2:
                        display = "Enter coefficient a in the poison rod correction factor k3 = 1 + a * n * N0\r\n" +
                            "/ N * [1 - exp(-lambda * Gd)] * (b - c * E); see LA-11965-MS, pp 25-30.\r\n" +
                            "For PWR and BWR fuel the recommended values for a are 0.0213 and 0.0572, respectively.";
                        cell.ToolTipText = display;
                        break;
                    case 3:
                        display = "Enter the error of coefficient a in the poison rod correction factor k3 = 1\r\n" +
                            "+ a * n * N0/ N * [1 - exp(-lambda * Gd)] * (b - c * E); see LA-11965-MS, pp 25-30.\r\n" +
                            "For PWR and BWR fuel the recommended values for sigma (a) are 0.0.000532 and 0.000143, respectively.";
                        cell.ToolTipText = display;
                        break;
                    case 4:
                        display = "Enter coefficient b in the poison rod correction factor k3 = 1 + a * n * N0\r\n" +
                            "/ N * [1 - exp(-lambda * Gd)] * (b - c * E); see LA-11965-MS, pp 25-30.\r\n" +
                            "For PWR and BWR fuel the recommended values for b are 2.27 and 1.92, respectively.";
                        cell.ToolTipText = display;
                        break;
                    case 5:
                        display = "Enter the error of coefficient b in the poison rod correction factor k3 = 1\r\n" +
                            "+ a * n * N0/ N * [1 - exp(-lambda * Gd)] * (b - c * E); see LA-11965-MS, pp 25-30.\r\n" +
                            "For PWR and BWR fuel the recommended value for sigma (b) is 0.01.";
                        cell.ToolTipText = display;
                        break;
                    case 6:
                        display = "Enter coefficient c in the poison rod correction factor k3 = 1 + a * n * N0\r\n" +
                            "/ N * [1 - exp(-lambda * Gd)] * (b - c * E); see LA-11965-MS, pp 25-30.\r\n" +
                            "For PWR and BWR fuel the recommended values for c are 0.40 and 0.29, respectively.\r\n" +
                            "(2.1000E-001 for PWR no Cd. JRCIspra2001).";
                        cell.ToolTipText = display;
                        break;
                    case 7:
                        display = "Enter the error of coefficient c in the poison rod correction factor k3 = 1\r\n" +
                            "+ a * n * N0/ N * [1 - exp(-lambda * Gd)] * (b - c * E); see LA-11965-MS, pp 25-30.\r\n" +
                            "For PWR and BWR fuel the recommended values for sigma (c) are 0.005 and 0.01, respectively.";
                        cell.ToolTipText = display;
                        break;
                }
            }
        }

    }
}
