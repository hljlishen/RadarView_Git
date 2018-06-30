using Utilities;

namespace TargetManagerPackage
{
    public class MouseTargetTracker
    {
        public TargetTrack track { get; set; }
        private readonly TargetManager targetManager;
        private static float trackHeight = 150f;
        public static float TrackHeight
        {
            get => trackHeight + Tools.RandomInt(-25,25);
            set => trackHeight = value;
        }

        public MouseTargetTracker(TargetManager targetManager)
        {
            this.targetManager = targetManager;
        }

        public void DeleteTrack() => track = null;

        public void DeleteActiveTarget()
        {
            if (track == null || !track.Active) return;

            targetManager.NotifyAllObservers(track, NotifyType.Delete);
            SystemCommunicator.DeleteTrack(track);  //消批
            track = null;
        }

        public void UpdateTrack(TargetDot dot)
        {
            if (track == null)
            {
                track = TargetTrack.CreateTargetTrack(dot, null, 13);
                track.IsFake = true;
                //TargetTrack.SetTrackHeight(track, TrackHeight);
                track.SectorIndex = dot.SectorIndex;
                targetManager.NotifyAllObservers(track, NotifyType.Add);
                targetManager.Sectors[dot.SectorIndex].AddTrack(track);
            }
            else
            {
                track.Locations.Add(track.CurrentCoordinate);
                track.Update(dot.CurrentCoordinate);
                if (dot.SectorIndex != track.SectorIndex)
                {
                    targetManager.NotifyAllObservers(track, NotifyType.Update);
                }
                else
                {
                    targetManager.NotifyAllObservers(track, NotifyType.Update);
                }
                TargetTrack.SetTrackHeight(track, TrackHeight);
                targetManager.NotifyAllObservers(track, NotifyType.Update);
            }
            SystemCommunicator.UpdateTrack(track);  //发送给系统
            OpticalDeviceCommunicator.CreateOpticalDeviceCommunicator().SendTrack(track);
        }
    }
}
