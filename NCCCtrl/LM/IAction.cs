/*
 * 186846
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
namespace NCC
{
         // olde-fashioned callbacks, called by timers
    public interface IActionStatus
    {
        // an action is an HV Calib operation or a Measurement "assay", or others yet unspecified

        // gather files into a list, e.g. the "1177", waiting for a detector to respond, querying for data across nodes or from a raw file tree)
        void PreActionStatusTimerCB(object o);
        // set up and do intensive pre-processing on data, parsing and counting NCD files and DAQ assay intervals, pre-processing cycles from a DB query into the "neutron mine")
        void ActionPreparationTimerCB(object o);
        // the obvious, update an action while it is occuring
        void ActionInProgressStatusTimerCB(object o);

    }

    /// <summary>
    ///  controller has active control of processing through these three entry points
    /// </summary>
    public interface IActionControl
    {

        void CancelCurrentAction();

        void StopCurrentAction();

        void StartAction();

    }


    public abstract class ActionEvents 
    {
        // an action is an HV Calib operation or a Measurement "assay", or others yet unspecified
        public enum EventType { PreAction, ActionPrep, ActionStart, ActionInProgress, ActionStop, ActionCancel, ActionFinished }

        // new-fangled events
        Dictionary<EventType, Action<object>> handlers;

        /* e.g.
        OnPreActionEvent; getting config details

        OnActionPreparationEvent; is doing the file gather

        OnActionStartEvent; start the file cycle parsing loop, start up the analyzer threads, start the cycle conditioning, start the post-analysis

        OnActionInProgressEvent; doing the file by file cycle parsing/analyzing loop

        OnActionStopEvent; user or internal state stops "Preaction, Preparation, Start, in progress", finish the current action and move on to Finished step

        OnActionCancelEvent; user or internal state cancels, this is a stop, with no closure

        OnActionFinishedEvent;
*/

        public ActionEvents()
        {
            handlers = new Dictionary<EventType, Action<object>>();
        }


        public void SetEventHandler(EventType et, Action<object> ac)
        {
            handlers.Remove(et);
            handlers.Add(et, ac);
        }

        public void FireEvent(EventType et, object o)
        {
            Action<object> ac = null;
            handlers.TryGetValue(et, out ac);
            if (ac != null)
                ac(o);
        }

        public Action<object> GetEventHandler(EventType et)
        {
            return handlers[et];
        }

        /// <summary>
        /// Map from event types to a fixed log id
        /// </summary>
        static readonly public Dictionary<EventType, int> logid = new Dictionary<EventType, int>(){
                            {EventType.PreAction, 66710},{EventType.ActionPrep, 66711},
                            {EventType.ActionStart, 66712},{EventType.ActionInProgress, 66713},
                            {EventType.ActionStop, 66714},{EventType.ActionCancel, 66715},
                            {EventType.ActionFinished, 66716}};



    }
        

}
