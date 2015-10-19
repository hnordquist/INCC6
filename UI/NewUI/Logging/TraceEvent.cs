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
using System.Diagnostics;

namespace NewUI.Logging
{
    /// <summary>
    /// Provides information about a trace event.
    /// </summary>
    public class TraceEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceEvent"/> class.
        /// </summary>
        /// <param name="eventCache">The <see cref="TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">The name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">The numeric identifier for the event.</param>
        /// <param name="message">The message.</param>
        public TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            DateTime = eventCache.DateTime;
            ThreadId = eventCache.ThreadId;
            Timestamp = eventCache.Timestamp;
            Source = source;
            EventType = eventType;
            Id = id;
            Message = message;
        }

        /// <summary>
        /// Gets the date and time at which the event trace occurred.
        /// </summary>
        public DateTime DateTime
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a unique identifier for the current managed thread.
        /// </summary>
        public string ThreadId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the current number of ticks in the timer mechanism.
        /// </summary>
        public long Timestamp
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the name used to identify the output, typically the name of the application that generated the trace event.
        /// </summary>
        public string Source
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets one of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.
        /// </summary>
        public TraceEventType EventType
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the numeric identifier for the event.
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        public string Message
        {
            get;
            private set;
        }

        public override string ToString()
        {
            return Message;
        }
    }
}
