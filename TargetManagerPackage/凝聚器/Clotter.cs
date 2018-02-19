using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    abstract class Clotter : SectorProcessor
    {
        public abstract void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells);

        public static void MoveNewDotToOldDot(Sector s)
        {
            s.oldDots.Clear();

            foreach (TargetDot dot in s.newDots)
            {
                s.oldDots.Add(dot);
            }

            s.newDots.Clear();
        }
    }
}
