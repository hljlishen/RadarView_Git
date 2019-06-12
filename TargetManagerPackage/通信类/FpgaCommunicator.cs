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
        Rt11 = 11,
        Rt5 = 5,
        RtClose = 0
    }
    public class FpgaCommunicator
    {
        private static bool _isAmplifierOpen;
        private static RangeType _currentRange;
        public static bool IsAmplifierOpen
        {
            get => _isAmplifierOpen;
            set
            {
                _isAmplifierOpen = value;
                SetFpgaMode();
            }
        }
        public static RangeType CurrentRange
        {
            get => _currentRange;
            set
            {
                _currentRange = value;
                SetFpgaMode();
            }
        }
        private static byte[] FrameBytes = new byte[5] { 0x50, 0x32, 0x46, 0x64, 0 };
        private const string LocalIpAndPortString = "192.168.10.99:2013";
        private const string FpgaIpAndPortString = "192.168.10.5:2005";

        static byte MakeFifthBytes()
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
            }

            return fifthByte;
        }

        private static byte AddAdjustAntennaAngleBit(byte fifthByte) => (byte) (fifthByte | 0x04);

        public static void SetFpgaMode()
        {
            FrameBytes[4] = MakeFifthBytes();

            UdpEthernetCenter.SendData(FrameBytes, LocalIpAndPortString, FpgaIpAndPortString);
        }

        public static void SetCurrentAntennaAngleToZero()
        {
            byte fifthByte = MakeFifthBytes();
            fifthByte = AddAdjustAntennaAngleBit(fifthByte);
            FrameBytes[4] = fifthByte;
            UdpEthernetCenter.SendData(FrameBytes, LocalIpAndPortString, FpgaIpAndPortString);
        }
    }
}