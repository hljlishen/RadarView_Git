using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace CycleDataDrivePackage
{
    public class UdpCycleDataReader : CycleDataReader
    {
        private readonly DataRecorder _recorder;

        public UdpCycleDataReader()
        {
            _recorder = new DataRecorder();
        }

        protected override void ReadData()
        {
            try
            {
                ProcessUdpData();
            }
            catch
            {
                MessageBox.Show("无网络连接");
            }
        }

        public static (Socket, EndPoint) GetUdpConnectionObjects(string localIp, string localPort, string remoteIp,
            string remotePort)
        {
            //得到本机IP，设置TCP端口号         
            var ip = new IPEndPoint(IPAddress.Parse(localIp), int.Parse(localPort));
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //绑定网络地址

            try
            {
                socket.Bind(ip);
            }
            catch 
            {
                socket = null;
            }

            EndPoint point = new IPEndPoint(IPAddress.Parse(remoteIp), int.Parse(remotePort));

            return (socket, point);
        }

        private void ProcessUdpData()
        {
            UdpEthernetCenter.BeginSendData(new byte[] { 1, 2, 3 }, "192.168.1.13:2013", Source);
            UdpEthernetCenter.BeginRecvData("192.168.1.13:2013", Source, NotifyAllObservers);
        }

        protected override void NotifyAllObservers(byte[] rawData)
        {
            base.NotifyAllObservers(rawData);
            _recorder.RecordBytes(rawData, 0, rawData.Length);
        }
    }
}
