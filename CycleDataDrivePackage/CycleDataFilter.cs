
namespace CycleDataDrivePackage
{
    public class CycleDataFilter
    {
        public static int AmThreshold = 0;
        public static int SpeedMaximum = 121;
        public static int SpeedMinimum = 7;
        public static int[] Permanent = {364, 365, 366, 367, 368, 369, 370, 729, 730, 731, 794, 795, 796, 945, 946, 947 };

        public static bool Pass(DistanceCell cell)
        {
            return cell.speed > SpeedMinimum && cell.speed < SpeedMaximum && cell.sumAM > AmThreshold
                   && cell.el > 0;
        }
    }
}
