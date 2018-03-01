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
        public AzimuthCell(byte[] data)
        {
            DisCells = Hashtable.Synchronized(new Hashtable());

            int angleI = DistanceCell.MakeInt(data, HeadLength, AzimuthLength);
            Angle = ((float)angleI) * 360 / 65536;

            int cellCount = DistanceCell.MakeInt(data, HeadLength + AzimuthLength, DistanceCellCountLength);
            if(cellCount == 0)  //没有距离单元
            {
                return;
            }

            int pos = 48;
            for(int i = 0; i < cellCount; i++)
            {
                var cell = new DistanceCell(data, pos);
                try
                {
                    if (CycleDataFilter.Pass(cell))     //滤波
                        DisCells.Add(cell.index, cell);
                }
                catch
                {
                    // ignored
                }

                pos += DistanceCell.CellLength();
            }
        }

        public float GetAngle()
        {
            return Angle;
        }

        public void Dispose()
        {
            DisCells.Clear();
        }
    }
}
