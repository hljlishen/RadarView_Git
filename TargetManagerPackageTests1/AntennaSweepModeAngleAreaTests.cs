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
    public class AntennaSweepModeAngleAreaTests
    {
        [TestMethod()]
        public void AntennaSweepModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void InitializeSweepModeDataTest()
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
            RotateRate rate = RotateRate.Rpm5;
            RotateDirection direction = RotateDirection.ClockWise;
            RotateMode expectedMode = RotateMode.ClockWise5;
            Assert.AreEqual(expectedMode, AntennaRotateModeController.GetSweepMode(direction, rate));

            rate = RotateRate.Rpm0;
            direction = RotateDirection.ClockWise;
            expectedMode = RotateMode.Stop;
            Assert.AreEqual(expectedMode, AntennaRotateModeController.GetSweepMode(direction, rate));

            rate = RotateRate.Rpm0;
            direction = RotateDirection.CounterClockWise;
            expectedMode = RotateMode.Stop;
            Assert.AreEqual(expectedMode, AntennaRotateModeController.GetSweepMode(direction, rate));

            rate = RotateRate.Rpm10;
            direction = RotateDirection.CounterClockWise;
            expectedMode = RotateMode.CounterClockWise10;
            Assert.AreEqual(expectedMode, AntennaRotateModeController.GetSweepMode(direction, rate));
        }
    }
}