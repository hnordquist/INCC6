using System;
using AnalysisDefs;
using NCCReporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestINCCAlgorithms
{
    using Integ = NCC.IntegrationHelpers;
    using theDB = DB;
    [TestClass]
    public class TestINCCAlgorithms
    {
        
        public String rootLoc;
        Measurement temp;
        Detector det;

        [TestMethod]
        public void Init()
        {
            //Set up our test environment
            DB.DBMain.SwitchDB("c:\\code\\incc\\deployment\\incc6.sqlite");
            
            Integ.SetNewCurrentDetector("test", true);
            AcquireParameters ap = new AcquireParameters();
            ap.acquire_type = AcquireConvergence.CycleCount;
            ap.data_src = DetectorDefs.ConstructedSource.CycleFile;

            temp = Integ.BuildMeasurementTemp(ap, new Detector(), AssaySelector.MeasurementOption.rates);

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
