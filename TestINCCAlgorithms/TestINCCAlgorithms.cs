using System;
using System.Data;
using System.Data.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestINCCAlgorithms
{
    
    [TestClass]

    public class TestINCCAlgorithms
    {
        
        [TestMethod]
        public void GetConfig()
        {
            System.Data.Common.DbConnection conn = DB.DBMain.CreateConnection();
            //conn.
        }
        [TestMethod]
        
        public void TestGetFromDB()
        {
          
        }
        [TestMethod]
        public void CycleConditioning()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();

        }
        [TestMethod]
        public void TestPassiveCalibrationCurveINCC5INCC6()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();
        }
        [TestMethod]
        public void TestActiveCalibrationCurveINCC5INCC6()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();

        }
        [TestMethod]
        public void TestPassiveMultiplictyINCC5INCC6()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();

        }
        [TestMethod]
        public void TestActiveMultiplictyINCC5INCC6()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();

        }
        [TestMethod]
        public void TestCmRatioINCC5INCC6()
        {
            return;
        }
        [TestMethod]
        public void TestActivePassiveINCC5INCC6()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();

        }
        [TestMethod]
        public void TestKnownMINCC5INCC6()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();

        }
        [TestMethod]
        public void TestCollarINCC5INCC6()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();

        }
        [TestMethod]
        public void TestKnownAlphaINCC5INCC6()
        {
            NCCConfig.Config cfg = new NCCConfig.Config();

        }
    }
}
