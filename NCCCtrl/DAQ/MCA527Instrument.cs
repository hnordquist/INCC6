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
using System.Threading;
using System.Threading.Tasks;
using Analysis;
using AnalysisDefs;
using DAQ;
using Device;
using NCCFile;
using NCCReporter;
namespace Instr
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
            m_voltage = (ushort)detector.MultiplicityParams.SR.highVoltage;
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
        public new MCA527RawDataTransform RDT
        {
            get { return (MCA527RawDataTransform) base.RDT; }
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
                RDT = new MCA527RawDataTransform();
                RDT.Init(
                    (dataLog != null) ? dataLog : NC.App.Logger(LMLoggers.AppSection.Data),
                    (analysisLog != null) ? analysisLog : NC.App.Logger(LMLoggers.AppSection.Analysis));
            }
        }

        /// <summary>
        /// Connects to the instrument.
        /// </summary>
        /// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
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
				//m_device.CallbackObject = new MCADeviceCallbackObject();  // URGENT: next fill this callback in
				m_device.Initialize();
				DAQState = DAQInstrState.Online;
				m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Connected to {1}, MCA527 firmware version is {2}", ElectronicsId, DeviceName, DeviceInfo.FirmwareVersion);
			}
			catch (MCADeviceLostConnectionException mex)
			{
				DAQState = DAQInstrState.Offline;
				throw mex;
			}
			catch (Exception pex) 
			{
				DAQState = DAQInstrState.Offline;
				throw pex;
			} 

			m_logger.Flush();
        }


       /// <summary>
        /// Prepare MCA-527 for a new sweep or cycle
        /// </summary>
        /// <param name="cycle">Cycle sequence number</param>
        /// <param name="secs">Cycle length in seconds</param>
        /// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
		bool InitSettings(ushort cycle, uint secs)
		{
			MCAResponse response = null;
			response = m_device.Client.Send(MCACommand.Clear(ClearMode.ClearMeasurementData0));
			if (response == null)
			{ throw new MCADeviceLostConnectionException(); }
			response = m_device.Client.Send(MCACommand.Clear(ClearMode.ClearMeasurementData1));
			if (response == null)
			{ throw new MCADeviceLostConnectionException(); }
			if (cycle == 1)  // init some values on first cycle
			{
				response = m_device.Client.Send(MCACommand.SetPresets(Presets.Real, secs));
				if (response == null)
				{ throw new MCADeviceLostConnectionException(); }
				response = m_device.Client.Send(MCACommand.SetRepeat(1));
				if (response == null)
				{ throw new MCADeviceLostConnectionException(); }
			}
			return true;
		}

		/// <summary>
		/// Starts an assay operation.
		/// </summary>
		/// <param name="measurement">The measurement.</param>
		/// <exception cref="InvalidOperationException">An operation is already in progress.</exception>
		public override void StartAssay(Measurement measurement)
		{
			m_logger.TraceEvent(LogLevels.Info, 0,
				"MCA527[{0}]: Starting {1}s assay...",
				DeviceName, measurement.AcquireState.lm.Interval);
			m_logger.Flush();

			lock (m_monitor)
			{
				if (m_cancellationTokenSource != null)
					throw new InvalidOperationException("An operation is already in progress.");
				m_cancellationTokenSource = new CancellationTokenSource();
			}
			CancellationToken cta = NC.App.Opstate.CancelStopAbort.NewLinkedCancelStopAbortAndClientToken(m_cancellationTokenSource.Token);
			Task.Factory.StartNew(() => PerformAssay(measurement, cta), cta,
					TaskCreationOptions.PreferFairness, 
					TaskScheduler.Default);

		}

		/// <summary>
		/// Performs a single cycle assay operation.
		/// </summary>
		/// <param name="measurement">The measurement.</param>
		/// <param name="cancellationToken">The cancellation token to observe.</param>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
		/// <exception cref="MCADeviceBadDataException">An error occurred with the raw data strem state.</exception>
		protected void PerformAssay(Measurement measurement, CancellationToken cancellationToken)
		{
			//m_device.mHeartbeatSemaphore.Wait();
			MCA527ProcessingState ps = (MCA527ProcessingState)(RDT.State);

			try
			{
				if (ps == null)
					throw new Exception("Big L bogus state");
				ushort seq = measurement.CurrentRepetition;
				m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Started assay {1}", DeviceName, seq);
				m_logger.Flush();

				if (m_setvoltage)
						SetVoltage(m_voltage, MaxSetVoltageTime, CancellationToken.None);

				if (seq == 1)  // init var on first cycle
					ps.device = m_device;

				bool x = InitSettings(seq, (uint)measurement.AcquireState.lm.Interval);  // truncate for discrete integer result

				Stopwatch stopwatch = new Stopwatch();
				TimeSpan duration = TimeSpan.FromSeconds((uint)measurement.AcquireState.lm.Interval);
				byte[] buffer = new byte[1024 * 1024];
				long total = 0;

				ps.BeginSweep(measurement.CurrentRepetition);

				// a5 5a 42 00 01 00 ae d5 44 56 b9 9b
				// flags: 0x0001 => spectrum is cleared and a new start time is set
				// start time: 0x5644d5ae => seconds since Dec 31, 1969, 16:00:00 GMT
				uint secondsSinceEpoch = (uint)(Math.Abs(Math.Round((DateTime.UtcNow - MCADevice.MCA527EpochTime).TotalSeconds)));
  				MCAResponse response = m_device.Client.Send(MCACommand.Start(StartFlag.SpectrumClearedNewStartTime,
																false, false, false, secondsSinceEpoch));
				if (response == null) { throw new MCADeviceLostConnectionException(); }

				bool done = false;
				const uint CommonMemoryBlockSize = 1440;
				// what's the most that could be left over from a previous attempt to decode? => 3 bytes
				byte[] rawBuffer = new byte[CommonMemoryBlockSize + 3];
				ulong[] timestampsBuffer = new ulong[CommonMemoryBlockSize + 1];

				uint commonMemoryReadIndex = 0;
				uint rawBufferOffset = 0;

				stopwatch.Start();
				m_logger.TraceEvent(LogLevels.Verbose, 11901, "{0} start time for {1}", DateTime.Now.ToString(), seq);

				ulong accumulatedTime = 0;
				int maxindex = 0;
				TimeSpan interregnum = stopwatch.Elapsed;
				while (interregnum <= duration)
				{
					cancellationToken.ThrowIfCancellationRequested();

					QueryState527ExResponse qs527er = (QueryState527ExResponse)m_device.Client.Send(MCACommand.QueryState527Ex());
					if (qs527er == null) { throw new MCADeviceLostConnectionException(); }

					MCAState state = qs527er.MCAState;
					done = state != MCAState.Run;

					// pull off some data while we are waiting...
					uint commonMemoryFillLevel = qs527er.CommonMemoryFillLevel;
					uint bytesAvailable = commonMemoryFillLevel - commonMemoryReadIndex;

					if (state != MCAState.Run && bytesAvailable == 0)
					{
						break;
					}

					if (bytesAvailable >= CommonMemoryBlockSize)
					{
						QueryCommonMemoryResponse qcmr = (QueryCommonMemoryResponse)m_device.Client.Send(MCACommand.QueryCommonMemory(commonMemoryReadIndex / 2));
						if (qcmr == null)
							{ throw new MCADeviceLostConnectionException(); }
						// bytesToCopy needs to always be even, so that commonMemoryReadIndex always stays even...
						uint bytesToCopy = Math.Min(bytesAvailable / 2, CommonMemoryBlockSize / 2) * 2;
						qcmr.CopyData(0, rawBuffer, (int)rawBufferOffset, (int)bytesToCopy);

						if (ps.file.writer != null)
						{
							ps.file.WriteTimestampsRawDataChunk(rawBuffer, 0, (int)bytesToCopy);
							//m_logger.TraceEvent(LogLevels.Verbose, 0, "{0} bytes to copy", bytesToCopy);
						}

						rawBufferOffset += bytesToCopy;
						commonMemoryReadIndex += bytesToCopy;
						uint timestampsCount = m_device.TransformRawData(rawBuffer, ref rawBufferOffset, timestampsBuffer);

						// make sure rawBufferOffset is never greater than 3 after transforming data
						// => means something has gone wrong
						if (rawBufferOffset > 3)
						{
							throw new MCADeviceBadDataException();
						}
						// accumulate a big buffer full before handing the buffer over for processing

						interregnum = stopwatch.Elapsed;
						// copy the data out...
						if (timestampsCount > 0)
						{
							if (maxindex < RDT.State.maxValuesInBuffer && interregnum <= duration)
							{
								if (RDT.State.timeArray.Count < maxindex+timestampsCount)
									RDT.State.timeArray.AddRange(new ulong[timestampsCount]);
								for (int i = 0; i < timestampsCount; i++)
								{
									accumulatedTime += timestampsBuffer[i];
									RDT.State.timeArray[maxindex] = accumulatedTime;
									maxindex++;  // always 1 more than the last index
								}
								RDT.State.NumValuesParsed += timestampsCount;
								RDT.State.hitsPerChn[0] += timestampsCount;
								RDT.State.NumTotalsEncountered = RDT.State.NumValuesParsed; // only one channel
							} 
							else
							{
								long _max = RDT.State.NumValuesParsed;
								if (_max < RDT.State.neutronEventArray.Count)  // equalize the length of the empty neutron channel event list 
									RDT.State.neutronEventArray.RemoveRange((int)_max, RDT.State.neutronEventArray.Count - (int)_max);
								else if (_max > RDT.State.neutronEventArray.Count)
									RDT.State.neutronEventArray.AddRange(new uint[_max - RDT.State.neutronEventArray.Count]);
								if (_max < RDT.State.timeArray.Count)
									RDT.State.timeArray.RemoveRange((int)_max, RDT.State.timeArray.Count - (int)_max);
								else if (_max > RDT.State.timeArray.Count)
									RDT.State.timeArray.AddRange(new ulong[_max - RDT.State.timeArray.Count]);
								RDT.PassBufferToTheCounters((int)_max);
								m_logger.TraceEvent(LogLevels.Verbose, 88, "{0} timestampsCount {1}", timestampsCount, RDT.State.NumValuesParsed);
							}
						}
					} 
					else if (bytesAvailable > 0 && state != MCAState.Run)
					{
						// special case for when there's not a whole block left to read
						// we can only read up to the address: CommonMemorySize - 1440
						uint commonMemorySize = qs527er.CommonMemorySize;

						uint readAddress = commonMemoryReadIndex;
						uint readOffset = 0;
						if (readAddress > commonMemorySize - 1440)
						{
							readOffset = readAddress - (commonMemorySize - 1440);
							readAddress -= readOffset;
						}

						QueryCommonMemoryResponse qcmr = (QueryCommonMemoryResponse)m_device.Client.Send(MCACommand.QueryCommonMemory(readAddress / 2));
						if (qcmr == null)
						{ throw new MCADeviceLostConnectionException(); }
						uint bytesToCopy = bytesAvailable;
						qcmr.CopyData((int)readOffset, rawBuffer, (int)rawBufferOffset, (int)bytesToCopy);

						if (ps.file.writer != null)
						{
							ps.file.WriteTimestampsRawDataChunk(rawBuffer, (int)readOffset, (int)bytesToCopy);
							//m_logger.TraceEvent(LogLevels.Verbose, 0, "{0} bytes to copy", bytesToCopy);
						}

						rawBufferOffset += bytesToCopy;
						commonMemoryReadIndex += bytesToCopy;

						uint timestampsCount = m_device.TransformRawData(rawBuffer, ref rawBufferOffset, timestampsBuffer);

						//if (rawBufferOffset > 0) {
						// apparently this can happen. Perhaps when the device gets cut off (because of a timer event), right in the middle of writing?
						//throw new MCADeviceBadDataException();
						//}
						interregnum = stopwatch.Elapsed;

						if (timestampsCount > 0)
						{
							// copy the timestampsBuffer value into the RDT.State.timeArray, Q: wait to fill a much large internal buffer before calling the transform?
							if (maxindex < RDT.State.maxValuesInBuffer && interregnum <= duration)
							{
								if (RDT.State.timeArray.Count < maxindex+timestampsCount)
									RDT.State.timeArray.AddRange(new ulong[timestampsCount]);
								for (int i = 0; i < timestampsCount; i++)
								{
									accumulatedTime += timestampsBuffer[i];
									RDT.State.timeArray[maxindex] = accumulatedTime;
									maxindex++;  // always 1 more than the last index
								}
								RDT.State.NumValuesParsed += timestampsCount;
								RDT.State.hitsPerChn[0] += timestampsCount;
								RDT.State.NumTotalsEncountered = RDT.State.NumValuesParsed; // only one channel
							} 
							else
							{
								long _max = RDT.State.NumValuesParsed;
								if (_max < RDT.State.neutronEventArray.Count)  // equalize the length of the empty neutron channel event list 
									RDT.State.neutronEventArray.RemoveRange((int)_max, RDT.State.neutronEventArray.Count - (int)_max);
								else if (_max > RDT.State.neutronEventArray.Count)
									RDT.State.neutronEventArray.AddRange(new uint[_max - RDT.State.neutronEventArray.Count]);
								if (_max < RDT.State.timeArray.Count)
									RDT.State.timeArray.RemoveRange((int)_max, RDT.State.timeArray.Count - (int)_max);
								else if (_max > RDT.State.timeArray.Count)
									RDT.State.timeArray.AddRange(new ulong[_max - RDT.State.timeArray.Count]);

								RDT.PassBufferToTheCounters((int)_max);
							}
						}
					} 
					else
					{
						// give the device a break
						Thread.Sleep(20); // 100 ms
					}
				}

				stopwatch.Stop();
				m_logger.TraceEvent(LogLevels.Verbose, 11901, "{0} stop time for {1}", DateTime.Now.ToString(), seq);
				ps.FinishedSweep(seq, stopwatch.Elapsed.TotalSeconds);
				
				m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Finished assay; read {1} bytes in {2}s for {3}", DeviceName, total, stopwatch.Elapsed.TotalSeconds, seq);
				m_logger.Flush();
				lock (m_monitor)
				{
					m_cancellationTokenSource.Dispose();
					m_cancellationTokenSource = null;
				}
				DAQControl.HandleEndOfCycleProcessing(this, new StreamStatusBlock(@"MCA527 Done"));
				m_logger.TraceEvent(LogLevels.Verbose, 11911, "HandleEndOfCycle for {0}", seq);
				m_logger.Flush();
				if (NC.App.AppContext.LiveFileWrite)
				{
					m_device.CreateWriteHeaderAndClose(ps.file);
					m_logger.TraceEvent(LogLevels.Verbose, 11921, "WriteHeader for {0}", seq);
					m_logger.Flush();
				}
			}
			catch (OperationCanceledException)
			{
				m_logger.TraceEvent(LogLevels.Warning, 767, "MCA527[{0}]: Stopping assay", DeviceName);
				m_logger.Flush();
				DAQControl.StopActiveAssayImmediately();
				//throw; cannot catch easily due to 4.5 task model or my c^&p coding, so just log and stop the task
			}
			catch (Exception ex)
			{
				m_logger.TraceEvent(LogLevels.Error, 0, "MCA527[{0}]: Error during assay: {1}", DeviceName, ex.Message);
				m_logger.TraceException(ex, true);
				m_logger.Flush();
				DAQControl.HandleFatalGeneralError(this, ex);
				//throw; cannot catch upthread due to 4.5 task model
			}
			finally
			{
				//m_device.mHeartbeatSemaphore.Release();
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
        public override void StartHVCalibration(int voltage, TimeSpan duration)
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
            Task.Factory.StartNew(() => PerformHVCalibration(voltage, duration, cancellationToken), cancellationToken);
        }

        /// <summary>
        /// Performs a high voltage calibration operation.
        /// </summary>
        /// <param name="voltage">The voltage to set in volts.</param>
        /// <param name="duration">The length of the measurement to take.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to monitor for cancellation requests.</param>
        /// <exception cref="OperationCanceledException">Cancellation was requested.</exception>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
        private void PerformHVCalibration(int voltage, TimeSpan duration, CancellationToken cancellationToken)
        {
            //try {
            //    m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Started HV calibration", DeviceName);
            //    m_logger.Flush();
            //    uint x = SetVoltage((ushort)voltage, MaxSetVoltageTime, cancellationToken);

            //    MCA527RateCounter counter = new MCA527RateCounter(m_device);
            //    counter.TakeMeasurement(duration, cancellationToken);

            //    HVControl.HVStatus status = new HVControl.HVStatus();
            //    status.HVread = (int) m_device.GetHighVoltage();
            //    status.HVsetpt = voltage;

            //    for (int i = 0; i < MCA527.ChannelCount; i++) {
            //        status.counts[i] = (ulong) counter.ChannelCounts[i];
            //    }

            //    lock (m_monitor) {
            //        m_cancellationTokenSource.Dispose();
            //        m_cancellationTokenSource = null;
            //    }

            //    m_logger.TraceEvent(LogLevels.Info, 0,
            //        "MCA527[{0}]: Finished HV calibration; read {1} bytes in {2}s",
            //        DeviceName, counter.ByteCount, counter.Time.TotalSeconds);
            //    m_logger.Flush();

            //    DAQControl.gControl.AppendHVCalibration(status);
            //    DAQControl.gControl.StepHVCalibration();
            //}
            //catch (OperationCanceledException) {
            //    m_logger.TraceEvent(LogLevels.Info, 0, "MCA527[{0}]: Stopped HV calibration", DeviceName);
            //    m_logger.Flush();
            //    DAQControl.gControl.MajorOperationCompleted();  // causes pending control thread caller to move forward
            //    PendingComplete();
            //    //throw;
            //}
            //catch (Exception ex) {
            //    m_logger.TraceEvent(LogLevels.Error, 0, "MCA527[{0}]: Error during HV calibration: {1}", DeviceName, ex.Message);
            //    m_logger.TraceException(ex, true);
            //    m_logger.Flush();
            //    DAQControl.gControl.MajorOperationCompleted();  // causes pending control thread caller to move forward
            //    PendingComplete();
            //    //throw;
            //}
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
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
		/// <exception cref="TimeoutException">The voltage was not set within the allotted time.</exception>
		/// <remarks>
		/// This method does not return until one of the following occurs:
		/// <list type="number">
		/// <item><description>The voltage is within <see cref="VoltageTolerance"/> volts of <paramref name="voltage"/>.</description></item>
		/// <item><description>More than <paramref name="timeout"/> time has elapsed.</description></item>
		/// <item><description>Cancellation is requested through <paramref name="cancellationToken"/>.</description></item>
		/// </list>
		/// </remarks>
		private uint SetVoltage(ushort voltage, TimeSpan timeout, CancellationToken cancellationToken)
		{
			m_logger.TraceEvent(LogLevels.Verbose, 0, "MCA527[{0}]: Setting voltage to {1} volts, timeout is {2}, tolerance is {3}...", DeviceName, voltage, MaxSetVoltageTime.ToString("g"), VoltageTolerance);
			m_logger.Flush();
			Stopwatch stopwatch = Stopwatch.StartNew();

			m_device.SetHighVoltage(voltage, BiasInhibitInput.InhibitOff);  // don't wait here, but loop and report, permitting cancel at any time in case the HW hose-ed

			uint hvx = m_device.GetHighVoltage();

			while (Math.Abs(hvx - voltage) > VoltageTolerance) {
                if (cancellationToken.IsCancellationRequested) {
                    m_logger.TraceEvent(LogLevels.Verbose, 0, "MCA527[{0}]: Cancellation requested while setting voltage", DeviceName);
                    m_logger.Flush();

                    cancellationToken.ThrowIfCancellationRequested();
                }
                else if (stopwatch.Elapsed > timeout) {
                    m_logger.TraceEvent(LogLevels.Warning, 0, "MCA527[{0}]: Timed out while setting voltage (last seen {1} volts)", DeviceName, hvx);
                    m_logger.Flush();

                    throw new TimeoutException("Timed out while setting voltage");
                }
				hvx = m_device.GetHighVoltage();

                if (stopwatch.Elapsed.Milliseconds % 10000 == 0)
                    m_logger.TraceEvent(LogLevels.Verbose, 0, "MCA527[{0}]: At {1} volts", DeviceName, hvx);
				Thread.Sleep(100);
            }
            m_logger.TraceEvent(LogLevels.Verbose, 0, "MCA527[{0}]: Voltage set to {1} volts (Elapsed time: {2})",
                DeviceName, hvx, stopwatch.Elapsed.ToString("g"));
            m_logger.Flush();
			return hvx;
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

        private CancellationTokenSource m_cancellationTokenSource;
        private MCADevice m_device;
        private LMLoggers.LognLM m_logger = NC.App.Logger(LMLoggers.AppSection.Collect);
        private object m_monitor = new object();
        private ushort m_voltage;
        private bool m_setvoltage;
        private int VoltageTolerance;
    }
}
