using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public interface IWaveGateObserver
    {
        void NotifyChange(WaveGate wg, WaveGateSubjectNotifyState state);    //通知观察者波门被注销
    }
}
