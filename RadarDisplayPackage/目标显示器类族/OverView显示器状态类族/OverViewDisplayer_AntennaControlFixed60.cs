using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    internal class OverViewDisplayer_AntennaControlFixed60 : OverViewDisplayerState
    {
        private readonly Brush antennaRangeAreaBrush;
        private readonly Brush dragLineBrush;
        private Point2F beginLinePoint;
        private Point2F dragLinePoint;
        private Point2F endLinePoint;
        private PathGeometry Pie;
        private float beginAngle;       //鼠标点击位置与原点连线与正北夹角
        private float dragAngle;        //鼠标拖动位置与原点连线与正北夹角
        private float endAngle;
        //float sweepAngleMinimum = 5; //扫描方位的最小值
        private float FixedSweepAngle = 30f;

        public OverViewDisplayer_AntennaControlFixed60(OverViewDisplayer displayer) : base(displayer)
        {
            beginLinePoint = displayer.coordinateSystem.OriginalPoint;
            dragLinePoint = displayer.coordinateSystem.OriginalPoint;
            antennaRangeAreaBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            antennaRangeAreaBrush.Opacity = 0.6f;
            dragLineBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(0, 0, 1));
            dragLineBrush.Opacity = 0.7f;
            dragLinePoint = new Point2F();
        }
        public override void Draw()
        {
            SizeF size = new SizeF(displayer.coordinateSystem.CoordinateArea.Width / 2, displayer.coordinateSystem.CoordinateArea.Height / 2);
            if (!isMouseDown) return;
            Pie = displayer.Factory.CreatePathGeometry();   //扇形区域

            //开始合成扇形
            GeometrySink gs = Pie.Open();
            gs.BeginFigure(displayer.coordinateSystem.OriginalPoint, FigureBegin.Filled);

            //添加第一条线
            gs.AddLine(beginLinePoint);   //原始代码

            //扇形的X轴Y轴半径是矩形框width的一半

            //添加弧线
            ArcSegment arc = new ArcSegment(endLinePoint, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            //添加第二条线
            gs.AddLine(displayer.coordinateSystem.OriginalPoint);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();

            //绘制区域
            DrawSweepSection();
            displayer.Canvas.DrawLine(displayer.coordinateSystem.OriginalPoint, dragLinePoint, dragLineBrush, 3);

            //释放资源
            gs.Dispose();
            Pie.Dispose();
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            //计算拖拽位置和坐标原点连线的正北夹角
            dragAngle = Tools.AngleToNorth(displayer.coordinateSystem.OriginalPoint, Tools.PointToPoint2F(e.Location));
            dragLinePoint = displayer.coordinateSystem.CalIntersectionPoint(dragAngle);

            shouldExecuteCmd = true;
            //计算开始角度及开始角度与圆周交点坐标
            beginAngle = dragAngle - FixedSweepAngle / 2;
            beginAngle = Tools.StandardAngle(beginAngle);
            beginLinePoint = displayer.coordinateSystem.CalIntersectionPoint(beginAngle);

            //计算结束角度及开始角度与圆周交点坐标
            endAngle = dragAngle + FixedSweepAngle / 2;
            endAngle = Tools.StandardAngle(endAngle);
            endLinePoint = displayer.coordinateSystem.CalIntersectionPoint(endAngle);
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (isMouseDown || e.Button != MouseButtons.Left) return;
            shouldExecuteCmd = false;   //鼠标点击但未拖动，此时松开鼠标不执行命令
            isMouseDown = true;
            mouseDownPosition = Tools.PointToPoint2F(e.Location);

            //计算开始角度及开始角度与圆周交点坐标
            dragAngle = Tools.AngleToNorth(displayer.coordinateSystem.OriginalPoint, Tools.PointToPoint2F(e.Location));
            dragLinePoint = displayer.coordinateSystem.CalIntersectionPoint(dragAngle);

            //计算开始角度交点
            beginAngle = dragAngle - FixedSweepAngle / 2;
            beginAngle = Tools.StandardAngle(beginAngle);
            beginLinePoint = displayer.coordinateSystem.CalIntersectionPoint(beginAngle);

            //计算结束角度及开始角度与圆周交点坐标
            endAngle = dragAngle + FixedSweepAngle / 2;
            endAngle = Tools.StandardAngle(endAngle);
            endLinePoint = displayer.coordinateSystem.CalIntersectionPoint(endAngle);

            //dragAngle += 30;
        }

        public override void Dispose()
        {
            base.Dispose();
            antennaRangeAreaBrush?.Dispose();
        }

        public override OverViewState GetState() => OverViewState.AntennaControl;

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);
            displayer.SendNewSweepSection(new AngleArea( beginAngle, endAngle));
            SetDefualtValue();
        }

        void SetDefualtValue()
        {
            beginLinePoint = displayer.coordinateSystem.OriginalPoint;
            dragLinePoint = displayer.coordinateSystem.OriginalPoint;
            beginAngle = 0f;
            dragAngle = 0f;
        }


        public void DrawSweepSection()
        {
            displayer.Canvas.FillGeometry(Pie, antennaRangeAreaBrush);
            displayer.Canvas.DrawGeometry(Pie, antennaRangeAreaBrush, 3);
        }
    }
}
