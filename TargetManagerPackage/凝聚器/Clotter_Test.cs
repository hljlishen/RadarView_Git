using System.Collections.Generic;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    class Clotter_Test : Clotter
    {
        public override void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells)
        {
            MoveNewDotToOldDot(center);

            foreach (AzimuthCell cell in cells)
            {
                foreach (object o in cell.DisCells.Values)
                {
                    if (center.IsAngleInArea(cell.GetAngle()))
                    {
                        DistanceCell dis = (DistanceCell) o;
                        TargetDot dot = new TargetDot(cell.GetAngle(), dis.el, dis.Distance) {AmValue = dis.sumAM};
                        center.AddNewDot(dot);
                    }
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
