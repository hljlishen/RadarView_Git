using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public abstract class TrackDisplayer
    {
        protected System.Windows.Forms.Timer timer;                              //定时器，用于刷新显示数据
        protected float distance;                           //距离量程,或者高度
        protected Control displayControl;                   //用于显示信息的控件
        protected ITargetDataProvider dataProvider;    //目标航迹和目标点迹数据的提供者
        internal TargetViewManager targetsManager;       //目标管理器
        //protected Thread t;
        //protected object threadLock = new object();

        public TrackDisplayer(Control Holder)
        {
            Holder.Controls.Clear();
            DisplayControl = Holder;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 30;
            timer.Tick += Timer_Tick;
            timer.Start();

            //TrackManager_TestCase_Random该类用于测试绘图程序
            dataProvider = TargetManagerFactory.CreateTargetDataProvider(); 
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateInfomation();
        }

        public int UpdateInterval
        {
            get { return timer.Interval; }
            set { timer.Interval = value; }
        }

        public virtual float Distance
        {
            get
            {
                return distance;
            }

            set
            {
                distance = value;
            }
        }

        public Control DisplayControl
        {
            get
            {
                return displayControl;
            }

            set
            {
                displayControl = value;
            }
        }

        public virtual void UpdateInfomation() { }
    }
}