using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    abstract class EthernetData
    {
        protected byte DataType { get; set; } //标识
        protected byte SourceDeviceCode { get; set; }         //源设备代号
        protected byte DestinationDeviceCode { get; set; }    //目标设备代号

        protected EthernetData(byte type, byte srcCode, byte desCode)
        {
            DataType = type;
            SourceDeviceCode = srcCode;
            DestinationDeviceCode = desCode;
        }

        public static byte[] IntToByteLsb(int number, int reqiredMinimumLength)   //整型转化为byte数组，小端模式
        {
            List<byte> ls = new List<byte>();
            if (number < 255)
                ls.Add((byte)number);

            else
            {
                int tmp = number;

                for (int shiftCount = 0; ; shiftCount += 8)
                {
                    tmp = number >> shiftCount;
                    if (tmp == 0)
                        break; ;
                    byte b = (byte)(tmp & 0xff);
                    ls.Add(b);
                }
            }


            int actualLength = ls.Count;
            for (int i = 0; i < reqiredMinimumLength - actualLength; i++)
            {
                ls.Add(0);
            }

            return ls.ToArray();
        }
    }
}
