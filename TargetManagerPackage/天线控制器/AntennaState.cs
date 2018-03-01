using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntennaControlPackage;

namespace TargetManagerPackage
{
    abstract class AntennaState
    {
        float preciousAntennaAngle = 0;
        float currentAntennaAngle = 0;
        protected IServoController servoController;

        public AntennaState()
        {
            servoController = ServoControllerFactory.CreateServoController();   //伺服控制器
        }

        public abstract RotateDirection GetDirection();

        public float PreciousAntennaAngle
        {
            get
            {
                return preciousAntennaAngle;
            }
            set { preciousAntennaAngle = value; }
        }

        public float CurrentAntennaAngle
        {
            get
            {
                return currentAntennaAngle;
            }

            set
            {
                preciousAntennaAngle = currentAntennaAngle;
                currentAntennaAngle = value;
            }
        }

        public static uint Rate { get; private set; } = 10;

        public virtual void SetRotationRate(uint r)    //设置转速
        {
            Rate = r;
        }
    }
}
