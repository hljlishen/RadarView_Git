using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using System.Drawing;
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    internal abstract class CoordinateSystem
    {
        protected double zoomPercent = 0.9;    //矩形范围的缩小比例
        private double range;                  //坐标系的最大量程，该变量在不同的坐标系下有不同的含义
        private Point2F originalPoint;        //坐标系原点在位图上的坐标
        private readonly Rect visibleArea;      //可以显示出来的区域范围,坐标系放大以后visiableArea只是coordinateArea的一部分
        readonly Point2F visibleCenter;  //可视区域中心点
        Rect coordniteArea;   //坐标系中用于绘制目标点和目标航迹的区域。在极坐标系中该区域即为最外层绿圈对应的矩形；直角坐标系中该区域即为两个坐标轴围起来的矩形
        protected D2DFactory factory;     //用于计算pathGeometry
        readonly Rect originalRect;      //视图初始化时的原始大小，复位视图时候用这个值
        public CoordinateSystem(Rect drawArea, double Range, D2DFactory factory)     //drawArea为的目标区域和坐标刻度区域的和
        {
            coordniteArea = FindCoordinateArea(drawArea);         //绘制坐标系的范围
            originalRect = coordniteArea;
            visibleArea = drawArea;
            visibleCenter = FindCenterPosition(visibleArea);
            originalPoint = FindOriginalPoint(coordniteArea);     //原点位置
            range = Range;
            this.factory = factory;
        }

        public Point2F OriginalPoint
        {
            get
            {
                return originalPoint;
            }

            set
            {
                originalPoint = value;
                
            }
        }

        public Rect CoordniteArea
        {
            get
            {
                return coordniteArea;
            }

            set
            {
                coordniteArea = value;
                originalPoint = FindOriginalPoint(coordniteArea);   //重新计算原点
            }
        }

        public double Range
        {
            get
            {
                return range;
            }

            set
            {
                range = value;
            }
        }

        public Rect OriginalRect
        {
            get
            {
                return originalRect;
            }
        }

        public Rect VisibleArea
        {
            get
            {
                return visibleArea;
            }
        }

        public Point2F VisibleCenter
        {
            get
            {
                return visibleCenter;
            }
        }

        public abstract Point2F CoordinateToPoint(PolarCoordinate coordinate);     //计算一个坐标在当前坐标系下的位置

        protected abstract Point2F FindOriginalPoint(Rect coordniteArea);   //计算坐标系原点位置

        protected virtual Rect FindCoordinateArea(Rect drawArea)  //计算绘制坐标系的矩形范围
        {
            return ZoomRectangle(drawArea, zoomPercent);
        }

        public static double AngleToRadian(double angle)
        {
            return System.Math.PI * angle / 180;
        }       //角度转弧度

        public static double RadianToAngle(double radian)
        {
            double a = 180 * radian;
            double b = a / (double)3.14159265358979323846264338;
            return  b;
        }       //弧度转角度

        public static Rect ZoomRectangle(Rect rect, double percent)     //以rect的中心点为中心，将rect放缩percent的百分比
        {
            Rect r = new Rect();
            Point2F location = new Point2F();
            double offsetX, offsetY;
            r.Width =(int)( rect.Width * percent);
            r.Height = (int)(rect.Height * percent);

            if (percent < 1)    //缩小
            {
                offsetX = (rect.Width - r.Width) / 2;
                offsetY = (rect.Height - r.Height) / 2;
                location.X = rect.Left + (int)offsetX;
                location.Y = rect.Top + (int)offsetY;
            }
            else   //放大
            {
                percent = percent - 1;
                offsetX = (r.Width - rect.Width) / 2;
                offsetY = (r.Height - rect.Height) / 2;
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
            Point2F p = new Point2F();
            p.X = rect.Left + rect.Width / 2;
            p.Y = rect.Top + rect.Height / 2;

            return p;
        }       //找矩形中心点

        public static float AngleToNorth(Point2F center, Point2F p)    //两点连线与垂直线的夹角
        {
            float distance = (float)DistanceBetween(center, p);
            float x = p.X - center.X;
            float angleR = (float)Math.Asin((float)(x / distance));
            double angle = RadianToAngle(angleR);

            float y = center.Y - p.Y;
            double angleR1 = Math.Acos(y / distance);
            angleR1 = RadianToAngle(angleR1);

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
            double a = Math.Pow((double)p1.X - p2.X, 2);
            double b = Math.Pow((double)p1.Y - p2.Y, 2);
            double c = Math.Sqrt(a + b);

            return c;
        }   //两点间的距离

        public static Rect MoveRect(Rect r, Point2F p) //将r横向移动p.x, 纵向移动p.y
        {
            Rect ret = r;
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

        public static Rectangle D2DRectToGDIRectangle(Rect r)
        {
            Rectangle ret = new Rectangle();

            //左上角位置
            Point p = new Point(r.Left, r.Top);
            ret.Location = p;

            ret.Width = r.Right - r.Left;
            ret.Height = r.Bottom - r.Top;

            return ret;
        }   

        public Point2F RadiusWiseZoomPosition(Point2F p, double r)
        {
            Point2F ret = new Point2F();

            //计算拖拽位置和坐标原点连线的正北夹角
            float angle = AngleToNorth(OriginalPoint, p);
            angle = (float)AngleToRadian(angle);

            //计算起始角度对应直线与坐标系外圈圆周的交点坐标
            ret.X = (int)(originalPoint.X + r * Math.Sin(angle));
            ret.Y = (int)(originalPoint.Y - r * Math.Cos(angle));

            return ret;
        }   //极坐标

        public virtual bool PointOutOfRange( Point p)
        {
            return false;
        }

        public abstract PolarCoordinate PointToCoordinate(Point2F p);

        public static float FindSmallArcBeginAngle(float a, float b)
        {
            float max = Math.Max(a, b);
            float min = Math.Min(a, b);

            if (max - min <= 180)
                return min;
            else
                return max;
        }

        public static float FindSmallArcEndAngle(float a, float b)
        {
            float max = Math.Max(a, b);
            float min = Math.Min(a, b);

            if (max - min <= 180)
                return max;
            else
                return min;
        }

        public virtual PathGeometry BuildWaveGateGeometry(Point2F position1, Point2F position2)
        {
            return null;
        }

        public abstract Point2F CalIntersectionPoint(float angle);

        public bool IsPointInVisibleRect( Point2F p)
        {
            return IsPointInRect(p, visibleArea);
        }

        public bool IsPointInRect(Point2F p, Rect r)
        {
            if (p.X >= r.Left && p.X <= r.Right && p.Y >= r.Top && p.Y <= r.Bottom)
                return true;
            else
                return false;
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
