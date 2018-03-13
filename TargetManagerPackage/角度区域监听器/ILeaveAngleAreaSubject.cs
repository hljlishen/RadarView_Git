namespace TargetManagerPackage
{
    public interface ILeaveAngleAreaSubject
    {
        void RegisterAngleArea(ILeaveAngleAreaObserver ob, AngleArea area);

        void UnregisterAngleArea(ILeaveAngleAreaObserver ob, AngleArea area);
    }
}
