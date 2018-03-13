using System;

namespace TargetManagerPackage
{
    public class PolarCoordinate
    {
        private float az;   //方位
        private float el;   //仰角
        private float dis;  //距离

        public float Az
        {
            get => az;

            set => az = value % 360;
        }

        public float El
        {
            get => el;

            set
            {
                el = value;
                if(el > 0 && dis > 0)
                {
                    ProjectedDis = (float)(dis * Math.Cos(AngleToRadian(el)));
                }
            }
        }

        public float Dis
        {
            get => dis;

            set
            {
                dis = value;
                if (el > 0 && dis > 0)
                {
                    ProjectedDis = (float)(dis * Math.Cos(AngleToRadian((float)el)));
                }
            }
        }

        public float ProjectedDis { get; set; }

        public PolarCoordinate()
        {
            Az = -1;
            El = -1;
            Dis = -1;
            ProjectedDis = -1;
        }

        public PolarCoordinate(PolarCoordinate c)
        {
            az = c.az;
            el = c.el;
            dis = c.dis;
            ProjectedDis = c.ProjectedDis;
        }

        public PolarCoordinate Copy()
        {
            return new PolarCoordinate(this);
        }

        public bool EqualsTo(PolarCoordinate c)
        {
            return false;
        }

        public static float AngleToRadian(float angle)
        {
            return (float)Math.PI * angle / 180;
        }

        public float X => (float)(dis * Math.Cos(AngleToRadian( el)) * Math.Cos(AngleToRadian( az)));

        public float Y => (float)(dis * Math.Cos(AngleToRadian(el)) * Math.Sin(AngleToRadian(az)));

        public float Z => (float)(dis * Math.Sin(AngleToRadian(el)));

        public static float DistanceBetween(PolarCoordinate c1, PolarCoordinate c2)
        {
            double r = Math.Pow(c1.X - c2.X, 2) + Math.Pow(c1.Y - c2.Y, 2) + Math.Pow(c1.Z - c2.Z, 2);
            return (float)Math.Sqrt(r);
        }

        public float DistanceTo(PolarCoordinate c)
        {
            return DistanceBetween(this, c);
        }
    }
}
