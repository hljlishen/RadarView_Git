using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    class SetAntennaToZeroDegreeCommand : AntennaCommand
    {
        public override void Execute() => antenna.SetAntennaToZeroDegree();
    }
}
