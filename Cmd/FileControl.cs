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
using NCC;
using NCCFile;
using NCCReporter;
namespace NCCCmd
{

    using NC = NCC.CentralizedState;

    // The control API is simple: CancelCurrentAction();StopCurrentAction();StartAction();
    //
    // Seven events can be subscribed to using an instance of the ActionEvents delegate (delegate void ActionChurn(object o))
    //    PreAction, ActionPrep, ActionStart, ActionInProgress, ActionStop, ActionCancel, ActionFinished
    // Each handler provides the primary object via the object parameters, here you can get a LMFile.FileCtrl
    // New handlers can be applied as shown below

    // Progress is tracked using the event handlers and /or the IActionStatus timer callbacks
    //
    // For LMFile.FileCtrl, four callbacks are offered for the unique processing steps of a file-based task
    //   Filegather = PreActionStatus;  // param is the FileListState
    //   NeutronCountingPrep = ActionPreparation; // no param
    //   Filerawprocessing = ActionInProgressStatus;   param is an Instrument
    //   Postprocessing = FurtherActionInProgressStatus; param is a Measurement
    //
    // the timer callback params are subject to change (Feb 21, 2011)

    /// <summary>
    /// Class for controlling file processing from a UI or other controlling client code
    /// </summary>
    public class FileControlBind : NCCFile.FileCtrl
    {

        public FileControlBind()
            : base(false)
        {
        }


        public void SetupEventHandlers()
        {
            LMLoggers.LognLM applog = NC.App.AppLogger;
            /// set up the 7 magic event handlers
            /// here I have a base handler that does the same thing for every event (writes a string to the log) 
            SetEventHandler(ActionEvents.EventType.PreAction, (object o) =>
             {
                 string s = FileCtrl.LoggableFileProcessingStatus(ActionEvents.EventType.PreAction, applog, LogLevels.Verbose, o);
             });

            SetEventHandler(ActionEvents.EventType.ActionPrep, (object o) =>
             {
                 string s = FileCtrl.LoggableFileProcessingStatus(ActionEvents.EventType.ActionPrep, applog, LogLevels.Verbose, o);
             });

            SetEventHandler(ActionEvents.EventType.ActionStart, (object o) =>
             {
                 string s = FileCtrl.LoggableFileProcessingStatus(ActionEvents.EventType.ActionStart, applog, LogLevels.Verbose, o);
             });

            SetEventHandler(ActionEvents.EventType.ActionInProgress, (object o) =>
             {
                 string s = FileCtrl.LoggableFileProcessingStatus(ActionEvents.EventType.ActionInProgress, applog, LogLevels.Verbose, o);
             });

            SetEventHandler(ActionEvents.EventType.ActionStop, (object o) =>
             {
                 string s = FileCtrl.LoggableFileProcessingStatus(ActionEvents.EventType.ActionStop, applog, LogLevels.Warning, o);
             });

            SetEventHandler(ActionEvents.EventType.ActionCancel, (object o) =>
             {
                 string s = FileCtrl.LoggableFileProcessingStatus(ActionEvents.EventType.ActionCancel, applog, LogLevels.Warning, o);
             });

            SetEventHandler(ActionEvents.EventType.ActionFinished, (object o) =>
             {
                 FileCtrl f = (FileCtrl)o;
                 if (o == null)
                 {
                     String s = "Finished: SOH " + NC.App.Opstate.SOH + " but no processing occurred";
                     applog.TraceEvent(LogLevels.Verbose, FileCtrl.logid[ActionEvents.EventType.ActionFinished], s);
                 }
                 else
                 {
                     FileCtrl.LoggableFileProcessingStatus(ActionEvents.EventType.ActionFinished, applog, LogLevels.Verbose, o);
                 }
                 NC.App.Opstate.SOH = NCC.OperatingState.Stopped;  // in case we got here after a Cancel
                 NC.App.Loggers.Flush();
             });

            NC.App.Opstate.SOH = NCC.OperatingState.Stopped;
        }

        // controller has active control of processing through these three entry points
        public new void StartAction()
        {
            base.StartAction();
            Run();
        }

        public new void CancelCurrentAction()
        {
            base.CancelCurrentAction();
        }

        public new void StopCurrentAction()
        {
            base.StopCurrentAction();
        }
    }

} 
