using System.Collections.Generic;
using System.Threading;
using Utilities;

namespace CycleDataDrivePackage
{
    public abstract class CycleDataReader : ICycleDataSubject
    {
        protected List<ICycleDataObserver> Obs; //观察者列表
        protected Thread ReadDatathread;        //读取周期数据线程
        protected int CmdEnd1 = 0x33;           //包位第一位
        protected int CmdEnd2 = 0x55;           //包位第二位
        protected static int DataMaximumLength = 4000;   //每个方位单元的数据长度
        private const int IntervalMax = 30;
        private const int IntervalMin = 1;
        private const int IntervalStep = 5;
        public static float AzAdjustment { get; set; } = 0; //方位角度调整值

        public int Interval { get; set; } = 1;

        public virtual string Source { get; set; }

        protected CycleDataReader()
        {
            Obs = new List<ICycleDataObserver>();

            ReadDatathread = new Thread(ReadData) { IsBackground = true };
            //timer = new UltraHighAccurateTimer() {Interval = 5};
            //timer.tick += ReadData;
        }

        public void RegisterObserver(ICycleDataObserver ob)
        {
            if (ob == null)
                return;
            lock (Obs)
            {
                if (!Obs.Contains(ob))
                {
                    Obs.Add(ob);
                }
            }
        }

        public void UnregisterObserver(ICycleDataObserver ob)
        {
            if (ob == null)
                return;
            lock (Obs)
            {
                if (Obs.Contains(ob))
                {
                    Obs.Remove(ob);
                }
            }
        }

        protected virtual void NotifyAllObservers(byte[] rawData)
        {
            lock (Obs)
            {
                foreach (ICycleDataObserver ob in Obs)
                    ob.NotifyNewCycleData(AdjustCycleDataAngle( rawData));
            }
        }

        private byte[] AdjustCycleDataAngle(byte[] cycleData)
        {
            (float antennaAngle, int angleInt) = AzimuthCell.GetAngleFromCycleData(cycleData);
            antennaAngle = Tools.ReverAngleDirection(antennaAngle);
            antennaAngle = Tools.StandardAngle(antennaAngle - AzAdjustment);
            int angleI = (int)(antennaAngle * 65535 / 360);
            byte azHigh = (byte)((angleI >> 8) & 0xff);
            byte azLow = (byte)angleI;
            List<byte> newData = new List<byte>(cycleData)
            {
                [28] = azHigh,
                [29] = azLow,
                [44] = azHigh,
                [45] = azLow
            };

            return newData.ToArray();
        }

        protected abstract void ReadData();

        public virtual void StartReading()
        {
            StopReading();
            ReadDatathread = new Thread(ReadData) { IsBackground = false };
            ReadDatathread.Start();
        }

        public void StopReading()
        {
            if (ReadDatathread?.ThreadState == ThreadState.Running)
                ReadDatathread.Abort();
        }

        public void Pause()
        {
            //if(ReadDatathread.ThreadState == ThreadState.Running)
                ReadDatathread.Suspend();
        }

        public void Resume()
        {
            if (ReadDatathread.ThreadState == ThreadState.Suspended)
                ReadDatathread.Resume();
        }

        public virtual void Dispose()
        {
            StopReading();
            ReadDatathread.Abort();
        }

        public void RebindSource(string source) => Source = source;

        public void SpeedUp()       //不合理设计，SpeedUp和SpeedDown函数只对BinDataSourceReader有意义，有待重构
        {
            lock (Source)
            {
                Interval -= IntervalStep;
                if (Interval < IntervalMin)
                    Interval = IntervalMin;
            }
        }

        public void SpeedDown()     //不合理设计，SpeedUp和SpeedDown函数只对BinDataSourceReader有意义
        {
            lock (Source)
            {
                Interval += IntervalStep;
                if (Interval > IntervalMax)
                    Interval = IntervalMax;
            }
        }
    }
}
