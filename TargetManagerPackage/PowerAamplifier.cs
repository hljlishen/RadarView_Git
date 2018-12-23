using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{

    public enum RangeType
    {
        Rt11,
        Rt5,
        RtClose
    }
    public class PowerAmplifier
    {
        private static bool _isAmplifierOpen;
        private static RangeType _currentRange;
        public static bool IsAmplifierOpen
        {
            get => _isAmplifierOpen;
            set
            {
                _isAmplifierOpen = value;
                SendCmd();
            }
        }
        public static RangeType CurrentRange
        {
            get => _currentRange;
            set
            {
                _currentRange = value;
                SendCmd();
            }
        }
        private static byte[] FrameBytes = new byte[5] { 0x50, 0x32, 0x46, 0x64, 0 };
        private const string LocalIpAndPortString = "192.168.10.99:2013";
        private const string FpgaIpAndPortString = "192.168.10.5:2005";

        public static void SendCmd()
        {
            byte fifthByte = 0;

            if (IsAmplifierOpen)
                fifthByte = 0x80;

            switch (CurrentRange)
            {
                case RangeType.Rt11:
                    fifthByte += 0x03;
                    break;
                case RangeType.Rt5:
                    fifthByte += 0x02;
                    break;
                case RangeType.RtClose:
                    fifthByte += 0x01;
                    break;
                default:
                    break;
            }

            FrameBytes[4] = fifthByte;

            UdpEthernetCenter.SendData(FrameBytes, LocalIpAndPortString, FpgaIpAndPortString);
        }
    }
}