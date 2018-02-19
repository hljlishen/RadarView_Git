using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    public class WaveGateManager_Test : IWaveGateDataProvider, IWaveGateController, ILeaveAngleAreaObserver
    {
        protected List<WaveGate> waveGates;
        protected List<IWaveGateObserver> obs;
        protected AngleAreaSurveillance surveillance;
        protected const int SemiAutoWaveGateLife = 3;   //半自动波门的生命长度（天线扫出该波门的次数）

        public WaveGateManager_Test()
        {
            waveGates = new List<WaveGate>();

            obs = new List<IWaveGateObserver>();

            surveillance = TargetManagerFactory.CreateAngleAreaSurveillance();
        }
        public void AddWaveGate(WaveGate gate)
        {
            if (gate == null)
                return;

            waveGates.Add(gate);

            if(gate.IsSemiAuto)     //半自动波门注册，监视角度区域，天线扫出该波门获得通知
            {
                surveillance.RegisterAngleArea(this, gate);
            }

            NotifyAllObservers(gate, WaveGateSubjectNotifyState.Add);
        }
            
        public void DelWaveGate(WaveGate gate)
        {
            if (gate == null)
                return;

            if (waveGates.Contains(gate))
                waveGates.Remove(gate);

            if(gate.IsSemiAuto) //手动删除的是半自动波门，反注册监听区域，天线扫出该区域不在获得通知
            {
                surveillance.UnregisterAngleArea(this, gate);
            }

            NotifyAllObservers(gate, WaveGateSubjectNotifyState.Delete);
        }

        public void RegisterObserver(IWaveGateObserver ob)        //注册观察者
        {
            if (!obs.Contains(ob))
                obs.Add(ob);
        }

        public void UnregisterObserver(IWaveGateObserver ob)      //注销观察者
        {
            if (obs.Contains(ob))
                obs.Remove(ob);
        }

        public  List<WaveGate> GetWaveGates()
        {
            return waveGates;
        }

        public WaveGate IsTargetInWaveGate(Target t)    //判断一个点是否在某一波门内
        {
            foreach(WaveGate gate in waveGates)
            {
                if(gate.IsCoordinateInWaveGate(t.CurrentCoordinate))
                {
                    return gate;
                }
            }
            return null;
        }

        public void NotifyLeaveAngleArea(AngleArea area)
        {
            WaveGate gate = (WaveGate)area;
            gate.SweepCount = gate.SweepCount + 1;  //该波门扫过计数加一

            if(gate.SweepCount >= SemiAutoWaveGateLife)     //删除半自动波门
            {
                surveillance.UnregisterAngleArea(this, gate);
                NotifyAllObservers(gate, WaveGateSubjectNotifyState.Delete);
                waveGates.Remove(gate);
            }
        }   //获得天线离开波门通知

        private void NotifyAllObservers(WaveGate gate, WaveGateSubjectNotifyState type)
        {
            foreach (IWaveGateObserver ob in obs)
            {
                ob.NotifyChange(gate, type);
            }
        }   //主动通知所有观察者，有波门发生变化

        public void DeleteActiveWaveGate()
        {
            for(int i = waveGates.Count - 1; i >= 0; i--)
            {
                if (waveGates[i].Active)
                    DelWaveGate(waveGates[i]);
            }
        }
    }
}
