﻿using System;

namespace CycleDataDrivePackage
{
    public class UdpCycleDataReader : CycleDataReader
    {
        private readonly DataRecorder _recorder;
        private const string UdpCycleDataReadIpAndPortString = "192.168.10.99:2013";

        public UdpCycleDataReader()
        {
            _recorder = new DataRecorder();
        }

        protected override void ReadData() => UdpEthernetCenter.RecvData(UdpCycleDataReadIpAndPortString, Source, NotifyAllObservers);

        protected override void NotifyAllObservers(byte[] rawData)
        {
            _recorder.RecordBytes(rawData, 0, rawData.Length);
            base.NotifyAllObservers(rawData);
        }

        public override void Dispose()
        {
            base.Dispose();
            _recorder.Dispose();
        }
    }
}
