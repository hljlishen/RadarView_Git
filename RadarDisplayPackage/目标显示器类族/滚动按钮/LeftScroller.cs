using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class LeftScroller : OverViewDispalerScroll
    {
        public LeftScroller(OverViewDisplayer displayer) : base(displayer)
        {
            DisplayArea = new Rect(0, 0, ScrollerWidth, displayer.DisplayControl.Size.Height);
        }

        protected override (Point2F, Point2F, Point2F) CalTriangleVertexs()
        {
            Point2F p1 = new Point2F(TriangleMargin , DisplayArea.Height / 2);
            Point2F p2 = new Point2F(ScrollerWidth - TriangleMargin, DisplayArea.Height / 4);
            Point2F p3 = new Point2F(ScrollerWidth - TriangleMargin, DisplayArea.Height * 3 / 4);
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
