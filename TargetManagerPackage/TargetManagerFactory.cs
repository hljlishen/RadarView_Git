namespace TargetManagerPackage
{
    public class TargetManagerFactory
    {
        private static WaveGateManager _waveGateManager;
        private static TargetManager _targetManager;
        private static AntennaSectionSweepController _sectionSweepController;
        private static DataSourceController _dataSourcController;
        private static AntennaLeaveAngleAreaSubject _antennaLeaveAngleAreaSubject;

        public static DataSourceController CreateDataSourceController() => _dataSourcController ?? (_dataSourcController = new DataSourceController());

        public static TargetManager CreateTrackManager() => _targetManager ?? (_targetManager = new TargetManager());

        public static ITargetDataProvider CreateTargetDataProvider() => CreateTrackManager();

        public static ITargetManagerController CreateTargetManagerController()  //目标管理器控制器
            => CreateTrackManager();

        public  static IWaveGateDataProvider CreateWaveGateDataProvider()       //目标管理器数据源
            => CreateWaveGateManager();

        private static WaveGateManager CreateWaveGateManager() => _waveGateManager ?? (_waveGateManager = new WaveGateManager());

        public static IWaveGateController CreateWaveGateController() => CreateWaveGateManager();

        private static AntennaSectionSweepController CreateAntennaManager() => _sectionSweepController ?? (_sectionSweepController = new AntennaSectionSweepController());

        public static IAntennaController CreateAntennaContoller() => CreateAntennaManager();

        public static IAntennaDataProvider CreateAntennaDataProvider() => CreateAntennaManager();

        public static AntennaLeaveAngleAreaSubject CreateAntennaLeaveAngleAreaSubject() => _antennaLeaveAngleAreaSubject ?? (_antennaLeaveAngleAreaSubject = new AntennaLeaveAngleAreaSubject());

        public static RotateDirection GetAntennaDirection() => (CreateAntennaManager() as IAntennaDataProvider).GetAntennaDirection();

        public static ISweepModeSubject CreateSweepModeSubject() => CreateAntennaManager();
    }
}
