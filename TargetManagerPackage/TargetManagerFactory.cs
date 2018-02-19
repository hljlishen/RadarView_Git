using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public class TargetManagerFactory
    {
        private static WaveGateManager_Test waveGateManager = null;
        private static TargetManager targetManager = null;
        private static AntennaManager antennaManager = null;
        private static DataSourceController dataSourcController = null;
        private static AngleAreaSurveillance angleAreaSurveillance = null;

        public static DataSourceController CreateDataSourceController()
        {
            if (dataSourcController == null)
                dataSourcController = new DataSourceController();
            return dataSourcController;
        }

        internal static TargetManager CreateTrackManager()
        {
            if (targetManager == null)
                targetManager = new TargetManager();

            return targetManager;
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

        private static WaveGateManager_Test CreateWaveGateManager()
        {
            if(waveGateManager == null)
                waveGateManager = new WaveGateManager_Test();

            return waveGateManager;
        }   //返回WaveGateManager_Test类

        public static IWaveGateController CreateWaveGateController()
        {
            return CreateWaveGateManager();
        }   //波门操作借口，添加、删除

        private static AntennaManager CreateAntennaManager()
        {
            if (antennaManager == null)
                antennaManager = new AntennaManager();

            return antennaManager;
        }

        public static IAntennaController CreateAntennaContoller()
        {
            return CreateAntennaManager();
        }

        public static IAntennaDataProvider CreateAntennaDataProvider()
        {
            return CreateAntennaManager();
        }

        public static AngleAreaSurveillance CreateAngleAreaSurveillance()
        {
            //return CreateAntennaManager();

            if (angleAreaSurveillance == null)
                angleAreaSurveillance = new AngleAreaSurveillance();
            return angleAreaSurveillance;
        }

        public static AntennaDirection GetAntennaDirection()
        {
            if (antennaManager == null)
                antennaManager = new AntennaManager();
            return antennaManager.GetAntennaDirection();
        }

        public static ISweepModeSubject CreateSweepModeSubject()
        {
            return CreateAntennaManager();
        }
    }
}
