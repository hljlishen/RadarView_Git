using System.Collections.Generic;

namespace CycleDataDrivePackage
{
    public class DistanceCellFilter
    {
        public static int AmThreshold = 0;
        public static int SpeedMaximum = 59;
        public static int SpeedMinimum = 7;
        public static int HeightMinimum = 25;
        public static double DistanceMax = double.MaxValue;
        public static double CircleDistanceBegin = 4900;      //回波圈开始的距离
        public static double CircleDistanceEnd = 5100;        //回波圈结束的距离
        public static int CircleAmThreshold = 2500000;        //回波圈幅度门限，大于该值的回波才会显示
        private static List<CircleFilter> circleFilters = new List<CircleFilter>();

        public static void AddCircleFilter(CircleFilter filter) => circleFilters.Add(filter);

        public static bool Pass(DistanceCell cell)
        {
            foreach(CircleFilter filter in circleFilters)
            {
                if (!filter.Pass(cell)) return false;
            }

            if (cell.Heigth <= HeightMinimum)
                return false;

            return cell.speed > SpeedMinimum && cell.speed < SpeedMaximum && cell.sumAM > AmThreshold
                   && cell.el > 0 && cell.Distance < DistanceMax;
        }
    }

    public class CircleFilter
    {
        double CircleDistanceBegin = 4900;      //回波圈开始的距离
        double CircleDistanceEnd = 5100;        //回波圈结束的距离
        int CircleAmThreshold = 2500000;        //回波圈幅度门限，大于该值的回波才会显示
        public CircleFilter(double beginDis, double endDis, int amThreshold)
        {
            CircleDistanceBegin = beginDis;
            CircleDistanceEnd = endDis;
            CircleAmThreshold = amThreshold;
        }

        public bool Pass(DistanceCell cell)
        {
            if (cell.Distance > CircleDistanceBegin && cell.Distance < CircleDistanceEnd && cell.sumAM < CircleAmThreshold)
                return false;
            return true;
        }
    }
}
