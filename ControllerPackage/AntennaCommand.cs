using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public abstract class AntennaCommand : Command
    {
        protected IAntennaController antenna;   //天线控制器

        public AntennaCommand()
        {
            antenna = TargetManagerFactory.CreateAntennaContoller();
        }

        public abstract void Execute();
    }
}
