using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class X80Data : UplinkData
    {
        private const byte LocalDeviceCode = 0x11;
        private const byte SystmDeviceCode = 0x31;
        private static int SendCount = 0;

        public X80Data(TargetTrack t) : base(0x80, LocalDeviceCode, SystmDeviceCode)
        {

        }
    }
}
