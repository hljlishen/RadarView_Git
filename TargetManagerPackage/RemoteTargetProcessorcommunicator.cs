using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                UdpCycleDataReader.GetUdpConnectionObjects("192.168.1.13", "10001", "127.0.0.1", "2010");

            targetManager = t;
        }

        public void SendRawData(byte[] rawData)
        {
            List<byte> ls = new List<byte>(SendSectorDotTargetsHead);   //命令头
            ls.AddRange(rawData); //添加扇区编号

            Thread t = new Thread(()=>_remoteSocket.SendTo(ls.ToArray(),_remoteEndPoint));
            t.Start();
            //_remoteSocket.BeginSendTo(ls.ToArray(), 0, rawData.Length, SocketFlags.Broadcast, _remoteEndPoint,null,null);
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
