using CycleDataDrivePackage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TargetManagerPackage
{
    public class TargetManager :ITargetDataProvider, ITargetManagerController, ILeaveAngleAreaObserver //目标管理器
    {
        public Sector[] Sectors;                  //扇区数组
        private readonly DotViewDeleter _viewDeleter;     //目标删除器
        private readonly Clotter _clotter;                //凝聚器
        private readonly Clotter _clotter47;
        private readonly TrackCorelator _trackCorelator;  //航迹相关器
        private DotCorelator _dotCorelator;               //自由点相关器
        private readonly FreeDotDeleter _freeDotDeleter;  //自由点删除器
        private readonly List<ITargetObserver> _obs;      //目标观察者，目标变化时观察者得到通知
        private readonly MouseTargetTracker mouseTargetTracker;
        private readonly List<TrackGenerator> trackGenerators;
        private static readonly object _locker = new object();


        //测试用变量
        private TargetManagerMode _mode;

        int ITargetDataProvider.GetSectorCount() => Sector.SectorCount;

        public TargetManager()
        {
            InitializeSectors();
            _obs = new List<ITargetObserver>();

            _clotter = new Clotter_Test();
            _clotter47 = new FourSevenClotter();

            _trackCorelator = new TrackCorelatorV1();//航迹相关器

            _mode = TargetManagerMode.Intelligent;
            _dotCorelator = new DotCorelatorV1();//自由点相关器

            _freeDotDeleter = new FreeDotDeleter();  //自由点删除器

            _viewDeleter = new DotViewDeleter();

            mouseTargetTracker = new MouseTargetTracker(this);

            trackGenerators = new List<TrackGenerator>();
        }

        public void AddTrackGenerator(PolarCoordinate coordinate)
        {
            trackGenerators.Add(new TrackGenerator(this, coordinate));
        }

        internal void InitializeSectors()
        {
            //删除所有目标视图
            DeleteAllTargetViews();

            DestroySectors();

            CreateSectors();
        }

        private void DestroySectors()
        {
            lock (_locker)
            {
                //获得角度区域监听器
                var antennaLeaveAngleAreaSubject = TargetManagerFactory.CreateAntennaLeaveAngleAreaSubject();
                if (Sectors == null) return;
                foreach (Sector s in Sectors) //注销角度区域
                    antennaLeaveAngleAreaSubject.UnregisterAngleArea(this, s);
            }
        }

        private void CreateSectors()
        {
            lock (_locker)
            {
                //获得角度区域监听器
                var antennaLeaveAngleAreaSubject = TargetManagerFactory.CreateAntennaLeaveAngleAreaSubject();

                //初始化扇区并注册角度区域观察者
                Sectors = new Sector[Sector.SectorCount];
                for (int i = 0; i < Sector.SectorCount; i++)
                {
                    Sectors[i] = CreateSector(i, Sector.SectorCount);
                    antennaLeaveAngleAreaSubject.RegisterAngleArea(this, Sectors[i]);
                }
            }
        }

        private static Sector CreateSector(int index, int totalCount)
        {
            float step = (float)360 / totalCount;
            float beginAngle = index * step;
            float endAngle = (index + 1) * step;

            if (endAngle >= 360)
                endAngle = endAngle - 360;

            return new Sector(index, beginAngle, endAngle);
        }

        private void DeleteAllTargetViews()
        {
            if (Sectors == null)
                return;
            lock (_locker)
            {
                foreach (Sector s in Sectors)
                {
                    NotifyDeleteSectorDots(s);
                    NotifyDeleteSectorTracks(s);
                    mouseTargetTracker?.DeleteTrack();
                    s.ClearAllTargets();
                    foreach (var generator in trackGenerators)
                    {
                        generator.Dispose();
                    }
                    trackGenerators.Clear();
                }
            }
        }

        public List<Target> GetTargetTracks()   //获取所有航迹对象
        {
            List<Target> ls = new List<Target>();
            lock (_locker)
            {
                foreach (Sector s in Sectors)
                {
                    ls.AddRange(s.StableTracks);
                }

                ////添加航迹生成器的航迹
                //foreach (var generator in trackGenerators)
                //{
                //    ls.Add(generator.track);
                //}

                ////添加鼠标追踪器的航迹
                //if (mouseTargetTracker.track != null)
                //    ls.Add(mouseTargetTracker.track);
            }

            return ls;
        }

        public List<Target> GetTargetDots()     //获取所有当前圈目标点，GraphicView只显示当前圈的目标点
        {
            lock (_locker)
            {
                List<Target> ret = new List<Target>();
                foreach (Sector sector in Sectors)
                    ret.AddRange(sector.GetVisibleTargetDots());

                return ret;
            }
        }

        public TargetManagerMode GetMode() => _mode;

        public void DeleteActiveTarget()    //删除选中的航迹
        {
            List<TargetTrack> ls = new List<TargetTrack>();
            lock (_locker)
            {
                foreach (Sector s in Sectors)
                {
                    ls.AddRange(s.GetActiveTrack());    //数据值删除航迹之前先保存
                    s.DeleteActiveTrack();
                }

                foreach (TargetTrack t in ls)    //通知观察者，这些航迹被删除
                {
                    NotifyAllObservers(t, NotifyType.Delete);
                }

                mouseTargetTracker.DeleteActiveTarget();    //如果mouseTargetTracker航迹被选中，则置为空

                //删除TrackGenerator产生的航迹
                for (int index = trackGenerators.Count - 1; index >= 0; index--)
                {
                    if (trackGenerators[index].DeleteTrackIfActive())
                        trackGenerators.RemoveAt(index);
                }

                ls.Clear();
            }
        }

        public void SelectTarget(Target t)
        {
            if (t is TargetTrack)
            {
                //GraphicTargetView发送这条指令时已经设置了t.Active的值，目标管理器要做的只是通知所有其他观察者更新该航迹状态
                //如果GraphicTargetView只是发送目标引用，由目标管理器负责设置目标的Active属性的话，目标管理器需要按扇区查找目标的位置，浪费资源
                NotifyAllObservers(t, NotifyType.Update);
            }
            else
            {
                ////此段代码正常处理手动起批，如果使用MouseTargetTracker用鼠标的点击跟踪航迹，应注释此段代码
                //if (_mode != TargetManagerMode.Manual)   //非手动模式不作处理
                //    return;

                //TargetTrack track = TargetTrack.CreateTargetTrack(t.CurrentCoordinate, t.CurrentCoordinate);
                //if (track != null)
                //{
                //    _sectors[t.sectorIndex].AddTrack(track);
                //    NotifyAllObservers(track, NotifyType.Add);
                //}

                ////使用MouseTargetTracker用鼠标的点击跟踪航迹,如果需要手动起批，应注释此行代码
                mouseTargetTracker.UpdateTrack((TargetDot)t);
            }
        }

        public void SwitchMode(TargetManagerMode toMode)      //切换起批模式
        {
            _mode = toMode;

            switch(toMode)
            {
                case TargetManagerMode.Intelligent:
                    _dotCorelator = new DotCorelator_Intelligence(_dotCorelator.Observers);
                    break;
                case TargetManagerMode.Auto:
                    _dotCorelator = new DotCorelator_WaveGate(_dotCorelator.Observers);
                    break;
                case TargetManagerMode.SemiAuto:
                    _dotCorelator = new DotCorelator_WaveGate(_dotCorelator.Observers);
                    break;
                case TargetManagerMode.Manual:
                    _dotCorelator = new DotCorelator_Manual(_dotCorelator.Observers);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(toMode), toMode, null);
            }
        }

        public void RegisterObserver(ITargetObserver ob)
        {
            if (!_obs.Contains(ob) && ob != null)
                _obs?.Add(ob);

            _clotter.RegisterObserver(ob);           //代理凝聚器观察者的注册
            _trackCorelator.RegisterObserver(ob);    //代理相关器观察者的注册 
            _dotCorelator.RegisterObserver(ob);
            _freeDotDeleter.RegisterObserver(ob);
            _viewDeleter.RegisterObserver(ob);
            _clotter47.RegisterObserver(ob);
        }

        public void UnregisterObserver(ITargetObserver ob)
        {
            if (_obs.Contains(ob))
                _obs?.Remove(ob);

            _clotter.UnregisterObserver(ob);             //代理凝聚器观察者的反注册
            _trackCorelator.UnregisterObserver(ob);      //代理相关器观察者的反注册 
            _dotCorelator.UnregisterObserver(ob);
            _freeDotDeleter.UnregisterObserver(ob);
            _viewDeleter.UnregisterObserver(ob);
            _clotter47.UnregisterObserver(ob);
        }

        public void NotifyAllObservers(Target t, NotifyType type)    //通知观察者有目标发生了变化
        {
            foreach (ITargetObserver ob in _obs)
                ob.NotifyChange(t, type);
        }

        protected Sector PreviousSector(Sector s)   //返回该扇区的前一个扇区的引用
        {
            RotateDirection direction = TargetManagerFactory.GetAntennaDirection();
            switch (direction)
            {
                case RotateDirection.ClockWise:
                    lock (_locker)
                    {
                        return s.Index == 0 ? Sectors[Sector.SectorCount - 1] : Sectors[s.Index - 1];
                    }
                case RotateDirection.CounterClockWise:
                    lock (_locker)
                    {
                        return s.Index == Sector.SectorCount - 1 ? Sectors[0] : Sectors[s.Index + 1];
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected Sector NextSector(Sector s)   //返回该扇区的下一个扇区的引用
        {
            RotateDirection direction = TargetManagerFactory.GetAntennaDirection();
            switch (direction)
            {
                case RotateDirection.ClockWise:
                    lock (_locker)
                    {
                        return s.Index == Sector.SectorCount - 1 ? Sectors[0] : Sectors[s.Index + 1];
                    }
                case RotateDirection.CounterClockWise:
                    lock (_locker)
                    {
                        return s.Index == 0 ? Sectors[Sector.SectorCount - 1] : Sectors[s.Index - 1];
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ProcessSector(object o)        //天线扫过一个扇区时调用该函数，对该扇区的前后扇区进行操作
        {
            lock (this)
            {
                //s的编号为index
                Sector s = (Sector)o;

                Sector s1 = NextSector(s);
                _viewDeleter.DeleteViews(s1, false);

                Sector tmp = PreviousSector(s);
                //Sector tmp = s;

                //index - 1扇区点迹凝聚
                AzimuthCell[] azCells = CycleDataMatrix.CreateCycleDataMatrix().GetAzimuthCellsInSectorSpan(tmp, tmp);//获取刚扫过的扇区所包含的方位单元数组
                _clotter47.Clot(tmp, NextSector(tmp), PreviousSector(tmp), azCells);

                //tmp = PreviousSector(tmp);
                //_trackCorelator.Corelate(tmp, NextSector(tmp), PreviousSector(tmp));    //航迹相关
                //_dotCorelator.Corelate(tmp, NextSector(tmp), PreviousSector(tmp));      //自由点起批

                foreach (var generator in trackGenerators)  //更新产生的航迹
                    generator.UpdateTrack(s);
            }
        }

        public void NotifyLeaveAngleArea(AngleArea sector) => ProcessSector(sector);  //获得角度区域监听器的通知，天线刚刚扫过一个扇区sector

        public void DeleteOutRangedTargets(AngleArea area)    //删除角度范围外的所有目标
        {
            lock (_locker)
            {
                foreach (Sector s in Sectors)
                {
                    if (area.IsAngleInArea(s.BeginAngle) || area.IsAngleInArea(s.EndAngle)) continue;
                    _viewDeleter.DeleteViews(s, true);  //删除所有目标视图，包括目标航迹
                    s.ClearAllTargets();                //清空无关扇区的所有数据
                }
            }
        }

        protected void NotifyDeleteSectorTracks(Sector s)
        {
            foreach (ITargetObserver ob in _obs)
                ob.NotifyUpdateSectorTrack(null, s.Index);   //传递null,表示没有航迹需要显示
        }

        protected void NotifyDeleteSectorDots(Sector s)
        {
            foreach (ITargetObserver ob in _obs)
                ob.NotifyUpdateSectorDot(null, s.Index);   //传递null,表示没有航迹需要显示
        }
    }
}
