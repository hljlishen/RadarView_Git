using System.Collections.Generic;

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
            List<byte> cmdBytes = new List<byte>(){ CommandTypeCode, SourceDeviceCode, DestinationDeviceCode, CommandType, (byte)_track.TrackId};

            cmdBytes.AddRange(IntToByteLsb((int)_track.Dis, 4));
            cmdBytes.AddRange(IntToByteLsb((int)(_track.Az * 100), 2));
            cmdBytes.AddRange(IntToByteLsb((int)(_track.El * 100), 2));
            cmdBytes.AddRange(Crc16(cmdBytes.ToArray()));

            return cmdBytes.ToArray();
        }

        public static (int, PolarCoordinate) GetSerialDataCoordinate(byte[] data)
        {
            if (data[3] != 0x04)
                return (-1, null);

            int trackId = data[4];
            float az = (float) (data[5] + data[6] << 8) / 100;
            float el = (float) (data[7] + data[8] << 8) / 100;
            float dis = 0f;

            PolarCoordinate coordinate =  new PolarCoordinate(az, el, dis);

            return (trackId, coordinate);
        }
    }
}
