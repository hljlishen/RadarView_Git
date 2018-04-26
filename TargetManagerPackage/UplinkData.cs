using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class UplinkData : EthernetData
    {
        protected UplinkData(byte type, byte srcCode, byte desCode) : base(type, srcCode, desCode)
        {

        }

        public virtual byte[] Serialize()
        {
            return new[] {DataType, SourceDeviceCode, DestinationDeviceCode};
        }
    }
}
