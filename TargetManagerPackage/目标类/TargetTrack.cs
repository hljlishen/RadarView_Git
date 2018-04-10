using System;
using System.Collections.Generic;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public class TargetTrack : Target,IDisposable
    {
        private const int TrackMaximumCount = 200;
        private static readonly int[] id = new int[TrackMaximumCount];
        public int trackID = 10;         //批号
        internal PolarCoordinate predictLocation; //预测坐标
        public List<PolarCoordinate> locations; //历史坐标，最新的在最后
        public double speed;        //速度
        public int score;           //航迹评分
        private static int TrackIdBytes = 2;
        private static int SpeedBytes = 2;
        protected static int AzBytes = 2;
        protected static int ElBytes = 2;
        protected static int DisBytes = 4;
        protected static int SpeedAzBytes = 2;
        public static int TotalBytes = AzBytes + ElBytes + DisBytes + TrackIdBytes + SpeedBytes + SpeedAzBytes;


        public TargetTrack()
        {
            locations = new List<PolarCoordinate>();
        }

        public TargetTrack(byte[] data, int p, int sector)     
        {

            locations = new List<PolarCoordinate>();
            int pos = p;

            sectorIndex = sector;

            trackID = DistanceCell.MakeInt(data, pos, TrackIdBytes);
            pos += TrackIdBytes;

            int az = DistanceCell.MakeInt(data, pos, AzBytes);
            pos += AzBytes;

            int dis = DistanceCell.MakeInt(data, pos, DisBytes);
            pos += DisBytes;

            int el = DistanceCell.MakeInt(data, pos, ElBytes);
            pos += ElBytes;

            speed = DistanceCell.MakeInt(data, pos, SpeedBytes);
            pos += SpeedBytes;

            int speedAz = DistanceCell.MakeInt(data, pos, SpeedAzBytes);

            AZ = (float)az / 10;
            EL = (float)el / 10;
            Dis = dis;
        }
        public TargetTrack(PolarCoordinate c)
        {
            currentCoordinate = c;
            locations = new List<PolarCoordinate>();
        }

        public void Update(PolarCoordinate c)
        {
            locations.Add(currentCoordinate.Copy());   //保存历史航迹
            currentCoordinate = c;
        }

        public static TargetTrack CreateTargetTrack(PolarCoordinate current, PolarCoordinate pre)
        {
            int trackid = -1;
            for(int i = 0; i < TrackMaximumCount; i++)
            {
                if(id[i] != 1)  
                {
                    trackid = i;
                    id[i] = 1;
                    break;
                }
            }

            if (trackid == -1)
            {
                return null;
            }

            TargetTrack t = new TargetTrack(current);
            if(pre != null)
                t.locations.Add(pre);   //上周期自由点的位置添加为历史位置
            t.trackID = trackid + 1;
            return t;
        }

        public static void ReleaseAllTrackIDs()
        {

        }

        public override byte[] Serialize()
        {
            byte[] coodinateBytes = base.Serialize();
            byte[] trackIdBytes = PolarCoordinate.FloatToBytes(trackID, 0);
            byte[] speedBytes = PolarCoordinate.FloatToBytes((float)speed, 1);

            List<byte> ls = new List<byte>(trackIdBytes);
            ls.AddRange(coodinateBytes);
            ls.AddRange(speedBytes);

            return ls.ToArray();
        }

        public void Dispose()
        {
            id[trackID - 1] = 0;    //释放ID号
            locations?.Clear();
        }
    }
}
