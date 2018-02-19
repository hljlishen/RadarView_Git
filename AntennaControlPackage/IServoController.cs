using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntennaControlPackage
{
    public enum RotationRate
    {
        Stop = 0,
        ClockWise_2 = 2,            //顺时针，每分钟2转
        ClockWise_5 = 5,
        ClockWise_10 = 10,
        ClockWise_20 = 20,
        CounterClockWise_2 = -2,
        CounterClockWise_5 = -5,
        CounterClockWise_10 = -10,
        CounterClockWise_20 = -20
    }
    public interface IServoController
    {
        void SetRotationRate(RotationRate rate);
    }
}
