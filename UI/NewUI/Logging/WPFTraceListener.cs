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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows;

namespace NewUI.Logging
{
    /// <summary>
    /// Provides an implementation of <see cref="TraceListener"/> that stores events in an <see cref="ObservableCollection"/>.
    /// </summary>
    public class WPFTraceListener : TraceListener
    {
        /// <summary>
        /// The maximum number of events to store in the collection. Earlier events will be removed in order to make room for new ones.
        /// </summary>
        public const int MaxEventCount = 1000;

        /// <summary>
        /// Initializes an instance of the <see cref="WPFTraceListener"/> class.
        /// </summary>
        public WPFTraceListener()
            : base()
        {
            m_events = new ObservableCollection<TraceEvent>();
            m_readOnlyEvents = new ReadOnlyObservableCollection<TraceEvent>(m_events);
        }

        /// <summary>
        /// Initializes an instance of the <see cref="WPFTraceListener"/> class with the specified name.
        /// </summary>
        /// <param name="name">The listener's name.</param>
        public WPFTraceListener(string name)
            : base(name)
        {
            m_events = new ObservableCollection<TraceEvent>();
            m_readOnlyEvents = new ReadOnlyObservableCollection<TraceEvent>(m_events);
        }

        /// <summary>
        /// Gets the events written to the listener.
        /// </summary>
        public ReadOnlyObservableCollection<TraceEvent> Events
        {
            get { return m_readOnlyEvents; }
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The trace data to emit.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            string message;

            if (data != null) {
                message = data.ToString();
            }
            else {
                message = string.Empty;
            }

            TraceEvent(eventCache, source, eventType, id, message);
        }

        /// <summary>
        /// Writes trace information, an array of data objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">An array of objects to emit as data.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            StringBuilder messageBuilder = new StringBuilder();

            if (data != null) {
                for (int i = 0; i < data.Length; i++) {
                    if (i != 0) {
                        messageBuilder.Append(", ");
                    }

                    if (data[i] != null) {
                        messageBuilder.Append(data[i].ToString());
                    }
                }
            }

            TraceEvent(eventCache, source, eventType, id, messageBuilder.ToString());
        }

        /// <summary>
        /// Writes trace and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            TraceEvent(eventCache, source, eventType, id, string.Empty);
        }

        /// <summary>
        /// Writes trace information, a message, and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="message">A message to write.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            TraceEvent traceEvent = new TraceEvent(eventCache, source, eventType, id, message);

            if (Application.Current != null) {
                Application.Current.Dispatcher.BeginInvoke(
                    new Action<TraceEvent>((e) => {
                        while (m_events.Count >= MaxEventCount) {
                            m_events.RemoveAt(0);
                        }

                        m_events.Add(e);
                    }),
                    traceEvent);
            }
        }

        /// <summary>
        /// Writes trace information, a formatted array of objects and event information to the listener specific output.
        /// </summary>
        /// <param name="eventCache">A <see cref="TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="format">A format string that contains zero or more format items, which correspond to objects in the <paramref name="args"/> array.</param>
        /// <param name="args">An <c>object</c> array containing zero or more objects to format.</param>
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            string message;

            if (args != null) {
                message = string.Format(CultureInfo.InvariantCulture, format, args);
            }
            else {
                message = format;
            }

            TraceEvent(eventCache, source, eventType, id, message);
        }

        /// <summary>
        /// Writes the specified message to the listener.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            // Do nothing
        }

        /// <summary>
        /// Writes the specified message to the listener, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            // Do nothing
        }

        private ObservableCollection<TraceEvent> m_events;
        private ReadOnlyObservableCollection<TraceEvent> m_readOnlyEvents;
    }
}
