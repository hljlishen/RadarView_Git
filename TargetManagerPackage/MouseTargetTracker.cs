

namespace TargetManagerPackage
{
    class MouseTargetTracker
    {
        public TargetTrack track { get; set; }
        private TargetManager targetManager;

        public MouseTargetTracker(TargetManager targetManager)
        {
            this.targetManager = targetManager;
        }

        public void DeleteActiveTarget()
        {
            if (track == null || !track.active) return;

            targetManager.NotifyAllObservers(track, NotifyType.Delete);
            SystemCommunicator.DeleteTrack(track);  //消批
            track = null;

        }

        public void UpdateTrack(TargetDot dot)
        {
            if (track == null)
            {
                track = TargetTrack.CreateTargetTrack(dot.CurrentCoordinate, null);
                track.sectorIndex = dot.sectorIndex;
                targetManager.NotifyAllObservers(track, NotifyType.Add);
            }
            else
            {
                track.locations.Add(track.CurrentCoordinate);
                track.Update(dot.CurrentCoordinate);
                if (dot.sectorIndex != track.sectorIndex)
                {
                    targetManager.NotifyAllObservers(track, NotifyType.Update);
                }
                else
                {
                    targetManager.NotifyAllObservers(track, NotifyType.Update);
                }
            }
            SystemCommunicator.UpdateTrack(track);  //发送给系统
            OpticalDeviceCommunicator.CreateOpticalDeviceCommunicator().SendTrack(track);
        }
    }
}
