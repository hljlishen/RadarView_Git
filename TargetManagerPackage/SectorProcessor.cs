using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    abstract class SectorProcessor : ITargetSubject
    {
       List<ITargetObserver> obs;  //观察者列表

        public SectorProcessor()
        {
            obs = new List<ITargetObserver>();
        }

        public SectorProcessor(List<ITargetObserver> ls)
        {
            obs = ls;
        }

        public List<ITargetObserver> Observers
        {
            get
            {
                return obs;
            }

            set
            {
                obs = value;
            }
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

        //protected void NotifyAllObservers(Target t, NotifyType type)
        //{
        //    foreach (ITargetObserver ob in obs)
        //        ob.NotifyAntennaDataChange(t, type);
        //}

        protected void NotifyUpdateSectorDot(Sector s)
        {
            foreach (ITargetObserver ob in obs)
                ob.NotifyUpdateSectorDot(s.newDots, s.index);
        }

        protected void NotifyUpdateSectorTrack(Sector s)
        {
            foreach (ITargetObserver ob in obs)
                ob.NotifyUpdateSectorTrack(s.tracks, s.index);
        }

        protected void NotifyDeleteSectorTrack(Sector s)
        {
            foreach (ITargetObserver ob in obs)
                ob.NotifyUpdateSectorTrack(null, s.index);   //传递null,表示没有航迹需要显示
        }

        protected void NotifyDeleteSectorDot(Sector s)
        {
            foreach (ITargetObserver ob in obs)
                ob.NotifyUpdateSectorDot(null, s.index);   //传递null,表示没有航迹需要显示
        }
    }
}
