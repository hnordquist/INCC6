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
using System.Net.Sockets;
using System.Text;
using System.Threading;
using DetectorDefs;
using NCCReporter;
using Instr;
namespace DAQ
{

    public partial class DAQControl
    {

        /// <summary>
        /// Handle data from the client sockets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void SL_DataReceived(object sender, SocketAsyncEventArgs e)
        {
            Socket s = e.UserToken as Socket;
            System.Net.IPEndPoint ep = (System.Net.IPEndPoint)s.RemoteEndPoint;

            packets = Interlocked.Increment(ref packets);
            // call the specific instrument handler here using the ID.
            // This routine is needed if there are more than one type of instrument.
            // match with port it came from, each connection will have a different port.
            LMInstrument activeInstr = Instruments.Active.MatchByPort(ep.Port);  // dev note: this might be slow, could speed up this by switching to a map rather than a list
            bool verbose = gControl.collog.ShouldTrace(LogLevels.Verbose);
            if (verbose)
            {
                PacketLogSLOW(activeInstr, e);
            }
            if (activeInstr.DAQState == DAQInstrState.ReceivingData)
            {
                activeInstr.NumProcessedRawDataBuffers++;

                if (verbose) /* diag buffer tracing */
                {
                    string temp = string.Format("{0}: bytes {1}, total bytes {2}, LM {3}",
                                             activeInstr.NumProcessedRawDataBuffers, e.BytesTransferred, numTotalBytes, Instruments.Active.IndexOf(activeInstr));
                    gControl.collog.TraceEvent(LogLevels.Verbose, 686, temp);
                }

                try
                {

                    if (CurState.IsQuitRequested)
                        throw new AnalysisDefs.CancellationRequestedException();
                    else if ((e.BytesTransferred % 2) == 0) // EVEN messages are data.
                    {
                        // dev note: file writing is controlled via LiveFileWrite
                        (activeInstr.file as NCCFile.NCDFile).Write(e.Buffer, e.Offset, e.BytesTransferred);

                        try
                        {
                            // this method copies the buffer and sends it on to neutron counting
                            var res = activeInstr.RDT.PassBufferToTheCounters(e.Buffer, e.Offset, e.BytesTransferred);
                            if (res != null) //Handle this error condition as if it is a regular ODD-count packet with the end marker
                            {
                                HandleEndOfCycleProcessing(activeInstr, res);
                            }
                        }
                        catch (Exception ex)
                        {
                            HandleFatalGeneralError(activeInstr, ex);
                        }
                    }
                    else //    ODD - last data is the status message.
                    {
                        try
                        {
                            LMComm.LMMMLingo.Tokens response = LMMMComm.DataPacketResponseMatch(e.Buffer, e.Offset, e.BytesTransferred);
                            // command parsing here should be as fast as possible because data collection is active and may be hampered by even short delays in this section
                            // it is a status message, the last message of an assay
                            if (response == LMComm.LMMMLingo.Tokens.statusdata) // dev note: must be odd length and end with '......' or '......' (old style) or else start with 'Status\r\n' (new style Nov. 2010)
                            {
                                try // // write the last message to the file.This might be a partial data + last message
                                {
                                    (activeInstr.file as NCCFile.NCDFile).Write(e.Buffer, e.Offset, e.BytesTransferred);
                                    (activeInstr.file as NCCFile.NCDFile).CloseWriter();
                                }
                                catch (Exception fex)
                                {
                                    if ((activeInstr.file as NCCFile.NCDFile).stream != null)
                                        gControl.collog.TraceEvent(LogLevels.Error, 642, "{0}: error on close {1}", (activeInstr.file as NCCFile.NCDFile).stream.Name, fex.Message);
                                }

                                var res = activeInstr.RDT.PassBufferToTheCounters(e.Buffer, e.Offset, e.BytesTransferred);
                                HandleEndOfCycleProcessing(activeInstr, res);
                            }
                            else if (response == LMComm.LMMMLingo.Tokens.tosenddatasize) // this says how many bytes are going to be sent
                            {
                                numTotalBytes = LMMMComm.ResponseToSendDataSize(e.Buffer, e.Offset, e.BytesTransferred);  // t
                                gControl.collog.TraceEvent(LogLevels.Verbose, 654, "Expecting " + Convert.ToString(numTotalBytes) + " (to sen)");
                            }
                            else if (response == LMComm.LMMMLingo.Tokens.unrecognizeddata) // unrecognized command
                            {
                                string temp = LMMMComm.ResponseUnrecoSample(e.Buffer, e.Offset, e.BytesTransferred);
                                gControl.collog.TraceInformation(temp);
                            }
                            else // it might be an odd message with some data tacked on the beginning
                            {
                                if (gControl.collectingFileData)
                                {
                                    (activeInstr.file as NCCFile.NCDFile).Write(e.Buffer, e.Offset, e.BytesTransferred);
                                    (activeInstr.file as NCCFile.NCDFile).CloseWriter();
                                }
                                if (response == LMComm.LMMMLingo.Tokens.rates) // somehow got a rates block
                                {
                                    RatesStatus p = new RatesStatus();
                                    String received = LMMMComm.NonDataResponse(e.Buffer, e.Offset, e.BytesTransferred);
                                    LMMMComm.SplitRatesReadResponse(received, ref p);
                                    gControl.collog.TraceInformation("ReceivingData rates {0} on LM {1}:{2}", p.ToString(), Instruments.Active.IndexOf(activeInstr), activeInstr.id.DetectorId);
                                }
                                CurState.State = DAQInstrState.Online;
                                gControl.collog.TraceInformation("Cycle " + CurState.Measurement.CurrentRepetition + " complete");
                                if ((CurState.Measurement.CurrentRepetition < CurState.Measurement.RequestedRepetitions) || (CurState.Measurement.RequestedRepetitions == 0))
                                {
                                    bool ok = activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement);
                                    if (ok)
                                    {
                                        gControl.FireEvent(EventType.ActionInProgress, gControl);
                                        gControl.StartLM_SRAssay();
                                    }
                                    else
                                        gControl.MajorOperationCompleted(); // the overall pend handle used by cmd line 
                                }
                                else
                                {
                                    gControl.collog.TraceInformation("All assay cycles completed, but with data in an odd-sized packet");
                                    bool ok = activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement, last:true);
                                    gControl.MajorOperationCompleted(); // the overall pend handle used by cmd line 
                                }
                            }
                        }
                        catch (AnalysisDefs.FatalNeutronCountingException fae)  // propagated up from the AnalysisHandler 
                        {
                            HandleFatalGeneralError(activeInstr, fae);
                        }
                        catch (AnalysisDefs.CancellationRequestedException)  // thrown from this method
                        {
                            StopActiveAssayImmediately();
                        }
                        catch (Exception oddex)
                        {
                            gControl.collog.TraceEvent(LogLevels.Error, 643, "Internal error, stopping active processing, {0}: Odd data length handler: {1}",
                                activeInstr.NumProcessedRawDataBuffers, oddex.Message);
                            HandleFatalGeneralError(activeInstr, oddex);
                        }
                    }
                }
                catch (AnalysisDefs.CancellationRequestedException)  // thrown from this method
                {
                    StopActiveAssayImmediately();
                }
                if (verbose) gControl.collog.Flush();
            }
            else if (activeInstr.DAQState == DAQInstrState.Online || activeInstr.DAQState == DAQInstrState.HVCalib) // we are not taking data.
            {
                String received = LMMMComm.NonDataResponse(e.Buffer, e.Offset, e.BytesTransferred);

                if (activeInstr.IsNew())
                {
                    string iname = String.Empty;
                    string type = InstrType.LMMM.ToString();
                    LMMMComm.SplitBroadcastResponse(received, ref type, ref iname, ref activeInstr.id.version);
                    activeInstr.id.DetectorId = iname;
                    activeInstr.id.SetSRType(type);
                    gControl.collog.TraceInformation("The new instrument is " + LMLoggers.LognLM.FlattenChars(received));
                    gControl.collog.TraceInformation(Instruments.All.Count + " instrument(s) online");
                    activeInstr.selected = true;  // this should only be set if there is no UI
                    CurState.State = DAQInstrState.Online;
                    gControl.FireEvent(EventType.ActionPrep, gControl);
                }
                else
                {
                    // slow parsing in this section is fine because data collection is not occurring
                    LMComm.LMMMLingo.Tokens response = LMMMComm.ResponseMatchPrefix(received);
                    if (response == LMComm.LMMMLingo.Tokens.cstatus) //  it is a status readback
                    {
                        LMMMComm.SplitCStatusResponse(received, ref activeInstr.lmstatus);
                        gControl.collog.TraceInformation(
                            "cStatus for LM {0}:{1} is dbg:{2}, leds:{3}, input:{4}, HVset:{5}, HV:{6}, LLD Max:{7}, LLD:{8}, (u1:{9})",
                            Instruments.Active.IndexOf(activeInstr), activeInstr.id.DetectorId, activeInstr.lmstatus.debug, activeInstr.lmstatus.leds, activeInstr.lmstatus.inputPath, activeInstr.lmstatus.setpoint, activeInstr.lmstatus.hv,
                            activeInstr.lmstatus.MaxLLD, activeInstr.lmstatus.LLD, activeInstr.lmstatus.u1);
                    }
                    else if (response == LMComm.LMMMLingo.Tokens.hvread) // it is a hv readback
                    {
                        int hv = 0;
                        LMMMComm.SplitHVReadResponse(received, ref hv);
                        gControl.collog.TraceInformation("HVread {0} volts for LM {1}:{2}", hv,
                             Instruments.Active.IndexOf(activeInstr), activeInstr.id.DetectorId);
                    }
                    else if (response == LMComm.LMMMLingo.Tokens.hvcalib) // it is a hv calibration point
                    {
                        HVControl.HVStatus hvst = new HVControl.HVStatus();
                        LMMMComm.SplitHVCalibResponse(received, ref hvst);
                        gControl.AppendHVCalibration(hvst);
                        gControl.collog.TraceInformation("HVcalib for LM {0}:{1} is [setpt:{2}, read:{3},  . . .]",
                              Instruments.Active.IndexOf(activeInstr), activeInstr.id.DetectorId, hvst.HVsetpt, hvst.HVread);
                        gControl.StepHVCalibration();
                    }
                    else if (response == LMComm.LMMMLingo.Tokens.rates) // it is a rates response
                    {
                        RatesStatus p = new RatesStatus();
                        LMMMComm.SplitRatesReadResponse(received, ref p);
                        gControl.collog.TraceInformation("Rates {0} on LM {1}:{2}", p.ToString(),
                             Instruments.Active.IndexOf(activeInstr), activeInstr.id.DetectorId);
                    }
                    else if (response == LMComm.LMMMLingo.Tokens.power) // it is a power status 
                    {
                        PowerStatus p = new PowerStatus();
                        LMMMComm.SplitPowerReadResponse(received, ref p);
                        gControl.collog.TraceInformation("Power AC {0}, Batt {1}, Batt Level {2} on LM {3}:{4}", p.ACPresent, p.batteryPresent, p.batterylevelPct,
                             Instruments.Active.IndexOf(activeInstr), activeInstr.id.DetectorId);
                    }
                    else if (response == LMComm.LMMMLingo.Tokens.lld) // LLD status
                    {
                        int lld = 0;
                        LMMMComm.SplitLLDReadResponse(received, ref lld);
                        gControl.collog.TraceInformation("LLD {0} on LM {1}:{2}", lld,
                             Instruments.Active.IndexOf(activeInstr), activeInstr.id.DetectorId);
                    }
                    else // RR: this could be binary data if you cancel an assay and linux is sending a buffer.
                    // RR: this is a large amount of data and we don't want it displayed.
                    // tbd RR: figure out how to turn this back on when data is finished dumping.
                    {
                        if (verbose)
                        {
                            PacketLogSLOW(activeInstr, e);
                        }
                    }
                }
                if (verbose) gControl.collog.Flush();
            }
        }
        

         /// <summary>
        /// Checks control state for DAQ continuation, then
        /// If at the end of a cycle
        /// 1) finishes cycle processing (wait for analyzer completion, get results, summarize counts, cycle conditioning steps)
        ///   a) starts the next assay cycle, or
        ///   b) notifies controller of DAQ measurement completion 
        /// </summary>
        /// <param name="activeInstr">instrument object associated with the current DAQ state</param>
        /// <param name="sb">status block object indicating end of a cycle</param>
        internal static void HandleEndOfCycleProcessing(LMInstrument activeInstr, Analysis.StreamStatusBlock sb)
        {

            activeInstr.RDT.PlaceStatusTextOnCurrentCycle(sb);
            
            bool done = true; // assume done for all instruments

            activeInstr.DAQState = DAQInstrState.Online;

            // tbd RR: need to lock this
            // for each networked instrument
            // check ALL instrument for done so we can set the assay.state to done or repeat
            // only look at active instrs in the listbox list.
            for (int j = 0; j < Instruments.Active.Count; j++) // NEXT: revisit this for mixing of the LM and SR behaviors
            {
                if (Instruments.Active[j].DAQState != DAQInstrState.Online)
                {
                    done = false;
                    break;
                }
            }

            if (done) // all are done
            {
                CurState.State = DAQInstrState.Online;
                gControl.collog.TraceInformation("Assay cycle " + CurState.Measurement.CurrentRepetition + " complete");
                if ((CurState.Measurement.CurrentRepetition < CurState.Measurement.RequestedRepetitions) ||
                    (CurState.Measurement.RequestedRepetitions == 0) && !CurState.IsQuitRequested)
                {
                    // dev note: this could   be spawned in a task because the end processing can be time consuming, delaying the start of next DAQ iteration 
                    bool ok = activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement);
                    if (ok)  // start the next cycle
                    {
                        gControl.FireEvent(EventType.ActionInProgress, gControl);
                        gControl.StartLM_SRAssay();
                    }
                    else
                        gControl.MajorOperationCompleted(); // the overall pend handle used by cmd line 
                }
                else
                {
                    gControl.collog.TraceInformation("All assay cycles completed");
                    activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement);  // do final processing on the last cycle, then do the last cycle closure processing
                    activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement,  last:true);
                    gControl.MajorOperationCompleted(); // the overall pend handle used by cmd line 
                }
            }
        }
        /// <summary>
        /// Handle a fatal exception from the neutron counting code during live DAQ
        /// Stop current assay, finish cycle summary, close NCD file, and signal controller to close down and write summary output
        /// </summary>
        /// <param name="activeInstr">Instrument source of data under analysis that raised the exception</param>
        /// <param name="e">exception instance raised, carries message for status recording</param>
        internal static void HandleFatalGeneralError(Instrument activeInstr, Exception e)
        {
            CurState.State = DAQInstrState.Online; // remaining buffers should now bypass DAQ section
            gControl.StopLMCAssay(false); // stop the instruments and close the open file
            gControl.collog.TraceException(e, false);
            if (activeInstr is LMInstrument)
                gControl.collog.TraceEvent(LogLevels.Info, 429, "Neutron counting incomplete: {0}, processing stopped at buffer {1}", e.Message, (activeInstr as LMInstrument).RDT.NumProcessedRawDataBuffers);
            else
                gControl.collog.TraceEvent(LogLevels.Info, 429, "DAQ processing incomplete: {0}, processing stopped", e.Message);
            activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement, last:true);
            gControl.MajorOperationCompleted();  // signal the controlling loop we are done
        }

        // test: this hasn't been tested well, and probably needs some work
        /// <summary>
        /// Handle a cancellation from the neutron counting code during live DAQ
        /// Stop current assay, dont wait for more data, close NCD file, and signal controller to close down and write summary output
        /// </summary>
        /// <param name="activeInstr">Instrument source of data that was active when cancellation request dectected</param>
        internal static void StopActiveAssayImmediately()
        {
            CurState.Measurement.RequestedRepetitions = CurState.Measurement.CurrentRepetition; // will this work ? ;)
            CurState.State = DAQInstrState.Online; // remaining buffers should now bypass DAQ section
            gControl.StopLMCAssay(false); // stop the instruments and close the NCD file
            gControl.collog.TraceEvent(LogLevels.Info, 428, "DAQ cancelled");
            foreach (Instrument activeInstr in Instruments.Active)
            {
                activeInstr.RDT.EndOfCycleProcessing(CurState.Measurement, last:true);
            }
            gControl.MajorOperationCompleted();  // signal the controlling loop we are done
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void SL_ClientConnected(object sender, SocketAsyncEventArgs e)
        {
            connections = Interlocked.Increment(ref connections);
            Socket s = e.UserToken as Socket;
            System.Net.IPEndPoint ep = (System.Net.IPEndPoint)s.RemoteEndPoint;

            gControl.collog.TraceInformation("New Connection : " + s.RemoteEndPoint.ToString());

            LMInstrument lmi = new LMInstrument(e, ep.Port);
            Instruments.All.Add(lmi);
            // dev note: need a parallel entry in the counting parameters map on the enclosing Measurement for each instrument and it;s SR params class instance

            // get the status of the instrument
            DAQControl.ReadInstrStatus(lmi);
        }

        // used for verbose data logging only
        static string bytestoASCIIchars(byte[] bdata, int len, int max)
        {
            int i;
            char dchar;
            if (len > max) len = max;
            StringBuilder dumptext = new StringBuilder();
            for (i = 0; i < len; i++)
            {
                dchar = (char)bdata[i];
                // replace 'non-printable' chars with a '.'.
                if (Char.IsControl(dchar))
                {
                    dchar = '.';
                }
                dumptext.Append(dchar);
            }
            return dumptext.ToString();
        }

        static void PacketLogSLOW(LMInstrument activeInstr, SocketAsyncEventArgs e)
        {
            // my little hack by Joe Longo, where I learn by doing
            byte[] bstr1 = new byte[e.BytesTransferred];
            Array.Copy(e.Buffer, e.Offset, bstr1, 0, e.BytesTransferred);
            string tesmp = bytestoASCIIchars(bstr1, e.BytesTransferred, 132);
            string eos = ((e.BytesTransferred % 2) == 0 ? "EVEN " : "ODD ");
            ulong buffo = 0;
            if (activeInstr != null)
            {
                if (activeInstr.RDT != null)
                    buffo = activeInstr.NumProcessedRawDataBuffers;
                eos += activeInstr.DAQState.ToString();
            }
            gControl.collog.TraceEvent(LogLevels.Verbose, 696, "{0}:{1} {2} bytes, state {3}", buffo, packets, e.BytesTransferred, eos);
            gControl.collog.TraceEvent(LogLevels.Verbose, 697, ">>>" + tesmp);
        }
    }
}