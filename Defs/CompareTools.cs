using System;
using System.Collections.Generic;

namespace AnalysisDefs
{
    public static class CompareTools
    {

        public static bool DoublesCompare(double a, double b)
        {
            if (a == b)
                return true;
            else
                return Math.Abs(a - b) <= .001;
        }
        public static bool DoublesArraysCompare(double[] a, double[] b)
        {
            bool same = true;
            for (int i = 0; i < a.Length; i++)
            {
                same = same && DoublesCompare(a[i], b[i]);
            }
            return same;
        }
        public static bool DoublesTupleCompares(VTuple t1, VTuple t2)
        {
            return DoublesCompare(t1.v, t2.v) && DoublesCompare(t1.err, t2.err);
        }
        public static bool ULongArraysCompare(ulong[] a, ulong[] b)
        {
            if (a.Length != b.Length)
                return false;
            bool same = true;
            for (int i = 0; i < a.Length; i++)
                same = same && a[i] == b[i];
            return same;
        }
    }

}

