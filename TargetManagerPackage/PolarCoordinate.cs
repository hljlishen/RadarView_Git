using System;
using System.Collections.Generic;
using Utilities;

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
                    ProjectedDis = (float)(dis * Math.Cos(Tools.DegreeToRadian(el)));
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
                    ProjectedDis = (float)(dis * Math.Cos(Tools.DegreeToRadian((float)el)));
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

        public PolarCoordinate(float az, float el, float dis)
        {
            this.az = az;
            this.el = el;
            this.dis = dis;
        }

        public PolarCoordinate(PolarCoordinate c)
        {
            az = c.az;
            el = c.el;
            dis = c.dis;
            ProjectedDis = c.ProjectedDis;
        }

        public PolarCoordinate Copy() => new PolarCoordinate(this);

        public float X => (float)(dis * Math.Cos(Tools.DegreeToRadian(el)) * Math.Sin(Tools.DegreeToRadian(az)));

        public float Y => (float)(dis * Math.Cos(Tools.DegreeToRadian(el)) * Math.Cos(Tools.DegreeToRadian(az)));

        public float Z => (float)(dis * Math.Sin(Tools.DegreeToRadian(el)));

        public static float DistanceBetween(PolarCoordinate c1, PolarCoordinate c2)
        {
            double r = Math.Pow(c1.X - c2.X, 2) + Math.Pow(c1.Y - c2.Y, 2) + Math.Pow(c1.Z - c2.Z, 2);
            return (float)Math.Sqrt(r);
        }

        public float DistanceTo(PolarCoordinate c) => DistanceBetween(this, c);

        public byte[] Serialize()
        {
            List<byte> ls = new List<byte>();
            ls.AddRange(Tools.FloatToBytes(az,1));
            ls.AddRange(Tools.FloatToBytes(el,1));
            ls.AddRange(Tools.FloatToBytes(dis,1));

            return ls.ToArray();
        }

        public static PolarCoordinate RetangularToPolarCoordinate(float x, float y, float z)
        {
            double hypotenuse = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
            double dis = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2) + Math.Pow(z, 2));
            double el = Tools.RadianToDegree(Math.Acos(hypotenuse / dis));
            double az = Tools.RadianToDegree(Math.Atan(Math.Abs(x) / Math.Abs(y)));

            if (x > 0 && y < 0)
                az = 180 - az;
            if (x < 0 && y < 0)
                az = 180 + az;
            if (x < 0 && y > 0)
                az = 360 - az;

            return new PolarCoordinate((float)az, (float)el, (float)dis);
        }
    }
}
