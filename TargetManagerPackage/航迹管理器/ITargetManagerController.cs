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

        void SwitchDotSource(bool sourceFlag); //true为原始视频，false为远程数据处理软件发送的凝聚点
    }
}
