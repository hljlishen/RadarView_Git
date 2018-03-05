using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public enum SweepMode
    {
        Normal,
        Section
    }
    public interface ISweepModeObserver
    {
        void NotifySweepModeChange(SweepMode mode);
    }
}
