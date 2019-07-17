
namespace TargetManagerPackage
{
    public class TrackSender : ITrackSender
    {
        private IPort port;
        private ITrackSerializer serializer;

        public TrackSender(IPort port, ITrackSerializer serializer)
        {
            this.port = port;
            this.serializer = serializer;
        }

        public void UpdateTrack(TargetTrack track)
        {
            byte[] trackBytes = serializer.Serialize(track, SerializeType.Update);
            port.Send(trackBytes);
        }

        public void NewTrack(TargetTrack track)
        {
            byte[] trackBytes = serializer.Serialize(track, SerializeType.New);
            port.Send(trackBytes);
        }

        public void DestoryTrack(TargetTrack track)
        {
            byte[] trackBytes = serializer.Serialize(track, SerializeType.Destory);
            port.Send(trackBytes);
        }
    }
}
