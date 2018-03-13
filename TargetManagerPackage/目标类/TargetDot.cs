using System;

namespace TargetManagerPackage
{
    public class TargetDot : Target
    {
        //int am; //回波幅度值
        public TargetDot() : base()
        {
            //CurrentPosition = new PolarCoordinate();
            Adopted = false;
        }

        public TargetDot(float az, float el, float dis)
        {
            CurrentCoordinate.Az = az;
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

            TargetDot dot1 = new TargetDot(az, el, dis);

            return dot1;
        }
    }
}
