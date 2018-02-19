using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public class DataSourceController
    {
        AntennaManager antennaManager;
        TargetManager targetManager;
        ICycleDataSubject cycleDataSubject;

        public DataSourceController()
        {
            antennaManager = (AntennaManager)TargetManagerFactory.CreateAntennaContoller();
            targetManager = (TargetManager)TargetManagerFactory.CreateTargetDataProvider();
        }

        public void ConnectDataSource(string type, string source)
        {
            cycleDataSubject?.UnregisterObserver(targetManager.Matrix);  //注销观察者
            cycleDataSubject?.UnregisterObserver(antennaManager);        //注销观察者
            cycleDataSubject?.Dispose();    //销毁对象

            switch (type)
            {
                case "BIN":
                    cycleDataSubject = CycleDataDriveFactory.CreateCycleDataSubject(ReaderType.BIN);
                    break;
                case "UDP":
                    cycleDataSubject = CycleDataDriveFactory.CreateCycleDataSubject(ReaderType.UDP);
                    break;
                default:
                    cycleDataSubject = CycleDataDriveFactory.CreateCycleDataSubject(ReaderType.UDP);    //，默认返回UDP
                    break;
            }

            targetManager.ConnectDataSource(cycleDataSubject);      //注册观察者
            antennaManager.ConnectDataSource(cycleDataSubject);     //注册观察者
            cycleDataSubject.RebindSource(source);                  //绑定数据源
            cycleDataSubject.StartReading();                        //开始读取数据
        }
    }
}
