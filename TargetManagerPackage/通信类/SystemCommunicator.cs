using CycleDataDrivePackage;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TargetManagerPackage
{
    public class SystemCommunicator
    {
        private const string LocalIpAndPortString = "192.168.10.99:10012";
        private const string SystemIpAndPortString = "192.168.10.33:10011";

        public static void UpdateTrack(TargetTrack track)
        {
            Send0X80Cmd(track); 
        }

        public static void DeleteTrack(TargetTrack track)
        {
            track.Dis = 0;
            Send0X80Cmd(track);
        }

        public static void Send0X80Cmd(TargetTrack t)
        {
            byte[] cmdBytes = new X80Data(t).Serialize();

            UdpEthernetCenter.SendData(cmdBytes, LocalIpAndPortString, SystemIpAndPortString);
        }

        //public static byte[] GetCmdBytes(TargetTrack t)
        //{
        //    List<byte> cmdData = new List<byte>();
        //    cmdData.Add(LocalComponentNumber);
        //    cmdData.Add(0);
        //    byte[] sendCmdCountBytes = IntToByteLsb(SendCommandCount++, 2);
        //    if (SendCommandCount >= 65536)
        //        SendCommandCount = 0;
        //    cmdData.AddRange(sendCmdCountBytes);
        //    cmdData.Add(SysteComponentmNumber);
        //    cmdData.Add(0x80);
        //    cmdData.Add(0); //发送次数
        //    cmdData.Add(29);    //长度
        //    cmdData.Add(0);     //长度高位
        //    byte[] trackIdBytes = IntToByteLsb(t.trackID, 2);
        //    cmdData.AddRange(trackIdBytes);
        //    cmdData.Add((byte)DateTime.Now.Hour);
        //    cmdData.Add((byte)DateTime.Now.Minute);
        //    cmdData.Add((byte)DateTime.Now.Second);
        //    cmdData.Add((byte)(DateTime.Now.Millisecond / 10));
        //    byte[] emptyBytes = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        //    cmdData.AddRange(emptyBytes);
        //    byte[] distanceBytes = IntToByteLsb((int)t.CurrentCoordinate.Dis, 4);
        //    cmdData.AddRange(distanceBytes);
        //    byte[] azBytes = IntToByteLsb((int)(t.AZ * 100), 2);
        //    cmdData.AddRange(azBytes);
        //    byte[] elBytes = IntToByteLsb((int)(t.EL * 100), 2);
        //    cmdData.AddRange(elBytes);
        //    cmdData.AddRange(new byte[] { 0, 0 });

        //    byte[] cmdBytes = AddXorCheckByte(cmdData.ToArray());

        //    return cmdBytes;
        //}

        //private void ProcessCommandData(byte[] cmdData)     //处理接收的数据
        //{
        //    if (CheckCommandValid(cmdData))
        //    {
        //        //校验通过
        //    }
        //    else
        //    {
        //        //校验不通过
        //    }
        //}

        //private bool CheckCommandValid(byte[] cmdData)  //检测命令有效性，校验字是否通过
        //{
        //    int length = cmdData.Length - 1;
        //    byte xor = CalBytesXor(cmdData, 0, length);

        //    return xor == cmdData[cmdData.Length - 1];
        //}

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
