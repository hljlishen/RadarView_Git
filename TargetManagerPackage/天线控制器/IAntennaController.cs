using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public enum RotateDirection
    {
        ClockWise = 1,
        CounterClockWise = 2
    }
    public interface IAntennaController
    {
        void SetSectionSweepMode(AngleArea area);       //切换扇扫模式，并设置开始和结束角度

        void SetNormalSweepMode(RotateDirection direct);       //切换成正常扫描模式

        void SetRotationRate(uint countPerMinute);               //设置转速
    }
}
