using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestAlgorithms
{
    [TestClass]
    public class TestAvgSum
    {


        AnalysisDefs.MultiplicityCountingRes summcr = new AnalysisDefs.MultiplicityCountingRes();
        AnalysisDefs.Rates ratessquared = new AnalysisDefs.Rates();
        double scaler1_err = 0;
        double scaler2_err = 0;
        double num_good_runs = 0;
        AnalysisDefs.CycleList cycles = new AnalysisDefs.CycleList();
        AnalysisDefs.Multiplicity mkey = new AnalysisDefs.Multiplicity();

        /*[TestMethod]
        public void TestSumCyclesZeroCyclesReturnsZero()
        {
            //arrange
            summcr.ASum = 0;
            summcr.RASum = 0;
            summcr.rates = new AnalysisDefs.Rates();
            double number_good_runs = 0;
            TimeSpan TS = new TimeSpan(10);

            AnalysisDefs.Cycle c1 = new AnalysisDefs.Cycle(null);
            cycles.Add(c1);
            
            //action
            INCCCore.INCCAnalysis.SumCycles(cycles, mkey, ref summcr, ref ratessquared, new AnalysisDefs.RatesAdjustments(), ref number_good_runs,ref TS );

            //assert
            Assert.AreEqual(summcr, new AnalysisDefs.MultiplicityCountingRes());
        }

        [TestMethod]
        public void TestAverageRatesOverCyclesReturnsAverage()
        {
            //arrange
            summcr.ASum = 0;
            summcr.RASum = 0;
            summcr.rates = new AnalysisDefs.Rates();

            //action
            INCCCore.INCCAnalysis.CalculateAverageRates(ref summcr, ref ratessquared, ref scaler1_err, ref scaler2_err, num_good_runs, new TimeSpan());

            //assert
        }
        [TestMethod]
        public void TestCalculateRatesErrorSample()
        {
            //arrange
            summcr.ASum = 0;
            summcr.RASum = 0;
            summcr.rates = new AnalysisDefs.Rates();

            //action
            INCCCore.INCCAnalysis.CalculateAverageRates(ref summcr, ref ratessquared, ref scaler1_err, ref scaler2_err, num_good_runs, new TimeSpan());

            //assert
        }
        [TestMethod]
        public void TestCalculateCovariance()
        {
            //arrange
            summcr.ASum = 0;
            summcr.RASum = 0;
            summcr.rates = new AnalysisDefs.Rates();

            //action
            INCCCore.INCCAnalysis.CalculateAverageRates(ref summcr, ref ratessquared, ref scaler1_err, ref scaler2_err, num_good_runs, new TimeSpan());

            //assert
        }*/

    }
}
