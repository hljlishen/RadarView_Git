namespace TargetManagerPackage
{
    public interface IWaveGateObserver
    {
        void NotifyChange(WaveGate wg, WaveGateSubjectNotifyState state);    //通知观察者波门被注销
    }
}
