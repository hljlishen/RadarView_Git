using System;
using Utilities;

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
            get => CurrentCoordinate.Az;
            set => CurrentCoordinate.Az = value;
        }

        public float EL
        {
            get => CurrentCoordinate.El;
            set => CurrentCoordinate.El = value;
        }

        public float Dis
        {
            get => CurrentCoordinate.Dis;
            set => CurrentCoordinate.Dis = value;
        }

        public float Height => (float)(currentCoordinate.Dis * Math.Sin(Tools.AngleToRadian(currentCoordinate.El)));

        public  PolarCoordinate CurrentCoordinate => currentCoordinate;

        public float DistanceTo(Target t) => CurrentCoordinate.DistanceTo(t.currentCoordinate);

        public virtual byte[] Serialize() => currentCoordinate.Serialize();
    }
}
