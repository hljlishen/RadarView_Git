using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage.目标类
{
    class PredictDistanceFilter : ITargetTrackFilter
    {
        public bool Pass(TargetTrack t)
        {
            PolarCoordinate c1 = t.Locations[0];
            PolarCoordinate c2 = t.Locations[1];

            double dis = c1.DistanceTo(c2);
            return dis < 100;
        }
    }
}
