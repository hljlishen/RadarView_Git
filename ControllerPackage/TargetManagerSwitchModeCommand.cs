using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public class TargetManagerSwitchModeCommand : TargetManagerCommand
    {
        TargetManagerMode mode;
        public TargetManagerSwitchModeCommand(TargetManagerMode mode) : base()
        {
            this.mode = mode;
        }

        public override void Execute()
        {
            targetController.SwitchMode(mode);
        }
    }
}
