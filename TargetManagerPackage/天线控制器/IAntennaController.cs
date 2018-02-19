using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public enum AntennaDirection
    {
        Stopped = 0,
        ClockWise = 1,
        CounterClockWise = 2
    }
    public interface IAntennaController
    {
        void SetSectionSweepMode(AngleArea area);       //切换扇扫模式，并设置开始和结束角度

        void SetNormalSweepMode(AntennaDirection direct);       //切换成正常扫描模式

        void SetRotationRate(uint countPerMinute);               //设置转速
    }
}
