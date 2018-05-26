using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    public class TargetManagerDeleteActiveTargetCommand : TargetManagerCommand
    {
        public TargetManagerDeleteActiveTargetCommand() : base()
        {

        }

        public override void Execute()
        {
            targetController.DeleteActiveTarget();
        }
    }
}
