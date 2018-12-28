using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    class SideViewDisplayerAntenna : GraphicTrackDisplayerAntenna
    {
        protected float glowLengthPercentage = 0.2f;
        protected float offsetPercentage = 1f;

        public SideViewDisplayerAntenna(RenderTarget canvas, D2DFactory factory, CoordinateSystem coordinateSystem) : base(canvas, factory, coordinateSystem)
        {
            
        }

        protected override void DrawAntenna()
        {
            Point2F originalPoint = coordinateSystem.OriginalPoint;
            Rect coordinateArea = coordinateSystem.CoordinateArea;
            if (antennaAngle < 0)
                antennaAngle += 360;
            antennaAngle %= 360;
            float x = coordinateArea.Left + ((float)antennaAngle / 360) * coordinateArea.Width;
            float y = coordinateArea.Top;

            canvas.DrawLine(new Point2F((int)x, coordinateArea.Bottom), new Point2F((int)x, coordinateArea.Top)
                , antennaBrush, antennaWidth);
        }
    }
}
