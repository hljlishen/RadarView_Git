using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class RightScroller : OverViewDispalerScroll
    {
        public RightScroller(OverViewDisplayer displayer) : base(displayer)
        {
            DisplayArea = new Rect(displayer.DisplayControl.Width - ScrollerWidth, 0, displayer.DisplayControl.Width,  displayer.DisplayControl.Bottom);
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
