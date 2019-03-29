using TargetManagerPackage.目标类;
namespace TargetManagerPackage
{
    public class TargetManagerFactory
    {
        private static WaveGateManager _waveGateManager;
        private static TargetManager _targetManager;
        private static AntennaSectionSweepController _sectionSweepController;
        private static DataSourceController _dataSourcController;
        private static AntennaLeaveAngleAreaSubject _antennaLeaveAngleAreaSubject;
        private static TrackChangeSectorObserver _trackObserver;


        public static void Initialize()
        {
            //初始化目标航迹类
            //ITrackSerializer serializer = new QhOpticalDeviceSerializer();//清华大学光电设备协议
            //IPort port = new UdpPort("10.14.16.88", 5600, 6000);    //清华大学光电设备Udp接口
            ITrackSerializer serializer = new Casc12thSerializer();     //12院协议
            IPort port = new UdpPort("192.168.10.33", 10011, 10012);    //12院Udp接口
            port.Open();
            TrackSender sender = new TrackSender(port, serializer);
            TargetTrack.Sender = sender;

            MouseTargetTracker.sender = sender;
            //TargetTrack.FindIdStrategy = new FromBeginningStrategy();
            TargetTrack.FindIdStrategy = new CirculateStrategy();


            //初始化RemoteController
            Casa12thSectionSweepCmdProcessor p = new Casa12thSectionSweepCmdProcessor(CreateAntennaContoller());
            IPort udp = new UdpPort("192.168.1.45", 8080, 1002);
            RemoteController remoteController = new RemoteController(udp);
            remoteController.AddProcessor(p);
        }
        public static DataSourceController CreateDataSourceController() => _dataSourcController ?? (_dataSourcController = new DataSourceController());

        public static TargetManager CreateTrackManager() => _targetManager ?? (_targetManager = new TargetManager());

        public static ITargetDataProvider CreateTargetDataProvider() => CreateTrackManager();

        public static ITargetManagerController CreateTargetManagerController()  //目标管理器控制器
            => CreateTrackManager();

        public  static IWaveGateDataProvider CreateWaveGateDataProvider()       //目标管理器数据源
            => CreateWaveGateManager();

        private static WaveGateManager CreateWaveGateManager() => _waveGateManager ?? (_waveGateManager = new WaveGateManager());

        public static IWaveGateController CreateWaveGateController() => CreateWaveGateManager();

        internal static AntennaSectionSweepController CreateAntennaManager() => _sectionSweepController ?? (_sectionSweepController = new AntennaSectionSweepController());

        public static IAntennaController CreateAntennaContoller() => CreateAntennaManager();

        public static IAntennaDataProvider CreateAntennaDataProvider() => CreateAntennaManager();

        public static AntennaLeaveAngleAreaSubject CreateAntennaLeaveAngleAreaSubject() => _antennaLeaveAngleAreaSubject ?? (_antennaLeaveAngleAreaSubject = new AntennaLeaveAngleAreaSubject());

        public static RotateDirection GetAntennaDirection() => (CreateAntennaManager() as IAntennaDataProvider).GetAntennaDirection();

        public static ISweepModeSubject CreateSweepModeSubject() => CreateAntennaManager();

        private static TrackChangeSectorObserver CreateTrackObserver() => _trackObserver ?? (_trackObserver = new TrackChangeSectorObserver());

        internal static void RegisterTrackObserver(TargetTrack track) => CreateTrackObserver().RegisterTrack(track);

        internal static void UnregisterTrackObserver(TargetTrack track) => CreateTrackObserver().UnregisterTrack(track);
    }
}
