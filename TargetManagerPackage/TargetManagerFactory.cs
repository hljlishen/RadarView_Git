namespace TargetManagerPackage
{
    public class TargetManagerFactory
    {
        private static WaveGateManager _waveGateManager;
        private static TargetManager _targetManager;
        private static AntennaSectionSweepController _sectionSweepController;
        private static DataSourceController _dataSourcController;
        private static AntennaLeaveAngleAreaSubject _antennaLeaveAngleAreaSubject;

        public static DataSourceController CreateDataSourceController()
        {
            return _dataSourcController ?? (_dataSourcController = new DataSourceController());
        }

        internal static TargetManager CreateTrackManager()
        {
            return _targetManager ?? (_targetManager = new TargetManager());
        }

        public static ITargetDataProvider CreateTargetDataProvider()
        {
            return CreateTrackManager();
        }

        public static ITargetManagerController CreateTargetManagerController()  //目标管理器控制器
        {
            return CreateTrackManager();
        }

        public  static IWaveGateDataProvider CreateWaveGateDataProvider()       //目标管理器数据源
        {
            return CreateWaveGateManager();
        }

        private static WaveGateManager CreateWaveGateManager()
        {
            return _waveGateManager ?? (_waveGateManager = new WaveGateManager());
        }   //返回WaveGateManager_Test类

        public static IWaveGateController CreateWaveGateController()
        {
            return CreateWaveGateManager();
        }   //波门操作借口，添加、删除

        private static AntennaSectionSweepController CreateAntennaManager()
        {
            return _sectionSweepController ?? (_sectionSweepController = new AntennaSectionSweepController());
        }

        public static IAntennaController CreateAntennaContoller()
        {
            return CreateAntennaManager();
        }

        public static IAntennaDataProvider CreateAntennaDataProvider()
        {
            return CreateAntennaManager();
        }

        public static AntennaLeaveAngleAreaSubject CreateAntennaLeaveAngleAreaSubject()
        {
            return _antennaLeaveAngleAreaSubject ?? (_antennaLeaveAngleAreaSubject = new AntennaLeaveAngleAreaSubject());
        }

        public static RotateDirection GetAntennaDirection()
        {
            return (CreateAntennaManager() as IAntennaDataProvider).GetAntennaDirection();
        }

        public static ISweepModeSubject CreateSweepModeSubject()
        {
            return CreateAntennaManager();
        }
    }
}
