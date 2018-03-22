namespace RadarDisplayPackage
{
    public interface IControlStateSubject
    {
        void RegisterObserver(IControlStateObserver ob);

        void UnregisterObserver(IControlStateObserver ob);
    }
}
