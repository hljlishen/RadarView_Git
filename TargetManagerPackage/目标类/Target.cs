using System;
using Utilities;

namespace TargetManagerPackage
{
    public class Target
    {

        protected int _sectorIndex = -1;
        public PolarCoordinate CurrentCoordinate { get; set; } = new PolarCoordinate();
        public bool Active { get; set; }     //是否被界面选中，删除航迹时删除所有被选中的航迹
        public virtual int SectorIndex
        { get => _sectorIndex; set=>_sectorIndex = value; }

        public int AmValue { get; set; } = 0;
        public const int AmValueMaximum = 65535;
        public const float DroneMaximumSpeed = 15;  //无人机速度最大值，单位：m/s
        public const float DroneMinimumSpeed = 2;  //无人机速度最大值，单位：m/s
        public DateTime LastRefreshTime { get; set; }
        public void SetRefreshTimeNow()
        {
            LastRefreshTime = DateTime.Now;
        }

        protected TimeSpan TimeSpanSinceLastRefresh(DateTime time) => time - LastRefreshTime;
        protected float MaximumFlyDistanceSinceLastRefresh(DateTime time) => (float)TimeSpanSinceLastRefresh(time).TotalSeconds * DroneMaximumSpeed;
        protected float MinimumFlyDistanceSinceLastRefresh(DateTime time)
        {
            float minDistance = 10;
            float min =  (float)TimeSpanSinceLastRefresh(time).TotalSeconds * DroneMinimumSpeed;
            return Math.Max(min, minDistance);
        }

        public float Az
        {
            get => CurrentCoordinate.Az;
            set => CurrentCoordinate.Az = value;
        }

        public float El
        {
            get => CurrentCoordinate.El;
            set => CurrentCoordinate.El = value;
        }

        public float Dis
        {
            get => CurrentCoordinate.Dis;
            set => CurrentCoordinate.Dis = value;
        }

        public float Height => (float)(CurrentCoordinate.Dis * Math.Sin(Tools.DegreeToRadian(CurrentCoordinate.El)));

        public float DistanceTo(Target t) => CurrentCoordinate.DistanceTo(t.CurrentCoordinate);

        public virtual byte[] Serialize() => CurrentCoordinate.Serialize();
    }
}
