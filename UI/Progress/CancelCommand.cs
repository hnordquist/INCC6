﻿/*
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
using System.Windows.Forms;
using System.Windows.Input;

namespace UI.Progress
{
    /// <summary>
    /// Adapts a <see cref="CancellationTokenSource"/> to the WPF <see cref="ICommand"/> interface.
    /// </summary>
    public class CancelCommand : ICommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelCommand"/> class.
        /// </summary>
        /// <param name="cancellationTokenSource">The cancellation token source, which may be <c>null</c>.</param>
        public CancelCommand(CancellationTokenSource cancellationTokenSource, int id)
        {
            m_cancellationTokenSource = cancellationTokenSource;
            m_id = id;

            if (m_cancellationTokenSource != null) {
                m_cancellationTokenSource.Token.Register(() => {
                    OnCanExecuteChanged(EventArgs.Empty);
                }, true);
            }
        }

        /// <summary>
        /// Determines whether this command can be executed.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        /// <returns><c>true</c> if this command can be executed; otherwise, <c>false</c>.</returns>
        public bool CanExecute(object parameter)
        {
            return m_cancellationTokenSource != null && !m_cancellationTokenSource.IsCancellationRequested;
        }

        /// <summary>
        /// Executes this command.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        public void Execute(object parameter)
        {
            if (m_cancellationTokenSource != null) {
                if (m_id == 2)  // this needs an enum
                {
                    DialogResult r = MessageBox.Show("Do you want to abort this process?", "Please", MessageBoxButtons.YesNo);
                    if (r == DialogResult.No)
                        return;
                }
                m_cancellationTokenSource.Cancel();
            }
        }

        /// <summary>
        /// Occurs when changes occur that affect whether this command can be executed.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises the <see cref="CanExecuteChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/> that contains data for the event.</param>
        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            EventHandler handler = CanExecuteChanged;

            if (handler != null) {
                handler(this, e);
            }
        }

        private CancellationTokenSource m_cancellationTokenSource;
        private int m_id;
    }
}
