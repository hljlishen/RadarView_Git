using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage.目标类
{
    public abstract class FindTrackIdStrategy
    {
        public const int TrackMaximumCount = 64;
        protected readonly int[] Id = new int[TrackMaximumCount];

        public abstract int NextId();
        public void ReleaseId(int id)
        {
            Id[id - 1] = 0;
        }
    }
}
