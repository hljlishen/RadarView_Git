using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class TestDataSender
    {
        public static TargetTrack track12 = TargetTrack.CreateTargetTrack(new TargetDot(82.2f, 10.8f, 1355f), null, 13);
        public static TargetTrack track68 = TargetTrack.CreateTargetTrack(new TargetDot(122.3f, 8.3f, 3266f), null, 13);
        public static TargetManager targetManager = TargetManagerFactory.CreateTrackManager();

        public static void SendTrack12()
        {
            track12.TrackId = 12;
            track12.SectorIndex = 0;
            track12.Dis = 1355f;
            SystemCommunicator.UpdateTrack(track12);
            targetManager.NotifyAllObservers(track12, NotifyType.Update);
        }

        public static void DeleteTrack12()
        {
            track12.TrackId = 12;
            track12.SectorIndex = 0;
            SystemCommunicator.DeleteTrack(track12);
            targetManager.NotifyAllObservers(track12, NotifyType.Delete);
        }

        public static void SendTrack68()
        {
            track68.TrackId = 68;
            track68.SectorIndex = 0;
            track68.Dis = 3266f;
            SystemCommunicator.UpdateTrack(track68);
            targetManager.NotifyAllObservers(track68, NotifyType.Update);
        }

        public static void DeleteTrack68()
        {
            track12.TrackId = 68;
            track12.SectorIndex = 0;
            SystemCommunicator.DeleteTrack(track68);
            targetManager.NotifyAllObservers(track68, NotifyType.Delete);
        }
    }
}
