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
        private TargetManager _targetManager;

        public RemoteTargetProcessorcommunicator(TargetManager t)
        {
            (_remoteSocket, _remoteEndPoint) =
                UdpCycleDataReader.GetUdpConnectionObjects("192.168.1.14", "10001", "192.168.1.23", "20010");

            _targetManager = t;
        }

        public void SendRawData(byte[] rawData)
        {
            IAntennaDataProvider antenna = TargetManagerFactory.CreateAntennaDataProvider();
            List<byte> ls = new List<byte>(new byte []{0x00, SendSectorDotTargetsHead});   //命令头
            byte isSectionSweep = (byte)(antenna.IsSectionSweeping() ? 1 : 0);
            ls.Add(isSectionSweep);
            if (isSectionSweep == 1)
            {
                ls.AddRange(AngleToBytes(antenna.GetSweepBeginAngle() , 1));
                ls.AddRange(AngleToBytes(antenna.GetSweepEndAngle(), 1));
            }
            else
            {
                ls.AddRange(new byte[] { 0, 0, 0, 0 });
            }
            ls.AddRange(rawData); //添加扇区编号

            Thread t = new Thread(SendData);
            t.Start(ls.ToArray());

            //Thread t = new Thread(()=> _remoteSocket.SendTo(ls.ToArray(), _remoteEndPoint));
            //t.Start();
        }

        private byte[] AngleToBytes(float angle, int validNum)  //要求长度为2字节，产生的数字只有长度为1，则前面部0
        {
            byte[] d = PolarCoordinate.FloatToBytes(angle, 1);
            if (d.Length == 1)  //长度为1时
            {
                d = new byte[] { 0x00, d[0] };
            }

            return d;
        }

        private void SendData(object data)
        {
            byte[] d = (byte[]) data;
            _remoteSocket?.SendTo(d, _remoteEndPoint);
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
                _remoteSocket?.ReceiveFrom(data, ref _remoteEndPoint);
                //_remoteSocket.Receive(data);
                byte head = data[1];

                int sectorNum = data[3];

                int targetCount = DistanceCell.MakeInt(data, 4, 2);

                if (head == 0xb1)
                {
                    List<TargetDot> ls = GetTargetDotsFromSerialData(data, targetCount, sectorNum);
                    _targetManager.TargetDotDataReceived(sectorNum, ls);
                }

                if (head == 0xb2)
                {
                    List<TargetTrack> ls = GetTargetTracksFromSerialData(data, targetCount, sectorNum);
                    _targetManager.TargetTrackDataReceived(sectorNum, ls);
                }
            }
        }

        private List<TargetDot> GetTargetDotsFromSerialData(byte[] data, int targetCount, int sectorIndex)
        {
            List<TargetDot> ls = new List<TargetDot>();

            int pos = 6;
            if (targetCount == 0)
                return ls;

            for(int i = 0; i < targetCount; i++)
            {
                ls.Add(new TargetDot(data,pos,sectorIndex));
                pos += TargetDot.TotalBytes;
            }

            return ls;
        }

        private List<TargetTrack> GetTargetTracksFromSerialData(byte[] data, int targetCount, int sectorIndex)
        {
            List<TargetTrack> ls = new List<TargetTrack>();

            int pos = 8;
            if (targetCount == 0)
                return null;

            for (int i = 0; i < targetCount; i++)
            {
                ls.Add(new TargetTrack(data, pos, sectorIndex));
                pos += TargetTrack.TotalBytes;
            }

            return ls;
        }

        public void SendSectionSweepState(bool isSectionSweeping, AngleArea area)
        {
            List<byte> ls = new List<byte>(new byte[] { 0x00, 0xA3 });
            byte state;
            if (isSectionSweeping)
            {
                state = 1;
                ls.Add(state);
                ls.AddRange(PolarCoordinate.FloatToBytes(area.BeginAngle, 1));
                ls.AddRange(PolarCoordinate.FloatToBytes(area.EndAngle, 1));
            }
            else
            {
                state = 0;
                ls.Add(state);
                ls.AddRange(new byte[] { 0, 0, 0, 0 });
            }

            Thread t = new Thread(() => SendData(ls.ToArray()));
            t.Start();
        }
    }
}
