﻿/*
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
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using AnalysisDefs;
using DetectorDefs;
using NCCReporter;
using Instr;

namespace LMComm
{

    using NC = NCC.CentralizedState;

    // These two related classes (TalkToLMMMM and LMMMLingo) contain the command and results string patterns for the LMMM 
    // They support cmd string to operation mapping with smarts for full token expansion and replacement
    // They also wrap the physical send of final constructed command strings 
    // They also do the string to token matching and deconstruction of return strings for use in the Receive/Accept handler
    public class TalkToALMM
    {

        //ALMMLingo cmdprocessor;
        //ALMMLingo cmdprocessor;
        ALMMController almmctrl;
        //Server _LMServer = null;

        /*public Server LMServer
        {
          get { return _LMServer; }
          set { _LMServer = value; }
        }*/
        public ALMMController ALMMCtrl
        {
            get { return almmctrl; }
            set { almmctrl = value; }

        }
        //Config cfg;
        LMLoggers.LognLM commlog = null;

        public TalkToALMM(LMLoggers.LognLM logger)
        {
            //cmdprocessor = new ALMMLingo(); // same for cfg copy

            commlog = logger;
            //cmdprocessor = new ALMMLingo();
            almmctrl = new ALMMController();
            //cmdprocessor.CommLog = logger;
        }


        /*internal void CommandPromptMatchPrefix(string input, ref ALMMLingo.OpDesc op)
        {
            cmdprocessor.CommandPromptMatchPrefix(input, ref op);
        }

        // string response expected, called on data received while in Online or HVClaib DAQ state
        public ALMMLingo.Tokens ResponseMatchPrefix(string received)
        {
            return cmdprocessor.ResponseMatchPrefix(received);
        }

        // an arbitrary byte string is contained in this response because this is called on data received while in the ReceivingData DAQ state
        public ALMMLingo.Tokens DataPacketResponseMatch(byte[] buffer, int offset, int bytesTransferred)
        {
            return cmdprocessor.DataPacketResponseMatch(buffer, offset, bytesTransferred);
        }

        public String NonDataResponse(byte[] buffer, int offset, int bytesTransferred)
        {
            return cmdprocessor.NonDataResponse(buffer, offset, bytesTransferred);
        }

        public UInt32 ResponseToSendDataSize(byte[] buffer, int offset, int bytesTransferred)
        {
            return cmdprocessor.ResponseToSendDataSize(buffer, offset, bytesTransferred);
        }

        public string ResponseUnrecoSample(byte[] buffer, int offset, int bytesTransferred)
        {
            return cmdprocessor.ResponseUnrecoSample(buffer, offset, bytesTransferred);
        }

        // specific parsing methods for responses
        public void SplitBroadcastResponse(string received, ref string itype, ref string iname, ref Int32 iversion)
        {
            cmdprocessor.SplitBroadcastResponse(received, ref itype, ref iname, ref iversion);
        }
        public void SplitCStatusResponse(string received, ref Instr.LMInstrStatus st)
        {
            cmdprocessor.SplitCStatusResponse(received, ref st);
        }
        public void SplitHVReadResponse(string received, ref int hv)
        {
            cmdprocessor.SplitHVReadResponse(received, ref hv);
        }
        public void SplitHVCalibResponse(string received, ref DAQ.HVControl.HVStatus hvst)
        {
            cmdprocessor.SplitHVCalibResponse(received, ref hvst);
        }
        public void SplitLLDReadResponse(string received, ref int hv)
        {
            cmdprocessor.SplitLLDReadResponse(received, ref hv);
        }
        public void SplitRatesReadResponse(string received,  ref Instr.RatesStatus p)
        {
            cmdprocessor.SplitRatesReadResponse(received, ref p);
        }
        public void SplitPowerReadResponse(string received, ref Instr.PowerStatus p)
        {
            cmdprocessor.SplitPowerReadResponse(received, ref p);
        }

        // extract the single integer arg if there, mark bad if expected and not there
        bool ParsePrompt(string line, ALMMLingo.OpDesc cmdt, ref Int32 res)
        {
            res = 0;
            bool good = !cmdt.needsArg;
            string[] temp = line.Split('=');
            if (cmdt.needsArg && temp.Count() > 1)
            {
                good = Int32.TryParse(temp[1], out res);
                //no assay command now
                //if (!good && cmdt.tok == ALMMLingo.Tokens.assay)
               // {
                //    good = AssaySelector.AssayTypeConv(temp[1], out res);
               // }
            }        
            return good;
        }
         

        //
        // "Send" wrapper hiding socket and the looping over list of LMMMs
        //
        private void SendToALMM(string cmd, Int32 specificLMIndex = -1)
        {
            if (!Instruments.Active.HasALMM())
            {
                commlog.TraceEvent(LogLevels.Warning, 325, "No ALMM instruments available to receive '" + cmd + "'");
                return;
            }
            try
            {
                if (CurrentLM == -1 && specificLMIndex == -1)  // all of them
                {
                    commlog.TraceInformation("Send " + LMLoggers.LognLM.FlattenChars(cmd) + ALMMLingo.eolprintrep + " to all the LMMM instruments on the subnet");
                    IEnumerator iter = Instruments.Active.GetLMEnumerator();
                    while (iter.MoveNext())
                    {
                        LMInstrument lmi = (LMInstrument)iter.Current;
                        commlog.TraceEvent(LogLevels.Verbose, 360, "Send '" + LMLoggers.LognLM.FlattenChars(cmd) + ALMMLingo.eolprintrep + "' to " + Instruments.Active.RankPositionInList(lmi) + " " + lmi.port + ", ");
                        //LMServer.SendData(cmd + ALMMLingo.eol, lmi.instrSocketEvent);
                    }
                }
                else
                {
                    int index = specificLMIndex > -1 ? specificLMIndex : CurrentLM;  // index override from live call in main code, not from command line
                    ALMMController.ReturnCode rc;
                    // make sure the element is actually there
                    commlog.TraceInformation("Send '" + LMLoggers.LognLM.FlattenChars(cmd) + ALMMLingo.eolprintrep + "' to LMMM instrument {0} on the subnet", index);
                    LMInstrument lmi = Instruments.Active.FindByIndexer(index);
                    if (lmi == null) // index must always be less than Count, the list is 0 based
                        commlog.TraceEvent(LogLevels.Warning, 325, "No LMMM instrument {0} available", index);
                    else
                        //LMServer.SendData(cmd + ALMMLingo.eol, lmi.instrSocketEvent); 
                    
                        rc = almmctrl.Send(cmd);
                }
            }
            catch (ObjectDisposedException ex)
            {
                commlog.TraceEvent(LogLevels.Error, 354, "LOST an instrument: " + ex.Message);
            }
            catch (SocketException se)
            {
                commlog.TraceEvent(LogLevels.Error, 355, "TCP/IP send socket error: " + se.Message);
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 356, "TCP/IP send failed: " + e.Message);
            }
        }

        // UDP broadcast
        public bool PostALMMCommand(ALMMLingo.Tokens cmd, bool terminator = false)
        {
            bool res = true;

            if (NC.App.AppContext.UseINCC5Ini)
            { // look up OpDesc instance from map
                // dispatch to thrift based on cmd token
                ALMMLingo.OpDesc op = null;
                cmdprocessor.LookupOpDescriptor(cmd, ref op);
            }
            else
            try {
                string cmds = cmdprocessor.ComposeCommandStrings(cmd, 0);
                if (cmds.Length > 0)
                {
                    LMConnectionInfo net = ((LMConnectionInfo)(NC.App.Opstate.Measurement.Detector.Id.FullConnInfo));
                    // broadcast go message to all cfg.Net.Subnet addresses. This is the instrument group.
                    Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    if (terminator) cmds += ALMMLingo.eol;
                    Byte[] sendBuffer = Encoding.ASCII.GetBytes(cmds);

                    IPEndPoint ep = new IPEndPoint(net.NetComm.ipaddress, net.NetComm.Port);
                    s.SendTo(sendBuffer, ep);
                    commlog.TraceEvent(LogLevels.Verbose, 361, "UDP send: '" + LMLoggers.LognLM.FlattenChars(cmds) + "'");

                }
            }
            catch (ObjectDisposedException ex)
            {
                commlog.TraceEvent(LogLevels.Error, 357, "LOST an instrument: " + ex.Message);
            }
            catch (SocketException se)
            {
                commlog.TraceEvent(LogLevels.Error, 358, "UDP send socket error: " + se.Message);
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 359, "UDP send cough: " + e.Message);
            }
            return res;
        }

        // TCP/IP synchronous send
        public bool FormatAndSendALMMCommand(ALMMLingo.Tokens cmd, Int32 arg = 0, Int32 LMRankPosition = -1)
        {
            bool res = true;

            if (NC.App.AppContext.UseINCC5Ini)
            {   // look up OpDesc instance from map
                // dispatch to thrift based on cmd token
                ALMMLingo.OpDesc op = null;
                cmdprocessor.LookupOpDescriptor(cmd, ref op);
            }
            else
            {
                try
                {
                    string cmds = cmdprocessor.ComposeCommandStrings(cmd, arg);
                    if (cmds.Length > 0)
                    {
                        SendToALMM(cmds, LMRankPosition);
                    }
                }
                catch (Exception e)
                {
                    commlog.TraceEvent(LogLevels.Error, 354, "TCP send parse issue: " + e.Message);
                }
            }
            return res;
        }
        
        internal Thread ProcessUserCommand(ALMMLingo.OpDesc cmdt, string tline, DAQ.DAQControl control, ref bool keepgoing)
        {
            Thread t = null;
            // each prompt command might have a '=' followed by a single numeric argument e.g. cmd = 1
            Int32 arg = 0;
            bool hasarg = ParsePrompt(tline, cmdt, ref arg);

            // if we didnt get an arg but one is required for the operation (5 or 6 commands, see the list) 
            // then go get the config value from the config state and use it for arg

            if (cmdt.needsArg && !hasarg) // use the lambda to get the config value
                arg = cmdt.cfgInt32.Value;

            keepgoing = true;
            switch (cmdt.tok)
            {
                case ALMMLingo.Tokens.quit: // quit console prompt
                    Console.WriteLine(NC.App.AbbrName + "> Be seeing you . . .");
                    keepgoing = false;
                    break;
                case ALMMLingo.Tokens.lm:  // set or show cur LM #
                    try
                    {
                        CurrentLM = arg;
                        Console.WriteLine(NC.App.AbbrName + ">LM= " + arg);
                    }
                    catch (Exception e)
                    {
                        commlog.TraceEvent(LogLevels.Error, 361, "Current instrument and measurement undefined or incomplete: " + e.Message);
                    }
                    break;
                case ALMMLingo.Tokens.help:
                    //Console.Write(NC.App.AbbrName + ">");
                    foreach (string s in cmdprocessor.CmdPromptHelp) Console.WriteLine(s);
                    break;
                case ALMMLingo.Tokens.config:  //
                    //Console.Write(NC.App.AbbrName + ">");
                    NCCConfig.Config.ShowCfg(NC.App.Config, NC.App.AppContext.Verbose() == System.Diagnostics.TraceEventType.Verbose);
                    break;
                case ALMMLingo.Tokens.stop:  // mostly for stopping an assay, might work for HV Calib too but must test it
                    Console.Write(NC.App.AbbrName + ">");
                    control.StopCurrentAction(); // NEXT: see if this works for HV Calib 
                    break;
                case ALMMLingo.Tokens.assay:
                    if (hasarg)
                    {
                        NC.App.Opstate.Measurement.MeasurementId.MeasOption = (AssaySelector.MeasurementOption)arg;
                        Console.WriteLine(NC.App.AbbrName + ">assay type= " + arg + " (" + ((AssaySelector.MeasurementOption)arg).ToString() + ")");
                    }
                    else
                    {
                        t = control.AssayOperation();
                    }
                    break;
                case ALMMLingo.Tokens.hvcalib:
                    t = control.HVCalibOperation();
                    break;
                case ALMMLingo.Tokens.broadcast:  // UDP "NDAC Control"
                    {
                        NCC.NCCAction x = NC.App.Opstate.Action;
                        NC.App.Opstate.Action = NCC.NCCAction.Discover;
                        control.StartLMDAQServer(null);
                        Console.WriteLine(NC.App.AbbrName + "> Broadcasting to LM instruments. . .");
                        PostALMMCommand(cmdt.tok);
                        Console.WriteLine(NC.App.AbbrName + "> Sent broadcast. Waiting for LM instruments to connect"); 
                        control.PrepSRDAQHandler();
                        control.ConnectSRInstruments();
                        NC.App.Opstate.Action = x;
                    }
                    break;
                default:
                    try
                    {
                        FormatAndSendALMMCommand(cmdt.tok, arg, CurrentLM);
                    }
                    catch (Exception e)
                    {
                        commlog.TraceEvent(LogLevels.Error, 360, "Current instrument and measurement undefined or incomplete: " + e.Message);
                    }
                    break;
            }
            return t;
        }

        // emulation entry

        public void Settle()  // this could go into the dispose or finalize#
        {

        }
        


        int CurrentLM
        {
            get { return NC.App.Opstate.Measurement.AcquireState.lm.LM; } set { NC.App.Opstate.Measurement.AcquireState.lm.LM = value; } }
        }*/
    }
}
