﻿/*
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
using AnalysisDefs;
using DetectorDefs;
namespace UI
{
    using NC = NCC.CentralizedState;

    public partial class IDDAcquireHoldup : Form
    {
        AcquireHandlers ah;

        public IDDAcquireHoldup()
        {
            InitializeComponent();

            // Generate an instance of the generic acquire dialog event handlers object (this now includes the AcquireParameters object used for change tracking)
            ah = new AcquireHandlers();
            ah.mo = AssaySelector.MeasurementOption.holdup;
            this.Text += " for detector " + ah.det.Id.DetectorName;


            // Populate the UI fields with values from the local AcquireParameters object
            this.QCTestsCheckBox.Checked = ah.ap.qc_tests;
            this.PrintResultsCheckBox.Checked = ah.ap.print;
            this.CommentAtEndCheckBox.Checked = ah.ap.ending_comment;
            this.NumCyclesTextBox.Text = Format.Rend(ah.ap.num_runs);
            this.CommentTextBox.Text = ah.ap.comment;
            this.CountTimeTextBox.Text = Format.Rend(ah.ap.run_count_time);
            this.DeclaredMassTextBox.Text = Format.Rend(ah.ap.mass);
            this.GloveboxIdComboBox.Items.Clear();
            GloveboxIdComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.InvChangeCodes.GetList())
            {
                this.GloveboxIdComboBox.Items.Add(desc.Name);
            }
  
            foreach (INCCDB.Descriptor desc in NC.App.DB.MBAs.GetList())
            {
                MBAComboBox.Items.Add(desc.Name);
            }
            this.ItemIdComboBox.Items.Clear();
            foreach (ItemId id in NC.App.DB.ItemIds.GetList())
            {
                ItemIdComboBox.Items.Add(id.item);
            }
            this.StratumIdComboBox.Items.Clear();
            //foreach (INCCDB.StratumDescriptor desc in NC.App.DB.StrataList(ah.det))
            //{
            //    StratumIdComboBox.Items.Add(desc.Desc);
            //}
            foreach (INCCDB.Descriptor desc in NC.App.DB.Stratums.GetList())
            {
                StratumIdComboBox.Items.Add(desc.Name);
            }
            this.MaterialTypeComboBox.Items.Clear();
            foreach (INCCDB.Descriptor desc in NC.App.DB.Materials.GetList())
            {
                MaterialTypeComboBox.Items.Add(desc.Name);
            }

            this.DataSourceComboBox.Items.Clear();
            foreach (ConstructedSource cs in Enum.GetValues(typeof(ConstructedSource)))
            {
                if (cs.AcquireChoices() || cs.LMFiles(ah.det.Id.SRType))
                    DataSourceComboBox.Items.Add(cs.NameForViewing(ah.det.Id.SRType));
            }
            DataSourceComboBox.SelectedItem = ah.ap.data_src.NameForViewing(ah.det.Id.SRType);
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (ah.OKButton_Click(sender, e) == System.Windows.Forms.DialogResult.OK)
            {
                //user can cancel in here during LM set-up, account for it.
                this.Visible = false;
                UIIntegration.Controller.SetAssay();  // tell the controller to do an assay operation using the current measurement state
                UIIntegration.Controller.Perform();  // start the measurement file or DAQ thread
                this.Close();
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {
            //  TODO:  Peace on Earth, goodwill towards manatees.
        }

        private void GloveboxIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.GloveboxIdComboBox_SelectedIndexChanged(sender, e);
        }

        private void MBAComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.MBAComboBox_SelectedIndexChanged(sender, e);
        }

        private void ItemIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.ItemIdComboBox_SelectedIndexChanged(sender, e);
        }

        private void StratumIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.StratumIdComboBox_SelectedIndexChanged(sender, e);
        }

        private void MaterialTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.MaterialTypeComboBox_SelectedIndexChanged(sender, e);
        }

        private void DataSourceComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ah.DataSourceComboBox_SelectedIndexChanged(sender, e);
        }

        private void QCTestsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ah.QCTestsCheckbox_CheckedChanged(sender, e);
        }

        private void PrintResultsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ah.PrintResultsCheckbox_CheckedChanged(sender, e);
        }

        private void CommentAtEndCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ah.CommentCheckbox_CheckedChanged(sender, e);
        }

        private void IsotopicsBtn_Click(object sender, EventArgs e)
        {
            // TODO:  Add button click handler
        }

        private void CompositeIsotopicsBtn_Click(object sender, EventArgs e)
        {
            // TODO:  Add button click handler
        }

        private void Pu240eCoeffBtn_Click(object sender, EventArgs e)
        {
            // TODO:  Add button click handler
        }

        private void DeclaredMassTextBox_Leave(object sender, EventArgs e)
        {
            ah.DeclaredMassTextBox_Leave(sender, e);
        }

        private void CommentTextBox_Leave(object sender, EventArgs e)
        {
            ah.CommentTextBox_Leave(sender, e);
        }

        private void CountTimeTextBox_Leave(object sender, EventArgs e)
        {
            ah.CountTimeTextBox_Leave(sender, e);
        }

        private void NumCyclesTextBox_Leave(object sender, EventArgs e)
        {
            ah.NumCyclesTextBox_Leave(sender, e);
        }

    }
}
