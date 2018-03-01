using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;
using System.IO;

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
                        TargetDot dot = new TargetDot(cell.GetAngle(), dis.el, dis.Distance);
                        dot.amValue = dis.sumAM;
                        center.AddNewDot(dot);
                    }
                }
            }

            NotifyUpdateSectorDot(center);  //通知更新该扇区的目标点视图
        }
    }
}
