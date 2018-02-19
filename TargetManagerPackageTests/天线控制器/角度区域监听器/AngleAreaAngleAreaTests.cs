using Microsoft.VisualStudio.TestTools.UnitTesting;
using TargetManagerPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage.Tests
{
    [TestClass()]
    public class AngleAreaAngleAreaTests
    {
        [TestMethod()]
        public void AngleAreaTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void IsZeroDegreeAcrossingAngleAreaTest()
        {
            AngleArea area = new AngleArea(359.9f, 10);
            bool ret = AngleArea.IsZeroDegreeAcrossingAngleArea(area);
            Assert.AreEqual(ret, true);
            //Assert.Fail();
        }

        [TestMethod()]
        public void SpliteNorthAcrossingAngleAreaTest()
        {
            //Assert.Fail();
        }

        [TestMethod()]
        public void IsAngleInAreaTest()
        {
            Assert.Fail();
        }
    }
}