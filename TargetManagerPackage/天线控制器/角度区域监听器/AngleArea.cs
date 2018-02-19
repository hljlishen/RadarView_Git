using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class AngleArea
    {
        public float BeginAngle { get; }
        public float EndAngle { get;}

        public readonly bool AcrossZeroDegree;

        public AngleArea(float begin, float end)
        {
            BeginAngle = MakeAngle0To360(begin);
            EndAngle = MakeAngle0To360(end);

            AcrossZeroDegree = IsZeroDegreeAcrossingAngleArea(this);
        }

        public static bool IsZeroDegreeAcrossingAngleArea(AngleArea area)
        {
            return area.BeginAngle > area.EndAngle;
        }

        public static List<AngleArea> SpliteNorthAcrossingAngleArea(AngleArea area)
        {
            List<AngleArea> ret = new List<AngleArea>();

            if (!IsZeroDegreeAcrossingAngleArea(area))
            {
                ret.Add(area);
            }
            else
            {
                AngleArea angleArea = new AngleArea(area.BeginAngle, 360);
                ret.Add(angleArea);
                angleArea = new AngleArea(0, area.EndAngle);
                ret.Add(angleArea);
            }

            return ret;
        }

        public bool IsAngleInArea(float angle)
        {
            if (!AcrossZeroDegree) return angle >= BeginAngle && angle < EndAngle;

            List<AngleArea> spliteAreas = SpliteNorthAcrossingAngleArea(this);
            bool ret = false;
            foreach (AngleArea area in spliteAreas)
            {
                if (!area.IsAngleInArea(angle)) continue;
                ret = true;
                break;
            }
            return ret;
        }

        public static float MakeAngle0To360(float angle)
        {
            if (angle < 0)  //负角度，反复假360，直到角度在[0,360)
            {
                while (angle < 0)
                    angle += 360;
                return angle;
            }

            if (angle > 360)   //超过360的角度，直接取余
            {
                return angle % 360;
            }

            return angle;       //正常角度，直接返回
        }
    }
}
