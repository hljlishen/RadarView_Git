using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class FreeDotDeleter : SectorProcessor
    {
        public void DeleteFreeDot(Sector s)
        {
            s.oldDots.Clear();
        }
    }
}
