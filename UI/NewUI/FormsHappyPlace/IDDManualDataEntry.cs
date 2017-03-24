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
using DetectorDefs;
using NCCReporter;
namespace NewUI
{  
    
    using NC = NCC.CentralizedState;

    public partial class IDDManualDataEntry : Form
    {
       
        void ClipboardMonitor_OnClipboardChange(ClipboardFormat format, object data)
        {
            m_log.TraceEvent(LogLevels.Verbose, 76543, "Clipboard changed ... format now: " + format.ToString());
        }

        public AcquireHandlers AH {set {ah = value; } }

        public IDDManualDataEntry()
        {
            m_refDate = DateTime.Now;

            InitializeComponent();

            ResetGrid();

            m_log = NC.App.DataLogger;
            ClipboardMonitor.Start();
            ClipboardMonitor.OnClipboardChange += new ClipboardMonitor.OnClipboardChangeEventHandler(ClipboardMonitor_OnClipboardChange);
            cyclesGridView.AutoResizeColumn(0, DataGridViewAutoSizeColumnMode.AllCellsExceptHeader);
        }

        void ResetGrid()
        {
            cyclesGridView.Rows.Clear();
            cyclesGridView.Rows.Add(MAX_MANUAL_ENTRIES);
            //Start with 200 empty rows
            for (int i = 0; i < MAX_MANUAL_ENTRIES; i++)
            {
                cyclesGridView.Rows[i].Cells[0].Value = (i + 1).ToString();
            }

        }

        void LoadFromCurrentCycles()
        {
            ResetGrid();
            foreach (Cycle c in NC.App.Opstate.Measurement.Cycles)
            {
                MultiplicityCountingRes res = c.MultiplicityResults(ah.det.MultiplicityParams); // APluralityOfMultiplicityAnalyzers: expand in some logical manner, e.g. enable user to select the analyzer and related results for manual entry and assignment
                DataGridViewRow r = cyclesGridView.Rows[c.seq - 1];
                r.Cells[1].Value = c.Totals.ToString();
                if (res != null)
                {
                    r.Cells[2].Value = res.RASum;
                    r.Cells[3].Value = res.ASum;
                }
                else
                {
                    r.Cells[2].Value = "0";
                    r.Cells[3].Value = "0";
                }
            }
        }


        void ClearMeasCycles()
        {
            NC.App.Opstate.Measurement.Cycles.Clear();
            // todo: need to null out other values on the measurement too...
        }

        public void FieldValorizer(TextBox t, ref double tgt, double low = Double.MinValue, double high = Double.MaxValue)
        {
            double d = tgt;
            bool modified = (Format.ToDbl(t.Text, ref d));
            if (modified && d >= low && d <= high) { tgt = d; }
            t.Text = tgt.ToString("F1");
        }


        private void CountTimeTextBox_Leave(object sender, EventArgs e)
        {
            FieldValorizer((TextBox)sender, ref m_counttime, low:1, high:1e6);
        }

        private void MeausurementDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime dt = ((DateTimePicker)sender).Value;
            if (m_refDate != dt)
            {
                m_refDate = dt;
            }
        }

        private void OKBtn_Click(object sender, EventArgs e)
        {
            ClipboardMonitor.OnClipboardChange -= new ClipboardMonitor.OnClipboardChangeEventHandler(ClipboardMonitor_OnClipboardChange);
            ClipboardMonitor.Stop(); // do not forget to stop

            ClearMeasCycles();
            CycleList newCycles = new CycleList();
            // NEXT: manual entry needs more work to get it completed, but you have a good start here
            Multiplicity key = new Multiplicity(ah.det.MultiplicityParams);  // APluralityOfMultiplicityAnalyzers: expand in some logical manner, e.g. enable user to select the analyzer and related results for manual entry and assignment
            for (int i = 0; i < MAX_MANUAL_ENTRIES; i++) // hard-coded limits are ... lame
            {
                DataGridViewRow r = cyclesGridView.Rows[i];
                if (r.Cells == null || (r.Cells[1].Value == null) || r.Cells[1].Value.ToString() == string.Empty)
                    break;
                
                Cycle cycle = new Cycle(m_log);

                ulong tots = 0, r_acc = 0, acc = 0;
                ulong.TryParse(r.Cells[1].Value.ToString(), out tots);
                ulong.TryParse((string)r.Cells[2].FormattedValue, out r_acc); // FormattedValue gives "" instead of the null checked for in the conditional above 
                ulong.TryParse((string)r.Cells[3].FormattedValue, out acc);

                newCycles.Add(cycle);
                cycle.Totals = tots;
                cycle.TS = new TimeSpan(0, 0, 0, 0, (int)(1000 * m_counttime)); // milliseconds permitted for LM and future
                cycle.SinglesRate = tots / m_counttime;
                cycle.SetQCStatus(key, QCTestStatus.Pass);
                cycle.seq = i+1;

                MultiplicityCountingRes mcr = new MultiplicityCountingRes(key.FA, cycle.seq);
                cycle.CountingAnalysisResults.Add(key, mcr);
                mcr.Totals = cycle.Totals;
                mcr.TS = cycle.TS;
                mcr.ASum = acc;
                mcr.RASum = r_acc;

                // assign the hits to a single channel (0)
                cycle.HitsPerChannel[0] = tots;
                mcr.RawSinglesRate.v = cycle.SinglesRate;

                // no alpha-beta, mult bins, HV, doubles, triples, raw nor corrected

            }

            int seq = 0;
            foreach (Cycle cycle in newCycles)  // add the necessary meta-data to the cycle identifier instance
            {
                seq++;
                cycle.UpdateDataSourceId(ConstructedSource.Manual, ah.det.Id.SRType,
                    new DateTimeOffset(m_refDate.AddTicks(cycle.TS.Ticks * cycle.seq)), string.Empty);
            }
            NC.App.Opstate.Measurement.Add(newCycles);
            if (newCycles.Count > 0)
                DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            ClipboardMonitor.OnClipboardChange -= new ClipboardMonitor.OnClipboardChangeEventHandler(ClipboardMonitor_OnClipboardChange);
            ClipboardMonitor.Stop(); // do not forget to stop
            Close();
        }

        private void HelpBtn_Click(object sender, EventArgs e)
        {

        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {

        }

        private void ClearBtn_Click(object sender, EventArgs e)
        {
            ResetGrid();
        }

        private void cycleListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cyclesGridView_CellValidating_1(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex > 0)
            {
                string text = e.FormattedValue.ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    Format.WTF q = Format.ToUInt64(ref text, low: 0, high: 100000000001);
                    if (q == Format.WTF.NonNumeric)
                    {
                        cyclesGridView.Rows[e.RowIndex].ErrorText = "Entry must be a non-negative integer";
                        e.Cancel = true;
                    }
                    else if (q == Format.WTF.Range)
                    {
                        cyclesGridView.Rows[e.RowIndex].ErrorText = "Entry must be >= " + 0.ToString() +" and <= " +  100000000000.ToString();
                        e.Cancel = true;
                    }
                }
            }
        }

        private void cyclesGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Clear the row error in case the user presses ESC.   
            cyclesGridView.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void cyclesGridView_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void UseCurrent_Click(object sender, EventArgs e)
        {
            LoadFromCurrentCycles();
        }

        LMLoggers.LognLM m_log;
        const int MAX_MANUAL_ENTRIES = 200;
        AcquireHandlers ah;
        double m_counttime = 1.0;
        DateTime m_refDate;
        public bool m_useCurrentCycles;
    }
}
