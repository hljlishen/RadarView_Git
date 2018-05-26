using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace TargetManagerPackage
{
    public class OpticalDeviceCommunicator
    {
        private Socket _opticalDeviceSocket;
        private const string LocalIpAddress = "192.168.10.99";
        private const string RemoteIpAndPort = "192.168.10.33:10091";
        private const string LocalPort = "10090";
        private const int Port = 60000;
        private Socket socketWatch;
        private static OpticalDeviceCommunicator _communicator;

        public static OpticalDeviceCommunicator CreateOpticalDeviceCommunicator() => _communicator ?? (_communicator = new OpticalDeviceCommunicator());

        private OpticalDeviceCommunicator()
        {
            socketWatch = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //服务端发送信息 需要1个IP地址和端口号
            IPAddress ipaddress = IPAddress.Parse(LocalIpAddress);
            IPEndPoint endpoint = new IPEndPoint(ipaddress, int.Parse(LocalPort));
            try
            {
                socketWatch.Bind(endpoint);
                socketWatch.Listen(1);
                Thread threadWatch = new Thread(WatchConnecting) {IsBackground = true};
                threadWatch.Start();
            }
            catch
            {
                // ignored
            }
        }

        public void SendData(byte[] data) => _opticalDeviceSocket?.Send(data);

        public void SendTrack(TargetTrack t)
        {
            //List<byte> cmdBytes = new List<byte> {0x68, 0x2, 0x1, 0x1, (byte) t.trackID};
            //cmdBytes.AddRange(SystemCommunicator.IntToByteLsb((int)t.Dis, 4));
            //cmdBytes.AddRange(SystemCommunicator.IntToByteLsb((int)(t.AZ * 100),2));
            //cmdBytes.AddRange(SystemCommunicator.IntToByteLsb((int)(t.EL * 100), 2));
            //cmdBytes.AddRange(CRC16(cmdBytes.ToArray()));

            //SendData(cmdBytes.ToArray());

            SendData(new X68Command(t).Serialize());
        }

        private void WatchConnecting()
        {
            while (true)  //持续不断监听客户端发来的请求
            {
                Socket socConnection = socketWatch.Accept();
                //txtMsg.AppendText("客户端连接成功" + "\r\n");
                //创建一个通信线程 
                socConnection.Send(new byte[] { 48, 49, 50 });
                ParameterizedThreadStart pts = ServerRecMsg;
                Thread thr = new Thread(pts) {IsBackground = true};
                //socConnections.Add(socConnection);
                _opticalDeviceSocket = socConnection;
                //启动线程
                thr.Start(socConnection);
                //dictThread.Add(thr);
            }
        }

        private void ServerRecMsg(object sokConnectionparn)
        {
            Socket sokClient = sokConnectionparn as Socket;
            byte[] arrMsgRec = new byte[1024];
            int length = sokClient.Receive(arrMsgRec);
            (int id, PolarCoordinate coordinate) = X68Command.GetSerialDataCoordinate(arrMsgRec);
        }
    }
}
