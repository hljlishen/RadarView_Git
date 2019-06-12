using System;
using System.Net.Sockets;
using Utilities;

namespace TargetManagerPackage
{
    class TcpClientPort : IPort
    {
        public event DataReceivedHandler DataReceived;
        private byte[] recvBuff;
        private const int recvBuffLength = 102400;

        public TcpClientPort(string ipAddr, int port)
        {
            ipAddress = ipAddr;
            this.port = port;
        }

        public void Close() => tcpClient?.Close();

        public bool IsOpen() => tcpClient == null ? false : tcpClient.Connected;

        public void Open()
        {
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(ipAddress, port, new AsyncCallback(ConnectHandler), new object());
        }

        private void ConnectHandler(IAsyncResult ar)
        {
            tcpClient.EndConnect(ar);
            netStream = tcpClient.GetStream();
            recvBuff = new byte[recvBuffLength];
            netStream.BeginRead(recvBuff, 0, recvBuffLength, new AsyncCallback(AsynReceiveData), recvBuff);
        }

        public void Send(byte[] sendData) => netStream?.Write(sendData, 0, sendData.Length);

        private void AsynReceiveData(IAsyncResult iar)
        {
            //byte[] CurrentBytes = (byte[])iar.AsyncState;
            //结束了本次数据接收,num为本次读取的长度，如果发送长度超过接收数组的长度，则会多次触发读取回调函数
            int num = netStream.EndRead(iar);

            //触发收到数据事件
            byte[] recvData = Tools.GetArrayRange(recvBuff, 0, num);

            DataReceived?.Invoke(recvData);
            //处理结果后马上启动数据异步读取【目前我每条接收的字节数据长度不会超过1024】
            recvBuff = new byte[recvBuffLength];
            netStream.BeginRead(recvBuff, 0, recvBuffLength, new AsyncCallback(AsynReceiveData), recvBuff);
        }

        private string ipAddress;
        private int port;
        private TcpClient tcpClient;
        private NetworkStream netStream;
    }
}
