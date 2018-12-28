using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage.目标类
{
    abstract class FindTrackIdStrategy
    {
        protected const int TrackMaximumCount = 200;
        protected static readonly int[] Id = new int[TrackMaximumCount];

        public abstract int NextId();
        public static void ReleaseId(int id)
        {
            Id[id - 1] = 0;
        }
    }
}
