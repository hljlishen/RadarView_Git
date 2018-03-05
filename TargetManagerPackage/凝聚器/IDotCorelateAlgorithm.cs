using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    interface IDotCorelateAlgorithm
    {
        bool CorelateDot(TargetDot dot1, TargetDot dot2);
    }
}
