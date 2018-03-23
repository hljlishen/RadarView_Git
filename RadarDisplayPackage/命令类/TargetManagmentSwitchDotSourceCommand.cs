using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    class TargetManagmentSwitchDotSourceCommand : TargetManagerCommand
    {
        private readonly bool _dotSource;
        public TargetManagmentSwitchDotSourceCommand(bool source)
        {
            _dotSource = source;
        }
        public override void Execute()
        {
            targetController.SwitchDotSource(_dotSource);
        }
    }
}
