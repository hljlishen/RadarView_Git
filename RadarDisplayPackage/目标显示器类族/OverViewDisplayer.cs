using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Windows.Forms;
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.DirectX.WindowsImagingComponent;

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
        protected float beginAngle;    //视图正上方对应的角度
        internal OverViewDisplayerState viewState;  //视图状态：放大、天线控制、自动、半自动
        const int MaximumZoom = 40;  //放大倍数的最大值，超过这个值不处理放大操作
        GraphicWaveGateViewManager waveGateViewManager;   //当前存在的波门视图
        SweepSectionManager sweepSectionManager;            //扫描区域视图管理器
        protected List<IControlStateObserver> obs;
        private MouseCoordinateDisplayer mouseCoordinateDisplayer;

        public OverViewDisplayer(Panel h) : base(h)
        {
            beginAngle = 0.0f;

            viewState = CreateState(OverViewState.Zoom);

            waveGateViewManager = new GraphicWaveGateViewManager(this) ;

            sweepSectionManager = new SweepSectionManager(this);

            mouseCoordinateDisplayer = new MouseCoordinateDisplayer(this);

            obs = new List<IControlStateObserver>();
            //Distance = 3000;
            h.MouseClick += HOnMouseClick;
        }

        private void HOnMouseClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (Control.ModifierKeys == Keys.Control)   //ctrl键按下,添加一个TrackGenerator
            {
                PolarCoordinate coordinate =
                    coordinateSystem.PointToCoordinate(CoordinateSystem.PointToPoint2F(mouseEventArgs.Location));
                TargetManagerAddTrackGeneratorCommand generator = new TargetManagerAddTrackGeneratorCommand(coordinate);
                generator.Execute();
            }
        }

        public override float Distance
        {
            get => base.Distance;

            set
            {
                base.Distance = value;
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

            //计算放大后左上角坐标

            //计算放大后的宽度和高度

            if (zoomedCoordinateArea.Width / drawArea.Width > MaximumZoom)  //放大超过极限
            {
                return;
            }
            SetCoordinateArea(zoomedCoordinateArea);
        }

        public void OffsetDisplayerToPoint(Point2F position)  //偏心显示
        {
            //float x = position.X - coordinateSystem.OriginalPoint.X;
            //float y = position.Y - coordinateSystem.OriginalPoint.Y;
            //Rect r = CoordinateSystem.MoveRect(coordinateSystem.CoordinateArea, new Point2F(x, y));
            //SetCoordinateArea(r);
        }

        private void SetCoordinateArea(Rect area)   //重新设置坐标系的位置，并根据新位置生成新的背景，天线，目标图层
        {
            Dispose();  //释放当前资源
            coordinateSystem.CoordinateArea = area;  //最先重算坐标系位置和大小，坐标系是其他部件的基础
            background = CreateBackground();        //根据坐标系重算背景位置大小
            antenna = CreateAntenna();              //根据坐标系重算天线位置大小
            waveGateViewManager = CreateWaveGateViewManager();  //根据坐标系重算波门位置大小
            viewState = CreateState(viewState.GetState());  //根据坐标系重算状态数据
            sweepSectionManager.CalSweepSectionView();      //重新计算区域大小
            targetsManager.Dispose();
            targetsManager = new GraphicTargetViewManager(this);
        }

        internal override GraphicTrackDisplayerBackground CreateBackground()
        {
            OverViewDisplayerBackground ovdb = new OverViewDisplayerBackground(Canvas, Factory, coordinateSystem);
            ovdb.Distance = distance;   //设置距离量程

            //创建角度分划线虚线风格
            StrokeStyleProperties ssp = new StrokeStyleProperties();
            ssp.DashStyle = DashStyle.DashDot;
            
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
            viewState?.Dispose();
            OverViewDisplayerState ret = null;
            switch (state)
            {
                case OverViewState.Zoom:
                    ret = new OverViewDisplayer_Zoom(this);
                    break;

                case OverViewState.AntennaControl:
                    ret = new OverViewDisplayer_AntennaControlFixed60(this);
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
            waveGateViewManager?.Dispose();
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
                DisplayControl.MouseDown -= viewState.MouseDown;
                DisplayControl.MouseUp -= viewState.MouseUp;
                DisplayControl.MouseMove -= viewState.MouseMove;
            }
            catch
            {
                // ignored
            }

            viewState?.Dispose();
        }

        public void SwitchState(OverViewState state)
        {
            if (state == viewState.GetState())
                return;
            viewState = CreateState(state);

            NotifyAllControlStateObservers();   //通知观察者
        }

        public void ResetView()
        {
            SetCoordinateArea(coordinateSystem.OriginalRect);    //OriginalRect为窗口初始化是，视图的原始大小
        }

        protected override void OtherDrawing()
        {
            base.OtherDrawing();

            waveGateViewManager.Draw();

            sweepSectionManager.Draw();

            mouseCoordinateDisplayer.Draw();

            viewState.Draw();
        }

        public void RegisterObserver(IControlStateObserver ob)
        {
            if (ob == null)
                return;
            if (!obs.Contains(ob))
                obs.Add(ob);
        }

        public void UnregisterObserver(IControlStateObserver ob)
        {
            if (ob == null)
                return;
            if (obs.Contains(ob))
                obs.Remove(ob);
        }

        protected void NotifyAllControlStateObservers()
        {
            foreach(IControlStateObserver ob in obs)
            {
                ob.NotifyChange(viewState.GetState());
            }
        }
    }
}