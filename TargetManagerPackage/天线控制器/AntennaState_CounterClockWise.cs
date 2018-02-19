using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntennaControlPackage;

namespace TargetManagerPackage
{
    class AntennaState_CounterClockWise : AntennaState
    {
        public override AntennaDirection GetDirection()
        {
            return AntennaDirection.CounterClockWise;
        }

        public override void SetRotationRate(uint rate)
        {
            base.SetRotationRate(rate);
            servoController.SetRotationRate((RotationRate)(-rate));
        }
    }
}
