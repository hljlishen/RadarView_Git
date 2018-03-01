using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public interface ILeaveAngleAreaSubject
    {
        void RegisterAngleArea(ILeaveAngleAreaObserver ob, AngleArea area);

        void UnregisterAngleArea(ILeaveAngleAreaObserver ob, AngleArea area);
    }
}
