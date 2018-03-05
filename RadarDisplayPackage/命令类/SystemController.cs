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
        public SystemController( OverViewDisplayer ovd )
        {
            //目标显示器命令初始化
            antennaControlStateCmd = new OverViewDisplayerAntennaControlCommand(ovd);
            autoWaveGateStateCmd = new OverViewDisplayerAutoWaveGateCommand(ovd);
            semiAutoWaveGateCmd = new OverViewDisplayerSemiAutoWaveGateCommand(ovd);
            zoomStateCmd = new OverViewDisplayerZoomStateCommand(ovd);
            ResetDisplayerCmd = new OverViewDisplayerResetCommand(ovd);

            delAvtiveTarget = new TargetManagerDeleteActiveTargetCommand();

            dataSourceController = TargetManagerFactory.CreateDataSourceController();

            deleteActiveWaveGatesCmd = new WaveGateDeleteActiveCommand();
        }


        //overviewdisplayer显示器的控制模式切换到放缩模式
        public void SwitchToZoomState()
        {
            zoomStateCmd.Execute();
        }


        //overviewdisplayer显示器的控制模式切换到天线扇扫控制
        public void SwitchToAntennaControlState()
        {
            antennaControlStateCmd.Execute();
        }


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
        protected void SwitchToAutoWaveGateState()
        {
            autoWaveGateStateCmd.Execute();
        }


        //overviewdisplayer显示器的控制模式切换到半自动波门
        protected void SwitchToSemiAutoWaveGateSate()
        {
            semiAutoWaveGateCmd.Execute();
        }


        //复位overviewdisplayer显示器
        public void ResetDisplayer()
        {
            ResetDisplayerCmd.Execute();
        }


        //设置天线扇扫模式
        public void AntennaSetSectionSweepMode(float begin, float end)
        {
            AntennaSetSectionSweepModeCommand cmd = new AntennaSetSectionSweepModeCommand(begin, end);
            cmd.Execute();
        }


        //设置天线转速
        public void AntennaSetRotationRate(RotateRate rate)
        {
            AntennaSetRotationRateCommand rotationRateCmd = new AntennaSetRotationRateCommand(rate);
            rotationRateCmd.Execute();
        }


        //设置天线为周扫模式
        public void AntennaSetRotateDirection(RotateDirection direct)
        {
            AntennaSetNormalSweepModeCommand cmd = new AntennaSetNormalSweepModeCommand(direct);
            cmd.Execute();
        }


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

        public void TargetManagerDeleteActiveTarget()
        {
            delAvtiveTarget.Execute();
        }

        public void ConnectDataSource(string readerType, string scr)
        {
            dataSourceController.ConnectDataSource(readerType, scr);
        }

        public void SetCycleDataFilterAmThreshold(int am)
        {
            if(am >= 0)
                CycleDataFilter.AMThreshold = am;
        }

        public void SetCycleDataFilterSpeedMinimum(int speed)
        {
            CycleDataFilter.SpeedMinimum = speed;
        }

        public void SetCycleDataFilterSpeedMaximum(int speed)
        {
            CycleDataFilter.SpeedMaximum = speed;
        }

        public int GetCycleDataFilterAmThreshold()
        {
            return CycleDataFilter.AMThreshold;
        }

        public int GetCycleDataFilterSpeedMinimum()
        {
            return CycleDataFilter.SpeedMinimum;
        }

        public int GetCycleDataFilterSpeedMaximum()
        {
            return CycleDataFilter.SpeedMaximum;
        }

        public void DeleteActiveWaveGates()
        {
            deleteActiveWaveGatesCmd.Execute();
        }
    }
}
