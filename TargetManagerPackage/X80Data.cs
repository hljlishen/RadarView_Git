using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class X80Data : EthernetData
    {
        private const byte LocalDeviceCode = 0x11;
        private const byte SystmDeviceCode = 0x31;
        private const byte RespondCode = 0;
        private const int CommandLength = 29;
        private readonly byte[] _reservedBytes = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
        private readonly int _trackId;
        private readonly double _distance;
        private readonly double _az;
        private readonly double _el;
        private const byte _targetProperty = 0;     //目标属性
        private const byte _beanCategory = 0;       //波束类型
        private byte _resendCount = 0;

        private static int SendCount = 0;
        private const int SendCountMax = 65535;

        public X80Data(TargetTrack t) : base(0x80, LocalDeviceCode, SystmDeviceCode)
        {
            _trackId = t.trackID;
            _distance = t.Dis;
            _az = t.AZ;
            _el = t.EL;
        }

        public byte[] Serialize()
        {
            List<byte> cmdBytes = new List<byte> {LocalDeviceCode, RespondCode};
            cmdBytes.AddRange(IntToByteLsb(NextSendCount(),2));     //发送计数
            cmdBytes.Add(SystmDeviceCode);
            cmdBytes.Add(CommandTypeCode); //0x80
            cmdBytes.Add(_resendCount);
            cmdBytes.AddRange(IntToByteLsb(CommandLength,2));
            cmdBytes.AddRange(IntToByteLsb(_trackId,2));
            cmdBytes.Add((byte)DateTime.Now.Hour);
            cmdBytes.Add((byte)DateTime.Now.Minute);
            cmdBytes.Add((byte)DateTime.Now.Second);
            cmdBytes.Add((byte)(DateTime.Now.Millisecond / 10));
            cmdBytes.AddRange(_reservedBytes);
            cmdBytes.AddRange(IntToByteLsb((int)_distance, 4));
            cmdBytes.AddRange(IntToByteLsb((int)(_az * 100), 2));
            cmdBytes.AddRange(IntToByteLsb((int)(_el * 100), 2));
            cmdBytes.Add(_targetProperty);
            cmdBytes.Add(_beanCategory);
            cmdBytes.Add(CalBytesXor(cmdBytes.ToArray(),0,cmdBytes.Count));

            return cmdBytes.ToArray();
        }

        private int NextSendCount() => SendCount == SendCountMax ? SendCount = 0 : SendCount++;
    }
}
