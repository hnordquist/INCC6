using System;
using AnalysisDefs;
using NCCReporter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestINCCAlgorithms
{
    using Integ = NCC.IntegrationHelpers;
    using N = NCC.CentralizedState;
    [TestClass]
    public class TestINCCAlgorithms
    {
        public NCCConfig.Config c;

        [TestMethod]
        public void Init()
        {
            Console.WriteLine("Loading configuration file");
            c = new NCCConfig.Config(); // gets DB params
            if (!N.App.LoadPersistenceConfig(c.DB)) // loads up DB, sets global AppContext
                Console.WriteLine("Could not load configuration.");
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}
