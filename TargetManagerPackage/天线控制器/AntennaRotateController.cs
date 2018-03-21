using AntennaControlPackage;
using System;
using System.Threading;

namespace TargetManagerPackage
{
    public class AntennaRotateController
    {
        protected IServoController ServoController;
        protected RotateMode Mode;
        protected RotateDirection Direction;
        protected RotateRate Rate;

        public AntennaRotateController()
        {
            ServoController = ServoControllerFactory.CreateServoController();
            SetSweepModeData(RotateDirection.ClockWise, RotateRate.Rpm5);  //默认设置为顺时针5转每分钟
        }

        public void SetRotateRate(RotateRate rotateRate)
        {
            SetSweepModeData(Direction, rotateRate);
            StartSweep();
        }

        public void SetRotateDirection(RotateDirection rotateDirection)
        {
            SetSweepModeData(rotateDirection, Rate);
            StartSweep();
        }

        public void SetSweepModeData(RotateDirection direction, RotateRate rate)
        {
            Direction = direction;
            Rate = rate;
            Mode = GetSweepMode();     //默认值为顺时针5转每分钟
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
            ServoController.SetRotationRate(Mode);
        }

        public void ReverseSweepDirection()
        {
            Direction = ReversedDirection(Direction);
            Mode = GetSweepMode();
            StartSweep();
        }

        public AngleArea CalAntiInertiaSection(AngleArea area)
        {
            switch (Rate)
            {
                case RotateRate.Rpm0:
                    return area;
                case RotateRate.Rpm2:
                    return new AngleArea(area.BeginAngle - 5, area.EndAngle + 5);
                case RotateRate.Rpm5:
                    return new AngleArea(area.BeginAngle - 5, area.EndAngle + 5);
                case RotateRate.Rpm10:
                    return new AngleArea(area.BeginAngle + 14, area.EndAngle - 14);
                case RotateRate.Rpm20:
                    return new AngleArea(area.BeginAngle + 14, area.EndAngle - 14);
                default:
                    return null;
            }

        }

        public static RotateDirection ReversedDirection(RotateDirection currentDirection)
        {
            switch (currentDirection)
            {
                case RotateDirection.ClockWise:
                    return RotateDirection.CounterClockWise;
                case RotateDirection.CounterClockWise:
                    return RotateDirection.ClockWise;
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentDirection), currentDirection, null);
            }
        }

        protected RotateMode GetSweepMode()
        {
            return GetSweepMode(Direction, Rate);
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
