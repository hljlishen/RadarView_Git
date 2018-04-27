using System;
using System.Collections.Generic;
using System.Windows.Forms;
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
        protected readonly List<IAntennaObserver> AntennaObservers;
        public float AntennaCurrentAngle;      //天线当前角度
        public float AntennaPreviousAngle;     //天线的前一个角度
        protected const float FloatValueEqualMinmumInterval = 0.001f;    //判断天线停止的最小角度
        protected ITargetManagerController TargetManagerController;
        protected bool _isSectionSweeping;
        private RotateDirection _preRotateDirection = RotateDirection.ClockWise;     //前一个角度计算出的天线方向

        protected AntennaDataManager() => AntennaObservers = new List<IAntennaObserver>();

        public void RegisterObserver(IAntennaObserver ob)
        {
            if (ob != null && !AntennaObservers.Contains(ob))
                AntennaObservers.Add(ob);
        }

        public void UnregisterObserver(IAntennaObserver ob)
        {
            if (ob != null && AntennaObservers.Contains(ob))
                AntennaObservers.Remove(ob);
        }

        public void NotifyAntennaDataChange()
        {
            foreach (IAntennaObserver ob in AntennaObservers)
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
                return AntennaPreviousAngle < 1 && AntennaCurrentAngle > 350
                    ? RotateDirection.CounterClockWise
                    : RotateDirection.ClockWise;
            }

            return AntennaPreviousAngle > 350 && AntennaCurrentAngle < 1
                ? RotateDirection.ClockWise
                : RotateDirection.CounterClockWise;
        }

        public void ConnectDataSource(ICycleDataSubject subject)
        {
            subject.RegisterObserver(this);
            TargetManagerController = TargetManagerFactory.CreateTargetManagerController();
        }

        public void NotifyNewCycleData(byte[] rawData)
        {
                UpdateAntennaAngle(rawData);

                var newDirection = GetAntennaDirection();

                if (newDirection != _preRotateDirection)
                {
                    _preRotateDirection = newDirection;
                    TargetManagerController.ClearRawData();
                }

                NotifyAntennaDataChange();     //通知观察者，天线角度已改变
        }

        private void UpdateAntennaAngle(byte[] rawData)
        {
            var azCell = new AzimuthCell(rawData);
            azCell.Angle = ReverAngleDirection(azCell.Angle);
            AntennaPreviousAngle = AntennaCurrentAngle;

            AntennaCurrentAngle = azCell.GetAngle();  //更新天线角度
        }

        public static float ReverAngleDirection(float angle)
        {
            float rAngle = 360f - angle;

            //rAngle -= 252f;     //矫正0方位

            //if (rAngle < 0)
            //    rAngle += 360;
            //rAngle %= 360;
            rAngle = PolarCoordinate.StandardAngle(rAngle);

            return rAngle;
        }
    }
}
