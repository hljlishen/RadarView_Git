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
