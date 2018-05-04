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

        public override void Scroll()
        {
            Rect r = new Rect
            {
                Top = Displayer.coordinateSystem.CoordinateArea.Top + VerticalStep,
                Right = Displayer.coordinateSystem.CoordinateArea.Right,
                Left = Displayer.coordinateSystem.CoordinateArea.Left,
                Bottom = Displayer.coordinateSystem.CoordinateArea.Bottom + VerticalStep
            };

            Displayer.SetCoordinateArea(r);
        }
    }
}
