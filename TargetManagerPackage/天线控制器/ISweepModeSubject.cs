namespace TargetManagerPackage
{
    public interface ISweepModeSubject
    {
        void RegisterSweepModeObserver(ISweepModeObserver ob);

        void UnregisterSweepModeObserver(ISweepModeObserver ob);

        AngleArea GetSweepSection();

        bool IsSectionSweeping();
    }
}
