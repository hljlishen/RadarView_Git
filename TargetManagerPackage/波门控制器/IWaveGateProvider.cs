using System.Collections.Generic;

namespace TargetManagerPackage
{
    public interface IWaveGateDataProvider : IWaveGateSubject
    {
        List<WaveGate> GetWaveGates();

        WaveGate IsTargetInWaveGate(Target t);
    }
}
