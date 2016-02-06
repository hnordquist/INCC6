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
using System.Linq;
using System.Windows.Forms;
using AnalysisDefs;
namespace NewUI
{
    using NC = NCC.CentralizedState;

    /// <summary>
    /// Implement
    /// </summary>
    public partial class IDDStratumIdDelete : Form
    {

        Detector det;
        List<INCCDB.StratumDescriptor> list;
        public void RefreshCombo()
        {
            list = NC.App.DB.StrataList();
            // Populate the combobox in the selector panel
            StratumIdComboBox.Items.Clear();
            foreach (INCCDB.StratumDescriptor sd in list)
            {
                StratumIdComboBox.Items.Add(sd);
            }
        }
        public IDDStratumIdDelete(Detector d)
        {
            InitializeComponent();
            det = d;
            RefreshCombo();
            if (StratumIdComboBox.Items.Count > 0)
                StratumIdComboBox.SelectedIndex = 0;
            this.Text += " for detector " + det.Id.DetectorName;
        }

        private void StratumIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            //todo: Check that this works as planned hn
            INCCDB.StratumDescriptor ToDelete = list.ElementAt(StratumIdComboBox.SelectedIndex);
            string name = ToDelete.Desc.Name;
            //Check for associated detectors, warn if associated
            DB.Strata strata = new DB.Strata();
            List<string> dets = strata.GetAssociationsByStratum(name);
            if (dets.Count > 0)
                if (MessageBox.Show ("There are detectors associated with this stratum.  Do you still want to delete?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            //check for associated items, warn if associated
            ItemIdListImpl il = new ItemIdListImpl();
            List<ItemId> ItemList = il.GetListByStratumID(name);
            if (ItemList.Count > 0)
                if (MessageBox.Show("There are items associated with this stratum. Do you still want to delete?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            //Check acquire_recs for the stratum id, warn if used
            List<AcquireParameters> acq_with_stratum = new List<AcquireParameters>();
            Dictionary <INCCDB.AcquireSelector, AcquireParameters> all_acq = NC.App.DB.AcquireParametersMap;
            foreach (KeyValuePair<INCCDB.AcquireSelector, AcquireParameters> kv in all_acq)
            {
                if (((AcquireParameters)kv.Value).stratum_id.Name == name)
                    acq_with_stratum.Add((AcquireParameters)kv.Value);
            }
            if (acq_with_stratum.Count > 0)
            {
                if (MessageBox.Show("There are acquisition records associated with this stratum. Do you still want to delete?", "WARNING", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
            //they have chosen to go ahead with the delete, remove detector associations
            foreach (string det_name in dets)
            {
                strata.Unassociate(det_name);
            }
            if (NC.App.DB.DeleteStratum(ToDelete.Desc))
            {
                RefreshCombo();
            }
            else
                MessageBox.Show("Failed to delete stratum from DB", "There was a problem deleting this stratum from the database.", MessageBoxButtons.OKCancel);
            DialogResult = DialogResult.OK;
            // Refreshes global strata list
            list = NC.App.DB.StrataList(det);
            this.Close();
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
