using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage
{
    public class CycleDataFilter
    {
        public static int AMThreshold = 0;
        public static int SpeedMaximum = 61;
        public static int SpeedMinimum = 3;
        public static int[] Permanent = {364, 365, 366, 367, 368, 369, 370, 729, 730, 731, 794, 795, 796, 945, 946, 947 };

        public static bool Pass(DistanceCell cell)
        {
            if (cell.speed > SpeedMinimum && cell.speed < SpeedMaximum && cell.sumAM > AMThreshold/*&& ! Permanent.Contains(cell.index)*/)
                return true;
            return false;
        }
    }
}
