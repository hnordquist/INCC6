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
namespace UI
{
    using NC = NCC.CentralizedState;

    public partial class IDDPoisonRodTypeAdd : Form
    {

        poison_rod_type_rec model;

        public IDDPoisonRodTypeAdd()
        {
            InitializeComponent();
            model = new poison_rod_type_rec();
            RefreshCombo(pick:false);
        }


        void RefreshCombo(bool pick)
        {
            // Populate the combobox in the selector panel
            CurrentPoisonRodTypesComboBox.Items.Clear();
            foreach (poison_rod_type_rec p in NC.App.DB.PoisonRods.GetList())
            {
                CurrentPoisonRodTypesComboBox.Items.Add(p.rod_type);
            }
            if (pick)
                CurrentPoisonRodTypesComboBox.SelectedItem = model.rod_type;
        }
        private void CurrentPoisonRodTypesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            model = new poison_rod_type_rec(NC.App.DB.PoisonRods.Get((string)CurrentPoisonRodTypesComboBox.SelectedItem));
            DefaultPoisonAbsorptionTextBox.Text = model.absorption_factor.ToString("F3");
            PoisonRodTypeTextBox.Text = model.rod_type;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(PoisonRodTypeTextBox.Text))
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            if (model.modified)
            {
                if (NC.App.DB.PoisonRods.Has(model))  // it is already there, nothing to do, user must select cancel (INCC5-style) to close
                {
                DialogResult = DialogResult.Cancel;
                    return;
                }
                else
                {
                    NC.App.DB.PoisonRods.Set(model);
                    NC.App.DB.PoisonRods.Refresh();
                    DialogResult = DialogResult.OK;
                    RefreshCombo(pick:true);
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

        private void DefaultPoisonAbsorptionTextBox_Leave(object sender, EventArgs e)
        { 
            if (model != null)
            {
                double d = model.absorption_factor;
                bool modified = Format.ToDbl(((TextBox)sender).Text, ref d);
                if (modified) { model.absorption_factor = d; model.modified = true; }
                ((TextBox)sender).Text = model.absorption_factor.ToString("F3");
		    }
        }

		private void PoisonRodTypeTextBox_Leave(object sender, EventArgs e)
		{
            if (model != null && !model.rod_type.Equals(((TextBox)sender).Text))
            {
                model.rod_type = ((TextBox)sender).Text;
                model.modified = true;
            }
		}
	}
}
