using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
     class Corelator_Test : TrackCorelator
    {
        public override void Corelate(Sector center, Sector left, Sector right)
        {
            //foreach (TargetDot dot in center.newDots)
            //{
            //    NotifyAllObservers(dot, NotifyType.Delete);
            //}
            NotifyDeleteSectorDot(center);
            center.newDots.Clear();
        }
    }
}
