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
using NCCReporter;
using System;
using System.Collections.Generic;

namespace Device
{
    /// <summary>
    /// Parses raw data from a PTR-32 device.
    /// </summary>
    public class MCA527Parser
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MCA527Parser"/> class.
        /// </summary>
        /// <param name="handler">An action to perform after parsing a block of events.</param>
        /// <exception cref="ArgumentNullException"><paramref name="handler"/> is <c>null</c>.</exception>
        public MCA527Parser(Action<List<ulong>, List<uint>, int> handler)
        {
            if (handler == null) {
                throw new ArgumentNullException();
            }

            m_handler = handler;
            m_eventTimeBuffer = new List<ulong>(EventBufferSize);
            m_eventTimeBuffer.AddRange(new ulong[EventBufferSize]);
            m_eventChannelMaskBuffer = new List<uint>(EventBufferSize);
            m_eventChannelMaskBuffer.AddRange(new uint[EventBufferSize]);
        }

        /// <summary>
        /// Resets the parser.
        /// </summary>
        /// <param name="timeLimit">The time limit in clock ticks. Events that occur after this time will be discarded.</param>
        public void Reset(ulong timeLimit)
        {
            m_timeLimit = timeLimit;
            m_eventCount = 0;
            m_value = 0;
            m_shift = 0;
            m_time = 0;
            m_channelMask = 0;
        }

        /// <summary>
        /// Parses raw data from a MCA527Parser device.
        /// </summary>
        /// <param name="buffer">The buffer containing the raw data to parse.</param>
        /// <param name="offset">The zero-based offset in <paramref name="buffer"/> at which to start parsing.</param>
        /// <param name="count">The maximum number of bytes to parse.</param>
        /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> or <paramref name="count"/> is negative.</exception>
        /// <exception cref="ArgumentException">The sum of <paramref name="offset"/> and <paramref name="count"/> is greater than the length of <paramref name="buffer"/>.</exception>
        public void Parse(byte[] buffer, int offset, int count)
        {
            if (buffer == null) {
                throw new ArgumentNullException();
            }

            if (offset < 0 || count < 0) {
                throw new ArgumentOutOfRangeException();
            }

            if (offset + count > buffer.Length) {
                throw new ArgumentException();
            }



            // The PTR-32's raw data stream is a sequence of variable-length integers (varints),
            // one for each event. See https://developers.google.com/protocol-buffers/docs/encoding
            // for detailed information on the encoding. With the PTR-32, a maximum of six bytes
            // can be used to encode a varint, resulting in a maximum of 42 bits in the decoded
            // value. Of these, the lowest five bits are the channel number, the next 32 bits are
            // a relative time stamp, and the highest five bits are unused.
            for (int i = 0; i < count; i++) {
                byte b = buffer[offset + i];

                if (m_shift < MaxShift) {
                    // Decode the next byte
                    m_value |= ((ulong) (b & 0x7F)) << m_shift;
                    m_shift += 7;

                    if ((b & 0x80) == 0) {
                        // Varint is complete, extract time and channel number
                        ulong time = m_value >> 5;
                        int channel = (int) (m_value & 0x1F);

                        if (time > 0) {
                            // If multiple channels record a hit in the same clock period, the
                            // PTR-32 reports these as multiple events; INCC, on the other hand,
                            // wants all channel hits in a single event. So we have to wait until
                            // we see a non-zero time increment before we can be sure the event is
                            // complete.
                            if (m_time < m_timeLimit) {
                                // init condition 
                                if (m_time == 0ul) m_time = time;
                                if (m_channelMask == 0u) m_channelMask |= 1u << channel;
                                m_eventTimeBuffer[m_eventCount] = m_time;
                                m_eventChannelMaskBuffer[m_eventCount] = m_channelMask;
                                m_eventCount++;

                                if (m_eventCount == EventBufferSize) {
                                    // Buffer is full, send events to handler
                                    m_handler(m_eventTimeBuffer, m_eventChannelMaskBuffer, m_eventCount);
                                    m_eventCount = 0;
                                }
                            }

                            // Reset for next event
                            m_time += time;
                            m_channelMask = 0;
                        }

                        // Record channel hit in current event channel mask
                        m_channelMask |= 1u << channel;

                        // Reset for next varint
                        m_value = 0;
                        m_shift = 0;
                    }
                }
                else {
                    // Exceeded maximum size of varint, attempt to resync
                    if ((b & 0x80) == 0) {
                        m_value = 0;
                        m_shift = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Flushes any buffered events.
        /// </summary>
        public void Flush()
        {
            if (m_channelMask != 0) {
                // Write the last event to the buffer
                if (m_time < m_timeLimit) {
                    m_eventTimeBuffer[m_eventCount] = m_time;
                    m_eventChannelMaskBuffer[m_eventCount] = m_channelMask;
                    m_eventCount++;
                }

                m_channelMask = 0;
            }

            if (m_eventCount > 0) {
                // Send events to handler
                m_handler(m_eventTimeBuffer, m_eventChannelMaskBuffer, m_eventCount);
                m_eventCount = 0;
            }
        }

        private const int EventBufferSize = 1024 * 1024;
        private const int MaxShift = 6 * 7;

        private Action<List<ulong>, List<uint>, int> m_handler;
        private ulong m_timeLimit = ulong.MaxValue;
        private List<ulong> m_eventTimeBuffer;
        private List<uint> m_eventChannelMaskBuffer;
        private int m_eventCount;

        private ulong m_value;
        private int m_shift;
        private ulong m_time;
        private uint m_channelMask;
    }
}
