using Microsoft.VisualStudio.TestTools.UnitTesting;
using CycleDataDrivePackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage.Tests
{
    [TestClass()]
    public class DistanceCellTests
    {
        [TestMethod()]
        public void CalElTest()
        {
            byte input = 0x32;
            double expect = 3.14;

            Assert.AreEqual(DistanceCell.CalEl(input),expect);
        }
    }
}