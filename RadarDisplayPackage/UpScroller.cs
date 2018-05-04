
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class UpScroller : OverViewDispalerScroll
    {
        public UpScroller(OverViewDisplayer displayer) : base(displayer)
        {
            DisplayArea = new Rect(0, 0, displayer.DisplayControl.Width, ScrollerWidth);
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
