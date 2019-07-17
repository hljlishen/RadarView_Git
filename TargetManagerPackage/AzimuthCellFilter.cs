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
        private static object _locker = new object();
        private static AngleArea passArea = null;
        public static AngleArea PassArea
        {
            get
            {
                return passArea;
            }
            set
            {
                lock(_locker)
                {
                    passArea = value;
                }
            }
        }

        public static bool Pass(AzimuthCell cell)
        {
            lock (_locker)
            {
                if (PassArea == null)
                    return true;
                return PassArea.IsAngleInArea(cell.Angle);
            }
        }
    }
}
