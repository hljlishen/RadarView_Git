
namespace CycleDataDrivePackage
{
    public enum ReaderType
    {
        Udp,
        Bin
    }
    public class CycleDataDriveFactory
    {
        private static UdpCycleDataReader _udpReader;
        private static BinFileReader _binReader;

        //private static BinFileReader CreateBinReader() => _binReader ?? (_binReader = new BinFileReader());
        private static BinFileReader CreateBinReader() => new BinFileReader();

        private static UdpCycleDataReader CreateUdpReader() => _udpReader ?? (_udpReader = new UdpCycleDataReader());

        public static ICycleDataSubject CreateCycleDataSubject(ReaderType type)    //文件读取
        {
            if (type == ReaderType.Bin)
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
