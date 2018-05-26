using System;
using System.Drawing;
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Utilities;

namespace RadarDisplayPackage
{
    public abstract class CoordinateSystem
    {
        protected double ZoomPercent = 0.9;    //矩形范围的缩小比例
        private Rect _coordinateArea;   //坐标系中用于绘制目标点和目标航迹的区域。在极坐标系中该区域即为最外层绿圈对应的矩形；直角坐标系中该区域即为两个坐标轴围起来的矩形
        protected D2DFactory Factory;     //用于计算pathGeometry


        protected CoordinateSystem(Rect drawArea, double range, D2DFactory factory)     //drawArea为的目标区域和坐标刻度区域的和
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            _coordinateArea = FindCoordinateArea(drawArea);         //绘制坐标系的范围
            OriginalRect = _coordinateArea;
            VisibleArea = drawArea;
            VisibleCenter = Tools.FindCenterPosition(VisibleArea);
            // ReSharper disable once VirtualMemberCallInConstructor
            OriginalPoint = FindOriginalPoint(_coordinateArea);     //原点位置
            Range = range;
            Factory = factory;
        }

        public Point2F OriginalPoint { get; set; }

        public Rect CoordinateArea
        {
            get => _coordinateArea;

            set
            {
                _coordinateArea = value;
                OriginalPoint = FindOriginalPoint(_coordinateArea);   //重新计算原点
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
            return Tools.ZoomRectangle(drawArea, ZoomPercent);
        }

        public virtual bool PointOutOfRange(Point p) => false;

        public abstract PolarCoordinate PointToCoordinate(Point2F p);

        public virtual PathGeometry BuildWaveGateGeometry(Point2F position1, Point2F position2)
        {
            return null;
        }

        public abstract Point2F CalIntersectionPoint(float angle);

        public bool IsPointInVisibleRect(Point2F p) => Tools.IsPointInRect(p, VisibleArea);
    }
}
