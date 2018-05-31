using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Utilities;

namespace RadarDisplayPackage
{
    class OverViewDisplayer_AntennaControl : OverViewDisplayerState
    {
        Brush antennaRangeAreaBrush;
        Point2F beginLinePoint;
        Point2F dragLinePoint;
        PathGeometry Pie;
        float beginAngle;       //鼠标点击位置与原点连线与正北夹角
        float dragAngle;        //鼠标拖动位置与原点连线与正北夹角
        float sweepAngleMinimum = 1; //扫描方位的最小值

        public OverViewDisplayer_AntennaControl(OverViewDisplayer displayer) : base(displayer)
        {
            beginLinePoint = displayer.coordinateSystem.OriginalPoint;
            dragLinePoint = displayer.coordinateSystem.OriginalPoint;
            antennaRangeAreaBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(255, 0, 0));
            antennaRangeAreaBrush.Opacity = 0.5f;
            dragLinePoint = new Point2F();
        }
        public override void Draw()
        {
            if (isMouseDown && Math.Abs(beginAngle - dragAngle) > sweepAngleMinimum)
            {
                Pie = displayer.Factory.CreatePathGeometry();   //扇形区域

                //开始合成扇形
                GeometrySink gs = Pie.Open();
                gs.BeginFigure(displayer.coordinateSystem.OriginalPoint, FigureBegin.Filled);

                //添加第一条线
                //float angle1 = CoordinateSystem.AngleToNorth(displayer.coordinateSystem.OriginalPoint, beginLinePoint);
                //angle1 -= 30;
                //if (angle1 < 0)
                //    angle1 += 360;
                //Point2F p = displayer.coordinateSystem.CalIntersectionPoint(angle1);
                //gs.AddLine(p);
                gs.AddLine(beginLinePoint);   //原始代码

                //判断起始角度和结束角度
                float begin = Tools.FindSmallArcBeginAngle(beginAngle, dragAngle);
                float end = Tools.FindSmallArcEndAngle(beginAngle, dragAngle);

                //判断上行扫过的方向，如果起始角度是鼠标点击的位置则扇形是顺时针扫过
                //如果起始角度是鼠标拖动位置，则扇形是逆时针扫过
                SweepDirection sd ;
                sd = Tools.FloatEquals(begin, beginAngle) ? SweepDirection.Clockwise : SweepDirection.Counterclockwise;

                //扇形的X轴Y轴半径是矩形框width的一半
                SizeF size = new SizeF(displayer.coordinateSystem.CoordinateArea.Width / 2, displayer.coordinateSystem.CoordinateArea.Height / 2);

                //添加弧线
                ArcSegment arc = new ArcSegment(dragLinePoint, size, 0, sd, ArcSize.Small);
                gs.AddArc(arc);

                //添加第二条线
                gs.AddLine(displayer.coordinateSystem.OriginalPoint);
                gs.EndFigure(FigureEnd.Closed);
                gs.Close();

                //绘制区域
                displayer.Canvas.FillGeometry(Pie, antennaRangeAreaBrush);
                displayer.Canvas.DrawGeometry(Pie, antennaRangeAreaBrush, 3);

                //释放资源
                gs.Dispose();
                Pie.Dispose();
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown)
                return;
            ////计算拖拽位置和坐标原点连线的正北夹角
            dragAngle = Tools.AngleToNorth(displayer.coordinateSystem.OriginalPoint, Tools.PointToPoint2F(e.Location));

            //计算拖动位置
            dragLinePoint = displayer.CoordinateSystem.RadiusWiseZoomPosition(Tools.PointToPoint2F(e.Location), displayer.coordinateSystem.CoordinateArea.Width / 2);
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (isMouseDown || e.Button != MouseButtons.Left)
                return;
            isMouseDown = true;
            mouseDownPosition = Tools.PointToPoint2F(e.Location);

            ////计算点击位置和坐标原点连线的正北夹角
            beginAngle = Tools.AngleToNorth(displayer.coordinateSystem.OriginalPoint, mouseDownPosition);
            dragAngle = beginAngle;

            //计算第一条线与圆周的夹角
            beginLinePoint = displayer.CoordinateSystem.RadiusWiseZoomPosition(Tools.PointToPoint2F( e.Location), displayer.coordinateSystem.CoordinateArea.Width / 2);
            dragLinePoint = beginLinePoint;
        }

        public override void Dispose()
        {
            base.Dispose();
            antennaRangeAreaBrush?.Dispose();
        }

        public override OverViewState GetState() => OverViewState.AntennaControl;
    }
}
