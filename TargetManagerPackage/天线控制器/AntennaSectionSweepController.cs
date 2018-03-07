using System;
using System.Collections.Generic;
using System.Threading;
using AntennaControlPackage;

namespace TargetManagerPackage
{
    internal class AntennaSectionSweepController : AntennaDataManager, IAntennaController, ILeaveAngleAreaObserver, ISweepModeSubject
    {
        private readonly List<ISweepModeObserver> _sweepModeObs;    //天线扫描状态观察者
        private AngleArea _modifiedSection;                         //去除惯性范围的区域
        private AngleArea _sweepSection;                            //用户设置的扇扫区域
        private readonly AntennaRotateController _rotateController;     //

        public AntennaSectionSweepController()
        {
            _sweepModeObs = new List<ISweepModeObserver>();
            _sweepSection = null;
            _rotateController = new AntennaRotateController();
        }

        public void SetSectionSweepMode(AngleArea area) //扇扫模式
        {
            if (_isSectionSweeping)
            {
                StopSectionSweep();     //先停止扇扫
            }
            StartSectionSweep(area);
            NotifySweepModeChange();
        }

        public void SetRotateDirection(RotateDirection direction)       //切换成正常扫描模式
        {
            StopSectionSweep();
            _rotateController.SetRotateDirection(direction);
            NotifySweepModeChange();                            //通知观察者扫描状态改变
        }

        public void SetRotateRate(RotateRate rate)    //不改变方向，只改变转速，界面
        {
            _rotateController.SetRotateRate(rate);
            if (_isSectionSweeping)
                StartSectionSweep(_sweepSection); //扇扫状态,重新计算惯性区域
        }

        private void StartSectionSweep(AngleArea area)
        {
            _isSectionSweeping = true;
            _sweepSection = area;
            UnregisterAngleAreaSurveillance();
            RegisterAngleAreaSurveillance(area);
        }

        private void UnregisterAngleAreaSurveillance()
        {
            var angleAreaSurveillance = TargetManagerFactory.CreateAngleAreaSurveillance();
            angleAreaSurveillance.UnregisterAngleArea(this, _modifiedSection);
        }

        private void RegisterAngleAreaSurveillance(AngleArea area)
        {
            var angleAreaSurveillance = TargetManagerFactory.CreateAngleAreaSurveillance();
            _modifiedSection = _rotateController.CalAntiInertiaSection(area);
            angleAreaSurveillance.RegisterAngleArea(this, _modifiedSection);
        }

        public override float GetSweepBeginAngle() => _sweepSection.BeginAngle;

        public override float GetSweepEndAngle() => _sweepSection.EndAngle;

        protected void StopSectionSweep()
        {
            _sweepSection = null;
            _isSectionSweeping = false;
            UnregisterAngleAreaSurveillance();
        }

        public void NotifyLeaveAngleArea(AngleArea area) => AntennaLeaveSectionSweepAngleArea();//天线扫过了扇扫区域，需要翻转天线

        private void AntennaLeaveSectionSweepAngleArea()
        {
            _rotateController.ReverseSweepDirection();
            TargetManagerController.DeleteOutRangedTargets(_sweepSection);
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
