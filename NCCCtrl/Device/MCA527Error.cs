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
namespace Device
{
    /// <summary>
    /// Specifies errors that can occur when communicating with a MCA-527 device.
    /// </summary>
    public enum MCA527Error
    {
        NoError         = 0,
        ConnReject      = 3001,
        ConnType        = 3002,
        ConnNoMode      = 3003,
        InvParam        = 3004,
        InvCmd          = 3005,
        Unknown         = 3006,
        JtagConflict    = 3007,
        NotImp          = 3008,
        NoMem           = 3009,
        Timeout         = 3010,
        Conflict        = 3011,
        BadPacket       = 3012,
        InvOption       = 3013,
        AlreadyCon      = 3014,
        Connected       = 3101,
        NotInit         = 3102,
        CantConnect     = 3103,
        AlreadyConnect  = 3104,
        SendError       = 3105,
        RcvError        = 3106,
        Abort           = 3107,
        TimeOut         = 3108,
        OutOfOrder      = 3109,
        ExtraData       = 3110,
        MissingData     = 3111,
        TridNotFound    = 3201,
        NotComplete     = 3202,
        NotConnected    = 3203,
        WrongMode       = 3204,
        WrongVersion    = 3205,
        DvctableDne     = 3301,
        DvctableCorrupt = 3302,
        DvcDne          = 3303,
        DpcutilInitFail = 3304,
        UnknownErr      = 3305,
        DvcTableOpen    = 3306,
        RegError        = 3307,
        NotifyRegFull   = 3308,
        NotifyNotFound  = 3309,
        OldDriverNewFw  = 3310,
        InvHandle       = 3311
    }
}
