using System;
using AnalysisDefs;
using NCCReporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestINCCAlgorithms
{
    [TestClass]
    public class TestINCCAlgorithms
    {
        NCCReporter.LMLoggers.LognLM testLog;
        public String rootLoc;
        Measurement testMeas;
        Measurement temp;
        Detector det;
        Multiplicity mult;
        AlphaBeta ab;
        NCCConfig.Config cfg;

        [TestMethod]
        public void Init()
        {
            //Set up our detector info.....
            DetectorDefs.DataSourceIdentifier dsid = new DetectorDefs.DataSourceIdentifier();
            dsid.DetectorName = "TEST";
            dsid.Type = "TEST";

            //Multiplicity params
            mult = new Multiplicity(FAType.FAOn);
            //Alpha Beta array
            ab = new AlphaBeta();

            det = new Detector(dsid, mult, ab);

            int pid = 1111;
            cfg = new NCCConfig.Config("c:\\code\\incc6\\deployment\\INCC6.exe.config");
            testLog = new NCCReporter.LMLoggers.LognLM("Test",null,pid);
            testMeas = new Measurement(AssaySelector.MeasurementOption.calibration, testLog);

            //Created the measurement to put info into for test case

        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
