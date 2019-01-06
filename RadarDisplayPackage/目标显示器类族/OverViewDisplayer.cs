
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Windows.Forms;
using TargetManagerPackage;
using Utilities;
using System;

namespace RadarDisplayPackage
{
    public enum OverViewState
    {
        Zoom,
        SemiAutoWaveGate,
        AutoWaveGate,
        AntennaControl
    }

    public class OverViewDisplayer : GraphicTrackDisplayer,IControlStateSubject
    {
        public delegate void NewWaveGateHandler(WaveGate waveGate);
        public event NewWaveGateHandler NewWaveGate;
        public delegate void NewSweepSectionHandler(AngleArea area);
        public event NewSweepSectionHandler NewSweepSection;
        protected float beginAngle;    //视图正上方对应的角度
        internal OverViewDisplayerState ViewState;  //视图状态：放大、天线控制、自动、半自动
        private const int MaximumZoom = 40;  //放大倍数的最大值，超过这个值不处理放大操作
        GraphicWaveGateViewManager _waveGateViewManager;   //当前存在的波门视图
        private readonly SweepSectionManager _sweepSectionManager;            //扫描区域视图管理器
        protected List<IControlStateObserver> Obs;
        private readonly MouseCoordinateDisplayer _mouseCoordinateDisplayer;
        private readonly UpScroller _upScroller;
        private readonly RightScroller _rightScroller;
        private readonly DownScroller _downScroller;
        private readonly LeftScroller _leftScroller;

        public CoordinateSystemOfPolar CoordinateSystem => (CoordinateSystemOfPolar) coordinateSystem;

        public OverViewDisplayer(Panel displayerContain) : base(displayerContain)
        {
            targetsManager = new CoordinateTargetViewManager(this);

            beginAngle = 0.0f;

            ViewState = CreateState(OverViewState.Zoom);

            _waveGateViewManager = new GraphicWaveGateViewManager(this) ;

            _sweepSectionManager = new SweepSectionManager(this);

            _mouseCoordinateDisplayer = new MouseCoordinateDisplayer(this);

            _upScroller = new UpScroller(this);
            _rightScroller = new RightScroller(this);
            _downScroller = new DownScroller(this);
            _leftScroller = new LeftScroller(this);

            Obs = new List<IControlStateObserver>();
            displayerContain.MouseClick += HOnMouseClick;
        }

        private void HOnMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (Control.ModifierKeys == Keys.Control)   //ctrl键按下,添加一个TrackGenerator
            {
                PolarCoordinate coordinate =
                    coordinateSystem.PointToCoordinate(Tools.PointToPoint2F(mouseEventArgs.Location));
                TargetManagerAddTrackGeneratorCommand generator = new TargetManagerAddTrackGeneratorCommand(coordinate);
                generator.Execute();
            }
            DisplayControl.Focus();
        }

        public override float Distance
        {
            get => base.Distance;

            set
            {
                base.Distance = value;
                CycleDataDrivePackage.DistanceCellFilter.DistanceMax = distance;
                background.Distance = value;
                SetCoordinateArea(coordinateSystem.CoordinateArea);
            }
        }
        public void ZoomArea(Rect zoomArea)    //视图放大
        {
            Rect coordinateArea = coordinateSystem.CoordinateArea;       //当前显示区域，需要将zoomArea放大到这个区域
            float mul = coordinateArea.Width / (float)zoomArea.Width;   //放大的倍数
            int x = zoomArea.Left - coordinateArea.Left;    //待放大区域左侧距离坐标区域左侧的差值
            int y = zoomArea.Top - coordinateArea.Top;      //待放大区域顶部距离坐标区域顶部的差值
            Rect zoomedCoordinateArea = new Rect
            {
                Left = coordinateArea.Left - (int) (x * mul),
                Top = coordinateArea.Top - (int) (y * mul),
                Width = (int) (coordinateArea.Width * mul),
                Height = (int) (coordinateArea.Height * mul)
            }; //放大后的coordinateArea

            if (zoomedCoordinateArea.Width / drawArea.Width > MaximumZoom)  //放大超过极限
                return;

            SetCoordinateArea(zoomedCoordinateArea);
        }

        public void OffsetDisplayerToPoint(Point2F position)  //偏心显示
        {
            //float x = position.X - coordinateSystem.OriginalPoint.X;
            //float y = position.Y - coordinateSystem.OriginalPoint.Y;
            //Rect r = CoordinateSystem.MoveRect(coordinateSystem.CoordinateArea, new Point2F(x, y));
            //SetCoordinateArea(r);
        }

        public void SetCoordinateArea(Rect area)   //重新设置坐标系的位置，并根据新位置生成新的背景，天线，目标图层
        {
            lock (_locker)
            {
                Dispose();  //释放当前资源
                coordinateSystem.CoordinateArea = area;  //最先重算坐标系位置和大小，坐标系是其他部件的基础
                background = CreateBackground();        //根据坐标系重算背景位置大小
                antenna = CreateAntenna();              //根据坐标系重算天线位置大小
                _waveGateViewManager = CreateWaveGateViewManager();  //根据坐标系重算波门位置大小
                ViewState = CreateState(ViewState.GetState());  //根据坐标系重算状态数据
                _sweepSectionManager.CalSweepSectionView();      //重新计算区域大小
                targetsManager.Dispose();
                targetsManager = new CoordinateTargetViewManager(this);
            }
            GC.Collect();
        }

        internal override GraphicTrackDisplayerBackground CreateBackground()
        {
            OverViewDisplayerBackground ovdb = new OverViewDisplayerBackground(Canvas, Factory, coordinateSystem);
            ovdb.Distance = distance;   //设置距离量程

            //创建角度分划线虚线风格
            StrokeStyleProperties ssp = new StrokeStyleProperties {DashStyle = DashStyle.DashDot};

            ovdb.AngleLineStrokeStyle = Factory.CreateStrokeStyle(ssp);
            
            return ovdb;
        }

        internal override GraphicTrackDisplayerAntenna CreateAntenna()
        {
            OverViewDisplayerAntenna ovda = new OverViewDisplayerAntenna(Canvas,Factory, coordinateSystem);
            return ovda;
        }

        internal OverViewDisplayerState CreateState(OverViewState state)
        {
            ViewState?.Dispose();
            OverViewDisplayerState ret = null;
            switch (state)
            {
                case OverViewState.Zoom:
                    ret = new OverViewDisplayer_Zoom(this);
                    break;

                case OverViewState.AntennaControl:
                    //ret = new OverViewDisplayerAntennaControlFixed60(this);
                    ret = new OverViewDisplayerAntennaContrlAttach(this);
                    break;

                case OverViewState.AutoWaveGate:
                    ret = new OverViewDisplayer_AutoWaveGate(this);
                    break;
                case OverViewState.SemiAutoWaveGate:
                    ret = new OverViewDisplayer_SemiAutoWaveGate(this);
                    break;
            }

            return ret;
        }

        internal GraphicWaveGateViewManager CreateWaveGateViewManager()
        {
            _waveGateViewManager?.Dispose();
            return new GraphicWaveGateViewManager(this);
        }

        internal override CoordinateSystem CreateCoordinateSystem()
        {
            return new CoordinateSystemOfPolar(drawArea, distance, Factory);
        }

        public override void Dispose()
        {
            base.Dispose();
            try
            {
                DisplayControl.MouseDown -= ViewState.MouseDown;
                DisplayControl.MouseUp -= ViewState.MouseUp;
                DisplayControl.MouseMove -= ViewState.MouseMove;
            }
            catch
            {
                // ignored
            }

            ViewState?.Dispose();
        }

        public void SwitchState(OverViewState state)
        {
            if (state == ViewState.GetState())
                return;
            ViewState = CreateState(state);

            NotifyAllControlStateObservers();   //通知观察者
        }

        public void ResetView() => SetCoordinateArea(coordinateSystem.OriginalRect);

        protected override void OtherDrawing()
        {
            base.OtherDrawing();

            _waveGateViewManager.Draw();

            _sweepSectionManager.Draw();

            _mouseCoordinateDisplayer.Draw();
            _upScroller.Draw();
            _rightScroller.Draw();
            _downScroller.Draw();
            _leftScroller.Draw();

            ViewState.Draw();
        }

        public void RegisterObserver(IControlStateObserver ob)
        {
            if (ob == null)
                return;
            if (!Obs.Contains(ob))
                Obs.Add(ob);
        }

        public void UnregisterObserver(IControlStateObserver ob)
        {
            if (ob == null)
                return;
            if (Obs.Contains(ob))
                Obs.Remove(ob);
        }

        protected void NotifyAllControlStateObservers()
        {
            foreach(IControlStateObserver ob in Obs)
            {
                ob.NotifyChange(ViewState.GetState());
            }
        }

        public virtual void SendNewWaveGate(WaveGate wavegate) => NewWaveGate?.Invoke(wavegate);

        public virtual void SendNewSweepSection(AngleArea area) => NewSweepSection?.Invoke(area);

        internal override ISweepSectionView CreateSweepSwctionView(AngleArea sweepSection)
        {
            return new OverSweepSectionView(sweepSection, this);
        }
    }
}