using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    internal class OverViewDisplayerAntennaControlFixed60 : OverViewDisplayerState
    {
        private readonly Brush _antennaRangeAreaBrush;
        protected Point2F BeginLinePoint;
        protected Point2F DragLinePoint;
        protected Point2F EndLinePoint;
        private PathGeometry _pie;
        protected float BeginAngle;       //鼠标点击位置与原点连线与正北夹角
        protected float DragAngle;        //鼠标拖动位置与原点连线与正北夹角
        protected float EndAngle;
        //float sweepAngleMinimum = 5; //扫描方位的最小值
        protected float FixedSweepAngle = 47f;

        public OverViewDisplayerAntennaControlFixed60(OverViewDisplayer displayer) : base(displayer)
        {
            BeginLinePoint = displayer.coordinateSystem.OriginalPoint;
            DragLinePoint = displayer.coordinateSystem.OriginalPoint;
            _antennaRangeAreaBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeAreaBrush.Opacity = 0.6f;
            Brush dragLineBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(0, 0, 1));
            dragLineBrush.Opacity = 0.7f;
            DragLinePoint = new Point2F();
        }
        public override void Draw()
        {
            if (!isMouseDown) return;

            var gs = CreateSectionSweepGeometry();

            //绘制区域
            DrawSectionSweepGeometry();

            //释放资源
            gs.Dispose();
            _pie.Dispose();
        }

        protected GeometrySink CreateSectionSweepGeometry()
        {

            //扇形的X轴Y轴半径是矩形框width的一半
            SizeF size = new SizeF(displayer.coordinateSystem.CoordinateArea.Width / 2, height: displayer.coordinateSystem.CoordinateArea.Height / 2);
            _pie = displayer.Factory.CreatePathGeometry();   //扇形区域

            //开始合成扇形
            GeometrySink gs = _pie.Open();
            gs.BeginFigure(displayer.coordinateSystem.OriginalPoint, FigureBegin.Filled);

            //添加第一条线
            gs.AddLine(BeginLinePoint);   //原始代码

            //添加弧线
            ArcSegment arc = new ArcSegment(EndLinePoint, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            //添加第二条线
            gs.AddLine(displayer.coordinateSystem.OriginalPoint);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();

            return gs;
        }

        protected virtual void DrawSectionSweepGeometry()
        {
            displayer.Canvas.FillGeometry(_pie, _antennaRangeAreaBrush);
            displayer.Canvas.DrawGeometry(_pie, _antennaRangeAreaBrush, 3);
            //displayer.Canvas.DrawLine(displayer.coordinateSystem.OriginalPoint, DragLinePoint, _dragLineBrush, 3);
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            //计算拖拽位置和坐标原点连线的正北夹角
            DragAngle = CalDragAngle(e);
            DragLinePoint = displayer.coordinateSystem.CalIntersectionPoint(DragAngle);

            shouldExecuteCmd = true;
            //计算开始角度及开始角度与圆周交点坐标
            BeginAngle = DragAngle - FixedSweepAngle / 2;
            BeginAngle = Tools.StandardAngle(BeginAngle);
            BeginLinePoint = displayer.coordinateSystem.CalIntersectionPoint(BeginAngle);

            //计算结束角度及开始角度与圆周交点坐标
            EndAngle = DragAngle + FixedSweepAngle / 2;
            EndAngle = Tools.StandardAngle(EndAngle);
            EndLinePoint = displayer.coordinateSystem.CalIntersectionPoint(EndAngle);
        }

        protected virtual float CalDragAngle(MouseEventArgs e)
        {
            return Tools.AngleToNorth(displayer.coordinateSystem.OriginalPoint, Tools.PointToPoint2F(e.Location));
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (!ShouldExecuteFunction(e)) return;
            shouldExecuteCmd = false;   //鼠标点击但未拖动，此时松开鼠标不执行命令
            isMouseDown = true;
            mouseDownPosition = Tools.PointToPoint2F(e.Location);

            //计算开始角度及开始角度与圆周交点坐标
            DragAngle = Tools.AngleToNorth(displayer.coordinateSystem.OriginalPoint, Tools.PointToPoint2F(e.Location));
            DragLinePoint = displayer.coordinateSystem.CalIntersectionPoint(DragAngle);

            //计算开始角度交点
            BeginAngle = DragAngle - FixedSweepAngle / 2;
            BeginAngle = Tools.StandardAngle(BeginAngle);
            BeginLinePoint = displayer.coordinateSystem.CalIntersectionPoint(BeginAngle);

            //计算结束角度及开始角度与圆周交点坐标
            EndAngle = DragAngle + FixedSweepAngle / 2;
            EndAngle = Tools.StandardAngle(EndAngle);
            EndLinePoint = displayer.coordinateSystem.CalIntersectionPoint(EndAngle);
        }

        public override void Dispose()
        {
            base.Dispose();
            _antennaRangeAreaBrush?.Dispose();
        }

        public override OverViewState GetState() => OverViewState.AntennaControl;

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            base.MouseUp(sender, e);
            if (Tools.FloatEquals(BeginAngle, EndAngle)) return;
            displayer.SendNewSweepSection(new AngleArea( BeginAngle, EndAngle));
            SetDefualtValue();
        }

        void SetDefualtValue()
        {
            BeginLinePoint = displayer.coordinateSystem.OriginalPoint;
            DragLinePoint = displayer.coordinateSystem.OriginalPoint;
            BeginAngle = 0f;
            DragAngle = 0f;
        }

    }
}
