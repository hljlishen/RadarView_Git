using System;
using System.Globalization;
using Utilities;

namespace CycleDataDrivePackage
{
    public class DistanceCell
    {
        public const float AntennaFixedEl = 15;
        public bool adopted = false;    //是否已经被录取
        public int index;               //单元编号
        //public int azIndex;             //所在方位单元的编号
        public float az;                //方位角
        public int azInt;               //换算成真实角度之前的整形值
        public int sumAM;               //和幅度
        public int differAM;            //差幅度
        public int speed;               //速度
        public float el;                //仰角
        public bool Occupied = false;   //是否已被划入区域
        public static readonly float Resolution = 2.92f;     //距离分辨率3米*2.5f
        private const int HeadLength = 2;                    //包头长度2字节
        private static readonly int DistanceLength = 2;      //距离长度2字节
        private static readonly int speedLength = 1;         //速度长度1字节
        private static readonly int el0Length = 2;            //仰角0长度
        private static readonly int el1Length = 2;            //仰角1长度
        private static readonly int backupLength = 1;           //备用字节
        private static readonly int elAm0Length = 3;         //幅度长度3字节
        private static readonly int elAm1Length = 3;      //幅度长度3字节
        public static readonly int Length = DistanceLength + elAm0Length + elAm1Length + speedLength  + HeadLength + el0Length + el1Length; //距离单元总长度

        public DistanceCell(byte[] data, int pos)
        {
            int p = pos + HeadLength;

            index = Tools.MakeInt(data, p, DistanceLength);
            p += DistanceLength;

            speed = Tools.MakeInt(data, p, speedLength);
            p += speedLength;

            double el0 = CalEl(data[p]);
            p += el0Length;

            double el1 = CalEl(data[p]);
            p += el1Length;
            p += backupLength;

            double eldif = el0 - el1;

            el = (float)Math.Asin(0.114f * eldif);
            el = (float)Tools.RadianToDegree(el);
            el += AntennaFixedEl;

            sumAM = Tools.MakeInt(data, p, elAm0Length);
            p += elAm0Length;

            differAM = Tools.MakeInt(data, p, elAm1Length);
        }

        public DistanceCell()
        {

        }

        public int Distance => (int)( Resolution * index);

        public static double CalEl(byte b)
        {
            byte high4 = (byte)(b >> 4);
            byte low4 = (byte)(b & 0xf);
            int intergerPart;

            switch (high4)
            {
                case 0:
                    intergerPart = 0;
                    break;
                case 1:
                    intergerPart = 1;
                    break;
                case 2:
                    intergerPart = 2;
                    break;
                case 3:
                    intergerPart = 3;
                    break;
                case 4:
                    intergerPart = -4;
                    break;
                case 5:
                    intergerPart = -3;
                    break;
                case 6:
                    intergerPart = -2;
                    break;
                case 7:
                    intergerPart = -1;
                    break;
                    default:
                        intergerPart = 0;
                        break;  
            }

            double decimalPart = ((double)low4) / 15f;

            return intergerPart + decimalPart;
        }


        private static int FindNearestValueIndex(float[] values, float value)     //查找values中与value最接近的值的下标
        {
            if (value > values[0])
                return 0;
            if (value < values[values.Length - 1])
                return values.Length - 1;
            for (int i = 0; i < values.Length; i++)
            {
                if (value <= values[i] && value > values[i + 1])
                {
                    float differI = Math.Abs(values[i] - value);
                    float differI1 = Math.Abs(values[i + 1] - value);
                    if (differI < differI1)
                        return i;
                    return i + 1;
                }
            }

            return -1;
        }
    }
}
