using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class X68Command : EthernetData
    {
        private readonly TargetTrack _track;
        private const byte CommandType = 0x1;
        public X68Command(TargetTrack track) : base(0x68, 0x2, 0x1)
        {
            _track = track;
        }

        public byte[] Serialize()
        {
            List<byte> cmdBytes = new List<byte>(){ CommandTypeCode, SourceDeviceCode, DestinationDeviceCode, CommandType, (byte)_track.trackID};

            cmdBytes.AddRange(IntToByteLsb((int)_track.Dis, 4));
            cmdBytes.AddRange(IntToByteLsb((int)(_track.AZ * 100), 2));
            cmdBytes.AddRange(IntToByteLsb((int)(_track.EL * 100), 2));
            cmdBytes.AddRange(Crc16(cmdBytes.ToArray()));

            return cmdBytes.ToArray();
        }
    }
}
