using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace CycleDataDrivePackage
{
    abstract class CycleDataReader : ICycleDataSubject
    {
        protected List<ICycleDataObserver> obs; //观察者列表
        protected Thread readDatathread;        //读取周期数据线程
        protected int CmdEnd1 = 0x33;           //包位第一位
        protected int CmdEnd2 = 0x55;           //包位第二位
        protected static int DataMaximumLength = 2000;   //每个方位单元的数据长度
        int interval = 1;
        private string source;

        public int Interval
        {
            get
            {
                return interval;
            }

            set
            {
                interval = value;
            }
        }

        public virtual string Source
        {
            get
            {
                return source;
            }

            set
            {
                source = value;
            }
        }

        public CycleDataReader()
        {
            obs = new List<ICycleDataObserver>();

            readDatathread = new Thread(ReadData);
            readDatathread.IsBackground = true;
        }

        public void RegisterObserver(ICycleDataObserver ob)
        {
            if (ob == null)
                return;
            if(!obs.Contains(ob))
            {
                obs.Add(ob);
            }
        }

        public void UnregisterObserver(ICycleDataObserver ob)
        {
            if (ob == null)
                return;
            if (obs.Contains(ob))
            {
                obs.Remove(ob);
            }
        }

        protected void NotifyAllObservers(AzimuthCell data)
        {
            foreach (ICycleDataObserver ob in obs)
                ob.NotifyNewCycleData(data);
        }

        protected abstract void ReadData();

        public void StartReading()
        {
            StopReading();
            readDatathread = new Thread(new ThreadStart(ReadData));
            readDatathread.IsBackground = false;
            readDatathread.Start();
        }

        public void StopReading()
        {
            if (readDatathread == null)
                return;

            if (readDatathread.ThreadState == ThreadState.Running)
                readDatathread.Abort();
        }

        public void Pause()
        {
            if(readDatathread.ThreadState == ThreadState.Running)
                readDatathread.Suspend();
        }

        public void Resume()
        {
            if (readDatathread.ThreadState == ThreadState.Suspended)
                readDatathread.Resume();
        }

        public virtual void Dispose()
        {
            StopReading();
            readDatathread = null;
        }

        public void RebindSource(string source)
        {
            Source = source;
        }
    }
}
