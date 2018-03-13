using System.Collections.Generic;

namespace TargetManagerPackage
{
    public enum NotifyType
    {
        Delete,
        Add,
        Update
    }

    public interface ITargetObserver
    {
        void NotifyChange(Target t, NotifyType type);

        void NotifyUpdateSectorDot(List<TargetDot> dots, int sectorIndex);

        void NotifyUpdateSectorTrack(List<TargetTrack> tracks, int sectorIndex);
    }
}
