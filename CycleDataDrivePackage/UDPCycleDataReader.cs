namespace CycleDataDrivePackage
{
    public class UdpCycleDataReader : CycleDataReader
    {
        private readonly DataRecorder _recorder;

        public UdpCycleDataReader()
        {
            _recorder = new DataRecorder();
            UdpEthernetCenter.RegisterIpAndPort("192.168.1.13:2013");
        }

        protected override void ReadData()
        {
            UdpEthernetCenter.RecvData("192.168.1.13:2013", Source, NotifyAllObservers);
        }

        protected override void NotifyAllObservers(byte[] rawData)
        {
            base.NotifyAllObservers(rawData);
            _recorder.RecordBytes(rawData, 0, rawData.Length);
        }
    }
}
