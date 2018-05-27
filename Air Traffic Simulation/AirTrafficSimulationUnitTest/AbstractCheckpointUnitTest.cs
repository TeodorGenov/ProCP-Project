using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Air_Traffic_Simulation;

namespace AirTrafficSimulationUnitTest
{

    [TestClass]
    public class AbstractCheckpointUnitTest
    {
       
        [TestMethod]
        public void CalculateDistanceBetweenPointsTest()
        {
            Airplane test = new Airplane("Test", 10, 10, 100, "t1");
            Airplane test2 = new Airplane("Test2", 6, 6, 100, "t2");
            //Assert.AreEqual(1, test.calcu)
        }
    }
}
