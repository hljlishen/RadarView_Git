﻿using CycleDataDrivePackage;
using System.Collections.Generic;

namespace TargetManagerPackage
{
    internal class RemoteTargetProcessorcommunicator : ICycleDataObserver
    {
        //private const string LocalIpAndPort = "192.168.1.14:10001";
        private const string LocalIpAndPort = "127.0.0.1:10001";
        private const string RemoteTargetProcessorIpAndPortString = "192.168.1.23:20010";
        private const byte SendSectorDotTargetsHead = 0xA4;
        private static RemoteTargetProcessorcommunicator _communicator;

        private RemoteTargetProcessorcommunicator()
        {
        }

        public static RemoteTargetProcessorcommunicator CreateCommunicator() =>
            _communicator ?? (_communicator = new RemoteTargetProcessorcommunicator());

        public static void SendRawData(byte[] rawData)
        {
            if (!IsSectionSweeping()) return;               //费扇扫状态不发送数据，否则对方应用会报错
            List<byte> cmdData = GenerateCommandHead();     //命令头
            cmdData = AddAntennaSectionSweepData(cmdData);  //添加扇扫信息
            cmdData.AddRange(rawData);                      //添加扇区编号

            //UdpEthernetCenter.SendData(cmdData.ToArray(), LocalIpAndPort, RemoteTargetProcessorIpAndPortString);
        }

        private static bool IsSectionSweeping() => TargetManagerFactory.CreateAntennaDataProvider().IsSectionSweeping();

        private static List<byte> GenerateCommandHead() => new List<byte>(new byte[] { 0x00, SendSectorDotTargetsHead });

        private static List<byte> AddAntennaSectionSweepData(List<byte> cmdData)
        {
            IAntennaDataProvider antenna = TargetManagerFactory.CreateAntennaDataProvider();
            byte isSectionSweep = (byte)(antenna.IsSectionSweeping() ? 1 : 0);
            cmdData.Add(isSectionSweep);
            if (isSectionSweep == 1)
            {
                cmdData.AddRange(AngleToBytes(antenna.GetSweepBeginAngle()));
                cmdData.AddRange(AngleToBytes(antenna.GetSweepEndAngle()));
            }
            else
            {
                cmdData.AddRange(new byte[] { 0, 0, 0, 0 });
            }

            return cmdData;
        }

        private static byte[] AngleToBytes(float angle)  //要求长度为2字节，产生的数字只有长度为1，则前面部0
        {
            byte[] d = PolarCoordinate.FloatToBytes(angle, 1);
            if (d.Length == 1)  //长度为1时
            {
                d = new byte[] { 0x00, d[0] };
            }

            return d;
        }

        public static void StartReceiveData() => UdpEthernetCenter.BeginRecvData(
            LocalIpAndPort,
            RemoteTargetProcessorIpAndPortString,
            ProcessRemoteTargetManagerUdpData);

        private static void ProcessRemoteTargetManagerUdpData(byte[] data)
        {
            byte head = data[1];

            int sectorNum = data[3];

            int targetCount = DistanceCell.MakeInt(data, 4, 2);

            if (head == 0xb1)
            {
                List<TargetDot> ls = GetTargetDotsFromSerialData(data, targetCount, sectorNum);
                TargetManagerFactory.CreateTrackManager().TargetDotDataReceivedFromRemoteTargetManager(sectorNum, ls);
            }

            if (head == 0xb2)
            {
                List<TargetTrack> ls = GetTargetTracksFromSerialData(data, targetCount, sectorNum);
                TargetManagerFactory.CreateTrackManager().TargetTrackDataReceivedFromRemoteTargetManager(sectorNum, ls);
            }
        }

        private static List<TargetDot> GetTargetDotsFromSerialData(byte[] data, int targetCount, int sectorIndex)
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

        private static List<TargetTrack> GetTargetTracksFromSerialData(byte[] data, int targetCount, int sectorIndex)
        {
            List<TargetTrack> ls = new List<TargetTrack>();

            int pos = 8;
            if (targetCount == 0)
                return ls;

            for (int i = 0; i < targetCount; i++)
            {
                ls.Add(new TargetTrack(data, pos, sectorIndex));
                pos += TargetTrack.TotalBytes;
            }

            return ls;
        }

        public void NotifyNewCycleData(byte[] rawData)
        {
            SendRawData(rawData);
        }
    }
}
