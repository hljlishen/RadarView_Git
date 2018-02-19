using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public class AntennaSetNormalSweepModeCommand : AntennaCommand
    {
        AntennaDirection direction;
        public AntennaSetNormalSweepModeCommand( AntennaDirection direction) : base()
        {
            this.direction = direction;
        }

        public override void Execute()
        {
            antenna.SetNormalSweepMode(direction);
        }
    }
}
