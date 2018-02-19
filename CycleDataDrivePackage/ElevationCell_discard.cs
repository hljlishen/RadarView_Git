using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections;

namespace CycleDataDrivePackage
{
    public class ElevationCell
    {
        public static float Coverage = 18.7f;     //系统仰角覆盖范围
        public static float Resolution = Coverage / AzimuthCell.ElevationCellsCount;  //仰角分辨率

        public static int HeadLength = 1;               //包头（即仰角单元编号）1字节
        public static int DistanceCellsCount = 1024;  //一个仰角单元中，距离单元的最大个数
        public static int TotalLength = HeadLength + DistanceCell.TotalLength * DistanceCellsCount;  //一个仰角单元的总长度
        private int index;          //仰角单元编号
        Hashtable disCells;         //距离单元

        public int Index
        {
            get
            {
                return index;
            }
        }

        public ElevationCell(int index)
        {
            this.index = index;
            disCells = Hashtable.Synchronized(new Hashtable());     //线程安全的哈希表
        }

        public void AddDistanceCell(DistanceCell cell)
        {
            disCells.Add(cell.Index, cell);
        }

        public void RemoveDistanceCell(DistanceCell cell)
        {
            disCells.Remove(cell.Index);
        }

        public void ClearDistanceCells()
        {
            disCells.Clear();
        }

        public float GetEL()    //当前仰角单元对应的仰角值
        {
            if (index == 0)
                return Resolution * 0.5f;
            else
                return Resolution * Index * 1.5f;
        }
    }
}
