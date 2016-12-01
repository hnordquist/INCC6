/*
This source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

This source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2016, International Atomic Energy Agency (IAEA), IAEA.org
   Authored by J. Longo

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS"AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AnalysisDefs;
using DetectorDefs;
using NCCReporter;
namespace NewUI
{
	using Integ = NCC.IntegrationHelpers;
	using N = NCC.CentralizedState;

	public partial class LMAcquire : Form
	{
		Detector det;
        AcquireParameters ap;
		bool PreserveAnalyzerChanges, AnalyzersLoaded, LMParamUpdate, AcqParamUpdate;
		bool FromINCC5Acquire { set; get; } // if called directly from INCC5 acquire, force user to select a multiplicity analyzer

		public enum LMSteps { FileBased, DAQBased, AnalysisSpec, Go };



		public LMAcquire(AcquireParameters _ap, Detector _det, bool fromINCC5Acq = false)
		{
			InitializeComponent();
			FromINCC5Acquire = fromINCC5Acq; 
			Text += (_det.Id.DetectorId);
			det = _det;
			ap = _ap;
			if (!FromINCC5Acquire)  // reset and build occurs in the INCC5 acquire handler code
			{
				ResetMeasurement();
				Integ.BuildMeasurement(ap, det, AssaySelector.MeasurementOption.unspecified);
			}
			PreserveAnalyzerChanges = AnalyzersLoaded = LMParamUpdate = AcqParamUpdate = false;
			BuildAnalyzerCombo();
			Swap(ap.data_src.Live());
			SelectTheBestINCC5AcquireVSRRow();
		}

		void BuildAnalyzerCombo()
		{
			DataGridViewColumnCollection dgvcc = AnalyzerGridView.Columns;
			DataGridViewComboBoxColumn c = (DataGridViewComboBoxColumn)dgvcc["Type"];
			c.Items.Add(TNameMap(typeof(Multiplicity), FAType.FAOn)); 
			c.Items.Add(TNameMap(typeof(Multiplicity), FAType.FAOff)); 
			c.Items.Add(TNameMap(typeof(Feynman)));
			c.Items.Add(TNameMap(typeof(Rossi)));
			c.Items.Add(TNameMap(typeof(TimeInterval)));
			c.Items.Add(TNameMap(typeof(Coincidence)));
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			TabControl t = (TabControl)sender;
			if (t.SelectedIndex == 0)
				LoadParams(LMSteps.FileBased);
			else if (t.SelectedIndex == 1)
				LoadParams(LMSteps.DAQBased);
			else if (t.SelectedIndex == 2)
				LoadParams(LMSteps.AnalysisSpec);  // only loads once, from the current measurement state
			else if (t.SelectedIndex == 3)
				LoadParams(LMSteps.Go);
		}


        private void Step2NCDRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (N.App.AppContext.NCDFileAssay != ((RadioButton)sender).Checked)
            {
                N.App.AppContext.modified = true; N.App.AppContext.NCDFileAssay = ((RadioButton)sender).Checked;
				AcqParamUpdate = true;
            }
            if (N.App.AppContext.NCDFileAssay)
                ap.data_src = ConstructedSource.NCDFile;
        }

        private void Step2SortedPulseRadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (N.App.AppContext.PulseFileAssay != ((RadioButton)sender).Checked)
            {
                N.App.AppContext.modified = true; N.App.AppContext.PulseFileAssay = ((RadioButton)sender).Checked;
				AcqParamUpdate = true;
            }
            if (N.App.AppContext.PulseFileAssay)
                ap.data_src = ConstructedSource.SortedPulseTextFile;
        }

        private void Step2PTR32RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (N.App.AppContext.PTRFileAssay != ((RadioButton)sender).Checked)
            {
                N.App.AppContext.modified = true; N.App.AppContext.PTRFileAssay = ((RadioButton)sender).Checked;
				AcqParamUpdate = true;
            }
            if (N.App.AppContext.PTRFileAssay)
                ap.data_src = ConstructedSource.PTRFile;
        }
        private void Step2MCA527RadioBtn_CheckedChanged(object sender, EventArgs e)
        {
            if (N.App.AppContext.MCA527FileAssay != ((RadioButton)sender).Checked)
            {
                N.App.AppContext.modified = true; N.App.AppContext.MCA527FileAssay = ((RadioButton)sender).Checked;
				AcqParamUpdate = true;
            }
            if (N.App.AppContext.MCA527FileAssay)
                ap.data_src = ConstructedSource.MCA527File;
        }

        private void Step2RecurseCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (N.App.AppContext.Recurse != ((CheckBox)sender).Checked)
            {
                N.App.AppContext.modified = true; N.App.AppContext.Recurse = ((CheckBox)sender).Checked;
            }
        }

        private void Step2OutputDirectoryTextBox_Leave(object sender, EventArgs e)
        {
            string s = ap.lm.Results;
            ap.lm.modified = Format.Changed(((TextBox)sender).Text, ref s);
            if (ap.lm.modified)
			{
                ap.lm.Results = s; LMParamUpdate = true;
			}
        }

        private void Step2BOutputDirectoryTextBox_Leave(object sender, EventArgs e)
        {
            string s = ap.lm.Results;
            ap.lm.modified = Format.Changed(((TextBox)sender).Text, ref s);
            if (ap.lm.modified)
			{
                ap.lm.Results = s; LMParamUpdate = true;
			}
		}

		private void Step2FilenameTextBox_TextChanged(object sender, EventArgs e)
        {
            //this.Step2ANextBtn.Enabled = true;
        }

        private void Step2FilenameTextBox_Leave(object sender, EventArgs e)
        {
            N.App.AppContext.FileInput = ((TextBox)(sender)).Text;
        }

		private void Step2BrowseBtn_Click(object sender, EventArgs e)
        {
            DialogResult dr = GetUsersInputSelection();
            if (dr == DialogResult.OK)
            {
                if (N.App.AppContext.FileInputList == null)
                    Step2InputDirectoryTextBox.Text = N.App.AppContext.FileInput;
                else
                    Step2InputDirectoryTextBox.Text = N.App.AppContext.FileInputList.Count.ToString() + " selected files";
            }
            else Step2InputDirectoryTextBox.Text = N.App.AppContext.FileInput;        
        }

        DialogResult GetUsersInputSelection()
        {
            string a = string.Empty, b = string.Empty, c = string.Empty, d = string.Empty;
            switch (ap.data_src)
            {
                case ConstructedSource.MCA527File:
                    a = "Select MCA files or folder";
                    b = "MCA527"; c = "mca";
                    break;
                case ConstructedSource.NCDFile:
                    a = "Select NCD files or folder";
                    b = "LMMM NCD"; c = "ncd";
                    break;
                case ConstructedSource.PTRFile:
                    a = "Select PTR-32 files or folder";
                    b = "PTR-32"; c = "bin"; d = "chn";
                    break;
                case ConstructedSource.SortedPulseTextFile:
                    a = "Select pulse files or folder";
                    b = "pulse"; c = "txt";
                    break;  
            }

            DialogResult dr = UIIntegration.GetUsersFilesFolder(a, N.App.AppContext.FileInput, b, c, d);
            return dr;

        }

        private void FilePicker2_Click(object sender, EventArgs e)
        {
            string s = UIIntegration.GetUsersFolder("Select Output Folder", ap.lm.Results);
            if (!string.IsNullOrEmpty(s) && !string.Equals(ap.lm.Results, s))
            {
                ap.lm.Results = s;
                Step2OutputDirectoryTextBox.Text = s;
                ap.modified = true;
				LMParamUpdate = true;
            }
            else
                Step2OutputDirectoryTextBox.Text = ap.lm.Results;

        }

		private void Step2IncludeConfigCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ap.lm.IncludeConfig != ((CheckBox)sender).Checked)
            {
                ap.lm.modified = true; ap.lm.IncludeConfig = ((CheckBox)sender).Checked; LMParamUpdate = true;
            }
        }

		private void Step2BIncludeConfigCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Step2IncludeConfigCheckBox_CheckedChanged(sender, e);
		}

        private void Step2AutoOpenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (N.App.AppContext.OpenResults != ((CheckBox)sender).Checked)
            {
                N.App.AppContext.modified = true; N.App.AppContext.OpenResults = ((CheckBox)sender).Checked;
            }
        }

		private void Step2BAutoOpenCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Step2AutoOpenCheckBox_CheckedChanged(sender, e);
		}


        private void Step2SaveEarlyTermCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ap.lm.SaveOnTerminate != ((CheckBox)sender).Checked)
            {
                ap.lm.modified = true; ap.lm.SaveOnTerminate = ((CheckBox)sender).Checked; LMParamUpdate = true;
            }
        }
		
		private void Step2BSaveEarlyTermCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			Step2SaveEarlyTermCheckBox_CheckedChanged(sender, e);
		}

		private void Step2BWriteDataFiles_CheckedChanged(object sender, EventArgs e)
		{
            if (N.App.AppContext.LiveFileWrite != ((CheckBox)sender).Checked)
            {
                N.App.AppContext.modified = true; N.App.AppContext.LiveFileWrite = ((CheckBox)sender).Checked;
            }
		}

        void LoadParams(LMSteps step)
        {
            switch (step)
            {
                case LMSteps.FileBased:
                    Step2InputDirectoryTextBox.Text = N.App.AppContext.FileInput;
                    Step2RecurseCheckBox.CheckState = (N.App.AppContext.Recurse ? CheckState.Checked : CheckState.Unchecked);
                    Step2AutoOpenCheckBox.CheckState = (N.App.AppContext.OpenResults ? CheckState.Checked : CheckState.Unchecked);
                    Step2IncludeConfigCheckBox.CheckState = (ap.lm.IncludeConfig ? CheckState.Checked : CheckState.Unchecked);
                    Step2SaveEarlyTermCheckBox.CheckState = (ap.lm.SaveOnTerminate ? CheckState.Checked : CheckState.Unchecked);

                    // el radio buton setter
                    if (N.App.AppContext.PTRFileAssay || det.Id.SRType == InstrType.PTR32)
                    {
                        Step2PTR32RadioBtn.Checked = true;
                        ap.data_src = ConstructedSource.PTRFile;
                    }
                    else if (N.App.AppContext.PulseFileAssay)
                    {
                        Step2SortedPulseRadioBtn.Checked = true;
                        ap.data_src = ConstructedSource.SortedPulseTextFile;
                    }
                    else if (N.App.AppContext.MCA527FileAssay || det.Id.SRType == InstrType.MCA527)
                    {
                        Step2MCA5272RadioBtn.Checked = true;
                        ap.data_src = ConstructedSource.MCA527File;
                    }
                    else // always the default
                    {
                        Step2NCDRadioBtn.Checked = true;
                    }

                    Step2OutputDirectoryTextBox.Text = ap.lm.Results;
                    //RefreshDetectorCombo(Step2ADetCB);
                    break;
                case LMSteps.DAQBased:
                    Step2BAutoOpenCheckBox.CheckState = (N.App.AppContext.OpenResults ? CheckState.Checked : CheckState.Unchecked);
                    Step2BIncludeConfigCheckBox.CheckState = (ap.lm.IncludeConfig ? CheckState.Checked : CheckState.Unchecked);
                    Step2BSaveEarlyTermCheckBox.CheckState = (ap.lm.SaveOnTerminate ? CheckState.Checked : CheckState.Unchecked);
                    if (FromINCC5Acquire)
                        IntervalTextBox.Text = ap.run_count_time.ToString();
                    else
                        IntervalTextBox.Text = ap.lm.Interval.ToString();
                    Step2BOutputDirectoryTextBox.Text = ap.lm.Results;
                    if (FromINCC5Acquire)
                        CycleNumTextBox.Text = ap.num_runs.ToString();
                    else
                        CycleNumTextBox.Text = ap.lm.Cycles.ToString();
                    Step2BAutoOpenCheckBox.CheckState = (N.App.AppContext.OpenResults ? CheckState.Checked : CheckState.Unchecked);
                    Step2BWriteDataFiles.CheckState = (N.App.AppContext.LiveFileWrite ? CheckState.Checked : CheckState.Unchecked);

                    RefreshDetectorCombo(Step2BDetectorComboBox);
					Step2BDetectorComboBox.SelectedItem = det;
                    break;
				case LMSteps.AnalysisSpec:
					LoadAnalyzerRows();  // loads just once
					break;
                case LMSteps.Go:
					Comment.Text = ap.comment;
					ResultsLocation.Text = ap.lm.Results;
					LoadDescriptiveViews(ap.data_src.Live());
                    break;
                default:
                    break;
            }
        }

		string[] barfoo;
		string TNameMap(Type t, FAType FA = FAType.FAOff)
		{
			if (barfoo == null)
				 barfoo = new string[] {"Fast Multiplicity", "Multiplicity", "Feynman", "Rossi-α (alpha)", "Event Spacing", "Coincidence" };
			if (t.Equals(typeof(Multiplicity)) && FA == FAType.FAOn)
				return barfoo[0];
			if (t.Equals(typeof(Multiplicity)) && FA == FAType.FAOff)
				return barfoo[1];
			if (t.Equals(typeof(Feynman)))
				return barfoo[2];
			if (t.Equals(typeof(Rossi)))
				return barfoo[3];
			if (t.Equals(typeof(TimeInterval)))
				return barfoo[4];
			if (t.Equals(typeof(Coincidence)))
				return barfoo[5];	
			return "";
		}
		void TTypeMap(string cs, out Type t, out FAType FA)
		{
			FA = FAType.FAOff;
			if (string.Compare(cs, barfoo[0]) == 0)
			{
				FA = FAType.FAOn;
				t = typeof(Multiplicity);
			}
			else if (string.Compare(cs, barfoo[1]) == 0)
				t = typeof(Multiplicity);
			else if (string.Compare(cs, barfoo[2]) == 0)
				t = typeof(Feynman);
			else if (string.Compare(cs, barfoo[3]) == 0)
				t = typeof(Rossi);
			else if (string.Compare(cs, barfoo[4]) == 0)
				t = typeof(TimeInterval);
			else if (string.Compare(cs, barfoo[5]) == 0)
				t = typeof(Coincidence);
			else
				t = typeof(Multiplicity);
		}

		ulong Construct(DataGridViewCell c)
		{
			ulong ul = 0;
			ulong.TryParse((string)(c.Value), out ul);
			return ul;
		}

		bool CheckedRow(DataGridViewRow row)
		{
			string s = (string)row.Cells[0].Value;
			return !string.IsNullOrEmpty(s) && (string.Compare(s,"yes") == 0);
		}
		bool CheckedChanged(DataGridViewRow row)
		{
			if (row == null || row.Cells[0].Tag == null)
				return false;
			bool origValue = (bool)row.Cells[0].Tag;
			return origValue != CheckedRow(row);
		}
        string[] ToSimpleValueArray(SpecificCountingAnalyzerParams s)
		{
			Type t = s.GetType();
			string[] vals = new string[5]; 
			vals[0] = s.Active ? "yes" : "no";
			if (t.Equals(typeof(Rossi)) || t.Equals(typeof(TimeInterval)) || t.Equals(typeof(Feynman)))
			{
				vals[1] = TNameMap(t, FAType.FAOff);
				vals[2] = s.gateWidthTics.ToString();
				vals[3] = string.Empty;
				vals[4] = string.Empty;
			}
			else if (t.Equals(typeof(Multiplicity)))
			{
				vals[1] = TNameMap(t, ((Multiplicity)s).FA);
				vals[2] = s.gateWidthTics.ToString();
				vals[3] = ((Multiplicity)s).SR.predelay.ToString();
				if (((Multiplicity)s).FA == FAType.FAOn)
				{
					vals[4] = ((Multiplicity)s).BackgroundGateTimeStepInTics.ToString();
				}
				else
				{				
					vals[4] = ((Multiplicity)s).AccidentalsGateDelayInTics.ToString();
				}
			}
			else if (t.Equals(typeof(Coincidence)))
			{
				vals[1] = TNameMap(t, FAType.FAOff);
				vals[2] = s.gateWidthTics.ToString();
				vals[3] = ((Coincidence)s).SR.predelay.ToString();
				vals[4] = ((Coincidence)s).AccidentalsGateDelayInTics.ToString();
			}
			return vals;
		}

		void SetRowDetails(DataGridViewRow row, SpecificCountingAnalyzerParams s)					
		{
			row.Cells[0].Tag = s.Active;
			row.Cells[2].Tag = s.gateWidthTics;
			row.Tag = s;
			Type t = s.GetType();
			row.Cells[3].ReadOnly = false;
			row.Cells[4].ReadOnly = false;
			if (t.Equals(typeof(Rossi)) || t.Equals(typeof(TimeInterval)) || t.Equals(typeof(Feynman)))
			{
				row.Cells[3].ReadOnly = true;
				row.Cells[4].ReadOnly = true;
            }
            else if (t.Equals(typeof(Multiplicity)))
			{
                row.Cells[3].Tag = ((Multiplicity)s).SR.predelay;
                if (((Multiplicity)s).FA == FAType.FAOn)
                {
                    row.Cells[4].Tag = ((Multiplicity)s).BackgroundGateTimeStepInTics;
                }
                else
                {
                    row.Cells[4].Tag = ((Multiplicity)s).AccidentalsGateDelayInTics;
                }
            }
            else if (t.Equals(typeof(Coincidence)))
			{
                row.Cells[3].Tag = ((Coincidence)s).SR.predelay;
                row.Cells[4].Tag = ((Coincidence)s).AccidentalsGateDelayInTics;
            }
        }

        void SetRODetails(DataGridViewRow row, Type t)
        {
            if (t.Equals(typeof(Rossi)) || t.Equals(typeof(TimeInterval)) || t.Equals(typeof(Feynman)))
            {
                row.Cells[3].ReadOnly = true;
                row.Cells[4].ReadOnly = true;
            }
            else if (t.Equals(typeof(Multiplicity)))
            {
                row.Cells[3].ReadOnly = false;
                row.Cells[4].ReadOnly = false;
            }
            else if (t.Equals(typeof(Coincidence)))
            {
                row.Cells[3].ReadOnly = false;
                row.Cells[4].ReadOnly = false;
           }
        }


        void LoadAnalyzerRows()
		{
			if (!AnalyzersLoaded)
			{
				CountingAnalysisParameters alt = CountingAnalysisParameters.Copy(N.App.Opstate.Measurement.AnalysisParams);
				foreach (SpecificCountingAnalyzerParams s in alt)
				{
					if (s.suspect)
						continue;
					string[] a = ToSimpleValueArray(s);
					int i = AnalyzerGridView.Rows.Add(a);
					SetRowDetails(AnalyzerGridView.Rows[i], s);
				}
				AnalyzersLoaded = true;
			}
		}

		void SelectTheRankedRow()
		{
			for(int i = 0; i < AnalyzerGridView.Rows.Count; i++)
			{
				DataGridViewRow row = AnalyzerGridView.Rows[i];
				if (row == null) // empty row, so just skip it
					continue;
				SpecificCountingAnalyzerParams r = (SpecificCountingAnalyzerParams)row.Tag;
				if (r == null) // empty row, so just skip it
					continue;
				if (r.Rank == SpecificCountingAnalyzerParams.Select && !r.suspect)
				{
					AnalyzerGridView.CurrentCell = row.Cells[0];
					break;
				}
			}
		}

        private void AnalyzerGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (cbm != null)
            {
                // remove the subscription for the selected index changed event
                cbm.SelectedIndexChanged -= new EventHandler(cbm_SelectedIndexChanged);
            }
            if (e.ColumnIndex > 1)
            {
				DataGridViewRow row = AnalyzerGridView.Rows[e.RowIndex];
				DataGridViewCell cell = row.Cells[e.ColumnIndex];
                ulong x = (cell.Tag == null ? 100ul : (ulong)cell.Tag);
                bool mod = (Format.ToNN((string)cell.Value, ref x));
                if (mod)
                {
                    cell.Tag = x;
					PreserveAnalyzerChanges = true;
                }
                else
                    cell.Value = x.ToString();
				row.ErrorText = string.Empty;
            }
		} 

		private void AnalyzerGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null)
            {
                string display = string.Empty;
                DataGridViewCell cell = AnalyzerGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                switch (e.ColumnIndex)
                {
                    case 0:
						if (string.IsNullOrEmpty(display))
							display = "Check this box to enable this analyzer";
                        cell.ToolTipText = display;
                        break;
                    case 1:
						if (string.IsNullOrEmpty(display))
	                        display = "The analyzer type";
                        cell.ToolTipText = display;
                        break;
                    case 2:
						if (string.IsNullOrEmpty(display))
	                        display = "Gate width (in 1e-7s ticks)";
                        cell.ToolTipText = display;
                        break;
                    case 3:
						if (cell.ReadOnly)
							display = "Unused";
						else
							display = "Detector predelay (in 1e-7s ticks)";
                        cell.ToolTipText = display;
                        break;
                    case 4:
						if (cell.ReadOnly)
							display = "Unused";
						else
						{
							Type t; FAType FA;
							TTypeMap((string)AnalyzerGridView.Rows[e.RowIndex].Cells[1].Value, out t, out FA);
							if (FA == FAType.FAOn)
								display = "Background gate width (in 1e-7s ticks)";
							else
								display = "Accidentals gate width (long delay, in 1e-7s ticks)";
						}
                        cell.ToolTipText = display;
                        break;
                }
            }
        }


        private void DeleteBtn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in AnalyzerGridView.SelectedRows)
            {
                if (row.Tag == null)
                    continue;
                SpecificCountingAnalyzerParams s = (SpecificCountingAnalyzerParams)row.Tag; // or index 0
                s.reason = "del";
            }                
            foreach (DataGridViewRow row in AnalyzerGridView.SelectedRows)
            {
                int idx = AnalyzerGridView.Rows.IndexOf(row);
                AnalyzerGridView.Rows.RemoveAt(idx);
				PreserveAnalyzerChanges = true;
            }
            AnalyzerGridView.Refresh();
        }

        SpecificCountingAnalyzerParams Constructed(DataGridViewRow row)
		{
			SpecificCountingAnalyzerParams s = null;
			Type t; FAType FA;
			if (row.Cells[1].Value == null)
				return null;
			TTypeMap((string)row.Cells[1].Value, out t, out FA);
			if (t.Equals(typeof(Multiplicity)))
			{
				s = new Multiplicity(FA);
				((Multiplicity)s).SR.predelay = Construct(row.Cells[3]);
				if (FA == FAType.FAOn)
					((Multiplicity)s).BackgroundGateTimeStepInTics = Construct(row.Cells[4]);
				else
					((Multiplicity)s).AccidentalsGateDelayInTics = Construct(row.Cells[4]);
			}
			else if (t.Equals(typeof(Feynman)))
			{
				s = new Feynman();
			}
			else if (t.Equals(typeof(Rossi)))
			{
				s = new Rossi();
			}
			else if (t.Equals(typeof(TimeInterval)))
			{
				s = new TimeInterval();
			}
			else if (t.Equals(typeof(Coincidence)))
			{
				s = new Coincidence();
				((Coincidence)s).SR.predelay = Construct(row.Cells[3]);
				((Coincidence)s).AccidentalsGateDelayInTics = Construct(row.Cells[4]);
			}
			s.gateWidthTics = Construct(row.Cells[2]);
			s.Active = CheckedRow(row);
			return s;
		}

		bool CompareAnalyzers(SpecificCountingAnalyzerParams original, SpecificCountingAnalyzerParams candidate)
		{
			if (original == null || candidate == null)  // skip empty rows
				return true;
			Type t = original.GetType();
			bool pihfd = t.Equals(candidate.GetType());
			if (pihfd)
				pihfd = (original.Active == candidate.Active);
			if (pihfd)
				pihfd = (original.gateWidthTics == candidate.gateWidthTics);
			if (pihfd)
			{
				if (t.Equals(typeof(Multiplicity)))
				{
					Multiplicity o = (Multiplicity)original;
					Multiplicity c = (Multiplicity)candidate;
					pihfd = (o.SR.predelay == c.SR.predelay);
					if (pihfd)
					{
						pihfd = o.FA == c.FA;
						if (pihfd)
						{
							if (o.FA == FAType.FAOff)
								pihfd = o.AccidentalsGateDelayInTics == c.AccidentalsGateDelayInTics;
							if (o.FA == FAType.FAOn)
								pihfd = o.BackgroundGateTimeStepInTics == c.BackgroundGateTimeStepInTics;
						}
					}
				}
			}
			return pihfd;
		}

		void ReconstructRow(DataGridViewRow row, Type t, FAType FA)
		{
			SpecificCountingAnalyzerParams s = (SpecificCountingAnalyzerParams)row.Tag;
			if (t.Equals(typeof(Multiplicity)) && typeof(Multiplicity) == s.GetType())  // keep the predelay and gw
			{
				Multiplicity m = (Multiplicity)s;
				if (FA != m.FA)  // set alt gate to default if FA changed
				{
					Multiplicity x = new Multiplicity(FA);
					if (FA == FAType.FAOn)
					{
						row.Cells[4].Value = x.BackgroundGateTimeStepInTics.ToString();
						row.Cells[4].Tag = x.BackgroundGateTimeStepInTics;
					}
					else
					{
						row.Cells[4].Value = x.AccidentalsGateDelayInTics.ToString();
						row.Cells[4].Tag = x.AccidentalsGateDelayInTics;
					}
				}
				else
				{
					if (FA == FAType.FAOn)
					{
						row.Cells[4].Value = m.BackgroundGateTimeStepInTics.ToString();
						row.Cells[4].Tag = m.BackgroundGateTimeStepInTics;
					}
					else
					{
						row.Cells[4].Value = m.AccidentalsGateDelayInTics.ToString();
						row.Cells[4].Tag = m.AccidentalsGateDelayInTics;
					}
				}
			}
			else if (t.Equals(s.GetType()))
			{
				ReconstructRow(row, s, t, FA);
			}
			row.Cells[2].Tag = s.gateWidthTics;
			row.Cells[2].Value = s.gateWidthTics.ToString();	
			row.Tag = s;
			SetRODetails(row, t);
		}

		void ConstructNewRow(DataGridViewRow row, Type t, FAType FA)
		{
			SpecificCountingAnalyzerParams s = null;
			if (t.Equals(typeof(Multiplicity)))
			{
				Multiplicity m = new Multiplicity(FA);
				row.Cells[3].Value = det.SRParams.predelay.ToString();
				row.Cells[3].Tag = det.SRParams.predelay;
				if (FA == FAType.FAOn)
				{
					row.Cells[4].Value = m.BackgroundGateTimeStepInTics.ToString();
					row.Cells[4].Tag = m.BackgroundGateTimeStepInTics;
				}
				else
				{
					row.Cells[4].Value = m.AccidentalsGateDelayInTics.ToString();
					row.Cells[4].Tag = m.AccidentalsGateDelayInTics;
				}
				m.gateWidthTics = det.SRParams.gateLength;
				s = m;
			}
			else if (t.Equals(typeof(Feynman)))
			{
				s = new Feynman();
			}
			else if (t.Equals(typeof(Rossi)))
			{
				s = new Rossi();
			}
			else if (t.Equals(typeof(TimeInterval)))
			{
				s = new TimeInterval();
			}
			else if (t.Equals(typeof(Coincidence)))
			{
				Coincidence c = new Coincidence();
				row.Cells[3].Value = det.SRParams.predelay.ToString();
				row.Cells[3].Tag = det.SRParams.predelay;
				row.Cells[4].Value = c.AccidentalsGateDelayInTics.ToString();
				row.Cells[4].Tag = c.AccidentalsGateDelayInTics;
				c.gateWidthTics = det.SRParams.gateLength;
				s = c;
			}

			row.Cells[0].Tag = s.Active;
			row.Cells[2].Tag = s.gateWidthTics;
			row.Cells[2].Value = s.gateWidthTics.ToString();	
			row.Tag = s;
			SetRODetails(row, t);		
		}
		void ReconstructRow(DataGridViewRow row, SpecificCountingAnalyzerParams s, Type t, FAType FA)
		{
			if (t.Equals(typeof(Multiplicity)))
			{
				Multiplicity m = (Multiplicity)s;
				row.Cells[3].Value = m.SR.predelay.ToString();
				row.Cells[3].Tag = m.SR.predelay;
				if (FA == FAType.FAOn)
				{
					row.Cells[4].Value = m.BackgroundGateTimeStepInTics.ToString();
					row.Cells[4].Tag = m.BackgroundGateTimeStepInTics;
				}
				else
				{
					row.Cells[4].Value = m.AccidentalsGateDelayInTics.ToString();
					row.Cells[4].Tag = m.AccidentalsGateDelayInTics;
				}
			}
			else if (t.Equals(typeof(Feynman)))
			{
			}
			else if (t.Equals(typeof(Rossi)))
			{
			}
			else if (t.Equals(typeof(TimeInterval)))
			{
			}
			else if (t.Equals(typeof(Coincidence)))
			{
				Coincidence c = (Coincidence)s;
				row.Cells[3].Value = c.SR.predelay.ToString();
				row.Cells[3].Tag = c.SR.predelay;
				row.Cells[4].Value = c.AccidentalsGateDelayInTics.ToString();
				row.Cells[4].Tag = c.AccidentalsGateDelayInTics;
			}

			row.Cells[0].Tag = s.Active;
			row.Cells[2].Tag = s.gateWidthTics;
			row.Cells[2].Value = s.gateWidthTics.ToString();	
			SetRODetails(row, t);		
		}

		void CheckActiveChecks()
		{
			foreach(DataGridViewRow row in AnalyzerGridView.Rows)
			{
				if (CheckedChanged(row))
				{
					PreserveAnalyzerChanges = true;
					break;
				}
			}
		}
		private void PreserveNewState()
        {    
			CheckActiveChecks();      
			if (PreserveAnalyzerChanges)
			{
				CountingAnalysisParameters cntap = new CountingAnalysisParameters();
				foreach(DataGridViewRow row in AnalyzerGridView.Rows)
				{
					SpecificCountingAnalyzerParams r = Constructed(row);
					if (r == null) // empty row, so just skip it
						continue;
					row.Cells[0].Tag = r.Active;  // reset the tag for the check box, it is the only one not updated elsewhere
					cntap.Add(r);
					r.modified = !CompareAnalyzers((SpecificCountingAnalyzerParams)row.Tag, r);
				}
				N.App.Opstate.Measurement.AnalysisParams = cntap;
				N.App.LMBD.ReplaceCounters(det, cntap);
				PreserveAnalyzerChanges = false;
			}
            if (N.App.AppContext.modified)
                N.App.LMBD.UpdateLMINCCAppContext();
        }


		/// <summary>
		/// Build analyzers, update db with state, ready to run
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OK_Click(object sender, EventArgs e)
        {
            // 1: create the analysis param objects
            // 2: associate the analysis params with the current detector/instrument definition (part of the contextual measurement state)
            // 3: assuming the measurement state is ready, start the live performance or the file-based IO
            PreserveNewState();
            SaveAcqStateChanges();   // for comment field use

            ///
            /// Because the FAOn, FAOff or coin settings may have changed, the CountingAnalysisResults and INCCMethodResults maps must be reconstructed
            ///
            N.App.Opstate.Measurement.InitializeResultsSummarizers();
            N.App.Opstate.Measurement.INCCAnalysisState.ClearINCCAnalysisResults();
            N.App.Opstate.Measurement.PrepareINCCResults();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Step4SaveExit_Click(object sender, EventArgs e)
        {
            PreserveNewState();
            SaveAcqStateChanges();
            DialogResult = DialogResult.Yes;
            Close();
        }

		private void Step2BSaveExit_Click(object sender, EventArgs e)
		{
            PreserveNewState();
            SaveAcqStateChanges();
            DialogResult = DialogResult.Yes;
            Close();
		}

		private void Step2ASaveExit_Click(object sender, EventArgs e)
		{
			Step2BSaveExit_Click(sender, e);
		}
		private void Step3SaveExit_Click(object sender, EventArgs e)
		{
			Step4SaveExit_Click(sender, e);
		}
        void SaveAcqStateChanges()
        {
			if (!FromINCC5Acquire && ap.qc_tests)
			{
				ap.qc_tests = false; ap.modified = true;
			}
            if (ap.modified || ap.lm.modified)
            {
				Measurement meas = N.App.Opstate.Measurement;
                INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, ap.item_type, DateTime.Now);
                ap.MeasDateTime = sel.TimeStamp; ap.lm.TimeStamp = sel.TimeStamp;
                N.App.DB.AddAcquireParams(sel, ap);  // update acquire and lmacquire tables with this new one
				if (LMParamUpdate)
				{
					meas.AcquireState.lm.Results = ap.lm.Results;
					meas.AcquireState.lm.IncludeConfig = ap.lm.IncludeConfig;
					meas.AcquireState.lm.SaveOnTerminate = ap.lm.SaveOnTerminate;
					meas.AcquireState.lm.Cycles = ap.lm.Cycles;
					meas.AcquireState.lm.Interval = ap.lm.Interval;
					LMParamUpdate = false;
				}
				if (AcqParamUpdate)
				{
					N.App.Opstate.Measurement.AcquireState.data_src = ap.data_src; // copy any new changes to the current measurement
					AcqParamUpdate = false;
				}
            }
        }

        private void Comment_Leave(object sender, EventArgs e)
        {
            if ((((TextBox)sender).Text) != ap.comment)
            {
                ap.modified = true;
                ap.comment = ((TextBox)sender).Text;
            }
        }
        private void Cancel_Click(object sender, EventArgs e)
		{
            Close();
		}

		private void SwapInputsL_Click(object sender, EventArgs e)
		{
			if (ap.data_src == ConstructedSource.Live)
				AcqParamUpdate = true;
            ap.data_src = ConstructedSource.PTRFile; 
			if (N.App.AppContext.PTRFileAssay)
				ap.data_src = ConstructedSource.PTRFile; 
			else if (N.App.AppContext.PulseFileAssay)
			    ap.data_src = ConstructedSource.SortedPulseTextFile; 
			else if (N.App.AppContext.NCDFileAssay)
		        ap.data_src = ConstructedSource.NCDFile; 
			else if (N.App.AppContext.MCA527FileAssay)
	            ap.data_src = ConstructedSource.MCA527File;
			Swap(false);
		}

		private void SwapInputsR_Click(object sender, EventArgs e)
		{
			if (ap.data_src != ConstructedSource.Live)
				AcqParamUpdate = true;
            ap.data_src = ConstructedSource.Live;
			Swap(true);
		}
        private void FSave_Click(object sender, EventArgs e)
        {
            SaveAcqStateChanges();
        }

        private void DSave_Click(object sender, EventArgs e)
        {
            SaveAcqStateChanges();
        }
        private void ASave_Click(object sender, EventArgs e)
        {
            PreserveNewState();
            SaveAcqStateChanges();
        }
        // NEXT: consider a pop-out report dialog with tabs for each report rather than the concatenated list  4 hrs 
        void ReportPreview()
        {

            RawAnalysisReport rrep = new RawAnalysisReport(N.App.Loggers.Logger(LMLoggers.AppSection.Control));
            rrep.GenerateInitialReportContent(N.App.Opstate.Measurement);

            Array sv = Enum.GetValues(typeof(MethodResultsReport.INCCReportSection));
            bool[] SectionChoices = new bool[sv.Length];
            SectionChoices[(int)MethodResultsReport.INCCReportSection.Header] = true;
            SectionChoices[(int)MethodResultsReport.INCCReportSection.Adjustments] = true;
            SectionChoices[(int)MethodResultsReport.INCCReportSection.Context] = true;
            SectionChoices[(int)MethodResultsReport.INCCReportSection.ShiftRegister] = true;
            SectionChoices[(int)MethodResultsReport.INCCReportSection.Isotopics] = true;
            MethodResultsReport mrep = new MethodResultsReport(N.App.Loggers.Logger(LMLoggers.AppSection.Control));
            mrep.ApplyReportSectionSelections(SectionChoices);
            mrep.GenerateInitialReportContent(N.App.Opstate.Measurement);

            ReportView.Items.Clear();

            int rep = 1;
            foreach (List<string> r in mrep.INCCResultsReports)
            {
                if (mrep.INCCResultsReports.Count > 0)
                    ReportView.Items.Add(new ListViewItem(new string[] { "############ INCC Report " + rep.ToString() }));
                foreach (string s in r)
                {
                    ReportView.Items.Add(new ListViewItem(new string[] { s }));
                }
                rep++;
            }
            if (rep > 1)  // add blank line for readability
				ReportView.Items.Add(new ListViewItem(new string[] { "" }));
            ReportView.Items.Add(new ListViewItem(new string[] { "############ List Mode CSV Report" }));
            foreach (string s in rrep.replines)
            {
                ReportView.Items.Add(new ListViewItem(new string[] { s }));
            }
        }
		void LoadDescriptiveViews(bool live)
		{
			LiveInputDescription.Visible = live;
			FileInputsView.Visible = !live;
            ReportPreview();
            if (live)
			{
				LiveInputDescription.Text = 
				 "Detector name: " + det.Id.DetectorId + "\r" +
                 "Shift register type: " + det.Id.SRType.INCC5ComboBoxString() + "\r" +
                 "Type description: " + det.Id.Type + "\r" + 
				 "Electronics description: " + det.Id.ElectronicsId;
			}
			if (!live)
			{
				FileInputsView.Items.Clear();
				if (N.App.AppContext.FileInputList == null)
				{
					ListViewItem lvi = null;
					bool isdir = Directory.Exists(N.App.AppContext.FileInput);
					bool isfile = System.IO.File.Exists(N.App.AppContext.FileInput);
					if (isdir)
						lvi = new ListViewItem(new string[] {
							"folder",
                            Directory.GetLastWriteTime(N.App.AppContext.FileInput).ToString("yy.MM.dd  HH:mm:ss"),
                            N.App.AppContext.FileInput
							});					
					else if (isfile)
						lvi = new ListViewItem(new string[] {
							System.IO.Path.GetFileName(N.App.AppContext.FileInput),
							System.IO.File.GetLastWriteTime(N.App.AppContext.FileInput).ToString("yy.MM.dd  HH:mm:ss"),
							System.IO.Path.GetDirectoryName(N.App.AppContext.FileInput)
					});	
					if (lvi != null) FileInputsView.Items.Add(lvi);
				}
				else
				{
					ListViewItem lvi = null;
					foreach (string s in N.App.AppContext.FileInputList)
					{
                        bool isfile = System.IO.File.Exists(s);
                        string fp = s;
                        if (isfile)
                        {
                            fp = System.IO.Path.GetFullPath(s);
                            lvi = new ListViewItem(new string[] {
                                System.IO.Path.GetFileName(fp),
                                System.IO.File.GetLastWriteTime(fp).ToString("yy.MM.dd  HH:mm:ss"),
                                System.IO.Path.GetDirectoryName(fp)
                            });
                        }
                        else
                            lvi = new ListViewItem(new string[] {
                                System.IO.Path.GetFileName(fp),
                                "-",
                                System.IO.Path.GetDirectoryName(fp)
                            });
                        if (lvi != null) FileInputsView.Items.Add(lvi);
					}
				}
			}
		}
		void RefreshDetectorCombo(ComboBox cb)
        {
            // Populate the combobox in the selector panel
            cb.Items.Clear();
            foreach (Detector d in N.App.DB.Detectors)
            {
                if (d.ListMode)
                    cb.Items.Add(d);
            }
        }
        private void Step2BDetectorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            det = (Detector)((ComboBox)sender).SelectedItem;
        }
        private void CycleNumTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number  
            int ap_num_runs = ap.lm.Cycles;
            ap.lm.modified |= (Format.ToPNZ(((TextBox)sender).Text, ref ap_num_runs));
            ((TextBox)sender).Text = Format.Rend(ap_num_runs);
            if (ap.lm.modified)
			{
				ap.modified = ap.lm.modified;
				ap.lm.Cycles = ap_num_runs;
				LMParamUpdate = true;
			}
        }
        private void IntervalTextBox_Leave(object sender, EventArgs e)
        {
            // Try to convert the text to a positive, non-zero number
            double ap_run_count_time = ap.lm.Interval;
            ap.lm.modified |= (Format.ToPNZ(((TextBox)sender).Text, ref ap_run_count_time));
            if (ap.lm.modified)
			{
				ap.modified = ap.lm.modified;
	            ap.lm.Interval = ap_run_count_time;
				LMParamUpdate = true;
			}
            ((TextBox)sender).Text = Format.Rend(ap_run_count_time);
        }

        private void CycleIntervalPatch() // patches are nearly always a bad thing eh wot?
        {
            if (ap.lm.Interval <= 0)
            {
                ap.lm.Interval = ap.run_count_time;
                ap.modified = ap.lm.modified = true;
            }

            if (ap.lm.Cycles < 1)
            {
                ap.lm.Cycles = ap.num_runs;
                ap.modified = ap.lm.modified = true;
            }
        }
        private void ReviewDetector_Click(object sender, EventArgs e)
        {
            Detector d = (Detector)Step2BDetectorComboBox.SelectedItem;
            if (d == null)
                return;
            int idx = -1, i = 0;
            if (d != null)
                foreach (object o in Step2BDetectorComboBox.Items)
                {
                    if (o.ToString().CompareTo(d.ToString()) == 0)
                    {
                        idx = i;
                        break;
                    }
                    i++;
                }

            if (d.ListMode)
            {
                LMConnectionParams f = new LMConnectionParams(d, ap, false);
                f.StartWithLMDetail();
                f.ShowDialog();
                if (f.DialogResult == DialogResult.OK)
                {
                }
            }
            else
            {
                MeasSetup f = new MeasSetup();
                f.InitialSelection = idx;
                f.ShowDialog();
                if (f.DialogResult == true)
                {

                }
            }
        }

		void Swap(bool livePage)
		{
			LoadParams(livePage ? LMSteps.DAQBased : LMSteps.FileBased);
			if (livePage)
				tabControl1.SelectedIndex = 1;
			else
				tabControl1.SelectedIndex = 0;
			tabControl1.Refresh();
		}

		void SelectTheBestINCC5AcquireVSRRow()
		{
			int idx = N.App.Opstate.Measurement.AnalysisParams.FindIndex(g => g.Rank == SpecificCountingAnalyzerParams.Select);
			if (idx < 0)
				return;
			LoadParams(LMSteps.AnalysisSpec);  // this will be the initial load
			tabControl1.SelectedIndex = 2;
			SelectTheRankedRow();
			tabControl1.Refresh();
		}


		void AnalyzerGridView_DataGridViewCellEventHandler(object sender, DataGridViewCellEventArgs e)
		{
			DataGridViewRow row = AnalyzerGridView.Rows[e.RowIndex];
			if (row == null || row.Tag == null)
				return;
			if (row.Cells[e.ColumnIndex].ReadOnly)  // anything readonly means not a VSR
			{
				AnalyzerGridView.Columns[3].HeaderText = "--";
				AnalyzerGridView.Columns[4].HeaderText = "--";
				return;
			}
			SpecificCountingAnalyzerParams r = (SpecificCountingAnalyzerParams)row.Tag;
			if (r == null)
				return;

			if (r.GetType().Equals(typeof(Multiplicity)))
			{
				AnalyzerGridView.Columns[3].HeaderText = "Predelay";
				if (((Multiplicity)r).FA == FAType.FAOn)
					AnalyzerGridView.Columns[4].HeaderText = "Bkg clock width";
				else
					AnalyzerGridView.Columns[4].HeaderText = "Long delay";
			}
			else if (r.GetType().Equals(typeof(Coincidence)))
			{
				AnalyzerGridView.Columns[3].HeaderText = "Predelay";
				AnalyzerGridView.Columns[4].HeaderText = "Long delay";
			} else
			{
				AnalyzerGridView.Columns[3].HeaderText = "---";
				AnalyzerGridView.Columns[4].HeaderText = "---";
			}

		}


        ComboBox cbm;
        DataGridViewCell currentCell;

        void AnalyzerGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Here add subscription for selected index changed event
            if (e.Control is ComboBox)
            {
                cbm = (ComboBox)e.Control;
                if (cbm != null)
                {
                    cbm.SelectedIndexChanged += new EventHandler(cbm_SelectedIndexChanged);
                }
                currentCell = AnalyzerGridView.CurrentCell;
            }
        }
 
        void cbm_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Invoke method if the selection changed event occurs
            BeginInvoke(new MethodInvoker(EndEdit));
        }

		void EndEdit()
        {
            // Change the content of appropriate cell when selected index changes
            if (cbm != null)
            {
				Type t; FAType FA;
				if (cbm.SelectedItem == null)
					return;
				TTypeMap((string)cbm.SelectedItem, out t, out FA);
				DataGridViewRow row = AnalyzerGridView.Rows[currentCell.RowIndex];
				if (row == null) // empty row, so just skip it
					return;
				SpecificCountingAnalyzerParams r = (SpecificCountingAnalyzerParams)row.Tag;
				if (r == null) // empty row, so fill it in with defaults
					ConstructNewRow(row, t, FA);				
				else
					// Update row with field defaults
					ReconstructRow(row, t, FA);	
				PreserveAnalyzerChanges = true;	
            }
        }

		string HeaderX(Type t, FAType FA)
		{
			if (t.Equals(typeof(Multiplicity)) && FA == FAType.FAOn)
			{
				return "Bkg clock width";
			}
			else if ((t.Equals(typeof(Multiplicity)) && FA == FAType.FAOff) ||
					  t.Equals(typeof(Coincidence)))
			{
				return "Long delay";
			}
			return "";
		}

		//////////////////////// Utilities
		public static void ResetMeasurement()
        {

            if (N.App.Opstate.Measurement != null)
            {
                N.App.Opstate.Measurement = null;
                LMLoggers.LognLM log = N.App.Loggers.Logger(LMLoggers.AppSection.Control);
                long mem = GC.GetTotalMemory(false);
                log.TraceEvent(LogLevels.Verbose, 4255, "Total GC Memory is {0:N0}Kb", mem / 1024L);
                log.TraceEvent(LogLevels.Verbose, 4248, "GC now");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                log.TraceEvent(LogLevels.Verbose, 4284, "GC complete");
                mem = GC.GetTotalMemory(true);
                log.TraceEvent(LogLevels.Verbose, 4255, "Total GC Memory now {0:N0}Kb", mem / 1024L);
            }
        }

	}
}
