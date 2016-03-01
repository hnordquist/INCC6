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
using System.Threading;
using System.Threading.Tasks;

namespace Device
{

	class MCAWriter : IDisposable
	{

		Stream Stream;
		public MCAWriter(Stream stream)
		{
			Stream = stream;
		}

		public Stream BaseStream
        {
			get { return Stream; }
		}
        
        public void Close()
        {
            Stream.Close();
        }

        public void Flush()
        {
            Stream.Flush();
        }

		public void Write(string str, int length)
        {
			int len = 0;
			if (str != null) {
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
				len = Math.Min(length, bytes.Length);
				Stream.Write(bytes, 0, len);
			}
			while (len++ < length) { Stream.WriteByte(0); }
		}

        public void Write(sbyte value)
        {
            Write((byte)value);
        }

        public void Write(byte value)
        {
            Stream.WriteByte(value);
        }

		public void Write(ushort value)
		{           
			Stream.WriteByte((byte)(value & 0xff));
			Stream.WriteByte((byte)((value >> 8) & 0xff));
		}

        public void Write(short value)
        {
            Write((ushort)value);
        }

		public void Write(uint value)
		{
			Stream.WriteByte((byte)(value & 0xff));
			Stream.WriteByte((byte)((value >> 8) & 0xff));
			Stream.WriteByte((byte)((value >> 16) & 0xff));
			Stream.WriteByte((byte)((value >> 24) & 0xff));
		}

        public void Write(int value)
        {
            Write((uint)value);
        }

        public void Write(long value)
        {
            Write((ulong)value);
        }

		private void Write(ulong value)
		{
			Stream.WriteByte((byte)(value & 0xff));
			Stream.WriteByte((byte)((value >> 8) & 0xff));
			Stream.WriteByte((byte)((value >> 16) & 0xff));
			Stream.WriteByte((byte)((value >> 24) & 0xff));
			Stream.WriteByte((byte)((value >> 32) & 0xff));
			Stream.WriteByte((byte)((value >> 40) & 0xff));
			Stream.WriteByte((byte)((value >> 48) & 0xff));
			Stream.WriteByte((byte)((value >> 56) & 0xff));
		}

		private void Write(double value)
		{
			Write(BitConverter.DoubleToInt64Bits(value));
		}

		static DateTime DelphiEpoch = new DateTime(1899, 12, 30);
		public void Write(DateTime value)
		{
			// convert to Delphi format
			// => number of days (IEEE 754 double) since delphi epoch
			// => delph epoch: Dec 30, 1899
			Write((value - DelphiEpoch).TotalDays);
		}

        public void Write(MCABasisFileBlockHeader header)
        {
            // jump to the beginning of the stream...
            Stream.Seek(0, SeekOrigin.Begin);
            Write(header.FileIdentification, 14);
            Write(header.UsedBytesOfTheBasisBlock);
            Write(header.FirmwareVersion);
            Write(header.HardwareVersion);
            Write(header.FirmwareModification);
            Write(header.HardwareModification);
            Write(header.SerialNumber);
            Write((ushort)header.GeneralMode);
        }

        const ushort BasisBlockSize = 228;

        public void Write(MCATimestampsBasisFileBlock basisFileBlock)
        {
            basisFileBlock.Header.UsedBytesOfTheBasisBlock = BasisBlockSize;
            basisFileBlock.DataCodingMethod = 0;
            //basisFileBlock.RepeatMode = ;
            //basisFileBlock.RepeatModeOptions = ;

            Write(basisFileBlock.Header);
            Write(basisFileBlock.ApplicationIdentification, 32);
            Write(basisFileBlock.TimeUnitLength);
            Write(basisFileBlock.Preset);
            Write(basisFileBlock.PresetValue);
            Write(basisFileBlock.PresetMemorySize);
            Write(basisFileBlock.UsedMemorySize);
            Write(basisFileBlock.HighVoltage);
            Write(basisFileBlock.HighVoltagePolarity);
            Write(basisFileBlock.HVInhibitMode);
            Write(basisFileBlock.PreamplifierPowerSwitches);
            Write(basisFileBlock.TTLLowLevel);
            Write(basisFileBlock.TTLHighLevel);
            Write(basisFileBlock.AmplifierCoarseGain);
            Write(basisFileBlock.ADCInputPolarity);
            Write(basisFileBlock.ShapingTimeChoice);
            Write(basisFileBlock.TriggerFilterForLowShapingTime);
            Write(basisFileBlock.TriggerFilterForHighShapingTime);
            Write(basisFileBlock.OffsetDAC);
            Write(basisFileBlock.TriggerLevelForAutomaticThresholdCalculation);
            Write(basisFileBlock.SetTriggerThreshold);
            Write(basisFileBlock.ExtensionPortPartAConfiguration);
            Write(basisFileBlock.ExtensionPortPartBConfiguration);
            Write(basisFileBlock.ExtensionPortPartCConfiguration);
            Write(basisFileBlock.ExtensionPortPartFConfiguration);
            Write(basisFileBlock.ExtensionPortRS232BaudRate);
            Write(basisFileBlock.ExtensionPortRS232Flags);
            Write(basisFileBlock.StartFlag);
            Write(basisFileBlock.StartTime);
            Write(basisFileBlock.RealTime);
            Write(basisFileBlock.BatteryCurrentAtStop);
            Write(basisFileBlock.ChargerCurrentAtStop);
            Write(basisFileBlock.HVPrimaryCurrentAtStop);
            Write(basisFileBlock.Plus12VPrimaryCurrentAtStop);
            Write(basisFileBlock.Minus12VPrimaryCurrentAtStop);
            Write(basisFileBlock.Plus24VPrimaryCurrentAtStop);
            Write(basisFileBlock.Minus24VPrimaryCurrentAtStop);
            Write(basisFileBlock.BatteryVoltageAtStop);
            Write(basisFileBlock.HighVoltageAtStop);
            Write(basisFileBlock.Plus12VActualValueAtStop);
            Write(basisFileBlock.Minus12VActualValueAtStop);
            Write(basisFileBlock.Plus24VActualValueAtStop);
            Write(basisFileBlock.Minus24VActualValueAtStop);
            Write(basisFileBlock.VoltageOnSubD9Pin3AtStop);
            Write(basisFileBlock.VoltageOnSubD9Pin5AtStop);
            Write(basisFileBlock.CurrentSourceStateOnSubD9Pin5);
            Write(basisFileBlock.CurrentSourceValueOnSubD9Pin5);
            Write(basisFileBlock.InputResistanceOnSubD9Pin5);
            Write(basisFileBlock.ADCCorrectionOffsetOnSubD9Pin5);
            Write(basisFileBlock.GainCorrectionFactorOnSubD9Pin5);
            Write(basisFileBlock.MCATemperatureAtStop);
            Write(basisFileBlock.DetectorTemperatureAtStop);
            Write(basisFileBlock.PowerModuleTemperatureAtStop);
            Write(basisFileBlock.RepeatMode);
            Write(basisFileBlock.RepeatModeOptions);
            Write(basisFileBlock.RepeatValue);
            Write(basisFileBlock.AHRCGroup0Width);
            Write(basisFileBlock.AHRCGroup1Width);
            Write(basisFileBlock.AHRCGroup2Width);
            Write(basisFileBlock.AHRCGroup3Width);
            Write(basisFileBlock.AHRCGroup4Width);
            Write(basisFileBlock.AHRCGroup5Width);
            Write(basisFileBlock.AHRCGroup6Width);
            Write(basisFileBlock.AHRCGroup7Width);
            Write(basisFileBlock.AHRCGroup8Width);
            Write(basisFileBlock.AHRCGroup9Width);
            Write(basisFileBlock.AHRCTriggerThreshold);
            Write(basisFileBlock.DataCodingMethod);
        }


        public void AdvanceToTimestampsBlock()
        {
            Stream.Seek(BasisBlockSize, SeekOrigin.Begin);
        }

        public void WriteTimestampsRawDataChunk(byte[] rawDataChunk, int offset, int count)
        {
            Stream.Write(rawDataChunk, offset, count);
        }

        public uint WriteTimestamps(IEnumerable<ulong> timestamps, ushort dataCodingMethod)
        {
            // 0: varying byte length
            // 1: unsigned chars
            // 2: unsigned shorts => default
            long startPosition = Stream.Position;
            switch (dataCodingMethod) {
            case 0:
                WriteTimestampsWithDataCodingMethod0(timestamps);
                break;
            case 1:
                WriteTimestampsWithDataCodingMethod1(timestamps);
                break;
            default:
                WriteTimestampsWithDataCodingMethod2(timestamps);
                break;
            }
            return (uint)(Stream.Position - startPosition);
        }

        void WriteTimestampsWithDataCodingMethod0(IEnumerable<ulong> timestamps)
        {
            foreach (uint timestamp in timestamps) {
                WriteTimestampWithDataCodingMethod0(timestamp);
            }
        }
        void WriteTimestampsWithDataCodingMethod1(IEnumerable<ulong> timestamps)
        {
            foreach (uint timestamp in timestamps) {
                WriteTimestampWithDataCodingMethod1(timestamp);
            }
        }
        void WriteTimestampsWithDataCodingMethod2(IEnumerable<ulong> timestamps)
        {
            foreach (uint timestamp in timestamps) {
                WriteTimestampWithDataCodingMethod2(timestamp);
            }
        }

        void WriteTimestampWithDataCodingMethod0(ulong timestamp)
        {
            while (timestamp > 0) {
                if (timestamp >= 67907775) {
                    WriteTimestampWithDataCodingMethod0UInt(67907775);
                    if (timestamp == 67907775) {
                        WriteTimestampWithDataCodingMethod0UInt(0);
                    }
                } else {
                    WriteTimestampWithDataCodingMethod0UInt((uint)timestamp);
                }
            }
        }

        void WriteTimestampWithDataCodingMethod0UInt(uint timestamp)
        {
            if (timestamp < 192) {
                Write((byte)timestamp);
            } else if (timestamp < 12480) {
                uint value = timestamp - 192;
                Write((byte)(0xc0 | (value >> 8)));
                Write((byte)(value & 0xff));
            } else if (timestamp < 798912) {
                uint value = timestamp - 12480;
                Write((byte)(0xf | (value >> 16)));
                Write((byte)((value >> 8) & 0xff));
                Write((byte)(value & 0xff));
            } else if (timestamp < 67907776) {
                uint value = timestamp - 798912;
                Write((byte)(0xfc | (value >> 24)));
                Write((byte)((value >> 16) & 0xff));
                Write((byte)((value >> 8) & 0xff));
                Write((byte)(value & 0xff));
            } else {
                // have to split it into two separate writes...
                WriteTimestampWithDataCodingMethod0(67907775);
                WriteTimestampWithDataCodingMethod0(timestamp - 67907775);
            }
        }
        void WriteTimestampWithDataCodingMethod1(ulong timestamp)
        {
            while (timestamp > 0) {
                if (timestamp > 255)
                    Write((byte)255);
                else
                    Write((byte)timestamp);
                timestamp -= 255;
            }
        }
        void WriteTimestampWithDataCodingMethod2(ulong timestamp)
        {
            while (timestamp > 0) {
                if (timestamp > 65535)
                    Write((ushort)65535);
                else
                    Write((ushort)timestamp);
                timestamp -= 65535;
            }
        }

        // FOR NOW, IGNORE RS232 RECEIVED DATA
        // this block exists if the used memory size is greater than zero *and*
        // extension port part a config is 5 or extension port part c config is 5
        // this should never be the case when used with INCC

        public void Dispose()
        {
            ((IDisposable)Stream).Dispose();
        }


    }


	class FileStreamReadException : Exception { }
	static class Extensions
	{
		public static DateTime ToMCADateTime(this uint startTime)
		{
			// MCA start time => number of seconds since Dec 31, 1969, 16:00:00 GMT
			DateTime mcaEpoch = new DateTime(1969, 12, 31, 16, 0, 0);
			int hours = 0;
			while (startTime > int.MaxValue) {
				hours += 1;
				startTime -= 60 * 60;
			}
			return mcaEpoch + new TimeSpan(hours, 0, (int)startTime);
		}

		public static String ReadString(this FileStream fileStream, int characterCount)
		{
			byte[] buffer = new byte[characterCount];
			int bytesRead = fileStream.Read(buffer, 0, characterCount);
			if (bytesRead != characterCount) { return null; }
			return System.Text.Encoding.UTF8.GetString(buffer);
		}
		private static byte[] bytes = new byte[8];
		public static sbyte ReadSByte(this FileStream fileStream)
		{
			int bytesRead = fileStream.Read(bytes, 0, 1);
			if (bytesRead != 1) { throw new FileStreamReadException(); }
			return (sbyte)bytes[0];
		}
		public static byte ReadUByte(this FileStream fileStream)
		{
			int bytesRead = fileStream.Read(bytes, 0, 1);
			if (bytesRead != 1) { throw new FileStreamReadException(); }
			return bytes[0];
		}
		public static ushort ReadUShort(this FileStream fileStream)
		{
			int bytesRead = fileStream.Read(bytes, 0, 2);
			if (bytesRead != 2) { throw new FileStreamReadException(); }
			return BitConverter.ToUInt16(bytes, 0);				
		}
		public static short ReadShort(this FileStream fileStream)
		{
			int bytesRead = fileStream.Read(bytes, 0, 2);
			if (bytesRead != 2) { throw new FileStreamReadException(); }
			return BitConverter.ToInt16(bytes, 0);				
		}
		public static uint ReadUInt(this FileStream fileStream)
		{
 			int bytesRead = fileStream.Read(bytes, 0, 4);
			if (bytesRead != 4) { throw new FileStreamReadException(); }
			return BitConverter.ToUInt32(bytes, 0);
		}
		public static int ReadInt(this FileStream fileStream)
		{
 			int bytesRead = fileStream.Read(bytes, 0, 4);
			if (bytesRead != 4) { throw new FileStreamReadException(); }
			return BitConverter.ToInt32(bytes, 0);			
		}
		public static long ReadLong(this FileStream fileStream)
		{
			int bytesRead = fileStream.Read(bytes, 0, 8);
			if (bytesRead != 8) { throw new FileStreamReadException(); }
			return BitConverter.ToInt64(bytes, 0);
		}
		public static uint ReadVariableLengthUInt(this FileStream fileStream)
		{
			int bytesRead = fileStream.Read(bytes, 0, 1);
			if (bytesRead != 1) { throw new FileStreamReadException(); }
			// if the value is less than 192, we are done
			if (bytes[0] < 0xC0) {
				return (uint)bytes[0];
			}
			if (bytes[0] < 0xF0) {
				// two bytes
				bytesRead = fileStream.Read(bytes, 1, 1);
				if (bytesRead != 1) { throw new FileStreamReadException(); }
				// 0b00111111 => 0x3f
				return ((((((uint)bytes[0]) & 0x3f) << 8) | ((uint)bytes[1]))) + 0xC0;
			} else if (bytes[0] < 0xFC) {
				// three bytes
				bytesRead = fileStream.Read(bytes, 1, 2);
				if (bytesRead != 2) { throw new FileStreamReadException(); }
				// 0b00001111 => 0x0f
				return (((((uint)bytes[0]) & 0x0f) << 16) | (((uint)bytes[1]) << 8) | ((uint)bytes[2])) + 0x30C0;
			} else {
				// four bytes
				bytesRead = fileStream.Read(bytes, 1, 3);
				if (bytesRead != 3) { throw new FileStreamReadException(); }
				// 0b00000011 => 0x03
				return (((((uint)bytes[0]) & 0x03) << 24) | (((uint)bytes[1]) << 16) | (((uint)bytes[2]) << 8) | ((uint)bytes[3])) + 0x0C30C0;
			}
		}



		public static void WriteUShort(this FileStream fileStream, ushort value)
		{
			fileStream.WriteByte((byte)(value & 0xff));
			fileStream.WriteByte((byte)((value >> 8) & 0xff));
		}

		public static void WriteUInt32(this FileStream fileStream, uint value)
		{
			fileStream.WriteByte((byte)(value & 0xff));
			fileStream.WriteByte((byte)((value >> 8) & 0xff));
			fileStream.WriteByte((byte)((value >> 16) & 0xff));
			fileStream.WriteByte((byte)((value >> 24) & 0xff));
		}

		public static void WriteTDateTime(this FileStream fileStream, DateTime dateTime)
		{
			// convert dateTime to Delphi format
			// => number of days (IEEE 754 double) since delphi epoch
			// => delphi epoch Dec 30, 1899
			DateTime delphiEpoch = new DateTime(1899, 12, 30);
			TimeSpan difference = dateTime - delphiEpoch;
			fileStream.Write(BitConverter.GetBytes(difference.TotalDays), 0, 8);
		}

		public static void WriteString(this FileStream fileStream, string str, int length)
		{
			int len = 0;
			if (str != null) {
				byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
				len = Math.Min(length, bytes.Length);
				fileStream.Write(bytes, 0, len);
			}
			while (len++ < length) { fileStream.WriteByte(0); }
		}

		public static void Write(this FileStream fileStream, MCABasisFileBlockHeader basisFileBlockHeader)
		{
			fileStream.WriteString(basisFileBlockHeader.FileIdentification, 14);
			fileStream.WriteUShort(basisFileBlockHeader.UsedBytesOfTheBasisBlock);

		}


	}


    class MCATimestampsBasisFileBlock
    {
        public MCABasisFileBlockHeader Header;
        string _ApplicationIdentification;
        public string ApplicationIdentification
        {
            get {
                if (_ApplicationIdentification == null) {
                    return "WinTimestamps Version 1.00.0000";
                }
                return _ApplicationIdentification;
            }
            set {
                _ApplicationIdentification = value;
            }
        }
        public ushort TimeUnitLength;
        public ushort Preset;
        public uint PresetValue;
        public uint PresetMemorySize;
        public uint UsedMemorySize;
        public ushort HighVoltage;
        public ushort HighVoltagePolarity;
        public short HVInhibitMode;
        public ushort PreamplifierPowerSwitches;
        public byte TTLLowLevel;
        public byte TTLHighLevel;
        public ushort AmplifierCoarseGain;
        public ushort ADCInputPolarity;
        public ushort ShapingTimeChoice;
        public byte TriggerFilterForLowShapingTime;
        public byte TriggerFilterForHighShapingTime;
        public ushort OffsetDAC;
        public ushort TriggerLevelForAutomaticThresholdCalculation;
        public int SetTriggerThreshold;
        public byte ExtensionPortPartAConfiguration;
        public byte ExtensionPortPartBConfiguration;
        public byte ExtensionPortPartCConfiguration;
        public byte ExtensionPortPartFConfiguration;
        public ushort ExtensionPortRS232BaudRate;
        public ushort ExtensionPortRS232Flags;
        public ushort StartFlag;
        public uint StartTime;
        public uint RealTime;
        public uint BatteryCurrentAtStop;
        public uint ChargerCurrentAtStop;
        public uint HVPrimaryCurrentAtStop;
        public uint Plus12VPrimaryCurrentAtStop;
        public uint Minus12VPrimaryCurrentAtStop;
        public uint Plus24VPrimaryCurrentAtStop;
        public uint Minus24VPrimaryCurrentAtStop;
        public uint BatteryVoltageAtStop;
        public uint HighVoltageAtStop;
        public byte Plus12VActualValueAtStop;
        public byte Minus12VActualValueAtStop;
        public byte Plus24VActualValueAtStop;
        public byte Minus24VActualValueAtStop;
        public ushort VoltageOnSubD9Pin3AtStop;
        public ushort VoltageOnSubD9Pin5AtStop;
        public ushort CurrentSourceStateOnSubD9Pin5;
        public ushort CurrentSourceValueOnSubD9Pin5;
        public ushort InputResistanceOnSubD9Pin5;
        public sbyte ADCCorrectionOffsetOnSubD9Pin5;
        public sbyte GainCorrectionFactorOnSubD9Pin5;
        public sbyte ADCCorrectionOffsetOnSubD9Pin3;
        public sbyte GainCorrectionFactorOnSubD9Pin3;
        public short MCATemperatureAtStop;
        public short DetectorTemperatureAtStop;
        public short PowerModuleTemperatureAtStop;
        public sbyte RepeatMode;
        public sbyte RepeatModeOptions;
        public ushort RepeatValue; // binary data format docs say this is short, firmware commands say it is ushort
        public uint AHRCGroup0Width;
        public uint AHRCGroup1Width;
        public uint AHRCGroup2Width;
        public uint AHRCGroup3Width;
        public uint AHRCGroup4Width;
        public uint AHRCGroup5Width;
        public uint AHRCGroup6Width;
        public uint AHRCGroup7Width;
        public uint AHRCGroup8Width;
        public uint AHRCGroup9Width;
        public ushort AHRCTriggerThreshold;
        public ushort DataCodingMethod;

        public static MCATimestampsBasisFileBlock Create(
            QueryStateResponse stateResponse,
            QueryState527ExResponse state527ExResponse,
            QueryState527Response state527Response,
            QueryPowerResponse powerResponse,
            QueryState527Ex2Response state527Ex2Response
        )       
        {
            MCATimestampsBasisFileBlock basisFileBlock = new MCATimestampsBasisFileBlock {
                // sampling rate: kHz : 1000
                // time unit length: ns : 1000000000
                // 1000000000 / (sampling_rate / 1000)
                // 1000000 / sampling_rate
                Header = MCABasisFileBlockHeader.Create(state527Response, stateResponse),
                TimeUnitLength = (ushort)(1000000 / state527ExResponse.ADCSamplingRate),
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
                // RepeatMode = ??
                // RepeatModeOptions = ??
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
                // DataCodingMethod = ??
            };
            return basisFileBlock;
        }


    }


	enum EndFlagCode : ushort {
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
			if (length == 138) {
				// check for command and checksum
				// if they don't make sense, check for a checksum at byte offset 130 =>
				// CMD_QUERY_SPECTRA and CMD_QUERY_SPECTRA_EX are special cases...
				ushort command = bytes.ReadFlippedUShort(106 + 4);
				ushort checksum = bytes.ReadUShort(126 + 4);
				// calculate the checksum, assume it is at position 126
				ushort calculatedChecksum = 0;
				for (int i = 2; i < 138; i += 2) {
					if (i == 126 + 4) { i += 2; } // skip the actual checksum...
					ushort value = bytes.ReadUShort(i);
					calculatedChecksum += value;
				}
				if (Commands.ValidCommands.Contains((CommandName)command) == false ||
					checksum != calculatedChecksum) {
					// perhaps it is a special command?
					// for now, just assume it is broken
					// TODO: Add support for CMD_QUERY_SPECTRA and CMD_QUERY_SPECTRA_EX
					return null;
				}

				switch ((CommandName)command) {
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
			else if (endFlag != (ushort)EndFlagCode.Success) {
				// URGENT: use logger here Console.WriteLine("EndFlagCode: {0}", endFlag);
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
	}

	class SetMeasurePZCResponse : MCAResponse
	{
		public short NumberOfMeasuredPulses { get { return Bytes.ReadShort(124 + 4); } }
		public short AveragedNegativeOffsetOfMeasuredInputPulses { get { return Bytes.ReadShort(128 + 4); } }
	}

	enum MCAState : ushort {
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

	enum BufferState : ushort {
		Occupied = 0x2000,
		Overrun = 0x4000,
		Filled = 0x8000
	}

	class QuerySystemDataResponse : MCAResponse
	{
		public long DetectedCounts { get { return Bytes.Read48BitLong(10 + 4); } }		
		public uint MMCAOnTime { get { return Bytes.ReadUInt(36 + 4); } }
		public uint RealTimeOfPreviousSweep { get { return Bytes.ReadUInt(40 + 4); }  }
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

	enum ExecutionRight : short {
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
			get {
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
        public uint ExtensionPortPulser3Width{ get { return Bytes.ReadUInt(80 + 4); } }
        public uint ExtensionPortCounter3 { get { return Bytes.ReadUInt(84 + 4); } }
        public uint ExtensionPortCounter3CPS { get { return Bytes.ReadUInt(88 + 4); } }
        public uint ExtensionPortCounter3OfPreviousSweep { get { return Bytes.ReadUInt(92 + 4); } }
        public short AdditionalTemperature1 { get { return Bytes.ReadShort(96 + 2); } }
        public short AdditionalTemperature1AtStop { get { return Bytes.ReadShort(98 + 2); } }
        public short AdditionalTemperature2 { get { return Bytes.ReadShort(100 + 2); } }
        public short AdditionalTemperature2AtStop { get { return Bytes.ReadShort(102 + 2); } }
        public MCAState MCAState { get { return (MCAState)(Bytes.ReadShort(128+4)); } }
    }

	class QueryUserDataResponse : MCAResponse
	{
		public uint[] Values
		{
			get {
				uint[] values = new uint[16];
				for (int i = 0; i < 16; i++) {
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
			get {
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
			get {
				short[] values = new short[12];
				for (int i = 0; i < 12; i++) {
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
	

    class MCAReader : IDisposable
    {

        Stream Stream;
        MCAReader(Stream stream)
        {
            Stream = stream;
        }

        public Stream BaseStream
        {
            get { return Stream; }
        }

        public void Close()
        {
            Stream.Close();
        }

        public string ReadString(int length)
        {
            byte[] buffer = new byte[length];
            int bytesRead = Stream.Read(buffer, 0, length);
            if (bytesRead != length) { throw new IOException(); }
            return System.Text.Encoding.UTF8.GetString(buffer);
        }

        byte[] Bytes = new byte[8];

        public byte ReadByte()
        {
            int bytesRead = Stream.Read(Bytes, 0, 1);
            if (bytesRead != 1 ) { throw new IOException(); }
            return Bytes[0];
        }
        public sbyte ReadSByte()
        {
            return (sbyte)ReadByte();
        }

        public ushort ReadUShort()
        {
            int bytesRead = Stream.Read(Bytes, 0, 2);
            if (bytesRead != 2 ) { throw new IOException(); }
            return (ushort)(Bytes[0] | (Bytes[1] << 8));
        }
        public short ReadShort()
        {
            return (short)ReadUShort();
        }

        public uint ReadUInt()
        {
            int bytesRead = Stream.Read(Bytes, 0, 4);
            if (bytesRead != 4 ) { throw new IOException(); }
            return (uint)(Bytes[0] | (Bytes[1] << 8) | (Bytes[2] << 16) | (Bytes[3] << 24));
        }
        public int ReadInt()
        {
            return (int)ReadUInt();
        }

        public ulong ReadULong()
        {
            int bytesRead = Stream.Read(Bytes, 0, 8);
            if (bytesRead != 8 ) { throw new IOException(); }
            uint val1 = (uint)(Bytes[0] | (Bytes[1] << 8) | (Bytes[2] << 16) | (Bytes[3] << 24));
            uint val2 = (uint)(Bytes[4] | (Bytes[5] << 8) | (Bytes[6] << 16) | (Bytes[7] << 24));
            return ((ulong)val1 | ((ulong)val2 << 32));
        }
        public long ReadLong()
        {
            return (long)ReadULong();
        }

        private double ReadDouble()
        {
            return BitConverter.Int64BitsToDouble(ReadLong());
        }

        static DateTime DelphiEpoch = new DateTime(1899, 12, 30);
        public DateTime ReadDateTime()
        {
            return DelphiEpoch.AddDays(ReadDouble());
        }

        public MCABasisFileBlockHeader ReadBasisFileBlockHeader()
        {
            return new MCABasisFileBlockHeader {
                FileIdentification = ReadString(14),
                UsedBytesOfTheBasisBlock = ReadUShort(),
                FirmwareVersion = ReadUShort(),
                HardwareVersion = ReadUShort(),
                FirmwareModification = ReadUShort(),
                HardwareModification = ReadUShort(),
                SerialNumber = ReadUShort(),
                GeneralMode = (Mode)ReadUShort()
            };
        }

        public MCATimestampsBasisFileBlock ReadTimestampsBasisFileBlock()
        {
            MCABasisFileBlockHeader header = ReadBasisFileBlockHeader();
            if (!(header.GeneralMode == Mode.LevelTriggeredTimestampRecorder ||
                  header.GeneralMode == Mode.EdgeTriggeredTimestampRecorder ||
                  header.GeneralMode == Mode.AHRCTimeStampRecorder)) {
                throw new IOException();
            }
            return new MCATimestampsBasisFileBlock {
                Header = header,
                ApplicationIdentification = ReadString(32),
                TimeUnitLength = ReadUShort(),
                Preset = ReadUShort(),
                PresetMemorySize = ReadUInt(),
                UsedMemorySize = ReadUInt(),
                HighVoltage = ReadUShort(),
                HighVoltagePolarity = ReadUShort(),
                HVInhibitMode = ReadShort(),
                PreamplifierPowerSwitches = ReadUShort(),
                TTLLowLevel = ReadByte(),
                TTLHighLevel = ReadByte(),
                AmplifierCoarseGain = ReadUShort(),
                ADCInputPolarity = ReadUShort(),
                ShapingTimeChoice = ReadUShort(),
                TriggerFilterForLowShapingTime = ReadByte(),
                TriggerFilterForHighShapingTime = ReadByte(),
                OffsetDAC = ReadUShort(),
                TriggerLevelForAutomaticThresholdCalculation = ReadUShort(),
                SetTriggerThreshold = ReadInt(),
                ExtensionPortPartAConfiguration = ReadByte(),
                ExtensionPortPartBConfiguration = ReadByte(),
                ExtensionPortPartCConfiguration = ReadByte(),
                ExtensionPortPartFConfiguration = ReadByte(),
                ExtensionPortRS232BaudRate = ReadUShort(),
                ExtensionPortRS232Flags = ReadUShort(),
                StartFlag = ReadUShort(),
                StartTime = ReadUInt(),
                RealTime = ReadUInt(),
                BatteryCurrentAtStop = ReadUInt(),
                ChargerCurrentAtStop = ReadUInt(),
                HVPrimaryCurrentAtStop = ReadUInt(),
                Plus12VPrimaryCurrentAtStop = ReadUInt(),
                Minus12VPrimaryCurrentAtStop = ReadUInt(),
                Plus24VPrimaryCurrentAtStop = ReadUInt(),
                Minus24VPrimaryCurrentAtStop = ReadUInt(),
                BatteryVoltageAtStop = ReadUInt(),
                HighVoltageAtStop = ReadUInt(),
                Plus12VActualValueAtStop = ReadByte(),
                Minus12VActualValueAtStop = ReadByte(),
                Plus24VActualValueAtStop = ReadByte(),
                Minus24VActualValueAtStop = ReadByte(),
                VoltageOnSubD9Pin3AtStop = ReadUShort(),
                VoltageOnSubD9Pin5AtStop = ReadUShort(),
                CurrentSourceStateOnSubD9Pin5 = ReadUShort(),
                CurrentSourceValueOnSubD9Pin5 = ReadUShort(),
                InputResistanceOnSubD9Pin5 = ReadUShort(),
                ADCCorrectionOffsetOnSubD9Pin5 = ReadSByte(),
                GainCorrectionFactorOnSubD9Pin5 = ReadSByte(),
                ADCCorrectionOffsetOnSubD9Pin3 = ReadSByte(),
                GainCorrectionFactorOnSubD9Pin3 = ReadSByte(),
                MCATemperatureAtStop = ReadShort(),
                DetectorTemperatureAtStop = ReadShort(),
                PowerModuleTemperatureAtStop = ReadShort(),
                RepeatMode = ReadSByte(),
                RepeatModeOptions = ReadSByte(),
                RepeatValue = ReadUShort(), // binary data format docs say this is short, firmware commands say it is ushort
                AHRCGroup0Width = ReadUInt(),
                AHRCGroup1Width = ReadUInt(),
                AHRCGroup2Width = ReadUInt(),
                AHRCGroup3Width = ReadUInt(),
                AHRCGroup4Width = ReadUInt(),
                AHRCGroup5Width = ReadUInt(),
                AHRCGroup6Width = ReadUInt(),
                AHRCGroup7Width = ReadUInt(),
                AHRCGroup8Width = ReadUInt(),
                AHRCGroup9Width = ReadUInt(),
                AHRCTriggerThreshold = ReadUShort(),
                DataCodingMethod = ReadUShort(),
            };
        }

        public IEnumerable<ulong> EnumerateTimestamps(uint usedMemorySize, ushort dataCodingMethod)
        {
            switch (dataCodingMethod) {
            case 0:
                return EnumerateTimestampsWithDataCodingMethod0(usedMemorySize);
            case 1:
                return EnumerateTimestampsWithDataCodingMethod1(usedMemorySize);
            default:
                return EnumerateTimestampsWithDataCodingMethod2(usedMemorySize);
            }
        }

        IEnumerable<ulong> EnumerateTimestampsWithDataCodingMethod0(uint usedMemorySize)
        {
            long startPosition = Stream.Position;
            while (Stream.Position - startPosition < usedMemorySize) {
                yield return ReadTimestampWithDataCodingMethod0();
            }
        }
        IEnumerable<ulong> EnumerateTimestampsWithDataCodingMethod1(uint usedMemorySize)
        {
            long startPosition = Stream.Position;
            while (Stream.Position - startPosition < usedMemorySize) {
                yield return ReadTimestampWithDataCodingMethod1();
            }
        }
        IEnumerable<ulong> EnumerateTimestampsWithDataCodingMethod2(uint usedMemorySize)
        {
            long startPosition = Stream.Position;
            while (Stream.Position - startPosition < usedMemorySize) {
                yield return ReadTimestampWithDataCodingMethod2();
            }
        }

        ulong ReadTimestampWithDataCodingMethod0()
        {
            ulong timestamp = 0;
            for (;;) {
                uint value = ReadTimestampWithDataCodingMethod0UInt();
                timestamp += value;
                if (value + 1 != 0x40c30c0) { break; }                
            }
            return timestamp;
        }

        uint ReadTimestampWithDataCodingMethod0UInt()
        {
            if (Stream.Read(Bytes, 0, 1) != 1) { throw new IOException(); }
            if (Bytes[0] < 0xc0) {
                return (uint)Bytes[0];
            } else if (Bytes[0] < 0xf0) {
                if (Stream.Read(Bytes, 1, 1) != 1) { throw new IOException(); }
                return (uint)((((Bytes[0] & 0x3f) << 8) | Bytes[1])) + 0xc0;;
            } else if (Bytes[0] < 0xfc) {
                if (Stream.Read(Bytes, 1, 2) != 2) { throw new IOException(); }
                return (uint)(((Bytes[0] & 0x0f) << 16) | (Bytes[1] << 8) | Bytes[2]) + 0x30c0;
            } else {
                if (Stream.Read(Bytes, 1, 3) != 3) { throw new IOException(); }
                return (uint)(((Bytes[0] & 0x03) << 24) | (Bytes[1] << 16) | (Bytes[2] << 8) | (Bytes[3])) + 0xc30c0;
            }
        }
        ulong ReadTimestampWithDataCodingMethod1()
        {
            ulong timestamp = 0;
            for (;;) {
                byte value = ReadByte();
                timestamp += value;
                if (value != byte.MaxValue) { break; }
            }
            return timestamp;
        }
        ulong ReadTimestampWithDataCodingMethod2()
        {
            ulong timestamp = 0;
            for (;;) {
                ushort value = ReadUShort();
                timestamp += value;
                if (value != ushort.MaxValue) { break; }
            }
            return timestamp;
        }

        public void Dispose()
        {
            ((IDisposable)Stream).Dispose();
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

   public enum MCAHardwareVersion : ushort {
        Full = 0,
        Lite = 1,
        OEM = 2,
        Micro = 3
    };

    enum MCAFeatures : uint {
        Expander                        = 0x00000001,
        InternalTemperature             = 0x00000002,
        ExternalTemperature             = 0x00000004,
        MACAddress                      = 0x00000008,
        PowerModule                     = 0x00000010,
        MicroSDCard                     = 0x00000020,
        PowerModuleTemperature          = 0x00000040,
        AnalogVoltages                  = 0x00000080,
        GatingInput                     = 0x00000100,
        ExtensionPort                   = 0x00000200,
        LFRejection                     = 0x00000400,
        JitterCorrection                = 0x00000800,
        AdjustableTriggerFilter         = 0x00001000,
        AdjustableBaselineRestorer      = 0x00002000,
        AdjustableCoarseGain            = 0x00004000,
        USBRS232                        = 0x00008000,
        USBChargingDefaultOff           = 0x00010000,
        NoOffsetDAC                     = 0x00020000,
        SDRAMOk                         = 0x00040000,
        TimestampRecorder               = 0x00080000,
        Bluetooth                       = 0x00100000,
        GatingByTime                    = 0x00200000,
        BootPresets                     = 0x00400000,
        DetectorInfo                    = 0x00800000,
        DetectorInfoValid               = 0x01000000,
        // AdditionalTemperature1          = 0x02000000, // not sure about this, not in header provided by GBS
        // AdditionalTemperature2          = 0x04000000, // not sure about this, not in header provided by GBS
        DebugInfo                       = 0x80000000
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

        public MCAHardwareVersion HardwareModification { get; private set; }
        public bool HasPowerModule { get; private set; }
        public ushort MaximumHighVoltage { get; private set; }

        public TimeSpan Timeout {
            get { return Client.DefaultTimeout; }
            set { Client.DefaultTimeout = value; }
        }

        public uint RetryCount {
            get { return Client.DefaultRetryCount; }
            set { Client.DefaultRetryCount = value; }
        }

		public static
        #if NETFX_45
            async Task<MCADeviceInfo[]>
        #else
            MCADeviceInfo[]
        #endif
        QueryDevices()
		{
			List<MCADeviceInfo> devicesList = new List<MCADeviceInfo>();
			MCAClient client = new MCAClient();
			const int retryCount = 3;
			for (int i = 0; i < retryCount; i++) {
				MCAResponse[] responses =
                #if NETFX_45 
                    await
                #endif
                    client.SendBroadcastAsync(MCACommand.QueryState527());
				if (responses.Length > 0) {
					foreach (QueryState527Response response in responses) {
                        devicesList.Add(new MCADeviceInfo {
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
			// need to stay in contact with it at least every 15 seconds to maintain ownership
			HeartbeatRunloop();
		}

		CancellationTokenSource CancelTokenSource = new CancellationTokenSource();
		TaskCompletionSource<bool> HeartbeatDone = new TaskCompletionSource<bool>();
		public void Close()
		{
			CancelTokenSource.Cancel();
			HeartbeatDone.Task.Wait();
			Client.Close();
		}

		public SemaphoreSlim mHeartbeatSemaphore = new SemaphoreSlim(1);

        // try to contact once every 5 seconds
		static TimeSpan HeartbeatTimeSpan = TimeSpan.FromSeconds(5);
#if NETFX_45
        async 
#endif
            void HeartbeatRunloop()
		{
#if NETFX_45
            try {
			    while (CancelTokenSource.Token.IsCancellationRequested == false) {
				    await Task.Delay(HeartbeatTimeSpan, CancelTokenSource.Token);                
				    mHeartbeatSemaphore.Wait();
				    QueryStateResponse qsr = (QueryStateResponse) await Client.SendAsync(MCACommand.QueryState());
				    mHeartbeatSemaphore.Release();
			    }
            } catch (TaskCanceledException) { }
#endif
			HeartbeatDone.SetResult(true);
		}

		public
#if NETFX_45
            async Task
#else
            void
#endif
            Initialize()
        {
#if NETFX_45
            mHeartbeatSemaphore.Wait();
			try {
				MCAResponse response = null;
				response = await Client.SendAsync(MCACommand.Init());
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetPreamplifierPower(PreamplifierPower.Minus12V |
																	PreamplifierPower.Plus12V |
																	PreamplifierPower.Minus24V |
																	PreamplifierPower.Plus24V));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetGeneralMode(Mode.EdgeTriggeredTimestampRecorder));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetInputPolarity(InputPolarity.Positive));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetTriggerFilter(0, 0));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetMeasurePZC(0, PoleZeroCancellationMode.SetPZCOnly));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetGain(2, 10000));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetTriggerParam(TriggerParameter.TriggerLevelForAutomaticThresholdCalculationForDirectInput, 0x258000));
				if (response == null) { throw new MCADeviceLostConnectionException(); }

                // query information about availability of high voltage
                QueryState527Response state527Response = (QueryState527Response) await Client.SendAsync(MCACommand.QueryState527());
                if (state527Response == null) { throw new MCADeviceLostConnectionException(); }
                HardwareModification = (MCAHardwareVersion)state527Response.MCAHardwareModification;
                uint features = state527Response.MCAFeatures;
                if ((features & (uint)MCAFeatures.PowerModule) != 0) {
                    HasPowerModule = true;
                }
                if (HasPowerModule) {
                    MaximumHighVoltage = state527Response.MaximumAllowedHighVoltage;
                }
                await SetHighVoltage(0, BiasInhibitInput.InhibitOff);
			} 
			catch (MCADeviceLostConnectionException ex)
			{
				throw ex;
			}
			catch {
				throw;
			}
			finally {
				mHeartbeatSemaphore.Release();
			}
#endif
        }


        public
#if NETFX_45
            async Task
#else
            void
#endif
        SetTimestampMode(TimestampMode mode)
        {
#if NETFX_45
            MCAResponse response = await Client.SendAsync(MCACommand.SetGeneralMode((Mode)((uint)mode)));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
#endif
        }

        public
#if NETFX_45
        async Task<TimestampMode> 
#else
        void
#endif
        GetTimestampMode()
        {
#if NETFX_45
            QueryState527Response response = (QueryState527Response) await Client.SendAsync(MCACommand.QueryState527());
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            return (TimestampMode)response.GeneralMCAMode;
#endif
        }

        public
#if NETFX_45
        async Task
#else
        void
#endif
          SetHighVoltage(ushort value, BiasInhibitInput inhibitSignal)
        {
            if (HasPowerModule == false) { return; }
            if (value > MaximumHighVoltage) { throw new ArgumentOutOfRangeException(); }
#if NETFX_45
            MCAResponse response = await Client.SendAsync(MCACommand.SetBias(value, inhibitSignal));
            if (response == null) { throw new MCADeviceLostConnectionException(); }
#endif
        }

        public
#if NETFX_45
            async Task<uint> 
#else
            uint
#endif
            GetHighVoltage()
        {
#if NETFX_45
            QueryPowerResponse response = (QueryPowerResponse) await Client.SendAsync(MCACommand.QueryPower());
            if (response == null) { throw new MCADeviceLostConnectionException(); }
            return response.CurrentHighVoltage;
#else
            return 0;
#endif
        }

        static public DateTime MCA527EpochTime = new DateTime(1969, 12, 31, 16, 0, 0);

		public
#if NETFX_45
            Task
#else
            int
#endif
            Measure(uint sweeps, uint secondsPerSweep)
		{
			return Measure(sweeps, secondsPerSweep, null);
		}

		public
#if NETFX_45
            async Task
#else
            int
#endif
            Measure(uint sweeps, uint secondsPerSweep, string captureToFile, CancellationToken cancellationToken = default(CancellationToken))
        {
#if NETFX_45
			mHeartbeatSemaphore.Wait();
			try {

				MCAResponse response = null;
				response = await Client.SendAsync(MCACommand.Clear(ClearMode.ClearMeasurementData0));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.Clear(ClearMode.ClearMeasurementData1));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetPresets(Presets.Real, secondsPerSweep));
				if (response == null) { throw new MCADeviceLostConnectionException(); }
				response = await Client.SendAsync(MCACommand.SetRepeat(1));
				if (response == null) { throw new MCADeviceLostConnectionException(); }

				for (uint i = 0; i < sweeps; i += 1) {

                    MCAWriter writer = null;
                    if (captureToFile != null) {
                        string outputFile = captureToFile;
                        string extension = Path.GetExtension(outputFile);
                        if (extension != null && extension.Length > 0) {
                            outputFile = Path.ChangeExtension(outputFile, null);
                        } else {
                            extension = "mca";
                        }
                        if (sweeps > 1) {
                            outputFile = outputFile + "#" + i.ToString().PadLeft(4, '0');
                        }
                        outputFile = Path.ChangeExtension(outputFile, extension);                        
                        writer = new MCAWriter(new FileStream(outputFile, FileMode.Create));
                        writer.AdvanceToTimestampsBlock();
                    }


					if (CallbackObject != null) {
						CallbackObject.BeginSweep(i);
					}
					// a5 5a 42 00 01 00 ae d5 44 56 b9 9b
					// flags: 0x0001 => spectrum is cleared and a new start time is set
					// start time: 0x5644d5ae => seconds since Dec 31, 1969, 16:00:00 GMT
					uint secondsSinceEpoch = (uint)(Math.Abs(Math.Round((DateTime.UtcNow - MCA527EpochTime).TotalSeconds)));
  					response = await Client.SendAsync(MCACommand.Start(StartFlag.SpectrumClearedNewStartTime,
																	false, false, false, secondsSinceEpoch));
					if (response == null) { throw new MCADeviceLostConnectionException(); }

					bool done = false;
					const uint CommonMemoryBlockSize = 1440;
					// what's the most that could be left over from a previous attempt to decode? => 3 bytes
					byte[] rawBuffer = new byte[CommonMemoryBlockSize + 3];
					uint[] timestampsBuffer = new uint[CommonMemoryBlockSize + 1];

					uint commonMemoryReadIndex = 0;
					uint rawBufferOffset = 0;

					for (;;) {

                        if (cancellationToken.IsCancellationRequested) {
                            throw new OperationCanceledException();
                        }

						QueryState527ExResponse qs527er = (QueryState527ExResponse) await Client.SendAsync(MCACommand.QueryState527Ex());
						if (qs527er == null) { throw new MCADeviceLostConnectionException(); }

						MCAState state = qs527er.MCAState;
						done = state != MCAState.Run;

						// pull off some data while we are waiting...
						uint commonMemoryFillLevel = qs527er.CommonMemoryFillLevel;
						uint bytesAvailable = commonMemoryFillLevel - commonMemoryReadIndex;

						if (state != MCAState.Run && bytesAvailable == 0) {
							break;
						}

						if (bytesAvailable >= CommonMemoryBlockSize) {
							QueryCommonMemoryResponse qcmr = (QueryCommonMemoryResponse) await Client.SendAsync(MCACommand.QueryCommonMemory(commonMemoryReadIndex / 2));
							if (qcmr == null) { throw new MCADeviceLostConnectionException(); }
							// bytesToCopy needs to always be even, so that commonMemoryReadIndex always stays even...
							uint bytesToCopy = Math.Min(bytesAvailable / 2, CommonMemoryBlockSize / 2) * 2;
							qcmr.CopyData(0, rawBuffer, (int)rawBufferOffset, (int)bytesToCopy);

                            if (writer != null) {
                                writer.WriteTimestampsRawDataChunk(rawBuffer, 0, (int)bytesToCopy);
                            }

							rawBufferOffset += bytesToCopy;
							commonMemoryReadIndex += bytesToCopy;
							uint timestampsCount = TransformRawData(rawBuffer, ref rawBufferOffset, timestampsBuffer);

							// make sure rawBufferOffset is never greater than 3 after transforming data
							// => means something has gone wrong
							if (rawBufferOffset > 3) {
								throw new MCADeviceBadDataException();
							}

							// copy the data out...
							if (timestampsCount > 0) {
								uint[] timestamps = new uint[timestampsCount];
								Array.Copy(timestampsBuffer, 0, timestamps, 0, timestampsCount);
								if (CallbackObject != null) {
									CallbackObject.ReadTimestamps(i, timestamps);
								}
							}
						} else if (bytesAvailable > 0 && state != MCAState.Run) {
							// special case for when there's not a whole block left to read
							// we can only read up to the address: CommonMemorySize - 1440
							uint commonMemorySize = qs527er.CommonMemorySize;

							uint readAddress = commonMemoryReadIndex;
							uint readOffset = 0;
							if (readAddress > commonMemorySize - 1440) {
								readOffset = readAddress - (commonMemorySize - 1440);
								readAddress -= readOffset;
							}

							QueryCommonMemoryResponse qcmr = (QueryCommonMemoryResponse) await Client.SendAsync(MCACommand.QueryCommonMemory(readAddress / 2));
							if (qcmr == null) { throw new MCADeviceLostConnectionException(); }
							uint bytesToCopy = bytesAvailable;
							qcmr.CopyData((int)readOffset, rawBuffer, (int)rawBufferOffset, (int)bytesToCopy);

                            if (writer != null) {
                                writer.WriteTimestampsRawDataChunk(rawBuffer, (int)readOffset, (int)bytesToCopy);
                            }

							rawBufferOffset += bytesToCopy;
							commonMemoryReadIndex += bytesToCopy;                            

							uint timestampsCount = TransformRawData(rawBuffer, ref rawBufferOffset, timestampsBuffer);

							//if (rawBufferOffset > 0) {
                                // apparently this can happen. Perhaps when the device gets cut off (because of a timer event), right in the middle of writing?
								//throw new MCADeviceBadDataException();
							//}
							if (timestampsCount > 0) {
								uint[] timestamps = new uint[timestampsCount];
								Array.Copy(timestampsBuffer, 0, timestamps, 0, timestampsCount);
								if (CallbackObject != null) {
									CallbackObject.ReadTimestamps(i, timestamps);
								}
							}
						} else {
							// give the device a break
							await Task.Delay(100); // 100 ms
						}
					}

					if (CallbackObject != null) {
						QuerySystemDataResponse qsdr = (QuerySystemDataResponse) await Client.SendAsync(MCACommand.QuerySystemData());
						if (qsdr == null) { throw new MCADeviceLostConnectionException(); }

                        if (writer != null) {
                            QueryStateResponse stateResponse = (QueryStateResponse) await Client.SendAsync(MCACommand.QueryState());
                            if (stateResponse == null) { throw new MCADeviceLostConnectionException(); }
                            QueryState527ExResponse state527ExResponse = (QueryState527ExResponse) await Client.SendAsync(MCACommand.QueryState527Ex());
                            QueryState527Response state527Response = (QueryState527Response) await Client.SendAsync(MCACommand.QueryState527());
                            QueryPowerResponse powerResponse = (QueryPowerResponse) await Client.SendAsync(MCACommand.QueryPower());
                            QueryState527Ex2Response state527Ex2Response = (QueryState527Ex2Response) await Client.SendAsync(MCACommand.QueryState527Ex2());
                            MCATimestampsBasisFileBlock basisFileBlock = MCATimestampsBasisFileBlock.Create(stateResponse, state527ExResponse, state527Response, powerResponse, state527Ex2Response);
                            basisFileBlock.DataCodingMethod = 0;
                            basisFileBlock.RepeatMode = (sbyte)(StartFlag.SpectrumClearedNewStartTime);
                            basisFileBlock.RepeatModeOptions = 0;
                            writer.Write(basisFileBlock);
                            writer.Close();
                        }

						uint realTime = qsdr.RealTimeOfPreviousSweep;
						uint realTimeMilliseconds = qsdr.FractionalDigitOfTheRealTimeOfPreviousSweep;
						double sweepDurationSeconds = ((double)realTime + (double)realTimeMilliseconds * 1e-3);

						CallbackObject.FinishedSweep(i, sweepDurationSeconds);
					}
				}
			} catch {
				throw;
			}
			finally {
				mHeartbeatSemaphore.Release();
			}
#else
            return 0;
#endif
        }

        uint TransformRawData(byte[] rawBuffer, ref uint rawBufferIndex, uint[] timestampsBuffer)
		{
			uint offset = 0;
			uint timestampIndex = 0;
			while (offset < rawBufferIndex) {
				uint byteCount = ReadEncodedValue(rawBuffer, offset, out timestampsBuffer[timestampIndex]);
				if (byteCount == 0) { break; }
				timestampIndex += 1;
				offset += byteCount;
			} 
			if (offset > 0 && rawBufferIndex > offset) {
				Array.Copy(rawBuffer, offset, rawBuffer, 0, rawBufferIndex - offset);
			}
			rawBufferIndex -= offset;
			return timestampIndex;
		}


		static uint ReadEncodedValue(byte[] data, uint offset, out uint value)
		{
			int len = data.Length;
			if (offset < len) {
				byte b0 = data[offset];
				if (b0 < 0xC0) {
					value = (uint)b0;
					return 1;
				} else if (b0 < 0xF0) {
					// two bytes
					if (offset + 1 < len) {
						byte b1 = data[offset + 1];
						value = ((((((uint)b0) & 0x3f) << 8) | ((uint)b1))) + 0xC0;
						return 2;
					}
				} else if (b0 < 0xFC) {
					if (offset + 2 < len) {
						byte b1 = data[offset + 1];
						byte b2 = data[offset + 2];
						value = (((((uint)b0) & 0x0f) << 16) | (((uint)b1) << 8) | ((uint)b2)) + 0x30C0;
						return 3;
					}
				} else {
					if (offset + 3 < len) {
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

	enum CommandName : ushort {
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
			foreach (CommandName command in Enum.GetValues(typeof(CommandName))) {
				ValidCommands.Add(command);
			}
		}
		public static HashSet<CommandName> ValidCommands;
	}

	enum StartFlag : ushort {
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
	enum PreamplifierPower : byte {
		Minus24V = 0x80,
		Plus24V = 0x40,
		Minus12V = 0x20,
		Plus12V = 0x10
	};
	enum Mode : byte {
		MCA = 0,
		TransientRecorder = 1,
		Oscilliscope = 2,
		LevelTriggeredTimestampRecorder = 3,
		EdgeTriggeredTimestampRecorder = 4,
		AHRCTimeStampRecorder = 5
	};
	enum Presets : byte {
		None = 0,
		Real = 1,
		Live = 2,
		Int = 3,
		Area = 4,
		RealMilliseconds = 5
	};
	enum InputPolarity : byte {
		Positive = 0,
		Negative = 1
	};
	enum PoleZeroCancellationMode : byte {
		SetPZCOnly = 0,
		SetPZCAndMeasurePZCOffset = 1
	}
	enum TriggerParameter : byte {
		TriggerLevelForAutomaticThresholdCalculation = 0,
		TriggerLevelForAutomaticThresholdCalculationForDirectInput = 1,
		SetTriggerThreshold = 2
	}
	enum ClearMode : byte {
		ClearMeasurementData0 = 0,
		ClearMeasurementData1 = 1,
		ClearROI = 2,
		ClearAll = 3
	}
	public enum BiasInhibitInput : int {
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

		public CommandName CommandName {
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

	class TaskQueue<T>
	{
		Queue<T> Queue = new Queue<T>();
		Queue<TaskCompletionSource<T>> Waiters = new Queue<TaskCompletionSource<T>>();
		public void Enqueue(T item)
		{
			lock (Queue) {
				if (Waiters.Count > 0) {
					TaskCompletionSource<T> waiter = Waiters.Dequeue();
					waiter.SetResult(item);
				} else {
					Queue.Enqueue(item);
				}
			}
		}
		public Task<T> Dequeue()
		{
			TaskCompletionSource<T> taskSource = new TaskCompletionSource<T>();
			lock (Queue) {
				if (Queue.Count > 0) {
					taskSource.SetResult(Queue.Dequeue());
				} else {
					Waiters.Enqueue(taskSource);
				}
			}
			return taskSource.Task;
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
			Receive();
		}

		static List<IPEndPoint> GetBroadcastSendIPs()
		{
			// We want to broadcast on all interfaces. Using IPAddress.Broadcast doesn't work sometimes
			// when there is a network device (router) between us and the MCA device
			// So we iterate over all network interfaces.
			List<IPEndPoint> BroadcastSendIPs = null;
			if (BroadcastSendIPs == null) {
				BroadcastSendIPs = new List<IPEndPoint>();
				foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces()) {
					if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Loopback) { continue; }
					if (networkInterface.OperationalStatus != OperationalStatus.Up) { continue; }
					foreach (UnicastIPAddressInformation unicastAddress in networkInterface.GetIPProperties().UnicastAddresses) {
						// the MCA 527 does IPv4 only
						if (unicastAddress.Address.AddressFamily == AddressFamily.InterNetwork) {
							byte[] maskBytes = unicastAddress.IPv4Mask.GetAddressBytes();
							byte[] addressBytes = unicastAddress.Address.GetAddressBytes();
							byte[] broadcastBytes = new byte[4];
							for (int i = 0; i < 4; i++) {
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
		public IPAddress BoundAddress {
			get { return _BoundAddress; }
			set {
				_BoundAddress = value;
				if (_BoundAddress == null) {
					SendIP = null;
				} else {
					SendIP = new IPEndPoint(_BoundAddress, MCA_PORT_NUMBER);
				}
			}
		}

		List<TaskQueue<MCAResponse>> ResponseQueues = new List<TaskQueue<MCAResponse>>();
#if NETFX_45
        async
#endif
        void Receive()
		{
#if NETFX_45
			while (true) {
				UdpReceiveResult result;
				try {
					result = await Udp.ReceiveAsync();
				} catch (ObjectDisposedException) {
					return;
				}
				IPEndPoint remoteEndPoint = result.RemoteEndPoint;
				byte[] bytes = result.Buffer;
				MCAResponse response = MCAResponse.ParseResponse(bytes);
				if (response != null) {
					response.RemoteAddress = remoteEndPoint.Address;
					// add it to every list in response listeners
					lock(ResponseQueues) {
						foreach (TaskQueue<MCAResponse> queue in ResponseQueues) {
							queue.Enqueue(response);
						}
					}
				}
			}
#endif
        }

        public
#if NETFX_45
        Task<MCAResponse>
#else
         MCAResponse
#endif
        SendAsync(MCACommand command)
        {
            return SendAsync(command, DefaultTimeout);
        }

        public
#if NETFX_45
            Task<MCAResponse>
#else
            MCAResponse
#endif
            SendAsync(MCACommand command, TimeSpan timeout)
        {
            return SendAsync(command, timeout, DefaultRetryCount);
        }

		public
#if NETFX_45
            async Task<MCAResponse>
#else
            MCAResponse
#endif
            SendAsync(MCACommand command, TimeSpan timeout, uint retryCount)
		{
			if (retryCount == 0)
				return null;
			MCAResponse response = null;
			while (response == null && retryCount > 0) {
				response =
#if NETFX_45
            await
#endif
                SendAsyncInternal(command, timeout);
				retryCount -= 1;
			}
			return response;
		}


#if NETFX_45
        async Task<MCAResponse>
#else
        MCAResponse
#endif
        SendAsyncInternal(MCACommand command, TimeSpan timeout)
		{
			if (SendIP == null) {
				return null;
			}
			TaskQueue<MCAResponse> queue = new TaskQueue<MCAResponse>();
			lock (ResponseQueues) {
				ResponseQueues.Add(queue);
			}
#if NETFX_45
            await Udp.SendAsync(command.Bytes, command.Bytes.Length, SendIP);
#endif
            DateTime endTime = DateTime.UtcNow + timeout;
			MCAResponse response = null;
			while (timeout > TimeSpan.Zero) {
				Task<MCAResponse> responseTask = queue.Dequeue();
				if (responseTask.Wait(timeout)) {
					if (responseTask.Result.CommandName == command.CommandName) {
						response = responseTask.Result;
						break;
					}
				}
				timeout = endTime - DateTime.UtcNow;
			}
			lock (ResponseQueues) {
				ResponseQueues.Remove(queue);
			}
			return response;
		}


		public
#if NETFX_45
           async Task<MCAResponse[]>
#else
            MCAResponse[]
#endif
            SendBroadcastAsync(MCACommand command)
			{
				return
				#if NETFX_45
					await 
				#endif
					SendBroadcastAsync(command, new TimeSpan(0, 0, 1));
			}

		public
#if NETFX_45
            async Task<MCAResponse[]> 
#else
            MCAResponse[]
#endif
        SendBroadcastAsync(MCACommand command, TimeSpan timeout)
		{
			TaskQueue<MCAResponse> queue = new TaskQueue<MCAResponse>();
			lock (ResponseQueues) {
				ResponseQueues.Add(queue);
			}
			List<Task<int>> sendTasks = new List<Task<int>>();
#if NETFX_45
			foreach (IPEndPoint sendIP in GetBroadcastSendIPs()) {
				Task<int> sendTask = Udp.SendAsync(command.Bytes, command.Bytes.Length, sendIP);
				sendTasks.Add(sendTask);
			}
			int[] t = await Task.WhenAll(sendTasks);
#endif
            DateTime endTime = DateTime.UtcNow + timeout;
			List<MCAResponse> responseList = new List<MCAResponse>();
			while (timeout > TimeSpan.Zero) {
				Task<MCAResponse> responseTask = queue.Dequeue();
				if (responseTask.Wait(timeout)) {
					MCAResponse response = responseTask.Result;
					if (response.CommandName == command.CommandName) {
						responseList.Add(response);
					}
				}
				timeout = endTime - DateTime.UtcNow;
			}
			lock (ResponseQueues) {
				ResponseQueues.Remove(queue);
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

	class MCABasisFileBlockHeader
	{
		public string FileIdentification;
		public ushort UsedBytesOfTheBasisBlock;
		public ushort FirmwareVersion;
		public ushort HardwareVersion;
		public ushort FirmwareModification;
		public ushort HardwareModification;
		public ushort SerialNumber;
		public Mode GeneralMode;

        public static MCABasisFileBlockHeader Create(
            QueryState527Response state527Response,
            QueryStateResponse stateResponse
        )
        {
            return new MCABasisFileBlockHeader {
                FileIdentification = "MCA527BIN_APP",
                FirmwareVersion = state527Response.MCAFirmwareVersion,
                HardwareVersion = state527Response.MCAHardwareVersion,
                FirmwareModification = state527Response.MCAFirmwareModification,
                HardwareModification = state527Response.MCAHardwareModification,
                SerialNumber = stateResponse.MCASerialNumber,
                GeneralMode = state527Response.GeneralMCAMode
            };
        }
	}

	internal static class MCAFileExtensions
	{

		internal async static Task CreateWriteHeaderAndClose(this MCADevice device, NCCFile.MCAFile file)
		{
			file.Log.TraceEvent(NCCReporter.LogLevels.Verbose, 120, "CreateWriteHeaderAndClose entry");
			QuerySystemDataResponse qsdr = (QuerySystemDataResponse) await device.Client.SendAsync(MCACommand.QuerySystemData());
			if (qsdr == null) { throw new MCADeviceLostConnectionException(); }
            if (file.stream == null)
			{
				file.Log.TraceEvent(NCCReporter.LogLevels.Verbose, 118, "CreateWriteHeaderAndClose reopen");
				// reopen the closed file for this final operation
				file.OpenForWriting();
			}
            if (file.stream != null)
			{
				file.Log.TraceEvent(NCCReporter.LogLevels.Verbose, 119, "CreateWriteHeaderAndClose write header");
                QueryStateResponse stateResponse = (QueryStateResponse) await device.Client.SendAsync(MCACommand.QueryState());
                if (stateResponse == null) { throw new MCADeviceLostConnectionException(); }
                QueryState527ExResponse state527ExResponse = (QueryState527ExResponse) await device.Client.SendAsync(MCACommand.QueryState527Ex());
                QueryState527Response state527Response = (QueryState527Response) await device.Client.SendAsync(MCACommand.QueryState527());
                QueryPowerResponse powerResponse = (QueryPowerResponse) await device.Client.SendAsync(MCACommand.QueryPower());
                QueryState527Ex2Response state527Ex2Response = (QueryState527Ex2Response) await device.Client.SendAsync(MCACommand.QueryState527Ex2());

				file.CreatePrimaryHeader(state527Response,stateResponse);
				file.CreateModeHeader(stateResponse,state527ExResponse,state527Response,powerResponse,state527Ex2Response);				
                file.rheader.DataCodingMethod = 0;
                file.rheader.RepeatMode = (sbyte)(StartFlag.SpectrumClearedNewStartTime);
                file.rheader.RepeatModeOptions = 0;
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
            file.rheader = new NCCFile.MCAFile.MCATimestampsRecorderModeHeader {
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
                // RepeatMode = ??
                // RepeatModeOptions = ??
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
           file.header = new NCCFile.MCAFile.MCAHeader {
                FileIdentification = "MCA527BIN_APP",
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

