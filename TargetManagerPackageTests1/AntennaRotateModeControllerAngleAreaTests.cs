using Microsoft.VisualStudio.TestTools.UnitTesting;
using TargetManagerPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntennaControlPackage;

namespace TargetManagerPackage.Tests
{
    [TestClass()]
    public class AntennaRotateModeControllerAngleAreaTests
    {
        [TestMethod()]
        public void SetSweepModeDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void StartSweepTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReverseSweepDirectionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReversedDirectionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSweepModeTest()
        {
            AntennaRotateController controller = new AntennaRotateController();
            RotateDirection direction = RotateDirection.ClockWise;
            RotateRate rate = RotateRate.Rpm10;
            RotateMode expectedMode = RotateMode.ClockWise10;
            Assert.AreEqual(expectedMode, AntennaRotateController.GetSweepMode(direction, rate));

            direction = RotateDirection.CounterClockWise;
            rate = RotateRate.Rpm10;
            expectedMode = RotateMode.CounterClockWise10;
            Assert.AreEqual(expectedMode, AntennaRotateController.GetSweepMode(direction, rate));
        }
    }
}