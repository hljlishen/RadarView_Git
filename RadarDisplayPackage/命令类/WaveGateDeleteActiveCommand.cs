using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class WaveGateDeleteActiveCommand : WaveGateCommand
    {
        public override void Execute()
        {
            waveGateController.DeleteActiveWaveGate();
        }
    }
}
