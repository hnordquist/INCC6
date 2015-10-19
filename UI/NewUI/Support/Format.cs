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

namespace NewUI
{
    static class Format
    {

        private const string Sci = "G4";
        private const string F2 = "F2";
        static public String Rend(Int32 d)
        {
            return d.ToString();
        }
        static public String Rend(Int64 d)
        {
            return d.ToString();
        }
        static public String Rend(Double d)
        {
            return d.ToString();
        }

        static public String Rend(Single d)
        {
            return d.ToString();
        }
        static public String Rend(UInt64 d)
        {
            return d.ToString();
        }
        static public String Rend(UInt32 d)
        {
            return d.ToString();
        }
        static public String Rend(UInt16 d)
        {
            return d.ToString();
        }
        static public String Rend(Int16 d)
        {
            return d.ToString();
        }
        static public String Rend(Byte d)
        {
            return d.ToString();
        }

        // Convert a string to a positive, non-zero double if possible
        // If the value is legal, place it into the TargetValue reference and return true.  Otherwise, return false.

        static public Boolean ToPNZ(String s, ref Double TargetValue)
        {
            Double NewValue;
            if (Double.TryParse(s, out NewValue))
            {
                if (NewValue > 0.0 && NewValue != TargetValue)
                {
                    TargetValue = NewValue;
                    return true;
                }
            }
            return false;
        }

        static public Boolean ToPNZ(String s, ref ushort TargetValue)
        {
            ushort NewValue;
            if (ushort.TryParse(s, out NewValue))
            {
                if (NewValue > 0.0 && NewValue != TargetValue)
                {
                    TargetValue = NewValue;
                    return true;
                }
            }
            return false;
        }


        static public Boolean ToPNZ(String s, ref int TargetValue)
        {
            int NewValue;
            if (int.TryParse(s, out NewValue))
            {
                if (NewValue > 0.0 && NewValue != TargetValue)
                {
                    TargetValue = NewValue;
                    return true;
                }
            }
            return false;
        }


        // Convert a string to a double between 0.0 and 100.0, inclusive, if possible.
        // If the value is legal, place it into the TargetValue reference and return true.  Otherwise, return false.
        static public Boolean ToPct(String s, ref Double TargetValue)
        {
            Double NewValue;
            if (Double.TryParse(s, out NewValue))
            {
                if (NewValue >= 0.0 && NewValue <= 100.0 && NewValue != TargetValue)
                {
                    TargetValue = NewValue;  // any range checking can go here
                    return true;
                }
            }
            return false;
        }

        // Convert a string to a non-negative double, if possible.
        // If the value is legal, place it into the TargetValue reference and return true.  Otherwise, return false.
        static public Boolean ToNN(String s, ref Double TargetValue)
        {
            Double NewValue;
            if (Double.TryParse(s, out NewValue))
            {
                if (NewValue >= 0.0 && NewValue != TargetValue)
                {
                    TargetValue = NewValue;  // any range checking can go here
                    return true;
                }
            }
            return false;
        }

        // Convert a string to a non-negative uint, if possible.
        // If the value is legal, place it into the TargetValue reference and return true.  Otherwise, return false.
        static public Boolean ToNN(String s, ref uint TargetValue)
        {
            uint NewValue;
            if (uint.TryParse(s, out NewValue))
            {
                if (NewValue >= 0.0 && NewValue != TargetValue)
                {
                    TargetValue = NewValue;  // any range checking can go here
                    return true;
                }
            }
            return false;
        }

        static public Boolean ToInt(String s, ref int TargetValue)
        {
            int NewValue;
            if (int.TryParse(s, out NewValue) && NewValue != TargetValue)
            {
                    TargetValue = NewValue;  // any range checking can go here
                    return true;
            }
            return false;
        }

        static public Boolean ToUInt(String s, ref uint TargetValue)
        {
            uint NewValue;
            if (uint.TryParse(s, out NewValue) && NewValue != TargetValue)
            {
                TargetValue = NewValue;  // any range checking can go here
                return true;
            }
            return false;
        }


        /// <summary>
        /// Convert a string to a non-negative integer, if possible.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="TargetValue"></param>
        /// <returns> If the value is legal, place it into the TargetValue reference and return true. Otherwise, return false.</returns>
        static public Boolean ToNZInt(String s, ref int TargetValue)
        {
            int NewValue;
            if (int.TryParse(s, out NewValue) && NewValue != TargetValue)
            {
                TargetValue = NewValue;  // any range checking can go here
                return true;
            }
            return false;
        }

        /// <summary>
        /// Convert a string to a non-negative ushort, if possible.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="TargetValue"></param>
        /// <returns> If the value is legal, place it into the TargetValue reference and return true. Otherwise, return false.</returns>
        static public Boolean ToNN(String s, ref ushort TargetValue)
        {
            ushort NewValue;
            if (ushort.TryParse(s, out NewValue))
            {
                if (NewValue >= 0.0 && NewValue != TargetValue)
                {
                    TargetValue = NewValue;  // any range checking can go here
                    return true;
                }
            }
            return false;
        }
        static public Boolean ToNN(String s, ref ulong TargetValue)
        {
            ulong NewValue;
            if (ulong.TryParse(s, out NewValue))
            {
                if (NewValue >= 0.0 && NewValue != TargetValue)
                {
                    TargetValue = NewValue;
                    return true;
                }
            }
            return false;
        }

        static public Boolean ToDbl(String s, ref Double TargetValue, double low = double.MinValue, double high = double.MaxValue)
        {
            Double NewValue;
            if (Double.TryParse(s, out NewValue))
            {
                if ((NewValue > low && NewValue < high) && NewValue != TargetValue)
                {
                    TargetValue = NewValue;
                    return true;
                }
            }
            return false;
        }


        static public Boolean ToInt32(String s, ref Int32 TargetValue, Int32 low = Int32.MinValue, Int32 high = Int32.MaxValue)
        {
            Int32 NewValue;
            if (Int32.TryParse(s, out NewValue))
            {
                if ((NewValue >= low && NewValue <= high) && NewValue != TargetValue)
                {
                    TargetValue = NewValue;
                    return true;
                }
            }
            return false;
        }

        static public Boolean ToUInt64(String s, ref UInt64 TargetValue, UInt64 low = UInt64.MinValue, UInt64 high = UInt64.MaxValue)
        {
            UInt64 NewValue;
            if (UInt64.TryParse(s, out NewValue))
            {
                if ((NewValue >= low && NewValue <= high) && NewValue != TargetValue)
                {
                    TargetValue = NewValue;
                    return true;
                }
            }
            return false;
        }

        public enum WTF { Good, NonNumeric, Range, Plotz }
        static public WTF ToUInt64(ref string text, UInt64 low = UInt64.MinValue, UInt64 high = UInt64.MaxValue)
        {
            UInt64 d = 0;
            bool modified = UInt64.TryParse(text, out d);
            if (modified)
            {
                if (d >= low && d <= high)
                {
                    text = d.ToString();
                    return WTF.Good;
                }
                else
                    return WTF.Range;
            }
            return WTF.NonNumeric;
        }

        static public Boolean Changed(String s, ref String TargetValue)
        {
            if (!s.Equals(TargetValue))
            {
                    TargetValue = s;
                    return true;
            }
            return false;
        }
    }
}
