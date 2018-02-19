using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntennaControlPackage;

namespace TargetManagerPackage
{
    class AntennaState_ClockWise : AntennaState
    {
        public override AntennaDirection GetDirection()
        {
            return AntennaDirection.ClockWise;
        }

        public override void SetRotationRate(uint rate)
        {
            base.SetRotationRate(rate);
            servoController.SetRotationRate((RotationRate)rate);
        }
    }
}
