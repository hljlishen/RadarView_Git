using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class GraphicWaveGateViewManager : IDisposable,IWaveGateObserver
    {
        OverViewDisplayer dispalyer;
        List<GraphicWaveGateView> views;

        IWaveGateDataProvider gateManager;   //波门管理对象

        public GraphicWaveGateViewManager(OverViewDisplayer dispalyer)
        {
            this.dispalyer = dispalyer;

            views = new List<GraphicWaveGateView>();

            //相应说表点击事件，该事件处理波门的选择
            dispalyer.DisplayControl.MouseClick += DisplayControl_MouseClick;

            //获取波门管理对象
            gateManager = TargetManagerFactory.CreateWaveGateDataProvider();
            gateManager.RegisterObserver(this); //注册监听

            //获取所有波门数据
            ReloadWaveGates();
        }

        private void DisplayControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            foreach (GraphicWaveGateView view in views)
            {
                view.HandleMouseClick(e.Location);
            }
        }

        public void Draw()
        {
            foreach (GraphicWaveGateView view in views)
            {
                view.Draw();
            }
        }

        public void ReloadWaveGates()       //重新读取所有波门数据，当显示视图放大，复位，偏心时需重新计算波门位置时，需调用此函数
        {
            foreach (GraphicWaveGateView view in views)
            {
                view?.Dispose();
            }
            views?.Clear();

            List<WaveGate> ls = gateManager.GetWaveGates();

            foreach (WaveGate gate in ls)
            {
                views.Add(new GraphicWaveGateView(gate, dispalyer));
            }
        }

        public void DeleteSelectedWaveGate()
        {
            List<GraphicWaveGateView> ls = new List<GraphicWaveGateView>();

            for (int i = views.Count - 1; i >= 0; i--)  //倒序便利list，可以实现批量删除
            {
                if (views[i].Selected)
                {
                    Command cmd = new WaveGateDeleteCommand(views[i].WaveGate);
                    cmd.Execute();
                }
            }
        }

        public void NotifyChange(WaveGate wg, WaveGateSubjectNotifyState state)
        {
            if (wg == null)
                return;

            if(state == WaveGateSubjectNotifyState.Add)
            {
                views.Add(new GraphicWaveGateView(wg, dispalyer));
            }
            else
            {
                for(int i = views.Count -1; i >=0; i--)
                {
                    if (views[i].WaveGate == wg)
                    {
                        views[i].Dispose();
                        views.Remove(views[i]);
                        
                        break;
                    }
                }
            }
        }

        public void Dispose()
        {
            dispalyer.DisplayControl.MouseClick -= DisplayControl_MouseClick;

            foreach(GraphicWaveGateView view in views)
            {
                view.Dispose();
            }
            gateManager.UnregisterObserver(this);
            views.Clear();
        }
    }
}
