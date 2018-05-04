using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class RightScroller : OverViewDispalerScroll
    {
        public RightScroller(OverViewDisplayer displayer) : base(displayer)
        {
            DisplayArea = new Rect(displayer.DisplayControl.Size.Width - ScrollerWidth, 0, displayer.DisplayControl.Size.Width,  displayer.DisplayControl.Size.Height);
        }

        protected override (Point2F, Point2F, Point2F) CalTriangleVertexs()
        {
            Point2F p1 = new Point2F(DisplayArea.Right - TriangleMargin, DisplayArea.Height / 2);
            Point2F p2 = new Point2F(DisplayArea.Right - ScrollerWidth + TriangleMargin, DisplayArea.Height / 4);
            Point2F p3 = new Point2F(DisplayArea.Right - ScrollerWidth + TriangleMargin, DisplayArea.Height * 3 / 4);
            return (p1, p2, p3);
        }

        public override void Scroll()
        {
            Rect r = new Rect
            {
                Top = Displayer.coordinateSystem.CoordinateArea.Top,
                Right = Displayer.coordinateSystem.CoordinateArea.Right + HorizontalStep,
                Left = Displayer.coordinateSystem.CoordinateArea.Left + HorizontalStep,
                Bottom = Displayer.coordinateSystem.CoordinateArea.Bottom 
            };

            Displayer.SetCoordinateArea(r);
        }
    }
}
