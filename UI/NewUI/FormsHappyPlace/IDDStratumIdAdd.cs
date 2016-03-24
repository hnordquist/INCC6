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
namespace NewUI
{

    using NC = NCC.CentralizedState;

    public partial class IDDStratumIdAdd : Form
    {
        INCCDB.StratumDescriptor st;
        Detector det;
        List<INCCDB.StratumDescriptor> list;
        void RefreshCombo()
        {
            list = NC.App.DB.StrataList(det);
            // Populate the combobox in the selector panel
            CurrentStratumIdsComboBox.Items.Clear();            
            foreach (INCCDB.StratumDescriptor sd in list)
            {
                CurrentStratumIdsComboBox.Items.Add(sd);
            }
        }

        public IDDStratumIdAdd(Detector d)
        {
            det = d;
            InitializeComponent();
            RefreshCombo();
            if (CurrentStratumIdsComboBox.Items.Count > 0)
                CurrentStratumIdsComboBox.SelectedIndex = 0;
            else
                st = new INCCDB.StratumDescriptor();
            this.Text += " for detector " + det.Id.DetectorName;
        }

        private void CurrentStratumIdsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            INCCDB.StratumDescriptor lst = (INCCDB.StratumDescriptor)CurrentStratumIdsComboBox.SelectedItem;
            st = new INCCDB.StratumDescriptor(lst);
            StratumIdTextBox.Text = st.Item1.Name;
            StratumIdDescTextBox.Text = st.Item1.Desc;
            HistoricalBiasTextBox.Text = st.Item2.bias_uncertainty.ToString("F3");
            RandomUncertaintyTextBox.Text = st.Item2.random_uncertainty.ToString("F3");
            SystematicUncertaintyTextBox.Text = st.Item2.systematic_uncertainty.ToString("F3");
        }

        private void StratumIdTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void StratumIdDescTextBox_Leave(object sender, EventArgs e)
        {

        }

        private void HistoricalBiasTextBox_Leave(object sender, EventArgs e)
        {
            double d = st.Item2.bias_uncertainty;
            bool b = Format.ToNN(((TextBox)sender).Text, ref d);
            ((TextBox)sender).Text = d.ToString("F3");
            st.Item2.modified |= b;
            if (b) st.Item2.bias_uncertainty = d;
        }

        private void RandomUncertaintyTextBox_Leave(object sender, EventArgs e)
        {
            double d = st.Item2.random_uncertainty;
            bool b = Format.ToNN(((TextBox)sender).Text, ref d);
            ((TextBox)sender).Text = d.ToString("F3");
            st.Item2.modified |= b;
            if (b) st.Item2.random_uncertainty = d;
        }

        private void SystematicUncertaintyTextBox_Leave(object sender, EventArgs e)
        {
            double d = st.Item2.systematic_uncertainty;
            bool b = Format.ToNN(((TextBox)sender).Text, ref d);
            ((TextBox)sender).Text = d.ToString("F3");
            st.Item2.modified |= b;
            if (b) st.Item2.systematic_uncertainty = d;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            INCCDB.Descriptor candidate = new INCCDB.Descriptor(StratumIdTextBox.Text, StratumIdDescTextBox.Text);
            st = new INCCDB.StratumDescriptor(candidate, st.Stratum);
            INCCDB.StratumDescriptor realst = (INCCDB.StratumDescriptor)CurrentStratumIdsComboBox.SelectedItem;
            if (realst != null && st.CompareTo(realst) != 0)
            {
                if (list.Exists(i => { return st.Desc.Name.CompareTo(i.Desc.Name) == 0; }))
                    return;

                st.Desc.modified = true;
                NC.App.DB.UpdateStratum(st.Desc, st.Stratum);  // creates it
                NC.App.DB.AssociateStratum(det, st.Desc, st.Stratum); // associates it with the detector
                RefreshCombo();
                INCCDB.StratumDescriptor newst = list.Find(i => { return st.Desc.Name.CompareTo(i.Desc.Name) == 0; });
                CurrentStratumIdsComboBox.SelectedItem = newst;
            }
            else
            {
                // It is brand spanking new.....
                st.Desc.modified = true;
                NC.App.DB.UpdateStratum(st.Desc, st.Stratum);  // creates it
                NC.App.DB.AssociateStratum(det, st.Desc, st.Stratum); // associates it with the detector
                RefreshCombo();
                INCCDB.StratumDescriptor newst = list.Find(i => { return st.Desc.Name.CompareTo(i.Desc.Name) == 0; });
                CurrentStratumIdsComboBox.SelectedItem = newst;
            }
            // Refreshes Strata List after add.
            list = NC.App.DB.StrataList(det);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

    }
}
