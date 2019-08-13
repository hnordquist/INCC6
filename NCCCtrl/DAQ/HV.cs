/*
Copyright (c) 2016, Los Alamos National Security, LLC
All rights reserved.
Copyright 2016. Los Alamos National Security, LLC. This software was produced under U.S. Government contract 
DE-AC52-06NA25396 for Los Alamos National Laboratory (LANL), which is operated by Los Alamos National Security, 
LLC for the U.S. Department of Energy. The U.S. Government has rights to use, reproduce, and distribute this software.
NEITHER THE GOVERNMENT NOR LOS ALAMOS NATIONAL SECURITY, LLC MAKES ANY WARRANTY, EXPRESS OR IMPLIED, 
OR ASSUMES ANY LIABILITY FOR THE USE OF THIS SOFTWARE. If software is modified to produce derivative works, 
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
using System.Threading;
using DetectorDefs;
using LMComm;
using NCC;
using NCCReporter;
using Instr;
namespace DAQ
{

    using NC = NCC.CentralizedState;
 
    public class HVControl
    {
        string instId;
        DateTime time;

        LMLoggers.LognLM ctrllog = null;

        // expose these as a struct for sampling by polling for status
        public int hvCalibPoint;  // last read or target value
        public int hvMaxCalibPoint;  // max final value
		public int hvMinCalibPoint;
        public int hvStep;
        int hvDelayms;
        bool hvx;
        internal HVExcel xp;

        AnalysisDefs.HVCalibrationParameters hvp;

        DAQControl control;

        internal List<HVStatus> HVSteps = null;

        public void ResetControlVars()
        {
            HVSteps = new List<HVStatus>();
            xp = null;
            hvp = IntegrationHelpers.GetCurrentHVCalibrationParams(NC.App.Opstate.Measurement.Detector);
            hvCalibPoint = hvMinCalibPoint = hvp.MinHV;  // starting point for stepping
            hvMaxCalibPoint = hvp.MaxHV;
            hvStep = hvp.Step;
            hvDelayms = hvp.DelayMS;
            if (hvp.HVDuration < hvp.DelayMS / 1000) // seconds to milliseconds
            {
                hvp.DelayMS = (hvp.HVDuration/1000) + 1000;
                hvDelayms = hvp.DelayMS;
                ctrllog.TraceEvent(LogLevels.Warning, 604, "HV delay modified to {0} milliseconds because the HV cycle duration ({1} sec) must be less than HV delay", hvp.DelayMS, hvp.HVDuration); 
            }
            hvx = NC.App.Opstate.Measurement.AcquireState.lm.HVX; // excel monitor flag
        }
        public HVControl(DAQControl control)
        {
            this.control = control;
            ctrllog = NC.App.Loggers.Logger(LMLoggers.AppSection.Control);
        }

        public void AddStepData(HVStatus hvst)
        {
            HVSteps.Add(hvst);

            if ((xp != null) && hvx)
            {
                xp.AddRow(hvst, HVSteps.Count);
                xp.ShowWB();
            }
        }

        public bool HVStartCalibration()
        {

            // only for first active instrument.
            Instrument inst = Instruments.Active.FirstActive();
            if (inst == null)
            {
                ctrllog.TraceInformation("No active instruments for HV calibration. . .");
                return false;
            }

            DAQControl.CurState.State = DAQInstrState.HVCalib;
            DAQControl.CurState.ResetTokens();
            ResetControlVars();
            inst.PendingReset(); // mas importante LOL
            inst.DAQState = DAQInstrState.HVCalib;

            ctrllog.TraceInformation("HV calibration starting");
            HVSteps = new List<HVStatus>(200);// a bit arbitrary, but performance here is not an issue
            time = DateTime.Now;

            return HVCalibRun();
        }

        // meant to support SR and LM 
        public bool HVCalibRun()
        {
            // only for first active instrument.
            Instrument inst = Instruments.Active.FirstActive();
            if (inst == null)
            {
                ctrllog.TraceInformation("No active instruments for HV calibration. . .");
                return false;
            }
            if (hvx)
            {
                if (xp == null)
                {
					if (ExcelPush.ExcelPresent(ctrllog))
					{
						xp = new HVExcel(ctrllog);
						xp.ShowWB();
						xp.AddHeaderRow(typeof(SimpleHVReport.HVVals));
					}
                }
            }
            instId = inst.id.DetectorId + "-" + inst.id.SRType.ToString();
			bool res = true;
			if (hvCalibPoint <= hvMaxCalibPoint)
            {

				// todo: catch spurious exceptions and return false
                if (DAQControl.CurState.IsQuitRequested) // leave and do not finish calibration
                {
                    ctrllog.TraceInformation("HV calibration cancelled");
                    inst.DAQState = DAQInstrState.Online;
                    DAQControl.CurState.State = DAQInstrState.Online;
                    ctrllog.Flush();
					DAQControl.gControl.MajorOperationCompleted();  // causes pending control thread caller to move forward
					inst.PendingComplete(); // each instr must complete for the waitall to move forward 
                }
                else
                {
                    if (inst.id.SRType.IsListMode())
                    {
                        if (inst.id.SRType == InstrType.PTR32 || inst.id.SRType == InstrType.MCA527)
						{
							Thread.Sleep(hvDelayms); // wait for HV to settle, nominally 2 seconds
                            inst.StartHVCalibration(hvCalibPoint, TimeSpan.FromSeconds(hvp.HVDuration));
                        }
                        else if (inst.id.SRType == InstrType.ALMM)
						{
                            int LM = control.FirstActiveIndexOf(inst);
                            //DAQControl.ALMMComm.FormatAndSendALMMCommand(ALMMLingo.Tokens.hvprep, hvCalibPoint, LM);
                            //DAQControl.ALMMComm.FormatAndSendALMMCommand(ALMMLingo.Tokens.hvset, hvCalibPoint, LM);
                            //TODO: send hv command here.
                            Thread.Sleep(hvDelayms); // wait for HV to settle, nominally 2 seconds
                            //DAQControl.ALMMComm.FormatAndSendALMMCommand(ALMMLingo.Tokens.hvcalib, 0, LM);
                        }
                    }
                    else
                    {                        
                        // (re)init the SR
						int status = 0;
                        if (hvCalibPoint == hvMinCalibPoint)  // starting point
                            status = control.SRWrangler.StartSRActionAndWait(inst.id, SRTakeDataHandler.SROp.InitializeSR, hvCalibPoint, hvp.HVDuration);
                        else  // reinit the SR to the next step up in the plateaux
                            status = control.SRWrangler.StartSRActionAndWait(inst.id, SRTakeDataHandler.SROp.ReInitializeSR, hvCalibPoint, hvp.HVDuration);                                                 

						if (status == INCCSR.SUCCESS || status == INCCSR.MEAS_CONTINUE)
						{	
							// do the run
							status = control.SRWrangler.StartSRActionAndWait(inst.id, SRTakeDataHandler.SROp.StartSRDAQ);  // NEXT: check if pending here is going to be an issue
							if (status == INCCSR.MEAS_CONTINUE)  // the SR started
							{
								control.SRWrangler.SetAction(inst.id, SRTakeDataHandler.SROp.WaitForResults); // event handler will pick up results when the internal timer polling in the thread detects results and fires the event
							}
						}
						else
							res = false;
                    }
                    control.FireEvent(ActionEvents.EventType.ActionInProgress, control);

                    hvCalibPoint += hvStep;
                }
            }
            else // we are done
            {
                ctrllog.TraceInformation("HV calibration complete. . .");
                inst.DAQState = DAQInstrState.Online;
                DAQControl.CurState.State = DAQInstrState.Online;
                ctrllog.Flush();
                DAQControl.gControl.MajorOperationCompleted();  // causes pending control thread caller to move forward
                inst.PendingComplete(); // each instr must complete for the waitall to move forward 
            }
            return res;
        }

        public void GenerateReport()
        {
            new SimpleHVReport(ctrllog).GenerateReport(hvp, HVSteps, time, instId);
        }


        public class HVStatus
        {
            public HVStatus()
            {
                time = DateTime.Now;
            }

			public void Copy(HVStatus src)
            {
                time = src.time;
				setpt = src.setpt;
				read = src.read;
				Array.Copy(src.counts, counts, counts.Length);
            }

            // as of late 2010, this going to be 34 values, the setpt, the actual read, and the counts per channel
            public void Extract(string text)
            {
                string[] hv_text = text.Split('=');
                string[] sval = hv_text[1].Split(',');

                Int32.TryParse(sval[0], out setpt);
                Int32.TryParse(sval[1], out read);
                for (int i = 2; i < NC.ChannelCount + 2 && i < sval.Length; i++)
                {
                    UInt64.TryParse(sval[i], out counts[i - 2]);
                }
            }

            public ulong[] counts = new ulong[NC.ChannelCount];

            private int setpt, read;

            public int HVsetpt
            {
                get { return setpt; }
                set { setpt = value; }
            }

            public int HVread
            {
                get { return read; }
                set { read = value; }
            }

            public DateTime time;

        }

    }



    public class SimpleHVReport
    {
        public enum HVVals
        {
            Run, Time, HVSetPt, HVRead,
            C1, C2, C3, C4, C5, C6, C7, C8, C9, C10, C11, C12, C13, C14, C15, C16, C17, C18, C19, C20, C21, C22, C23, C24, C25, C26, C27, C28, C29, C30, C31, C32
        };
        public enum HVCalibVals
        {
            MinHV, MaxHV, StepVolts, DurationSeconds, DelaySeconds
        };
        NCCReporter.LMLoggers.LognLM ctrllog = null;

        public SimpleHVReport(NCCReporter.LMLoggers.LognLM ctrllog)
        {
            this.ctrllog = ctrllog;
        }
        public static Row CreateRow(AnalysisDefs.HVCalibrationParameters h, int i)
        {
            Row row = new Row();
            row.Add((int)HVCalibVals.MinHV, h.MinHV.ToString());
            row.Add((int)HVCalibVals.MaxHV, h.MaxHV.ToString());
            row.Add((int)HVCalibVals.DurationSeconds, h.HVDuration.ToString());
            row.Add((int)HVCalibVals.StepVolts, h.Step.ToString());
            row.Add((int)HVCalibVals.DelaySeconds, (h.DelayMS / 1000).ToString());
            return row;
        }
        public static Row CreateRow(HVControl.HVStatus h, int i)
        {
            Row row = new Row();
            row.Add((int)HVVals.Run, i.ToString());
            row.Add((int)HVVals.Time, h.time.ToString("s"));
            row.Add((int)HVVals.HVRead, h.HVread.ToString());
            row.Add((int)HVVals.HVSetPt, h.HVsetpt.ToString());

            for (int j = (int)HVVals.C1; j <= (int)HVVals.C32; j++)
            {
                row.Add(j, h.counts[j - (int)HVVals.C1].ToString());
            }

            return row;
        }
        public void GenerateReport(AnalysisDefs.HVCalibrationParameters hvc, List<HVControl.HVStatus> HVSteps, DateTime time, String instId)
        {

            TabularReport t = new TabularReport(typeof(HVVals), NC.App.Loggers);  // default file output type is CSV
            try
            {
                t.CreateOutputFile(NC.App.Opstate.Measurement.AcquireState.lm.Results, instId + " HV " + time.ToString("yyyyMMddHHmmss"), null);

                t.rows = new Row[HVSteps.Count + 4 + 2];  // header, steps, space, header, current params, space, header, config
               // first, the column header
                int cols = System.Enum.GetValues(typeof(HVVals)).Length;
                int i = 0;

                // now for each hvcalib row
                for (i = 0; i < HVSteps.Count; i++)
                {
                    HVControl.HVStatus h = HVSteps[i];
                    Row row = CreateRow(h, i);
                    t.rows[i] = row;
                }

                // then do the current parameters
                t.rows[i++] = new Row(); 
                t.rows[i] = new Row(); t.rows[i++].GenFromEnum(typeof(HVCalibVals));
                t.rows[i++] = CreateRow(hvc, i);
                t.rows[i++] = new Row(); 

                // lastly, add the full software version state
                t.rows[i] = new Row(); t.rows[i++].Add(0, "Software application configuration details");
                Row[] temp = AnalysisDefs.SimpleReport.GenSoftwareConfigRows();
                Array.Resize(ref  t.rows, temp.Length + t.rows.Length + 2);
                Array.Copy(temp, 0, t.rows, i, temp.Length);

                t.CreateReport(3);

            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
            finally
            {
                t.CloseOutputFile();
            }
        }
    }

    // open bare worksheet, add rows one-by-one as they are added to the overall HV list above
    public class HVExcel : NCCReporter.ExcelPush
    {

        // dev note: pre-define template somewhere, and use it, having the very nice graph already prepared for line by line updating here
        public HVExcel(string existingWB, NCCReporter.LMLoggers.LognLM ctrllog) : base (existingWB, ctrllog)
        {
        }

        public HVExcel(NCCReporter.LMLoggers.LognLM ctrllog)
            : base(ctrllog)
        {
        }


        // implement incremental row addition using something like this
        public void AddRow(HVControl.HVStatus h, int r)
        {
            int lc = System.Enum.GetValues(typeof(SimpleHVReport.HVVals)).Length;
            Row row = SimpleHVReport.CreateRow(h, r);
            try
            {
#if EXCEL
                for (int j = 1; j <= lc; j++)
                {
                    target.Cells[r + 1, j] = row[j - 1];
                }
#endif
            }
            catch (Exception e)
            {
                ctrllog.TraceException(e);
            }
        }
    }

}
