/*
The MCA-527 INCC integration source code is Free Open Source Software. It is provided
with NO WARRANTY expressed or implied to the extent permitted by law.

The MCA-527 INCC integration source code is distributed under the New BSD license:

================================================================================

   Copyright (c) 2016, International Atomic Energy Agency (IAEA), IAEA.org
   Authored by J. Longo

   All rights reserved.

   Redistribution and use in source and binary forms, with or without
   modification, are permitted provided that the following conditions are met:

    * Redistributions of source code must retain the above copyright notice,
      this list of conditions and the following disclaimer.
    * Redistributions in binary form must reproduce the above copyright notice,
      this list of conditions and the following disclaimer in the documentation
      and/or other materials provided with the distribution.
    * Neither the name of IAEA nor the names of its contributors
      may be used to endorse or promote products derived from this software
      without specific prior written permission.

   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
   LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR
   A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR
   CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
   EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
   PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
   PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
   LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
   NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
   SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
using System.IO;

namespace Device
{
    /// <summary>
    /// The exception that is thrown when an error occurs communicating with a PTR-32 device.
    /// </summary>
    public class MCA527Exception : IOException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MCA527Error"/> class.
        /// </summary>
        /// <param name="errorCode">The error code.</param>
        public MCA527Exception(MCA527Error errorCode)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public MCA527Error ErrorCode
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
                    case MCA527Error.NoError:         return "No error occurred in transaction.";
                    case MCA527Error.InvParam:        return "Invalid parameter sent in API call.";
                    case MCA527Error.NoMem:           return "Not enough memory to carry out transaction.";
                    case MCA527Error.NotInit:         return "Communication device not initialized.";
                    case MCA527Error.CantConnect:     return "Can't connect to communication module.";
                    case MCA527Error.AlreadyConnect:  return "Already connected to communication device.";
                    case MCA527Error.SendError:       return "Error occurred while sending data to communication device.";
                    case MCA527Error.RcvError:        return "Error occurred while receiving data from communication device.";
                    case MCA527Error.Abort:           return "Error occurred while trying to abort transaction(s).";
                    case MCA527Error.OutOfOrder:      return "Completion out of order.";
                    case MCA527Error.ExtraData:       return "Too much data received from communication device.";
                    case MCA527Error.MissingData:     return "Nothing to send or data/address mismatched pairs.";
                    case MCA527Error.TridNotFound:    return "Unable to find matching TRID in transaction queue.";
                    case MCA527Error.NotComplete:     return "Transaction being cleared is not complete.";
                    case MCA527Error.NotConnected:    return "Not connected to communication device.";
                    case MCA527Error.WrongMode:       return "Connected in wrong mode (JTAG or data transfer).";
                    case MCA527Error.DvctableDne:     return "Device table doesn't exist (an empty one has been created).";
                    case MCA527Error.DvctableCorrupt: return "All or part of the device table is corrupted.";
                    case MCA527Error.DvcDne:          return "Device does not exist in device table.";
                    case MCA527Error.DpcutilInitFail: return "DpcInit API call failed.";
                    case MCA527Error.DvcTableOpen:    return "Communications devices dialog box already open.";
                    case MCA527Error.RegError:        return "Error occurred while accessing the registry.";
                }

                return string.Format("Internal error ({0}).", ErrorCode);
            }
        }
    }
}
