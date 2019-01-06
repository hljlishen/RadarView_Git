
namespace CycleDataDrivePackage
{
    public class DistanceCellFilter
    {
        public static int AmThreshold = 0;
        public static int SpeedMaximum = 59;
        public static int SpeedMinimum = 5;
        public static double DistanceMax = double.MaxValue;
        public static double CircleDistanceBegin = -1;      //回波圈开始的距离
        public static double CircleDistanceEnd = 0;         //回波圈结束的距离
        public static int CircleAmThreshold = 0;            //回波圈幅度门限，大于该值的回波才会显示

        public static bool Pass(DistanceCell cell)
        {
            if(cell.Distance > CircleDistanceBegin && cell.Distance < CircleDistanceEnd)
            {
                if (cell.sumAM < CircleAmThreshold)
                    return false;
            }
            return cell.speed > SpeedMinimum && cell.speed < SpeedMaximum && cell.sumAM > AmThreshold
                   && cell.el > 0 && cell.Distance < DistanceMax;
        }
    }
}
