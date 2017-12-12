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
using System.Windows.Forms;
using System.Collections.Generic;
using AnalysisDefs;

namespace UI
{
    using NC = NCC.CentralizedState;
    using Integ = NCC.IntegrationHelpers;
    public partial class IDDCollarItemData : Form
    {
        List<CollarItemId> collarItems = new List<CollarItemId>();
        AcquireParameters ap = Integ.GetCurrentAcquireParams();
        CollarItemId cur;
        public IDDCollarItemData()
        {
            InitializeComponent();
            this.Text = String.Format("Enter Collar Item Data for Item ID: {0}", ap.item_id);
            collarItems = NC.App.DB.CollarItemIds.GetList();
            foreach (poison_rod_type_rec pr in NC.App.DB.PoisonRods.GetList())
                PoisonRodTypeComboBox.Items.Add(pr.rod_type);
            
            PoisonRodTypeComboBox.SelectedIndex = 0;
            //Format numbers
            LengthTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            LengthErrorTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            TotalU235TextBox.NumberFormat = NumericTextBox.Formatter.F3;
            TotalU235ErrorTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            TotalU238TextBox.NumberFormat = NumericTextBox.Formatter.F3;
            TotalU238ErrorTextBox.NumberFormat = NumericTextBox.Formatter.F3;
            TotalPoisonRodsTextBox.NumStyles = System.Globalization.NumberStyles.Integer;
            TotalPoisonRodsTextBox.NumberFormat = NumericTextBox.Formatter.N3;
            TotalRodsTextBox.NumStyles = System.Globalization.NumberStyles.Integer;
            TotalRodsTextBox.NumberFormat = NumericTextBox.Formatter.N3;
            PoisonTextBox.NumberFormat = NumericTextBox.Formatter.P1;
            PoisonErrorTextBox.NumberFormat = NumericTextBox.Formatter.P1;

            cur = collarItems.Find(x => x.item_id == ap.item_id);
            if (cur != null)
            {
                // Set the rod type to match if it doesn't
                PoisonRodTypeComboBox.SelectedIndex = PoisonRodTypeComboBox.FindStringExact(cur.rod_type);
                LengthTextBox.Value = cur.length.v;
                LengthErrorTextBox.Value = cur.length.err;
                TotalU235TextBox.Value = cur.total_u235.v;
                TotalU235ErrorTextBox.Value = cur.total_u235.err;
                TotalU238TextBox.Value = cur.total_u238.v;
                TotalU238ErrorTextBox.Value = cur.total_u238.err;
                TotalPoisonRodsTextBox.Value = cur.total_poison_rods;
                TotalRodsTextBox.Value = cur.total_rods;
                PoisonTextBox.Value = cur.poison_percent.v;
                PoisonErrorTextBox.Value = cur.poison_percent.err;
            }
            else
            {
                cur = new CollarItemId(ap.item_id);
                cur.rod_type = PoisonRodTypeComboBox.SelectedItem.ToString();             
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            cur.length.v = LengthTextBox.Value;
            cur.length.err = LengthErrorTextBox.Value;
            cur.total_poison_rods = TotalPoisonRodsTextBox.Value;
            cur.total_rods = TotalRodsTextBox.Value;
            cur.total_u235.v = TotalU235TextBox.Value;
            cur.total_u235.err = TotalU238ErrorTextBox.Value;
            cur.total_u238.v = TotalU238TextBox.Value;
            cur.total_u238.err = TotalU238ErrorTextBox.Value;
            cur.length.err = LengthErrorTextBox.Value;
            cur.poison_percent.v = PoisonTextBox.Value;
            cur.poison_percent.err = PoisonErrorTextBox.Value;
            if (collarItems.Find(x => x.item_id == ap.item_id) == null)
            {
                cur.modified = true;
                //It wasn't there, add to the list.
                collarItems.Add(cur);
            }
            else
                cur.modified = cur.CompareTo(NC.App.DB.CollarItemIds.GetList().Find(x => x.item_id == cur.item_id)) != 0;
            //DialogResult = NC.App.DB.CollarItemIds.SetList(collarItems)!= -1?DialogResult.OK:DialogResult.None;
            //Not sure what this logic was, but it was bad. HN 6.14.2017
            DialogResult = DialogResult.OK;

            Close();
        }

        private void PoisonRodTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
