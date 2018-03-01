using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

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
            Source = "192.168.1.5:2005";
        }

        protected override void ReadData()
        {
            (string ip, string port) = TryParseIpAddressAndPort(Source);
            //_udpSocket = SetupUdpSocketObject("192.168.1.13", "2013", "192.168.1.5", "2005");
            _udpSocket = SetupUdpSocketObject("192.168.1.13", "2013", ip, port);
            ProcessUdpData();
        }

        private (string, string) TryParseIpAddressAndPort(string data)
        {
            string ip;
            string port;
            int index;
            try
            {
                index = data.IndexOf(":", StringComparison.Ordinal);
                ip = data.Substring(0, index);
                port = data.Substring(index + 1);
                return (ip, port);
            }
            catch (Exception e)
            {
                MessageBox.Show("输入的Ip或端口不正确");
                throw;
            }
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
