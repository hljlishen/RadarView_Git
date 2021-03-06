﻿using System;
using System.Windows.Forms;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public abstract class TrackDisplayer
    {
        protected Timer timer;                              //定时器，用于刷新显示数据
        protected float distance;                           //距离量程,或者高度
        protected ITargetDataProvider dataProvider;         //目标航迹和目标点迹数据的提供者
        internal TargetViewManager targetsManager;          //目标管理器
        protected object _locker = new object();


        protected TrackDisplayer(Control holder)
        {
            holder.Controls.Clear();
            DisplayControl = holder;

            timer = new Timer {Interval = 30};
            timer.Tick += Timer_Tick;
            timer.Start();

            //TrackManager_TestCase_Random该类用于测试绘图程序
            dataProvider = TargetManagerFactory.CreateTargetDataProvider(); 
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            lock (_locker)
            {
                UpdateInfomation();
            }
        }

        public int UpdateInterval
        {
            get => timer.Interval;
            set => timer.Interval = value;
        }

        public virtual float Distance
        {
            get => distance;

            set => distance = value;
        }

        public Control DisplayControl { get; set; }

        public virtual void UpdateInfomation() { }
    }
}