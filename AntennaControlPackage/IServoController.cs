namespace AntennaControlPackage
{
    public enum RotateMode
    {
        Stop = 0,
        ClockWise2 = 2,            //顺时针，每分钟2转
        ClockWise5 = 5,
        ClockWise10 = 10,
        ClockWise20 = 20,
        CounterClockWise2 = -2,
        CounterClockWise5 = -5,
        CounterClockWise10 = -10,
        CounterClockWise20 = -20
    }
    public interface IServoController
    {
        void SetRotationRate(RotateMode mode);
    }
}
