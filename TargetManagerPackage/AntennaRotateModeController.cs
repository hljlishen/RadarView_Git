using AntennaControlPackage;
using System;
using System.Threading;

namespace TargetManagerPackage
{
    public enum RotateRate
    {
        Rpm0 = 0,
        Rpm2 = 2,
        Rpm5 = 5,
        Rpm10 = 10,
        Rpm20 = 20
    }

    public class AntennaRotateModeController
    {
        protected IServoController ServoController;
        protected RotateMode mode;
        protected RotateDirection direction;
        protected RotateRate rate;

        public RotateMode Mode => mode;
        public RotateRate Rate => rate;
        public RotateDirection Direction => direction;

        public AntennaRotateModeController()
        {
            ServoController = ServoControllerFactory.CreateServoController();
            SetSweepModeData(RotateDirection.ClockWise, RotateRate.Rpm5);  //默认设置为顺时针5转每分钟
        }

        public void SetSweepModeData(RotateDirection direction, RotateRate rate)
        {
            this.direction = direction;
            this.rate = rate;
            mode = GetSweepMode();     //默认值为顺时针5转每分钟
        }
        public void StartSweep()
        {
            var startSweepThread = new Thread(Sweep);
            startSweepThread.Start();
        }

        protected void Sweep()
        {
            ServoController.SetRotationRate(RotateMode.Stop);
            Thread.Sleep(200);
            ServoController.SetRotationRate(mode);
        }

        public void ReverseSweepDirection()
        {
            direction = ReversedDirection(direction);
            StartSweep();
        }

        public static RotateDirection ReversedDirection(RotateDirection d)
        {
            switch (d)
            {
                case RotateDirection.ClockWise:
                    return RotateDirection.CounterClockWise;
                case RotateDirection.CounterClockWise:
                    return RotateDirection.ClockWise;
                default:
                    throw new ArgumentOutOfRangeException(nameof(d), d, null);
            }
        }

        protected RotateMode GetSweepMode()
        {
            return GetSweepMode(direction, rate);
        }
        public static RotateMode GetSweepMode(RotateDirection direction, RotateRate rate)
        {
            int sign;

            switch (direction)
            {
                case RotateDirection.ClockWise:
                    sign = 1;
                    break;
                case RotateDirection.CounterClockWise:
                    sign = -1;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return (RotateMode)((int)rate * sign);
        }
    }
}
