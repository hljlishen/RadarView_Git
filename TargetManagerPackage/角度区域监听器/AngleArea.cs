using System.Collections.Generic;
using Utilities;

namespace TargetManagerPackage
{
    public class AngleArea
    {
        public float BeginAngle { get; private set; }
        public float EndAngle { get; private set; }

        public bool AcrossZeroDegree { get; private set; }

        public AngleArea(float begin, float end)
        {
            Initialize(begin, end);
        }

        private void Initialize(float begin, float end)
        {
            BeginAngle = Tools.StandardAngle(begin);
            EndAngle = Tools.StandardAngle(end);
            if (Tools.FloatEquals(EndAngle, 0))    //区域的终止角度应为360，不能是0
                EndAngle = 360;

            AcrossZeroDegree = IsZeroDegreeAcrossingAngleArea(this);
        }

        public AngleArea(double begin, double end)
        {
            Initialize((float)begin, (float)end);
        }

        public static bool IsZeroDegreeAcrossingAngleArea(AngleArea area) => area.BeginAngle > area.EndAngle;

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
    }
}
