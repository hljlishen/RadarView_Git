namespace TargetManagerPackage
{
    public interface ITargetSubject
    {
        void RegisterObserver(ITargetObserver ob);

        void UnregisterObserver(ITargetObserver ob);
    }
}
