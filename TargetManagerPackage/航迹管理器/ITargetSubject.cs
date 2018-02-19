using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public interface ITargetSubject
    {
        void RegisterObserver(ITargetObserver ob);

        void UnregisterObserver(ITargetObserver ob);
    }
}
