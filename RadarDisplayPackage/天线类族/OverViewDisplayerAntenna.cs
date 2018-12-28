using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    class OverViewDisplayerAntenna : GraphicTrackDisplayerAntenna
    {
        protected int glowPieCount = 360;   //余辉包含的扇区快数
        protected int glowSweepAngle = 360;  //余辉覆盖的角度
        protected float glowSweepStep;      //余辉每个扇区块的覆盖角度
        public OverViewDisplayerAntenna(RenderTarget canvas, D2DFactory factory, CoordinateSystem coordinateSystem) : base(canvas, factory, coordinateSystem)
        {
            glowSweepStep = ((float)glowSweepAngle) / ((float)glowPieCount);
            
        }

        protected override void DrawAntenna()
        {
            //Layer layer = canvas.CreateLayer();
            //LayerParameters lp = new LayerParameters();
            //lp.ContentBounds = new RectF(0, 0, 500, 500);
            //lp.Opacity = 1;
            //canvas.PushLayer(lp, layer);
            Point2F center = coordinateSystem.OriginalPoint;
            Rect coordinateArea = coordinateSystem.CoordinateArea;

            float x1 = center.X + (int)(coordinateArea.Width / 2 * Math.Sin(Tools.DegreeToRadian(antennaAngle)));
            float y1 = center.Y - (int)(coordinateArea.Width / 2 * Math.Cos(Tools.DegreeToRadian(antennaAngle)));
            canvas.DrawLine( center, new Point2F(x1, y1),antennaBrush,antennaWidth);
            //canvas.PopLayer();
        }
    }
}
