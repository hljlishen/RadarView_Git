

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
            if (track == null || !track.Active) return;

            targetManager.NotifyAllObservers(track, NotifyType.Delete);
            SystemCommunicator.DeleteTrack(track);  //消批
            track = null;

        }

        public void UpdateTrack(TargetDot dot)
        {
            if (track == null)
            {
                track = TargetTrack.CreateTargetTrack(dot.CurrentCoordinate, null, 13);
                track.SectorIndex = dot.SectorIndex;
                targetManager.NotifyAllObservers(track, NotifyType.Add);
            }
            else
            {
                track.Locations.Add(track.CurrentCoordinate);
                track.Update(dot.CurrentCoordinate);
                //if (dot.sectorIndex != track.sectorIndex)
                //{
                //    targetManager.NotifyAllObservers(track, NotifyType.Update);
                //}
                //else
                //{
                //    targetManager.NotifyAllObservers(track, NotifyType.Update);
                //}
                targetManager.NotifyAllObservers(track, NotifyType.Update);
            }
            SystemCommunicator.UpdateTrack(track);  //发送给系统
            OpticalDeviceCommunicator.CreateOpticalDeviceCommunicator().SendTrack(track);
        }
    }
}
