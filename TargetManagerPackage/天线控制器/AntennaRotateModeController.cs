using AntennaControlPackage;
using System;
using System.Threading;

namespace TargetManagerPackage
{
    public class AntennaRotateModeController
    {
        protected IServoController ServoController;
        protected RotateMode mode;
        protected RotateDirection direction;
        protected RotateRate rate;

        public AntennaRotateModeController()
        {
            ServoController = ServoControllerFactory.CreateServoController();
            SetSweepModeData(RotateDirection.ClockWise, RotateRate.Rpm5);  //默认设置为顺时针5转每分钟
        }

        public void SetRotateRate(RotateRate rotateRate)
        {
            SetSweepModeData(direction, rotateRate);
            StartSweep();
        }

        public void SetRotateDirection(RotateDirection rotateDirection)
        {
            SetSweepModeData(rotateDirection, rate);
            StartSweep();
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

        public void StopSweep()
        {
            ServoController.SetRotationRate(RotateMode.Stop);
        }

        protected void Sweep()
        {
            StopSweep();
            Thread.Sleep(200);
            ServoController.SetRotationRate(mode);
        }

        public void ReverseSweepDirection()
        {
            direction = ReversedDirection(direction);
            mode = GetSweepMode();
            StartSweep();
        }

        public AngleArea CalAntiInertiaSection(AngleArea area)
        {
            switch (rate)
            {
                case RotateRate.Rpm0:
                    return area;
                case RotateRate.Rpm2:
                    return new AngleArea(area.BeginAngle - 20, area.EndAngle + 20);
                case RotateRate.Rpm5:
                    return new AngleArea(area.BeginAngle - 10, area.EndAngle + 10);
                case RotateRate.Rpm10:
                    return new AngleArea(area.BeginAngle + 14, area.EndAngle - 14);
                case RotateRate.Rpm20:
                    return new AngleArea(area.BeginAngle + 14, area.EndAngle - 14);
                default:
                    return null;
            }

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
