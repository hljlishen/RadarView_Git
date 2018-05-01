using System.Collections.Generic;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public abstract class Clotter : SectorProcessor
    {
        public static bool ShouldShowOriginalVideo { get; set; } = true;
        public virtual void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells)
        {
            MoveNewDotToOldDot(center);

            if(ShouldShowOriginalVideo)
                ShowOriginalVideo(center, right, left, cells);
        }

        protected abstract List<TargetDot> ClotAzCells(List<AzimuthCell> azCells);

        public static void MoveNewDotToOldDot(Sector s)
        {
            s.oldDots.Clear();

            foreach (TargetDot dot in s.newDots)
            {
                s.oldDots.Add(dot);
            }

            s.newDots.Clear();
        }

        protected void ShowOriginalVideo(Sector center, Sector right, Sector left, AzimuthCell[] cells)
        {
            foreach (AzimuthCell cell in cells)
            {
                foreach (object o in cell.DisCells.Values)
                {
                    if (center.IsAngleInArea(cell.GetAngle()))
                    {
                        DistanceCell dis = (DistanceCell)o;
                        TargetDot dot = new TargetDot(cell.GetAngle(), dis.el, dis.Distance) { amValue = dis.sumAM };
                        center.AddNewDot(dot);
                    }
                }
            }
        }
    }
}
