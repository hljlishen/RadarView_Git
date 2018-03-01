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
        public override RotateDirection GetDirection()
        {
            return RotateDirection.Stopped;
        }

        public override void SetRotationRate(uint rate)     //停止状态设置转速无效
        {
            base.SetRotationRate(rate);
            servoController.SetRotationRate(AntennaControlPackage.RotateMode.Stop); //发送停止命令
        }
    }
}
