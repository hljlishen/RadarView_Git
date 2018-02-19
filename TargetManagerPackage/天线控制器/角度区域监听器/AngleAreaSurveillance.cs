using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class AngleAreaSurveillance : ILeaveAngleAreaSubject, IAntennaObserver,IDisposable
    {
        protected  List<KeyValuePair<ILeaveAngleAreaObserver, AngleArea>> areas;
        float PreciousAntennaAngle;
        float CurrentAntennaAngle;
        private readonly IAntennaDataProvider antenna;

        public AngleAreaSurveillance()
        {
            areas = new List<KeyValuePair<ILeaveAngleAreaObserver, AngleArea>>();
            antenna = TargetManagerFactory.CreateAntennaDataProvider();
            antenna.RegisterObserver(this);
        }

        public void RegisterAngleArea(ILeaveAngleAreaObserver ob, AngleArea area)
        {
            foreach(var p in areas)
            {
                if (p.Key == ob && p.Value == area)  //已经注册过
                    return;
            }

            var pair = new KeyValuePair<ILeaveAngleAreaObserver, AngleArea>(ob, area);
            areas.Add(pair);
        }

        public void UnregisterAngleArea(ILeaveAngleAreaObserver ob, AngleArea area)
        {
            foreach (var p in areas)
            {
                if (p.Key == ob && p.Value == area) //查找匹配对象
                {
                    areas.Remove(p);
                    break;
                }
            }
        }

        public void AntennaNotifyChange()
        {
            PreciousAntennaAngle = CurrentAntennaAngle;
            CurrentAntennaAngle = antenna.GetCurrentAntennaAngle();

            for (int i = areas.Count - 1; i >= 0; i--)   //遍历每个角度区域，判断天线是否刚刚扫过该区域，如果是则通知观察者
            {
                if (LeavingAngleArea(areas[i].Value))
                {
                    areas[i].Key.NotifyLeaveAngleArea(areas[i].Value);
                }
            }
        }

        public bool LeavingAngleArea(AngleArea area)
        {
            var isPreAngleInArea = area.IsAngleInArea(PreciousAntennaAngle);
            var isCurAngleInArea = area.IsAngleInArea(CurrentAntennaAngle);

            if (isPreAngleInArea && !isCurAngleInArea) //前一个角度在area里，当前角度在area外
            {
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            antenna.UnregisterObserver(this);
            areas = null;
        }
    }
}
