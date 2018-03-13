using System;

namespace TargetManagerPackage
{
    public class WaveGate : AngleArea
    {
        public float BeginDistance { get; }

        public float EndDistance { get; }

        public bool IsSemiAuto { get; }

        public int SweepCount { get; set; } = 0;

        public bool Active { get; set; } = false;

        public WaveGate(float beginAngle, float endAngle, float distance1, float distance2, bool isSemi) : base(beginAngle, endAngle)
        {
            BeginDistance = Math.Min(distance1,distance2);
            EndDistance = Math.Max(distance1, distance2);
            IsSemiAuto = isSemi;
        }

        public bool IsCoordinateInWaveGate(PolarCoordinate c)
        {
            if (c == null)
                return false;

            return IsAngleInArea(c.Az) && IsDistanceInWaveGateRange(c.ProjectedDis);
        }

        public bool IsDistanceInWaveGateRange(float distance)
        {
            return distance >= BeginDistance && distance <= EndDistance;
        }
    }
}
