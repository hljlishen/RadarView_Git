using CycleDataDrivePackage;
using System.Collections.Generic;
using System.Linq;

namespace TargetManagerPackage
{
    class Clotter_3DClot : Clotter
    {
        public override void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells)
        {
            List<TargetDot> dots = new List<TargetDot>();
            MoveNewDotToOldDot(center);  //上周期的新目标点变为本周起的自由点

            foreach (AzimuthCell cell in cells)     //将方位单元格中的每个距离单元格生成一个目标点
            {
                dots.AddRange
                (
                    from DistanceCell dis in cell.DisCells.Values
                    where !dis.adopted
                    select new TargetDot(cell.GetAngle(), dis.el, dis.Distance)
                );
            }

            List<TargetDot> ls = dots;
            for (int i = ls.Count - 1; i >= 0; i--)   //从后向前遍历所有点，距离<50的点直接取均值，从后向前合并
            {
                for (int j = i - 1; j >= 0; j--)
                {
                    if (ls[i].DistanceTo(ls[j]) < 5)
                    {
                        ls[j] = ls[i].GetMiddleDot(ls[j]);
                        ls.RemoveAt(i);
                        break;
                    }
                }
            }

            foreach(TargetDot dot in dots)
            {
                if (center.IsAngleInArea(dot.Az))
                {
                    center.AddNewDot(dot);
                }
            }

            NotifyUpdateSectorDot(center);  //通知更新该扇区的目标点视图
        }

        protected override List<TargetDot> ClotAzCells(List<AzimuthCell> azCells)
        {
            throw new System.NotImplementedException();
        }
    }
}
