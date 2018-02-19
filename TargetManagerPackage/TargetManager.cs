using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;
using System.Threading;

namespace TargetManagerPackage
{
   internal class TargetManager :ITargetDataProvider, ITargetManagerController, ILeaveAngleAreaObserver/*, ISweepModeObserver*/ //目标管理器
    {
        const int SectorCount = 72;     //扇区的个数
        Sector[] sectors;               //扇区数组
        DotViewDeleter viewDeleter;     //目标删除器
        Clotter clotter;                //凝聚器
        TrackCorelator trackCorelator;  //航迹相关器
        DotCorelator dotCorelator;      //自由点相关器
        FreeDotDeleter freeDotDeleter;  //自由点删除器
        List<ITargetObserver> obs;      //目标观察者，目标变化时观察者得到通知

        //测试用变量
        private TargetManagerMode mode;

        public int GetSectorCount()
        {
            return SectorCount;
        }
        internal CycleDataMatrix Matrix { get; private set; }

        public TargetManager()
        {
            //初始化周期数据管理器
            Matrix = new CycleDataMatrix();

            InitializeSectors();
            obs = new List<ITargetObserver>();

            clotter = new Clotter_Test();
            //clotter = new Clotter_3DClot();//凝聚器

            trackCorelator = new TrackCorelatorV1();//航迹相关器

            mode = TargetManagerMode.Manual;
            dotCorelator = new DotCorelator_Manual();//自由点相关器

            freeDotDeleter = new FreeDotDeleter();  //自由点删除器

            viewDeleter = new DotViewDeleter();
        }

        private void InitializeSectors()
        {
            //删除所有目标视图
            DeleteAllTargetViews();

            DistroySectors();

            CreateSectors();
        }

        private void DistroySectors()
        {
            //获得角度区域监听器
            AngleAreaSurveillance angleAreaSurveillance = TargetManagerFactory.CreateAngleAreaSurveillance();
            if (sectors != null)
            {
                foreach (Sector s in sectors)    //注销角度区域
                {
                    angleAreaSurveillance.UnregisterAngleArea(this, s);
                }
            }
        }

        private void CreateSectors()
        {
            //获得角度区域监听器
            AngleAreaSurveillance angleAreaSurveillance = TargetManagerFactory.CreateAngleAreaSurveillance();

            //初始化扇区并注册角度区域观察者
            sectors = new Sector[SectorCount];
            for (int i = 0; i < SectorCount; i++)
            {
                sectors[i] = CreateSector(i, SectorCount);
                angleAreaSurveillance.RegisterAngleArea(this, sectors[i]);
            }
        }

        private Sector CreateSector(int index, int totalCount)
        {
            float step = ((float)360) / totalCount;
            float beginAngle = index * step;
            float endAngle = (index + 1) * step;

            if (endAngle >= 360)
                endAngle = endAngle - 360;

            return new Sector(index, beginAngle, endAngle);
        }

        private void DeleteAllTargetViews()
        {
            if (sectors == null)
                return;
            foreach(Sector s in sectors)
            {
                NotifyDeleteSectorDot(s);
                s.newDots.Clear();

                NotifyDeleteSectorTrack(s);
                s.tracks.Clear();

                s.oldDots.Clear();
            }
        }

        public void ConnectDataSource(ICycleDataSubject subject)
        {
            Matrix.Dispose();
            Matrix = new CycleDataMatrix();
            subject.RegisterObserver(Matrix);
            InitializeSectors();
        }

        public List<Target> GetTargetTracks()   //获取所有航迹对象
        {
            List<Target> ls = new List<Target>();
            foreach (Sector s in sectors)
            {
                ls.AddRange(s.tracks);
            }
            return ls;
        }

        public List<Target> GetTargetDots()     //获取所有当前圈目标点，GraphicView只显示当前圈的目标点
        {
            List<Target> ls = new List<Target>();
            foreach (Sector s in sectors)
            {
                foreach(TargetDot dot in s.newDots)
                {
                    if (dot.ShouldDisplay)
                        ls.Add(dot);
                }
            }
            return ls;
        }

        public TargetManagerMode GetMode()
        {
            return mode;
        }

        public void DeleteActiveTarget()    //删除选中的航迹
        {
            List<TargetTrack> ls = new List<TargetTrack>();
            foreach(Sector s in sectors)
            {
                ls.AddRange(s.GetActiveTrack());    //数据值删除航迹之前先保存
                s.DeleteActiveTrack();
            }

            foreach(TargetTrack t in ls)    //通知观察者，这些航迹被删除
            {
                NotifyAllObservers(t, NotifyType.Delete);
            }

            ls.Clear();
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
                if (mode != TargetManagerMode.Manual)   //非手动模式不作处理
                    return;

                TargetTrack track = TargetTrack.CreateTargetTrack(t.CurrentCoordinate, t.CurrentCoordinate);
                if (track != null)
                {
                    sectors[t.sectorIndex].AddTrack(track);
                    NotifyAllObservers(track, NotifyType.Add);
                }
            }
        }

        public void SwitchMode(TargetManagerMode mode)      //切换起批模式
        {
            this.mode = mode;

            switch(mode)
            {
                case TargetManagerMode.Intelligent:
                    dotCorelator = new DotCorelator_Intelligence(dotCorelator.Observers);
                    break;
                case TargetManagerMode.Auto:
                    dotCorelator = new DotCorelator_WaveGate(dotCorelator.Observers);
                    break;
                case TargetManagerMode.SemiAuto:
                    dotCorelator = new DotCorelator_WaveGate(dotCorelator.Observers);
                    break;
                case TargetManagerMode.Manual:
                    dotCorelator = new DotCorelator_Manual(dotCorelator.Observers);
                    break;
            }
        }

        public void RegisterObserver(ITargetObserver ob)
        {
            if (!obs.Contains(ob) && ob != null)
                obs?.Add(ob);

            clotter.RegisterObserver(ob);           //代理凝聚器观察者的注册
            trackCorelator.RegisterObserver(ob);    //代理相关器观察者的注册 
            dotCorelator.RegisterObserver(ob);
            freeDotDeleter.RegisterObserver(ob);
            viewDeleter.RegisterObserver(ob);
        }

        public void UnregisterObserver(ITargetObserver ob)
        {
            if (obs.Contains(ob))
                obs?.Remove(ob);

            clotter.UnregisterObserver(ob);             //代理凝聚器观察者的反注册
            trackCorelator.UnregisterObserver(ob);      //代理相关器观察者的反注册 
            dotCorelator.UnregisterObserver(ob);
            freeDotDeleter.UnregisterObserver(ob);
            viewDeleter.UnregisterObserver(ob);
        }

        protected void NotifyAllObservers(Target t, NotifyType type)    //通知观察者有目标发生了变化
        {
            foreach (ITargetObserver ob in obs)
                ob.NotifyChange(t, type);
        }

        protected Sector PreviousSector(Sector s)   //返回该扇区的前一个扇区的引用
        {
            AntennaDirection direction = TargetManagerFactory.GetAntennaDirection();
            switch (direction)
            {
                case AntennaDirection.ClockWise:
                    return s.index == 0 ? sectors[SectorCount - 1] : sectors[s.index - 1];
                case AntennaDirection.CounterClockWise:
                    return s.index == SectorCount - 1 ? sectors[0] : sectors[s.index + 1];
                case AntennaDirection.Stopped:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected Sector NextSector(Sector s)   //返回该扇区的下一个扇区的引用
        {
            AntennaDirection direction = TargetManagerFactory.GetAntennaDirection();
            switch (direction)
            {
                case AntennaDirection.ClockWise:
                    return s.index == SectorCount - 1 ? sectors[0] : sectors[s.index + 1];
                case AntennaDirection.CounterClockWise:
                    return s.index == 0 ? sectors[SectorCount - 1] : sectors[s.index - 1];
                case AntennaDirection.Stopped:
                    return null;
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

                if (s.index == 0)
                {

                }
                Sector s1 = NextSector(s);
                viewDeleter.DeleteViews(s1, false);
                //s1 = NextSector(s1);
                //viewDeleter.DeleteViews(s1, false);
                //s1 = NextSector(s1);
                //viewDeleter.DeleteViews(s1, false);
                //s1 = NextSector(s1);
                //viewDeleter.DeleteViews(s1, false);

                Sector tmp = PreviousSector(s);

                //index - 1扇区点迹凝聚
                AzimuthCell[] azCells = GetAzimuthCellsInSectorSpan(PreviousSector(tmp), NextSector(tmp));//获取刚扫过的扇区所包含的方位单元数组
                Sector pre = PreviousSector(tmp);
                Sector nex = NextSector(tmp);
                clotter.Clot(tmp, nex, pre, azCells);

                //对s.index - 2的扇区进行相关
                tmp = PreviousSector(tmp);
                pre = PreviousSector(tmp);
                nex = NextSector(tmp);
                trackCorelator.Corelate(tmp, nex, pre);

                //对s.index-3的扇区进行自由点相关
                tmp = PreviousSector(tmp);
                pre = PreviousSector(tmp);
                nex = NextSector(tmp);
                dotCorelator.Corelate(tmp, nex, pre);

                //s.index - 4扇区，删除自由点
                tmp = PreviousSector(tmp);
                freeDotDeleter.DeleteFreeDot(tmp);

                //tmp = PreviousSector(tmp);
                //viewDeleter.DeleteViews(tmp,false);
            }
        }

        public AzimuthCell[] GetAzimuthCellsInSectorSpan(Sector previous, Sector next)
        {
            AngleArea area = CalCoveredAngleArea(previous, next);

            return Matrix.GetAzimuthCellArray(area);
        }

        private static AngleArea CalCoveredAngleArea(Sector s1, Sector s2)
        {
            float begin;
            float end;

            if (Math.Abs(s2.index - s1.index) > 2)   //大于2说明三个扇区跨越360度
            {
                begin = (s1.index > s2.index) ? s1.BeginAngle : s2.BeginAngle;
                end = (s1.index < s2.index) ? s1.EndAngle : s2.EndAngle;
            }
            else
            {
                begin = (s1.index < s2.index) ? s1.BeginAngle : s2.BeginAngle;
                end = (s1.index > s2.index) ? s1.EndAngle : s2.EndAngle;
            }

            return new AngleArea(begin, end);
        }

        public void NotifyLeaveAngleArea(AngleArea sector)   //获得角度区域监听器的通知，天线刚刚扫过一个扇区sector
        {
            //Thread processThread = new Thread(new ParameterizedThreadStart(ProcessSector));
            //processThread.Priority = ThreadPriority.BelowNormal;
            //processThread.Start(sector);

            ProcessSector(sector);
        }

        public void DeleteOutRangedTargets(AngleArea area)    //删除角度范围外的所有目标
        {
            foreach (Sector s in sectors)
            {
                if (!area.IsAngleInArea(s.BeginAngle) && !area.IsAngleInArea(s.EndAngle))
                {
                    viewDeleter.DeleteViews(s, true); //删除所有目标视图，包括目标航迹
                    s.ClearAllTargets(); //清空无关扇区的所有数据
                }
            }
        }

        public void ClearRawData()
        {
            Matrix.Clear();
        }

        protected void NotifyDeleteSectorTrack(Sector s)
        {
            foreach (ITargetObserver ob in obs)
                ob.NotifyUpdateSectorTrack(null, s.index);   //传递null,表示没有航迹需要显示
        }

        protected void NotifyDeleteSectorDot(Sector s)
        {
            foreach (ITargetObserver ob in obs)
                ob.NotifyUpdateSectorDot(null, s.index);   //传递null,表示没有航迹需要显示
        }
    }
}
