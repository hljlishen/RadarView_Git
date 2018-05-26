using System.Drawing;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class DownScroller : OverViewDispalerScroll
    {
        public DownScroller(OverViewDisplayer displayer) : base(displayer)
        {
            Size s = displayer.DisplayControl.Size;
            DisplayArea = new Rect(0, s.Height - ScrollerWidth, s.Width, s.Height);
        }


        protected override (Point2F, Point2F, Point2F) CalTriangleVertexs()
        {
            Point2F p1 = new Point2F(DisplayArea.Width / 2 + DisplayArea.Left, DisplayArea.Bottom - TriangleMargin);
            Point2F p2 = new Point2F(DisplayArea.Left + DisplayArea.Width / 4, DisplayArea.Bottom + TriangleMargin - ScrollerWidth);
            Point2F p3 = new Point2F(DisplayArea.Left + DisplayArea.Width * 3 / 4, DisplayArea.Bottom + TriangleMargin - ScrollerWidth);
            return (p1, p2, p3);
        }

        public override void Scroll()
        {
            Rect r = new Rect
            {
                Top = Displayer.coordinateSystem.CoordinateArea.Top - VerticalStep,
                Right = Displayer.coordinateSystem.CoordinateArea.Right,
                Left = Displayer.coordinateSystem.CoordinateArea.Left,
                Bottom = Displayer.coordinateSystem.CoordinateArea.Bottom - VerticalStep
            };

            Displayer.SetCoordinateArea(r);
        }
    }
}
