using System;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public class DataSourceController
    {
        public const string DefaultRadarIpAndPort = "192.168.10.5:2005";
        private readonly AntennaSectionSweepController _antennaManager;
        private readonly CycleDataMatrix _cycleDataMatrix;
        private readonly RemoteTargetProcessorcommunicator _communicator;
        private ICycleDataSubject _cycleDataSubject;

        public DataSourceController()
        {
            _antennaManager = (AntennaSectionSweepController)TargetManagerFactory.CreateAntennaContoller();
            _cycleDataMatrix = CycleDataMatrix.CreaCycleDataMatrix();
            _communicator = RemoteTargetProcessorcommunicator.CreateCommunicator();
        }

        public void ConnectDataSource(string type, string source)
        {
            DestroyOldCycleDataSubject();

            ConnectNewCycleDataSubject(type, source);

            StartCycleDataSubject();
        }

        public void SpeedUp() => _cycleDataSubject.SpeedUp();

        public void SpeedDown() => _cycleDataSubject.SpeedDown();

        public void Pause() => _cycleDataSubject.Pause();

        public void Resume() => _cycleDataSubject.Resume();

        private static ICycleDataSubject GetCycleDataSubject(string type)
        {
            switch (type)
            {
                case "BIN":
                    return CycleDataDriveFactory.CreateCycleDataSubject(ReaderType.Bin);
                case "UDP":
                    return CycleDataDriveFactory.CreateCycleDataSubject(ReaderType.Udp);
                default:
                    throw new Exception("请求的数据源类型错误");    //错误
            }
        }

        private void DestroyOldCycleDataSubject()   //废除之前的数据源对象
        {
            _cycleDataSubject?.UnregisterObserver(_cycleDataMatrix);          //注销观察者
            _cycleDataSubject?.UnregisterObserver(_antennaManager);        //注销观察者
            _cycleDataSubject?.UnregisterObserver(_communicator);
            _cycleDataSubject?.Dispose();    //销毁对象
        }

        private void ConnectNewCycleDataSubject(string type, string source)     //链接新数据源
        {
            _cycleDataSubject = GetCycleDataSubject(type);       //获取新的周期数据对象

            _cycleDataMatrix.Clear();
            _cycleDataSubject.RegisterObserver(_cycleDataMatrix);
            TargetManagerFactory.CreateTrackManager().InitializeSectors();  //删除所有目标
            _cycleDataSubject.RegisterObserver(_antennaManager);
            _cycleDataSubject.RebindSource(source);                  //绑定数据源
            _cycleDataSubject.RegisterObserver(RemoteTargetProcessorcommunicator.CreateCommunicator());
            RemoteTargetProcessorcommunicator.StartReceiveData();
        }

        private void StartCycleDataSubject()    //数据源开始读取数据
        {
            _cycleDataSubject.StartReading();
        }
    }
}
