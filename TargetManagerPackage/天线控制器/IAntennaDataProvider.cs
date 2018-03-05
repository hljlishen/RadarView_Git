using System;
using System.Collections.Generic;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public interface IAntennaDataProvider : IAntennaSubject
    {
        float GetSweepBeginAngle();

        float GetSweepEndAngle();

        float GetCurrentAntennaAngle(); //获取天线当前位置，0-360度

        bool IsSectionSweeping();            //正常扫描还是扇扫，返回0为正常，1为扇扫

        RotateDirection GetAntennaDirection(); //回去当前天线的扫描方向
    }

    public abstract class AntennaDataManager : IAntennaDataProvider, ICycleDataObserver
    {
        protected readonly List<IAntennaObserver> _antennaObservers;
        public float AntennaCurrentAngle;      //天线当前角度
        public float AntennaPreviousAngle;     //天线的前一个角度
        protected const float FloatValueEqualMinmumInterval = 0.001f;    //判断天线停止的最小角度
        protected ITargetManagerController TargetManagerController;
        protected bool _isSectionSweeping;
        private RotateDirection _preRotateDirection = RotateDirection.ClockWise;     //前一个角度计算出的天线方向

        protected AntennaDataManager() => _antennaObservers = new List<IAntennaObserver>();

        public void RegisterObserver(IAntennaObserver ob)
        {
            if (ob != null && !_antennaObservers.Contains(ob))
                _antennaObservers.Add(ob);
        }

        public void UnregisterObserver(IAntennaObserver ob)
        {
            if (ob != null && _antennaObservers.Contains(ob))
                _antennaObservers.Remove(ob);
        }

        public void NotifyAntennaDataChange()
        {
            foreach (IAntennaObserver ob in _antennaObservers)
                ob.AntennaNotifyChange();
        }

        public abstract float GetSweepBeginAngle();

        public abstract float GetSweepEndAngle();

        public float GetCurrentAntennaAngle() => AntennaCurrentAngle;

        public bool IsSectionSweeping() => _isSectionSweeping;

        public RotateDirection GetAntennaDirection()
        {
            if (Math.Abs(AntennaPreviousAngle - AntennaCurrentAngle) < FloatValueEqualMinmumInterval)
            {
                return _preRotateDirection;
            }

            if (AntennaPreviousAngle < AntennaCurrentAngle)
            {
                if (AntennaPreviousAngle < 1 && AntennaCurrentAngle > 350)  //逆时针跨越360度
                    return RotateDirection.CounterClockWise;
                return RotateDirection.ClockWise;
            }

            if (AntennaPreviousAngle > 350 && AntennaCurrentAngle < 1)
                return RotateDirection.ClockWise;
            return RotateDirection.CounterClockWise;
        }

        public void ConnectDataSource(ICycleDataSubject subject)
        {
            subject.RegisterObserver(this);
            TargetManagerController = TargetManagerFactory.CreateTargetManagerController();
        }

        public void NotifyNewCycleData(AzimuthCell data)
        {
            try
            {
                AntennaPreviousAngle = AntennaCurrentAngle;

                AntennaCurrentAngle = data.GetAngle();  //更新天线角度

                var newDirection = GetAntennaDirection();

                if (newDirection != _preRotateDirection)
                {
                    _preRotateDirection = newDirection;
                    TargetManagerController.ClearRawData();
                }

                NotifyAntennaDataChange();     //通知观察者，天线角度已改变
            }
            catch
            {
                // ignored
            }
        }
    }
}
