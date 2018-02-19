using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage
{
    public class AzimuthCell
    {
        public static int ElevationCellsCount = 4; //一个方位单元中，仰角单元最大个数
        public static int AzimuthCellCount = 2048;  //扫描一周方位单元数
        public static float Resolution = 360 / AzimuthCellCount; //方位分辨率
        List<ElevationCell> elCells;                //仰角单元
        public static int HeadLength = 2;   //包头（即方位单元编号）2字节
        public static int TotalLength = HeadLength + ElevationCellsCount * ElevationCell.TotalLength;   //一个方为单元的总长度
        int index;

        public float GetAngle()
        {
            if (index == 0)
                return Resolution * 0.5f;
            else
                return Resolution * index * 1.5f;
        }

        public int GetIndex()
        {
            return index;
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public AzimuthCell( int index)
        {
            this.index = index;
            elCells = new  List<ElevationCell>();
        }

        public void AddElevationCell(ElevationCell cell)
        {
            elCells.Add(cell);
        }
    }
}
