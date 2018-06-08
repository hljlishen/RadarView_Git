using CycleDataDrivePackage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace TargetManagerPackage.Tests
{
    [TestClass()]
    public class TargetAreaEdgeTests
    {
        [TestMethod()]
        public void GetCellTargetAreaEdgeTest()
        {
            AzimuthCell azCell = new AzimuthCell {Angle = 100};
            DistanceCell disCell1 = new DistanceCell {index = 10};
            DistanceCell disCell2 = new DistanceCell { index = 11 };
            DistanceCell disCell3 = new DistanceCell { index = 12 };
            DistanceCell disCell4 = new DistanceCell { index = 13 };
            DistanceCell disCell5 = new DistanceCell { index = 14 };
            DistanceCell disCell6 = new DistanceCell { index = 100 };
            DistanceCell disCell7 = new DistanceCell { index = 101 };
            DistanceCell disCell8 = new DistanceCell { index = 122 };
            DistanceCell disCell9 = new DistanceCell { index = 132 };

            //azCell.DisCells.Add(10, disCell1);
            //azCell.DisCells.Add(11, disCell2);
            //azCell.DisCells.Add(12, disCell3);
            //azCell.DisCells.Add(13, disCell4);
            //azCell.DisCells.Add(14, disCell5);
            //azCell.DisCells.Add(100, disCell6);
            //azCell.DisCells.Add(101, disCell7);
            //azCell.DisCells.Add(122, disCell8);
            azCell.DisCells.Add(132, disCell9);

            AzimuthCell azCell1 = new AzimuthCell {Angle = 50};
            DistanceCell disCell11 = new DistanceCell { index = 10 };
            DistanceCell disCell12 = new DistanceCell { index = 11 };
            DistanceCell disCell13 = new DistanceCell { index = 12 };
            DistanceCell disCell14 = new DistanceCell { index = 13 };
            DistanceCell disCell15 = new DistanceCell { index = 15 };
            DistanceCell disCell16 = new DistanceCell { index = 100 };
            DistanceCell disCell17 = new DistanceCell { index = 101 };
            DistanceCell disCell18 = new DistanceCell { index = 122 };
            DistanceCell disCell19 = new DistanceCell { index = 132 };

            //azCell1.DisCells.Add(10, disCell11);
            //azCell1.DisCells.Add(11, disCell12);
            //azCell1.DisCells.Add(12, disCell13);
            //azCell1.DisCells.Add(13, disCell14);
            //azCell1.DisCells.Add(15, disCell15);
            //azCell1.DisCells.Add(100, disCell16);
            //azCell1.DisCells.Add(101, disCell17);
            //azCell1.DisCells.Add(122, disCell18);
            azCell1.DisCells.Add(132, disCell19);

            AzimuthCell azCell2 = new AzimuthCell { Angle = 200 };
            DistanceCell disCellq = new DistanceCell { index = 10 };
            DistanceCell disCellw = new DistanceCell { index = 11 };
            DistanceCell disCelle = new DistanceCell { index = 12 };
            DistanceCell disCellr = new DistanceCell { index = 13 };
            DistanceCell disCellt = new DistanceCell { index = 15 };
            DistanceCell disCelly = new DistanceCell { index = 100 };
            DistanceCell disCellu = new DistanceCell { index = 101 };
            DistanceCell disCelli = new DistanceCell { index = 122 };
            DistanceCell disCello = new DistanceCell { index = 132 };

            //azCell2.DisCells.Add(10, disCellq);
            //azCell2.DisCells.Add(11, disCellw);
            //azCell2.DisCells.Add(12, disCelle);
            //azCell2.DisCells.Add(13, disCellr);
            azCell2.DisCells.Add(132, disCello);
            List<AzimuthCell> ls = new List<AzimuthCell>();
            for (int i = 0; i < 20; i++)
            {
                AzimuthCell azCellx = new AzimuthCell() {Angle = azCell.Angle++};
                DistanceCell dis = new DistanceCell(){index = 132};
                azCellx.DisCells.Add(132,dis);
                ls.Add(azCellx);
            }

            List<TargetArea> ls1 = FourSevenClotter.GetTargetAreas(ls);

            List<TargetAreaEdge> ret = FourSevenClotter.GetTargetAreaEdges(azCell);
            Assert.IsTrue(true);
        }
    }
}