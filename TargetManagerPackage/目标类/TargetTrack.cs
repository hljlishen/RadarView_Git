using System;
using System.Collections.Generic;
using Utilities;

namespace TargetManagerPackage
{
    public class TargetTrack : Target,IDisposable
    {
        private const int TrackMaximumCount = 200;
        private static readonly int[] Id = new int[TrackMaximumCount];
        public int TrackId { get; set; } = 10;         //批号
        internal PolarCoordinate predictLocation; //预测坐标
        public List<PolarCoordinate> Locations; //历史坐标，最新的在最后
        public double Speed { get; } = 0;

        public int Score { get; set; }           //航迹评分

        public static void SetTrackHeight(TargetTrack track , float height)
        {
            float angle = (float)Tools.RadianToAngle((float)Math.Asin(height / track.Dis));
            track.El = angle;
        }

        private TargetTrack(PolarCoordinate c)
        {
            CurrentCoordinate = c;
            Locations = new List<PolarCoordinate>();
            SetRefreshTimeNow();
        }

        public void Update(PolarCoordinate c)
        {
            Locations.Add(CurrentCoordinate.Copy());   //保存历史航迹
            CurrentCoordinate = c;
        }

        public static TargetTrack CreateTargetTrack(PolarCoordinate current, PolarCoordinate pre, int initScore)
        {
            int trackid = -1;
            for(int i = 0; i < TrackMaximumCount; i++)
            {
                if(Id[i] != 1)  
                {
                    trackid = i;
                    Id[i] = 1;
                    break;
                }
            }

            if (trackid == -1)
            {
                return null;
            }

            TargetTrack t = new TargetTrack(current) {Score = initScore};
            if(pre != null)
                t.Locations.Add(pre);   //上周期自由点的位置添加为历史位置
            t.TrackId = trackid + 1;
            return t;
        }

        public static void ReleaseAllTrackIDs()
        {

        }

        public override byte[] Serialize()
        {
            byte[] coodinateBytes = base.Serialize();
            byte[] trackIdBytes = Tools.FloatToBytes(TrackId, 0);
            byte[] speedBytes = Tools.FloatToBytes((float)Speed, 1);

            List<byte> ls = new List<byte>(trackIdBytes);
            ls.AddRange(coodinateBytes);
            ls.AddRange(speedBytes);

            return ls.ToArray();
        }

        public void Dispose()
        {
            Id[TrackId - 1] = 0;    //释放ID号
            Locations?.Clear();
        }
    }
}
