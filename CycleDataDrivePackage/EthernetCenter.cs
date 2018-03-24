using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace CycleDataDrivePackage
{
    public class UdpEthernetCenter
    {
        public const int MaximumReadLength = 20480;
        public delegate void ReceiveDataHandler(byte[] data);
        private static readonly Dictionary<string, Socket> SocketDictionary = new Dictionary<string, Socket>();

        public static void BeginSendData(byte[] data, string localIpAndPort, string remoteIpAndPort)
        {
            Socket socket = GetSocket(localIpAndPort);
            if (socket == null)
            {
                MessageBox.Show("Socket无效,UdpEthernetCenter.BeginSendData()调用失败");
                return;
            }
            EndPoint remoteEndPoint = GetIpEndPoint(remoteIpAndPort);
            Thread t = new Thread(()=>socket.SendTo(data,remoteEndPoint));
            t.Start();
        }

        public static void BeginRecvData(string localIpAndPort, string remoteIpAndPort, ReceiveDataHandler handler)
        {
            Thread t = new Thread(()=> RecvData(localIpAndPort,remoteIpAndPort,handler));
            t.Start();
        }

        private static void RecvData(string localIpAndPort, string remoteIpAndPort, ReceiveDataHandler handler)
        {
            while (true)
            {
                byte[] data = new byte[MaximumReadLength];
                Socket socket = GetSocket(localIpAndPort);
                if (socket == null) return;
                EndPoint remotEndPoint = GetIpEndPoint(remoteIpAndPort);
                int byteCount = socket.ReceiveFrom(data, ref remotEndPoint);

                //移除多余的字节并返回
                List<byte> ls = new List<byte>(data);
                ls.RemoveRange(byteCount, MaximumReadLength - byteCount);
                handler(ls.ToArray());
            }
        }

        public static (string, string) ParseIpAddressAndPort(string ipAndPortString)   //将格式为192.168.1.1:1234的字符串解析为IP地址和端口
        {
            string trim = ipAndPortString.Trim();
            try
            {
                var index = trim.IndexOf(":", StringComparison.Ordinal);
                var ip = trim.Substring(0, index);
                var port = trim.Substring(index + 1);
                return (ip, port);
            }
            catch
            {
                MessageBox.Show(@"输入的Ip或端口不正确");
                throw;
            }
        }

        public static Socket GetSocket(string localIpAndPort)
        {
            if(SocketDictionary.Keys.Contains(localIpAndPort))
                return SocketDictionary[localIpAndPort];

            Socket socket = CreateUdpSocket(localIpAndPort);
            if (socket == null) return null;
            SocketDictionary.Add(localIpAndPort, socket);
            return socket;
        }

        public static Socket CreateUdpSocket(string ipAndPort)
        {
            //得到本机IP，设置TCP端口号         
            var ip = GetIpEndPoint(ipAndPort);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //绑定网络地址
            try
            {
                socket.Bind(ip);
            }
            catch
            {
                socket = null;  //绑定失败返回null
            }

            return socket;
        }

        public static EndPoint GetIpEndPoint(string ipAndPort)
        {
            (string ip, string port) = ParseIpAddressAndPort(ipAndPort);
            return new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
        }
    }
}
