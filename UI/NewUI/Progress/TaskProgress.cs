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
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using NCC;

namespace NewUI.Progress
{
    /// <summary>
    /// Wraps a <see cref="Task"/>, <see cref="CancellationTokenSource"/>, and
    /// <see cref="ProgressTracker"/>.
    /// </summary>
    public class TaskProgress : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskProgress"/> class with the specified
        /// description, task, cancellation token source, and progress tracker.
        /// </summary>
        /// <param name="title">The title for the progess pop-up.</param>
        /// <param name="description">The description of the task.</param>
        /// <param name="task">The task.</param>
        /// <param name="cancellationTokenSource">The cancellation token source that cancels the task.</param>
        /// <param name="progressTracker">The progress tracker that tracks the task's progress.</param>
        /// <exception cref="ArgumentNullException"><paramref name="description"/> or <paramref name="task"/> is <c>null</c>.</exception>
        public TaskProgress(string title, string description, Task task, CancelStopAbort cancelStopAbortTokensSource, ProgressTracker progressTracker, bool isAssay/*, string rates*/)
        {
            if (description == null || task == null) {
                throw new ArgumentNullException();
            }

            m_stopCommand = new CancelCommand(cancelStopAbortTokensSource.StopTokenSource, 0);
            m_cancelCommand = new CancelCommand(cancelStopAbortTokensSource.CancellationTokenSource, 1);
            m_abortCommand = new CancelCommand(cancelStopAbortTokensSource.AbortTokenSource, 2);  // make an enum!

            m_description = description;
            m_isProgressIndeterminate = true;
            m_progress = 0;
            m_progressTracker = progressTracker;
            m_task = task;
            m_title = title;
            m_isAssay = isAssay;
            //m_rates = rates;

            if (m_progressTracker != null) {
                m_progressTracker.ProgressChanged += (sender, e) => {
                    IsProgressIndeterminate = false;
                    Progress = e.ProgressPercentage;
                    Description = (string)e.UserState;
                };
            }
        }

        /// <summary>
        /// Gets the command that cancels the task.
        /// </summary>
        public ICommand CancelCommand
        {
            get { return m_cancelCommand; }
        }
        public ICommand  StopCommand
        {
            get { return m_stopCommand; }
        }
        public ICommand AbortCommand
        {
            get { return m_abortCommand; }
        }
        /// <summary>
        /// Gets the description of the task.
        /// </summary>
        public string Description
        {
            get { return m_description; }
            private set
            {
                if (m_description.CompareTo(value) != 0)
                {
                    m_description = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Description"));
                }
            }
        }

        /// <summary>
        /// Gets the title of the task
        /// </summary>
        public string Title
        {
            get { return m_title; }
        }

        /*public string Rates
        {
            get { return m_rates; }
            set { m_rates = value; OnPropertyChanged(new PropertyChangedEventArgs("CycleRates")); }
        }*/

        public string CancelContent
        {
            get { return (m_isAssay ? "Abort measurement" : "Cancel"); }
        }

        public string QuitVisibility
        { 
            get { return (m_isAssay ? "Visible" : "Hidden"); }
        }
        public string CancelVisibility
        {
            get { return "Visible"; }
        }
        /// <summary>
        /// Gets whether the task's progress is indeterminate.
        /// </summary>
        public bool IsProgressIndeterminate
        {
            get { return m_isProgressIndeterminate; }
            private set
            {
                if (value != m_isProgressIndeterminate) {
                    m_isProgressIndeterminate = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("IsProgressIndeterminate"));
                }
            }
        }

        /// <summary>
        /// Gets the task's progress.
        /// </summary>
        public int Progress
        {
            get { return m_progress; }
            private set
            {
                if (value != m_progress) {
                    m_progress = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Progress"));
                }
            }
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="PropertyChangedEventArgs"/> containing data for the event.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null) {
                handler(this, e);
            }
        }

        private ICommand m_cancelCommand, m_stopCommand, m_abortCommand;
        private string m_description;
        private string m_title = "Frob-nitz";
        private bool m_isProgressIndeterminate, m_isAssay;
        private int m_progress;
        private ProgressTracker m_progressTracker;
        private Task m_task;
        //private string m_rates;
    }
}
