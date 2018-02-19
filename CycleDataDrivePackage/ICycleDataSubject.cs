using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage
{
   public interface ICycleDataSubject : IDisposable
    {
        void RegisterObserver(ICycleDataObserver ob);

        void UnregisterObserver(ICycleDataObserver ob);

        void RebindSource(string source);

        void StartReading();
    }
}
