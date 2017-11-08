using AnalysisDefs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using NCCTransfer;

namespace TestINCCAlgorithms
{
    [TestClass]
    public class TestINCCAlgorithms
    {
        }

        [TestMethod]
        public void TestMethod1()
        {
            Setup();
            LoadMeasurementFromFile("C:\\CODE\\INCC6 Test Suite\\Test INCCAlgorithms\\data\\713P0048.VER");
        }
    }
}
