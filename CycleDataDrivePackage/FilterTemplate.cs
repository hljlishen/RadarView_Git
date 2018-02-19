using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage
{
    internal abstract class FilterTemplate : ICycleDataFilter   //滤波器模板类，定义生成方位单元的流程
    {
        public AzimuthCell Process(byte[] data)
        {
            return CreateAzimuthCell(data); 
        }

        protected abstract bool Pass(DistanceCell cell);

        ElevationCell CreateElevationCell(byte[] data)      //基于数组拷贝的算法
        {
            int elIndex = data[0];    //第一个8位为仰角单元编号
            ElevationCell elCell = new ElevationCell(elIndex);
            int pos = ElevationCell.HeadLength;    //从取完仰角包头的位置开始创建距离单元
            byte[] tmp = new byte[DistanceCell.TotalLength];    //存放一个距离单元数据的缓存
            for(int i = 0; i < ElevationCell.DistanceCellsCount; i++)     //根据一个仰角单元包含的距离单元的个数，循环创建距离单元
            {
                System.Buffer.BlockCopy(data, pos, tmp, 0, tmp.Length);

                //创建距离单元对象
                DistanceCell cell = new DistanceCell(tmp);
                if (Pass(cell))
                    elCell.AddDistanceCell(cell);

                pos += DistanceCell.TotalLength;    //指向下一距离单元
            }

            return elCell;
        }

        ElevationCell CreateElevationCell(byte[] data, int begin)
        {
            int elIndex = 0;
            int pos = begin;

            //计算仰角单元编号
            for (int i = 0; i <ElevationCell.HeadLength; i++)
            {
                elIndex = elIndex << 8;
                elIndex += data[pos++];
            }
            ElevationCell elCell = new ElevationCell(elIndex);

            //生成距离单元
            for(int i = 0; i < ElevationCell.DistanceCellsCount; i ++)
            {
                DistanceCell cell = new DistanceCell(data, pos);
                pos += DistanceCell.TotalLength;

                if (Pass(cell))
                    elCell.AddDistanceCell(cell);
            }

            return elCell;
        }

        public AzimuthCell CreateAzimuthCell(byte[] data)
        {
            int index = 0;
            int pos = 0;
            //计算方位单元编号
            for (int i = 0; i < AzimuthCell.HeadLength; i++)
            {
                index = index << 8;
                index += data[pos++];
            }
            AzimuthCell azCell = new AzimuthCell(index);

            for(int i = 0; i < AzimuthCell.ElevationCellsCount; i ++)
            {
                ElevationCell elCell = CreateElevationCell(data, pos);
                azCell.AddElevationCell(elCell);
                pos += ElevationCell.TotalLength;
            }

            return azCell;
        }
    }
}
