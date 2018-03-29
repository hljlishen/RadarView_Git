namespace CycleDataDrivePackage
{
    public class UdpCycleDataReader : CycleDataReader
    {
        private readonly DataRecorder _recorder;
        private const string UdpCycleDataReadIpAndPortString = "192.168.2.33:2013";

        public UdpCycleDataReader()
        {
            _recorder = new DataRecorder();
            UdpEthernetCenter.RegisterIpAndPort(UdpCycleDataReadIpAndPortString);
        }

        protected override void ReadData()
        {
            UdpEthernetCenter.RecvData(UdpCycleDataReadIpAndPortString, Source, NotifyAllObservers);
        }

        protected override void NotifyAllObservers(byte[] rawData)
        {
            base.NotifyAllObservers(rawData);
            _recorder.RecordBytes(rawData, 0, rawData.Length);
        }
    }
}
