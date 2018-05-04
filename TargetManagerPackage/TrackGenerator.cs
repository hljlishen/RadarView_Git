using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            track = TargetTrack.CreateTargetTrack(coordinate, null);
            track.sectorIndex = GetTrackSectorId(track);
            targetManager.NotifyAllObservers(track, NotifyType.Add);
            center = coordinate;
        }

        public void UpdateTrack(Sector s)
        {
            if (track.sectorIndex != s.Index) return;
            MoveTrack();

            targetManager.NotifyAllObservers(track, NotifyType.Update);
            SystemCommunicator.UpdateTrack(track);
            OpticalDeviceCommunicator.CreateOpticalDeviceCommunicator().SendTrack(track);
        }

        public bool DeleteTrackIfActive()
        {
            if (!track.active) return false;

            targetManager.NotifyAllObservers(track, NotifyType.Delete);
            SystemCommunicator.DeleteTrack(track);
            return true;
        }

        private int GetTrackSectorId(TargetTrack t)
        {
            foreach (var s in targetManager.Sectors)
            {
                if (s.IsAngleInArea(t.AZ))
                {
                    return s.Index;
                }
            }

            return -1;
        }

        public void MoveTrack()
        {
            track.Update(NextCoordinate());
            int sectorIndex = GetTrackSectorId(track);
            if(track.sectorIndex != sectorIndex)
                targetManager.NotifyAllObservers(track, NotifyType.Delete);
            track.sectorIndex = sectorIndex;
            targetManager.NotifyAllObservers(track, NotifyType.Update);
        }

        private PolarCoordinate NextCoordinate()
        {
            PolarCoordinate coordinate = new PolarCoordinate();
            Random random = new Random(DateTime.Now.Millisecond);
            int distanceRandomValue = random.Next(10, 50);
            int azRandomValue = random.Next(0, 10);
            double x = center.Dis * Math.Cos(PolarCoordinate.AngleToRadian(90 - center.Az));
            double y = center.Dis * Math.Sin(PolarCoordinate.AngleToRadian(90 - center.Az));
            double x1 = x + 100 * Math.Cos(PolarCoordinate.AngleToRadian(90 - angle));
            double y1 = y + 100 * Math.Sin(PolarCoordinate.AngleToRadian(90 - angle));
            double alpha = 90 - PolarCoordinate.RadianToAngle(Math.Atan2(y1, x1));
            alpha = PolarCoordinate.StandardAngle((float) alpha);
            double r = Math.Sqrt(Math.Pow(x1, 2) + Math.Pow(y1, 2));
            coordinate.Az = (float)alpha ;
            coordinate.Dis = (float)r + distanceRandomValue;
            coordinate.El = 0;
            angle += 51.4f + azRandomValue;
            //angle = random.Next(0, 360);
            return coordinate;
        }
    }
}
