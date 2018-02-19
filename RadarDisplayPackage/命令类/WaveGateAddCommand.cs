using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class WaveGateAddCommand : WaveGateCommand
    {
        WaveGate wg;
        public WaveGateAddCommand(WaveGate wg) : base()
        {
            this.wg = wg;
        }

        public override void Execute()
        {
            waveGateController.AddWaveGate(wg);
        }
    }
}
