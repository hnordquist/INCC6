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
    using System.Drawing;
    using NC = NCC.CentralizedState;

    public partial class IDDCompositeIsotopics : Form
    {
        public IDDCompositeIsotopics()
        {
            //CalendarColumn col6 = new CalendarColumn();
            //CalendarColumn col8 = new CalendarColumn();
            //col8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            //col8.HeaderText = "Am Date";
            //col8.Name = "AmDate";
            //col8.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //col6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            //col6.HeaderText = "Pu Date";
            //col6.Name = "PuDate";
            //col6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            //IsoDataGrid.Columns.RemoveAt(8);
            //IsoDataGrid.Columns.RemoveAt(6);
            //IsoDataGrid.Columns.AddRange()
            InitializeComponent();
            RefreshIsoCodeCombo();
            RefreshIdComboWithDefault(string.Empty);

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
                long key = -1;
                if ((key = NC.App.DB.CompositeIsotopics.Set(iso)) >= 0)
                {
                    NC.App.DB.CompositeIsotopics.AddComposites(iso.isotopicComponents, key);
                    applog.TraceInformation("'" + iso.id + "' composite isotopics updated/added");
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

        }

        private void AddBtn_Click(object sender, EventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (modified)
            {
                // URGNT: copy thevmodel in the single isotopcs implementation
                if (!NC.App.DB.CompositeIsotopics.Has(m_comp_iso))
                    NC.App.DB.CompositeIsotopics.GetList().Add(m_comp_iso);   // add to in-memory list 
                List<CompositeIsotopics> list = NC.App.DB.CompositeIsotopics.GetMatch(i => i.modified);
                foreach (CompositeIsotopics iso in list)
                {
                    long pk = NC.App.DB.CompositeIsotopics.Set(iso);             // add to database 
                    applog.TraceInformation((pk >= 0 ? "Saved " : "Unable to save ") + iso.id + " isotopics");
                    NC.App.DB.CompositeIsotopics.AddComposites(iso.isotopicComponents, pk);
                }
                modified = false;
            }
            // etc/ RefreshIdComboWithDefaultOrSet(m_comp_iso.id);

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
            foreach (Isotopics.SourceCode sc in System.Enum.GetValues(typeof(Isotopics.SourceCode))) // could use the GetOptionType scheme here
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

        public CompositeIsotopics GetSelectedIsotopics { get { return m_comp_iso; } }

        CompositeIsotopics m_comp_iso;
        const double isomin = 99.7;
        const double isomax = 100.3;
        const double TOLERANCE = .00001;
        NCCReporter.LMLoggers.LognLM applog;
        bool modified = false;

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
            // Use the wanker date format.
            this.Style.Format = "yyyy-MM-dd";
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
                ctl.Value = (DateTime)this.Value;
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
            this.CustomFormat = "yyyy-MM-dd";
            this.Format = DateTimePickerFormat.Custom;
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
