using System.Collections.Generic;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public abstract class Clotter : SectorProcessor
    {
        public static bool ShouldShowOriginalVideo { get; set; } = false;
        public virtual void Clot(Sector center, Sector nextSector, Sector preSector, AzimuthCell[] cells)
        {
            //MoveNewDotToOldDot(center);
            center.BeginProcessSector();

            if(ShouldShowOriginalVideo)
                ShowOriginalVideo(center, nextSector, preSector, cells);
        }

        protected abstract List<TargetDot> ClotAzCells(List<AzimuthCell> azCells);

        public static void MoveNewDotToOldDot(Sector s)
        {
            s.OldDots.Clear();

            foreach (TargetDot dot in s.NewDots)
            {
                s.OldDots.Add(dot);
            }

            s.NewDots.Clear();
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
                        TargetDot dot = new TargetDot(cell.GetAngle(), dis.el, dis.Distance) { AmValue = dis.sumAM };
                        center.AddNewDot(dot);
                    }
                }
            }
        }
    }
}
