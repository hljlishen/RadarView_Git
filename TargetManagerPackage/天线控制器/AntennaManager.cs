using AntennaControlPackage;
using CycleDataDrivePackage;
using System;
using System.Collections.Generic;
using System.Threading;

namespace TargetManagerPackage
{
    public class AntennaManager : AntennaDataManager, IAntennaController,  ILeaveAngleAreaObserver, ISweepModeSubject
    {
        private readonly List<ISweepModeObserver> _sweepModeObs;    //天线扫描状态观察者
        private AngleArea _modifiedSection;                         //去除惯性范围的区域
        private AngleArea _sweepSection;                            //用户设置的扇扫区域
        private uint _rotationRate = 5;                             //初始状态5转每分钟
        private readonly IServoController _servoController;
        private AntennaDirection _intentionalDirection = AntennaDirection.ClockWise;     //期望的天线扫描方向，由于惯性的存在，可能与真实方向不一致
        private AntennaDirection _preAntennaDirection = AntennaDirection.ClockWise;     //前一个角度计算出的天线方向

        public AntennaManager()
        {
            _sweepModeObs = new List<ISweepModeObserver>();
            _sweepSection = null;
            _servoController = ServoControllerFactory.CreateServoController();
        }

        public void SetSectionSweepMode(AngleArea area) //扇扫模式
        {
            _sweepSection = area;
            _isSectionSweeping = true;
            var angleAreaSurveillance = TargetManagerFactory.CreateAngleAreaSurveillance();

            if (_isSectionSweeping)
            {
                angleAreaSurveillance.UnregisterAngleArea(this, _modifiedSection);
            }
            _modifiedSection = CalAntiInertiaSection(_sweepSection);
            angleAreaSurveillance.RegisterAngleArea(this, _modifiedSection);
            NotifySweepModeChange();
        }

        private AngleArea CalAntiInertiaSection(AngleArea area)
        {
            if (_rotationRate == 2)
                return new AngleArea(area.BeginAngle - 20 , area.EndAngle + 20);
            if(_rotationRate == 5)
                return new AngleArea(area.BeginAngle - 10, area.EndAngle + 10);
            if (_rotationRate == 10)
                return new AngleArea(area.BeginAngle + 14, area.EndAngle - 14);
            return new AngleArea(area.BeginAngle, area.EndAngle);
        }

        public void SetNormalSweepMode(AntennaDirection direction)       //切换成正常扫描模式
        {
            StopSectionSweep();
            StartSwitchToDirection(direction);
            NotifySweepModeChange();                            //通知观察者扫描状态改变
        }

        private void StartSwitchToDirection(AntennaDirection direction)
        {
            var t = new Thread(SwitchToDirection);
            t.Start(direction);
        }

        private void SwitchToDirection(object o)
        {
            lock (this)
            {
                var direction = (AntennaDirection)o;
                _intentionalDirection = direction;       
                SetpAntennaSweepState(AntennaDirection.Stopped, _rotationRate);     //先让天线停止
                Thread.Sleep(200);                                                  //等待
                SetpAntennaSweepState(direction, _rotationRate);                    //改变天线方向，不改变当前转速
            }
        }

        public void SetRotationRate(uint countPerMinute)    //不改变方向，只改变转速，界面
        {
            SetpAntennaSweepState(_intentionalDirection, countPerMinute);
            if (!_isSectionSweeping) return;        //不是扇扫状态
            var angleAreaSurveillance = TargetManagerFactory.CreateAngleAreaSurveillance();
            angleAreaSurveillance.UnregisterAngleArea(this, _modifiedSection);
            _modifiedSection = CalAntiInertiaSection(_sweepSection);
            angleAreaSurveillance.RegisterAngleArea(this, _modifiedSection);
        }

        private void SetpAntennaSweepState(AntennaDirection direction, uint countPerMinute)
        {
            lock (this)
            {
                _rotationRate = countPerMinute;  //保存当前设置的转速
                var rate = GetRotationRate(direction, countPerMinute);
                _servoController.SetRotationRate(rate);
            }
        }

        public static RotationRate GetRotationRate(AntennaDirection direction, uint countPerMinute)
        {
            var sign = 0;

            switch (direction)
            {
                case AntennaDirection.ClockWise:
                    sign = 1;
                    break;
                case AntennaDirection.CounterClockWise:
                    sign = -1;
                    break;
                case AntennaDirection.Stopped:
                    sign = 0;
                    break;
            }

            return  (RotationRate)(countPerMinute * sign);
        }

        public override float GetSweepBeginAngle() => _sweepSection.BeginAngle;

        public override float GetSweepEndAngle() => _sweepSection.EndAngle;

        protected void StopSectionSweep()
        {
            _sweepSection = null;
            _isSectionSweeping = false;
            TargetManagerFactory.CreateAngleAreaSurveillance().UnregisterAngleArea(this, _modifiedSection);
        }

        public void NotifyLeaveAngleArea(AngleArea area) => AntennaLeaveSectionSweepAngleArea();//天线扫过了扇扫区域，需要翻转天线

        private void AntennaLeaveSectionSweepAngleArea()
        {
            StartSwitchToDirection(ReversedDirection(GetAntennaDirection()));
            _targetManagerController.DeleteOutRangedTargets(_sweepSection);
        }

        public void RegisterSweepModeObserver(ISweepModeObserver ob)
        {
            if (ob != null && !_sweepModeObs.Contains(ob))
                _sweepModeObs.Add(ob);
        }

        public void UnregisterSweepModeObserver(ISweepModeObserver ob)
        {
            if (ob != null && _sweepModeObs.Contains(ob))
                _sweepModeObs.Remove(ob);
        }

        protected void NotifySweepModeChange()
        {
            foreach (var ob in _sweepModeObs)
            {
                ob.NotifySweepModeChange(_isSectionSweeping ? SweepMode.Section : SweepMode.Normal);
            }
        }

        public AngleArea GetSweepSection() => _sweepSection;
    }
}
