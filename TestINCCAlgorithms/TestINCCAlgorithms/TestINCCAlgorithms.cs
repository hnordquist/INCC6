using System;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestINCCAlgorithms
{
    [TestClass]
    public class TestINCCAlgorithms
    {
        String provider = "System.Data.SQLite";
        String connString = "Data Source='c:\\code\\incc6\\deployment\\INCC6.sqlite';Version=3;New=False;Compress=True;PRAGMA foreign_keys=on;";
        String dbFile = "incc6.sqlite";
        public void GetConfig()
        {
            NCC.CentralizedState.App.Config.DB.MyDBConnectionString = connString;
            NCC.CentralizedState.App.Config.DB.MyProviderName = provider;
            NCC.CentralizedState.App.DB.GetMeasurementCounts("XX/YY/XXXX");
        }
        [TestMethod]
        public void TestMethod1()
        {
            GetConfig();
        }
    }
}
