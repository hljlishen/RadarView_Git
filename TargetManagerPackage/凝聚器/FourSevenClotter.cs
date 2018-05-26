using CycleDataDrivePackage;
using System.Collections.Generic;
using System;

namespace TargetManagerPackage
{
    class FourSevenClotter : Clotter
    {
        //private static int AreaWidthMinimum = 20;
        //private static int AreaWidthMaximum = 150;
        //private const int ExtentionEndTotalCount = 80;
        //private const int ExtentionEndThreshold = 10;
        //private int count = 0;
        public override void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells)
        {
            base.Clot(center, right,left,cells);

            List<TargetDot> dots = ClotAzCells(new List<AzimuthCell>(cells));

            foreach (var targetDot in dots)
            {
                if (center.IsAngleInArea(targetDot.AZ))
                    center.AddNewDot(targetDot);
                if(right.IsAngleInArea(targetDot.AZ))
                    right.AddPrepareDot(targetDot);
                if (left.IsAngleInArea(targetDot.AZ))
                    left.AddPrepareDot(targetDot);
            }
            center.LoadPrepareDot();
            NotifyUpdateSectorDot(center);  //通知更新该扇区的目标点视图
        }

        protected override List<TargetDot> ClotAzCells(List<AzimuthCell> azCells)
        {
            List<TargetDot> dots = TargetArea.ClotAzCells(azCells);
            dots = MergeDotsNear(dots);
            return dots;
        }

        public List<TargetDot> MergeDotsNear(List<TargetDot> dots)
        {
            List<TargetDot> ls = dots;
            for (int i = ls.Count - 1; i >= 0; i--)   //从后向前遍历所有点，距离<50的点直接取均值，从后向前合并
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (ls[i].DistanceTo(ls[j]) < 80 && Math.Abs( ls[i].Dis - ls[j].Dis) < 4)
                    {
                        ls[j] = ls[i].GetMiddleDot(ls[j]);
                        ls[j].IsClotDot = true;
                        ls.RemoveAt(i);
                        break;
                    }
                }
            }

            return ls;
        }
    }
}
