using Microsoft.VisualStudio.TestTools.UnitTesting;
using TargetManagerPackage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage.Tests
{
    [TestClass()]
    public class TargetAreaTests
    {
        [TestMethod()]
        public void GetTargetAreasTest()
        {
            AzimuthCell azCell = new AzimuthCell();
            DistanceCell disCell1 = new DistanceCell { index = 10 };
            DistanceCell disCell2 = new DistanceCell { index = 11 };
            DistanceCell disCell3 = new DistanceCell { index = 12 };
            DistanceCell disCell4 = new DistanceCell { index = 13 };
            DistanceCell disCell5 = new DistanceCell { index = 15 };
            DistanceCell disCell6 = new DistanceCell { index = 100 };
            DistanceCell disCell7 = new DistanceCell { index = 101 };
            DistanceCell disCell8 = new DistanceCell { index = 122 };
            DistanceCell disCell9 = new DistanceCell { index = 132 };

            azCell.DisCells.Add(10, disCell1);
            azCell.DisCells.Add(11, disCell2);
            azCell.DisCells.Add(12, disCell3);
            azCell.DisCells.Add(13, disCell4);
            azCell.DisCells.Add(15, disCell5);
            azCell.DisCells.Add(100, disCell6);
            azCell.DisCells.Add(101, disCell7);
            azCell.DisCells.Add(122, disCell8);
            azCell.DisCells.Add(132, disCell9);

            List<TargetAreaEdge> ret = TargetAreaEdge.GetTargetAreaEdges(azCell);
            Assert.IsTrue(true);
        }
    }
}