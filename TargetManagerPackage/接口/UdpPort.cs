using System;
using System.Net;
using System.Net.Sockets;
using Utilities;
using System.Threading;

namespace TargetManagerPackage
{
    public class UdpPort : IPort
    {
        public event DataReceivedHandler DataReceived;
        public Socket udpRecvSocket { get; private set; }
        public Socket udpSendSocket { get; private set; }
        private EndPoint remoteEndPoint;
        //private const int buffLength = 102400;
        //private byte[] buff;

        public UdpPort(string endPointIp, int endPointPort, int localPort)
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(endPointIp), endPointPort);

            udpRecvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //IPEndPoint localIp = new IPEndPoint(IPAddress.Parse("192.168.10.99"), localPort);
            IPEndPoint localIp = new IPEndPoint(IPAddress.Any, localPort);
            udpRecvSocket.Bind(localIp);

            udpSendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //buff = new byte[buffLength];
        }
        public void Close() => udpRecvSocket?.Close();

        public bool IsOpen()
        {
            if (udpRecvSocket == null)
                return false;
            return udpRecvSocket.Connected;
        }

        public void Open()
        {
            BeginReceive();
        }

        private void BeginReceive()
        {
            //buff = new byte[buffLength];
            Thread t = new Thread(recvThread);
            t.Start();
            //udpSocket.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(recvHandler), udpSocket);
        }

        private void recvThread()
        {
            while(true)
            {
                byte[] buff = new byte[1024];
                int num = udpRecvSocket.ReceiveFrom(buff, ref remoteEndPoint);
                byte[] recvData = Tools.GetArrayRange(buff, 0, num);
                DataReceived?.Invoke(recvData);
            }
        }

        //private void recvHandler(IAsyncResult iar)
        //{
        //    //byte[] CurrentBytes = (byte[])iar.AsyncState;
        //    //结束了本次数据接收,num为本次读取的长度，如果发送长度超过接收数组的长度，则会多次触发读取回调函数
        //    int num = udpRecvSocket.EndReceive(iar);

        //    IPAddress addr = IPAddress.Parse("192.168.10.33");
        //    EndPoint end = new IPEndPoint(addr, 10011);
        //    num = udpRecvSocket.ReceiveFrom(buff, ref end);

        //    //触发收到数据事件
        //    if (num > 0)
        //    {
        //        byte[] recvData = Tools.GetArrayRange(buff, 0, num);
        //        DataReceived?.Invoke(recvData);
        //    }

        //    //处理结果后马上启动数据异步读取【目前我每条接收的字节数据长度不会超过1024】
        //    BeginReceive();
        //}

        public void Send(byte[] sendData)
        {
            udpSendSocket.SendTo(sendData, remoteEndPoint);
        }
    }
}
