using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

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
                float begin = CoordinateSystem.FindSmallArcBeginAngle(beginAngle, dragAngle);
                float end = CoordinateSystem.FindSmallArcEndAngle(beginAngle, dragAngle);

                //判断上行扫过的方向，如果起始角度是鼠标点击的位置则扇形是顺时针扫过
                //如果起始角度是鼠标拖动位置，则扇形是逆时针扫过
                SweepDirection sd ;
                if (begin == beginAngle)
                {
                    sd = SweepDirection.Clockwise;
                }
                else
                {
                    sd = SweepDirection.Counterclockwise;
                }

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
            else
            {
                ////计算拖拽位置和坐标原点连线的正北夹角
                dragAngle = CoordinateSystem.AngleToNorth(displayer.coordinateSystem.OriginalPoint, CoordinateSystem.PointToPoint2F(e.Location));

                //计算拖动位置
                dragLinePoint = displayer.coordinateSystem.RadiusWiseZoomPosition(CoordinateSystem.PointToPoint2F(e.Location), displayer.coordinateSystem.CoordinateArea.Width / 2);
            }
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
                return;
            else
            {
                isMouseDown = true;
                mouseDownPosition = CoordinateSystem.PointToPoint2F(e.Location);

                ////计算点击位置和坐标原点连线的正北夹角
                beginAngle = CoordinateSystem.AngleToNorth(displayer.coordinateSystem.OriginalPoint, mouseDownPosition);
                dragAngle = beginAngle;

                //计算第一条线与圆周的夹角
                beginLinePoint = displayer.coordinateSystem.RadiusWiseZoomPosition(CoordinateSystem.PointToPoint2F( e.Location), displayer.coordinateSystem.CoordinateArea.Width / 2);
                dragLinePoint = beginLinePoint;
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            //antennaRangeAreaPen?.Dispose();
            antennaRangeAreaBrush?.Dispose();
        }

        public override OverViewState GetState()
        {
            return OverViewState.AntennaControl;
        }

        protected override Command CreateCommand()
        {
            if (Math.Abs(beginAngle - dragAngle) < sweepAngleMinimum)
                return new NullCommand();

            //控制天线扫描范围的开始和结束角度
            float begin = CoordinateSystem.FindSmallArcBeginAngle(beginAngle, dragAngle);
            float end = CoordinateSystem.FindSmallArcEndAngle(beginAngle, dragAngle);

            //创建天线控制命令
            Command cmd = new AntennaSetSectionSweepModeCommand(begin, end); //传入-1表示按原始扫描速度进行扇扫

            //复位变量
            beginLinePoint = displayer.coordinateSystem.OriginalPoint;
            dragLinePoint = displayer.coordinateSystem.OriginalPoint;
            beginAngle = 0f;
            dragAngle = 0f;

            return cmd;
        }
    }
}
