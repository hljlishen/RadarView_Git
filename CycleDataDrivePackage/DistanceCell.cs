using System;
using Utilities;

namespace CycleDataDrivePackage
{
    public class DistanceCell
    {
        public static float ElAdjustment = -5f;
        public const float AntennaFixedEl = 12;
        public bool adopted = false;    //是否已经被录取
        public int index;               //单元编号
        public float az;                //方位角
        public int azInt;               //换算成真实角度之前的整形值
        public int sumAM;               //和幅度
        public int differAM;            //差幅度
        public int speed;               //速度
        public float el;                //仰角
        public bool Occupied = false;   //是否已被划入区域
        public static readonly float Resolution = 5.84f;     //距离分辨率3米*2.5f
        private const int HeadLength = 2;                    //包头长度2字节
        private static readonly int DistanceLength = 2;      //距离长度2字节
        private static readonly int speedLength = 1;         //速度长度1字节
        private static readonly int elLength = 3;            //仰角0长度
        private static readonly int elAm0Length = 4;         //幅度长度3字节
        private static readonly int elAm1Length = 4;      //幅度长度3字节
        public static readonly int Length = DistanceLength + elAm0Length + elAm1Length + speedLength  + 
            HeadLength + elLength; //距离单元总长度

        public DistanceCell(byte[] data, int pos)
        {
            int p = pos + HeadLength;

            index = Tools.MakeInt(data, p, DistanceLength);
            p += DistanceLength;

            speed = Tools.MakeInt(data, p, speedLength);
            p += speedLength;

            uint elTmp = (uint)Tools.MakeInt(data, p, elLength);
            p += elLength;
            short el1Tmp = (short)(elTmp & 0x3ff);
            double el1 = CalEl(el1Tmp);
            short el0Tmp = (short)((elTmp & 0xffc000) >> 14); 
            double el0 = CalEl(el0Tmp);

            double eldif = el0 - el1;
            //eldif -= 2.28f;/* 第二套设备俯仰角校正系数*/
            eldif -= 0.83f;/* 第一套设备俯仰角校正系数*/
            if (eldif < -Math.PI)
                eldif += Math.PI * 2;
            else if (eldif > Math.PI)
                eldif -= Math.PI * 2;

            el = (float)Math.Asin(0.03f * eldif);
            el = (float)Tools.RadianToDegree(el)/* - 3.9f*/;
            el += AntennaFixedEl- ElAdjustment;

            sumAM = Tools.MakeInt(data, p, elAm0Length);
            p += elAm0Length;

            differAM = Tools.MakeInt(data, p, elAm1Length);

            //if (index * Resolution > 3400 && index * Resolution < 3700 && sumAM > 1711101)
            //    Console.WriteLine(sumAM);
        }

        public DistanceCell()
        {

        }

        public int Distance => (int)( Resolution * index);

        public double Heigth => Math.Sin(Tools.DegreeToRadian(el)) * Distance;

        public static double CalEl(short b)
        {
            byte high4 = (byte)(b >> 7);
            byte low4 = (byte)(b & 0x7f);
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

            double decimalPart = ((double)low4) / 127;

            return intergerPart + decimalPart /*< 0 ? intergerPart + decimalPart + 2*Math.PI : intergerPart + decimalPart*/;
        }

        //private static int FindNearestValueIndex(float[] values, float value)     //查找values中与value最接近的值的下标
        //{
        //    if (value > values[0])
        //        return 0;
        //    if (value < values[values.Length - 1])
        //        return values.Length - 1;
        //    for (int i = 0; i < values.Length; i++)
        //    {
        //        if (value <= values[i] && value > values[i + 1])
        //        {
        //            float differI = Math.Abs(values[i] - value);
        //            float differI1 = Math.Abs(values[i + 1] - value);
        //            if (differI < differI1)
        //                return i;
        //            return i + 1;
        //        }
        //    }

        //    return -1;
        //}
    }
}
