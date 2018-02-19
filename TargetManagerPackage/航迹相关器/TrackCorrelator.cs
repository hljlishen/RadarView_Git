using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    abstract class TrackCorelator : SectorProcessor
    {
        public abstract void Corelate(Sector center, Sector left, Sector right);
    }
}
