using System;
using System.Collections.Generic;
using Utilities;
using TargetManagerPackage.目标类;

namespace TargetManagerPackage
{
    public class TargetTrack : Target, IDisposable, IComparable
    {
        public delegate void ChangeSectorHandler(Target target, int toSectorIndex);
        public ChangeSectorHandler ChangeSector;
        public bool IsFocused { get; } = false;
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

        internal static FindTrackIdStrategy FindIdStrategy { get; set; } = null;
        internal static TrackSender Sender { get; set; } = null;

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

            //距离判断
            //if (c.Dis < 1000)
            //{
            //    Random random = new Random(DateTime.Now.Millisecond);
            //    AdjustHeigthTo(80 + random.Next(-5, 5), c);
            //}

            //if (c.Dis >= 1000 && c.Dis < 2000)
            //{
            //    Random random = new Random(DateTime.Now.Millisecond);
            //    AdjustHeigthTo(150 + random.Next(-5, 5), c);
            //}

            //if (c.Dis >= 2000)
            //{
            //    Random random = new Random(DateTime.Now.Millisecond);
            //    AdjustHeigthTo(250 + random.Next(-5, 5), c);
            //}

            Locations.Add(CurrentCoordinate.Copy());   //保存历史航迹
            CurrentCoordinate = c;
            SetRefreshTimeNow();        //设置更新时间
            //OutputInfo("update:  ");
            //SystemCommunicator.UpdateTrack(this);
            Sender.UpdateTrack(this);
        }

        public static void AdjustHeigthTo(float height, PolarCoordinate c)
        {
            double projectedDis = c.Dis * Math.Cos(Tools.DegreeToRadian(c.El));
            c.El = (float)Tools.RadianToDegree( Math.Atan2(height, projectedDis));
            c.Dis = (float)Math.Sqrt(Math.Pow(projectedDis, 2) + Math.Pow(height, 2));
        }

        public void OutputInfo(string infoHead)
        {
            string info = infoHead + $"id={TrackId}   socre={Score}    historyCount={Locations.Count}";
            Console.WriteLine(info);
        }

        public void Destory()
        {
            //OutputInfo("delete:  ");
            Dispose();
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
            int trackid = FindIdStrategy.NextId();

            if (trackid == 0)
            {
                return null;
            }

            TargetTrack t = new TargetTrack(current, pre)
            {
                Score = initScore,
                TrackId = trackid
            };

            //SystemCommunicator.UpdateTrack(t);  //发送目标信息给控制中心
            Sender.NewTrack(t);
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
            //SystemCommunicator.DeleteTrack(this);
            Sender.DestoryTrack(this);
            Unfocus();
            FindIdStrategy.ReleaseId(TrackId);
            Locations?.Clear();
        }

        public int CompareTo(object obj)
        {
            TargetTrack track = (TargetTrack) obj;
            return Score.CompareTo(track.Score);
        }

        public float GetCorelateRadius()
        {
            if (Locations.Count == 0)
                return 200;
            return 500;
        }

        public void Focus()
        {
            Sender.UpdateTrack(this);
            //TargetManagerFactory.RegisterTrackObserver(this);
        }

        public void Unfocus() => TargetManagerFactory.UnregisterTrackObserver(this);
    }
}
