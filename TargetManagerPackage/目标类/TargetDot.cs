using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class TargetDot : Target
    {
        //int am; //回波幅度值
        bool adopted;
        bool shouldDisplay = true;
        public TargetDot() : base()
        {
            //CurrentPosition = new PolarCoordinate();
            adopted = false;
        }

        public TargetDot(float az, float el, float dis)
        {
            CurrentCoordinate.Az = az;
            EL = el;
            Dis = dis;
            CurrentCoordinate.ProjectedDis = (float)(dis * Math.Cos(AngleToRadian(el)));
        }

        public bool Adopted
        {
            get
            {
                return adopted;
            }

            set
            {
                adopted = value;
            }
        }

        public bool ShouldDisplay
        {
            get
            {
                return shouldDisplay;
            }

            set
            {
                shouldDisplay = value;
            }
        }

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
