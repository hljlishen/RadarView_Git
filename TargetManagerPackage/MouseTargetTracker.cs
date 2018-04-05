

namespace TargetManagerPackage
{
    class MouseTargetTracker
    {
        private TargetTrack track;
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
                track = new TargetTrack(dot.CurrentCoordinate)
                {
                    sectorIndex = dot.sectorIndex,
                    trackID = 1
                };
                //targetManager.Sectors[dot.sectorIndex].tracks.Add(track);
                targetManager.NotifyAllObservers(track, NotifyType.Add);
            }
            else
            {
                track.locations.Add(track.CurrentCoordinate);
                track.Update(dot.CurrentCoordinate);
                if (dot.sectorIndex != track.sectorIndex)
                {
                    //targetManager.Sectors[track.sectorIndex].tracks.Remove(track);
                    //targetManager.NotifyAllObservers(track, NotifyType.Delete);
                    //track.sectorIndex = dot.sectorIndex;
                    //targetManager.Sectors[dot.sectorIndex].tracks.Add(track);
                    targetManager.NotifyAllObservers(track, NotifyType.Update);
                }
                else
                {
                    //if(!targetManager.Sectors[track.sectorIndex].tracks.Contains(track))
                    //    targetManager.Sectors[dot.sectorIndex].tracks.Add(track);
                    targetManager.NotifyAllObservers(track, NotifyType.Update);
                }
            }
            SystemCommunicator.UpdateTrack(track);  //发送给系统
        }
    }
}
