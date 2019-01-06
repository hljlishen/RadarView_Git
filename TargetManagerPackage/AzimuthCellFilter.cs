using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    class AzimuthCellFilter
    {
        public static AngleArea PassArea { get; set; } = null;

        public static bool Pass(AzimuthCell cell)
        {
            if (PassArea == null)
                return true;
            return PassArea.IsAngleInArea(cell.Angle);
        }
    }
}
