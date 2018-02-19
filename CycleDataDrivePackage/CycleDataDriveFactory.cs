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

        //public static void RegisterCycleDataObserver(ICycleDataObserver ob) //注册周期数据观察者
        //{
        //    udpReader?.RegisterObserver(ob);
        //    binReader?.RegisterObserver(ob);
        //}
        private static BinFileReader CreateBinReader()
        {
            if (binReader == null)
                binReader = new BinFileReader();
            return binReader;
        }

        private static UdpCycleDataReader CreateUDPReader()
        {
            if (udpReader == null)
                udpReader = new UdpCycleDataReader();
            return udpReader;
        }

        public static ICycleDataSubject CreateCycleDataSubject(ReaderType type)    //文件读取
        {
            if (type == ReaderType.BIN)
            {
                return CreateBinReader();
            }
            else
            {
                return CreateUDPReader();
            }
        }
    }
}
