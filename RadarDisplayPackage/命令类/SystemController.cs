using System;
using TargetManagerPackage;
using CycleDataDrivePackage;

namespace RadarDisplayPackage
{
    public class SystemController       //命令类族的门面类
    {
        //目标显示器命令
        private readonly OverViewDisplayerAntennaControlCommand antennaControlStateCmd;
        private readonly OverViewDisplayerAutoWaveGateCommand autoWaveGateStateCmd;
        private readonly OverViewDisplayerSemiAutoWaveGateCommand semiAutoWaveGateCmd;
        private readonly OverViewDisplayerZoomStateCommand zoomStateCmd;
        private readonly OverViewDisplayerResetCommand ResetDisplayerCmd;

        //目标管理器命令
        private readonly TargetManagerDeleteActiveTargetCommand delAvtiveTarget;

        //数据源控制器
        private readonly DataSourceController dataSourceController;

        //波门命令
        private readonly WaveGateDeleteActiveCommand deleteActiveWaveGatesCmd;

        //配置文件读写器
        Config config;
        public SystemController( OverViewDisplayer ovd )
        {
            TargetManagerFactory.Initialize();
            config = new Config();
            config.SetElAdjustment(2.2f);
            LoadConfig();

            //目标显示器命令初始化
            antennaControlStateCmd = new OverViewDisplayerAntennaControlCommand(ovd);
            autoWaveGateStateCmd = new OverViewDisplayerAutoWaveGateCommand(ovd);
            semiAutoWaveGateCmd = new OverViewDisplayerSemiAutoWaveGateCommand(ovd);
            zoomStateCmd = new OverViewDisplayerZoomStateCommand(ovd);
            ResetDisplayerCmd = new OverViewDisplayerResetCommand(ovd);

            delAvtiveTarget = new TargetManagerDeleteActiveTargetCommand();

            dataSourceController = TargetManagerFactory.CreateDataSourceController();

            deleteActiveWaveGatesCmd = new WaveGateDeleteActiveCommand();
            ovd.NewSweepSection += OnNewSweepSection;
            ovd.NewWaveGate += Ovd_NewWaveGate;
        }

        private void LoadConfig()
        {
            SetCycleDataFilterAmThreshold(GetCycleDataFilterAmThreshold());
            SetCycleDataFilterHeightThreshold(GetCycleDataFilterHeightThreshold());
            SetCycleDataFilterSpeedMaximum(GetCycleDataFilterSpeedMaximum());
            SetCycleDataFilterSpeedMinimum(GetCycleDataFilterSpeedMinimum());
            SetAzAdjustment(GetAzAdjustment());
            SetElAdjustment(GetElAdjustment());
        }

        private void Ovd_NewWaveGate(WaveGate waveGate) => new WaveGateAddCommand(waveGate).Execute();

        private void OnNewSweepSection(AngleArea area) => new AntennaSetSectionSweepModeCommand(area).Execute();


        //overviewdisplayer显示器的控制模式切换到放缩模式
        public void SwitchToZoomState() => zoomStateCmd.Execute();


        //overviewdisplayer显示器的控制模式切换到天线扇扫控制
        public void SwitchToAntennaControlState() => antennaControlStateCmd.Execute();

        //overviewdisplayer显示器的控制模式切换到波门模式，需要读取targetmannager当前的模式决定切换到自动波门还是半自动波门
        public void SwitchToWaveGateState()
        {
            ITargetDataProvider provider = TargetManagerFactory.CreateTargetDataProvider();
            TargetManagerMode mode = provider.GetMode();

            switch(mode)
            {
                case TargetManagerMode.Auto:
                    SwitchToAutoWaveGateState();
                    break;
                case TargetManagerMode.SemiAuto:
                    SwitchToSemiAutoWaveGateSate();
                    break;
                case TargetManagerMode.Manual:
                    break;
                case TargetManagerMode.Intelligent:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        //overviewdisplayer显示器的控制模式切换到自动波门
        protected void SwitchToAutoWaveGateState() => autoWaveGateStateCmd.Execute();

        //overviewdisplayer显示器的控制模式切换到半自动波门
        protected void SwitchToSemiAutoWaveGateSate() => semiAutoWaveGateCmd.Execute();

        //复位overviewdisplayer显示器
        public void ResetDisplayer() => ResetDisplayerCmd.Execute();

        //设置天线扇扫模式
        public void AntennaSetSectionSweepMode(float begin, float end) => new AntennaSetSectionSweepModeCommand(begin, end).Execute();

        //设置天线转速
        public void AntennaSetRotationRate(RotateRate rate) => new AntennaSetRotationRateCommand(rate).Execute();

        //设置天线为周扫模式
        public void AntennaSetRotateDirection(RotateDirection direct) => new AntennaSetNormalSweepModeCommand(direct).Execute();


        public void AntennaSetZeroDegree() => new SetAntennaToZeroDegreeCommand().Execute();

        //切换起批模式
        public void TargetManagerSwitchMode(string  modeName)
        {
            TargetManagerMode mode;

            switch (modeName)
            {
                case "自动":
                    mode = TargetManagerMode.Auto;
                    break;
                case "半自动":
                    mode = TargetManagerMode.SemiAuto;
                    break;
                case "手动":
                    mode = TargetManagerMode.Manual;
                    break;
                case "智能":
                    mode = TargetManagerMode.Intelligent;
                    break;
                default:
                    mode = TargetManagerMode.Auto;
                    break;
            }
            TargetManagerSwitchModeCommand cmd = new TargetManagerSwitchModeCommand(mode);
            cmd.Execute();
        }

        public void TargetManagerDeleteActiveTarget() => delAvtiveTarget.Execute();

        public void ConnectDataSource(string readerType, string scr) => dataSourceController.ConnectDataSource(readerType, scr);

        public void SetCycleDataFilterAmThreshold(int am)
        {
            DistanceCellFilter.AmThreshold = am >= 0 ? am : DistanceCellFilter.AmThreshold;
            config.SetAmMin(DistanceCellFilter.AmThreshold);
        }

        public void SetCycleDataFilterSpeedMinimum(int speed)
        {
            DistanceCellFilter.SpeedMinimum = speed;
            config.SetSpeedMin(speed);
        }

        public void SetCycleDataFilterSpeedMaximum(int speed)
        {
            DistanceCellFilter.SpeedMaximum = speed;
            config.SetSpeedMax(speed);
        }

        public void SetCycleDataFilterHeightThreshold(int height)
        {
            DistanceCellFilter.HeightMinimum = height;
            config.SetHeightMin(height);
        }

        public int GetCycleDataFilterAmThreshold() => config.GetAmMin();

        public int GetCycleDataFilterSpeedMinimum()
        {
            //return DistanceCellFilter.SpeedMinimum;
            return config.GetSpeedMin();
        }

        public int GetCycleDataFilterSpeedMaximum()
        {
            //return DistanceCellFilter.SpeedMaximum;
            return config.GetSpeedMax();
        }

        public int GetCycleDataFilterHeightThreshold()
        {
            //return DistanceCellFilter.HeightMinimum;
            return config.GetHeightMin();
        }

        public float GetAzAdjustment() => config.GetAzAdjustment();

        public void SetAzAdjustment(float adjustment)
        {
            CycleDataReader.AzAdjustment = adjustment;
            config.SetAzAdjustment(adjustment);
        }

        public float GetElAdjustment() => config.GetElAdjustment();

        public void SetElAdjustment(float adjustment)
        {
            DistanceCell.ElAdjustment = adjustment;
            config.SetElAdjustment(adjustment);
        }

        public void DeleteActiveWaveGates() => deleteActiveWaveGatesCmd.Execute();

        public void DataSourceSpeedUp() => dataSourceController.SpeedUp();

        public void DataSourceSpeedDown() => dataSourceController.SpeedDown();

        public void DataSourcePause() => dataSourceController.Pause();

        public void DataSourceResume() => dataSourceController.Resume();

        public void SetDisplayTrackCourseStatus(bool state) => GraphicTargetTrackViewDrawer.ShouldDrawCourse = state;

        public float GetAngleAdjustment() => CycleDataReader.AzAdjustment;

        public float SetAngleAdjustment(float angleAdjust) => CycleDataReader.AzAdjustment = angleAdjust;

        public static void SetTrackHeight(float height)
        {
            MouseTargetTracker.TrackHeight = height > 0 ? height : 0;
        }

        public static float GetTrackHeight()
        {
            return MouseTargetTracker.TrackHeight;
        }

        //public void SetAzAdjustment(float adjustment) => AzimuthCell.AzAdjustment = adjustment;
    }
}
