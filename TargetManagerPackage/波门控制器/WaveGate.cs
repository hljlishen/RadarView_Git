using System;

namespace TargetManagerPackage
{
    public class WaveGate : AngleArea
    {
        float beginDistance;
        float endDistance;
        bool isSemiAuto;
        int sweepCount = 0;
        bool active = false;

        public float BeginDistance
        {
            get
            {
                return beginDistance;
            }
        }

        public float EndDistance
        {
            get
            {
                return endDistance;
            }
        }

        public bool IsSemiAuto
        {
            get
            {
                return isSemiAuto;
            }
        }

        public int SweepCount
        {
            get
            {
                return sweepCount;
            }

            set
            {
                sweepCount = value;
            }
        }

        public bool Active
        {
            get
            {
                return active;
            }

            set
            {
                active = value;
            }
        }

        public WaveGate(float beginAngle, float endAngle, float distance1, float distance2, bool isSemi) : base(beginAngle, endAngle)
        {
            beginDistance = Math.Min(distance1,distance2);
            endDistance = Math.Max(distance1, distance2);
            isSemiAuto = isSemi;
        }

        public bool IsCoordinateInWaveGate(PolarCoordinate c)
        {
            if (c == null)
                return false;

            //if(IsAngleInWaveGateRange(c.Az) && IsDistanceInWaveGateRange(c.ProjectedDis))
            if (IsAngleInArea(c.Az) && IsDistanceInWaveGateRange(c.ProjectedDis))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsDistanceInWaveGateRange(float distance)
        {
            if (distance >= BeginDistance && distance <= EndDistance)
                return true;
            else
                return false;
        }
    }
}
