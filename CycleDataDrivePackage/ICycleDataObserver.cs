using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage
{
    public interface ICycleDataObserver
    {
        void NotifyNewCycleData(AzimuthCell data);
    }
}
