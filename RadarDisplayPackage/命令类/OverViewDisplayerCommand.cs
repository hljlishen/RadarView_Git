using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    public abstract class OverViewDisplayerCommand : Command
    {
        protected OverViewDisplayer ovd;

        public OverViewDisplayerCommand(OverViewDisplayer ovd)
        {
            this.ovd = ovd;
        }

        public abstract void Execute();
    }
}
