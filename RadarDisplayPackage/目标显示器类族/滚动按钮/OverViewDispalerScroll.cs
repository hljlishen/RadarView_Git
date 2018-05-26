using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using Utilities;

namespace RadarDisplayPackage
{
    abstract class OverViewDispalerScroll
    {
        protected OverViewDisplayer Displayer;
        protected int HorizontalStep;
        protected int VerticalStep;
        protected Rect DisplayArea;
        protected bool MouseInArea;
        protected Brush BorderBrush;
        protected Brush FillBrush;
        protected float BorderWidth;
        protected Brush TriangleBrush;
        private const int StepCoff = 100;
        protected const int ScrollerWidth = 40;
        protected bool IsMouseDown = false;
        protected const int TriangleMargin = 4;
        protected float _brushFadeStep = 0.05f;

        protected OverViewDispalerScroll(OverViewDisplayer displayer)
        {
            Displayer = displayer;
            HorizontalStep = displayer.coordinateSystem.CoordinateArea.Width / StepCoff;
            VerticalStep = displayer.coordinateSystem.CoordinateArea.Height / StepCoff;
            BorderBrush = displayer.Canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb(255, 255, 0));
            FillBrush = displayer.Canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb(255, 255, 0));
            FillBrush.Opacity = 0.3f;
            TriangleBrush = displayer.Canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb(255, 255, 255));
            TriangleBrush.Opacity = 0.5f;
            BorderWidth = 2;
            Displayer.DisplayControl.MouseMove += DisplayControl_MouseMove;
            Displayer.DisplayControl.MouseLeave += DisplayControl_MouseLeave;
            Displayer.DisplayControl.MouseDown += DisplayControl_MouseDown;
            Displayer.DisplayControl.MouseUp += DisplayControl_MouseUp;
        }

        private void DisplayControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e) => IsMouseDown = false;

        private void DisplayControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) => IsMouseDown = true;

        private void DisplayControl_MouseLeave(object sender, EventArgs e) => MouseInArea = false;

        private void DisplayControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) => MouseInArea = Tools.IsPointInRect(Tools.PointToPoint2F(e.Location), DisplayArea);

        public void Draw()
        {
            if (!MouseInArea) return;
            DrawScroller();
            if(IsMouseDown)
                Scroll();
        }

        protected virtual void DrawScroller()
        {
            Displayer.Canvas.DrawRectangle(new RectF(DisplayArea.Left, DisplayArea.Top, DisplayArea.Right, DisplayArea.Bottom), BorderBrush, BorderWidth);
            Displayer.Canvas.FillRectangle(new RectF(DisplayArea.Left, DisplayArea.Top, DisplayArea.Right, DisplayArea.Bottom), FillBrush);
            (Point2F p1, Point2F p2, Point2F p3) = CalTriangleVertexs();
            PathGeometry triangle = BuildTriangle(p1, p2, p3);
            DrawTriangle(triangle);
            //BorderBrush.Opacity -= _brushFadeStep;
            //FillBrush.Opacity -= _brushFadeStep;
            //TriangleBrush.Opacity -= _brushFadeStep;
        }

        protected abstract (Point2F, Point2F, Point2F) CalTriangleVertexs();

        protected PathGeometry BuildTriangle(Point2F p1, Point2F p2, Point2F p3)
        {
            PathGeometry triangle = Displayer.Factory.CreatePathGeometry();
            GeometrySink gs = triangle.Open();
            gs.BeginFigure(p1, FigureBegin.Filled);
            gs.AddLine(p2);
            gs.AddLine(p3);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();

            return triangle;
        }

        protected virtual void DrawTriangle(PathGeometry triangle) => Displayer.Canvas.FillGeometry(triangle, TriangleBrush);

        public abstract void Scroll();
    }
}
