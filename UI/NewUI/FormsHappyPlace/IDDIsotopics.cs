/*
Copyright (c) 2015, Los Alamos National Security, LLC
All rights reserved.
Copyright 2015. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
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

	public partial class IDDIsotopics : Form
    {

        void RefreshIsoCodeCombo()
        {
            IsotopicsSourceCodeComboBox.Items.Clear();
            foreach (Isotopics.SourceCode sc in System.Enum.GetValues(typeof(Isotopics.SourceCode))) // could use the GetOptionType scheme here
            {
                IsotopicsSourceCodeComboBox.Items.Add(sc.ToString());
            }
        }

        void PopulateWithSelectedItem()
        {
            // Fill in the GUI elements with the current values stored in the local data structure
            PuDateTimePicker.Value = m_iso.pu_date;
            AmDateTimePicker.Value = m_iso.am_date;

            Am241ErrorTextBox.Text = m_iso.am241_err.ToString("F6");
            Am241PercentTextBox.Text = m_iso.am241.ToString("F6");

            Pu238PercentTextBox.Text = m_iso.pu238.ToString("F6");
            Pu239PercentTextBox.Text = m_iso.pu239.ToString("F6");
            Pu240PercentTextBox.Text = m_iso.pu240.ToString("F6");
            Pu241PercentTextBox.Text = m_iso.pu241.ToString("F6");
            Pu242PercentTextBox.Text = m_iso.pu242.ToString("F6");

            Pu238ErrorTextBox.Text = m_iso.pu238_err.ToString("F6");
            Pu239ErrorTextBox.Text = m_iso.pu239_err.ToString("F6");
            Pu240ErrorTextBox.Text = m_iso.pu240_err.ToString("F6");
            Pu241ErrorTextBox.Text = m_iso.pu241_err.ToString("F6");
            Pu242ErrorTextBox.Text = m_iso.pu242_err.ToString("F6");

            IsotopicsSourceCodeComboBox.SelectedItem = m_iso.source_code.ToString();
        }

        public IDDIsotopics(string selected = "default")
        {
            InitializeComponent();
            applog = NC.App.Logger(NCCReporter.LMLoggers.AppSection.App);
            RefreshIsoCodeCombo();
            RefreshIdComboWithDefaultOrSet(selected);
        }

        private void IsotopicsIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string newid = (string)cb.SelectedItem;

            try
            {
                if (NC.App.DB.Isotopics.Has(newid))
                    m_iso = NC.App.DB.Isotopics.Get(newid);
            }
            catch (InvalidOperationException)  //?? this was an early experiment that doesn't make sense now
            {
                // iso is a new entry that has not yet been saved to the real list
                m_iso = new Isotopics();
                m_iso.id = cb.Text;
                m_iso.modified = true;
                NC.App.DB.Isotopics.GetList().Add(m_iso);  // add to the in-memory list
                NC.App.DB.Isotopics.Set(m_iso); // add to the database
            }
            PopulateWithSelectedItem(); // Populated with local copy for now
        }

        private void IsotopicsSourceCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            System.Enum.TryParse<Isotopics.SourceCode>((string)cb.SelectedItem, out m_iso.source_code);
            if (string.Compare(m_iso.source_code.ToString(), cb.Text, StringComparison.OrdinalIgnoreCase) != 0)
                modified = m_iso.modified = true;
        }

        private void PuDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (!m_iso.pu_date.Equals(dt))
            {
                modified = m_iso.modified = true;
                m_iso.pu_date = dt;
            }
        }

        private void AmDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (!m_iso.am_date.Equals(dt))
            {
                modified = m_iso.modified = true;
                m_iso.am_date = dt;
            }
        }

        private bool GutCheck()
        {
            double isotopic_sum = 0;
            if (PuPlusAmRadioButton.Checked)//Was set to wrong radio button HN 3.16.2015
            {
                isotopic_sum = m_iso.pu238 + m_iso.pu239 + m_iso.pu240 +
                    m_iso.pu241 + m_iso.pu242 + m_iso.am241;
            }
            else
            {
                isotopic_sum = m_iso.pu238 + m_iso.pu239 + m_iso.pu240 +
                m_iso.pu241 + m_iso.pu242;
            }
            if ((isotopic_sum > isomax) || (isotopic_sum < isomin))
            {
                String s = String.Format("Error: Isotopics total = {0}%\r\nIsotopics total must be within {1}% and {2}%",
                    isotopic_sum, isomin, isomax);
                MessageBox.Show(s, m_iso.id);
                return false;
            }
            return true;
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                if (GutCheck())  // next: does this behavior follow that of INCC5? It is supposed to replicate INCC5 behavior
                    SaveSetBtn_Click(sender, e);
                else
                    return;
                ((Button) sender).DialogResult = DialogResult.OK;
            }
            this.Close();

        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        bool modified = false;

        /// <summary>
        /// Parse text field to as percent (double), 
        /// assign value to current isotopics instance,
        /// format the active text field for user display.
        /// </summary>
        /// <param name="sender">dialog text field event handler</param>
        /// <param name="i">isotopic entry index</param>
        private void IsoPctCheck(TextBox sender, Isotope i)
        {
            m_iso.modified |= Format.ToPct(sender.Text, ref m_iso[i].v);
            sender.Text = m_iso[i].v.ToString("F6");
        }

        /// <summary>
        /// Parse text field to as non-negative double, 
        /// assign value to current isotopics instance,
        /// format the active text field for user display.
        /// </summary>
        /// <param name="sender">dialog text field event handler</param>
        /// <param name="i">isotopic entry index</param>
        private void IsoSigmaNNCheck(TextBox sender, Isotope i)
        {
            m_iso.modified |= Format.ToNN(sender.Text, ref m_iso[i].sigma);
            sender.Text = m_iso[i].sigma.ToString("F6");
        }

        private void Am241PercentTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Am241PercentTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.am241 - temp) < TOLERANCE))
            {
                modified = true;
                IsoPctCheck((TextBox)sender, Isotope.am241);
            }
        }

        private void Pu242PercentTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu242PercentTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu242 - temp) < TOLERANCE))
            {
                modified = true;
                IsoPctCheck((TextBox)sender, Isotope.pu242);
            }
        }

        private void Pu241PercentTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu241PercentTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu241 - temp) < TOLERANCE))
            {
                modified = true;
                IsoPctCheck((TextBox)sender, Isotope.pu241);
            }
        }

        private void Pu240PercentTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu240PercentTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu240 - temp) < TOLERANCE))
            {
                modified = true;
                IsoPctCheck((TextBox)sender, Isotope.pu240);
            }
        }

        private void Pu239PercentTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu239PercentTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu239 - temp) < TOLERANCE))
            {
                modified = true;
                IsoPctCheck((TextBox)sender, Isotope.pu239);
            }
        }

        private void Pu238PercentTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu238PercentTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu238 - temp) < TOLERANCE))
            {
                modified = true;
                IsoPctCheck((TextBox)sender, Isotope.pu238);
            }
        }

        private void Am241ErrorTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Am241ErrorTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.am241_err - temp) < TOLERANCE))
            {
                modified = true;
                IsoSigmaNNCheck((TextBox)sender, Isotope.am241);
            }
        }

        private void Pu242ErrorTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu242ErrorTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu242_err - temp) < TOLERANCE))
            {
                modified = true;
                IsoSigmaNNCheck((TextBox)sender, Isotope.pu242);
            }
        }

        private void Pu241ErrorTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu241ErrorTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu241_err - temp) < TOLERANCE))
            {
                modified = true;
                IsoSigmaNNCheck((TextBox)sender, Isotope.pu241);
            }
        }

        private void Pu240ErrorTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu240ErrorTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu240_err - temp) < TOLERANCE))
            {
                modified = true;
                IsoSigmaNNCheck((TextBox)sender, Isotope.pu240);
            }
        }

        private void Pu238ErrorTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu238ErrorTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu238_err - temp) < TOLERANCE))
            {
                modified = true;
                IsoSigmaNNCheck((TextBox)sender, Isotope.pu238);
            }
        }

        private void Pu239ErrorTextBox_Leave(object sender, EventArgs e)
        {
            double temp;
            Double.TryParse(Pu239ErrorTextBox.Text, out temp);
            if (!(Math.Abs(m_iso.pu239_err - temp) < TOLERANCE))
            {
                modified = true;
                IsoSigmaNNCheck((TextBox)sender, Isotope.pu239);
            }
        }


        private void ReadFromFileBtn_Click(object sender, EventArgs e)
        {
			OpenFileDialog RestoreFileDialog = new OpenFileDialog();
            List<string> paths = new List<string>();
            RestoreFileDialog.CheckFileExists = false;
			RestoreFileDialog.Filter = "NCC isotopic files (*.*)|*.*";
            RestoreFileDialog.InitialDirectory = NC.App.AppContext.FileInput;
            RestoreFileDialog.Title = "Select an isotopics file";
            RestoreFileDialog.Multiselect = false;
            RestoreFileDialog.RestoreDirectory = true;
            DialogResult r = DialogResult.No;
            r = RestoreFileDialog.ShowDialog();
            if (r != DialogResult.OK)
				return;            
			NCCFile.IsoFiles possibleIsoAndCompIsoFilesToAttemptProcessingUpon = new NCCFile.IsoFiles();
			possibleIsoAndCompIsoFilesToAttemptProcessingUpon.Process(new List<string>(RestoreFileDialog.FileNames));
			foreach (Isotopics iso in possibleIsoAndCompIsoFilesToAttemptProcessingUpon.IsoIsotopics) // add all new values into the database
			{
				iso.modified = true;
				if (NC.App.DB.Isotopics.Set(iso) >= 0)
				{
					applog.TraceInformation("'" + iso.id + "' isotopics updated/added");
				}
			}
			int count = possibleIsoAndCompIsoFilesToAttemptProcessingUpon.IsoIsotopics.Count;
			if (count > 0)
			{
				NC.App.DB.Isotopics.Refresh();  // update isotopics in-memory list from the freshly updated database 
				RefreshIdComboWithDefaultOrSet(possibleIsoAndCompIsoFilesToAttemptProcessingUpon.IsoIsotopics[count-1].id);  // make the last read iso the currrent iso
			}
         }

        private void WriteToFileBtn_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NYI", "NOT IMPLEMENTED....YET");
        }

        private void AddNewSetBtn_Click(object sender, EventArgs e)
        {
            IDDIsotopicsAdd ia = new IDDIsotopicsAdd();
            if (ia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
				if (string.IsNullOrEmpty(ia.ID))
					return;

				// check for Id existence 
				if (!NC.App.DB.Isotopics.Has(ia.ID))
				{
                    Isotopics iso = new Isotopics(m_iso); // copy current values to new iso
                    iso.id = String.Copy(ia.ID);
                    modified = iso.modified = true;
                    NC.App.DB.Isotopics.Revert(m_iso);  // revert originating selection on in-memory list back to DB values

					//PopulateWithSelectedItem();
                    NC.App.DB.Isotopics.GetList().Add(iso);				
					// do not add to database until the OK or the Save set button is selected NC.App.DB.Isotopics.Set(m_iso);
                    applog.TraceInformation("New isotopics " + iso.id + " (not saved to database)");
                    IsotopicsIdComboBox.Items.Add(iso.id);
                    IsotopicsIdComboBox.SelectedItem = iso.id;  // force m_iso assignment in event handler
				}
				else
					MessageBox.Show("'" + ia.ID + "' is already in use", "I .... uuuh oh");


            }
        }

        private void SaveSetBtn_Click(object sender, EventArgs e)
        {
            if (!GutCheck())
            {
                // it's all ready the current iso m_iso = NC.App.DB.Isotopics.Get(x => 0 == string.Compare(x.id, m_iso.id, StringComparison.OrdinalIgnoreCase));
                PopulateWithSelectedItem();
                return;
            }
            else
            {
                // save the current state 
                if (modified)
                {
                    if (!NC.App.DB.Isotopics.Has(m_iso))
                        NC.App.DB.Isotopics.GetList().Add(m_iso); 	// add to in-memory list 
                    List<Isotopics> list = NC.App.DB.Isotopics.GetMatch(i => i.modified);
                    foreach (Isotopics iso in list)
                    {
                        long pk = NC.App.DB.Isotopics.Set(iso);				// add to database 
                        applog.TraceInformation((pk >= 0 ? "Saved " : "Unable to save ") + iso.id + " isotopics");
                    }
                    modified = false;
                }
            }
            RefreshIdComboWithDefaultOrSet(m_iso.id);

        }

        private void EditIdBtn_Click(object sender, EventArgs e)
        {
            IDDIsotopicsEdit ia = new IDDIsotopicsEdit(m_iso.id);
            if (ia.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string oldId = m_iso.id;
                m_iso.id = String.Copy(ia.NewID);  // changes the id on the object on the list - m_iso is a reference to an element in the in-memory list
                if (NC.App.DB.Isotopics.Rename(oldId, ia.NewID))
                {
                    IsotopicsIdComboBox.Items.Remove(oldId);
                    IsotopicsIdComboBox.Items.Add(m_iso.id);
                    applog.TraceInformation("Renamed " + oldId + " to " + m_iso.id);
                    IsotopicsIdComboBox.SelectedItem = m_iso.id;
                    //PopulateWithSelectedItem();
                }
            }
        }

        private void DeleteSetBtn_Click(object sender, EventArgs e)
        {
            String s = String.Format("Delete {0} isotopics data set?", m_iso.id);
            DialogResult r = MessageBox.Show(s, NC.App.Name, MessageBoxButtons.YesNo);
            if (r == System.Windows.Forms.DialogResult.Yes)
            {
                if (NC.App.DB.Isotopics.Delete(m_iso)) // deletes from in-memory list and database
                {
                    applog.TraceInformation("Deleted " + m_iso.id + " isotopics");
                    RefreshIdComboWithDefaultOrSet();
                }
                else
                    applog.TraceInformation("Unable to delete " + m_iso.id + " isotopics");

            }
        }

        private void RefreshIdComboWithDefaultOrSet(string id="default")
        {
            IsotopicsIdComboBox.Items.Clear();

			// get the in-memory list
			List<Isotopics> isolist = NC.App.DB.Isotopics.GetList();

            if (isolist.Count > 0)  // look for id on the list
            {
                //This never loaded all the isotopics before?? hn 4.30.2015
                foreach (Isotopics tope in isolist)
                {
                    IsotopicsIdComboBox.Items.Add(tope.id);
                }
                m_iso = isolist.Find(i => string.Equals(i.id, id, StringComparison.OrdinalIgnoreCase));
            }
            else // Should never hit here, happens if DB is blank. Add a single "Default"
            {
                m_iso = new Isotopics();
                m_iso.modified = true;
				isolist.Add(m_iso); // add to in-memory list
                NC.App.DB.Isotopics.Set(m_iso);  // set in database
            }

            PuPlusAmRadioButton.Checked = false;
            AmPercentOfPuRadioButton.Checked = true;
            IsotopicsIdComboBox.SelectedItem = m_iso.id;  // forces event handler to load dialog fields
        }

        public Isotopics GetSelectedIsotopics { get { return m_iso; } }

		Isotopics m_iso;
        NCCReporter.LMLoggers.LognLM applog;

        const double isomin = 99.7;
        const double isomax = 100.3;
        const double TOLERANCE = .00001;

    }
}
