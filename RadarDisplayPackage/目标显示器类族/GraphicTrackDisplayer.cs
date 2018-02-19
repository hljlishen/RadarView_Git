using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Windows.Forms;

namespace RadarDisplayPackage
{
    public abstract class GraphicTrackDisplayer : TrackDisplayer, IDisposable
    {
        protected Rect drawArea;            //雷达显示区域和目标点显示的区域，是canvas的一部分
        internal GraphicTrackDisplayerAntenna antenna;          //天线对象
        internal GraphicTrackDisplayerBackground background;    //背景对象，静态图像
        internal CoordinateSystem coordinateSystem;             //坐标系

        protected GraphicTrackDisplayer(Panel holder) : base(holder)
        {
            InitializeDirect2D(holder);     //初始化direct2d
            drawArea = new Rect(0, 0, holder.Width, holder.Height);
            distance = 5000;

            // ReSharper disable once VirtualMemberCallInConstructor
            coordinateSystem = CreateCoordinateSystem();    //最先创建坐标系
            targetsManager = new GraphicTargetViewManager(this);
            background = CreateBackground();    //创建背景
            antenna = CreateAntenna();          //创建天线
        }

        protected void InitializeDirect2D(Panel holder) //用控件初始化Direct2D变量
        {
            Factory = D2DFactory.CreateFactory(D2DFactoryType.Multithreaded);   //创建工厂
            var rtps = new RenderTargetProperties();
            var hrtp = new HwndRenderTargetProperties(holder.Handle,new SizeU((uint)holder.Width, (uint)holder.Height), PresentOptions.None);
            Canvas = Factory.CreateHwndRenderTarget(rtps, hrtp);
        }

        internal abstract GraphicTrackDisplayerBackground CreateBackground();

        internal abstract GraphicTrackDisplayerAntenna CreateAntenna();

        internal abstract CoordinateSystem CreateCoordinateSystem();

        public override void UpdateInfomation()   //定时器时间到，更新picbox中的图形，需要绘制天线和航迹
        {
            if (Canvas == null)
                return;
            Canvas.BeginDraw();
            Canvas.Clear();

            //绘制背景
            background.Draw();

            //绘制目标
            targetsManager.DisplayTargetViews();

            //绘制天线
            antenna.Draw();

            //其他：波门、扫描范围线
            OtherDrawing(); //模板模式的hook

            Canvas.EndDraw();
        }

        public override float Distance
        {
            get => base.Distance;

            set
            {
                distance = value;
                coordinateSystem.Range = value;
            }
        }

        public D2DFactory Factory { get; private set; }

        public HwndRenderTarget Canvas { get; private set; }

        public virtual void Dispose()
        {
            antenna?.Dispose();         //天线对象
            background?.Dispose();   //背景对象，静态图像
        }

        protected virtual void OtherDrawing(){ }

        public static ColorF GetColorFFromRgb(int r, int g, int b)
        {
            return new ColorF(new ColorI(r, g, b));
        }
    }
}
