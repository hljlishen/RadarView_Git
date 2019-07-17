using System;
using System.Collections.Generic;

namespace TargetManagerPackage
{
    public class AntennaLeaveAngleAreaSubject : ILeaveAngleAreaSubject, IAntennaObserver,IDisposable
    {
        protected List<KeyValuePair<ILeaveAngleAreaObserver, AngleArea>> Areas;     //problem2
        private float _preciousAntennaAngle;
        private float _currentAntennaAngle;
        private readonly IAntennaDataProvider _antenna;
        private object _locker = new object();

        public AntennaLeaveAngleAreaSubject()
        {
            Areas = new List<KeyValuePair<ILeaveAngleAreaObserver, AngleArea>>();
            _antenna = TargetManagerFactory.CreateAntennaDataProvider();
            _antenna.RegisterObserver(this);
        }

        public void RegisterAngleArea(ILeaveAngleAreaObserver ob, AngleArea area)
        {
            lock (_locker)
            {
                foreach (var p in Areas)
                {
                    if (p.Key == ob && p.Value == area)  //已经注册过
                        return;
                }

                var pair = new KeyValuePair<ILeaveAngleAreaObserver, AngleArea>(ob, area);
                Areas.Add(pair);
            }
        }

        public void UnregisterAngleArea(ILeaveAngleAreaObserver ob, AngleArea area)
        {
            lock (_locker)
            {
                foreach (var p in Areas)    //problem4
                {
                    if (p.Key != ob || p.Value != area) continue;
                    Areas.Remove(p);
                    break;
                }
            }
        }

        public void AntennaNotifyChange()
        {
            lock (_locker)
            {
                _preciousAntennaAngle = _currentAntennaAngle;
                _currentAntennaAngle = _antenna.GetCurrentAntennaAngle();

                for (int i = Areas.Count - 1; i >= 0; i--)   //遍历每个角度区域，判断天线是否刚刚扫过该区域，如果是则通知观察者
                {
                    if (LeavingAngleArea(Areas[i].Value))
                    {
                        Areas[i].Key.NotifyLeaveAngleArea(Areas[i].Value);  //problem3
                    }
                }
            }
        }

        public bool LeavingAngleArea(AngleArea area)
        {
            var isPreAngleInArea = area.IsAngleInArea(_preciousAntennaAngle);
            var isCurAngleInArea = area.IsAngleInArea(_currentAntennaAngle);

            return isPreAngleInArea && !isCurAngleInArea;
        }

        public void Dispose()
        {
            _antenna.UnregisterObserver(this);
            Areas = null;
        }
    }
}
