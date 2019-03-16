using System;
using Utilities;
using System.Collections.Generic;

namespace TargetManagerPackage
{
    class QhOpticalDeviceSerializer : ITrackSerializer  //2019.3.13清华大学联试，发给光电设备的协议
    {
        public byte[] Serialize(TargetTrack track, SerializeType type)
        {
            byte[] timeBytes = GetTime();
            byte trackState = GetStateByte(type);
            byte[] idBytes = Tools.IntToByteLsb(track.TrackId, 2);
            byte[] historyCount = Tools.IntToByteLsb(track.Locations.Count,2);  //历史点太多会有溢出
            byte[] azBytes = Tools.IntToByteLsb((int)(track.Az * 100), 2);
            byte[] elBytes = Tools.IntToByteLsb((int)(track.El * 100), 2);
            byte[] disBytes = Tools.IntToByteLsb((int)(track.Dis), 4);
            byte[] spdBytes = Tools.IntToByteLsb((int)(track.Speed * 100), 4);
            byte[] aglBytes = Tools.IntToByteLsb((int)(track.Az * 10), 2);
            byte[] reserveBytes = new byte[20];

            List<byte> ret = new List<byte>();
            ret.AddRange(frameHead);
            ret.AddRange(timeBytes);
            ret.Add(trackState);
            ret.AddRange(idBytes);
            ret.AddRange(historyCount);
            ret.AddRange(azBytes);
            ret.AddRange(elBytes);
            ret.AddRange(disBytes);
            ret.AddRange(spdBytes);
            ret.AddRange(aglBytes);
            ret.AddRange(reserveBytes);

            return ret.ToArray();
        }

        private byte GetStateByte(SerializeType type)
        {
            switch (type)
            {
                case SerializeType.New:
                    return 0;
                case SerializeType.Update:
                    return 1;
                case SerializeType.Destory:
                    return 2;
                default:
                    return 0;
            }
        }

        private byte[] GetTime()
        {
            TimeSpan diff = DateTime.Now - initialTime;
            int diffI = (int)diff.TotalMilliseconds;
            return Tools.IntToByteLsb(diffI, 4);
        }

        private byte[] frameHead = new byte[] { 0xcc, 0x55, 0x55, 0xcc };
        private DateTime initialTime;

        public QhOpticalDeviceSerializer()
        {
            initialTime = DateTime.Now;
        }
    }
}
