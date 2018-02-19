using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public abstract class TargetManagerCommand : Command
    {
        protected ITargetManagerController targetController;

        public TargetManagerCommand()
        {
            targetController = TargetManagerFactory.CreateTargetManagerController();
        }

        public abstract void Execute();
    }
}
