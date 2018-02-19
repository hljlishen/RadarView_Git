using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class DotCorelatorAlgorithm_DistanceNear : IDotCorelateAlgorithm
    {
        private const float DotNearThreshold = 50f;
        public bool CorelateDot(TargetDot dot1, TargetDot dot2)
        {
            float dis = dot1.DistanceTo(dot2);

            if (dis < DotNearThreshold)
                return true;
            else
                return false;
        }
    }
}
