using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class WaveGateDeleteCommand : WaveGateCommand
    {
        WaveGate wg;
        public WaveGateDeleteCommand(WaveGate wg) : base()
        {
            this.wg = wg;
        }

        public override void Execute()
        {
            waveGateController.DelWaveGate(wg);
        }
    }
}
