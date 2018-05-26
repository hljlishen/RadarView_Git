using TargetManagerPackage;

namespace RadarDisplayPackage
{
    abstract class WaveGateCommand : ICommand
    {
        protected IWaveGateController waveGateController;

        protected WaveGateCommand()
        {
            waveGateController = TargetManagerFactory.CreateWaveGateController();
        }

        public abstract void Execute();
    }
}
