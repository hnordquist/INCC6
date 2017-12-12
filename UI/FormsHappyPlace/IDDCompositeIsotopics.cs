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
namespace UI
{
    using NC = NCC.CentralizedState;
    using Integ = NCC.IntegrationHelpers;

    public partial class IDDCompositeIsotopics : Form
    {

        public IDDCompositeIsotopics(string selected)
        {
            InitializeComponent();
            RefreshIsoCodeCombo();
            acq = Integ.GetCurrentAcquireParams();
            if (string.IsNullOrEmpty(selected))
            {
                // get current acquire composite isotopics id and use that
                selected = acq.comp_isotopics_id;                  
            }    
            RefreshIdComboWithDefault(selected);     
        }


		void LoadEntry(CompositeIsotopics comp_iso)
		{
            DataGridViewRowCollection rows = this.IsoDataGrid.Rows;
			rows.Clear();
            string[] summ = new string[9];
			summ[0] = comp_iso.pu_mass.ToString("F3");
			summ[1] = comp_iso.pu238.ToString("F6");
			summ[2] = comp_iso.pu239.ToString("F6");
			summ[3] = comp_iso.pu240.ToString("F6");
			summ[4] = comp_iso.pu241.ToString("F6");
			summ[5] = comp_iso.pu242.ToString("F6");
			summ[6] = comp_iso.pu_date.ToString("yyyy-MM-dd");
			summ[7] = comp_iso.am241.ToString("F6");
			summ[8] = comp_iso.am_date.ToString("yyyy-MM-dd");
            rows.Add(summ);

			foreach(CompositeIsotopic ci in comp_iso.isotopicComponents)
			{
				string[] sub = new string[9];
				sub[0] = ci.pu_mass.ToString("F3");
				sub[1] = ci.pu238.ToString("F6");
				sub[2] = ci.pu239.ToString("F6");
				sub[3] = ci.pu240.ToString("F6");
				sub[4] = ci.pu241.ToString("F6");
				sub[5] = ci.pu242.ToString("F6");
				sub[6] = ci.pu_date.ToString("yyyy-MM-dd");
				sub[7] = ci.am241.ToString("F6");
				sub[8] = ci.am_date.ToString("yyyy-MM-dd");
				rows.Add(sub);
			}

		}


		void PopulateWithSelectedItem()
        {
            // Fill in the GUI elements with the current values stored in the local data structure
            ReferenceDateTimePicker.Value = m_comp_iso.ref_date;
            IsoSrcCodeComboBox.SelectedItem = m_comp_iso.source_code.ToString();
			LoadEntry(m_comp_iso);
        }

        private void IsotopicsIdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
			ComboBox cb = (ComboBox)sender;
            string newid = (string)cb.SelectedItem;
			if (NC.App.DB.CompositeIsotopics.Has(newid))
				m_comp_iso = NC.App.DB.CompositeIsotopics.Get(newid);            
			PopulateWithSelectedItem();
        }

        private void ReferenceDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (!m_comp_iso.ref_date.Equals(dt))
            {
                modified = m_comp_iso.modified = true;
                m_comp_iso.ref_date = dt;
            }
        }

        private void IsoSrcCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            Enum.TryParse((string)cb.SelectedItem, out m_comp_iso.source_code);
            if (string.Compare(m_comp_iso.source_code.ToString(), cb.Text, StringComparison.OrdinalIgnoreCase) != 0)
                modified = m_comp_iso.modified = true;
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
				if (NC.App.DB.CompositeIsotopics.Has(iso.id))
				{
					MessageBox.Show("Isotopics id " + iso.id + " already exists.\r\nNew isotopics data set not created.", "Hallo");
					continue;
				}
				CalculateAndPersist(iso);
                iso.modified = true;
                long key = -1;
                if ((key = NC.App.DB.CompositeIsotopics.Set(iso)) > 0)
                {
                    NC.App.AppLogger.TraceInformation("'" + iso.id + "' composite isotopics added");
                }
            }
            int count = possibleCompIsoFilesToAttemptProcessingUpon.Results.CompIsoIsotopics.Count;
            if (count > 0)
            {
                NC.App.DB.CompositeIsotopics.Refresh();  // update isotopics in-memory list from the freshly updated database 
                RefreshIdComboWithDefault(possibleCompIsoFilesToAttemptProcessingUpon.Results.CompIsoIsotopics[count - 1].id);  // make the last read iso the current iso
            }
        }

        private void WriteBtn_Click(object sender, EventArgs e)
        {
            string id = "Default";
            if (IsotopicsIdComboBox.SelectedItem != null)
                id = IsotopicsIdComboBox.SelectedItem.ToString();
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Isotopics files (.csv)|*.csv|                 (.txt)| *.txt";
            dlg.DefaultExt = ".csv";
            dlg.FileName = id + ".csv";
            dlg.InitialDirectory = NC.App.AppContext.ResultsFilePath;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                NCCFile.INCC5FileExportUtils x = new NCCFile.INCC5FileExportUtils();
                x.Output.Filename = dlg.FileName;
                x.CompIsoIsotopics = m_comp_iso;
                x.ProcessCompositeIsotopicsToFile();
            } 
        }
   

    private void AddBtn_Click(object sender, EventArgs e)
        {
            IDDIsotopicsAdd ia = new IDDIsotopicsAdd();
            if (ia.ShowDialog() == DialogResult.OK)
            {
				if (string.IsNullOrEmpty(ia.ID))
					return;

				// check for Id existence 
				if (!NC.App.DB.CompositeIsotopics.Has(ia.ID))
				{
                    CompositeIsotopics iso = new CompositeIsotopics(m_comp_iso); // copy current values to new iso
                    iso.id = string.Copy(ia.ID);
                    modified = iso.modified = true;
                    NC.App.DB.CompositeIsotopics.Revert(iso);  // revert originating selection on in-memory list back to DB values
                    NC.App.DB.CompositeIsotopics.GetList().Add(iso);				
					// do not add to database until the OK or the Save set button is selected
                    NC.App.AppLogger.TraceInformation("New composite isotopics " + iso.id + " (not saved to database)");
                    IsotopicsIdComboBox.Items.Add(iso.id);
                    IsotopicsIdComboBox.SelectedItem = iso.id;  // force m_iso assignment in event handler
					calcQ = true;
				}
				else
					MessageBox.Show("'" + ia.ID + "' is already in use", "I .... uuuh oh");
			}
        }


		private void CalculateBtn_Click(object sender, EventArgs e)
		{
			CalculateAndPersist(m_comp_iso);
		}

		void CalculateAndPersist(CompositeIsotopics comp_iso)
		{
			if (!GutCheck(calc: true))
				return;
			Isotopics newiso = null;
			uint retcode = comp_iso.CombinedCalculation(out newiso, NC.App.AppContext.INCCParity, NC.App.AppLogger);
			if (retcode == 36783)
				MessageBox.Show("Unable to update isotopics, sum of Pu isotopes must be greater than zero");
			else if (retcode == 36784)
				MessageBox.Show("Sum of masses = 0.\r\nNew isotopics not calculated and stored.");
			else
			{
				newiso.modified = comp_iso.modified = true;
				if (!NC.App.DB.CompositeIsotopics.Has(comp_iso.id))
					NC.App.DB.CompositeIsotopics.GetList().Add(comp_iso);   // add to in-memory list 
				List<CompositeIsotopics> list = NC.App.DB.CompositeIsotopics.GetMatch(i => i.modified);
				foreach (CompositeIsotopics iso in list)
				{
					long pk = NC.App.DB.CompositeIsotopics.Set(iso);             // add to database 
					NC.App.AppLogger.TraceInformation((pk >= 0 ? "Saved " : "Unable to save ") + iso.id + " composite isotopics");
				}
				comp_iso.modified = false;
                LoadEntry(comp_iso);
                string msg = string.Format(
						"Composite Isotopics id:\t{0}\n\nIsotopics Source Code:\t{1}\n\nMass\t\t{2,8:F3}\nPu238\t\t{3,8:F6}\nPu239\t\t{4,8:F6}\nPu240\t\t{5,8:F6}\nPu241\t\t{6,8:F6}\nPu242\t\t{7,8:F6}\nPu Date\t\t{8}\nAm241\t\t{9,8:F6}\nAm Date\t\t{10}\n\nIsotopics are updated to the reference date.",
						newiso.id, newiso.source_code, comp_iso.MassSum,
						newiso.pu238, newiso.pu239, newiso.pu240,
						newiso.pu241, newiso.pu242, newiso.pu_date,
						newiso.am241, newiso.am_date);
				DialogResult res = MessageBox.Show(msg, "Accept Results", MessageBoxButtons.OKCancel);
				if (res == DialogResult.OK)
				{
					if (!NC.App.DB.Isotopics.Has(newiso.id))
					{
						NC.App.DB.Isotopics.GetList().Add(newiso);
						long pk = NC.App.DB.Isotopics.Set(newiso);              // add to database 
						NC.App.AppLogger.TraceInformation((pk >= 0 ? "Saved " : "Unable to save ") + newiso.id + " isotopics");
						if (pk >= 0)
							NC.App.DB.Isotopics.Refresh();
					} else
					{
						NC.App.DB.Isotopics.Replace(newiso);
						NC.App.AppLogger.TraceInformation("Replaced " + newiso.id + " isotopics");
					}
					calcQ = false;
				}
                else
				    MessageBox.Show("New isotopics not stored.");
			}


		}

		private void SaveBtn_Click(object sender, EventArgs e)
        {
             if (!GutCheck(false))
            {
                // it's all ready the current iso
                PopulateWithSelectedItem();
                return;
            }
            else
            {
				if (modified)
				{
					if (!NC.App.DB.CompositeIsotopics.Has(m_comp_iso))
						NC.App.DB.CompositeIsotopics.GetList().Add(m_comp_iso);   // add to in-memory list 
					List<CompositeIsotopics> list = NC.App.DB.CompositeIsotopics.GetMatch(i => i.modified);
					foreach (CompositeIsotopics iso in list)
					{
						long pk = NC.App.DB.CompositeIsotopics.Set(iso);             // add to database 
						NC.App.AppLogger.TraceInformation((pk >= 0 ? "Saved " : "Unable to save ") + iso.id + " isotopics");
					}
					modified = false;
					calcQ = true;
				}
				RefreshIdComboWithDefault(m_comp_iso.id);
			}

        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            IDDIsotopicsEdit ia = new IDDIsotopicsEdit(m_comp_iso.id);
            if (ia.ShowDialog() == DialogResult.OK)
            {
                string oldId = m_comp_iso.id;
                m_comp_iso.id = string.Copy(ia.NewID);  // changes the id on the object on the list - m_comp_iso is a reference to an element in the in-memory list
                if (NC.App.DB.CompositeIsotopics.Rename(oldId, ia.NewID))
                {
                    IsotopicsIdComboBox.Items.Remove(oldId);
                    IsotopicsIdComboBox.Items.Add(m_comp_iso.id);
                    NC.App.AppLogger.TraceInformation("Renamed " + oldId + " to " + m_comp_iso.id);
                    IsotopicsIdComboBox.SelectedItem = m_comp_iso.id;
					calcQ = true;
                }
            }
        }

        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            string s = string.Format("Delete {0} composite isotopics data set?", m_comp_iso.id);
            DialogResult r = MessageBox.Show(s, NC.App.Name, MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
            {
                if (NC.App.DB.CompositeIsotopics.Delete(m_comp_iso)) // deletes from in-memory list and database
                {
                    NC.App.AppLogger.TraceInformation("Deleted " + m_comp_iso.id + " composite isotopics");
                    RefreshIdComboWithDefault(string.Empty);
                }
                else
                    NC.App.AppLogger.TraceInformation("Unable to delete " + m_comp_iso.id + " composite isotopics");
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
			if (ExitCheck())
            {
                // save as current acquire composite isotopics
                acq.isotopics_id = string.Copy(m_comp_iso.id);
                acq.comp_isotopics_id = string.Copy(m_comp_iso.id);
                NC.App.DB.UpdateAcquireParams(acq);
                Close();
            }
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
			if (ExitCheck())
			{
				if (modified)  // the reference variable m_comp_iso is pointing to an existing composite isotopics value that has been modified but now needs to be reverted
				{
                    NC.App.DB.CompositeIsotopics.Revert(m_comp_iso);  // revert originating selection on in-memory list back to DB values
				}
				Close();
        }
        }

		bool ExitCheck()
		{
			if (!calcQ)
				return true;
			string s = "You have not done a 'Calculate and Store Isotopics' for the last set of composite isotopics entered or modified.\n\nDo you want to return to the composite isotopics dialog box?";
            DialogResult r = MessageBox.Show(s, NC.App.Name, MessageBoxButtons.YesNo);
            if (r == DialogResult.Yes)
				return false;
			else
				return true;
		}

        private void HelpBtn_Click(object sender, EventArgs e)
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
            foreach (Isotopics.SourceCode sc in Enum.GetValues(typeof(Isotopics.SourceCode)))
            {
                IsoSrcCodeComboBox.Items.Add(sc.ToString());
            }
        }
        private void RefreshIdComboWithDefault(string id)
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
                if (!string.IsNullOrEmpty(id))
                    m_comp_iso = isolist.Find(i => string.Equals(i.id, id, StringComparison.OrdinalIgnoreCase));
                else
                    m_comp_iso = isolist[0];
            }

            if (m_comp_iso != null)
                 IsotopicsIdComboBox.SelectedItem = m_comp_iso.id;  // forces event handler to load dialog fields
        }
		private bool GutCheck(bool calc)
        {
            double sum = 0;
			if (m_comp_iso.pu_mass == 0.0)
				return true;
			sum = m_comp_iso.Summed;
			if ((sum > isomax) || (sum < isomin))
			{
				string s = string.Format("Error: Isotopics total for line 1 = {0}%\r\nIsotopics total must be within {1}% and {2}%" + (calc ? "\r\nNew isotopics not calculated and stored." : ""),
					sum, isomin, isomax);
				MessageBox.Show(s, m_comp_iso.id);
				return false;
			}
			int line = 1;
			foreach(CompositeIsotopic ci in m_comp_iso.isotopicComponents)
			{
				line++;
				if (ci.pu_mass == 0.0f)
					continue;
				sum = ci.Summed;
				if ((sum > isomax) || (sum < isomin))
				{
					string s = string.Format("Error: Isotopics total for line {0} = {1}%\r\nIsotopics total must be within {2}% and {3}%" + (calc ? "\r\nNew isotopics not calculated and stored." : ""),
						line, sum, isomin, isomax);
					MessageBox.Show(s, m_comp_iso.id);
					return false;
				}
			}
			return true;
        }

        public CompositeIsotopics GetSelectedIsotopics { get { return m_comp_iso; } }

        CompositeIsotopics m_comp_iso;
        AcquireParameters acq;
        const double isomin = 99.7;
        const double isomax = 100.3;
        const double TOLERANCE = .00001;
        bool modified = false, calcQ = false;

        private void SelButton_Click(object sender, EventArgs e)
		{
			IsotopicsList il = new IsotopicsList(iso: false);
			if (il.ShowDialog() == DialogResult.OK)
			{
				m_comp_iso = il.GetSingleSelectedCompIsotopics();
				IsotopicsIdComboBox.SelectedItem = m_comp_iso.id;
			}
		}
	}

	public class CalendarColumn : DataGridViewColumn
    {
        public CalendarColumn() : base(new CalendarCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(CalendarCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class CalendarCell : DataGridViewTextBoxCell
    {

        public CalendarCell()
            : base()
        {
            // Use the short wanker date format.
            this.Style.Format = "d";//"yyyy-MM-dd";
        }

        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            CalendarEditingControl ctl = DataGridView.EditingControl as CalendarEditingControl;
            // Use the default row value when Value property is null.
            if (this.Value == null)
            {
                ctl.Value = (DateTime)this.DefaultNewRowValue;
            }
            else
            {
				DateTime dt = ctl.Value;
				DateTime.TryParse((string)Value, out dt);
				ctl.Value = dt;
            }
        }

        public override Type EditType
        {
            get
            {
                // Return the type of the editing control that CalendarCell uses.
                return typeof(CalendarEditingControl);
            }
        }

        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.  
                return typeof(DateTime);
            }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return DateTime.Now;
            }
        }
    }

    class CalendarEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        public CalendarEditingControl()
        {
            //this.CustomFormat = "yyyy-MM-dd";
            this.Format = DateTimePickerFormat.Short;
       }

        // Implements the IDataGridViewEditingControl.EditingControlFormattedValue 
        // property.
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToShortDateString();
            }
            set
            {
                if (value is String)
                {
                    try
                    {
                        // This will throw an exception of the string is 
                        // null, empty, or not in the format of a date.
                        this.Value = DateTime.Parse((String)value);
                    }
                    catch
                    {
                        // In the case of an exception, just use the 
                        // default value so we're not left with a null
                        // value.
                        this.Value = DateTime.Now;
                    }
                }
            }
        }

        // Implements the 
        // IDataGridViewEditingControl.GetEditingControlFormattedValue method.
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        // Implements the 
        // IDataGridViewEditingControl.ApplyCellStyleToEditingControl method.
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        // Implements the IDataGridViewEditingControl.EditingControlRowIndex 
        // property.
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        // Implements the IDataGridViewEditingControl.EditingControlWantsInputKey 
        // method.
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        // Implements the IDataGridViewEditingControl.PrepareEditingControlForEdit 
        // method.
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        // Implements the IDataGridViewEditingControl
        // .RepositionEditingControlOnValueChange property.
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlDataGridView property.
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingControlValueChanged property.
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        // Implements the IDataGridViewEditingControl
        // .EditingPanelCursor property.
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }
    }
}
