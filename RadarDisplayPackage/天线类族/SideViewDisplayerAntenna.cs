﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class SideViewDisplayerAntenna : GraphicTrackDisplayerAntenna
    {
        protected float glowLengthPercentage = 0.2f;
        protected float offsetPercentage = 1f;

        public SideViewDisplayerAntenna(RenderTarget canvas, D2DFactory factory, CoordinateSystem coordinateSystem) : base(canvas, factory, coordinateSystem)
        {
            
        }

        protected override void DrawAntenna()
        {
            Point2F originalPoint = coordinateSystem.OriginalPoint;
            Rect coordinateArea = coordinateSystem.CoordinateArea;
            if (antennaAngle < 0)
                antennaAngle += 360;
            antennaAngle %= 360;
            float x = coordinateArea.Left + ((float)antennaAngle / 360) * coordinateArea.Width;
            float y = coordinateArea.Top;

            canvas.DrawLine(new Point2F((int)x, coordinateArea.Bottom), new Point2F((int)x, coordinateArea.Top)
                , antennaBrush, antennaWidth);
            DrawSweepBorderLine();
        }

        protected override void DrawGlow(RotateDirection d)
        {
            //Rectangle axisRect = coordinateArea;
            //originalPoint = new Point(axisRect.Left, axisRect.Bottom);

            //float fLastAngle = antennaAngle;
            //int count = (int)(axisRect.Width * glowLengthPercentage);
            //float x = axisRect.Left + ((float)antennaAngle / 360) * axisRect.Width, x1;
            //float y = axisRect.Top;

            //for (int i = 1; i < count; i++)
            //{
            //    Brush brush = new SolidBrush(Color.FromArgb(255 - (i * 255 / (count)), Color.Green));
            //    //if (d == RotateDirection.ClockWise)
            //    x1 = x - i;
            //    //else
            //    //    x1 = x + i;
            //    if (x1 > axisRect.Left)
            //        drawer.DrawLine(new Pen(brush), new Point((int)x1, axisRect.Bottom), new Point((int)x1, axisRect.Top));
            //    else
            //        drawer.DrawLine(new Pen(brush), new Point((int)x1 + axisRect.Width, axisRect.Bottom), new Point((int)x1 + axisRect.Width, axisRect.Top));
            //}
        }

        protected override void DrawSweepBorderLine()
        {
            base.DrawSweepBorderLine();

            if (sweepBeginAngle == 0 && sweepEndAngle == 0)
                return;

            Point2F sweepBorderLinePoints1 = coordinateSystem.CalIntersectionPoint(sweepBeginAngle);
            Point2F sweepBorderLinePoints2 = coordinateSystem.CalIntersectionPoint(sweepEndAngle);

            canvas.DrawLine(new Point2F(sweepBorderLinePoints1.X, coordinateSystem.CoordinateArea.Top), sweepBorderLinePoints1, sweepBorderBrush, 3);
            canvas.DrawLine(new Point2F(sweepBorderLinePoints2.X, coordinateSystem.CoordinateArea.Top), sweepBorderLinePoints2, sweepBorderBrush, 3);
        }
    }
}
