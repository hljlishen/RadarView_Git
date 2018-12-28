using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    internal abstract class GraphicTrackDisplayerAntenna : IDisposable,IAntennaObserver
    {
        protected float preAntennaAngle = 0.0f;
        protected float antennaAngle = 0.0f;
        protected Brush antennaBrush;
        protected float antennaWidth = 3;
        protected RenderTarget canvas;
        protected D2DFactory factory;
        protected CoordinateSystem coordinateSystem;
        protected IAntennaDataProvider antenna;

        public virtual float AntennaAngle
        {
            get => antennaAngle;
            set => antennaAngle = Tools.StandardAngle(value);
        }

        protected GraphicTrackDisplayerAntenna(RenderTarget canvas, D2DFactory factory, CoordinateSystem coordinateSystem)
        {
            this.canvas = canvas;
            this.factory = factory;
            this.coordinateSystem = coordinateSystem;
            antennaBrush = canvas.CreateSolidColorBrush(new ColorF(255, 255, 255)); //白色

            //注册观察者
            antenna = TargetManagerFactory.CreateAntennaDataProvider();
            antenna.RegisterObserver(this);
        }

        public  void Draw()
        {
            DrawAntenna();
        }

        abstract protected void DrawAntenna();

        public void AntennaNotifyChange()
        {
            antennaAngle = antenna.GetCurrentAntennaAngle();
        }

        public virtual void Dispose()
        {
            antennaBrush?.Dispose();
            antenna.UnregisterObserver(this);
        }
    }
}
