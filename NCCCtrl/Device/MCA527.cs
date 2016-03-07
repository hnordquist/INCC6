/*
The MCA-527 INCC integration source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

The MCA-527 INCC integration source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2016, International Atomic Energy Agency (IAEA), IAEA.org
   Authored by J. Longo

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
using System.Text;

namespace Device
{
    /// <summary>
    /// Represents a 32-channel Pulse Train Recorder (PTR-32) device.
    /// </summary>
    public class MCA527 : IDisposable
    {
        /// <summary>
        /// The number of channels.
        /// </summary>
        public const int ChannelCount = 1;

        /// <summary>
        /// The clock frequency in hertz (Hz).
        /// </summary>
        public const long Frequency = 100000000;

        /// <summary>
        /// The maximum voltage in volts (V).
        /// </summary>
        public const int MaxVoltage = 2000;

        /// <summary>
        /// Initializes static members of the <see cref="MCA527"/> class.
        /// </summary>
        static MCA527()
        {

        }

        /// <summary>
        /// Gets the names of the devices registered with the system.
        /// </summary>
        /// <returns>The names of the devices registered with the system.</returns>
        public static IEnumerable<string> GetDeviceNames()
        {
            return null;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MCA527"/> class.
        /// </summary>
        /// <param name="deviceName">The name of the device to open.</param>
        /// <exception cref="ArgumentNullException"><paramref name="deviceName"/> is <c>null</c>.</exception>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
        public MCA527(string deviceName)
        {
            if (deviceName == null)
            {
                throw new ArgumentNullException();
            }

            m_deviceName = deviceName;
            m_firmwareVersion = GetFirmwareVersion();
            m_voltageCorrections = GetVoltageCorrections();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MCA527"/> class.
        /// </summary>
        ~MCA527()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the number of bytes available to be read.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The device has been closed.</exception>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
        public int Available
        {
            get
            {
                const int MainBufferSize = 1 << 24;
                const int SplitterBufferSize = 1 << 11;

                CheckDisposed();
                BinaryReader reader = GetRegisterReader(2);

                if (m_firmwareVersion[18] <= '2')
                {
                    // V2 or lower
                    int mainBufferCount = reader.ReadInt32();
                    int splitterBufferCount = reader.ReadInt32();

                    return Math.Max(0, mainBufferCount + splitterBufferCount - 4);
                }
                else
                {
                    // V3 or higher
                    int mainBufferReadOffset = reader.ReadInt32();
                    int mainBufferWriteOffset = reader.ReadInt32();
                    int mainBufferCount = (mainBufferWriteOffset - mainBufferReadOffset + MainBufferSize) % MainBufferSize;
                    int splitterBufferReadOffset = reader.ReadInt16();
                    int splitterBufferWriteOffset = reader.ReadInt16();
                    int splitterBufferCount = (splitterBufferWriteOffset - splitterBufferReadOffset + SplitterBufferSize) % SplitterBufferSize;
                    reader.ReadInt64(); // Clock count
                    reader.ReadInt16(); // Voltage

                    return Math.Max(0, mainBufferCount + splitterBufferCount - 4);
                }
            }
        }

        /// <summary>
        /// Gets the device name.
        /// </summary>
        public string DeviceName
        {
            get { return m_deviceName; }
        }

        /// <summary>
        /// Gets a string identifying the firmware version.
        /// </summary>
        public string FirmwareVersion
        {
            get { return m_firmwareVersion; }
        }

        /// <summary>
        /// Gets a string identifying the firmware version.
        /// </summary>
        public double[] VoltageCorrectionFactors
        {
            get { return this.m_voltageCorrections; }
        }
        /// <summary>
        /// Gets the actual voltage in volts (V).
        /// </summary>
        /// <exception cref="ObjectDisposedException">The device has been closed.</exception>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
        public int Voltage
        {
            get
            {
                CheckDisposed();
                int rawValue = GetRegisterReader(4).ReadInt16() & 0x0FFF;
                return (int)Math.Round(m_voltageCorrections[0] * 1.024 * rawValue + m_voltageCorrections[1]);
            }
        }

        /// <summary>
        /// Closes the device and releases any associated resources.
        /// </summary>
        public void Close()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="MCA527"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="MCA527"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!m_disposed)
            {
                if (disposing)
                {
                    foreach (BinaryReader reader in m_readers.Values)
                    {
                        reader.Dispose();
                    }

                    foreach (BinaryWriter writer in m_writers.Values)
                    {
                        writer.Dispose();
                    }
                }

                m_disposed = true;
            }
        }


        // devnote: About the data buffer József Huszti Jun 9, 2015.
        // There are two buffers for transmitting data. The main buffer has a capacity 
        // of 16MByte (16777216). It stores 16 bit data. There is a small buffer after it 
        // just before the 8bit transfer interface, which converts 16 bit values to 8 bit bytes. 
        // This has a capacity of 2048 bytes. Both buffers are working as FIFOs. They can 
        // hold together a maximum of 16M+2k-4 bytes. If the buffers are full, no more values 
        // are accepted from the input stage.

        /// <summary>
        /// Reads a sequence of bytes from the device.
        /// </summary>
        /// <param name="buffer">The byte array in which to store the bytes read.</param>
        /// <param name="offset">The zero-based offset in <paramref name="buffer"/> at which to begin storing the bytes read.</param>
        /// <param name="count">The maximum number of bytes to read.</param>
        /// <returns>The number of bytes read. This can be less than the number of bytes requested or zero if no data is available.</returns>
        /// <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
        /// <exception cref="ObjectDisposedException">The device has been closed.</exception>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
        public int Read(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException();
            }

            if (offset < 0 || count < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (offset + count > buffer.Length)
            {
                throw new ArgumentException();
            }

            CheckDisposed();
            int bytesToRead = Math.Min(Available, count);

            if (bytesToRead > 0)
            {
                return GetRegisterReader(1).Read(buffer, offset, bytesToRead);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Resets the device.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The device has been closed.</exception>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
        public void Reset()
        {
            CheckDisposed();
            GetRegisterWriter(0).Write((byte)0);
        }


        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the device has been closed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">The device has been closed.</exception>
        protected void CheckDisposed()
        {
            if (m_disposed)
            {
                throw new ObjectDisposedException(null);
            }
        }

        /// <summary>
        /// Gets a <see cref="BinaryReader"/> that reads from the specified device register.
        /// </summary>
        /// <param name="register">The device register to read from.</param>
        /// <returns>A <see cref="BinaryReader"/> that reads from the specified device register.</returns>
        protected BinaryReader GetRegisterReader(byte register)
        {
            BinaryReader reader = null;
            return reader;
        }

        /// <summary>
        /// Gets a <see cref="BinaryWriter"/> that writes to the specified device register.
        /// </summary>
        /// <param name="register">The device register to write to.</param>
        /// <returns>A <see cref="BinaryWriter"/> that writes to the specified device register.</returns>
        protected BinaryWriter GetRegisterWriter(byte register)
        {
            BinaryWriter writer = null;


            return writer;
        }

        /// <summary>
        /// Gets a string identifying the firmware version.
        /// </summary>
        /// <returns>A string identifying the firmware version.</returns>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
        private string GetFirmwareVersion()
        {
			string s =  Encoding.ASCII.GetString(GetRegisterReader(3).ReadBytes(32));
            return s.TrimEnd('\0', ' ');
        }

        /// <summary>
        /// Gets the voltage corrections factors.
        /// </summary>
        /// <returns>The voltage correction factors.</returns>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
        private double[] GetVoltageCorrections()
        {
            double[] voltageCorrections = new double[] { 1, 0 };
            string str = Encoding.ASCII.GetString(GetRegisterReader(6).ReadBytes(32)).TrimEnd('\0', ' ');
            string[] strings = str.Split(new char[] { ' ' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (strings.Length >= 2)
            {
                Double.TryParse(strings[0], out voltageCorrections[0]);
                Double.TryParse(strings[1], out voltageCorrections[1]);
            }

            return voltageCorrections;
        }

        /// <summary>
        /// Implements a stream that can read from or write to a device register.
        /// </summary>
        private class RegisterStream : Stream
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RegisterStream"/> class.
            /// </summary>
            /// <param name="handle">The interface handle.</param>
            /// <param name="register">The device register to read from or write to.</param>
            public RegisterStream(IntPtr handle, byte register)
            {
                m_handle = handle;
                m_register = register;
            }

            /// <summary>
            /// Gets a value indicating whether the stream supports reading.
            /// </summary>
            public override bool CanRead
            {
                get { return true; }
            }

            /// <summary>
            /// Gets a value indicating whether the stream supports seeking.
            /// </summary>
            public override bool CanSeek
            {
                get { return false; }
            }

            /// <summary>
            /// Gets a value indicating whether the stream supports writing.
            /// </summary>
            public override bool CanWrite
            {
                get { return true; }
            }

            /// <summary>
            /// Gets the length in bytes of the stream.
            /// </summary>
            /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
            public override long Length
            {
                get { throw new NotSupportedException(); }
            }

            /// <summary>
            /// Gets or sets the position within the stream.
            /// </summary>
            /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
            public override long Position
            {
                get { throw new NotSupportedException(); }
                set { throw new NotSupportedException(); }
            }

            /// <summary>
            /// Clears all buffers for the stream and causes any buffered data to be written to the underlying device.
            /// </summary>
            public override void Flush()
            {
                // Do nothing
            }

            /// <summary>
            /// Reads a sequence of bytes from the stream.
            /// </summary>
            /// <param name="buffer">The byte array in which to store the bytes read.</param>
            /// <param name="offset">The zero-based offset in <paramref name="buffer"/> at which to begin storing the bytes read.</param>
            /// <param name="count">The maximum number of bytes to read.</param>
            /// <returns>The number of bytes read. This can be less than the number of bytes requested or zero if no data is available.</returns>
            /// <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <c>null</c>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
            public override unsafe int Read(byte[] buffer, int offset, int count)
            {
                if (buffer == null)
                {
                    throw new ArgumentNullException();
                }

                if (offset < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (offset + count > buffer.Length)
                {
                    throw new ArgumentException();
                }


                return count;
            }

            /// <summary>
            /// Sets the position within the stream.
            /// </summary>
            /// <param name="offset">A byte offset relative to <paramref name="origin"/>.</param>
            /// <param name="origin">A value of type <see cref="SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
            /// <returns>The new position within the stream.</returns>
            /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
            public override long Seek(long offset, SeekOrigin origin)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Sets the length of the stream.
            /// </summary>
            /// <param name="value">The desired length of the stream in bytes.</param>
            /// <exception cref="NotSupportedException">The stream does not support seeking.</exception>
            public override void SetLength(long value)
            {
                throw new NotSupportedException();
            }

            /// <summary>
            /// Writes a sequence of bytes to the stream.
            /// </summary>
            /// <param name="buffer">The byte array containing the bytes to write.</param>
            /// <param name="offset">The zero-based offset in <paramref name="buffer"/> at which to begin writing bytes.</param>
            /// <param name="count">The number of bytes to write.</param>
            /// <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the buffer length.</exception>
            /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <c>null</c>.</exception>
            /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
		/// <exception cref="MCADeviceLostConnectionException">An error occurred communicating with the device.</exception>
            public override unsafe void Write(byte[] buffer, int offset, int count)
            {
                if (buffer == null)
                {
                    throw new ArgumentNullException();
                }

                if (offset < 0 || count < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                if (offset + count > buffer.Length)
                {
                    throw new ArgumentException();
                }

            }

            private IntPtr m_handle;
            private byte m_register;
        }

        private string m_deviceName;
        private bool m_disposed;
        private string m_firmwareVersion;
        private IDictionary<byte, BinaryReader> m_readers = new Dictionary<byte, BinaryReader>();
        private double[] m_voltageCorrections;
        private IDictionary<byte, BinaryWriter> m_writers = new Dictionary<byte, BinaryWriter>();
    }
}
