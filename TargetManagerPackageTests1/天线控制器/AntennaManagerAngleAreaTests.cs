using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TargetManagerPackage.Tests
{
    [TestClass()]
    public class AntennaManagerAngleAreaTests
    {
        private AntennaManager antenna;
        public void AntennaManagerTest()
        {
            //Assert.Fail();
            antenna = new AntennaManager();
        }

        [TestMethod()]
        public void ConnectDataSourceTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NotifyNewCycleDataTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RegisterObserverTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UnregisterObserverTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void NotifyChangeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetCurrentAntennaAngleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetSectionSweepModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetNormalSweepModeTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SetRotationRateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSweepBeginAngleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSweepEndAngleTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSweepStateTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void SwitchDirectionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetAntennaDirectionTest()
        {
            antenna = new AntennaManager
            {
                AntennaCurrentAngle = 10f,
                AntennaPreviousAngle = 9f
            };

            //普通顺时针
            AntennaDirection expecteDirection = AntennaDirection.ClockWise;
            Assert.AreEqual(expecteDirection, antenna.GetAntennaDirection());

            //普通逆时针
            antenna.AntennaCurrentAngle = 8.99f;
            antenna.AntennaPreviousAngle = 9f;
            expecteDirection = AntennaDirection.CounterClockWise;
            Assert.AreEqual(expecteDirection, antenna.GetAntennaDirection());

            //跨越360逆时针
            antenna.AntennaCurrentAngle = 359.99f;
            antenna.AntennaPreviousAngle = 0.001f;
            expecteDirection = AntennaDirection.CounterClockWise;
            Assert.AreEqual(expecteDirection, antenna.GetAntennaDirection());

            //跨越360顺时针
            antenna.AntennaCurrentAngle = 0.000f;
            antenna.AntennaPreviousAngle = 359.99f;
            expecteDirection = AntennaDirection.ClockWise;
            Assert.AreEqual(expecteDirection, antenna.GetAntennaDirection());
        }

        [TestMethod()]
        public void NotifyLeaveAngleAreaTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ReversedDirectionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void RegisterSweepModeObserverTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UnregisterSweepModeObserverTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetSweepSectionTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsSectionSweepingTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetRotationRateTest()
        {
            antenna = new AntennaManager();
            AntennaDirection direction = AntennaDirection.ClockWise;
            uint count = 2;
             
        }
    }
}