using System;
using System.Net;
using System.Net.Sockets;

namespace TargetManagerPackage
{
    public class UdpPort : IPort
    {
        public event DataReceivedHandler DataReceived;
        public Socket udpSocket { get; private set; }
        private IPEndPoint remoteEndPoint;
        private const int buffLength = 102400;
        private byte[] buff;

        public UdpPort(string endPointIp, int endPointPort, int localPort)
        {
            remoteEndPoint = new IPEndPoint(IPAddress.Parse(endPointIp), endPointPort);
            //remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, 15000);

            udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //udpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
            IPEndPoint localIp = new IPEndPoint(IPAddress.Any, localPort);
            udpSocket.Bind(localIp);
        }
        public void Close() => udpSocket?.Close();

        public bool IsOpen()
        {
            if (udpSocket == null)
                return false;
            return udpSocket.Connected;
        }

        public void Open()
        {
            BeginReceive();
        }

        private void BeginReceive()
        {
            buff = new byte[buffLength];
            udpSocket.BeginReceive(buff, 0, buff.Length, SocketFlags.None, new AsyncCallback(recvHandler), udpSocket);
        }

        private void recvHandler(IAsyncResult iar)
        {
            //byte[] CurrentBytes = (byte[])iar.AsyncState;
            //结束了本次数据接收,num为本次读取的长度，如果发送长度超过接收数组的长度，则会多次触发读取回调函数
            int num = udpSocket.EndReceive(iar);

            IPAddress addr = IPAddress.Parse("192.168.10.33");
            EndPoint end = new IPEndPoint(addr, 10011);
            udpSocket.ReceiveFrom(buff, ref end);

            //触发收到数据事件
            // byte[] recvData = Tools.GetArrayRange(buff, 0, num);
            byte[] recvData = buff;
            DataReceived?.Invoke(recvData);

            //处理结果后马上启动数据异步读取【目前我每条接收的字节数据长度不会超过1024】
            BeginReceive();
        }

        public void Send(byte[] sendData)
        {
            udpSocket.SendTo(sendData, remoteEndPoint);
        }
    }
}
