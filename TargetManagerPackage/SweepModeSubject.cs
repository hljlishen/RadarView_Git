using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    internal class SweepModeSubject : ISweepModeSubject
    {
        private AngleArea _sweepSection;
        private readonly List<ISweepModeObserver> _sweepModeObservers;

        public SweepModeSubject()
        {
            _sweepModeObservers = new List<ISweepModeObserver>();
        }

        public void RegisterSweepModeObserver(ISweepModeObserver ob)
        {
            if (ob != null && !_sweepModeObservers.Contains(ob))
                _sweepModeObservers.Add(ob);
        }

        public void UnregisterSweepModeObserver(ISweepModeObserver ob)
        {
            if (ob != null && _sweepModeObservers.Contains(ob))
                _sweepModeObservers.Remove(ob);
        }

        public AngleArea GetSweepSection() => _sweepSection;

        public bool IsSectionSweeping()
        {
            throw new NotImplementedException();
        }
    }
}
