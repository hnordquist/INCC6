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
using System.IO;

namespace Device
{
    /// <summary>
    /// The exception that is thrown when an error occurs communicating with a PTR-32 device.
    /// </summary>
    public class Ptr32Exception : IOException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ptr32Exception"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        public Ptr32Exception(Ptr32Error errorCode)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public Ptr32Error ErrorCode
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public override string Message
        {
            get
            {
                // The error messages are taken from "Digilent Port Communications Programmer's Reference Manual" (2005-06-03).
                switch (ErrorCode) {
                    case Ptr32Error.NoError:         return "No error occurred in transaction.";
                    case Ptr32Error.InvParam:        return "Invalid parameter sent in API call.";
                    case Ptr32Error.NoMem:           return "Not enough memory to carry out transaction.";
                    case Ptr32Error.NotInit:         return "Communication device not initialized.";
                    case Ptr32Error.CantConnect:     return "Can't connect to communication module.";
                    case Ptr32Error.AlreadyConnect:  return "Already connected to communication device.";
                    case Ptr32Error.SendError:       return "Error occurred while sending data to communication device.";
                    case Ptr32Error.RcvError:        return "Error occurred while receiving data from communication device.";
                    case Ptr32Error.Abort:           return "Error occurred while trying to abort transaction(s).";
                    case Ptr32Error.OutOfOrder:      return "Completion out of order.";
                    case Ptr32Error.ExtraData:       return "Too much data received from communication device.";
                    case Ptr32Error.MissingData:     return "Nothing to send or data/address mismatched pairs.";
                    case Ptr32Error.TridNotFound:    return "Unable to find matching TRID in transaction queue.";
                    case Ptr32Error.NotComplete:     return "Transaction being cleared is not complete.";
                    case Ptr32Error.NotConnected:    return "Not connected to communication device.";
                    case Ptr32Error.WrongMode:       return "Connected in wrong mode (JTAG or data transfer).";
                    case Ptr32Error.DvctableDne:     return "Device table doesn't exist (an empty one has been created).";
                    case Ptr32Error.DvctableCorrupt: return "All or part of the device table is corrupted.";
                    case Ptr32Error.DvcDne:          return "Device does not exist in device table.";
                    case Ptr32Error.DpcutilInitFail: return "DpcInit API call failed.";
                    case Ptr32Error.DvcTableOpen:    return "Communications devices dialog box already open.";
                    case Ptr32Error.RegError:        return "Error occurred while accessing the registry.";
                }

                return string.Format("Internal error ({0}).", ErrorCode);
            }
        }
    }
}
