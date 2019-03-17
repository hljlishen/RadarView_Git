using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    class SetAntennaToDegreeCommand : AntennaCommand
    {
        private float stopDegree;
        public SetAntennaToDegreeCommand(float degree)
        {
            this.stopDegree = degree;
        }
        public override void Execute() => antenna.SetAntennaToZeroDegree(stopDegree);
    }
}
