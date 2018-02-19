using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage
{
    public class DistanceCell
    {
        private int index;       //距离单元的序号
        public static int HeadLength = 2;   //距离单元头（即距离单元编号）包含2个字节
        public static int AMLength = 4;    //幅度值的长度,4个字节
        public static int AMCount = 8;      //一个距离单元中包含的峰值个数
        public static int TotalLength = HeadLength + AMCount * AMLength;    //一个距离单元的总长度
        public static int Resolution = 6;   //距离分辨率6米

        ////8个速度点的采样值
        public int[] speedAM;

        public DistanceCell(byte[] data)    
        {
            byte[] tmp = new byte[AMCount];
            speedAM = new int[AMCount];
            index = data[0] << 8 + data[1];
            int pos = HeadLength;

            for (int i = 0; i < AMCount; i++)
            {
                System.Buffer.BlockCopy(data, pos, tmp, 0, AMLength);
                speedAM[i] = System.BitConverter.ToInt32(tmp, 0);
                pos += AMLength;
            }
        }

        public DistanceCell(byte[] data, int begin)
        {
            index = 0;
            int pos = begin;
            int am = 0;
            speedAM = new int[AMCount];
            //计算距离单元编号
            for (int i= 0; i < HeadLength; i++)
            {
                index = index << 8;
                index += data[pos++];
            }

            //计算速度的幅度
            for(int i = 0; i < AMCount; i++)
            {
                for(int j = 0; j < AMLength; j++)
                {
                    am = am << 8;
                    am += data[pos++];
                }

                speedAM[i] = am;
                am = 0;
            }
        }

        public int Index
        {
            get
            {
                return index;
            }
        }

        public float GetDistance()      //获取当前距离单元的距离值
        {
            if (index == 0)
                return Resolution / 2;
            else
                return Index * Resolution * 1.5f;
        }
    }
}
