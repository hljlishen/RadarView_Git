using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class WaveGateDeleteCommand : WaveGateCommand
    {
        WaveGate wg;
        public WaveGateDeleteCommand(WaveGate wg)
        {
            this.wg = wg;
        }

        public override void Execute() => waveGateController.DelWaveGate(wg);
    }
}
