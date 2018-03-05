using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public interface ISweepModeSubject
    {
        void RegisterSweepModeObserver(ISweepModeObserver ob);

        void UnregisterSweepModeObserver(ISweepModeObserver ob);

        AngleArea GetSweepSection();

        bool IsSectionSweeping();
    }
}
