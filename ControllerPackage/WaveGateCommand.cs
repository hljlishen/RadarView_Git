using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    abstract class WaveGateCommand : Command
    {
        protected IWaveGateController waveGateController;

        public WaveGateCommand()
        {
            waveGateController = TargetManagerFactory.CreateWaveGateController();
        }

        public abstract void Execute();
    }
}
