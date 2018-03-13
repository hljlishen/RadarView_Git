namespace TargetManagerPackage
{
    public enum TargetManagerMode
    {
        Auto,
        SemiAuto,
        Manual,
        Intelligent
    }
    public interface ITargetManagerController
    {
        void SwitchMode(TargetManagerMode mode);

        void SelectTarget(Target t);

        void DeleteActiveTarget();

        void DeleteOutRangedTargets(AngleArea area);

        void ClearRawData();
    }
}
