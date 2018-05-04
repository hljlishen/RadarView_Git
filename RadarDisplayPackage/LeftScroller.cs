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
            DisplayArea = new Rect(0, 0, ScrollerWidth, displayer.DisplayControl.Bottom);
        }

        public override void Scroll()
        {
            Rect r = new Rect
            {
                Top = Displayer.coordinateSystem.CoordinateArea.Top,
                Right = Displayer.coordinateSystem.CoordinateArea.Right - HorizontalStep,
                Left = Displayer.coordinateSystem.CoordinateArea.Left - HorizontalStep,
                Bottom = Displayer.coordinateSystem.CoordinateArea.Bottom
            };

            Displayer.SetCoordinateArea(r);
        }
    }
}
