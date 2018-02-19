using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    //class AntennaController_Test : IAntennaController, IAntennaDataProvider,ICycleDataObserver
    //{
    //    float preAngle = 0;
    //    static float antennaAngle = 0;
    //    static float beginAngle = -1;
    //    static float endAngle = -1;
    //    static int state = 0;  //0是正常扫描，1是扇扫
    //    static float step = 1;
    //    static Timer timer = null;
    //    static bool start = false;
    //    List<IAntennaObserver> obs;
    //    ICycleDataSubject cycleDataSubject;
    //    int rotationCount = 0;

    //    public AntennaController_Test()
    //    {
    //        if (timer == null)
    //        {
    //            timer = new Timer();
    //            timer.Interval = 20;
    //            timer.Tick += Timer_Tick;
    //            timer.Start();
    //        }

    //        obs = new List<IAntennaObserver>();
    //        cycleDataSubject = CycleDataDriveFactory.CreateCycleDataSubject();
    //        cycleDataSubject.RegisterObserver(this);
    //    }

    //    private void Timer_Tick(object sender, EventArgs e)
    //    {
    //        if (antennaAngle < 0)
    //            antennaAngle += 360;
    //        antennaAngle += step;
    //        antennaAngle %= 360; //360度取模

    //        if (preAngle > 300 && antennaAngle < 10)
    //            rotationCount++;
    //        preAngle = antennaAngle;

    //        NotifyChange();     //通知观察者

    //        if (state == 0)
    //            return;
    //        else   //扇扫模式
    //        {
    //            if (antennaAngle > beginAngle && antennaAngle < endAngle)
    //            {
    //                start = true;
    //            }
    //            if(start)
    //            {
    //                if (antennaAngle > endAngle)
    //                    step *= -1;
    //                if (antennaAngle < beginAngle)
    //                    step *= -1;
    //            }
    //        }
    //    }

    //    public void NotifyChange()
    //    {
    //        foreach (IAntennaObserver ob in obs)
    //            ob.AntennaNotifyChange();
    //    }

    //    public void RegisterObserver(IAntennaObserver ob)
    //    {
    //        if(ob != null && !obs.Contains(ob))
    //            obs.Add(ob);
    //    }

    //    public void UnregisterObserver(IAntennaObserver ob)
    //    {
    //        if (ob != null && obs.Contains(ob))
    //            obs.Remove(ob);
    //    }

    //    public float GetCurrentAntennaAngle()
    //    {
    //        return antennaAngle;
    //    }

    //    public void SetSectionSweepMode(float begin, float end) //扇扫模式
    //    {
    //        beginAngle = begin;
    //        endAngle = end;
    //        state = 1;  //扇扫状态

    //        NotifyChange();
    //    }

    //    public void SetNormalSweepMode(AntennaDirection direction)       //切换成正常扫描模式
    //    {
    //        beginAngle = -1;
    //        endAngle = -1;
    //        start = false;
    //        step = Math.Abs(step);
    //        state = 0;  //正常扫描状态

    //        if (direction == AntennaDirection.CountClockWise)
    //            step *= -1;

    //        NotifyChange();
    //    }

    //    public void SetRotationRate(int countPerMinute)
    //    {
    //        bool isPositive = step > 0;

    //        step = GetStep(countPerMinute);

    //        if (!isPositive)
    //            step *= -1;

    //        NotifyChange();
    //    }

    //    public float GetSweepBeginAngle()
    //    { return beginAngle; }

    //    public float GetSweepEndAngle()
    //    { return endAngle; }

    //    public int GetSweepState()
    //    {
    //        return state;
    //    }

    //    private float GetStep(int rate)
    //    {
    //        if (rate < 0)
    //            return step;

    //        float s;
    //        if (rate == 2)
    //        {
    //            s = 0.5f;
    //        }
    //        else if (rate == 5)
    //        {
    //            s = 1;
    //        }
    //        else if (rate == 10)
    //        {
    //            s = 1.5f;
    //        }
    //        else
    //        {
    //            s = 2;
    //        }

    //        return s;
    //    }

    //    public void Notify(AzimuthCell data)
    //    {

    //    }

    //    public int GetRotationCount()
    //    {
    //        return rotationCount;
    //    }
    ////}
}
