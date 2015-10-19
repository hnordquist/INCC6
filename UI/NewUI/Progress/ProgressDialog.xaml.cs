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
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

using NCC;
using System.Collections;

namespace NewUI.Progress
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <exception cref="ArgumentNullException"><paramref name="task"/> is <c>null</c>.</exception>
        private ProgressDialog(Task task)
        {
            if (task == null) {
                throw new ArgumentNullException();
            }

            InitializeComponent();
            task.ContinueWith(_ => Close(), TaskScheduler.FromCurrentSynchronizationContext());
        }

        /// <summary>
        /// Displays a progress dialog for the specified task.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="title">The title of the task progress dialog.</param>
        /// <param name="description">The description of the task.</param>
        /// <param name="task">The task.</param>
        /// <exception cref="ArgumentNullException"><paramref name="description"/> or <paramref name="task"/> is <c>null</c>.</exception>
        public static void Show(Window owner, string title, string description, Task task)
        {
            Show(owner, title, description, task, null, null, false);
        }

        /// <summary>
        /// Displays a progress dialog for the specified task. The dialog allows the user to cancel
        /// the task and displays the task's progress.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="title">The title of the task progress dialog.</param>
        /// <param name="description">The description of the task.</param>
        /// <param name="task">The task.</param>
        /// <param name="cancelStopAbortTokensSource">The cancellation token source that cancels the task.</param>
        /// <param name="progressTracker">The progress tracker that tracks the task's progress.</param>
        /// <exception cref="ArgumentNullException"><paramref name="description"/> or <paramref name="task"/> is <c>null</c>.</exception>
        public static void Show(Window owner, string title, string description, Task task, CancelStopAbort cancellationTokenSource)
        {
            Show(owner, title, description, task, cancellationTokenSource, null, false);
        }

        /// <summary>
        /// Displays a progress dialog for the specified task. The dialog allows the user to cancel
        /// the task and displays the task's progress.
        /// </summary>
        /// <param name="owner">The owner window.</param>
        /// <param name="title">The title of the task progress dialog.</param>
        /// <param name="description">The description of the task.</param>
        /// <param name="task">The task.</param>
        /// <param name="cancelStopAbortTokensSource">The cancellation token source that cancels the task.</param>
        /// <param name="progressTracker">The progress tracker that tracks the task's progress.</param>
        /// <exception cref="ArgumentNullException"><paramref name="description"/> or <paramref name="task"/> is <c>null</c>.</exception>
        public static void Show(Window owner, string title, string description, Task task, CancelStopAbort cancelStopAbortTokensSource, ProgressTracker progressTracker, bool isAssay)
        {
            ProgressDialog dialog = new ProgressDialog(task);
            //Disable close button, as it doesn't work.  HN 6.15.2015
            //dialog.WindowStyle = WindowStyle.None;
            dialog.Topmost = true;
            dialog.DataContext = new TaskProgress(title, description, task, cancelStopAbortTokensSource, progressTracker, isAssay);
            dialog.Owner = owner;
            if (!isAssay)
            {

            }
            if (progressTracker.m_modal)
                dialog.ShowDialog();
            else
                dialog.Show();
        }

    }
}
