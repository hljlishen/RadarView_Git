using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace TargetManagerPackage
{
    class TrackGenerator
    {
        public TargetTrack track { get; }
        private TargetManager targetManager;
        private float angle = 0;
        private PolarCoordinate center;

        public TrackGenerator(TargetManager targetManager, PolarCoordinate coordinate)
        {
            this.targetManager = targetManager;
            //track = new TargetTrack(coordinate);
            track = TargetTrack.CreateTargetTrack(coordinate, null, 13);
            track.SectorIndex = GetTrackSectorId(track);
            targetManager.NotifyAllObservers(track, NotifyType.Add);
            center = coordinate;
        }

        public void UpdateTrack(Sector s)
        {
            if (track.SectorIndex != s.Index) return;
            MoveTrack();
            targetManager.NotifyAllObservers(track, NotifyType.Update);
            track.SetRefreshTimeNow();  //设置更新时间
            SystemCommunicator.UpdateTrack(track);
            OpticalDeviceCommunicator.CreateOpticalDeviceCommunicator().SendTrack(track);
        }

        public bool DeleteTrackIfActive()
        {
            if (!track.Active) return false;

            targetManager.NotifyAllObservers(track, NotifyType.Delete);
            SystemCommunicator.DeleteTrack(track);
            return true;
        }

        private int GetTrackSectorId(TargetTrack t)
        {
            foreach (var s in targetManager.Sectors)
            {
                if (s.IsAngleInArea(t.Az))
                {
                    return s.Index;
                }
            }

            return -1;
        }

        public void MoveTrack()
        {
            track.Update(NextStraightCoordinate());
            TargetTrack.SetTrackHeight(track, MouseTargetTracker.TrackHeight);
            int sectorIndex = GetTrackSectorId(track);
            if(track.SectorIndex != sectorIndex)
                targetManager.NotifyAllObservers(track, NotifyType.Delete);
            track.SectorIndex = sectorIndex;
            targetManager.NotifyAllObservers(track, NotifyType.Update);
        }

        private PolarCoordinate NextCircleCoordinate()
        {
            PolarCoordinate coordinate = new PolarCoordinate();
            int distanceRandomValue = Tools.RandomInt(10, 50);
            int azRandomValue = Tools.RandomInt(0, 10);
            double x = center.Dis * Math.Cos(Tools.AngleToRadian(90 - center.Az));
            double y = center.Dis * Math.Sin(Tools.AngleToRadian(90 - center.Az));
            double x1 = x + 100 * Math.Cos(Tools.AngleToRadian(90 - angle));
            double y1 = y + 100 * Math.Sin(Tools.AngleToRadian(90 - angle));
            double alpha = 90 - Tools.RadianToAngle(Math.Atan2(y1, x1));
            alpha = Tools.StandardAngle((float) alpha);
            double r = Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2));
            coordinate.Az = (float)alpha ;
            coordinate.Dis = (float)r + distanceRandomValue;
            coordinate.El = 0;
            angle += 51.4f + azRandomValue;;
            return coordinate;
        }

        private PolarCoordinate NextStraightCoordinate()
        {
            float speed = 10f; //速度10m/s
            float dis = track.Dis;
            TimeSpan time = DateTime.Now - track.LastRefreshTime;
            dis = dis + (float)time.TotalSeconds * speed;
            return new PolarCoordinate(track.CurrentCoordinate.Az + Tools.RandomInt(-3,3) / 6f, track.CurrentCoordinate.El, dis);
        }
    }
}
