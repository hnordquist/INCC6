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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;

namespace Device
{
    /// <summary>
    /// Takes a measurement from a <see cref="Ptr32"/> and calculate the event rates.
    /// </summary>
    public class Ptr32RateCounter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ptr32RateCounter"/> class.
        /// </summary>
        /// <param name="device">The <see cref="Ptr32"/> device from which to take measurements.</param>
        /// <exception cref="ArgumentNullException"><paramref name="device"/> is <c>null</c>.</exception>
        public Ptr32RateCounter(Ptr32 device)
        {
            if (device == null) {
                throw new ArgumentNullException("device");
            }

            m_device = device;
            m_parser = new Ptr32Parser(Analyze);
        }

        /// <summary>
        /// Gets the number of bytes read.
        /// </summary>
        public long ByteCount
        {
            get { return m_byteCount; }
        }

        /// <summary>
        /// Gets a number of events in each channel.
        /// </summary>
        public ReadOnlyCollection<long> ChannelCounts
        {
            get { return Array.AsReadOnly(m_channelCounts); }
        }

        /// <summary>
        /// Gets the event rate, in events per second, in each channel.
        /// </summary>
        public ReadOnlyCollection<double> ChannelRates
        {
            get { return Array.AsReadOnly(m_channelRates); }
        }

        /// <summary>
        /// Gets the number of events in all channels.
        /// </summary>
        public long Count
        {
            get { return m_count; }
        }

        /// <summary>
        /// Gets the event rate, in events per second, in all channels.
        /// </summary>
        public double Rate
        {
            get { return m_rate; }
        }

        /// <summary>
        /// Gets the length of time measured.
        /// </summary>
        public TimeSpan Time
        {
            get { return m_time; }
        }

        /// <summary>
        /// Takes a measurement of the specified duration and updates the event rates.
        /// </summary>
        /// <param name="duration">The duration of the measurement to take.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to monitor for cancellation requests.</param>
        /// <exception cref="OperationCanceledException">Cancellation was requested.</exception>
        /// <exception cref="Ptr32Exception">An error occurred communicating with the device.</exception>
        public void TakeMeasurement(TimeSpan duration, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024 * 1024];
            Stopwatch stopwatch = new Stopwatch();

            m_device.Reset();
            m_parser.Reset((ulong) duration.Ticks * 10);
            stopwatch.Start();

            while (stopwatch.Elapsed < duration) {
                cancellationToken.ThrowIfCancellationRequested();

                if (m_device.Available > 0) {
                    int bytesRead = m_device.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0) {
                        m_byteCount += bytesRead;
                        m_parser.Parse(buffer, 0, bytesRead);
                    }
                }

                Thread.Yield();
            }

            stopwatch.Stop();
            m_parser.Flush();

            m_time += stopwatch.Elapsed;
            double totalSeconds = m_time.TotalSeconds;

            if (totalSeconds > 0) {
                for (int i = 0; i < Ptr32.ChannelCount; i++) {
                    m_channelRates[i] = m_channelCounts[i] / totalSeconds;
                }

                m_rate = m_count / totalSeconds;
            }
            else {
                Array.Clear(m_channelRates, 0, Ptr32.ChannelCount);
                m_rate = 0;
            }
        }

        /// <summary>
        /// Resets the event rates.
        /// </summary>
        public void Reset()
        {
            m_byteCount = 0;
            Array.Clear(m_channelCounts, 0, m_channelCounts.Length);
            Array.Clear(m_channelRates, 0, m_channelRates.Length);
            m_count = 0;
            m_rate = 0;
            m_time = TimeSpan.Zero;
        }

        /// <summary>
        /// Analyzes the specified events.
        /// </summary>
        /// <param name="times">The event times, in device clock ticks.</param>
        /// <param name="channelMasks">The event channel masks.</param>
        /// <param name="count">The number of events.</param>
        private void Analyze(List<ulong> times, List<uint> channelMasks, int count)
        {
            for (int i = 0; i < count; i++) {
                for (int channel = 0; channel < Ptr32.ChannelCount; channel++) {
                    if ((channelMasks[i] & (1u << channel)) != 0) {
                        m_channelCounts[channel]++;
                        m_count++;
                    }
                }
            }
        }

        private long m_byteCount;
        private long[] m_channelCounts = new long[Ptr32.ChannelCount];
        private double[] m_channelRates = new double[Ptr32.ChannelCount];
        private long m_count;
        private Ptr32 m_device;
        private Ptr32Parser m_parser;
        private double m_rate;
        private TimeSpan m_time;
    }
}
