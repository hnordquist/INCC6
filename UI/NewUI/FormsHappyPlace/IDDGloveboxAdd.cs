/*
Copyright (c) 2016, Los Alamos National Security, LLC
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
namespace NewUI
{
	using NC = NCC.CentralizedState;
	public partial class IDDGloveboxAdd : Form
    {
        public IDDGloveboxAdd()
        {
            InitializeComponent();
            model = new holdup_config_rec();
			RefreshHCCombo(pick:false);
        }
		
        void RefreshHCCombo(bool pick)
        {
            // Populate the combobox in the selector panel
            CurrentGloveboxIdsComboBox.Items.Clear();
			List<holdup_config_rec> list = NC.App.DB.HoldupConfigParameters.GetList();  
            foreach (holdup_config_rec hc in list)
            {
				CurrentGloveboxIdsComboBox.Items.Add(hc.glovebox_id);  // add strings
            }
            if (pick)
                CurrentGloveboxIdsComboBox.SelectedItem = model.glovebox_id; // yeah 

        }
        holdup_config_rec model;
        private void CurrentGloveboxIdsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model = new holdup_config_rec(NC.App.DB.HoldupConfigParameters.Get((string)CurrentGloveboxIdsComboBox.SelectedItem)); // expect and use strings
            GloveboxIdTextBox.Text = model.glovebox_id;
            NumRowsTextBox.Text = model.num_rows.ToString();
            NumColsTextBox.Text = model.num_columns.ToString();
            DistanceTextBox.Text = model.distance.ToString("F1");
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(GloveboxIdTextBox.Text))
            {
                DialogResult = DialogResult.Cancel;
                return;
            } 
            if (NC.App.DB.HoldupConfigParameters.Has(model))  // it is already there, nothing to do, user must select cancel (INCC5-style) to close
            {
                return;
            }
            else
            {
                NC.App.DB.HoldupConfigParameters.Set(model);
                NC.App.DB.HoldupConfigParameters.Refresh();
                DialogResult = DialogResult.OK;
                RefreshHCCombo(pick:true);
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void GloveboxIdTextBox_Leave(object sender, EventArgs e)
        {
            if (!model.glovebox_id.Equals(((TextBox)sender).Text))
            {
                model.glovebox_id = ((TextBox)sender).Text;
                model.modified = true;
            }
        }

        private void NumRowsTextBox_Leave(object sender, EventArgs e)
        {
            ushort u = model.num_rows;
            model.modified = (Format.ToUShort(((TextBox)sender).Text, ref u));
            if (model.modified) { model.num_rows = u; }
            ((TextBox)sender).Text = model.num_rows.ToString();
        }

        private void NumColsTextBox_Leave(object sender, EventArgs e)
        {
            ushort u = model.num_columns;
            model.modified = (Format.ToUShort(((TextBox)sender).Text, ref u));
            if (model.modified) { model.num_columns = u; }
            ((TextBox)sender).Text = model.num_columns.ToString();
        }

        private void DistanceTextBox_Leave(object sender, EventArgs e)
        {
            double d = model.distance;
            model.modified = (Format.ToDblBracket(((TextBox)sender).Text, ref d, 0.0, 1000000.0));   // lol
            if (model.modified) { model.distance = d; }
            ((TextBox)sender).Text = model.distance.ToString("F1");
        }
    }
}
