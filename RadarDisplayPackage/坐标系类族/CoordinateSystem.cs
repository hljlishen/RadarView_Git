using System;
using System.Drawing;
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    internal abstract class CoordinateSystem
    {
        protected double ZoomPercent = 0.9;    //矩形范围的缩小比例
        private Rect _coordniteArea;   //坐标系中用于绘制目标点和目标航迹的区域。在极坐标系中该区域即为最外层绿圈对应的矩形；直角坐标系中该区域即为两个坐标轴围起来的矩形
        protected D2DFactory Factory;     //用于计算pathGeometry

        protected CoordinateSystem(Rect drawArea, double range, D2DFactory factory)     //drawArea为的目标区域和坐标刻度区域的和
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            _coordniteArea = FindCoordinateArea(drawArea);         //绘制坐标系的范围
            OriginalRect = _coordniteArea;
            VisibleArea = drawArea;
            VisibleCenter = FindCenterPosition(VisibleArea);
            // ReSharper disable once VirtualMemberCallInConstructor
            OriginalPoint = FindOriginalPoint(_coordniteArea);     //原点位置
            Range = range;
            Factory = factory;
        }

        public Point2F OriginalPoint { get; set; }

        public Rect CoordniteArea
        {
            get => _coordniteArea;

            set
            {
                _coordniteArea = value;
                OriginalPoint = FindOriginalPoint(_coordniteArea);   //重新计算原点
            }
        }

        public double Range { get; set; }

        public Rect OriginalRect { get; }

        public Rect VisibleArea { get; }

        public Point2F VisibleCenter { get; }

        public abstract Point2F CoordinateToPoint(PolarCoordinate coordinate);     //计算一个坐标在当前坐标系下的位置

        protected abstract Point2F FindOriginalPoint(Rect coordniteArea);   //计算坐标系原点位置

        protected virtual Rect FindCoordinateArea(Rect drawArea)  //计算绘制坐标系的矩形范围
        {
            return ZoomRectangle(drawArea, ZoomPercent);
        }

        public static double AngleToRadian(double angle)
        {
            return Math.PI * angle / 180;
        }       //角度转弧度

        public static double RadianToAngle(double radian)
        {
            var a = 180 * radian;
            var b = a / 3.14159265358979323846264338;
            return  b;
        }       //弧度转角度

        public static Rect ZoomRectangle(Rect rect, double percent)     //以rect的中心点为中心，将rect放缩percent的百分比
        {
            var r = new Rect();
            var location = new Point2F();
            double offsetX, offsetY;
            r.Width =(int)( rect.Width * percent);
            r.Height = (int)(rect.Height * percent);

            if (percent < 1)    //缩小
            {
                offsetX = (double)(rect.Width - r.Width) / 2;
                offsetY = (double)(rect.Height - r.Height) / 2;
                location.X = rect.Left + (int)offsetX;
                location.Y = rect.Top + (int)offsetY;
            }
            else   //放大
            {
                offsetX = (double)(r.Width - rect.Width) / 2;
                offsetY = (double)(r.Height - rect.Height) / 2;
                location.X = rect.Left - (int)offsetX;
                location.Y = rect.Top - (int)offsetY;
            }

            r.Left = (int)location.X + rect.Left;
            r.Top = (int)location.Y + rect.Top;
            r.Right += (int)(location.X);
            r.Bottom += (int)location.Y;
            return r;
        }

        public static Point2F FindCenterPosition(Rect rect)
        {
            if (rect == null)
                throw (new Exception("rectangle对象为空"));
            var p = new Point2F
            {
                X = rect.Left + rect.Width / 2,
                Y = rect.Top + rect.Height / 2
            };

            return p;
        }       //找矩形中心点

        public static float AngleToNorth(Point2F center, Point2F p)    //两点连线与垂直线的夹角
        {
            var distance = (float)DistanceBetween(center, p);
            var x = p.X - center.X;
            var angleR = (float)Math.Asin(x / distance);
            var angle = RadianToAngle(angleR);

            //float y = center.Y - p.Y;
            //double angleR1 = Math.Acos(y / distance);
            //angleR1 = RadianToAngle(angleR1);

            if (p.X >= center.X && p.Y <= center.Y)   //第一象限
            {

            }
            else if (p.X >= center.X && p.Y > center.Y) //第二象限
            {
                angle = 180 - angle;
            }
            else if (p.X < center.X && p.Y > center.Y) //第三象限
            {
                angle = 180 + Math.Abs(angle);
            }
            else//第四象限
            {
                angle = 360 + angle;
            }

            return (float)angle;
        }

        public static double DistanceBetween(Point2F p1, Point2F p2)
        {
            var a = Math.Pow((double)p1.X - p2.X, 2);
            var b = Math.Pow((double)p1.Y - p2.Y, 2);
            var c = Math.Sqrt(a + b);

            return c;
        }   //两点间的距离

        public static Rect MoveRect(Rect r, Point2F p) //将r横向移动p.x, 纵向移动p.y
        {
            var ret = r;
            ret.Left += (int)p.X;
            ret.Right += (int)p.X;
            ret.Top += (int)p.Y;
            ret.Bottom += (int)p.Y;

            return ret;
        }

        public static Point2F PointToPoint2F(Point p)
        {
            return new Point2F(p.X, p.Y);
        }

        public static Point Point2FToPoint(Point2F p)
        {
            return new Point((int)p.X, (int)p.Y);
        }

        public static Rectangle D2DRectToGdiRectangle(Rect r)
        {
            var ret = new Rectangle();

            //左上角位置
            var p = new Point(r.Left, r.Top);
            ret.Location = p;

            ret.Width = r.Right - r.Left;
            ret.Height = r.Bottom - r.Top;

            return ret;
        }   

        public Point2F RadiusWiseZoomPosition(Point2F p, double r)
        {
            var ret = new Point2F();

            //计算拖拽位置和坐标原点连线的正北夹角
            var angle = AngleToNorth(OriginalPoint, p);
            angle = (float)AngleToRadian(angle);

            //计算起始角度对应直线与坐标系外圈圆周的交点坐标
            ret.X = (int)(OriginalPoint.X + r * Math.Sin(angle));
            ret.Y = (int)(OriginalPoint.Y - r * Math.Cos(angle));

            return ret;
        }   //极坐标

        public virtual bool PointOutOfRange( Point p)
        {
            return false;
        }

        public abstract PolarCoordinate PointToCoordinate(Point2F p);

        public static float FindSmallArcBeginAngle(float a, float b)
        {
            var max = Math.Max(a, b);
            var min = Math.Min(a, b);

            return max - min <= 180 ? min : max;
        }

        public static float FindSmallArcEndAngle(float a, float b)
        {
            var max = Math.Max(a, b);
            var min = Math.Min(a, b);

            return max - min <= 180 ? max : min;
        }

        public virtual PathGeometry BuildWaveGateGeometry(Point2F position1, Point2F position2)
        {
            return null;
        }

        public abstract Point2F CalIntersectionPoint(float angle);

        public bool IsPointInVisibleRect( Point2F p)
        {
            return IsPointInRect(p, VisibleArea);
        }

        public bool IsPointInRect(Point2F p, Rect r)
        {
            return p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom;
        }

        public static float StandardAngle(float angle) //将角度转化为0-360的浮点数
        {
            if (angle < 0)
                angle += 360;
            angle %= 360;

            return angle;
        }
    }
}
