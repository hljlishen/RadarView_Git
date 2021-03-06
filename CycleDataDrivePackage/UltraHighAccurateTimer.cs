﻿using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;

namespace CycleDataDrivePackage
{
    public class UltraHighAccurateTimer
    {
        public delegate void ManualTimerEventHandler();
        public event ManualTimerEventHandler tick;
        private long clockFrequency;            // result of QueryPerformanceFrequency() 
        bool running = true;
        Thread thread;

        private int intervalMs;                     // interval in mimliseccond;

        /// 
        /// Timer inteval in milisecond
        /// 
        public int Interval
        {
            get => intervalMs;
            set
            {
                intervalMs = value;
                intevalTicks = (long)(value * (double)clockFrequency / 1000);
            }
        }
        private long intevalTicks;
        private long nextTriggerTime;               // the time when next task will be executed

        /// 
        /// Pointer to a variable that receives the current performance-counter value, in counts. 
        /// 
        /// 
        /// If the function succeeds, the return value is nonzero.
        /// 
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        /// 
        /// Pointer to a variable that receives the current performance-counter frequency, 
        /// in counts per second. 
        /// If the installed hardware does not support a high-resolution performance counter, 
        /// this parameter can be zero. 
        /// 
        /// 
        /// If the installed hardware supports a high-resolution performance counter, 
        /// the return value is nonzero.
        /// 
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);


        protected void OnTick()
        {
            tick?.Invoke();
        }

        public UltraHighAccurateTimer()
        {
            if (QueryPerformanceFrequency(out clockFrequency) == false)
            {
                // Frequency not supported
                throw new Win32Exception("QueryPerformanceFrequency() function is not supported");
            }

            thread = new Thread(new ThreadStart(ThreadProc));
            thread.Name = "HighAccuracyTimer";
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.Highest;
        }

        /// 
        /// 进程主程序
        /// 
        /// 
        private void ThreadProc()
        {
            long currTime;
            GetTick(out currTime);
            nextTriggerTime = currTime + intevalTicks;
            while (running)
            {
                while (currTime < nextTriggerTime)
                {
                    GetTick(out currTime);
                }   // wailt an interval
                nextTriggerTime = currTime + intevalTicks;
                //Console.WriteLine(DateTime.Now.ToString("ss.ffff"));
                tick?.Invoke();

                //if (tick != null)
                //{
                //    tick();
                //}
            }
        }

        public bool GetTick(out long currentTickCount)
        {
            if (QueryPerformanceCounter(out currentTickCount) == false)
                throw new Win32Exception("QueryPerformanceCounter() failed!");
            else
                return true;
        }

        public void Start()
        {
            thread.Start();
        }
        public void Stop()
        {
            running = false;
        }

        ~UltraHighAccurateTimer()
        {
            running = false;
            thread.Abort();
        }
    }
}
