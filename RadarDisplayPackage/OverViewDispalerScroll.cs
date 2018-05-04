using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;

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
        private const int StepCoff = 100;
        protected const int ScrollerWidth = 30;
        protected bool IsMouseDown = false;

        protected OverViewDispalerScroll(OverViewDisplayer displayer)
        {
            Displayer = displayer;
            HorizontalStep = displayer.coordinateSystem.CoordinateArea.Width / StepCoff;
            VerticalStep = displayer.coordinateSystem.CoordinateArea.Height / StepCoff;
            BorderBrush = displayer.Canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(255, 255, 0));
            FillBrush = displayer.Canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(255, 255, 0));
            FillBrush.Opacity = 0.3f;
            BorderWidth = 2;
            Displayer.DisplayControl.MouseMove += DisplayControl_MouseMove;
            Displayer.DisplayControl.MouseLeave += DisplayControl_MouseLeave;
            Displayer.DisplayControl.MouseDown += DisplayControl_MouseDown;
            Displayer.DisplayControl.MouseUp += DisplayControl_MouseUp;
        }

        private void DisplayControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            IsMouseDown = false;
        }

        private void DisplayControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            IsMouseDown = true;
        }

        private void DisplayControl_MouseLeave(object sender, EventArgs e) => MouseInArea = false;

        private void DisplayControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) => MouseInArea = CoordinateSystem.IsPointInRect(CoordinateSystem.PointToPoint2F(e.Location), DisplayArea);

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
        }

        public abstract void Scroll();
    }
}
