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
    public class PolarCoordinateTests
    {
        [TestMethod()]
        public void BytesToFloatTest()
        {
            byte[] value = { 0xbb, 0xaa };
            float expected = 4804.2f;

            Assert.AreEqual(expected, PolarCoordinate.BytesToFloat(value, 1));
        }

        [TestMethod()]
        public void FloatToBytesTest()
        {
            float value = 4804.2f;
            byte[] data = PolarCoordinate.FloatToBytes(value, 1);
            byte[] expected = {0xbb, 0xaa};

            Assert.AreEqual(expected[0], data[0]);
            Assert.AreEqual(expected[1], data[1]);
        }
    }
}