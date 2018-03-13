using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CycleDataDrivePackage
{
    public enum ReaderType
    {
        UDP,
        BIN
    }
    public class CycleDataDriveFactory
    {
        private static UdpCycleDataReader udpReader = null;
        private static BinFileReader binReader =  null;

        private static BinFileReader CreateBinReader() => binReader ?? (binReader = new BinFileReader());

        private static UdpCycleDataReader CreateUdpReader() => udpReader ?? (udpReader = new UdpCycleDataReader());

        public static ICycleDataSubject CreateCycleDataSubject(ReaderType type)    //文件读取
        {
            if (type == ReaderType.BIN)
            {
                return CreateBinReader();
            }
            else
            {
                return CreateUdpReader();
            }
        }
    }
}
