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
using System.Windows.Forms;
using AnalysisDefs;
namespace NewUI
{
	using NC = NCC.CentralizedState;

	public partial class IDDCompositeIsotopics : Form
    {
        public IDDCompositeIsotopics()
        {
            InitializeComponent();
            RefreshIsoCodeCombo();
            RefreshIdComboWithDefault();
            applog = NC.App.Logger(NCCReporter.LMLoggers.AppSection.App);
        }

        private void CalculateBtn_Click(object sender, EventArgs e)
        {

        }

        private void IsotopicsIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ReferenceDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (!m_comp_iso.pu_date.Equals(dt))
            {
                modified = m_comp_iso.modified = true;
                m_comp_iso.pu_date = dt;
            }
        }

        private void IsoSrcCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ReadBtn_Click(object sender, EventArgs e)
        {
			OpenFileDialog RestoreFileDialog = new OpenFileDialog();
            List<string> paths = new List<string>();
            RestoreFileDialog.CheckFileExists = false;
			RestoreFileDialog.Filter = "all files (*.*)|*.*";
            RestoreFileDialog.InitialDirectory = NC.App.AppContext.FileInput;
            RestoreFileDialog.Title = "Select a composite isotopics file";
            RestoreFileDialog.Multiselect = false;
            RestoreFileDialog.RestoreDirectory = true;
            DialogResult r = DialogResult.No;
            r = RestoreFileDialog.ShowDialog();
            if (r != DialogResult.OK)
				return;            
			NCCFile.IsoFiles possibleCompIsoFilesToAttemptProcessingUpon = new NCCFile.IsoFiles();
			possibleCompIsoFilesToAttemptProcessingUpon.Process(new List<string>(RestoreFileDialog.FileNames));
			foreach (CompositeIsotopics iso in possibleCompIsoFilesToAttemptProcessingUpon.Results.CompIsoIsotopics) // add all new values into the database
			{
				iso.modified = true;
				if (NC.App.DB.CompositeIsotopics.Set(iso) >= 0)
				{
					applog.TraceInformation("'" + iso.id + "' composite isotopics updated/added");
				}
			}
			int count = possibleCompIsoFilesToAttemptProcessingUpon.Results.CompIsoIsotopics.Count;
			if (count > 0)
			{
				NC.App.DB.Isotopics.Refresh();  // update isotopics in-memory list from the freshly updated database 
				RefreshIdComboWithDefault(possibleCompIsoFilesToAttemptProcessingUpon.Results.CompIsoIsotopics[count-1].id);  // make the last read iso the current iso
			}
        }

        private void WriteBtn_Click(object sender, EventArgs e)
        {

        }

        private void AddBtn_Click(object sender, EventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, EventArgs e)
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void RefreshIsoIDCombo(ComboBox box)
        {
            box.Items.Clear();
            foreach (CompositeIsotopics i in NC.App.DB.CompositeIsotopics.GetList())
            {
                box.Items.Add(i.id);
            }
        }
        void RefreshIsoCodeCombo()
        {
            IsoSrcCodeComboBox.Items.Clear();
            foreach (CompositeIsotopics.SourceCode sc in System.Enum.GetValues(typeof(CompositeIsotopics.SourceCode))) // could use the GetOptionType scheme here
            {
                IsoSrcCodeComboBox.Items.Add(sc.ToString());
            }
        }
		private void RefreshIdComboWithDefault(string id = "default")
		{
			IsotopicsIdComboBox.Items.Clear();

			// get the in-memory list
			List<CompositeIsotopics> isolist = NC.App.DB.CompositeIsotopics.GetList();

			if (isolist.Count > 0)  // look for id on the list
			{
				foreach (CompositeIsotopics tope in isolist)
				{
					IsotopicsIdComboBox.Items.Add(tope.id);
				}
				m_comp_iso = isolist.Find(i => string.Equals(i.id, id, StringComparison.OrdinalIgnoreCase));
			} 
			else // Should never hit here, happens if DB is blank. Add a single "Default"
			{
				m_comp_iso = new CompositeIsotopics();
				m_comp_iso.modified = true;
				isolist.Add(m_comp_iso); // add to in-memory list
				NC.App.DB.CompositeIsotopics.Set(m_comp_iso);  // set in database
			}
			IsotopicsIdComboBox.SelectedItem = m_comp_iso.id;  // forces event handler to load dialog fields
		}

		public CompositeIsotopics GetSelectedIsotopics { get { return m_comp_iso; } }

        CompositeIsotopics m_comp_iso;
        const double isomin = 99.7;
        const double isomax = 100.3;
        const double TOLERANCE = .00001;
		NCCReporter.LMLoggers.LognLM applog;
		bool modified = false;
		
    }
}
