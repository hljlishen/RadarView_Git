using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;

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
            Rect coordinateArea = coordinateSystem.CoordniteArea;

            float x1 = center.X + (int)(coordinateArea.Width / 2 * Math.Sin(GraphicTrackDisplayerBackground.DegreeToRadian(antennaAngle)));
            float y1 = center.Y - (int)(coordinateArea.Width / 2 * Math.Cos(GraphicTrackDisplayerBackground.DegreeToRadian(antennaAngle)));
            canvas.DrawLine( center, new Point2F(x1, y1),antennaBrush,antennaWidth);
            //canvas.PopLayer();
        }

        protected override void DrawGlow(RotateDirection d)
        {
            //float fLastAngle = antennaAngle - 90;

            //for (int i = 0; i < glowPieCount; i++)
            //{
            //    int alpha = i * 255 / (glowPieCount);
            //    Brush brush = new SolidBrush(Color.FromArgb(alpha, Color.Green));
            //    canvas.dra(brush, coordinateArea, fLastAngle, glowSweepStep);
            //    fLastAngle += glowSweepStep;
            //}
        }

        protected override void DrawSweepBorderLine()
        {
            base.DrawSweepBorderLine();

            Point2F sweepBorderLinePoints1 = coordinateSystem.CalIntersectionPoint(sweepBeginAngle);
            Point2F sweepBorderLinePoints2 = coordinateSystem.CalIntersectionPoint(sweepEndAngle);

            canvas.DrawLine(coordinateSystem.OriginalPoint, sweepBorderLinePoints1, sweepBorderBrush, 3);
            canvas.DrawLine(coordinateSystem.OriginalPoint, sweepBorderLinePoints2, sweepBorderBrush, 3);
        }
    }
}
