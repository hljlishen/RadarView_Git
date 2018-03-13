namespace TargetManagerPackage
{
    public enum SweepMode
    {
        Normal,
        Section
    }
    public interface ISweepModeObserver
    {
        void NotifySweepModeChange(SweepMode mode);
    }
}
