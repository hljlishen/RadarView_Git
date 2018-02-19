using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadarDisplayPackage
{
    public interface IControlStateSubject
    {
        void RegisterObserver(IControlStateObserver ob);

        void UnregisterObserver(IControlStateObserver ob);
    }
}
