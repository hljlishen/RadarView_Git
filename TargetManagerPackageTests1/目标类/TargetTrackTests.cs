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
    public class TargetTrackTests
    {
        [TestMethod()]
        public void AdjustHeigthToTest()
        {
            PolarCoordinate c = new PolarCoordinate(30, 30, 100);

            TargetTrack.AdjustHeigthTo(80, c);

            Assert.AreEqual(30, c.Az);
            //Assert.AreEqual(c.Dis, 117.898262023926);
            Assert.AreEqual(c.El, 42.7313);
        }
    }
}