using System;
using Utilities;

namespace TargetManagerPackage
{
    public class TargetDot : Target
    {
        public bool IsClotDot { get; set; } = false;
        public int DotWidth { get; set; } = 1;

        //int am; //回波幅度值
        public TargetDot()
        {
            Adopted = false;
            SetRefreshTimeNow();
        }

        public bool IsDotRelated(TargetDot dot)
        {
            float dis = DistanceTo(dot);
            return dis <= MaximumFlyDistanceSinceLastRefresh(DateTime.Now) && dis > MinimumFlyDistanceSinceLastRefresh(DateTime.Now);
        }

        public TargetDot(float az, float el, float dis)
        {
            Az = az;
            El = el;
            Dis = dis;
            CurrentCoordinate.ProjectedDis = (float)(dis * Math.Cos(Tools.AngleToRadian(el)));
            SetRefreshTimeNow();
        }

        public bool Adopted { get; set; }

        public bool ShouldDisplay { get; set; } = true;

        public TargetDot GetMiddleDot(TargetDot dot)
        {
            float az = (Az + dot.Az) / 2;
            float el = (El + dot.El) / 2;
            float dis = (Dis + dot.Dis) / 2;
            int am = (AmValue + dot.AmValue) / 2;

            TargetDot dot1 = new TargetDot(az, el, dis){AmValue = am};

            return dot1;
        }
    }
}
