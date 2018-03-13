namespace AntennaControlPackage
{
    public class ServoControllerFactory
    {
        private static ServoController _servoController;

        public static IServoController CreateServoController()
        {
            return _servoController ?? (_servoController = new ServoController());
        }
    }
}
