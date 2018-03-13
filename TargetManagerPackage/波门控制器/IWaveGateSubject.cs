namespace TargetManagerPackage
{
    public enum WaveGateSubjectNotifyState
    {
        Add,
        Delete
    }
    public interface IWaveGateSubject
    {
        void RegisterObserver(IWaveGateObserver ob);        //注册观察者

        void UnregisterObserver(IWaveGateObserver ob);      //注销观察者

    }
}
