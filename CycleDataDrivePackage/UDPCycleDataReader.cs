using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace CycleDataDrivePackage
{
    class UdpCycleDataReader : CycleDataReader
    {
        private readonly DataRecorder _recorder;

        private Socket _udpSocket;
        private EndPoint _remote;

        public UdpCycleDataReader()
        {
            _recorder = new DataRecorder();
        }

        protected override void ReadData()
        {
            _udpSocket = SetupUdpSocketObject("192.168.1.13", Source, "192.168.1.5", "2005");
            ProcessUdpData();
        }
        private Socket SetupUdpSocketObject(string localIp, string localPort, string remoteIp, string remotePort)
        {
            //得到本机IP，设置TCP端口号         
            var ip = new IPEndPoint(IPAddress.Parse(localIp), int.Parse(localPort));
            var ret = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //绑定网络地址
            ret.Bind(ip);

            var sender = new IPEndPoint(IPAddress.Any, 0);
            _remote = sender;

            EndPoint point = new IPEndPoint(IPAddress.Parse(remoteIp), int.Parse(remotePort));
            ret.SendTo(new byte[] { 1, 2, 3 }, point);      //发送一帧数据才能收到数据

            return ret;
        }

        private void ProcessUdpData()
        {
            while (true)
            {
                var data = new byte[DataMaximumLength];
                //发送接受信息
                var recv = _udpSocket.ReceiveFrom(data, ref _remote);
                _recorder.RecordBytes(data, 0, recv);        //记录数据
                var cell = new AzimuthCell(data);
                NotifyAllObservers(cell);   //发送通知
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            _udpSocket.Dispose();
        }
    }
}
