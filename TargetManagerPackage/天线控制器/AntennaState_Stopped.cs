using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntennaControlPackage;

namespace TargetManagerPackage
{
    class AntennaState_Stopped : AntennaState
    {
        public override AntennaDirection GetDirection()
        {
            return AntennaDirection.Stopped;
        }

        public override void SetRotationRate(uint rate)     //停止状态设置转速无效
        {
            base.SetRotationRate(rate);
            servoController.SetRotationRate(RotationRate.Stop); //发送停止命令
        }
    }
}
