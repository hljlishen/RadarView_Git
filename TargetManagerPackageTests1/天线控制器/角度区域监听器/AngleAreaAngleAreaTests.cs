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
        public void MakeAngle0To360Test()
        {
            float angle = -730.7f;
            float expected = 349.3f;
            Assert.AreEqual(AngleArea.MakeAngle0To360(angle), expected);

            angle = 55.8f;
            expected = 55.8f;
            Assert.AreEqual(AngleArea.MakeAngle0To360(angle), expected);

            //angle = 360.1f;
            //expected = 0.1f;
            //Assert.AreEqual(AngleArea.MakeAngle0To360(angle), expected);
        }
    }
}