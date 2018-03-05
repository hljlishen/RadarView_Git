using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public interface ILeaveAngleAreaObserver
    {
        void NotifyLeaveAngleArea(AngleArea area);
    }
}
