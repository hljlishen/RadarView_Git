namespace TargetManagerPackage
{
    abstract class TrackCorelator : SectorProcessor
    {
        public abstract void Corelate(Sector center, Sector left, Sector right);
    }
}
