namespace TargetManagerPackage
{
    public enum RotateDirection
    {
        ClockWise = 1,
        CounterClockWise = 2
    }

    public enum RotateRate
    {
        Rpm0 = 0,
        Rpm2 = 2,
        Rpm5 = 5,
        Rpm10 = 10,
        Rpm20 = 20
    }


    public interface IAntennaController
    {
        void SetSectionSweepMode(AngleArea area);       //切换扇扫模式，并设置开始和结束角度

        void SetRotateDirection(RotateDirection direct);//停止扇扫，切换成正常扫描模式

        void SetRotateRate(RotateRate rate);

        void SetAntennaToZeroDegree();
    }
}
