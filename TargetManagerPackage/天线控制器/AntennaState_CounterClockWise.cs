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
        public override RotateDirection GetDirection()
        {
            return RotateDirection.CounterClockWise;
        }

        public override void SetRotationRate(uint rate)
        {
            base.SetRotationRate(rate);
            servoController.SetRotationRate((AntennaControlPackage.RotateMode)(-rate));
        }
    }
}
