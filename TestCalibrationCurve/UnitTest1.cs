using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AnalysisDefs;

namespace TestCalibrationCurve
{
    [TestClass]
    public class TestCalibrationCurve
    {
        public Measurement temp;
        public MeasurementTuple tuple;

        [TestMethod]
        public void RetrieveCalMeasFromDB()
        {
            // Prepare measurement
            AcquireParameters ap = null;
            AssaySelector.MeasurementOption mo = AssaySelector.MeasurementOption.calibration;
            AcquireParameters acq = new AcquireParameters();
            Detector det = new Detector();
            INCCDB.AcquireSelector sel = new INCCDB.AcquireSelector(det, ap.item_type, DateTime.Now);

            temp = new Measurement(mo,null);

        }
    }
}
