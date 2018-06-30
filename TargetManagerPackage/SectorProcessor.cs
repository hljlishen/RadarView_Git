using System.Collections.Generic;

namespace TargetManagerPackage
{
    public class SectorProcessor : ITargetSubject
    {
        private static object _locker = new object();
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
            lock (_locker)
            {
                if (!Observers.Contains(ob) && ob != null)
                    Observers?.Add(ob);
            }
        }

        public void UnregisterObserver(ITargetObserver ob)
        {
            lock (_locker)
            {
                if (Observers.Contains(ob))
                    Observers?.Remove(ob);
            }
        }

        protected void NotifyUpdateSectorDot(Sector s)
        {
            lock (_locker)
            {
                foreach (ITargetObserver ob in Observers)
                    ob.NotifyUpdateSectorDot(s.NewDots, s.Index);
            }
        }

        protected void NotifyUpdateSectorTrack(Sector s)
        {
            lock(_locker)
            foreach (ITargetObserver ob in Observers)
                ob.NotifyUpdateSectorTrack(s.StableTracks, s.Index);
        }

        protected void NotifyDeleteSectorTrack(Sector s)
        {
            lock (_locker)
                foreach (ITargetObserver ob in Observers)
                    ob.NotifyUpdateSectorTrack(null, s.Index);   //传递null,表示没有航迹需要显示
        }

        protected void NotifyDeleteSectorDot(Sector s)
        {
            lock (_locker)
                foreach (ITargetObserver ob in Observers)
                    ob.NotifyUpdateSectorDot(null, s.Index);   //传递null,表示没有航迹需要显示
        }
    }
}
