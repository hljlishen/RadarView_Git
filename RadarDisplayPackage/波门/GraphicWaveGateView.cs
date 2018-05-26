using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using TargetManagerPackage;
using Utilities;


namespace RadarDisplayPackage
{
    class GraphicWaveGateView : IDisposable
    {
        RenderTarget canvas;
        PathGeometry geometry;
        WaveGate waveGate;
        CoordinateSystem coodinateSystem;
        Point2F innerLeft, outterRight;
        //bool selected;
        Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush waveGateBrush;
        protected OverViewDisplayer displayer;
        int dashStyleCount = 0;     //显示边界风格计数，DashDot和Dash交替显示
        const int dashStyleTimes = 10;  //显示边界风格切换的次数，到达10次之后切换边界风格，DashDot->Dash或Dash->DashDot

        public GraphicWaveGateView(WaveGate waveGate, OverViewDisplayer ovd)
        {
            displayer = ovd;
            canvas = displayer.Canvas;
            
            this.waveGate = waveGate;

            waveGateBrush = canvas.CreateSolidColorBrush(waveGate.IsSemiAuto ? 
                Tools.GetColorFFromRgb(255 ,255 ,0) : 
                Tools.GetColorFFromRgb(245, 222, 179));

            coodinateSystem = displayer.coordinateSystem;

            //Selected = false;

            PolarCoordinate c = new PolarCoordinate
            {
                Az = waveGate.BeginAngle,
                //ProjectedDis = waveGate.BeginDistance;    //之前版本将距离赋给ProjectedDis
                Dis = waveGate.BeginDistance
            };

            innerLeft = coodinateSystem.CoordinateToPoint(c);

            c.Az = waveGate.EndAngle;
            //c.ProjectedDis = waveGate.EndDistance;  //之前版本将距离赋给projectedDis
            c.Dis = waveGate.EndDistance;
            outterRight = coodinateSystem.CoordinateToPoint(c);

            geometry = coodinateSystem.BuildWaveGateGeometry(innerLeft, outterRight);
        }

        public void Draw()
        {
            StrokeStyleProperties ssp = new StrokeStyleProperties();
            if (Selected)   //如果选中，填充区域coo
            {
                ssp = GetSelectedWaveGateStrokeStyleProperties();
            }
            else
            {
                waveGateBrush.Opacity = 0.2f;
                ssp.DashStyle = DashStyle.Solid;
            }


            canvas.FillGeometry(geometry, waveGateBrush);

            StrokeStyle ss = displayer.Factory.CreateStrokeStyle(ssp);

            waveGateBrush.Opacity = 1;
            canvas.DrawGeometry(geometry, waveGateBrush, 1.5f, ss);
        }

        private StrokeStyleProperties GetSelectedWaveGateStrokeStyleProperties()
        {
            StrokeStyleProperties ssp = new StrokeStyleProperties();
            waveGateBrush.Opacity = 0.6f;
            if (dashStyleCount < dashStyleTimes)
            {
                dashStyleCount++;
                ssp.DashStyle = DashStyle.DashDot;
            }
            else if (dashStyleCount < dashStyleTimes * 2)
            {
                dashStyleCount++;
                ssp.DashStyle = DashStyle.Dash;
            }
            else
            {
                dashStyleCount = 0;
                ssp.DashStyle = DashStyle.Dash;
            }

            return ssp;
        }

        public WaveGate WaveGate => waveGate;

        public bool Selected
        {
            get => waveGate.Active;

            set => waveGate.Active = value;
        }

        public void HandleMouseClick(Point p)
        {
            Point2F p1 = Tools.PointToPoint2F(p);
            if(IsPointInView(p1))    //如果点在波门范围内
                Selected = !Selected;   //selected取反
        }

        private bool IsPointInView(Point2F point)
        {
            PolarCoordinate c = coodinateSystem.PointToCoordinate(point);
            return waveGate.IsCoordinateInWaveGate(c);
        }

        public void Dispose()
        {
            waveGateBrush?.Dispose();
        }
    }
}
