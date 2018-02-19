using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntennaControlPackage
{
    public class ServoControllerFactory
    {
        private static ServoController servoController = null;

        public static IServoController CreateServoController()
        {
            if (servoController == null)
                servoController = new ServoController();
            return servoController;
        }
    }
}
