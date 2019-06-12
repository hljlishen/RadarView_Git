using System.Collections.Generic;
using System;
using Utilities;

namespace TargetManagerPackage
{
    public class Casc12thSerializer : ITrackSerializer
    {
        public byte[] Serialize(TargetTrack track, SerializeType type)
        {
            //if (type == SerializeType.Destory)
            //    track.Dis = 0;
            List<byte> cmdBytes = new List<byte> { LocalDeviceCode, RespondCode };
            cmdBytes.AddRange(Tools.IntToByteLsb(NextSendCount(), 2));     //发送计数
            cmdBytes.Add(SystmDeviceCode);
            cmdBytes.Add(CommandTypeCode); //0x80
            cmdBytes.Add(_resendCount);
            cmdBytes.AddRange(Tools.IntToByteLsb(CommandLength, 2));
            cmdBytes.AddRange(Tools.IntToByteLsb(track.TrackId, 2));
            cmdBytes.Add((byte)DateTime.Now.Hour);
            cmdBytes.Add((byte)DateTime.Now.Minute);
            cmdBytes.Add((byte)DateTime.Now.Second);
            cmdBytes.Add((byte)(DateTime.Now.Millisecond / 10));
            cmdBytes.AddRange(_reservedBytes);
            if(type != SerializeType.Destory)
                cmdBytes.AddRange(Tools.IntToByteLsb((int)track.Dis, 4));
            else
                cmdBytes.AddRange(Tools.IntToByteLsb(0, 4));
            cmdBytes.AddRange(Tools.IntToByteLsb((int)(track.Az * 100), 2));
            cmdBytes.AddRange(Tools.IntToByteLsb((int)(track.El * 100), 2));
            cmdBytes.Add(_targetProperty);
            cmdBytes.Add(_beanCategory);
            cmdBytes.Add(Tools.CalBytesXor(cmdBytes.ToArray(), 0, cmdBytes.Count));

            return cmdBytes.ToArray();
        }

        private int NextSendCount() => SendCount == SendCountMax ? SendCount = 0 : SendCount++;

        private const byte LocalDeviceCode = 0x11;
        private const byte SystmDeviceCode = 0x31;
        private const byte RespondCode = 0;
        private const int CommandLength = 29;
        private readonly byte[] _reservedBytes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        private const byte _targetProperty = 0;     //目标属性
        private const byte _beanCategory = 0;       //波束类型
        private byte _resendCount = 0;
        private static int SendCount = 0;
        private const int SendCountMax = 65535;
        private const byte CommandTypeCode = 0x80;
    }
}
