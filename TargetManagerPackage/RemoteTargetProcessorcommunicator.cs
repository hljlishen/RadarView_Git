using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    internal class RemoteTargetProcessorcommunicator
    {
        private Socket _remoteSocket;
        private EndPoint _remoteEndPoint;
        private const byte SendSectorDotTargetsHead = 0xA4;
        private const int ReceiveBytesMax = 10000;
        private TargetManager targetManager;

        public RemoteTargetProcessorcommunicator(TargetManager t)
        {

            (_remoteSocket, _remoteEndPoint) =
                UdpCycleDataReader.GetUdpConnectionObjects("192.168.1.13", "10001", "192.168.1.13", "2010");

            targetManager = t;
        }

        public void SendRawData(byte[] rawData)
        {
            List<byte> ls = new List<byte>(SendSectorDotTargetsHead);   //命令头
            ls.AddRange(rawData); //添加扇区编号

            Thread t = new Thread(SendData);
            t.Start(ls.ToArray());
        }

        private void SendData(object data)
        {
            byte[] d = (byte[]) data;
            _remoteSocket.SendTo(d, _remoteEndPoint);
        }

        public void StartReceiveData()
        {
            Thread t = new Thread(ReadData);
            t.Start();
        }

        public void ReadData()
        {
            while (true)
            {
                byte[] data = new byte[ReceiveBytesMax];
                _remoteSocket.ReceiveFrom(data, ref _remoteEndPoint);
            }
        }
    }
}
