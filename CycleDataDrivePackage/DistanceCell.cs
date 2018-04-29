using System;
using System.Globalization;

namespace CycleDataDrivePackage
{
    public class DistanceCell
    {
        //负角度查找表
        //private static float[] negtiveELQuotients =
        //    new float[] {185.3906918f,12.33585555f,6.365799734f,4.277005523f,3.209456925f,2.558984464f,2.119461434f,1.801352778f,1.559488289f,
        //                 1.368608609f,1.213479705f,1.084364634f,0.974745935f,0.880093382f,0.797157012f,0.723541455f,0.657438998f,0.597456262f,
        //                 0.542498171f,0.491688147f,0.444311805f,0.399776264f,0.357580008f,0.31728995f,0.278523422f,0.240933475f,0.2041963f,
        //                 0.1679998f,0.13203258f,0.095972895f,0.059479428f};
        //正角度查找表
        //private static float[] positiveELQuotients =
        //    new float[] {185.3906918f,14.22946203f,6.83676424f,4.485692671f,3.326420751f,2.633493791f,2.170896803f,1.838859138f,
        //                 1.587936016f,1.390826038f,1.231219423f,1.098768574f,0.986588255f,0.889915747f,0.805348005f,0.730385235f,
        //                 0.663146185f,0.602184111f,0.546364051f,0.494778668f,0.446689036f,0.401481911f,0.358638101f,0.317708371f,
        //                 0.27829445f,0.240033423f,0.202584187f,0.165614914f,0.128790544f,0.091759402f,0.054138745f,0.015520686f};

        //private static readonly float[] PositiveElQuotients = 
        //{
        //    2191.656216f, 35.66167268f, 17.63037088f, 11.62734139f, 8.60416259f, 6.76900941f, 5.527169949f, 4.62446665f,
        //    3.934095881f, 3.385768165f, 2.937416279f, 2.56235225f, 2.242852051f, 1.96670355f, 1.725234706f,
        //    1.512130547f, 1.322693501f, 1.153364312f, 1.001402065f
        //};

        //private static readonly float[] NegtiveElQuotients = 
        //{
        //    2191.656216f, 34.77525341f, 17.39654637f, 11.51446319f, 8.533727764f, 6.718343523f, 5.487374934f,
        //    4.591365295f, 3.905483401f, 3.360382487f, 2.914492221f, 2.541406835f, 2.223572287f, 1.948883788f,
        //    1.708739713f, 1.496872692f, 1.30861807f, 1.140439668f, 0.989612816f
        //};

        private static readonly float[] NegtiveElQuotients =
        {
            10.73922842f, 8.343471514f, 4.831024976f, 3.554619579f, 2.716732344f, 1.93684351f, 1.560125859f,
            1.281842999f, 1.183244534f, 1.060719085f, 0.966420582f, 0.87088672f, 0.783003903f, 0.691371164f,
            0.560331546f, 0.517145026f, 0.435177335f, 0.356825475f
        };

        private static readonly float[] PositiveElQuotients =
        {
            10.73922842f, 6.509474939f, 3.845684648f, 2.730261387f, 2.503597851f, 1.962304668f, 1.819103253f,
            1.361438115f, 1.033520649f, 0.919221707f, 0.780879387f, 0.676289249f, 0.601455362f, 0.5175328f,
            0.453029894f, 0.377893981f, 0.305324603f, 0.267858707f, 0.274810742f
        };


        public bool adopted = false;    //是否已经被录取
        public int index;               //单元编号
        public int azIndex;             //所在方位单元的编号
        public int sumAM;               //和幅度
        public int differAM;            //差幅度
        public int speed;               //速度
        public float el;                //仰角
        public static readonly float Resolution = 2.92f;     //距离分辨率3米*2.5f
        private const int HeadLength = 2;                    //包头长度2字节
        private static readonly int DistanceLength = 2;      //距离长度2字节
        private static readonly int sumAmLength = 4;         //幅度长度4字节
        private static readonly int differAmLength = 4;      //幅度长度4字节
        private static readonly int speedLength = 2;         //速度长度1字节
        private static readonly int elSignLength = 2;        //俯仰角符号2字节
        public static readonly int Length = DistanceLength + sumAmLength + differAmLength + speedLength  + HeadLength + elSignLength; //距离单元总长度

        public DistanceCell(byte[] data, int pos)
        {
            int p = pos + HeadLength;

            index =  MakeInt(data, p, DistanceLength);
            p += DistanceLength;

            speed = MakeInt(data, p, speedLength);
            p += speedLength;

            int sign = MakeInt(data, p, elSignLength);
            p += elSignLength;

            sumAM = MakeInt(data, p, sumAmLength);
            p += sumAmLength;

            differAM = MakeInt(data, p, differAmLength);

            float quotient = ((float)sumAM) / differAM;
            int valueIndex;

            if (sign == 1)   //负角度
            {
                valueIndex = FindNearestValueIndex(NegtiveElQuotients, quotient);
                el = -valueIndex;
            }
            else             //正角度
            {
                valueIndex = FindNearestValueIndex(PositiveElQuotients, quotient);
                el = valueIndex;
            }

            el += 15;
        }

        public int Distance => (int)( Resolution * index);

        public static int MakeInt( byte[] data, int pos, int count)     //用字节组合成整型，高位在前
        {
            int ret = 0;
            for (int i = 0; i < count; i++, pos++)
            {
                ret <<= 8;
                ret += data[pos];
            }

            return ret;
        }

        private static int FindNearestValueIndex(float[] values, float value)     //查找values中与value最接近的值的下标
        {
            if (value > values[0])
                return 0;
            if (value < values[values.Length - 1])
                return values.Length - 1;
            for(int i = 0; i < values.Length; i ++)
            {
                if(value <= values[i] && value > values[i+1] )
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
