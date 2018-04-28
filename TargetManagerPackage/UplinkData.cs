using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class UplinkData : EthernetData
    {
        protected UplinkData(byte typeCode, byte srcCode, byte desCode) : base(typeCode, srcCode, desCode)
        {

        }

        public virtual byte[] Serialize()
        {
            return new[] {CommandTypeCode, SourceDeviceCode, DestinationDeviceCode};
        }
    }
}
