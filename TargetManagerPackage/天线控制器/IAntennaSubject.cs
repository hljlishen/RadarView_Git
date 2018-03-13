namespace TargetManagerPackage
{
    public interface IAntennaSubject
    {
        void RegisterObserver(IAntennaObserver ob);

        void UnregisterObserver(IAntennaObserver ob);
    }
}
