namespace TargetManagerPackage
{
    public interface ITrackSender
    {
        void DestoryTrack(TargetTrack track);
        void NewTrack(TargetTrack track);
        void UpdateTrack(TargetTrack track);
    }
}