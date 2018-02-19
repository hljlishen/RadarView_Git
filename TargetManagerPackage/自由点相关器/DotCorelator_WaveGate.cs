using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class DotCorelator_WaveGate : DotCorelator
    {
        IWaveGateDataProvider waveGateDataProvider;

        public DotCorelator_WaveGate(List<ITargetObserver> ls) : base(ls)
        {
            waveGateDataProvider = TargetManagerFactory.CreateWaveGateDataProvider();
        }
        public DotCorelator_WaveGate() : base()
        {
            waveGateDataProvider = TargetManagerFactory.CreateWaveGateDataProvider();
        }
        protected override bool DotsCanCorelate(TargetDot dot1, TargetDot dot2)
        {
            WaveGate gate1 = waveGateDataProvider.IsTargetInWaveGate(dot1);
            WaveGate gate2 = waveGateDataProvider.IsTargetInWaveGate(dot2);

            if (gate1 != null && gate2 != null && gate1 == gate2)
                return true;
            else
                return false;
        }
    }
}
