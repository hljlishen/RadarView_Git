using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    internal class CoordinateSystemOfRetangular : CoordinateSystem
    {
        public CoordinateSystemOfRetangular(Rect drawArea, double range, D2DFactory factory) : base(drawArea, range, factory)
        {
            //zoomPercent = 0.8;
            Range = 5000;   //默认5公里
        }

        protected override Point2F FindOriginalPoint(Rect coordniteArea)
        {
            return new Point2F(coordniteArea.Left, coordniteArea.Bottom);   //直角坐标系的原点即为区域左下角
        }

        public override bool PointOutOfRange(Point p)
        {
            if (p.X >= CoordinateArea.Left && p.X <= CoordinateArea.Right && p.Y >= CoordinateArea.Top && p.Y <= CoordinateArea.Bottom)
                return false;
            else
                return true;
        }

        public override Point2F CoordinateToPoint(PolarCoordinate coordinate)
        {
            double r = (coordinate.Az / 360) * CoordinateArea.Width; //
            double height = coordinate.Dis * Math.Sin(Tools.DegreeToRadian(coordinate.El));

            //if (height > Range)     //是否超出范围
            //{
            //    p = new Point2F();
            //    return false;
            //}
            
            double h = height * CoordinateArea.Height / Range;

            float x1 = OriginalPoint.X + (int)r;
            float y1 = OriginalPoint.Y - (int)h;

            Point2F p = new Point2F(x1, y1);

            return p;
        }

        public override PolarCoordinate PointToCoordinate(Point2F p)
        {
            return new PolarCoordinate();
        }

        public override Point2F CalIntersectionPoint(float angle)
        {
            double x = CoordinateArea.Width * angle / 360 + OriginalPoint.X;
            double y = CoordinateArea.Bottom;

            return new Point2F((float)x, (float)y);
        }
    }
}
