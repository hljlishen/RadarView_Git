using System;
using System.Collections.Generic;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    abstract class TargetViewManager : ITargetObserver, IDisposable
    {
        protected ITargetDataProvider targetProvider;
        protected TrackDisplayer displayer;
        protected List<TargetView>[] _DotViews;
        protected List<TargetView>[] _TrackViews;
        protected static object _locker =  new object();
        protected const int ShowTrackScoreThreshold = 3;

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

            lock (_locker)
            {
                _DotViews = new List<TargetView>[count];
                _TrackViews = new List<TargetView>[count];

                for (int i = 0; i < count; i++)     //初始化链表
                {
                    _DotViews[i] = new List<TargetView>();
                    _TrackViews[i] = new List<TargetView>();
                }
            }
        }

        public abstract void DisplayTargetViews();

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

            lock (_locker)
            {
                if (t is TargetTrack)
                {
                    foreach (TargetView view in _TrackViews[t.SectorIndex])
                    {
                        if (t != view.Target) continue;
                        _TrackViews[t.SectorIndex].Remove(view);
                        view.Dispose();
                        break;      //跳出循环，否则会导致遍历失败
                    }
                }
                else
                {
                    foreach (TargetView view in _DotViews[t.SectorIndex])
                    {
                        if (t != view.Target) continue;
                        _DotViews[t.SectorIndex].Remove(view);
                        view.Dispose();
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

            lock (_locker)
            {
                if (t is TargetTrack)
                    _TrackViews[t.SectorIndex].Add(view);
                else
                    _DotViews[t.SectorIndex].Add(view);
            }
        }

        protected virtual void UpDateTarget(Target t)
        {
            //先删除后添加
            RemoveTarget(t);
            AddTarget(t);
        }

        public abstract TargetView CreateTargetView(Target taget);

        protected abstract void LoadTargetViews(List<Target> ls);

        public virtual void NotifyUpdateSectorDot(List<TargetDot> newDots, int sectorIndex)
        {
            lock (_locker)
            {
                _DotViews[sectorIndex].Clear();

                if (newDots == null) //空对象表示删除该区域所有点
                    return;

                //添加目标视图
                foreach (TargetDot dot in newDots)
                {
                    var view = CreateTargetView(dot);
                    _DotViews[sectorIndex].Add(view);
                }
            }
        }

        public virtual void NotifyUpdateSectorTrack(List<TargetTrack> trackList, int sectorIndex)
        {
            _TrackViews[sectorIndex].Clear();

            if (trackList == null)   //空对象表示删除该区域所有点
                return;

            //添加目标视图
            foreach (TargetTrack track in trackList)
            {
                //if (track.Score <= ShowTrackScoreThreshold) continue;
                TargetView view = CreateTargetView(track);
                if(view != null)
                    _TrackViews[sectorIndex].Add(view);
            }
        }

        public virtual void Dispose()
        {
            targetProvider.UnregisterObserver(this);
        }
    }
}
