using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    public class OverViewDisplayerAntennaControlCommand : OverViewDisplayerCommand
    {
        public OverViewDisplayerAntennaControlCommand( OverViewDisplayer ovd) : base(ovd)
        {

        }

        public override void Execute()
        {
            ovd.SwitchState(OverViewState.AntennaControl);
        }
    }
}
