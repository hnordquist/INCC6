/*
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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DetectorDefs;
using NCCReporter;

namespace LMComm
{
    using NC = NCC.CentralizedState;


    // These two related classes (TalkToLMMMM and LMMMLingo) contain the command and results string patterns for the LMMM 
    // They support cmd string to operation mapping with smarts for full token expansion and replacement
    // They also wrap the physical send of final constructed command strings 
    // They also do the string to token matching and deconstruction of return strings for use in the Receive/Accept handler

    // 1: for constructing well-formed command strings to send to LMMM
    // 2: for deconstructing response strings sent from the LMMM
    // 3: for parsing command prompt entries into (1)
    public class LMMMLingo
    {
        public enum Tokens
        {
            unknown, broadcast,
            prep, arm, go, bgo, assay, cancel,   // connect and assay commands
            quit, lm, help, config, stop,  // interpreted by command prompt
            exit, leds, debug, cstatus, input, rates, power, lld, // LMMMM/NPOD commands
            hvread, hvset, hvcalib, hvprep, // hv commands
            tosenddatasize, unrecognizeddata, statusdata, odddata, assaycancelled, // response strings while in active data mode now include "rates"
            shutdown, // shutdown the test server 
            lmmax
        };
        const int pktlen = 12;  // 8 0'd bytes with 4 bytes for data size
        internal const string eol = "\r\n"; // this eol is specific to N Div LMMM devices, they use the Windows EOL convention, and I hard-code it here because it is part of the lingo!
        internal const string eolprintrep = "\\r\\n";

        private System.Collections.Hashtable LMMMOpStringMap; // for real use, e.g. direct invocation on the socket
        private System.Collections.Hashtable CmdStringOpDescMap;  // for flags and prompt parsing
        private System.Collections.Hashtable LMMMOpOpDescMap;  // for fake use, e.g. thrift method calls to MLMEm
        private List<string> NonDataResponses;  // always a few stragglers in the model

        LMLoggers.LognLM commlog = null;
        public LMLoggers.LognLM CommLog
        {
            get { return commlog; }
            set { commlog = value; }
        }

        // love this C# anonymous function feature!
        public class ConfigInt32Value
        {
            private Int32 val;
            private Func<Int32> getValue;

            // Constructor.
            public ConfigInt32Value(Func<Int32> func)
            {
                val = 0;
                getValue = func;
            }

            public Int32 Value
            {
                get
                {
                    // Execute the delegate.
                    val = getValue();
                    return val;
                }
            }
        }

        // struct holding properties of permitted command prompt tokens and the larger set of LMMM ops 
        // all prompt cmds take 0 or 1 int arg, so it's easy
        public class OpDesc
        {
            public OpDesc(bool empty)
            {
                tok = Tokens.unknown;
                needsArg = isAPromptCmd = false; cfgInt32 = null;
            }
            public OpDesc(Tokens t, bool isCmd)
            {
                tok = t; isAPromptCmd = isCmd; needsArg = false; cfgInt32 = null;
            }
            public OpDesc(Tokens t, bool isCmd, ConfigInt32Value f)
            {
                tok = t; isAPromptCmd = isCmd; needsArg = true; cfgInt32 = f;
            }
            public Tokens tok;
            public bool isAPromptCmd;
            public bool needsArg;
            //         public bool UDP = false;
            public ConfigInt32Value cfgInt32;

            public override string ToString()
            {
                return tok + (needsArg ? "=#" : "") + (isAPromptCmd ? " (a prompt)" : "");
            }
        };


        // class to encode and support all commands to and from an LMMM and a client
        public LMMMLingo()
        {
            LMConnectionInfo lmci = NC.App.LMBD.LastBestConnInfo();

            CmdStringOpDescMap = new System.Collections.Hashtable();
            CmdStringOpDescMap.Add(Tokens.broadcast.ToString(), new OpDesc(Tokens.broadcast, true));  //+ config with duration and separation and [master|slave]
            CmdStringOpDescMap.Add(Tokens.prep.ToString(), new OpDesc(Tokens.prep, false));  // config with duration and separation and [master|slave]
            CmdStringOpDescMap.Add("a", new OpDesc(Tokens.arm, false));     // arm if asynch broadcast go
            CmdStringOpDescMap.Add(Tokens.arm.ToString(), new OpDesc(Tokens.arm, false));
            CmdStringOpDescMap.Add(Tokens.go.ToString(), new OpDesc(Tokens.go, false));     // TCP/IP tell LMMM to go go go 
            CmdStringOpDescMap.Add("start", new OpDesc(Tokens.assay, true));  // *** prep, [arm], go
            CmdStringOpDescMap.Add(Tokens.assay.ToString(), new OpDesc(Tokens.assay, true,
                            new ConfigInt32Value(() => { int v = (int)NC.App.Opstate.Measurement.MeasurementId.MeasOption; return v; })));    //+ start assay
            CmdStringOpDescMap.Add(Tokens.stop.ToString(), new OpDesc(Tokens.stop, true));    //+ stop full assay
            CmdStringOpDescMap.Add(Tokens.cancel.ToString(), new OpDesc(Tokens.cancel, true));  //+ stop current cycle 

            CmdStringOpDescMap.Add(Tokens.lm.ToString(), new OpDesc(Tokens.lm, true,
                            new ConfigInt32Value(() => { int r = NC.App.Opstate.Measurement.AcquireState.lm.LM; return r; })));    //+ get or set context current LM #
            CmdStringOpDescMap.Add(Tokens.quit.ToString(), new OpDesc(Tokens.quit, true));  //+ quit console prompt
            CmdStringOpDescMap.Add(Tokens.help.ToString(), new OpDesc(Tokens.help, true));  //+ show console prompt help
            CmdStringOpDescMap.Add(Tokens.config.ToString(), new OpDesc(Tokens.config, true));  //+ show current loaded config settings

            CmdStringOpDescMap.Add(Tokens.exit.ToString(), new OpDesc(Tokens.exit, true));  //+ tell LMMM to exit linux apps, WTM
            CmdStringOpDescMap.Add(Tokens.leds.ToString(), new OpDesc(Tokens.leds, true,
                            new ConfigInt32Value(() => { int v = lmci.DeviceConfig.LEDs; return v; })));  //+ set and get leds value
            CmdStringOpDescMap.Add(Tokens.debug.ToString(), new OpDesc(Tokens.debug, true,
                            new ConfigInt32Value(() => { int v = lmci.DeviceConfig.Debug; return v; })));//+ set debug value on LMMM 
            CmdStringOpDescMap.Add(Tokens.cstatus.ToString(), new OpDesc(Tokens.cstatus, true)); //+ request cstatus, this is a response prefix too
            CmdStringOpDescMap.Add(Tokens.input.ToString(), new OpDesc(Tokens.input, true,
                            new ConfigInt32Value(() => { int v = lmci.DeviceConfig.Input; return v; }))); //+ set input on LMMM
            CmdStringOpDescMap.Add(Tokens.lld.ToString(), new OpDesc(Tokens.lld, true,
                            new ConfigInt32Value(() => { int v = lmci.DeviceConfig.LLD; return v; }))); //+ read/set LLD on NPOD
            CmdStringOpDescMap.Add(Tokens.rates.ToString(), new OpDesc(Tokens.rates, true)); //+ read rates on LMMM/NPOD
            CmdStringOpDescMap.Add(Tokens.power.ToString(), new OpDesc(Tokens.power, true)); //+ read power on NPOD
            CmdStringOpDescMap.Add(Tokens.hvset.ToString(), new OpDesc(Tokens.hvset, true,
                            new ConfigInt32Value(() => { int v = lmci.DeviceConfig.HV; return v; })));//+ set high voltage 
            CmdStringOpDescMap.Add("hv", new OpDesc(Tokens.hvread, false));    // read hv,  // this is a response prefix too
            CmdStringOpDescMap.Add(Tokens.hvread.ToString(), new OpDesc(Tokens.hvread, true));    //+ read hv 
            CmdStringOpDescMap.Add(Tokens.hvprep.ToString(), new OpDesc(Tokens.hvprep, false));  // HV calib config with voltage and duration
            CmdStringOpDescMap.Add(Tokens.hvcalib.ToString(), new OpDesc(Tokens.hvcalib, true));  //do HV Calib op, response prefix too
            CmdStringOpDescMap.Add(Tokens.shutdown.ToString(), new OpDesc(Tokens.shutdown, true));  // todo: shutdown the test server

            // responses associated with the direct data collection
            CmdStringOpDescMap.Add("to sen", new OpDesc(Tokens.tosenddatasize, false));
            CmdStringOpDescMap.Add("unreco", new OpDesc(Tokens.unrecognizeddata, false));
            CmdStringOpDescMap.Add("status", new OpDesc(Tokens.statusdata, false));
            CmdStringOpDescMap.Add(".....", new OpDesc(Tokens.statusdata, false)); // 6th char is ' '  or '.'
            CmdStringOpDescMap.Add("lled. ", new OpDesc(Tokens.assaycancelled, false)); // 6th char is ' ', 'Assay Cancelled. '
            //CmdStringtoTokenMap.Add("*", Tokens.odddata);

            // for moving from token to op desc to test driver
            LMMMOpOpDescMap = new System.Collections.Hashtable();
            LMMMOpOpDescMap.Add(Tokens.broadcast, CmdStringOpDescMap[Tokens.broadcast.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.prep, CmdStringOpDescMap[Tokens.prep.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.arm, CmdStringOpDescMap[Tokens.arm.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.go, CmdStringOpDescMap[Tokens.go.ToString()]);
            //       LMMMOpOpStructMap.Add(Tokens.bgo, CmdStringOpStructMap["bgo"]);
            LMMMOpOpDescMap.Add(Tokens.cstatus, CmdStringOpDescMap[Tokens.cstatus.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.exit, CmdStringOpDescMap[Tokens.exit.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.debug, CmdStringOpDescMap[Tokens.debug.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.leds, CmdStringOpDescMap[Tokens.leds.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.input, CmdStringOpDescMap[Tokens.input.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.rates, CmdStringOpDescMap[Tokens.rates.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.lld, CmdStringOpDescMap[Tokens.lld.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.power, CmdStringOpDescMap[Tokens.power.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.shutdown, CmdStringOpDescMap[Tokens.shutdown.ToString()]); // todo: implement this op
            LMMMOpOpDescMap.Add(Tokens.cancel, CmdStringOpDescMap[Tokens.cancel.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.stop, CmdStringOpDescMap[Tokens.stop.ToString()]);
            // hv calib ops
            LMMMOpOpDescMap.Add(Tokens.hvread, CmdStringOpDescMap[Tokens.hvread.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.hvprep, CmdStringOpDescMap[Tokens.hvprep.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.hvcalib, CmdStringOpDescMap[Tokens.hvcalib.ToString()]);
            LMMMOpOpDescMap.Add(Tokens.hvset, CmdStringOpDescMap[Tokens.hvset.ToString()]);

            // base strings used to and from the LMMM
            LMMMOpStringMap = new System.Collections.Hashtable();
            LMMMOpStringMap.Add(Tokens.broadcast, "NDAC Control");
            LMMMOpStringMap.Add(Tokens.prep, "Duration = {0}" + eol + "Separation = {1}");
            LMMMOpStringMap.Add(Tokens.arm, "Arm");
            LMMMOpStringMap.Add(Tokens.go, "Go");
            LMMMOpStringMap.Add(Tokens.assay, "***");  // chain of actions: loop of these strings, ["master\r\n"|"slave\r\n"][Feedback = 1\r\n"]"Duration = {0}\r\n";Separation = {1}\r\n"; followed by optional Arm, then Go
            LMMMOpStringMap.Add(Tokens.cancel, "Cancel");
            LMMMOpStringMap.Add(Tokens.quit, "**");   // shell command
            LMMMOpStringMap.Add(Tokens.exit, "Exit");
            LMMMOpStringMap.Add(Tokens.lm, "**");  // shell command
            LMMMOpStringMap.Add(Tokens.leds, "Leds");
            LMMMOpStringMap.Add(Tokens.lld, "LLD");
            LMMMOpStringMap.Add(Tokens.cstatus, "cStatus");  // response prefix as well
            LMMMOpStringMap.Add(Tokens.input, "input");
            LMMMOpStringMap.Add(Tokens.debug, "Debug");
            LMMMOpStringMap.Add(Tokens.rates, "Rates");
            LMMMOpStringMap.Add(Tokens.power, "Power");
            LMMMOpStringMap.Add(Tokens.hvset, "HVset"); // HV single command set op, but see next line for the plateau prep command
            LMMMOpStringMap.Add(Tokens.hvprep, "HVset = {0}" + eol + "Duration = {1}"); // prep line
            LMMMOpStringMap.Add(Tokens.hvread, "HVread");
            LMMMOpStringMap.Add(Tokens.hvcalib, "HVcalib");  // / request calibration status

            // yes this is hack for the six special response strings that need to be
            // matched during non-data processing response string parsing.
            NonDataResponses = new List<string>();
            NonDataResponses.Add(((string)LMMMOpStringMap[Tokens.cstatus]).ToLower());
            NonDataResponses.Add(((string)LMMMOpStringMap[Tokens.hvread]).ToLower());
            NonDataResponses.Add(((string)LMMMOpStringMap[Tokens.hvcalib]).ToLower());
            NonDataResponses.Add(((string)LMMMOpStringMap[Tokens.rates]).ToLower());
            NonDataResponses.Add(((string)LMMMOpStringMap[Tokens.power]).ToLower());
            NonDataResponses.Add(((string)LMMMOpStringMap[Tokens.lld]).ToLower());

        }

        private string NCCConfigurePrep(string temp)
        {
            string res = String.Format(temp, NC.App.Opstate.Measurement.AcquireState.lm.Interval, NC.App.Opstate.Measurement.AcquireState.lm.Separation);
            // no non no! res += eol;
            if (NC.App.Opstate.Measurement.AcquireState.lm.Feedback)
            {
                res = "Feedback = 1" + eol + res;
            }
            return res;
        }
        private string LMHVConfigurePrep(string temp, int curpt)
        {
            string res = String.Format(temp, curpt, NC.App.Opstate.Measurement.AcquireState.lm.HVDuration);
            return res;
        }

        //  action to LMMM code string, with set/get flag and values for each command, 
        public string ComposeCommandStrings(Tokens a, Int32 arg)
        {
            string res = "";
            if (LMMMOpStringMap.ContainsKey(a))
                res = (string)LMMMOpStringMap[a];
            // got the base, now decorate it

            switch (a)
            {
                case Tokens.broadcast:
                    break;
                case Tokens.prep:
                    res = NCCConfigurePrep(res); // ["master\r\n"|"slave\r\n"][Feedback = 1\r\n"]"Duration = {0}\r\n";Separation = {1}\r\n";
                    break;
                case Tokens.arm:
                    break;
                case Tokens.go:
                    break;
                case Tokens.bgo:
                    break;
                case Tokens.assay: // ["master\r\n"|"slave\r\n"][Feedback = 1\r\n"]"Duration = {0}\r\n";Separation = {1}\r\n";
                    res = (string)LMMMOpStringMap[Tokens.prep];
                    res = NCCConfigurePrep(res);
                    break;
                case Tokens.cancel:
                    break;
                case Tokens.stop:
                    break;
                case Tokens.lm:
                    res = res[0] + " = " + arg;
                    break;
                case Tokens.exit:
                    break;
                case Tokens.leds:
                    res = res + " = " + arg;
                    break;
                case Tokens.debug:
                    res = res + " = " + arg;
                    break;
                case Tokens.cstatus:
                    res = res + " =";  // not using arg, but not using op desc arg flag to tell this fact either
                    break;
                case Tokens.input:
                    res = res + " = " + arg;
                    break;
                case Tokens.hvread:
                    res = res + " = ";
                    break;
                case Tokens.hvset:
                    res = res + " = " + arg;
                    break;
                case Tokens.hvprep:
                    res = LMHVConfigurePrep(res, arg); // ["HVSet = {0}\r\nDuration = {1}\r\n]";
                    break;
                case Tokens.hvcalib:
                    break;
                case Tokens.rates:
                    break;
                case Tokens.lld:
                    ; //res = res + " = ";// + arg;
                    break;
                case Tokens.power:
                    break;
                case Tokens.shutdown:
                    break;
            }
            return res;
        }

        internal string[] CmdPromptHelp = new string[]{
        "  broadcast    send UDP NDAC Control on subnet",
        "  assay|start  do an assay process",
        "  hvcalib      do the hv calibration process",
        "  cancel       stop the active cycle",
        "  stop         stop the active cycle and the overall assay process",

        "  lm=#         set the current LM #, defaults to -1 (''all'')",
        
        "  config       show current configuration",
        "  quit|^Z      quit this interactive prompt session",
        "  help         show this text",

        "  exit         set the flag to exit linux apps",
        "  leds, leds=0, leds=2  set the leds value 0 (cycle) or 2 (activity)",
        "                              leds alone uses config value",
        "  debug, debug=0, debug=1  set the debug value 0 or 1",
        "                                  debug alone uses config value",
        "  input, input=0, input=1  set the input value to 0 (TTL) or 1 (Diff/Ribbon)",
        "                                  input alone uses config value",
        "  cstatus=     request the 8 element status",
        
        "  hvread       request the hv value",
        "  hvset=#      set hv to an integer value",
        "                                  hvset alone uses config value",
        "  rates=       request the rates",        
        "  lld=[mV]     set or request the LLD mV",
        "  power=       request power status", 
        "  shutdown     shut down the test emulator process",
                          };


        internal void LookupOpDescriptor(LMMMLingo.Tokens cmd, ref LMMMLingo.OpDesc op)
        {
            try
            {
                op = (LMMMLingo.OpDesc)LMMMOpOpDescMap[cmd];
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 888, "Unable to look up cmd in descriptor map:" + e.Message);
            }
        }


        internal void CommandPromptMatchPrefix(string input, ref LMMMLingo.OpDesc op)
        {
            string s = input.ToLower();
            foreach (DictionaryEntry possible in CmdStringOpDescMap)  // this supposed to be an IDictionaryElement or something like that. we get a k,v pair here
            {
                if (s.StartsWith((string)possible.Key) &&
                    ((OpDesc)possible.Value).isAPromptCmd) // it is a prompt prefix
                {
                    op = ((OpDesc)possible.Value);
                    break;
                }
            }
        }

        // an arbitrary byte string is contained in this response because this is called on data received while in the ReceivingData DAQ state

        // once upon a time this test worked, but last firmware update removed the trailing "..... " or "......" and we can only look for ASCII "Status" now
        public Tokens DataPacketResponseMatch(byte[] buffer, int offset, int bytesTransferred)
        {
            string commonstr = System.Text.ASCIIEncoding.ASCII.GetString(buffer, offset + pktlen, 6);  // six chars long
            Tokens res = Tokens.unknown;
            string s = commonstr.ToLower();
            string str_end = String.Empty;
            if (CmdStringOpDescMap.ContainsKey(s))
            {
                res = ((OpDesc)CmdStringOpDescMap[s]).tok;
            }
            else
            {
                str_end = System.Text.ASCIIEncoding.ASCII.GetString(buffer,
                                                offset + bytesTransferred - 6, 5); // last char is a space or .
                if (CmdStringOpDescMap.ContainsKey(str_end))
                    res = ((OpDesc)CmdStringOpDescMap[str_end]).tok;

            }
            // no luck, so now do the full scan, 
            // dev note: so far I am only using this for "Status" on an ODD message during active DAQ, but should generalize this to the other four too.
            if (res == Tokens.unknown)
            {
                byte[] b = { 0x53, 0x74, 0x61, 0x74, 0x75, 0x73, 0x0D, 0x0A };
                // "Status\r\n" ASCII

                if (commlog.ShouldTrace(LogLevels.Info))
                {
                    string temp = string.Format("Unaligned data =, 1st looked at '{0}', then looked at '{1}'", LMLoggers.LognLM.FlattenChars(s), LMLoggers.LognLM.FlattenChars(str_end));
                    commlog.TraceEvent(LogLevels.Verbose, 771751, temp);
                }
                for (int i = 0; i < (bytesTransferred - b.Length); i++)
                {
                    try
                    {
                        if (buffer[i + 0 + offset] == b[0]) // gimme an 'S'
                            if (buffer[i + 1 + offset] == b[1]) // gimme an 't'
                                if (buffer[i + 2 + offset] == b[2]) // gimme an 'a'
                                    if (buffer[i + 3 + offset] == b[3]) // gimme an 't'
                                        if (buffer[i + 4 + offset] == b[4]) // gimme an 'u'
                                            if (buffer[i + 5 + offset] == b[5]) // gimme an 's'
                                                if (buffer[i + 6 + offset] == b[6]) // gimme an '\r'
                                                    if (buffer[i + 7 + offset] == b[7]) // gimme an '\n'
                                                    {
                                                        res = Tokens.statusdata;
                                                        if (commlog.ShouldTrace(LogLevels.Info))
                                                        {
                                                            string temp = string.Format("Found status marker at {0} into buffer", i + 0 + offset);
                                                            commlog.TraceEvent(LogLevels.Verbose, 771757, temp);
                                                        }
                                                        break;
                                                    }
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            return res;
        }

        public String NonDataResponse(byte[] buffer, int offset, int bytesTransferred)
        {
            return Encoding.ASCII.GetString(buffer, offset + pktlen, bytesTransferred - pktlen);
        }

        // string response expected, called on data received while in Online or HVClaib DAQ state
        public Tokens ResponseMatchPrefix(string received)
        {
            Tokens res = Tokens.unknown;
            string s = received.ToLower();
            string[] t = s.Split('=');
            if (t == null || t.Length < 1)
                return res;
            string u = t[0].Trim();
            foreach (string possible in NonDataResponses)
            {
                if (u.Equals(possible))
                { // need to look at the first n chars for a match of a certain subset of known response strings
                    res = ((OpDesc)CmdStringOpDescMap[possible]).tok;
                    break;
                }
            }
            return res;
        }

        public UInt32 ResponseToSendDataSize(byte[] buffer, int offset, int bytesTransferred)
        {
            try
            {
                string str2 = System.Text.ASCIIEncoding.ASCII.GetString(buffer, offset + 22, bytesTransferred - 22);
                return Convert.ToUInt32(str2);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string ResponseUnrecoSample(byte[] buffer, int offset, int bytesTransferred)
        {
            try
            {
                string str2 = System.Text.ASCIIEncoding.ASCII.GetString(buffer,
                      offset + pktlen, Math.Min(100, bytesTransferred - pktlen));
                return str2;
            }
            catch (Exception)
            {
                return "*";
            }
        }

        public void SplitBroadcastResponse(string received, ref string itype, ref string iname, ref Int32 iversion)
        {
            try
            {  // e.g. REI,N-1 LMMM,20100202,110aa32 
                string[] str = received.Split(',');
                itype = str[0];
                iname = str[1];
                iversion = Convert.ToInt32(str[2]);
                // str[3] is something else but I dunno what.
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 889, "SplitBroadcastResponse gagged on:" + received + "; " + e.Message);
            }
        }

        // 
        // cStatus = 0,0,2,500,506,2500,200,0 as of Dec 2010 rev HW
        public void SplitCStatusResponse(string received, ref Instr.LMInstrStatus st)
        {
            try
            {
                string[] text = received.Split('=');
                string[] sval = text[1].Split(',');

                // debug/verbose on the box, not the app
                bool state;
                if (Convert.ToInt32(sval[0]) == 1) state = true; else state = false;
                st.debug = state;

                // input path
                st.inputPath = Convert.ToInt32(sval[1]);  // 0 or 1

                // led output state
                st.leds = Convert.ToInt32(sval[2]);  // 0 or 2

                // hv setpoint
                st.setpoint = Convert.ToInt32(sval[3]);

                // hv
                st.hv = Convert.ToInt32(sval[4]);

                // LLD max
                st.MaxLLD = Convert.ToInt32(sval[5]);
                // LLD mv
                st.LLD = Convert.ToInt32(sval[6]);
                // u3 ????
                st.u1 = Convert.ToInt32(sval[7]);

            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 890, "SplitCStatusResponse barfed on:" + received + "; " + e.Message);
            }

        }
        public void SplitHVReadResponse(string received, ref int hv)
        {
            try
            {
                string[] hv_text = received.Split('=');
                int val = Convert.ToInt32(hv_text[1]); // RR: filter end junk
                hv = val;
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 891, "SplitHVReadResponse barfed on:" + received + "; " + e.Message);
            }
        }

        public void SplitHVCalibResponse(string received, ref DAQ.HVControl.HVStatus hvst)
        {
            try
            {
                hvst.Extract(received);
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 892, "SplitHVCalibResponse barfed on:" + received + "; " + e.Message);
            }
        }
        public void SplitPowerReadResponse(string received, ref Instr.PowerStatus p)
        {
            try
            {
                string[] aaa = received.Split('=');
                string[] txt = aaa[1].Split(',');
                int r;
                bool b = Int32.TryParse(txt[0], out r);
                if (b) p.ACPresent = r;
                b = Int32.TryParse(txt[1], out r);
                if (b) p.batteryPresent = r;
                b = Int32.TryParse(txt[2], out r);
                if (b) p.batterylevelPct = r;
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 892, "SplitPowerReadResponse barfed on:" + received + "; " + e.Message);
            }
        }
        public void SplitRatesReadResponse(string received, ref Instr.RatesStatus p)
        {
            try
            {
                string[] ff = received.Split('=');
                string[] txt = ff[1].Split(',');
                for (int i = 1; i <= NC.ChannelCount && i < txt.Length; i++)
                {
                    int r;
                    bool b = Int32.TryParse(txt[i], out r);
                    if (b)
                        p.channels[i] = r;
                }
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 893, "SplitRatesReadResponse barfed on:" + received + "; " + e.Message);
            }
        }
        public void SplitLLDReadResponse(string received, ref int lld)
        { //LLD = 2500 
            try
            {
                string[] lld_text = received.Split('=');
                int val = Convert.ToInt32(lld_text[1]);
                lld = val;
            }
            catch (Exception e)
            {
                commlog.TraceEvent(LogLevels.Error, 894, "SplitLLDReadResponse barfed on:" + received + "; " + e.Message);
            }
        }
        public void ParseStatusText()
        {
        }
    }
   
}
