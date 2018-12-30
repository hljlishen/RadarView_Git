using Microsoft.VisualStudio.TestTools.UnitTesting;
using TargetManagerPackage.目标类;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage.目标类.Tests
{
    [TestClass()]
    public class CirculateStrategyTests
    {
        private CirculateStrategy strategy = new CirculateStrategy();

        private void Setup()
        {
            for(int i = 0; i < FindTrackIdStrategy.TrackMaximumCount; i++)
            {
                strategy.NextId();
            }
        }
        [TestMethod()]
        public void NextIdTest()
        {
            Setup();
            strategy.ReleaseId(10);
            strategy.NextId();
            strategy.ReleaseId(1);
            strategy.ReleaseId(20);
            int id = strategy.NextId();

            Assert.AreEqual(id, 20);

            id = strategy.NextId();
            Assert.AreEqual(id, 1);

            id = strategy.NextId();
            Assert.AreEqual(id, 0);

            id = strategy.NextId();
            Assert.AreEqual(id, 0);

            strategy.ReleaseId(102);
            Assert.AreEqual(strategy.NextId(), 102);

            strategy.ReleaseId(103);
            Assert.AreEqual(strategy.NextId(), 103);

            strategy.ReleaseId(102);
            Assert.AreEqual(strategy.NextId(), 102);
        }
    }
}