using System;
using System.Collections;

namespace CycleDataDrivePackage
{
    public class AzimuthCell : IDisposable
    {
        public Hashtable DisCells;
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
            DisCells = Hashtable.Synchronized(new Hashtable());

            Angle = GetAngleFromCycleData(data);

            int cellCount = DistanceCell.MakeInt(data, HeadLength + AzimuthLength, DistanceCellCountLength);

            int pos = DistanceCellsDataStartPosition;
            for(int i = 0; i < cellCount; i++)
            {
                var cell = new DistanceCell(data, pos);

                if (CycleDataFilter.Pass(cell) && !DisCells.Contains(cell.index))     //滤波
                    DisCells.Add(cell.index, cell);

                pos += DistanceCell.Length;
            }
        }

        private static float CalAngle(int angleIntFromCycleData)
        {
            float realAngle = ((float)angleIntFromCycleData) * 360 / 65536;
            return realAngle;
        }

        public float GetAngle() => Angle;

        public void Dispose() => DisCells.Clear();

        public static float GetAngleFromCycleData(byte[] data)
        {
            int angleI = DistanceCell.MakeInt(data, HeadLength, AzimuthLength);
            return CalAngle(angleI);
        }

        public static float StandardAngle(float angle) //将角度转化为0-360的浮点数
        {
            if (angle < 0)
                angle += 360;
            angle %= 360;

            return angle;
        }

        public static float ReverAngleDirection(float angle)
        {
            float rAngle = 360f - angle;
            rAngle = StandardAngle(rAngle);

            return rAngle;
        }
    }
}
