using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;

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
        protected float sweepBeginAngle;
        protected float sweepEndAngle;
        protected Brush sweepBorderBrush;

        public virtual float AntennaAngle
        {
            get
            {
                return antennaAngle;
            }
            set
            {
                if (value >= 360)
                {
                    value = value % 360;
                }
                if (value < 0)
                {
                    value = value % 360;
                    value += 360;
                }
                antennaAngle = value;
            }
        }

        public GraphicTrackDisplayerAntenna(RenderTarget canvas, D2DFactory factory, CoordinateSystem coordinateSystem)
        {
            this.canvas = canvas;
            this.factory = factory;
            this.coordinateSystem = coordinateSystem;
            antennaBrush = canvas.CreateSolidColorBrush(new ColorF(255, 255, 255)); //白色

            //注册观察者
            antenna = TargetManagerFactory.CreateAntennaDataProvider();
            antenna.RegisterObserver(this);

            sweepBorderBrush = canvas.CreateSolidColorBrush(new ColorF(255, 0, 0)); //红色
        }

        public  void Draw()
        {
            DrawAntenna();
            if(antenna.IsSectionSweeping())
                DrawSweepBorderLine();      //画扫描边界线
        }

        abstract protected void DrawAntenna();
        abstract protected void DrawGlow(AntennaDirection d);

        public void AntennaNotifyChange()
        {
            antennaAngle = antenna.GetCurrentAntennaAngle();

            if(antenna.IsSectionSweeping()) //扇扫
            {
                sweepBeginAngle = antenna.GetSweepBeginAngle();
                sweepEndAngle = antenna.GetSweepEndAngle();
            }
        }

        public virtual void Dispose()
        {
            antennaBrush?.Dispose();
            sweepBorderBrush?.Dispose();
            antenna.UnregisterObserver(this);
        }

        protected virtual void DrawSweepBorderLine()
        {

        }
    }
}
