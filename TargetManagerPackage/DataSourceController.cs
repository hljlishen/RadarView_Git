using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public class DataSourceController
    {
        private readonly AntennaSectionSweepController _antennaManager;
        private readonly TargetManager _targetManager;
        private ICycleDataSubject _cycleDataSubject;

        public DataSourceController()
        {
            _antennaManager = (AntennaSectionSweepController)TargetManagerFactory.CreateAntennaContoller();
            _targetManager = (TargetManager)TargetManagerFactory.CreateTargetDataProvider();
        }

        public void ConnectDataSource(string type, string source)
        {
            DestroyOldCycleDataSubject();

            ConnectNewCycleDataSubject(type, source);

            StartCycleDataSubject();
        }

        public void SpeedUp()
        {
            _cycleDataSubject.SpeedUp();
        }

        public void SpeedDown()
        {
            _cycleDataSubject.SpeedDown();
        }

        private static ICycleDataSubject GetCycleDataSubject(string type)
        {
            switch (type)
            {
                case "BIN":
                    return CycleDataDriveFactory.CreateCycleDataSubject(ReaderType.Bin);
                case "UDP":
                    return CycleDataDriveFactory.CreateCycleDataSubject(ReaderType.Udp);
                default:
                    return CycleDataDriveFactory.CreateCycleDataSubject(ReaderType.Udp);    //，默认返回UDP
            }
        }

        private void DestroyOldCycleDataSubject()   //废除之前的数据源对象
        {
            _cycleDataSubject?.UnregisterObserver(_targetManager);          //注销观察者
            _cycleDataSubject?.UnregisterObserver(_antennaManager);        //注销观察者
            _cycleDataSubject?.Dispose();    //销毁对象
        }

        private void ConnectNewCycleDataSubject(string type, string source)     //链接新数据源
        {
            _cycleDataSubject = GetCycleDataSubject(type);       //获取新的周期数据对象

            _targetManager.ConnectDataSource(_cycleDataSubject);      //注册观察者
            _antennaManager.ConnectDataSource(_cycleDataSubject);     //注册观察者
            _cycleDataSubject.RebindSource(source);                  //绑定数据源
        }

        private void StartCycleDataSubject()    //数据源开始读取数据
        {
            _cycleDataSubject.StartReading();
        }
    }
}
