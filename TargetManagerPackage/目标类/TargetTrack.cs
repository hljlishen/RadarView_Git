using System;
using System.Collections.Generic;
using Utilities;

namespace TargetManagerPackage
{
    public class TargetTrack : Target,IDisposable, IComparable
    {
        public delegate void ChangeSectorHandler(Target target, int toSectorIndex);
        public ChangeSectorHandler ChangeSector;
        public bool IsFocused { get; } = false;
        private const int TrackMaximumCount = 200;
        private static readonly int[] Id = new int[TrackMaximumCount];
        public int TrackId { get; set; } = 10;         //批号
        public PolarCoordinate PredictLocation; //预测坐标
        public List<PolarCoordinate> Locations; //历史坐标，最新的在最后
        public const double XSpeedMaximum = 16;
        public const double YSpeedMaximum = 16;
        public const double ZSpeedMaximum = 10;

        public bool IsFake { get; set; } = false;

        public double Speed => Math.Sqrt(Math.Pow(XSpeed, 2) + Math.Pow(YSpeed, 2) + Math.Pow(ZSpeed, 2));

        public double XSpeed { get; set; } = 0;
        public double YSpeed { get; set; } = 0;
        public double ZSpeed { get; set; } = 0;
        private static int latestId = 0;

        public override int SectorIndex
        {
            get => _sectorIndex;
            set
            {
                if (value == _sectorIndex) return;
                ChangeSector?.Invoke(this, value);
                _sectorIndex = value;
            }
        }

        public PolarCoordinate PredictCoordinate(DateTime time)
        {
            if (Locations.Count == 0)   //没有历史位置，返回当前位置
                return CurrentCoordinate;
            TimeSpan interval = time - LastRefreshTime;
            double x = CurrentCoordinate.X + XSpeed * interval.TotalSeconds;
            double y = CurrentCoordinate.Y + YSpeed * interval.TotalSeconds;
            double z = CurrentCoordinate.Z + ZSpeed * interval.TotalSeconds;

            return PolarCoordinate.RetangularToPolarCoordinate((float)x, (float)y, (float)z);
        }

        public int Score { get; set; }           //航迹评分
        public const int ScoreMaximum = 12;
        public const int ScoreMinimum = 0;

        public void ScoreAdd(int socre)
        {
            Score += socre;

            Score = Score > ScoreMaximum ? ScoreMaximum : Score;
            Score = Score < ScoreMinimum ? ScoreMinimum : Score;
        }

        public static void SetTrackHeight(TargetTrack track , float height)
        {
            float angle = (float)Tools.RadianToDegree((float)Math.Asin(height / track.Dis));
            track.El = angle;
        }

        private TargetTrack(TargetDot currunt, TargetDot pre)
        {
            CurrentCoordinate = currunt.CurrentCoordinate;
            Locations = new List<PolarCoordinate>();
            if (pre != null)
            {
                Locations.Add(pre.CurrentCoordinate);
                (XSpeed, YSpeed, ZSpeed) = CalSpeed(pre.CurrentCoordinate, currunt.CurrentCoordinate,
                    currunt.LastRefreshTime - pre.LastRefreshTime); //计算速度
            }
            else
            {
                XSpeed = XSpeedMaximum;
                YSpeed = YSpeedMaximum;
                ZSpeed = ZSpeedMaximum;
            }

            SetRefreshTimeNow();
        }

        public void Update(PolarCoordinate c)
        {
            (XSpeed, YSpeed, ZSpeed) = CalSpeed(CurrentCoordinate, c, DateTime.Now - LastRefreshTime);  //计算三个方向速度
            Locations.Add(CurrentCoordinate.Copy());   //保存历史航迹
            CurrentCoordinate = c;
            SetRefreshTimeNow();        //设置更新时间
        }

        public (float, float, float) CalSpeed(PolarCoordinate lastCoordinate, PolarCoordinate curruntCoordinate, TimeSpan time)
        {
            double xSpeed = (curruntCoordinate.X - lastCoordinate.X) / time.TotalSeconds;
            double ySpeed = (curruntCoordinate.Y - lastCoordinate.Y) / time.TotalSeconds;
            double zSpeed = (curruntCoordinate.Z - lastCoordinate.Z) / time.TotalSeconds;

            return ((float) xSpeed, (float) ySpeed, (float) zSpeed);
        }

        public static TargetTrack CreateTargetTrack(TargetDot current, TargetDot pre, int initScore)
        {
            int trackid = GetNextTrackIdUpword();

            if (trackid == 0)
            {
                return null;
            }

            TargetTrack t = new TargetTrack(current, pre)
            {
                Score = initScore,
                TrackId = trackid
            };

            SystemCommunicator.UpdateTrack(t);  //发送目标信息给控制中心
            return t;
        }

        private static int GetNextTrackId()
        {
            int trackid = -1;
            for(int i = 0; i < TrackMaximumCount; i++)
            {
                if(Id[i] != 1)  
                {
                    trackid = i;
                    Id[i] = 1;
                    return i + 1;
                }
            }

            return 0;
        }

        private static int GetNextTrackIdUpword()
        {
            for (int i = latestId + 1; i != latestId; i++)
            {
                if (i >= TrackMaximumCount)
                    i = 1;
                if (Id[i] != 1)
                {
                    latestId = i;
                    Id[i] = 1;
                    return i;
                }
            }

            return 0;
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
            Unfocus();
            Id[TrackId - 1] = 0;    //释放ID号
            Locations?.Clear();
        }

        public int CompareTo(object obj)
        {
            TargetTrack track = (TargetTrack) obj;
            if (Score > track.Score) return 1;
            if (Score < track.Score) return -1;
            return 0;
        }

        public float GetCrelateRadius()
        {
            if (Locations.Count == 0)
                return 500;
            return 50;
        }

        public void Focus()
        {
            TargetManagerFactory.RegisterTrackObserver(this);
        }

        public void Unfocus()
        {
            TargetManagerFactory.UnregisterTrackObserver(this);
        }
    }
}
