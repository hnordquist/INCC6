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
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AnalysisDefs;
using Device;
using NCC;
using NCCFile;
using NCCReporter;

namespace LMDAQ
{
	using NC = NCC.CentralizedState;

	/// <summary>
	/// Represents a MCA-527 device 
	/// </summary>
	public class MCA527Instrument : LMInstrument, IEquatable<MCA527Instrument>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MCA527Instrument"/> class.
        /// </summary>
        /// <param name="detector">The detector.</param>
        public MCA527Instrument(Detector detector)
            : base(detector)
        {
            LMMMConfig lm = ((DetectorDefs.LMConnectionInfo)(detector.Id.FullConnInfo)).DeviceConfig;
            m_setvoltage = ((lm.LEDs == 2) || (lm.LEDs == 0)) ? false : true;
            m_voltage = (int)detector.MultiplicityParams.SR.highVoltage;
            file = new MCAFile();
            file.Log = NC.App.Loggers.Logger(LMLoggers.AppSection.Collect);
            MaxSetVoltageTime = TimeSpan.FromSeconds(((DetectorDefs.LMConnectionInfo)(detector.Id.FullConnInfo)).DeviceConfig.HVTimeout);
            VoltageTolerance = lm.VoltageTolerance;
        }

        /// <summary>
        /// Gets the device name.
        /// </summary>
        public string DeviceName
        {
            get { return id.DetectorId; }
        }

		/// <summary>
        /// Gets the device electronics id.
        /// </summary>
        public string ElectronicsId
        {
            get { return id.ElectronicsId; }
        }

		/// <summary>
        /// The device IP endpoint.
        /// </summary>
        public Device.MCADeviceInfo DeviceInfo
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the raw data transform (RDT).
        /// </summary>
        public new Analysis.MCA527RawDataTransform RDT
        {
            get { return (Analysis.MCA527RawDataTransform) base.RDT; }
            set { base.RDT = value; }
        }

        /// <summary>
        /// Initializes the instrument.
        /// </summary>
        /// <param name="dataLog">The data log.</param>
        /// <param name="analysisLog">The analysis log.</param>
        public override void Init(LMLoggers.LognLM dataLog, LMLoggers.LognLM analysisLog)
        {
            if (RDT == null) {
                RDT = new Analysis.MCA527RawDataTransform();
                RDT.Init(
                    (dataLog != null) ? dataLog : NC.App.Logger(LMLoggers.AppSection.Data),
                    (analysisLog != null) ? analysisLog : NC.App.Logger(LMLoggers.AppSection.Analysis));
            }
        }

        /// <summary>
        /// Connects to the instrument.
        /// </summary>
        /// <exception cref="MCA527Exception">An error occurred communicating with the device.</exception>
        public override void Connect()
        {
            if (m_device != null)
            {
                // Already connected
                return;
            }

			try 
			{ 
				m_device =  MCADevice.ConnectToDeviceAtAddress(DeviceInfo.Address);
				//m_device.CallbackObject = new MCADeviceCallbackObject();  // URGENT next fill this callback in
				m_device.Initialize().Wait();
				DAQState = DAQInstrState.Online;
				m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Connected to {1}, MCA527 firmware version is {2}", ElectronicsId, DeviceName, DeviceInfo.FirmwareVersion);
			}
			catch (Exception pex) 
			{
				DAQState = DAQInstrState.Offline;
				throw new Exception("MCA527 connect problem", pex);
			} 

			m_logger.Flush();

        }

        /// <summary>
        /// Starts an assay operation.
        /// </summary>
        /// <param name="measurement">The measurement.</param>
        /// <exception cref="InvalidOperationException">An operation is already in progress.</exception>
        public override Task StartAssay(Measurement measurement)
        {
            m_logger.TraceEvent(LogLevels.Info, 0,
                "MCA527[{0}]: Starting {1}s assay...",
                DeviceName, measurement.AcquireState.lm.Interval);
            m_logger.Flush();

            lock (m_monitor) {
                if (m_cancellationTokenSource != null) {
                    throw new InvalidOperationException("An operation is already in progress.");
                }

                m_cancellationTokenSource = new CancellationTokenSource();
            }

            CancellationToken cta = NC.App.Opstate.CancelStopAbort.NewLinkedCancelStopAbortAndClientToken(m_cancellationTokenSource.Token);
            return Task.Factory.StartNew(() => PerformAssay(measurement, cta), cta);
        }

        /// <summary>
        /// Performs an assay operation.
        /// </summary>
        /// <param name="measurement">The measurement.</param>
        /// <param name="cancellationToken">The cancellation token to observe.</param>
        /// <exception cref="MCA527Exception">An error occurred communicating with the device.</exception>
        protected async void PerformAssay(Measurement measurement, CancellationToken cancellationToken)
        {
            try {
                m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Started assay", DeviceName);
                m_logger.Flush();

                //if (m_setvoltage)
                //   await SetVoltage(m_voltage, MaxSetVoltageTime, CancellationToken.None);
            
                Stopwatch stopwatch = new Stopwatch();
                TimeSpan duration = TimeSpan.FromSeconds(measurement.AcquireState.lm.Interval);
                byte[] buffer = new byte[1024 * 1024];
                long total = 0;

                //m_device.Reset();
                stopwatch.Start();
                m_logger.TraceEvent(LogLevels.Verbose, 11901, "{0} start", DateTime.Now.ToString());
                while (stopwatch.Elapsed < duration) {
                //    cancellationToken.ThrowIfCancellationRequested();

                //    if (m_device.Available > 0) {
                //        int bytesRead = m_device.Read(buffer, 0, buffer.Length);

                //        if (bytesRead > 0) {
                //            RDT.PassBufferToTheCounters(buffer, 0, bytesRead);
                //            total += bytesRead;
                //        }
                //    }
                }

                stopwatch.Stop();
                m_logger.TraceEvent(LogLevels.Verbose, 11901, "{0} stop", DateTime.Now.ToString());

                lock (m_monitor) {
                    m_cancellationTokenSource.Dispose();
                    m_cancellationTokenSource = null;
                }

                m_logger.TraceEvent(LogLevels.Info, 0,
                    "MCA527[{0}]: Finished assay; read {1} bytes in {2}s",
                    DeviceName, total, stopwatch.Elapsed.TotalSeconds);
                m_logger.Flush();
                DAQControl.HandleEndOfCycleProcessing(this, new Analysis.StreamStatusBlock(@"MCA527 Done"));
            }
            catch (OperationCanceledException) {
                //Analysis.StreamStatusBlock ssb = new Analysis.StreamStatusBlock(@"Assay Cancelled.");
                //DAQControl.HandleEndOfCycleProcessing(this, ssb);

                m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Stopped assay", DeviceName);
                m_logger.Flush();
                DAQControl.StopActiveAssayImmediately();
                throw;
            }
            catch (Exception ex) {
                m_logger.TraceEvent(LogLevels.Error, 0, "MCA527[{0}]: Error during assay: {1}", DeviceName, ex.Message);
                m_logger.TraceException(ex, true);
                m_logger.Flush();
                DAQControl.HandleFatalGeneralError(this, ex);
                throw;
            }
        }

        /// <summary>
        /// Stops the currently executing assay operation.
        /// </summary>
        public override void StopAssay()
        {
            m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}] Stopping assay...", DeviceName);
            m_logger.Flush();

            lock (m_monitor) {
                if (m_cancellationTokenSource != null) {
                    m_cancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// Starts a high voltage calibration operation.
        /// </summary>
        /// <param name="voltage">The voltage to set in volts.</param>
        /// <param name="duration">The length of the measurement to take.</param>
        /// <exception cref="InvalidOperationException">An operation is already in progress.</exception>
        public override Task StartHVCalibration(int voltage, TimeSpan duration)
        {
            m_logger.TraceEvent(LogLevels.Info, 0,
                "MCA527[{0}]: Starting HV calibration at {1}V for {2}s...",
                DeviceName, voltage, duration.TotalSeconds);
            m_logger.Flush();

            lock (m_monitor) {
                if (m_cancellationTokenSource != null) {
                    throw new InvalidOperationException("An operation is already in progress.");
                }

                m_cancellationTokenSource = new CancellationTokenSource();
            }

            CancellationToken cancellationToken = m_cancellationTokenSource.Token;
            return Task.Factory.StartNew(() => PerformHVCalibration(voltage, duration, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Performs a high voltage calibration operation.
        /// </summary>
        /// <param name="voltage">The voltage to set in volts.</param>
        /// <param name="duration">The length of the measurement to take.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to monitor for cancellation requests.</param>
        /// <exception cref="OperationCanceledException">Cancellation was requested.</exception>
        /// <exception cref="MCA527Exception">An error occurred communicating with the device.</exception>
        private async void PerformHVCalibration(int voltage, TimeSpan duration, CancellationToken cancellationToken)
        {
            try {
                m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Started HV calibration", DeviceName);
                m_logger.Flush();

                SetVoltage(voltage, MaxSetVoltageTime, cancellationToken);

                MCA527RateCounter counter = new MCA527RateCounter(m_device);
                counter.TakeMeasurement(duration, cancellationToken);

                HVControl.HVStatus status = new HVControl.HVStatus();
                status.HVread = (int) await m_device.GetHighVoltage();
                status.HVsetpt = voltage;

                for (int i = 0; i < MCA527.ChannelCount; i++) {
                    status.counts[i] = (ulong) counter.ChannelCounts[i];
                }

                lock (m_monitor) {
                    m_cancellationTokenSource.Dispose();
                    m_cancellationTokenSource = null;
                }

                m_logger.TraceEvent(LogLevels.Info, 0,
                    "MCA527[{0}]: Finished HV calibration; read {1} bytes in {2}s",
                    DeviceName, counter.ByteCount, counter.Time.TotalSeconds);
                m_logger.Flush();

                DAQControl.gControl.AppendHVCalibration(status);
                DAQControl.gControl.StepHVCalibration();
            }
            catch (OperationCanceledException) {
                m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Stopped HV calibration", DeviceName);
                m_logger.Flush();
                DAQControl.gControl.MajorOperationCompleted();  // causes pending control thread caller to move forward
                PendingComplete();
                throw;
            }
            catch (Exception ex) {
                m_logger.TraceEvent(LogLevels.Error, 0, "MCA527[{0}]: Error during HV calibration: {1}", DeviceName, ex.Message);
                m_logger.TraceException(ex, true);
                m_logger.Flush();
                DAQControl.gControl.MajorOperationCompleted();  // causes pending control thread caller to move forward
                PendingComplete();
                throw;
            }
        }

        /// <summary>
        /// Stops the currently executing high voltage calibration operation.
        /// </summary>
        public override void StopHVCalibration()
        {
            m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}] Stopping HV calibration...", DeviceName);
            m_logger.Flush();

            lock (m_monitor) {
                if (m_cancellationTokenSource != null) {
                    m_cancellationTokenSource.Cancel();
                }
            }
        }

        /// <summary>
        /// Closes the connection to the instrument.
        /// </summary>
        public override void Disconnect()
        {
            if (m_device == null) {
                // Not connected
                return;
            }

            //m_device.SetVoltage(0);
            m_device.Close();
            m_device = null;

            m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}] Disconnected", DeviceName);
            m_logger.Flush();
        }

        /// <summary>
        /// Sets the voltage.
        /// </summary>
        /// <param name="voltage">The voltage to set in volts.</param>
        /// <param name="timeout">The maximum length of time to wait for the voltage to be set.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="OperationCanceledException">Cancellation was requested through <paramref name="cancellationToken"/>.</exception>
        /// <exception cref="MCA527Exception">An error occurred communicating with the device.</exception>
        /// <exception cref="TimeoutException">The voltage was not set within the allotted time.</exception>
        /// <remarks>
        /// This method does not return until one of the following occurs:
        /// <list type="number">
        /// <item><description>The voltage is within <see cref="VoltageTolerance"/> volts of <paramref name="voltage"/>.</description></item>
        /// <item><description>More than <paramref name="timeout"/> time has elapsed.</description></item>
        /// <item><description>Cancellation is requested through <paramref name="cancellationToken"/>.</description></item>
        /// </list>
        /// </remarks>
        private void SetVoltage(int voltage, TimeSpan timeout, CancellationToken cancellationToken)
        {
            m_logger.TraceEvent(LogLevels.Verbose, 0, "MCA527[{0}]: Setting voltage to {1} volts, timeout is {2}, tolerance is {3}...", DeviceName, voltage, MaxSetVoltageTime.ToString("g"), VoltageTolerance);
            m_logger.Flush();

            // set the voltage and check it, loop for timeout seconds waiting for it to settle at the desired value
            //return m_device.SetHighVoltage(0, BiasInhibitInput.InhibitOff).Wait();
			           
            //m_logger.TraceEvent(LogLevels.Verbose, 0, "MCA527[{0}]: Voltage set to {1} volts (Elapsed time: {2})",
            //    DeviceName, lrvoltage, stopwatch.Elapsed.ToString("g"));
            //m_logger.Flush();
        }

				/// <summary>
        /// Gets a string identifying the DpcUtil.dll version.
        /// </summary>
        /// <returns>A string identifying the DpcUtil.dll version or a 'not found' text.</returns>
        /// <exception cref="MCA527Exception">An error occurred communicating with the device.</exception>
        protected static string GetDpcUtilsVersion()
        {
			string dllpath = System.IO.Path.Combine(Environment.SystemDirectory, "dpcutil.dll");
			if (System.IO.File.Exists(dllpath)) 
			{ 
				System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(dllpath);
				 return fvi.FileVersion;
			}
			else
				return "... MCA527 USB driver not found " + dllpath;           
        }

        /// <summary>
        /// Determines whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the current object is equal to <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(MCA527Instrument other)
        {
            return base.Equals((LMInstrument) other);
        }

        /// <summary>
        /// Determines whether the current object is equal to another object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the current object is equal to <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object other)
        {
            if (other == null) {
                return false;
            }

            if (GetType() != other.GetType()) {
                return false;
            }

            return Equals((MCA527Instrument) other);
        }

        /// <summary>
        /// Gets a hash code for the current object.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Determines whether two <see cref="MCA527Instrument"/> objects are equal.
        /// </summary>
        /// <param name="first">The first object to compare.</param>
        /// <param name="second">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are equal; otherwise, <c>false</c>.</returns>
        public static bool operator ==(MCA527Instrument first, MCA527Instrument second)
        {
            if (first == null) {
                if (second == null) {
                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                if (second == null) {
                    return false;
                }
                else {
                    return first.Equals(second);
                }
            }
        }

        /// <summary>
        /// Determines whether two <see cref="MCA527Instrument"/> objects are unequal.
        /// </summary>
        /// <param name="first">The first object to compare.</param>
        /// <param name="second">The second object to compare.</param>
        /// <returns><c>true</c> if <paramref name="first"/> and <paramref name="second"/> are not equal; otherwise, <c>false</c>.</returns>
        public static bool operator !=(MCA527Instrument first, MCA527Instrument second)
        {
            if (first == null) {
                if (second == null) {
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                if (second == null) {
                    return true;
                }
                else {
                    return !first.Equals(second);
                }
            }
        }

        private readonly TimeSpan MaxSetVoltageTime;
        //private const int VoltageTolerance = 1;

        private CancellationTokenSource m_cancellationTokenSource;
        private MCADevice m_device;
        private LMLoggers.LognLM m_logger = CentralizedState.App.Logger(LMLoggers.AppSection.Collect);
        private object m_monitor = new object();
        private int m_voltage;
        private bool m_setvoltage;
        private int VoltageTolerance;
    }
}
