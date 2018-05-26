using System;
using System.Collections;
using System.Collections.Generic;
using Utilities;

namespace CycleDataDrivePackage
{
    public class AzimuthCell : IDisposable, IComparable
    {
        public Dictionary<int, DistanceCell> DisCells;
        public const int HeadLength = 28; //包头28字节
        public const int TailLength = 2; //包尾2字节
        public const int DistanceCellCountLength = 2; //距离单元个数2字节
        public const int AzimuthLength = 2; //方位角长度
        public float Angle;
        public const int AzimuthCellCount = 2048;  //扫描一周方位单元数
        public const int DistanceCellCountMax = 64;    //最大距离单元个数
        public const float Resolution = ((float)360) / AzimuthCellCount; //方位分辨率
        public const int DistanceCellMaxCount = 59;
        private const int DistanceCellsDataStartPosition = 48;
        public AzimuthCell(byte[] data)
        {
            DisCells = new Dictionary<int, DistanceCell>();

            int angleI;
            (Angle, angleI) = GetAngleFromCycleData(data);

            int cellCount = Tools.MakeInt(data, HeadLength + AzimuthLength, DistanceCellCountLength);

            int pos = DistanceCellsDataStartPosition;
            for(int i = 0; i < cellCount; i++)
            {
                var cell = new DistanceCell(data, pos){az = Angle, azInt = angleI};

                if (CycleDataFilter.Pass(cell) && !DisCells.ContainsKey(cell.index))     //滤波
                    DisCells.Add(cell.index, cell);

                pos += DistanceCell.Length;
            }
        }

        public AzimuthCell()
        {
            DisCells = new Dictionary<int, DistanceCell>();
        }

        private static float CalAngle(int angleIntFromCycleData)
        {
            float realAngle = ((float)angleIntFromCycleData) * 360 / 65536;
            return realAngle;
        }

        public float GetAngle() => Angle;

        public void Dispose() => DisCells.Clear();

        public static (float, int) GetAngleFromCycleData(byte[] data)
        {
            int angleI = Tools.MakeInt(data, HeadLength, AzimuthLength);
            return (CalAngle(angleI), angleI);
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            AzimuthCell otherCell = (AzimuthCell) obj;
            if (Angle - otherCell.Angle < 0.000001f)
                return 0;
            if (Angle > otherCell.Angle)
                return 1;
            else
                return -1;
        }
    }
}
