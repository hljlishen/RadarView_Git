using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public class AntennaSetRotationRateCommand : AntennaCommand
    {
        RotateRate rate;
        public AntennaSetRotationRateCommand(RotateRate rate): base()
        {
            this.rate = rate;
        }

        public override void Execute() => antenna.SetRotateRate(rate);
    }
}
