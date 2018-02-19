using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class Target
    {
        protected PolarCoordinate currentCoordinate;
        public bool active;     //是否被界面选中，删除航迹时删除所有被选中的航迹
        public int sectorIndex = -1;
        public int amValue = 0;
        public const int AmValueMaximum = 65535;

        public Target()
        {
            currentCoordinate = new PolarCoordinate();
            active = false;
        }

        public float AZ
        {
            get { return CurrentCoordinate.Az; }
            set { CurrentCoordinate.Az = value; }
        }

        public float EL
        {
            get { return CurrentCoordinate.El; }
            set { CurrentCoordinate.El = value; }
        }

        public float Dis
        {
            get { return CurrentCoordinate.Dis; }
            set { CurrentCoordinate.Dis = value; }
        }

        public float Height
        {
            get
            {
                return (float)(currentCoordinate.Dis * Math.Sin(AngleToRadian(currentCoordinate.El)));
            }
        }

        public  PolarCoordinate CurrentCoordinate
        {
            get
            {
                return currentCoordinate;
            }
        }

        public static double AngleToRadian(double angle)
        {
            return System.Math.PI * angle / 180;
        }

        public float DistanceTo(Target t)
        {
            return CurrentCoordinate.DistanceTo(t.CurrentCoordinate);
        }
    }
}
