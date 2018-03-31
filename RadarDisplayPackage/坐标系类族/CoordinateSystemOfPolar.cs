//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using System.Drawing;
using SweepDirection = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SweepDirection;

namespace RadarDisplayPackage
{
    internal class CoordinateSystemOfPolar : CoordinateSystem
    {
        public CoordinateSystemOfPolar(Rect drawArea, double Range, D2DFactory factory) :base(drawArea, Range, factory)
        {
            ZoomPercent = 1.5;
            this.Range = Range;
        }

        protected override Point2F FindOriginalPoint(Rect coordniteArea)
        {
            return FindCenterPosition(coordniteArea);   //极坐标系的坐标原点即为区域中心点
        }

        public override Point2F CoordinateToPoint(PolarCoordinate coordinate)
        {
            double projectedDistance;
            if (coordinate.ProjectedDis != -1)      //有待优化，取projectedDis的方式不合理
                projectedDistance = coordinate.ProjectedDis;
            else
                projectedDistance = coordinate.Dis * Math.Cos(AngleToRadian(coordinate.El));  //水平面的投影距离

            //projectedDistance = coordinate.Dis;     //会导致波门绘制失效

            double r = (((double)CoordinateArea.Width / 2) / (Range)) * (projectedDistance);
            double sin = Math.Sin(AngleToRadian(coordinate.Az));
            double cos = Math.Cos(AngleToRadian(coordinate.Az));
            double x1 = r * sin;
            double y1 = r * cos;
            x1 = OriginalPoint.X + x1;
            y1 = OriginalPoint.Y - y1;

            Point2F p = new Point2F((int)x1, (int)y1);

            return p;
        }

        public override PolarCoordinate PointToCoordinate(Point2F p)
        {
            PolarCoordinate c = new PolarCoordinate();
            c.Az = AngleToNorth(OriginalPoint, p);
            float r = (float)DistanceBetween(OriginalPoint, p);
            float projectedDistance = (float)(r * Range * 2 / CoordinateArea.Width);
            c.ProjectedDis = projectedDistance;
            c.El = -1;
            //c.Dis = -1;
            c.Dis = c.ProjectedDis;
            c.El = 0;

            return c;
        }

        public override bool PointOutOfRange(Point p)
        {
            float distance = (float)DistanceBetween(OriginalPoint, PointToPoint2F(p));

            if (distance > CoordinateArea.Width / 2)
                return true;
            else
                return false;
        }

        public override PathGeometry BuildWaveGateGeometry(Point2F position1, Point2F position2)
        {
            Point2F innerLeft, outterLeft, outterRight, innerRight;
            PathGeometry waveGate = Factory.CreatePathGeometry();

            float mouseBeginAngle = CoordinateSystem.AngleToNorth(OriginalPoint, position1);
            float mouseEndAngle = CoordinateSystem.AngleToNorth(OriginalPoint, position2);

            float begin = FindSmallArcBeginAngle(mouseBeginAngle, mouseEndAngle);
            float end = FindSmallArcEndAngle(mouseBeginAngle, mouseEndAngle);

            float mouseBeginDis = (float)DistanceBetween(OriginalPoint, position1);
            float mouseEndDis = (float)DistanceBetween(OriginalPoint, position2);
            Point2F mouseBeginZoomed = RadiusWiseZoomPosition(position1, mouseEndDis);
            Point2F mouseDragZoomed = RadiusWiseZoomPosition(position2, mouseBeginDis);

            if (begin == mouseBeginAngle)    //扇形在鼠标点击一侧开始顺时针扫过
            {
                if (mouseBeginDis < mouseEndDis) //鼠标向外拖
                {
                    innerLeft = position1;
                    outterLeft = mouseBeginZoomed;
                    outterRight = position2;
                    innerRight = mouseDragZoomed;
                }
                else    //鼠标向内拖
                {
                    innerLeft = mouseBeginZoomed;
                    outterLeft = position1;
                    outterRight = mouseDragZoomed;
                    innerRight = position2;
                }
            }
            else   //扇形在鼠标拖动一侧开始顺时针扫过
            {
                if (mouseBeginDis < mouseEndDis) //鼠标向外拖
                {
                    innerLeft = mouseDragZoomed;
                    outterLeft = position2;
                    outterRight = mouseBeginZoomed;
                    innerRight = position1;
                }
                else    //鼠标向内拖
                {
                    innerLeft = position2;
                    outterLeft = mouseDragZoomed;
                    outterRight = position1;
                    innerRight = mouseBeginZoomed;
                }
            }

            GeometrySink gs = waveGate.Open();
            gs.BeginFigure(innerLeft, FigureBegin.Filled);
            gs.AddLine(outterLeft);

            float disMax = Math.Max(mouseBeginDis, mouseEndDis);
            float disMin = Math.Min(mouseBeginDis, mouseEndDis);

            Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF size = new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF(disMax, disMax);
            ArcSegment arc = new ArcSegment(outterRight, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            gs.AddLine(innerRight);
            size = new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF(disMin, disMin);
            arc = new ArcSegment(innerLeft, size, 0, SweepDirection.Counterclockwise, ArcSize.Small);
            gs.AddArc(arc);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();


            return waveGate;
        }

        public override Point2F CalIntersectionPoint(float angle)
        {
            double x = CoordinateArea.Width * Math.Sin(AngleToRadian(angle)) / 2 + OriginalPoint.X;
            double y = OriginalPoint.Y - CoordinateArea.Width * Math.Cos(AngleToRadian(angle)) / 2;

            return new Point2F((float)x, (float)y);
        }

        public Point2F CalIntersectionPoint(float angle, float radius)
        {
            double x = radius * Math.Sin(AngleToRadian(angle)) + OriginalPoint.X;
            double y = OriginalPoint.Y - radius * Math.Cos(AngleToRadian(angle));

            return new Point2F((float)x, (float)y);
        }
    }
}
