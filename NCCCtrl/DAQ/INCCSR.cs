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
using System.Text;
using System.Threading;
using AnalysisDefs;
using DetectorDefs;
using LMSR;
using NCCReporter;
using NCCTransfer;
namespace DAQ
{

    using SR = INCCSR;

    /// <summary>
    /// the port of take_data.cpp will manage the transition from unsafe run_rec to Cycle. 
    /// </summary>
    public class INCCSR
    {

        public INCCSR(ShiftRegisterParameters sr, DataSourceIdentifier id, AcquireParameters acq, TestParameters test, LMLoggers.LognLM log)
        {
            sr_parms = new ShiftRegisterParameters(sr);
            dsid = new DataSourceIdentifier(id);
            dsid.SerialPort -= 1; // serial ports are 0 based at the HW layer 
            acquire_parms = new AcquireParameters(acq);
            test_parms = new TestParameters(test);
            this.log = log;
        }

        CancellationTokenSource lcts;

        public CancellationTokenSource LinkedCancelSource
        {
            set { lcts = value; }
        }
        LMLoggers.LognLM log;
        ShiftRegisterParameters sr_parms;
        DataSourceIdentifier dsid;

        public DataSourceIdentifier Id
        {
            get { return dsid; }
        }

        public ShiftRegisterParameters SRParams
        {
            get { return sr_parms; }
        }

        public AcquireParameters AcqParams
        {
            get { return acquire_parms; }
        }

        AcquireParameters acquire_parms;
        TestParameters test_parms;

 
        public static string SRAPIReturnStatusCode(int idx)
        {
            string res = String.Empty;
            if (idx < 0)
            {
                if (idx == -1) res = "FAIL";
                else if (idx == -2) res = "FAIL_ALL";
                else if (idx == -3) res = "null result due to ignoble end";
                else res = "SR API code: " + idx.ToString();
            }
            else if (idx == 0)
                res = "SUCCESS";
            else if (idx > sr_h.SR_TIMEOUT)
            {
                switch (idx)
                {
                    case 10: res = "MEAS_CONTINUE";
                        break;
                    case 11: res = "MEAS_TERMINATED";
                        break;
                    case 12: res = "MEAS_ABORTED";
                        break;
                    case 13: res = "MEAS_NEXT";
                        break;
                    case 14: res = "ZERO_COUNT_TIME";
                        break;
                    case 15: res = "SR_FAIL";
                        break;
                    case 16: res = "CHECKSUM_FAIL";
                        break;
                    case 17: res = "ACC_SNGL_FAIL";
                        break;
                    case 18: res = "ANALYSIS_FAIL";
                        break;
                    case 19: res = "RUN_TEST_FAIL";
                        break;
                    case 20: res = "FAIL_MASS_LIMIT";
                        break;
                }
            }
            else
            {
                res = sr_h.SRFunctionReturnStatusCode(idx);
            }

            return res;
        }

        public bool JSR15orUNAPMasqueradingAsAMSR { get { return (dsid.SRType == InstrType.AMSR && dsid.BaudRate != PSR_BAUD_RATE); } }
        public bool AMSRFerSher { get { return (dsid.SRType == InstrType.AMSR && dsid.BaudRate == PSR_BAUD_RATE); } }

        public const int SUCCESS = 0;
        public const int FAIL = -1;
        public const int FAIL_ALL = -2;

        public const int MEAS_CONTINUE = 10;
        public const int MEAS_TERMINATED = 11;
        public const int MEAS_ABORTED = 12;
        public const int MEAS_NEXT = 13;
        public const int ZERO_COUNT_TIME = 14;
        public const int SR_FAIL = 15;
        public const int CHECKSUM_FAIL = 16;
        public const int ACC_SNGL_FAIL = 17;
        public const int ANALYSIS_FAIL = 18;
        public const int RUN_TEST_FAIL = 19;
        public const int FAIL_MASS_LIMIT = 20;


        public const Int16 MSR4A_BAUD_RATE = 9600;	/* MSR4A baud rate */
        public const Int16 JSR11_BAUD_RATE = 300;	/* JSR11 baud rate */
        public const Int16 JSR12_BAUD_RATE = 9600;	/* JSR12 baud rate */
        public const Int16 PSR_BAUD_RATE = 9600;	/* PSR baud rate */
        public const Int16 DGSR_BAUD_RATE = 9600;	/* DGSR baud rate */
        public const Int32 HHMR_BAUD_RATE = 9600;	 // user-level override available in DB and UI for UNAP-as-AMSR, JSR15, and UNAP baud rate
        public const Int32 JSR15_BAUD_RATE = 9600;	  

        public const int SR_NOT_FINISHED = 99;		/* shift register is still counting */
        public const int SR_TRY_AGAIN = 98;			/* try again to restart shift reg. */
        public const int SR_NUM_TRYS = 2;
        /*============================================================================
        *
        * function name: sr_init()			file name: sr.cpp
        *
        * purpose: open, set parameters and initialize an
        *	    MSR4A, JSR-11, JSR-12 JSR-14, PSR, DGSR or AMSR type shift register.
        *
        * return value: SR_SUCCESS/SR_INVPORT/SR_IOERR/SR_TIMEOUT/ZERO_COUNT_TIME
        *
        * special notes: if there is a failure, the shift register port is closed.
        *
        * revision history:
        *
        *  date	author		revision
        *  ----	------		--------
        *  11/30/93	Bill Harker	created
        *
        *============================================================================*/

        double restart_time_per_run, restart_sethigh_voltage;

        unsafe public int sr_init(double time_per_run, double sethigh_voltage)
        {
            restart_time_per_run = time_per_run; restart_sethigh_voltage = sethigh_voltage;
            const double MAX_HV_DELTA = 2.0;

            LMSR.sr_h.sr_parms parms;
            double high_voltage, previous_high_voltage;
            int status;
            ushort i;

            if (time_per_run <= 0.0)
                return (SR.ZERO_COUNT_TIME);

            /* as a diagnostic, dump all shift register data to a file */
            /* sr_set_log_fname ("sr_data.tst"); */


            // todo: add use of progress event reporting via the timer here
            //        "Please wait, opening shift register.");

            log.TraceEvent(LogLevels.Info, 0x4f32F, "Please wait, opening shift register");

            /* open shift register */
            i = 0;
            InstrType insttype = dsid.SRType;
            do
            {
                i++;
                if (dsid.SRType == InstrType.JSR11)
                {
                    dsid.BaudRate = JSR11_BAUD_RATE;
                }
                else if (dsid.SRType == InstrType.JSR12)
                {
                    dsid.BaudRate = JSR12_BAUD_RATE;
                }
                else if ((dsid.SRType == InstrType.PSR) ||
                    (dsid.SRType == InstrType.AMSR) || dsid.SRType.isVirtualAMSR())
                {
                    if (dsid.SRType == InstrType.AMSR && dsid.BaudRate != PSR_BAUD_RATE) // AMSR faux override for UNAP
                        log.TraceEvent(LogLevels.Verbose, 0x4f32D, "AMSR Baud rate override at " + dsid.BaudRate.ToString()); // retain AMSR setting, probably fine
                    else if (dsid.SRType == InstrType.PSR && dsid.BaudRate != PSR_BAUD_RATE) // PSR faux override for JSR15
                        log.TraceEvent(LogLevels.Verbose, 0x4f32D, "PSR Baud rate override at " + dsid.BaudRate.ToString()); //retain PSR setting, probably bad fail for JSR15
                    else if (dsid.SRType.BigLoveMultiplicity())
                        log.TraceEvent(LogLevels.Verbose, 0x4f337, dsid.SRType.ToString() + " Baud rate is " + dsid.BaudRate.ToString()); // explicitly JSR15 or UNAP
                    else
                    {
                        dsid.BaudRate = PSR_BAUD_RATE; // the original INCC5 treatment
                        insttype = InstrType.PSR;
                    }
                }
                else if (dsid.SRType == InstrType.DGSR)
                {
                    dsid.BaudRate = DGSR_BAUD_RATE;
                }
                else if (dsid.SRType == InstrType.JSR15 || dsid.SRType == InstrType.UNAP)
                {
                    // use the user-level setting dsid.BaudRate 
                    log.TraceEvent(LogLevels.Verbose, 0x4f335, "JSR15 Baud rate is " + dsid.BaudRate.ToString()); // dsid.BaudRate
                }
                else
                {
                    dsid.BaudRate = MSR4A_BAUD_RATE;
                }
                status = SRLib.Open(dsid.SerialPort, (int)insttype, dsid.BaudRate);
                if (status != sr_h.SR_SUCCESS)
                {
                    sr_stop();
                    if (i < SR.SR_NUM_TRYS)
                        log.TraceEvent(LogLevels.Warning, 0x4f335, "Retrying the shift register init process");
               }
            } while ((status != sr_h.SR_SUCCESS) && (i < SR.SR_NUM_TRYS) && !lcts.IsCancellationRequested);

            if ((status != sr_h.SR_SUCCESS))
            {
                log.TraceEvent(LogLevels.Warning, 0x4f330, "{0}; Unable to open shift register serial port {1} for the {2} at rate {3}\r\nCheck serial port # and baud rate, traditionally 9600 (300 for the JSR-11), can be higher for JSR15/HHMR and later devices",
                                sr_h.SRFunctionReturnStatusCode(status), dsid.SerialPort + 1, dsid.SRType.ToString(), dsid.BaudRate.ToString());
                //log.TraceEvent(LogLevels.Warning, 0x4f330, "{0}; Unable to open shift register serial port.\r\nMake sure that the shift register is turned on and properly connected.\r\nMake sure that the baud rate is set to 9600 (300 for the JSR-11).\r\nYou should also use the Measurement Parameters dialog box under Setup to verify that you have selected the correct shift register type and serial port", sr_h.SRFunctionReturnStatusCode(status));
                return (status);
            }
            else
            {
                log.TraceEvent(LogLevels.Verbose, 0x4f32B, "Opened shift register serial port {0} for the {1} at rate {2}", dsid.SerialPort + 1, dsid.SRType.ToString(), dsid.BaudRate);
            }

            /* set shift register parameters */
            parms.predelay = sr_parms.predelayMS;
            parms.gate = sr_parms.gateLengthMS;
            parms.time = time_per_run;
            parms.hv = sethigh_voltage;
            if (dsid.SRType == InstrType.DGSR)
                parms.gate2 = sr_parms.gateLengthMS; // todo: DGSR gate_length2, but no-one will ever need it
            else
                parms.gate2 = 0.0;
            i = 0;
            do
            {
                status = SRLib.Control(dsid.SerialPort, sr_h.SR_SET_PARMS, ref parms);
                i++;
            } while ((status != sr_h.SR_SUCCESS) && (i < SR.SR_NUM_TRYS) && !lcts.IsCancellationRequested);
            if ((status != sr_h.SR_SUCCESS))
            {
                log.TraceEvent(LogLevels.Warning, 0x4f331, "{0}; Unable to set shift register parameters; HV {0}, predelay {1}, gate {2}; {3} second cycle", sr_h.SRFunctionReturnStatusCode(status), parms.hv, parms.predelay, parms.gate, parms.time);
                sr_stop();
                return (status);
            }
            else
            {
                log.TraceEvent(LogLevels.Verbose, 0x4f32A, "Set shift register parameters; HV {0}, predelay {1}, gate {2}; {3} second cycle", parms.hv, parms.predelay, parms.gate, parms.time);
            }
            /* if a PSR, JSR-12, DGSR or AMSR (or JSR15/UNAP)  then wait for high voltage to stabilize. */
            if ((dsid.SRType == InstrType.PSR) ||
                dsid.SRType.isDG_AMSR_Match() ||
                (dsid.SRType == InstrType.JSR12))
            {
                i = 0;
                high_voltage = 0.0;
                do
                {
                    previous_high_voltage = high_voltage;
                    Thread.Sleep(200);  // devnote: time needed for HV operations to settle, much more needed for HHMR
                    if (dsid.SRType == InstrType.PSR || dsid.SRType == InstrType.AMSR || dsid.SRType == InstrType.UNAP)
                    {
                        status = SRLib.Ioctl(dsid.SerialPort, sr_h.SR_PSR_GET_HV,
                            ref high_voltage);
                    }
                    else if (dsid.SRType == InstrType.JSR12)
                    {
                        status = SRLib.Ioctl(dsid.SerialPort, sr_h.SR_JSR12_GET_HV,
                            ref high_voltage);
                    }
                    else if (dsid.SRType == InstrType.JSR15) // HHMR
                    {
                        //log.TraceEvent(LogLevels.Warning, 0x4f334, "SR_HHMR_GET_HV aka SRIOR(SR_TYPE_HHMR, 15) times out, skipping HV stabilization step");
                        //status = sr_h.SR_SUCCESS;
                        //break;
                        if (i == 0)
                            Thread.Sleep(1000); // initially wait for HHMR to settle
                        status = SRLib.Ioctl(dsid.SerialPort, sr_h.SR_HHMR_GET_HV,
                            ref high_voltage);
                    }
                    else
                    {
                        status = SRLib.Ioctl(dsid.SerialPort, sr_h.SR_DGSR_GET_HV,
                            ref high_voltage);
                    }
                    if (i != 0)
                        Thread.Sleep(1000); // 1 sec
					if (lcts.IsCancellationRequested)  // punt here
						break;
                    i++;
                } while (((Math.Abs(high_voltage - previous_high_voltage) > MAX_HV_DELTA) &&  // while have not matched voltage
						 (status == sr_h.SR_SUCCESS) && (i < 100)) // and the connection is good and we haven't yet tried 100 times 
							|| status == sr_h.SR_TIMEOUT);                   // OR it simply timed out
                // When a timeout occurred, was just exiting.  Check for TIMEOUT condition and try again up to 100 tries hn
                if ((status != sr_h.SR_SUCCESS) || (i >= 100))
                {
                    log.TraceEvent(LogLevels.Warning, 0x4f332, "{0}; Unable to set shift register high voltage", sr_h.SRFunctionReturnStatusCode(status));
                }
            }

            return (status);

        }




        //* <f> */
        //*============================================================================
        //*
        //* function name: sr_start_counting()
        //*
        //* purpose: zero out the shift register counters and start a reading.
        //*
        //* return value: SRWrapdoth.SR_SUCCESS/SR_TRY_AGAIN/FAIL
        //*
        //* special notes: if there is a failure, the shift register port is closed.
        //*
        //* revision history:
        //*
        //*  date	author		revision
        //*  ----	------		--------
        //*  11/30/93	Bill Harker	created
        //*
        //*============================================================================*/

        public int sr_start_counting()
        {

            ushort i;
            int status;
            byte sr_status;

            log.TraceEvent(LogLevels.Info, 0x4f322, "Start counting with the shift register");

            /* first make sure the shift register is stopped. */
            i = 0;
            do
            {
                status = SRLib.Control(dsid.SerialPort, sr_h.SR_STOP, null);
                i++;
            } while ((status != sr_h.SR_SUCCESS) && (i < SR.SR_NUM_TRYS) && !lcts.IsCancellationRequested);
            if (status != sr_h.SR_SUCCESS)
            {
                status = sr_restart();
                if (status == SUCCESS)
                    return (SR.SR_TRY_AGAIN);
                else
                    return (SR.FAIL);
            }

            /* check shift register status to be sure it is stopped. */
            sr_status = 0;
            i = 0;
            do
            {
                status = SRLib.Control(dsid.SerialPort, sr_h.SR_GET_STATUS, ref sr_status);
                i++;
            } while ((status != sr_h.SR_SUCCESS) && (i < SR.SR_NUM_TRYS) && !lcts.IsCancellationRequested);
            if (status != sr_h.SR_SUCCESS)
            {
                log.TraceEvent(LogLevels.Warning, 0x44F0A, "{0} Status on shift register", sr_h.SRFunctionReturnStatusCode(status));
                status = sr_restart();
                if (status == SR.SUCCESS)
                    return (SR.SR_TRY_AGAIN);
                else
                    return (SR.FAIL);
            }
            if ((sr_status & sr_h.SR_S_STOPPED) == 0)
            {
                log.TraceEvent(LogLevels.Warning, 0x44F0B, "{0:x8} status on shift register", sr_status);
                status = sr_restart();
                if (status == SR.SUCCESS)
                    return (SR.SR_TRY_AGAIN);
                else
                    return (SR.FAIL);
            }

            i = 0;
            do
            {
                /* zero out all shift register counters */
                SRLib.Control(dsid.SerialPort, sr_h.SR_ZERO, null);

                /* start shift register counting */
                status = SRLib.Control(dsid.SerialPort, sr_h.SR_START, null);
                i++;
            } while ((status != sr_h.SR_SUCCESS) && (i < SR.SR_NUM_TRYS) && !lcts.IsCancellationRequested);
            if (status != sr_h.SR_SUCCESS)
            {
                log.TraceEvent(LogLevels.Warning, 0x44F0D, "{0} Status on shift register", sr_h.SRFunctionReturnStatusCode(status));
                status = sr_restart();
                if (status == SUCCESS)
                    return (SR.SR_TRY_AGAIN);
                else
                    return (SR.FAIL);
            }

            /* make sure shift register actually started. */
            sr_status = 0;
            i = 0;
            do
            {
                status = SRLib.Control(dsid.SerialPort, sr_h.SR_GET_STATUS, ref sr_status);
                i++;
            }
            while ((status != sr_h.SR_SUCCESS) && (i < SR.SR_NUM_TRYS) && !lcts.IsCancellationRequested);
            if ((sr_status != 0) || (status != sr_h.SR_SUCCESS))
            {
                status = sr_restart();
                if (status == SR.SUCCESS)
                    return (SR.SR_TRY_AGAIN);
                else
                    return (SR.FAIL);
            }

            return (sr_h.SR_SUCCESS);

        }


        //* <f> */
        //*============================================================================
        //*
        //* function name: sr_get_data()
        //*
        //* purpose: check for shift register stopped, and if so read the data and
        //*	    put it in the database run record.
        //*
        //* return value: SRWrapdoth.SR_SUCCESS/SR_NOT_FINISHED/ZERO_COUNT_TIME/SR_TRY_AGAIN/FAIL
        //*
        //* special notes: if there is a failure, the shift register port is closed.
        //*
        //* revision history:
        //*
        //*  date	author		revision
        //*  ----	------		--------
        //*  11/30/93	Bill Harker	created
        //*
        //*============================================================================*/

        public unsafe int sr_get_data(ref run_rec_ext run_ptr)
        {

            sr_h.sr_rdout rdout = new sr_h.sr_rdout();		/* sr scaler data */
            sr_h.sr_mult_rdout mult_rdout;	/* sr multiplicity data */
            sr_h.sr_mult_rdout2 mult_rdout2;	/* sr multiplicity data */
            int bincount = sr_h.SR_MAX_MULT;
            byte sr_status;
            int status, mult_status;
            ushort num_trys;
            ushort i, j;

            /* check for shift register stopped */
            sr_status = 0;
            status = SRLib.Control(dsid.SerialPort, sr_h.SR_GET_STATUS, ref sr_status);
            if (status != sr_h.SR_SUCCESS)
            {
                status = sr_restart();
                if (status == SUCCESS)
                    return (SR_TRY_AGAIN);
                else
                    return (FAIL);
            }
            if ((sr_status & sr_h.SR_S_FAULT) != 0)
            {
                status = sr_restart();
                if (status == SUCCESS)
                    return (SR_TRY_AGAIN);
                else
                    return (FAIL);
            }
            if (((sr_status & sr_h.SR_S_TIMEOUT) == 0) && ((sr_status & sr_h.SR_S_STOPPED) == 0))
                return (SR_NOT_FINISHED);
            log.TraceEvent(LogLevels.Info, 0x4f32E, "Getting data from the shift register");
            /* get a set of scaler data from the shift register */
            num_trys = 0;
            do
            {
                status = SRLib.Control(dsid.SerialPort, sr_h.SR_GET_RDOUT, ref rdout);
                num_trys++;
            } while ((status == sr_h.SR_TIMEOUT) && (num_trys <= SR_NUM_TRYS));

            /* get a set of multiplicity data from the shift register */
            if ((dsid.SRType == InstrType.MSR4A) ||
                (dsid.SRType == InstrType.PSR) || AMSRFerSher)
            {
                mult_rdout = new sr_h.sr_mult_rdout();	/* sr multiplicity data */
                num_trys = 0;
                do
                {
                    Thread.Sleep(100);		/* allow windows system to execute */
                    mult_status = SRLib.Control(dsid.SerialPort, sr_h.SR_GET_MULT_RDOUT, ref mult_rdout);
                    num_trys++;
                } while ((mult_status == sr_h.SR_TIMEOUT) && (num_trys <= SR_NUM_TRYS));
            }
            else if (JSR15orUNAPMasqueradingAsAMSR || dsid.SRType == InstrType.JSR15 || dsid.SRType == InstrType.UNAP)
            {
                bincount = sr_h.SR_MAX_MULT2;
                mult_rdout2 = new sr_h.sr_mult_rdout2();	/* 512 sr multiplicity data */
                num_trys = 0;
                do
                {
                    Thread.Sleep(100);		/* allow windows system to execute */
                    mult_status = SRLib.Control(dsid.SerialPort, sr_h.SR_GET_MULT_RDOUT2, ref mult_rdout2);
                    num_trys++;
                } while ((mult_status == sr_h.SR_TIMEOUT) && (num_trys <= SR_NUM_TRYS));
            }
            else
            {
                mult_rdout = new sr_h.sr_mult_rdout();
                mult_status = sr_h.SR_SUCCESS;
                mult_rdout.n = bincount;
                for (j = 0; j < bincount; j++)
                {
                    mult_rdout.rpa[j] = 0;
                    mult_rdout.a[j] = 0;
                }
            }

            if ((status != sr_h.SR_SUCCESS) || (mult_status != sr_h.SR_SUCCESS))
            {
                status = sr_restart();
                if (status == SUCCESS)
                    return (SR_TRY_AGAIN);
                else
                    return (FAIL);
            }
            else if (rdout.time <= 0)
            {
                return (ZERO_COUNT_TIME);
            }

            /* read high voltage */
            if (dsid.SRType == InstrType.JSR12)
            {
                SRLib.Ioctl(dsid.SerialPort, sr_h.SR_JSR12_GET_HV,
                    ref run_ptr.run_high_voltage);
            }
            else if ((dsid.SRType == InstrType.PSR) ||
                (dsid.SRType == InstrType.AMSR) || (dsid.SRType == InstrType.UNAP))
            {
                SRLib.Ioctl(dsid.SerialPort, sr_h.SR_PSR_GET_HV,
                 ref run_ptr.run_high_voltage);
            }
            else if (dsid.SRType == InstrType.JSR15) // HHMR
            {
                SRLib.Ioctl(dsid.SerialPort, sr_h.SR_HHMR_GET_HV,
                    ref run_ptr.run_high_voltage);
            }
            else if (dsid.SRType == InstrType.DGSR)
            {
                SRLib.Ioctl(dsid.SerialPort, sr_h.SR_DGSR_GET_HV,
                    ref run_ptr.run_high_voltage);
            }
            else
            {
                run_ptr.run_high_voltage = 0.0;
            }

            /* put shift register data in run record */
            /* use current date and time */
            string dt = String.Empty;
            int x = NCCTransfer.INCC.Gen32.gen_date_time(NCCTransfer.INCC.Gen32.GEN_DTF_IAEA, ref dt);

            //gen_date_time (GEN_DTF_IAEA, &string_addr);
            //strcpy (run_date_time_string, string_addr);
            Byte[] dtba = Encoding.ASCII.GetBytes(dt);
            fixed (byte* rd = run_ptr.run_date,
                    rt = run_ptr.run_time,
                    tst = run_ptr.run_tests)
            {
                TransferUtils.Copy(dtba, 0, rd, 0, INCC.DATE_TIME_LENGTH);
                TransferUtils.Copy(dtba, 9, rt, 0, INCC.DATE_TIME_LENGTH); // ? index?
                TransferUtils.PassPack(tst);

                run_ptr.run_count_time = rdout.time;
                run_ptr.run_singles = rdout.totals;
                run_ptr.run_scaler1 = rdout.totals2;
                run_ptr.run_scaler2 = rdout.totals3;
                run_ptr.run_reals_plus_acc = rdout.rpa;
                if (dsid.SRType == InstrType.DGSR)  // todo: gate_length2, but no-one will ever need it
                {
                    run_ptr.run_acc = rdout.a * (sr_parms.gateLengthMS / sr_parms.gateLengthMS);
                    //  sr_parms.gate_length2);
                }
                else
                {
                    run_ptr.run_acc = rdout.a;
                }

                fixed (double* RA = run_ptr.run_mult_reals_plus_acc, A = run_ptr.run_mult_acc)
                {
                        for (i = 0; i < bincount; i++)
                        {
                            if (bincount == sr_h.SR_MAX_MULT)
                            {
                                RA[i] = mult_rdout.rpa[i];
                                A[i] = mult_rdout.a[i];
                            }
                            else  if (bincount == sr_h.SR_MAX_MULT2)
                            {
                                RA[i] = mult_rdout2.rpa[i];
                                A[i] = mult_rdout2.a[i];
                            }
                        }
                }
            }
            return (sr_h.SR_SUCCESS);

        }



        //* <f> */
        //*============================================================================
        //*
        //* function name: sr_stop()
        //*
        //* purpose: stop and close shift register.
        //*
        //* return value: None.
        //*
        //* special notes:
        //*
        //* revision history:
        //*
        //*  date	author		revision
        //*  ----	------		--------
        //*  11/30/93	Bill Harker	created
        //*
        //*============================================================================*/

        public int sr_stop()
        {
            int status;
            log.TraceEvent(LogLevels.Info, 0x4f32D, "Stopping the shift register");
            status = SRLib.Control(dsid.SerialPort, sr_h.SR_STOP, null);
            if (status != sr_h.SR_NOTOPEN)
            {
                SRLib.Close(dsid.SerialPort);
                log.TraceEvent(LogLevels.Info, 0x4f32C, "Closing the serial port");
            }
            return status;
        }


        //* <f> */
        //*============================================================================
        //*
        //* function name: sr_restart()
        //*
        //* purpose: stop and restart the shift register.
        //*
        //* return value: SUCCESS/FAIL
        //*
        //* special notes:
        //*
        //* revision history:
        //*
        //*  date	author		revision
        //*  ----	------		--------
        //*  11/30/93	Bill Harker	created
        //*
        //*============================================================================*/

        public int sr_restart()
        {

            int status;

            Thread.Sleep(200);		/* allow windows system to execute */
            sr_stop();

            status = sr_init(restart_time_per_run, restart_sethigh_voltage);
            if ((status != sr_h.SR_SUCCESS))
            {
                log.TraceEvent(LogLevels.Warning, 0x4f333, "{0}; Unable to restart shift register", sr_h.SRFunctionReturnStatusCode(status));
                return (FAIL);
            }

            return (SUCCESS);

        }
    }
}
