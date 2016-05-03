/*
The MCA-527 INCC integration source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

The MCA-527 INCC integration source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2016, International Atomic Energy Agency (IAEA), IAEA.org
   Authored by E. Johnson

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
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
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
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Device
{

    class FileStreamReadException : Exception { }

    enum EndFlagCode : ushort
    {
        Success = 0xB99B,
        TimeoutError = 0xA4AA,
        BaudError = 0xA5AA,
        InvalidPreambleOrEndFlagError = 0xA6AA,
        MicroSDCardError = 0xA7AA,
        FileWritingInProcessError = 0xA8AA,
        FirmwareCompatibilityError = 0xA9AA,
        InvalidParameterError = 0xAAAA,
        UnknownCommandError = 0xABAA,
        MeasurementRunningError = 0xACAA,
        ExecutionRightViolationError = 0xADAA,
        MeasurementStoppedError = 0xAEAA,
        IncorrectModeError = 0xAFAA
    }

    class MCAResponse
    {
        public byte[] Bytes;
        public IPAddress RemoteAddress;

        public virtual CommandName CommandName { get { return (CommandName)(Bytes.ReadFlippedUShort(106 + 4)); } }
        public virtual ushort Checksum { get { return Bytes.ReadUShort(126 + 4); } }
        public EndFlagCode EndFlagCode { get { return (EndFlagCode)(Bytes.ReadFlippedUShort(Bytes.Length - 2)); } }

        public override string ToString()
        {
            return string.Format("<Response: command:{0}, checksum:0x{1:X2}, endflag:{2}>", CommandName.ToString(), Checksum, EndFlagCode);
        }

        public static MCAResponse ParseResponse(byte[] bytes)
        {
            // check if it is valid!
            // must start with preamble (0xA5, 0x5A) (2x for UDP packets)
            // must end with an end flag
            // if end flag indicates an error, that's all we know
            // if it's successful:
            //  => 8 sent data bytes (part of command) are at byte offset 106
            //  => 2 byte checksum at byte offset 126
            int length = bytes.Length;
            if (length < 138) { return null; } // 132, plus 4 for preamble, 2 for end flag
            if (bytes.ReadFlippedUShort(0) != 0xA55A || bytes.ReadFlippedUShort(2) != 0xA55A) { return null; }
            ushort endFlag = bytes.ReadFlippedUShort(length - 2);
            if (length == 138)
            {
                // check for command and checksum
                // if they don't make sense, check for a checksum at byte offset 130 =>
                // CMD_QUERY_SPECTRA and CMD_QUERY_SPECTRA_EX are special cases...
                ushort command = bytes.ReadFlippedUShort(106 + 4);
                ushort checksum = bytes.ReadUShort(126 + 4);
                // calculate the checksum, assume it is at position 126
                ushort calculatedChecksum = 0;
                for (int i = 2; i < 138; i += 2)
                {
                    if (i == 126 + 4) { i += 2; } // skip the actual checksum...
                    ushort value = bytes.ReadUShort(i);
                    calculatedChecksum += value;
                }
                if (Commands.ValidCommands.Contains((CommandName)command) == false ||
                    checksum != calculatedChecksum)
                {
                    // perhaps it is a special command?
                    // for now, just assume it is broken
                    // TODO: Add support for CMD_QUERY_SPECTRA and CMD_QUERY_SPECTRA_EX
                    return null;
                }

                switch ((CommandName)command)
                {
                    case CommandName.CMD_SET_MEASURE_PZC:
                        return new SetMeasurePZCResponse { Bytes = bytes };
                    case CommandName.CMD_QUERY_POWER:
                        return new QueryPowerResponse { Bytes = bytes };
                    case CommandName.CMD_QUERY_STATE:
                        return new QueryStateResponse { Bytes = bytes };
                    case CommandName.CMD_QUERY_STATE_527:
                        return new QueryState527Response { Bytes = bytes };
                    case CommandName.CMD_QUERY_STATE_527EX:
                        return new QueryState527ExResponse { Bytes = bytes };
                    case CommandName.CMD_QUERY_STATE_527EX2:
                        return new QueryState527Ex2Response { Bytes = bytes };
                    case CommandName.CMD_QUERY_USER_DATA:
                        return new QueryUserDataResponse { Bytes = bytes };
                    case CommandName.CMD_QUERY_ADJUSTMENT_TABLE:
                        return new QueryAdjustmentTableResponse { Bytes = bytes };
                    case CommandName.CMD_QUERY_SYSTEM_DATA:
                        return new QuerySystemDataResponse { Bytes = bytes };
                }
                return new MCAResponse { Bytes = bytes };
            }
            else if (endFlag != (ushort)EndFlagCode.Success)
            {
                // todo: use logger here Console.WriteLine("EndFlagCode: {0}", endFlag);
                return null;
            }
            else if (length == 1018 + 6) { return null; } // CMD_QUERY_OSCI_SCREEN
            else if (length == 1034 + 6) { return null; } // CMD_QUERY_HISTOGRAM, CMD_QUERY_DETECTOR_INFO
            else if (length == 1036 + 6) { return null; } // CMD_QUERY_EXTENSION_RS323_RX
            else if (length == 1050 + 6) { return null; } // CMD_QUERY_AHRC_HISTOGRAM
            else if (length == 1414 + 6) { return null; } // CMD_QUERY_OSCI_SCREEN_EX
            else if (length == 1450 + 6) { return new QueryCommonMemoryResponse { Bytes = bytes }; }
            else if (length == 1452 + 6) { return null; } // not sure about the 6, CMD_QUERY_OSCI_MAIN_FILTER_RESULTS
            else if (length == 1454 + 6) { return null; } // CMD_QUERY_OSCI_TRIGGER_FILTER_RESULTS
            else { return null; }
        }

        bool IsError { get { return EndFlagCode != EndFlagCode.Success; } }

		internal void Init(CommandName cmd) {
			Bytes = new byte[Commands.ExpectedByteCount[cmd]]; }
    }

    class SetMeasurePZCResponse : MCAResponse
    {
        public short NumberOfMeasuredPulses { get { return Bytes.ReadShort(124 + 4); } }
        public short AveragedNegativeOffsetOfMeasuredInputPulses { get { return Bytes.ReadShort(128 + 4); } }
    }

    enum MCAState : ushort
    {
        Ready = 1,
        Run = 2,
        Suspend = 3,
        Finish = 4,
        Stop = 5,
        Fail = 6,
        WaitForTrigger = 7
    }


    class QueryPowerResponse : MCAResponse
    {
        public uint BatteryCurrent { get { return Bytes.ReadUInt(0 + 4); } }
        public uint HVPrimaryCurrent { get { return Bytes.ReadUInt(4 + 4); } }
        public uint Plus12VPrimaryCurrent { get { return Bytes.ReadUInt(8 + 4); } }
        public uint Minus12VPrimaryCurrent { get { return Bytes.ReadUInt(12 + 4); } }
        public uint Plus24VPrimaryCurrent { get { return Bytes.ReadUInt(16 + 4); } }
        public uint Minus24VPrimaryCurrent { get { return Bytes.ReadUInt(20 + 4); } }
        public uint BatteryVoltage { get { return Bytes.ReadUInt(24 + 4); } }
        public uint HV { get { return Bytes.ReadUInt(28 + 4); } }
        // HV state : meaningless for MCA527
        public byte Plus12VActualValue { get { return Bytes.ReadByte(36 + 4); } }
        public byte Minus12VActualValue { get { return Bytes.ReadByte(37 + 4); } }
        public byte Plus24VActualValue { get { return Bytes.ReadByte(38 + 4); } }
        public byte Minus24VActualValue { get { return Bytes.ReadByte(39 + 4); } }
        public uint CurrentHighVoltage { get { return Bytes.ReadUInt(40 + 4); } }
        public ushort VoltageOnSubD9Pin3 { get { return Bytes.ReadUShort(44 + 4); } }
        public ushort VoltageOnSubD9Pin5 { get { return Bytes.ReadUShort(46 + 4); } }
        public uint PowerSwitches { get { return Bytes.ReadUInt(48 + 4); } }
        public uint ChargerCurrent { get { return Bytes.ReadUInt(52 + 4); } }
        public ushort CurrentSourceValueOnSubD9Pin5 { get { return Bytes.ReadUShort(56 + 4); } }
        public ushort CurrentSourceStateOnSubD9Pin5 { get { return Bytes.ReadUShort(58 + 4); } }
        public ushort InputResistanceOnSubD9Pin5 { get { return Bytes.ReadUShort(60 + 4); } }
        public sbyte ADCCorrectionOffsetOnSubD9Pin5 { get { return Bytes.ReadSByte(62 + 4); } }
        public sbyte GainCorrectionFactorOnSubD9Pin5 { get { return Bytes.ReadSByte(63 + 4); } }
        public uint BatteryCurrentAtStop { get { return Bytes.ReadUInt(64 + 4); } }
        public uint HVPrimaryCurrentAtStop { get { return Bytes.ReadUInt(68 + 4); } }
        public uint Plus12VPrimaryCurrentAtStop { get { return Bytes.ReadUInt(72 + 4); } }
        public uint Minus12VPrimaryCurrentAtStop { get { return Bytes.ReadUInt(76 + 4); } }
        public uint Plus24VPrimaryCurrentAtStop { get { return Bytes.ReadUInt(80 + 4); } }
        public uint Minus24VPrimaryCurrentAtStop { get { return Bytes.ReadUInt(84 + 4); } }
        public uint BatteryVoltageAtStop { get { return Bytes.ReadUInt(88 + 4); } }
        public uint HVAtStop { get { return Bytes.ReadUInt(92 + 4); } }
        public sbyte ADCCorrectionOffsetOnSubD9Pin3 { get { return Bytes.ReadSByte(96 + 4); } }
        public sbyte GainCorrectionFactorOnSubD9Pin3 { get { return Bytes.ReadSByte(97 + 4); } }
        public ushort HVControlVoltage { get { return Bytes.ReadUShort(98 + 4); } }
        public byte Plus12VActualValueAtStop { get { return Bytes.ReadByte(100 + 4); } }
        public byte Minus12VActualValueAtStop { get { return Bytes.ReadByte(101 + 4); } }
        public byte Plus24VActualValueAtStop { get { return Bytes.ReadByte(102 + 4); } }
        public byte Minus24VActualValueAtStop { get { return Bytes.ReadByte(103 + 4); } }
        public ushort VoltageOnSubD9Pin3AtStop { get { return Bytes.ReadUShort(104 + 4); } }
        public ushort VoltageOnSubD9Pin5AtStop { get { return Bytes.ReadUShort(114 + 4); } }
        public uint ChargerCurrentAtStop { get { return Bytes.ReadUInt(116 + 4); } }
        public byte PowerModuleDetectorInfoDACType { get { return Bytes.ReadByte(124 + 4); } }
        public byte PowerModuleFeatures { get { return Bytes.ReadByte(125 + 4); } }
        public MCAState MCAState { get { return (MCAState)(Bytes.ReadUShort(128 + 4)); } }
    }

    enum BufferState : ushort
    {
        Occupied = 0x2000,
        Overrun = 0x4000,
        Filled = 0x8000
    }

    class QuerySystemDataResponse : MCAResponse
    {
        public long DetectedCounts { get { return Bytes.Read48BitLong(10 + 4); } }
        public uint MMCAOnTime { get { return Bytes.ReadUInt(36 + 4); } }
        public uint RealTimeOfPreviousSweep { get { return Bytes.ReadUInt(40 + 4); } }
        public uint DeadTimeOfPreviousSweep { get { return Bytes.ReadUInt(44 + 4); } }
        public uint StartTimeOfPreviousSweep { get { return Bytes.ReadUInt(48 + 4); } }
        public uint FastDeadTimeOfPreviousSweep { get { return Bytes.ReadUInt(52 + 4); } }
        public uint ElapsedSweeps { get { return Bytes.ReadUInt(56 + 4); } }
        public uint BusyTimeOfPreviousSweep { get { return Bytes.ReadUInt(60 + 4); } }
        public ushort FractionalDigitOfTheRealTimeOfPreviousSweep { get { return Bytes.ReadUShort(64 + 4); } }
        public long DetectedCountsOfPreviousSweep { get { return Bytes.Read48BitLong(74 + 4); } }
        public uint CounterOfStabilizationSteps { get { return Bytes.ReadUInt(80 + 4); } }
        public int CurrentStabilizationOffset { get { return Bytes.ReadInt(84 + 4); } }
        public int MaximalNegativeStabilizationOffset { get { return Bytes.ReadInt(88 + 4); } }
        public int MaximalPositiveStabilizationOffset { get { return Bytes.ReadInt(92 + 4); } }
        public uint CounterOfReceivedCommands { get { return Bytes.ReadUInt(96 + 4); } }
        public uint CounterOfUnsuccessfulCommands { get { return Bytes.ReadUInt(100 + 4); } }
        public BufferState ReadOutBufferState { get { return (BufferState)(Bytes.ReadUShort(114 + 4)); } }
        public uint StabilizationAreaPreset { get { return Bytes.ReadUInt(116 + 4); } }
        public ushort StabilizationTimePreset { get { return Bytes.ReadUShort(120 + 4); } }
        public byte LowShapingTime { get { return Bytes.ReadByte(122 + 4); } }
        public byte HighShapingTime { get { return Bytes.ReadByte(123 + 4); } }
        public byte MinimumRecommendedCoreClock { get { return Bytes.ReadByte(124 + 4); } }
        public byte MaximumAllowedCoreClock { get { return Bytes.ReadByte(125 + 4); } }
        public MCAState MCAState { get { return (MCAState)(Bytes.ReadUShort(128 + 4)); } }
    }

    class QueryStateResponse : MCAResponse
    {
        public ushort MCAAcquireMode { get { return Bytes.ReadUShort(0 + 4); } }
        public ushort Preset { get { return Bytes.ReadUShort(2 + 4); } }
        public uint PresetValue { get { return Bytes.ReadUInt(4 + 4); } }
        public uint ElapsedPreset { get { return Bytes.ReadUInt(8 + 4); } }
        public ushort RepeatValue { get { return Bytes.ReadUShort(12 + 4); } }
        public ushort ElapsedSweeps { get { return Bytes.ReadUShort(14 + 4); } }
        public ushort MCSTimePerChannel { get { return Bytes.ReadUShort(16 + 4); } }
        public ushort ElapsedTimePerChannel { get { return Bytes.ReadUShort(18 + 4); } }
        public uint RealTime { get { return Bytes.ReadUInt(20 + 4); } }
        public uint MCACountsPerSecond { get { return Bytes.ReadUInt(24 + 4); } }
        public uint MCSCountsPerChannel { get { return Bytes.ReadUInt(24 + 4); } }
        public uint DeadTime { get { return Bytes.ReadUInt(28 + 4); } }
        public uint BusyTime { get { return Bytes.ReadUInt(32 + 4); } }
        public ushort MCAChannels { get { return Bytes.ReadUShort(36 + 4); } }
        public ushort Threshold { get { return Bytes.ReadUShort(38 + 4); } }
        public ushort LLD { get { return Bytes.ReadUShort(40 + 4); } }
        public ushort ULD { get { return Bytes.ReadUShort(42 + 4); } }
        public ushort ROIBegin { get { return Bytes.ReadUShort(44 + 4); } }
        public ushort ROIEnd { get { return Bytes.ReadUShort(46 + 4); } }
        public ushort AmplifierCourseGain { get { return Bytes.ReadUShort(48 + 4); } }
        public ushort AmplifierFineGain { get { return Bytes.ReadUShort(50 + 4); } }
        public ushort SlowDiscriminatorValue { get { return Bytes.ReadUShort(52 + 4); } }
        public ushort FastDiscriminatorValue { get { return Bytes.ReadUShort(54 + 4); } }
        public ushort HighVoltage { get { return Bytes.ReadUShort(56 + 4); } }
        public ushort HighVoltagePolarity { get { return Bytes.ReadUShort(58 + 4); } }
        public ushort PowerSwitches { get { return Bytes.ReadUShort(60 + 4); } }
        public ushort PZCValue { get { return Bytes.ReadUShort(62 + 4); } }
        public ushort TimeOffsetForPZCAndDTCLow { get { return Bytes.ReadUShort(64 + 4); } }
        public ushort TimeOffsetForPZCAndDTCHigh { get { return Bytes.ReadUShort(66 + 4); } }
        public ushort StabilizationStateOrChannel { get { return Bytes.ReadUShort(68 + 4); } }
        public ushort StabilizationResult { get { return Bytes.ReadUShort(70 + 4); } }
        public ushort StablilizationROIBegin { get { return Bytes.ReadUShort(72 + 4); } }
        public ushort StabilizationROIEnd { get { return Bytes.ReadUShort(74 + 4); } }
        public ushort ADCInput { get { return Bytes.ReadUShort(76 + 4); } }
        public ushort ADCInputPolarity { get { return Bytes.ReadUShort(78 + 4); } }
        public ushort ShapingTimeChoice { get { return Bytes.ReadUShort(80 + 4); } }
        public ushort PileUpRejectionState { get { return Bytes.ReadUShort(82 + 4); } }
        public ushort MCSInput { get { return Bytes.ReadUShort(84 + 4); } }
        public ushort MCASerialNumber { get { return Bytes.ReadUShort(86 + 4); } }
        public ushort MCAHardwareVersion { get { return Bytes.ReadUShort(88 + 4); } }
        public ushort MCAFirmwareVersion { get { return Bytes.ReadUShort(90 + 4); } }
        public ushort MCSChannels { get { return Bytes.ReadUShort(92 + 4); } }
        public ushort LastPowerState { get { return Bytes.ReadUShort(94 + 4); } }
        public ushort BatteryCapacity { get { return Bytes.ReadUShort(96 + 4); } }
        public ushort BatteryLifeTime { get { return Bytes.ReadUShort(98 + 4); } }
        public uint StartTime { get { return Bytes.ReadUInt(100 + 4); } }
        public ushort TDF { get { return Bytes.ReadUShort(104 + 4); } }
        public BufferState ReadOutBufferState { get { return (BufferState)Bytes.ReadUShort(114 + 4); } }
        public uint CountsPerSecond { get { return Bytes.ReadUInt(116 + 4); } }
        public ushort DifferentialDeadTime { get { return Bytes.ReadUShort(120 + 4); } }
        public short HVInhibitMode { get { return Bytes.ReadShort(122 + 4); } }
        public ushort HVInhibitState { get { return Bytes.ReadUShort(124 + 4); } }
        public MCAState MCAState { get { return (MCAState)(Bytes.ReadUShort(128 + 4)); } }
        public ushort StartFlag { get { return Bytes.ReadUShort(130 + 4); } }
    }

    enum ExecutionRight : short
    {
        NotGranted = -1,
        Reserved = 0,
        // Granted is anything else...
    };

    class QueryState527Response : MCAResponse
    {
        public ushort MCAHardwareVersion { get { return Bytes.ReadUShort(0 + 4); } }
        public ushort MCAFirmwareVersion { get { return Bytes.ReadUShort(2 + 4); } }
        public ushort MCAHardwareModification { get { return Bytes.ReadUShort(4 + 4); } }
        public ushort MCAFirmwareModification { get { return Bytes.ReadUShort(6 + 4); } }
        public uint MCAFeatures { get { return Bytes.ReadUInt(8 + 4); } }
        public uint TimeOnInternalClock { get { return Bytes.ReadUInt(12 + 4); } }
        public uint TestingPhase { get { return Bytes.ReadUInt(20 + 4); } }
        public short MCATemperature { get { return Bytes.ReadShort(24 + 4); } }
        public Mode GeneralMCAMode { get { return (Mode)Bytes.ReadUShort(26 + 4); } }
        public uint DiscardedCycles { get { return Bytes.ReadUInt(28 + 4); } }
        public ushort CoreClock { get { return Bytes.ReadUShort(32 + 4); } }
        public byte TriggerFilterForLowShapingTime { get { return Bytes.ReadByte(34 + 4); } }
        public byte TriggerFilterForHighShapingTime { get { return Bytes.ReadByte(35 + 4); } }
        public ushort ExpanderFlags { get { return Bytes.ReadUShort(36 + 4); } }
        public ushort OffsetDAC { get { return Bytes.ReadUShort(38 + 4); } }
        public short DetectorTemperature { get { return Bytes.ReadShort(40 + 4); } }
        public short PowerModuleTemperature { get { return Bytes.ReadShort(42 + 4); } }
        public ushort MCASerialNumber { get { return Bytes.ReadUShort(44 + 4); } }
        public bool AmIRightHolder { get { return Bytes.ReadShort(46 + 4) != 0; } }
        public IPAddress RightHolderIPAddress
        {
            get
            {
                byte[] bytes = new byte[4];
                bytes[0] = Bytes.ReadByte(48 + 4);
                bytes[1] = Bytes.ReadByte(49 + 4);
                bytes[2] = Bytes.ReadByte(50 + 4);
                bytes[3] = Bytes.ReadByte(51 + 4);
                return new IPAddress(bytes);
            }
        }
        public ushort RightHolderUDPPort { get { return Bytes.ReadUShort(52 + 4); } }
        public ExecutionRight ExecutionRight { get { return (ExecutionRight)(Bytes.ReadShort(54 + 4)); } }
        public ushort MaximumMCAChannels { get { return Bytes.ReadUShort(56 + 4); } }
        public byte PowerModuleFirmwareVersion { get { return Bytes.ReadByte(58 + 4); } }
        public byte PowerModuleHardwareVersion { get { return Bytes.ReadByte(59 + 4); } }
        public ushort PowerModuleSerialNumber { get { return Bytes.ReadUShort(60 + 4); } }
        public ushort PowerModuleID { get { return Bytes.ReadUShort(62 + 4); } }
        public ushort MaximumAllowedHighVoltage { get { return Bytes.ReadUShort(64 + 4); } }
        public ushort Threshold { get { return Bytes.ReadUShort(66 + 4); } }
        public uint FastDeadTime { get { return Bytes.ReadUInt(68 + 4); } }
        public ushort EvaluationFilterType { get { return Bytes.ReadUShort(72 + 4); } }
        public ushort FlattopTime { get { return Bytes.ReadUShort(74 + 4); } }
        public ushort EvaluationFilterSize { get { return Bytes.ReadUShort(76 + 4); } }
        public ushort TriggerLevelForAutomaticThresholdCalculation { get { return Bytes.ReadUShort(78 + 4); } }
        public short MCATemperatureAtStop { get { return Bytes.ReadShort(80 + 4); } }
        public short DetectorTemperatureAtStop { get { return Bytes.ReadShort(82 + 4); } }
        public uint CustomizedIPAddress { get { return Bytes.ReadUInt(84 + 4); } }
        public uint ActualIPAddress { get { return Bytes.ReadUInt(88 + 4); } }
        public uint MCSTimePerChannel { get { return Bytes.ReadUInt(92 + 4); } }
        public uint ElapsedTimePerChannel { get { return Bytes.ReadUInt(96 + 4); } }
        public int AutoTriggerThreshold { get { return Bytes.ReadInt(100 + 4); } }
        public short PowerModuleTemperatureAtStop { get { return Bytes.ReadShort(104 + 4); } }
        public byte JitterCorrection { get { return Bytes.ReadByte(114 + 4); } }
        public byte BaselineRestoring { get { return Bytes.ReadByte(115 + 4); } }
        public int SetTriggerThreshold { get { return Bytes.ReadInt(116 + 4); } }
        public byte InputMode { get { return Bytes.ReadByte(120 + 4); } }
        public byte HighestAllowedShapingTime { get { return Bytes.ReadByte(121 + 4); } }
        public byte GatingMode { get { return Bytes.ReadByte(122 + 4); } }
        public byte GatingSignal { get { return Bytes.ReadByte(123 + 4); } }
        public byte GatingShift { get { return Bytes.ReadByte(124 + 4); } }
        public byte HardwareBasedCoarseGrainLevels { get { return Bytes.ReadByte(125 + 4); } }
        public ushort DifferentialFastDeadTime { get { return Bytes.ReadUShort(130 + 4); } }
    }

    class QueryState527ExResponse : MCAResponse
    {
        public uint CommonMemorySize { get { return Bytes.ReadUInt(0 + 4); } }
        public uint CommonMemoryFillStop { get { return Bytes.ReadUInt(4 + 4); } }
        public uint CommonMemoryFillLevel { get { return Bytes.ReadUInt(8 + 4); } }
        public short OscilloscopeTimeResolution { get { return Bytes.ReadShort(12 + 4); } }
        public ushort OscilloscopeTriggerSource { get { return Bytes.ReadUShort(14 + 4); } }
        public ushort OscilloscopeTriggerPosition { get { return Bytes.ReadUShort(16 + 4); } }
        public ushort OscilloscopeTriggerThreshold { get { return Bytes.ReadUShort(18 + 4); } }
        public uint PURCounter { get { return Bytes.ReadUInt(20 + 4); } }
        public byte ExtensionPortPartAConfiguration { get { return Bytes.ReadByte(24 + 4); } }
        public byte ExtensionPortPartBConfiguration { get { return Bytes.ReadByte(25 + 4); } }
        public byte ExtensionPortPartCConfiguration { get { return Bytes.ReadByte(26 + 4); } }
        public byte ExtensionPortPartDConfiguration { get { return Bytes.ReadByte(27 + 4); } }
        public byte ExtensionPortPartEConfiguration { get { return Bytes.ReadByte(28 + 4); } }
        public byte ExtensionPortPartFConfiguration { get { return Bytes.ReadByte(29 + 4); } }
        public byte ExtensionPortPartsAvailability { get { return Bytes.ReadByte(30 + 4); } }
        public byte ExtensionPortStateFlags { get { return Bytes.ReadByte(31 + 4); } }
        public byte ExtensionPortPolarityFlags { get { return Bytes.ReadByte(32 + 4); } }
        public byte HighestAllowedFlattopTime { get { return Bytes.ReadByte(33 + 4); } }
        public ushort SizeOfTheBootingPresetsDataStructure { get { return Bytes.ReadUShort(34 + 4); } }
        public uint ExtensionPortPulser1Period { get { return Bytes.ReadUInt(36 + 4); } }
        public uint ExtensionPortPulser2Period { get { return Bytes.ReadUInt(40 + 4); } }
        public uint ExtensionPortPulser1Width { get { return Bytes.ReadUInt(44 + 4); } }
        public uint ExtensionPortPulser2Width { get { return Bytes.ReadUInt(48 + 4); } }
        public ushort ExtensionPortRS232BaudRate { get { return Bytes.ReadUShort(52 + 4); } }
        public ushort ExtensionPortRS232Flags { get { return Bytes.ReadUShort(54 + 4); } }
        public uint ExtensionPortCounter1 { get { return Bytes.ReadUInt(56 + 4); } }
        public uint ExtensionPortCounter1CPS { get { return Bytes.ReadUInt(60 + 4); } }
        public uint ExtensionPortCounter1OfPreviousSweep { get { return Bytes.ReadUInt(64 + 4); } }
        public uint ExtensionPortCounter2 { get { return Bytes.ReadUInt(68 + 4); } }
        public uint ExtensionPortCounter2CPS { get { return Bytes.ReadUInt(72 + 4); } }
        public uint ExtensionPortCounter2OfPreviousSweep { get { return Bytes.ReadUInt(76 + 4); } }
        public ushort RS232TransferBufferByteCount { get { return Bytes.ReadUShort(80 + 4); } }
        public ushort FractionalDigitsOfRealTime { get { return Bytes.ReadUShort(82 + 4); } }
        public uint PURCounterOfPreviousSweep { get { return Bytes.ReadUInt(84 + 4); } }
        public uint TriggerFilterAvailabilityFlags { get { return Bytes.ReadUInt(88 + 4); } }
        public short TriggerFilterValue1 { get { return Bytes.ReadShort(92 + 4); } }
        public short TriggerFilterValue2 { get { return Bytes.ReadShort(94 + 4); } }
        public byte TTLLowLevel { get { return Bytes.ReadByte(96 + 4); } }
        public byte TTLHighLevel { get { return Bytes.ReadByte(97 + 4); } }
        public ushort TriggerLevelForAutomaticThresholdCalculationForDirectInput { get { return Bytes.ReadUShort(98 + 4); } }
        public uint ADCOverflowsPerSecond { get { return Bytes.ReadUInt(100 + 4); } }
        public ushort ADCSamplingRate { get { return Bytes.ReadUShort(104 + 4); } }
        public ushort FileSizeCorrespondingToSetup { get { return Bytes.ReadUShort(114 + 4); } }
        public uint TotalMicroSDCardMemorySize { get { return Bytes.ReadUInt(116 + 4); } }
        public uint FreeMicroSDCardMemorySize { get { return Bytes.ReadUInt(120 + 4); } }
        public sbyte FileWritingState { get { return Bytes.ReadSByte(124 + 4); } }
        public sbyte ResultOfLastFileWriting { get { return Bytes.ReadSByte(125 + 4); } }
        public MCAState MCAState { get { return (MCAState)(Bytes.ReadShort(128 + 4)); } }
        public short RS485BusBaudRate { get { return Bytes.ReadShort(130 + 4); } }
    }

    class QueryState527Ex2Response : MCAResponse
    {
        public uint AHRCGroup0Width { get { return Bytes.ReadUInt(0 + 4); } }
        public uint AHRCGroup1Width { get { return Bytes.ReadUInt(4 + 4); } }
        public uint AHRCGroup2Width { get { return Bytes.ReadUInt(8 + 4); } }
        public uint AHRCGroup3Width { get { return Bytes.ReadUInt(12 + 4); } }
        public uint AHRCGroup4Width { get { return Bytes.ReadUInt(16 + 4); } }
        public uint AHRCGroup5Width { get { return Bytes.ReadUInt(20 + 4); } }
        public uint AHRCGroup6Width { get { return Bytes.ReadUInt(24 + 4); } }
        public uint AHRCGroup7Width { get { return Bytes.ReadUInt(28 + 4); } }
        public uint AHRCGroup8Width { get { return Bytes.ReadUInt(32 + 4); } }
        public uint AHRCGroup9Width { get { return Bytes.ReadUInt(36 + 4); } }
        public ushort AHRCTriggerThreshold { get { return Bytes.ReadUShort(40 + 4); } }
        public uint TimeWindow0WidthForGatingModeSortByTime { get { return Bytes.ReadUInt(44 + 4); } }
        public uint TimeWindow1WidthForGatingModeSortByTime { get { return Bytes.ReadUInt(48 + 4); } }
        public uint TimeWindow2WidthForGatingModeSortByTime { get { return Bytes.ReadUInt(52 + 4); } }
        public uint TimeWindow3WidthForGatingModeSortByTime { get { return Bytes.ReadUInt(56 + 4); } }
        public uint TimeWindow4WidthForGatingModeSortByTime { get { return Bytes.ReadUInt(60 + 4); } }
        public uint TimeWindow5WidthForGatingModeSortByTime { get { return Bytes.ReadUInt(64 + 4); } }
        public uint TimeWindow6WidthForGatingModeSortByTime { get { return Bytes.ReadUInt(68 + 4); } }
        public uint TimeWindow7WidthForGatingModeSortByTime { get { return Bytes.ReadUInt(72 + 4); } }
        public uint ExtensionPortPulser3Period { get { return Bytes.ReadUInt(76 + 4); } }
        public uint ExtensionPortPulser3Width { get { return Bytes.ReadUInt(80 + 4); } }
        public uint ExtensionPortCounter3 { get { return Bytes.ReadUInt(84 + 4); } }
        public uint ExtensionPortCounter3CPS { get { return Bytes.ReadUInt(88 + 4); } }
        public uint ExtensionPortCounter3OfPreviousSweep { get { return Bytes.ReadUInt(92 + 4); } }
        public short AdditionalTemperature1 { get { return Bytes.ReadShort(96 + 2); } }
        public short AdditionalTemperature1AtStop { get { return Bytes.ReadShort(98 + 2); } }
        public short AdditionalTemperature2 { get { return Bytes.ReadShort(100 + 2); } }
        public short AdditionalTemperature2AtStop { get { return Bytes.ReadShort(102 + 2); } }
        public MCAState MCAState { get { return (MCAState)(Bytes.ReadShort(128 + 4)); } }
    }

    class QueryUserDataResponse : MCAResponse
    {
        public uint[] Values
        {
            get
            {
                uint[] values = new uint[16];
                for (int i = 0; i < 16; i++)
                {
                    values[i] = Bytes.ReadUInt(i * 4 + 4);
                }
                return values;
            }
        }
    }

    class QueryCommonMemoryResponse : MCAResponse
    {
        public byte[] Data
        {
            get
            {
                byte[] data = new byte[1440];
                Array.Copy(Bytes, 0 + 4, data, 0, 1440);
                return data;
            }
        }
        public void CopyData(int sourceIndex, Array destinationArray, int destinationIndex, int length)
        {
            Array.Copy(Bytes, sourceIndex + 4, destinationArray, destinationIndex, length);
        }
        public override CommandName CommandName { get { return (CommandName)(Bytes.ReadFlippedUShort(1440 + 4)); } }
        public override ushort Checksum { get { return Bytes.ReadUShort(1448 + 4); } }
    }

    class QueryAdjustmentTableResponse : MCAResponse
    {
        public short[] Values
        {
            get
            {
                short[] values = new short[12];
                for (int i = 0; i < 12; i++)
                {
                    values[i] = Bytes.ReadShort(i * 2 + 4);
                }
                return values;
            }
        }
        public MCAState MCAState { get { return (MCAState)(Bytes.ReadUShort(128 + 4)); } }
    }

    public static class ByteArrayExtensions
    {
        public static ushort ReadUShort(this byte[] bytes, int index)
        {
            return (ushort)(bytes[index] | (bytes[index + 1] << 8));
        }
        public static ushort ReadFlippedUShort(this byte[] bytes, int index)
        {
            return (ushort)((bytes[index] << 8) | bytes[index + 1]);
        }
        public static short ReadShort(this byte[] bytes, int index)
        {
            return (short)(bytes[index] | (bytes[index + 1] << 8));
        }
        public static uint ReadUInt(this byte[] bytes, int index)
        {
            return (uint)(bytes[index] | (bytes[index + 1] << 8) | (bytes[index + 2] << 16) | (bytes[index + 3] << 24));
        }
        public static byte ReadByte(this byte[] bytes, int index)
        {
            return bytes[index];
        }
        public static sbyte ReadSByte(this byte[] bytes, int index)
        {
            return (sbyte)bytes[index];
        }
        public static int ReadInt(this byte[] bytes, int index)
        {
            return (int)(bytes[index] | (bytes[index + 1] << 8) | (bytes[index + 2] << 16) | (bytes[index + 3] << 24));
        }
        public static long Read48BitLong(this byte[] bytes, int index)
        {
            return ((long)bytes[index]) | (((long)bytes[index + 1]) << 8) | (((long)bytes[index + 2]) << 16) | (((long)bytes[index + 3]) << 24) | (((long)bytes[index + 4]) << 32) | (((long)bytes[index + 5]) << 40);
        }
    }

	
    public interface IMCADeviceCallbackObject
    {
        void BeginSweep(uint sweepNumber);
        void ReadTimestamps(uint sweepNumber, uint[] timestamps);
        void FinishedSweep(uint sweepNumber, double sweepDurationSeconds);
    }

    class MCADeviceLostConnectionException : Exception { }
    class MCADeviceBadDataException : Exception { }

    public class MCADeviceInfo
    {
        public IPAddress Address;
        public ushort HardwareVersion;
        public ushort FirmwareVersion;
        public ushort Serial;
        public bool Available;
    }

    public enum MCAHardwareVersion : ushort
    {
        Full = 0,
        Lite = 1,
        OEM = 2,
        Micro = 3
    };

    enum MCAFeatures : uint
    {
        Expander = 0x00000001,
        InternalTemperature = 0x00000002,
        ExternalTemperature = 0x00000004,
        MACAddress = 0x00000008,
        PowerModule = 0x00000010,
        MicroSDCard = 0x00000020,
        PowerModuleTemperature = 0x00000040,
        AnalogVoltages = 0x00000080,
        GatingInput = 0x00000100,
        ExtensionPort = 0x00000200,
        LFRejection = 0x00000400,
        JitterCorrection = 0x00000800,
        AdjustableTriggerFilter = 0x00001000,
        AdjustableBaselineRestorer = 0x00002000,
        AdjustableCoarseGain = 0x00004000,
        USBRS232 = 0x00008000,
        USBChargingDefaultOff = 0x00010000,
        NoOffsetDAC = 0x00020000,
        SDRAMOk = 0x00040000,
        TimestampRecorder = 0x00080000,
        Bluetooth = 0x00100000,
        GatingByTime = 0x00200000,
        BootPresets = 0x00400000,
        DetectorInfo = 0x00800000,
        DetectorInfoValid = 0x01000000,
        // AdditionalTemperature1          = 0x02000000, // not sure about this, not in header provided by GBS
        // AdditionalTemperature2          = 0x04000000, // not sure about this, not in header provided by GBS
        DebugInfo = 0x80000000
    };

    public enum TimestampMode : byte
    {
        LevelTriggered = 3,
        EdgeTriggered = 4,
        AnalogHighRateCounting = 5
    };

    public class MCADevice
    {

        public IMCADeviceCallbackObject CallbackObject;
		//public System.Threading.ManualResetEventSlim Writing;

        public MCAHardwareVersion HardwareModification { get; private set; }
        public bool HasPowerModule { get; private set; }
        public ushort MaximumHighVoltage { get; private set; }

        public TimeSpan Timeout
        {
            get { return Client.DefaultTimeout; }
            set { Client.DefaultTimeout = value; }
        }

        public uint RetryCount
        {
            get { return Client.DefaultRetryCount; }
            set { Client.DefaultRetryCount = value; }
        }

        public static MCADeviceInfo[] QueryDevices()
        {
            List<MCADeviceInfo> devicesList = new List<MCADeviceInfo>();
            MCAClient client = new MCAClient();
            const int retryCount = 3;
            for (int i = 0; i < retryCount; i++)
            {
                MCAResponse[] responses = client.SendBroadcast(MCACommand.QueryState527());
                if (responses.Length > 0)
                {
                    foreach (QueryState527Response response in responses)
                    {
                        devicesList.Add(new MCADeviceInfo
                        {
                            Address = response.RemoteAddress,
                            HardwareVersion = response.MCAHardwareVersion,
                            FirmwareVersion = response.MCAFirmwareVersion,
                            Serial = response.MCASerialNumber,
                            Available = (response.ExecutionRight == ExecutionRight.NotGranted || response.AmIRightHolder == true)
                        });
                    }
                    break;
                }
            }
            client.Close();
            MCADeviceInfo[] deviceInfos = new MCADeviceInfo[devicesList.Count];
            devicesList.CopyTo(deviceInfos);
            return deviceInfos;
        }

        public static MCADevice ConnectToDeviceAtAddress(IPAddress address)
        {
            MCADevice device = new MCADevice(address);
            return device;
        }

        internal MCAClient Client = new MCAClient();

        MCADevice(IPAddress address)
        {
            Client.BoundAddress = address;
			//Writing = new System.Threading.ManualResetEventSlim(true);
        }

        public void Close()
        {
            Client.Close();
        }


        // must call this at least once every 15 seconds or you will lose the connection!
        public void Heartbeat()
        {
            Client.Send(MCACommand.QueryState());
        }

        public void Initialize()
        {
            MCAResponse response = null;
            response = Client.Send(MCACommand.Init());
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            response = Client.Send(MCACommand.SetPreamplifierPower(PreamplifierPower.Minus12V |
                                                                    PreamplifierPower.Plus12V |
                                                                    PreamplifierPower.Minus24V |
                                                                    PreamplifierPower.Plus24V));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            response = Client.Send(MCACommand.SetGeneralMode(Mode.EdgeTriggeredTimestampRecorder));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            response = Client.Send(MCACommand.SetInputPolarity(InputPolarity.Positive));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            response = Client.Send(MCACommand.SetTriggerFilter(0, 0));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            response = Client.Send(MCACommand.SetMeasurePZC(0, PoleZeroCancellationMode.SetPZCOnly));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            response = Client.Send(MCACommand.SetGain(2, 10000));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            response = Client.Send(MCACommand.SetTriggerParam(TriggerParameter.TriggerLevelForAutomaticThresholdCalculationForDirectInput, 0x258000));
            if (response == null) { throw new MCADeviceLostConnectionException(); }

            // query information about availability of high voltage
            QueryState527Response state527Response = (QueryState527Response)Client.Send(MCACommand.QueryState527());
            if (state527Response == null) { throw new MCADeviceLostConnectionException(); }
            HardwareModification = (MCAHardwareVersion)state527Response.MCAHardwareModification;
            uint features = state527Response.MCAFeatures;
            if ((features & (uint)MCAFeatures.PowerModule) != 0)
            {
                HasPowerModule = true;
            }
            if (HasPowerModule)
            {
                MaximumHighVoltage = state527Response.MaximumAllowedHighVoltage;
            }
            SetHighVoltage(0, BiasInhibitInput.InhibitOff);
        }

        public void SetTimestampMode(TimestampMode mode)
        {
            MCAResponse response = Client.Send(MCACommand.SetGeneralMode((Mode)((uint)mode)));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
        }

        public TimestampMode GetTimestampMode()
        {
            QueryState527Response response = (QueryState527Response)Client.Send(MCACommand.QueryState527());
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            return (TimestampMode)response.GeneralMCAMode;
        }

        public void SetHighVoltage(ushort value, BiasInhibitInput inhibitSignal)
        {
            if (HasPowerModule == false) { return; }
            if (value > MaximumHighVoltage) { throw new ArgumentOutOfRangeException(); }
            MCAResponse response = Client.Send(MCACommand.SetBias(value, inhibitSignal));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
        }

        public uint GetHighVoltage()
        {
            QueryPowerResponse response = (QueryPowerResponse)Client.Send(MCACommand.QueryPower());
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            return response.CurrentHighVoltage;
        }

        static public DateTime MCA527EpochTime = new DateTime(1969, 12, 31, 16, 0, 0);

        public uint TransformRawData(byte[] rawBuffer, ref uint rawBufferIndex, ulong[] timestampsBuffer)
        {
			uint offset = 0;
			uint timestampIndex = 0;
            uint byteCount = 0;
			uint temp = 0;
			while (offset < rawBufferIndex) 
			{
				byteCount = ReadEncodedValue(rawBuffer, offset, out temp);
				timestampsBuffer[timestampIndex] = temp;
				if (byteCount == 0) { break; }
				timestampIndex += 1;
				offset += byteCount;
			}
            if (offset > rawBufferIndex) // need to undo one read...
			{
                offset -= byteCount;
                timestampIndex -= 1;
            }
			if (offset > 0 && rawBufferIndex > offset)
			{
				Array.Copy(rawBuffer, offset, rawBuffer, 0, rawBufferIndex - offset);
			}
       		rawBufferIndex -= offset;
			return timestampIndex;
        }


        static uint ReadEncodedValue(byte[] data, uint offset, out uint value)
        {
            int len = data.Length;
            if (offset < len)
            {
                byte b0 = data[offset];
                if (b0 < 0xC0)
                {
                    value = (uint)b0;
                    return 1;
                }
                else if (b0 < 0xF0)
                {
                    // two bytes
                    if (offset + 1 < len)
                    {
                        byte b1 = data[offset + 1];
                        value = ((((((uint)b0) & 0x3f) << 8) | ((uint)b1))) + 0xC0;
                        return 2;
                    }
                }
                else if (b0 < 0xFC)
                {
                    if (offset + 2 < len)
                    {
                        byte b1 = data[offset + 1];
                        byte b2 = data[offset + 2];
                        value = (((((uint)b0) & 0x0f) << 16) | (((uint)b1) << 8) | ((uint)b2)) + 0x30C0;
                        return 3;
                    }
                }
                else {
                    if (offset + 3 < len)
                    {
                        byte b1 = data[offset + 1];
                        byte b2 = data[offset + 2];
                        byte b3 = data[offset + 3];
                        value = (((((uint)b0) & 0x03) << 24) | (((uint)b1) << 16) | (((uint)b2) << 8) | ((uint)b3)) + 0x0C30C0;
                        return 4;
                    }
                }
            }
            value = 0;
            return 0;
        }

        // Start
        // QueryCommonMemory

    }

    enum CommandName : ushort
    {
        CMD_INIT = 0x4100,
        CMD_CLEAR = 0x4400,
        // CMD_SAVE_MCA_STATE = 0x3601,
        CMD_START = 0x4200,
        // CMD_STOP = 0x4300,
        CMD_SET_GENERAL_MODE = 0x0501,
        // CMD_SET_MODE = 0x4500,
        // CMD_SET_ADC_RES_DISCR = 0x4600,
        CMD_SET_PRESETS = 0x4800,
        // CMD_SET_ROI = 0x49900
        CMD_SET_REPEAT = 0x4A00,
        // CMD_SET_MCS_CHANNEL = 0x6300,
        // CMD_SET_TIME_PER_CHANNEL = 0x4B00,
        // CMD_SET_TIME_PER_CHANNEL527 = 0x1501,
        CMD_SET_GAIN = 0x4C00,
        // CMD_SET_OFFSET_DAC = 0x0A01,
        CMD_SET_INPUT_POLARITY = 0x5600,
        // CMD_SET_MCA_INPUT = 0x5400,
        // CMD_SET_MCS_INPUT = 0x5500,
        // CMD_SET_THRESHOLD = 0x4700,
        // CMD_SET_THRESHOLD_TENTHS = 0x0D01,
        // CMD_SET_SHAPING_TIME = 0x5200,
        // CMD_SET_SHAPING_TIME_PAIR = 0x0C01,
        CMD_SET_TRIGGER_FILTER = 0x0301,
        CMD_SET_TRIGGER_PARAM = 0x0601,
        // CMD_SET_EVAL_FILTER_TYPE = 0x1401,
        // CMD_SET_FLAT_TOP_TIME = 0x1301,
        // CMD_SET_JITTER_CORRECTION = 0x1601,
        // CMD_SET_BASELINE_RESTORING = 0x1801,
        // CMD_SET_PUR = 0x5300,
        // CMD_SET_FAST = 0x5000,
        // CMD_SET_SLOW = 0x5100,
        CMD_SET_MEASURE_PZC = 0x5800, // Takes at least 800 ms to return
                                      // CMD_SET_PZC_TIME_OFFSET = 0x6000,
                                      // CMD_SET_GATING = 0x0F01,
                                      // CMD_SET_GATING_TIME_WINDOW_WIDTH = 0x3201,
                                      // CMD_SET_STABILISATION = 0x4D00,
                                      // CMD_SET_STAB_PARAM = 0x6700,
        CMD_SET_PREAMPLIFIER_POWER = 0x4E00,
        CMD_SET_BIAS = 0x4F00,
        // CMD_SET_PIN5_CURRENT_SOURCE = 0x1901,
        // CMD_SET_TDF = 0x6100,
        // CMD_SET_UF6_ROIS = 0x6400,
        CMD_SET_USER_DATA = 0x5700,
        // CMD_SET_TIME = 0x0401,
        // CMD_SET_IP_ADDRESS = 0x0B01,
        // CMD_SET_COMMON_MEMORY_FILL_STOP = 0x1701,
        // CMD_SET_TTL_LEVELS = 0x2701,
        // CMD_SET_OSCI_TRIGGER = 0x1101,
        // CMD_SET_CORE_CLOCK = 0x2D01,
        // CMD_SET_AHRC_PARAM = 0x2D01,
        // CMD_SET_EXTENSION_PORT = 0x1A01,
        // CMD_SET_EXTENSION_POLARITY = 0x1B01,
        // CMD_SET_EXTENSION_PULSER_PERIOD = 0x1C01,
        // CMD_SET_EXTENSION_PULSER_WIDTH = 0x1D01,
        // CMD_SET_EXTENSION_RS232 = 0x1E01,
        // CMD_CLEAR_EXTENSION_RS232 = 0x1F01,
        // CMD_WRITE_EXTENSION_RS232_TX_ASCII = 0x2001,
        // CMD_WRITE_EXTENSION_RS232_TX_BINARY = 0x2101,
        // CMD_START_EXTENSION_PULSER = 0x2201,
        // CMD_STOP_EXTENSION_PULSER = 0x2301,
        // CMD_SET_EXTENSION_OUTPUT = 0x2401,
        // CMD_WRITE_FILE = 0x2801,
        CMD_QUERY_POWER = 0x5900,
        CMD_QUERY_SYSTEM_DATA = 0x6200,
        CMD_QUERY_STATE = 0x5A00,
        CMD_QUERY_STATE_527 = 0x0101,
        CMD_QUERY_STATE_527EX = 0x1001,
        CMD_QUERY_STATE_527EX2 = 0x2F01,
        CMD_QUERY_USER_DATA = 0x5E00,
        // CMD_QUERY_SPECTRA = 0x5B00,
        // CMD_QUERY_SPECTRA_EX = 0x0201,
        // CMD_QUERY_EXTENSION_RS232_RX = 0x2501,
        // CMD_QUERY_HISTOGRAM = 0x0901,
        // CMD_QUERY_VOLTAGE_CURRENT = 0x0500,
        // CMD_QUERY_CENTROID = 0x5F00,
        // CMD_QUERY_ENRICHMENT = 0x5D00,
        // CMD_QUERY_UF6_ROIS = 0x6500,
        // CMD_QUERY_UF6_INFO = 0x6600,
        CMD_QUERY_COMMON_MEMORY = 0x0701,
        // CMD_QUERY_OSCI_SCREEN = 0x1201,
        // CMD_QUERY_OSCI_SCREEN_EX = 0x2901,
        // CMD_QUERY_OSCI_TRIGGER_FILTER_RESULTS = 0x2A01,
        // CMD_QUERY_OSCI_MAIN_FILTER_RESULTS = 0x2B01,
        // CMD_QUERY_AHRC_HISTOGRAM = 0x2B01,
        // CMD_QUERY_DETECTOR_INFO = 0x3301,
        CMD_QUERY_ADJUSTMENT_TABLE = 0x2601,
    };

    


    static class Commands
    {
        static Commands()
        {
            ValidCommands = new HashSet<CommandName>();
            ExpectedByteCount = new Dictionary<CommandName, ushort>();
            foreach (CommandName command in Enum.GetValues(typeof(CommandName)))
            {
                ValidCommands.Add(command);
                switch (command)
                {
                    case CommandName.CMD_SET_MEASURE_PZC:
                        ExpectedByteCount.Add(command, 128 + 4);
					break;
                    case CommandName.CMD_QUERY_POWER:
                        ExpectedByteCount.Add(command, 128 + 4);
					break;
                    case CommandName.CMD_QUERY_STATE:
                         ExpectedByteCount.Add(command, 130 + 4);
					break;
                    case CommandName.CMD_QUERY_STATE_527:
                         ExpectedByteCount.Add(command, 130 + 4);
					break;
                    case CommandName.CMD_QUERY_STATE_527EX:
                         ExpectedByteCount.Add(command, 130 + 4);
					break;
                    case CommandName.CMD_QUERY_STATE_527EX2:
                         ExpectedByteCount.Add(command, 128 + 4);
					break;
                    case CommandName.CMD_QUERY_USER_DATA:
                          ExpectedByteCount.Add(command, 64);
					break;
                    case CommandName.CMD_QUERY_ADJUSTMENT_TABLE:
                         ExpectedByteCount.Add(command, 24);
					break;
                    case CommandName.CMD_QUERY_SYSTEM_DATA:
                        ExpectedByteCount.Add(command, 128 + 4);
					break;
					default:
                        ExpectedByteCount.Add(command, 64);
					break;
                }
			}
        }
        public static HashSet<CommandName> ValidCommands;
		public static Dictionary<CommandName, ushort> ExpectedByteCount;

    }

    enum StartFlag : ushort
    {
        Unchanged = 0,
        SpectrumClearedNewStartTime = 1,
        RepeatMode1 = 1 << 2,
        RepeatMode2 = 1 << 3,
        RepeatMode3 = 1 << 4,
        RepeatMode4 = 1 << 5,
        RepeatMode5 = 1 << 6,
        RepeatMode6 = 1 << 7,
        RepeatMode7 = 1 << 8
    }

    [Flags()]
    enum PreamplifierPower : byte
    {
        Minus24V = 0x80,
        Plus24V = 0x40,
        Minus12V = 0x20,
        Plus12V = 0x10
    };
    enum Mode : byte
    {
        MCA = 0,
        TransientRecorder = 1,
        Oscilliscope = 2,
        LevelTriggeredTimestampRecorder = 3,
        EdgeTriggeredTimestampRecorder = 4,
        AHRCTimeStampRecorder = 5
    };
    enum Presets : byte
    {
        None = 0,
        Real = 1,
        Live = 2,
        Int = 3,
        Area = 4,
        RealMilliseconds = 5
    };
    enum InputPolarity : byte
    {
        Positive = 0,
        Negative = 1
    };
    enum PoleZeroCancellationMode : byte
    {
        SetPZCOnly = 0,
        SetPZCAndMeasurePZCOffset = 1
    }
    enum TriggerParameter : byte
    {
        TriggerLevelForAutomaticThresholdCalculation = 0,
        TriggerLevelForAutomaticThresholdCalculationForDirectInput = 1,
        SetTriggerThreshold = 2
    }
    enum ClearMode : byte
    {
        ClearMeasurementData0 = 0,
        ClearMeasurementData1 = 1,
        ClearROI = 2,
        ClearAll = 3
    }
    public enum BiasInhibitInput : int
    {
        InhibitOff = 0,
        CanberraHPGeMode = 1,
        DSGHPGeMode = 2,
        OrtecHPGeMode = -1
    }

    internal class MCACommand
    {
        public readonly byte[] Bytes;
        MCACommand(CommandName command)
        {
            Bytes = new byte[] {
                0xA5, 0x5A,
                (byte)(((ushort)command >> 8) & 0xff), (byte)((ushort)command & 0xff),
                0x00, 0x00,
                0x00, 0x00, 0x00, 0x00,
                0xB9, 0x9B
            };
        }
        MCACommand(CommandName command, ushort param1)
        {
            Bytes = new byte[] {
                0xA5, 0x5A,
                (byte)(((ushort)command >> 8) & 0xff), (byte)((ushort)command & 0xff),
                (byte)(param1 & 0xff), (byte)((param1 >> 8) & 0xff),
                0x00, 0x00, 0x00, 0x00,
                0xB9, 0x9B
            };
        }
        MCACommand(CommandName command, uint param1)
        {
            Bytes = new byte[] {
                0xA5, 0x5A,
                (byte)(((ushort)command >> 8) & 0xff), (byte)((ushort)command & 0xff),
                (byte)(param1 & 0xff), (byte)((param1 >> 8) & 0xff),
                (byte)((param1 >> 16) & 0xff), (byte)((param1 >> 24) & 0xff),
                0x00, 0x00,
                0xB9, 0x9B
            };
        }
        MCACommand(CommandName command, byte param1, byte param2)
        {
            Bytes = new byte[] {
                0xA5, 0x5A,
                (byte)(((ushort)command >> 8) & 0xff), (byte)((ushort)command & 0xff),
                param1, 0x00,
                param2, 0x00, 0x00, 0x00,
                0xB9, 0x9B
            };
        }
        MCACommand(CommandName command, ushort param1, ushort param2)
        {
            Bytes = new byte[] {
                0xA5, 0x5A,
                (byte)(((ushort)command >> 8) & 0xff), (byte)((ushort)command & 0xff),
                (byte)(param1 & 0xff), (byte)((param1 >> 8) & 0xff),
                (byte)(param2 & 0xff), (byte)((param2 >> 8) & 0xff),
                0x00, 0x00,
                0xB9, 0x9B
            };
        }
        MCACommand(CommandName command, byte param1, uint param2)
        {
            Bytes = new byte[] {
                0xA5, 0x5A,
                (byte)(((ushort)command >> 8) & 0xff), (byte)((ushort)command & 0xff),
                param1, 0x00,
                (byte)(param2 & 0xff), (byte)((param2 >> 8) & 0xff),
                (byte)((param2 >> 16) & 0xff), (byte)((param2 >> 24) & 0xff),
                0xB9, 0x9B
            };
        }
        MCACommand(CommandName command, ushort param1, uint param2)
        {
            Bytes = new byte[] {
                0xA5, 0x5A,
                (byte)(((ushort)command >> 8) & 0xff), (byte)((ushort)command & 0xff),
                (byte)(param1 & 0xff), (byte)((param1 >> 8) & 0xff),
                (byte)(param2 & 0xff), (byte)((param2 >> 8) & 0xff),
                (byte)((param2 >> 16) & 0xff), (byte)((param2 >> 24) & 0xff),
                0xB9, 0x9B
            };
        }

        public CommandName CommandName
        {
            get { return (CommandName)(Bytes.ReadFlippedUShort(2)); }
        }

        public override string ToString()
        {
            return string.Format("<Command: {0} : {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2} {9:X2} {10:X2} {11:X2} {12:X2}>", CommandName, Bytes[0], Bytes[1], Bytes[2], Bytes[3], Bytes[4], Bytes[5], Bytes[6], Bytes[7], Bytes[8], Bytes[9], Bytes[10], Bytes[11]);
        }

        public static MCACommand QueryState()
        {
            return new MCACommand(CommandName.CMD_QUERY_STATE);
        }
        public static MCACommand QueryState527()
        {
            return new MCACommand(CommandName.CMD_QUERY_STATE_527);
        }
        public static MCACommand QueryState527Ex()
        {
            return new MCACommand(CommandName.CMD_QUERY_STATE_527EX);
        }
        public static MCACommand QueryState527Ex2()
        {
            return new MCACommand(CommandName.CMD_QUERY_STATE_527EX2);
        }
        public static MCACommand QueryPower()
        {
            return new MCACommand(CommandName.CMD_QUERY_POWER);
        }
        public static MCACommand QuerySystemData()
        {
            return new MCACommand(CommandName.CMD_QUERY_SYSTEM_DATA);
        }
        public static MCACommand QueryUserData(byte address)
        {
            return new MCACommand(CommandName.CMD_QUERY_USER_DATA, address);
        }
        public static MCACommand QueryAdjustmentTable()
        {
            return new MCACommand(CommandName.CMD_QUERY_ADJUSTMENT_TABLE);
        }
        public static MCACommand Init()
        {
            return new MCACommand(CommandName.CMD_INIT);
        }
        public static MCACommand SetPreamplifierPower(PreamplifierPower power)
        {
            byte pp = (byte)((byte)power & 0xff);
            return new MCACommand(CommandName.CMD_SET_PREAMPLIFIER_POWER, pp);
        }
        public static MCACommand SetGeneralMode(Mode mode)
        {
            return new MCACommand(CommandName.CMD_SET_GENERAL_MODE, (byte)mode);
        }
        public static MCACommand SetPresets(Presets presets, uint value)
        {
            return new MCACommand(CommandName.CMD_SET_PRESETS, (byte)presets, value);
        }
        public static MCACommand SetInputPolarity(InputPolarity inputPolarity)
        {
            return new MCACommand(CommandName.CMD_SET_INPUT_POLARITY, (byte)inputPolarity);
        }
        public static MCACommand SetUserData(byte address, uint value)
        {
            return new MCACommand(CommandName.CMD_SET_USER_DATA, address, value);
        }
        // not sure what these mean, tfl and tfl must be between 0 and 4
        public static MCACommand SetTriggerFilter(byte triggerFilterLow, byte triggerFilterHigh)
        {
            byte tfl = triggerFilterLow > 4 ? (byte)4 : triggerFilterLow;
            byte tfh = triggerFilterHigh > 4 ? (byte)4 : triggerFilterHigh;
            return new MCACommand(CommandName.CMD_SET_TRIGGER_FILTER, tfl, tfh);
        }
        public static MCACommand SetMeasurePZC(ushort controlVoltage, PoleZeroCancellationMode mode)
        {
            return new MCACommand(CommandName.CMD_SET_MEASURE_PZC, (ushort)mode, controlVoltage);
        }
        // cg: 2, 5, 10, 20, 50, 100, 200, 500 or 1000?
        // fg: 5000 ... 65000?
        public static MCACommand SetGain(ushort coarseGain, ushort fineGain)
        {
            return new MCACommand(CommandName.CMD_SET_GAIN, coarseGain, fineGain);
        }
        public static MCACommand SetTriggerParam(TriggerParameter triggerParameter, uint value)
        {
            return new MCACommand(CommandName.CMD_SET_TRIGGER_PARAM, (byte)triggerParameter, value);
        }
        public static MCACommand Clear(ClearMode mode)
        {
            return new MCACommand(CommandName.CMD_CLEAR, (byte)mode);
        }
        public static MCACommand SetBias(ushort highVoltage, BiasInhibitInput inhibitInput)
        {
            return new MCACommand(CommandName.CMD_SET_BIAS, highVoltage, (uint)inhibitInput);
        }
        public static MCACommand SetRepeat(ushort repetitions)
        {
            return new MCACommand(CommandName.CMD_SET_REPEAT, repetitions);
        }
        public static MCACommand Start(StartFlag startFlags, bool trigger1Active, bool trigger2Active,
                                    bool trigger3Active, uint startTime)
        {
            // bit 15 : trigger 1 => 0b1000 0000 0000 0000 => 0x8000
            // bit 14 : trigger 2 => 0x4000
            // bit 13 : trigger 3 => 0x2000
            ushort flags = (ushort)((ushort)startFlags | (trigger1Active ? 0x8000 : 0x0000) |
                (trigger2Active ? 0x4000 : 0x0000) | (trigger3Active ? 0x20000 : 0x0000));
            return new MCACommand(CommandName.CMD_START, flags, startTime);
        }
        public static MCACommand QueryCommonMemory(uint offset)
        {
            return new MCACommand(CommandName.CMD_QUERY_COMMON_MEMORY, offset);
        }

    }

    internal class MCAClient : IDisposable
    {
        const int MCA_PORT_NUMBER = 50000;

        UdpClient Udp;
        int Port;
        IPEndPoint SendIP;

        public TimeSpan DefaultTimeout = new TimeSpan(0, 0, 1);
        public uint DefaultRetryCount = 3;

        public MCAClient()
        {
            // TODO: catch case where client cannot be created because port is not available!
            //Console.WriteLine("Creating UDP port");
            Udp = new UdpClient(0);
            Port = ((IPEndPoint)Udp.Client.LocalEndPoint).Port;
            Udp.EnableBroadcast = true;
        }

        static List<IPEndPoint> GetBroadcastSendIPs()
        {
            // We want to broadcast on all interfaces. Using IPAddress.Broadcast doesn't work sometimes
            // when there is a network device (router) between us and the MCA device
            // So we iterate over all network interfaces.
            List<IPEndPoint> BroadcastSendIPs = null;
            if (BroadcastSendIPs == null)
            {
                BroadcastSendIPs = new List<IPEndPoint>();
                foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback) { continue; }
                    if (networkInterface.OperationalStatus != OperationalStatus.Up) { continue; }
                    foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        // the MCA 527 does IPv4 only
                        if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            byte[] maskBytes = unicastAddress.IPv4Mask.GetAddressBytes();
                            byte[] addressBytes = unicastAddress.Address.GetAddressBytes();
                            byte[] broadcastBytes = new byte[4];
                            for (int i = 0; i < 4; i++)
                            {
                                // invert mask bytes, OR with address
                                broadcastBytes[i] = (byte)((maskBytes[i] ^ 0xff) | addressBytes[i]);
                            }
                            BroadcastSendIPs.Add(new IPEndPoint(new IPAddress(broadcastBytes), MCA_PORT_NUMBER));
                        }
                    }
                }
            }
            BroadcastSendIPs.Add(new IPEndPoint(IPAddress.Broadcast, MCA_PORT_NUMBER));
            return BroadcastSendIPs;
        }

        public void Close()
        {
            Udp.Close();
        }

        IPAddress _BoundAddress = null;
        public IPAddress BoundAddress
        {
            get { return _BoundAddress; }
            set
            {
                _BoundAddress = value;
                if (_BoundAddress == null)
                {
                    SendIP = null;
                }
                else {
                    SendIP = new IPEndPoint(_BoundAddress, MCA_PORT_NUMBER);
                }
            }
        }

        public MCAResponse Send(MCACommand command)
        {
            return Send(command, DefaultTimeout);
        }

        public MCAResponse Send(MCACommand command, TimeSpan timeout)
        {
            return Send(command, timeout, DefaultRetryCount);
        }

        public MCAResponse Send(MCACommand command, TimeSpan timeout, uint retryCount)
        {
            Udp.Client.SendTimeout = (int)Math.Round(timeout.TotalMilliseconds);
            Udp.Client.ReceiveTimeout = (int)Math.Round(timeout.TotalMilliseconds);

            IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
            MCAResponse response = null;
            for (int i = 0; i < retryCount; i += 1)
            {
                Udp.Send(command.Bytes, command.Bytes.Length, SendIP);
                byte[] receivedBytes = null;
                try
                {
                    receivedBytes = Udp.Receive(ref from);
                }
                catch { }
                if (receivedBytes != null)
                {
                    response = MCAResponse.ParseResponse(receivedBytes);
                    if (response != null && response.CommandName == command.CommandName)
                    {
                        response.RemoteAddress = from.Address;
                        break;
                    }
                }
            }
            return response;
        }

        public MCAResponse[] SendBroadcast(MCACommand command)
        {
            return SendBroadcast(command, new TimeSpan(0, 0, 1));
        }

        public MCAResponse[] SendBroadcast(MCACommand command, TimeSpan timeout)
        {
            Udp.Client.SendTimeout = (int)Math.Round(timeout.TotalMilliseconds);

            foreach (IPEndPoint sendIP in GetBroadcastSendIPs())
            {
                Udp.Send(command.Bytes, command.Bytes.Length, sendIP);
            }

            List<MCAResponse> responseList = new List<MCAResponse>();
            IPEndPoint from = new IPEndPoint(IPAddress.Any, 0);
            DateTime endTime = DateTime.UtcNow + timeout;
            while (timeout > TimeSpan.Zero)
            {
                Udp.Client.ReceiveTimeout = (int)Math.Round(timeout.TotalMilliseconds);
                byte[] receivedBytes = null;
                try
                {
                    receivedBytes = Udp.Receive(ref from);
                }
                catch { } // SocketException for example upon a timeout
                if (receivedBytes != null)
                {
                    MCAResponse response = MCAResponse.ParseResponse(receivedBytes);
                    if (response != null && response.CommandName == command.CommandName)
                    {
                        response.RemoteAddress = from.Address;
                        responseList.Add(response);
                    }
                }
                timeout = endTime - DateTime.UtcNow;
            }

            MCAResponse[] responses = new MCAResponse[responseList.Count];
            responseList.CopyTo(responses);
            return responses;
        }

        public void Dispose()
        {
            ((IDisposable)Udp).Dispose();
        }
    }

    internal static class MCAFileExtensions
    {

        internal static void CreateWriteHeaderAndClose(this MCADevice device, NCCFile.MCAFile file)
        {
            //file.Log.TraceEvent(NCCReporter.LogLevels.Verbose, 120, "CreateWriteHeaderAndClose entry");
            MCAResponse qsdr = device.Client.Send(MCACommand.QuerySystemData());
            if (qsdr == null) { throw new MCADeviceLostConnectionException(); }
            if (file.stream == null)
            {
                //file.Log.TraceEvent(NCCReporter.LogLevels.Verbose, 118, "CreateWriteHeaderAndClose reopen");
                // reopen the closed file for this final operation
                file.OpenForWriting();
            }
            if (file.stream != null)
            {
					QueryStateResponse stateResponse = null;
					QueryState527ExResponse state527ExResponse = null;
					QueryState527Response state527Response = null;  
					QueryPowerResponse powerResponse = null;
					QueryState527Ex2Response state527Ex2Response = null;
				try
				{
					stateResponse = (QueryStateResponse)device.Client.Send(MCACommand.QueryState());
					if (stateResponse == null)
					{ throw new MCADeviceLostConnectionException(); }
					state527ExResponse = (QueryState527ExResponse)device.Client.Send(MCACommand.QueryState527Ex());
					if (stateResponse == null)
					{ throw new MCADeviceLostConnectionException(); }
					state527Response = (QueryState527Response)device.Client.Send(MCACommand.QueryState527());
					if (stateResponse == null)
					{ throw new MCADeviceLostConnectionException(); }
					powerResponse = (QueryPowerResponse)device.Client.Send(MCACommand.QueryPower());
					if (stateResponse == null)
					{ throw new MCADeviceLostConnectionException(); }
					state527Ex2Response = (QueryState527Ex2Response)device.Client.Send(MCACommand.QueryState527Ex2());
					if (stateResponse == null)
					{ throw new MCADeviceLostConnectionException(); }
				}
				catch (MCADeviceLostConnectionException)
				{
					file.Log.TraceEvent(NCCReporter.LogLevels.Warning, 118, "Device connection lost during query");
				} 
				catch (Exception ex)
				{
					file.Log.TraceEvent(NCCReporter.LogLevels.Warning, 118, ex.Message);
				} 
				finally
				{
					file.CreatePrimaryHeader(state527Response, stateResponse);
					file.CreateModeHeader(stateResponse, state527ExResponse, state527Response, powerResponse, state527Ex2Response);
				}
                file.rheader.DataCodingMethod = 0;
                file.WriteHeader();
                file.CloseWriter();
            }
        }

        internal static void CreateModeHeader(this NCCFile.MCAFile file,
            QueryStateResponse stateResponse,
            QueryState527ExResponse state527ExResponse,
            QueryState527Response state527Response,
            QueryPowerResponse powerResponse,
            QueryState527Ex2Response state527Ex2Response
        )
        {
			if (stateResponse == null) { stateResponse = new QueryStateResponse(); stateResponse.Init(CommandName.CMD_QUERY_STATE); } // dummy values 
			if (state527Response == null) { state527Response = new QueryState527Response(); state527Response.Init(CommandName.CMD_QUERY_STATE_527); } // dummy values
			if (state527ExResponse == null) { state527ExResponse = new QueryState527ExResponse(); state527ExResponse.Init(CommandName.CMD_QUERY_STATE_527EX); state527ExResponse.Bytes[105] = 1;} // dummy values 
			if (state527Ex2Response == null) { state527Ex2Response = new QueryState527Ex2Response(); state527Ex2Response.Init(CommandName.CMD_QUERY_STATE_527EX2); } // dummy values
			if (powerResponse == null) { powerResponse = new QueryPowerResponse(); powerResponse.Init(CommandName.CMD_QUERY_POWER); } // dummy values

            file.rheader = new NCCFile.MCAFile.MCATimestampsRecorderModeHeader
            {
                // sampling rate: kHz : 1000
                // time unit length: ns : 1000000000
                // 1000000000 / (sampling_rate / 1000)
                // 1000000 / sampling_rate
                TimeUnitLengthNanoSec = (ushort)(1000000 / state527ExResponse.ADCSamplingRate),
                Preset = stateResponse.Preset,
                PresetValue = stateResponse.PresetValue,
                PresetMemorySize = state527ExResponse.CommonMemoryFillStop,
                UsedMemorySize = state527ExResponse.CommonMemoryFillLevel,
                HighVoltage = stateResponse.HighVoltage,
                HighVoltagePolarity = stateResponse.HighVoltagePolarity,
                HVInhibitMode = stateResponse.HVInhibitMode,
                PreamplifierPowerSwitches = stateResponse.PowerSwitches,
                TTLLowLevel = state527ExResponse.TTLLowLevel,
                TTLHighLevel = state527ExResponse.TTLHighLevel,
                AmplifierCoarseGain = stateResponse.AmplifierCourseGain,
                ADCInputPolarity = stateResponse.ADCInputPolarity,
                ShapingTimeChoice = stateResponse.ShapingTimeChoice,
                TriggerFilterForLowShapingTime = state527Response.TriggerFilterForLowShapingTime,
                TriggerFilterForHighShapingTime = state527Response.TriggerFilterForHighShapingTime,
                OffsetDAC = state527Response.OffsetDAC,
                TriggerLevelForAutomaticThresholdCalculation = state527Response.TriggerLevelForAutomaticThresholdCalculation,
                SetTriggerThreshold = state527Response.SetTriggerThreshold,
                ExtensionPortPartAConfiguration = state527ExResponse.ExtensionPortPartAConfiguration,
                ExtensionPortPartBConfiguration = state527ExResponse.ExtensionPortPartBConfiguration,
                ExtensionPortPartCConfiguration = state527ExResponse.ExtensionPortPartCConfiguration,
                ExtensionPortPartFConfiguration = state527ExResponse.ExtensionPortPartFConfiguration,
                ExtensionPortRS232BaudRate = state527ExResponse.ExtensionPortRS232BaudRate,
                ExtensionPortRS232Flags = state527ExResponse.ExtensionPortRS232Flags,
                StartFlag = stateResponse.StartFlag,
                StartTime = stateResponse.StartTime,
                RealTime = stateResponse.RealTime,
                BatteryCurrentAtStop = powerResponse.BatteryCurrentAtStop,
                ChargerCurrentAtStop = powerResponse.ChargerCurrentAtStop,
                HVPrimaryCurrentAtStop = powerResponse.HVPrimaryCurrentAtStop,
                Plus12VPrimaryCurrentAtStop = powerResponse.Plus12VPrimaryCurrentAtStop,
                Minus12VPrimaryCurrentAtStop = powerResponse.Minus12VPrimaryCurrentAtStop,
                Plus24VPrimaryCurrentAtStop = powerResponse.Plus24VPrimaryCurrentAtStop,
                Minus24VPrimaryCurrentAtStop = powerResponse.Minus24VPrimaryCurrentAtStop,
                BatteryVoltageAtStop = powerResponse.BatteryVoltageAtStop,
                HighVoltageAtStop = powerResponse.HVAtStop,
                Plus12VActualValueAtStop = powerResponse.Plus12VActualValueAtStop,
                Minus12VActualValueAtStop = powerResponse.Minus12VActualValueAtStop,
                Plus24VActualValueAtStop = powerResponse.Plus24VActualValueAtStop,
                Minus24VActualValueAtStop = powerResponse.Minus24VActualValueAtStop,
                VoltageOnSubD9Pin3AtStop = powerResponse.VoltageOnSubD9Pin3AtStop,
                VoltageOnSubD9Pin5AtStop = powerResponse.VoltageOnSubD9Pin5AtStop,
                CurrentSourceStateOnSubD9Pin5 = powerResponse.CurrentSourceStateOnSubD9Pin5,
                CurrentSourceValueOnSubD9Pin5 = powerResponse.CurrentSourceValueOnSubD9Pin5,
                InputResistanceOnSubD9Pin5 = powerResponse.InputResistanceOnSubD9Pin5,
                ADCCorrectionOffsetOnSubD9Pin5 = powerResponse.ADCCorrectionOffsetOnSubD9Pin5,
                GainCorrectionFactorOnSubD9Pin5 = powerResponse.GainCorrectionFactorOnSubD9Pin5,
                ADCCorrectionOffsetOnSubD9Pin3 = powerResponse.ADCCorrectionOffsetOnSubD9Pin3,
                GainCorrectionFactorOnSubD9Pin3 = powerResponse.GainCorrectionFactorOnSubD9Pin3,
                MCATemperatureAtStop = state527Response.MCATemperatureAtStop,
                DetectorTemperatureAtStop = state527Response.DetectorTemperatureAtStop,
                PowerModuleTemperatureAtStop = state527Response.PowerModuleTemperatureAtStop,
                RepeatMode = (sbyte)(StartFlag.SpectrumClearedNewStartTime),
                RepeatModeOptions = 0,
                RepeatValue = stateResponse.RepeatValue,
                AHRCGroup0Width = state527Ex2Response.AHRCGroup0Width,
                AHRCGroup1Width = state527Ex2Response.AHRCGroup1Width,
                AHRCGroup2Width = state527Ex2Response.AHRCGroup2Width,
                AHRCGroup3Width = state527Ex2Response.AHRCGroup3Width,
                AHRCGroup4Width = state527Ex2Response.AHRCGroup4Width,
                AHRCGroup5Width = state527Ex2Response.AHRCGroup5Width,
                AHRCGroup6Width = state527Ex2Response.AHRCGroup6Width,
                AHRCGroup7Width = state527Ex2Response.AHRCGroup7Width,
                AHRCGroup8Width = state527Ex2Response.AHRCGroup8Width,
                AHRCGroup9Width = state527Ex2Response.AHRCGroup9Width,
                AHRCTriggerThreshold = state527Ex2Response.AHRCTriggerThreshold,
            };
        }


        internal static void CreatePrimaryHeader(this NCCFile.MCAFile file,
            QueryState527Response state527Response,
            QueryStateResponse stateResponse

        )
        {
			if (stateResponse == null) { stateResponse = new QueryStateResponse(); stateResponse.Init(CommandName.CMD_QUERY_STATE); } // dummy values 
			if (state527Response == null) { state527Response = new QueryState527Response(); state527Response.Init(CommandName.CMD_QUERY_STATE_527); } // dummy values
            file.header = new NCCFile.MCAFile.MCAHeader
            {
                FileIdentification = "MCA527BIN_APP ",
                FirmwareVersion = state527Response.MCAFirmwareVersion,
                HardwareVersion = state527Response.MCAHardwareVersion,
                FirmwareModification = state527Response.MCAFirmwareModification,
                HardwareModification = state527Response.MCAHardwareModification,
                SerialNumber = stateResponse.MCASerialNumber,
                GeneralMode = (ushort)state527Response.GeneralMCAMode
            };
        }
    }


}

