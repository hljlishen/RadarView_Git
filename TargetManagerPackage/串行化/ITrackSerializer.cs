using System;

namespace TargetManagerPackage
{
    public enum SerializeType
    {
        New,
        Update,
        Destory
    };
    public interface ITrackSerializer
    {
        byte[] Serialize(TargetTrack track, SerializeType type);
    }
}
