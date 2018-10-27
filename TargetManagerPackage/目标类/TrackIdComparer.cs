using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class TrackIdComparer : IComparer<TargetTrack>
    {
        public int Compare(TargetTrack x, TargetTrack y)
        {
            if (x.TrackId > y.TrackId)
                return 1;
            if (x.TrackId < y.TrackId)
                return -1;
            return 0;
        }
    }
}
