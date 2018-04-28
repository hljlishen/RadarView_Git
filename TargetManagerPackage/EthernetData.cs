using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public abstract class EthernetData
    {
        protected byte CommandTypeCode { get; set; } //标识
        protected byte SourceDeviceCode { get; set; }         //源设备代号
        protected byte DestinationDeviceCode { get; set; }    //目标设备代号

        protected EthernetData(byte typeCode, byte srcCode, byte desCode)
        {
            CommandTypeCode = typeCode;
            SourceDeviceCode = srcCode;
            DestinationDeviceCode = desCode;
        }

        public static byte CalBytesXor(byte[] data, int offset, int length) //计算异或和
        {
            int tmp = data[offset] ^ data[offset + 1];
            for (int dataIndex = offset + 2; dataIndex <= length - offset - 1; dataIndex++)
            {
                tmp = tmp ^ data[dataIndex];
            }

            return (byte)(tmp & 0xff);
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
                ls.Add(0);

            return ls.ToArray();
        }

        public static byte[] Crc16(byte[] data)
        {
            byte[] returnVal = new byte[2];
            int i, flag;
            byte crc16Lo = 0xFF;
            byte crc16Hi = 0xFF;
            const byte cl = 0x86;
            const byte ch = 0x68;
            for (i = 0; i < data.Length; i++)
            {
                crc16Lo = (byte)(crc16Lo ^ data[i]);//每一个数据与CRC寄存器进行异或
                for (flag = 0; flag <= 7; flag++)
                {
                    var saveHi = crc16Hi;
                    var saveLo = crc16Lo;
                    crc16Hi = (byte)(crc16Hi >> 1);//高位右移一位
                    crc16Lo = (byte)(crc16Lo >> 1);//低位右移一位
                    if ((saveHi & 0x01) == 0x01)//如果高位字节最后一位为
                    {
                        crc16Lo = (byte)(crc16Lo | 0x80);//则低位字节右移后前面补 否则自动补0
                    }

                    if ((saveLo & 0x01) != 0x01) continue;
                    crc16Hi = (byte)(crc16Hi ^ ch);
                    crc16Lo = (byte)(crc16Lo ^ cl);
                }
            }
            returnVal[0] = crc16Hi;//CRC高位
            returnVal[1] = crc16Lo;//CRC低位
            return returnVal;
        }
    }
}
