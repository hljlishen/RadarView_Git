using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
