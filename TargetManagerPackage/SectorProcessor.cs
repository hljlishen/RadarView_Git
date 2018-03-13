using System.Collections.Generic;

namespace TargetManagerPackage
{
    abstract class SectorProcessor : ITargetSubject
    {
        protected SectorProcessor()
        {
            Observers = new List<ITargetObserver>();
        }

        protected SectorProcessor(List<ITargetObserver> ls)
        {
            Observers = ls;
        }

        public List<ITargetObserver> Observers { get; set; }

        public void RegisterObserver(ITargetObserver ob)
        {
            if (!Observers.Contains(ob) && ob != null)
                Observers?.Add(ob);
        }

        public void UnregisterObserver(ITargetObserver ob)
        {
            if (Observers.Contains(ob))
                Observers?.Remove(ob);
        }

        //protected void NotifyAllObservers(Target t, NotifyType type)
        //{
        //    foreach (ITargetObserver ob in obs)
        //        ob.NotifyAntennaDataChange(t, type);
        //}

        protected void NotifyUpdateSectorDot(Sector s)
        {
            foreach (ITargetObserver ob in Observers)
                ob.NotifyUpdateSectorDot(s.newDots, s.index);
        }

        protected void NotifyUpdateSectorTrack(Sector s)
        {
            foreach (ITargetObserver ob in Observers)
                ob.NotifyUpdateSectorTrack(s.tracks, s.index);
        }

        protected void NotifyDeleteSectorTrack(Sector s)
        {
            foreach (ITargetObserver ob in Observers)
                ob.NotifyUpdateSectorTrack(null, s.index);   //传递null,表示没有航迹需要显示
        }

        protected void NotifyDeleteSectorDot(Sector s)
        {
            foreach (ITargetObserver ob in Observers)
                ob.NotifyUpdateSectorDot(null, s.index);   //传递null,表示没有航迹需要显示
        }
    }
}
