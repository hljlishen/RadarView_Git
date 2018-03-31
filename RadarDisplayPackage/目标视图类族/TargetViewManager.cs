using System;
using System.Collections.Generic;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    abstract class TargetViewManager : ITargetObserver, IDisposable
    {
        protected ITargetDataProvider targetProvider;
        protected TrackDisplayer displayer;
        protected List<TargetView>[] dots;
        protected List<TargetView>[] tracks;

        protected TargetViewManager(TrackDisplayer displayer)
        {
            this.displayer = displayer;

            //获取航迹管理器
            targetProvider = TargetManagerFactory.CreateTargetDataProvider();
            targetProvider.RegisterObserver(this);

            InitializeTargetArray();    //初始化dots和tracks
        }

        private void InitializeTargetArray()
        {
            int count = targetProvider.GetSectorCount();    //扇区计数

            dots = new List<TargetView>[count];
            tracks = new List<TargetView>[count];

            for( int i = 0; i < count; i++)     //初始化链表
            {
                dots[i] = new List<TargetView>();
                tracks[i] = new List<TargetView>();
            }
        }

        public virtual void DisplayTargetViews()
        {
            //lock (views)
            //{
            //    foreach (GraphicTargetView view in views)
            //        view?.DisplayTarget();
            //}
        }

        public virtual void NotifyChange(Target t, NotifyType type)
        {
            switch (type)
            {
                case NotifyType.Delete:
                    RemoveTarget(t);
                    break;
                case NotifyType.Add:
                    AddTarget(t);
                    break;
                case NotifyType.Update:
                    UpDateTarget(t);
                    break;
            }
        }

        protected virtual void RemoveTarget(Target t)
        {
            if (t == null)
                return;

            if (t is TargetTrack)
            {
                foreach (TargetView view in tracks[t.sectorIndex])
                {
                    if (t == view.Target)
                    {
                        lock (tracks)
                        {
                            tracks[t.sectorIndex].Remove(view);
                            view.Dispose();
                        }
                        break;      //跳出循环，否则会导致遍历失败
                    }
                }
            }
            else
            {
                foreach (TargetView view in dots[t.sectorIndex])
                {
                    if (t == view.Target)
                    {
                        lock (dots)
                        {
                            dots[t.sectorIndex].Remove(view);
                            view.Dispose();
                        }
                        break;      //跳出循环，否则会导致遍历失败
                    }
                }
            }
        }

        protected virtual void AddTarget(Target t)
        {
            if (t == null)
                return;

            TargetView view = CreateTargetView(t);

            if(t is TargetTrack)
                tracks[t.sectorIndex].Add(view);
            else
                dots[t.sectorIndex].Add(view);
        }

        protected virtual void UpDateTarget(Target t)
        {
            //先删除后添加
            RemoveTarget(t);
            AddTarget(t);
        }

        public abstract TargetView CreateTargetView(Target taget);

        protected abstract void LoadTargetViews(List<Target> ls);

        public virtual void NotifyUpdateSectorDot(List<TargetDot> dots, int sectorIndex)
        {
            this.dots[sectorIndex].Clear();

            if (dots == null)   //空对象表示删除该区域所有点
                return;

            //添加目标视图
            foreach(TargetDot dot in dots)
            {
                var view = CreateTargetView(dot);
                this.dots[sectorIndex].Add(view);
            }
        }

        public virtual void NotifyUpdateSectorTrack(List<TargetTrack> trackList, int sectorIndex)
        {
            tracks[sectorIndex].Clear();

            if (trackList == null)   //空对象表示删除该区域所有点
                return;

            //添加目标视图
            foreach (TargetTrack track in trackList)
            {
                TargetView view = CreateTargetView(track);
                tracks[sectorIndex].Add(view);
            }
        }

        public virtual void Dispose()
        {
            targetProvider.UnregisterObserver(this);
        }
    }
}
