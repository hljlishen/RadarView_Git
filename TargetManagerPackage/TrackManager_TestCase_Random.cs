using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TargetManagerPackage
{
    public class TrackManager_TestCase_Random : ITargetDataProvider, ITargetManagerController
    {
        List<TargetTrack> tracks;
        List<TargetDot> dots;
        List<ITargetObserver> obs;
        private static TargetTrack track1 = null;
        private static TargetDot dot1 = null;
        private static TargetDot dot2 = null;
        private  Timer timerDot = null, timerTrack = null;
        private TargetManagerMode mode;

        public TrackManager_TestCase_Random()
        {
            mode = TargetManagerMode.Auto;

            tracks = new List<TargetTrack>();
            dots = new List<TargetDot>();
            obs = new List<ITargetObserver>();
            timerDot = new Timer();
            timerDot.Interval = 1000;
            timerDot.Tick += TimerDot_Tick;
            timerDot.Start();


            timerTrack = new Timer();
            timerTrack.Interval = 1000;
            timerTrack.Tick += TimerTrack_Tick;
            timerTrack.Start();

            if (dot1 == null)       //dot1是静态成员，只有第一次实例化时才对dot1赋值
            {
                Random rd = new Random();
                dot1 = new TargetDot();
                dot1.AZ = rd.Next(0, 180);
                dot1.EL = rd.Next(10, 19);
                dot1.Dis = rd.Next(2000, 4000);

                dot2 = new TargetDot();
                dot2.AZ = dot1.AZ + 2;
                dot2.EL = dot1.EL + 1;
                dot2.Dis = dot1.Dis + 100;
            }

            dots.Add(dot1);
            NotifyAllObservers(dot1, NotifyType.Add);
            dots.Add(dot2);
            NotifyAllObservers(dot2, NotifyType.Add);

            if (track1 == null)
            {
                Random rd = new Random();
                track1 = new TargetTrack();
                track1.trackID = 15;
                track1.AZ = rd.Next(180, 360);
                track1.EL = rd.Next(20, 89);
                track1.Dis = rd.Next(100, 4000);
            }

            float dis = 4000;
            float el = 10;
            float az = 0;
            float azStep = 30;

            for (int i = 1; az < 360; i++)
            {
                TargetTrack t = new TargetTrack();
                az = 15 + azStep * i;
                t.AZ = az;
                t.Dis = dis;
                t.EL = el;
                t.trackID = i;
                tracks.Add(t);
                NotifyAllObservers(t, NotifyType.Add);
            }
            tracks.Add(track1);
            NotifyAllObservers(track1, NotifyType.Add);
        }

        private void TimerTrack_Tick(object sender, EventArgs e)
        {
            float stepAz, stepEl, stepDis;
            Random rd = new Random();

            int seed = rd.Next(-1, 2);
            stepAz = 0.5f * seed;

            Random rd1 = new Random();
            int seed1 = rd1.Next(-3, 4);
            stepEl = 0.2f * seed1;

            rd = new Random();
            seed = rd.Next(-3, 4);
            stepDis = seed * 10f;

            track1.AZ += stepAz;
            if (track1.AZ >= 360)
                track1.AZ -= 360;
            if (track1.AZ < 0)
                track1.AZ += 360;

            track1.EL += stepEl;
            if (track1.EL < 2)
                track1.EL = 2;

            track1.Dis += stepDis;
            if (track1.Dis > 4000)
                track1.Dis = 4000;
            if (track1.Dis < 2000)
                track1.Dis = 2000;
            if(tracks.Contains(track1))
                NotifyAllObservers(track1, NotifyType.Update);
        }

        private void TimerDot_Tick(object sender, EventArgs e)
        {
            float stepAz, stepEl, stepDis;
            Random rd = new Random();

            int seed = rd.Next(-1, 2);
            stepAz = 0.5f * seed;

            rd = new Random();
            int seed2 = rd.Next(-3, 4);
            stepEl = 0.5f * seed2;

            rd = new Random();
            seed = rd.Next(-3, 4);
            stepDis = seed * 10f;

            dot1.AZ += stepAz;
            if (dot1.AZ >= 360)
                dot1.AZ -= 360;
            if (dot1.AZ < 0)
                dot1.AZ += 360;

            dot1.EL += stepEl;
            if (dot1.EL < 2)
                dot1.EL = 2;

            dot1.Dis += stepDis;
            if (dot1.Dis > 4000)
                dot1.Dis = 4000;
            if (dot1.Dis < 2000)
                dot1.Dis = 2000;
            NotifyAllObservers(dot1, NotifyType.Update);
        }

        public List<Target> GetTargetTracks()
        {
            List<Target> ls = new List<Target>();
            foreach(TargetTrack track in tracks)
            {
                ls.Add(track);
            }

            return ls;
        }

        public List<Target> GetTargetDots()
        {
            List<Target> ls = new List<Target>();
            foreach (TargetDot dot in dots)
            {
                ls.Add(dot);
            }

            return ls;
        }

        public void RegisterObserver(ITargetObserver ob)
        {
            if (!obs.Contains(ob) && ob != null)
                obs?.Add(ob);
        }

        public void UnregisterObserver(ITargetObserver ob)
        {
            if (obs.Contains(ob))
                obs?.Remove(ob);
        }

        protected void NotifyAllObservers(Target t, NotifyType type)
        {
            foreach (ITargetObserver ob in obs)
                ob.NotifyChange(t, type);
        }

        public void SwitchMode( TargetManagerMode mode)
        {
            this.mode = mode;
        }

        public TargetManagerMode GetMode()
        {
            return mode;
        }

        public void SelectTarget(Target t)  //View发送这条指令时已经设置了t.Active的值，目标管理器要做的只是通知所有其他观察者更新该航迹状态
        {
            //if(t is TargetTrack)
            //{
            //    //t.active = !t.active;
                NotifyAllObservers(t, NotifyType.Update);
            //}
        }

        public void DeleteActiveTarget()
        {
            for(int i = tracks.Count - 1; i >= 0; i--)
            {
                if(tracks[i].active)
                {
                    NotifyAllObservers(tracks[i], NotifyType.Delete);
                    tracks.Remove(tracks[i]);
                }
            }
        }
    }
}
