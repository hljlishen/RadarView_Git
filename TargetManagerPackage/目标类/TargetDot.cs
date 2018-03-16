using System;

namespace TargetManagerPackage
{
    public class TargetDot : Target
    {
        //int am; //回波幅度值
        public TargetDot()
        {
            Adopted = false;
        }

        public TargetDot(float az, float el, float dis)
        {
            AZ = az;
            EL = el;
            Dis = dis;
            CurrentCoordinate.ProjectedDis = (float)(dis * Math.Cos(AngleToRadian(el)));
        }

        public bool Adopted { get; set; }

        public bool ShouldDisplay { get; set; } = true;

        public TargetDot GetMiddleDot(TargetDot dot)
        {
            float az = (AZ + dot.AZ) / 2;
            float el = (EL + dot.EL) / 2;
            float dis = (Dis + dot.Dis) / 2;
            int am = (amValue + dot.amValue) / 2;

            TargetDot dot1 = new TargetDot(az, el, dis){amValue = am};

            return dot1;
        }
    }
}
