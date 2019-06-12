namespace TargetManagerPackage.目标类
{
    internal class SpeedFilter : ITargetTrackFilter
    {
        public bool Pass(TargetTrack t)
        {
            if (t.Speed > 25)
                return false;
            return true;
        }
    }
}
