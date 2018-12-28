using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage.目标类
{
    class FromBeginningStrategy : FindTrackIdStrategy
    {
        public override int NextId()
        {
            for (int i = 0; i < TrackMaximumCount; i++)
            {
                if (Id[i] != 1)
                {
                    Id[i] = 1;
                    return i + 1;
                }
            }

            return 0;
        }
    }
}
