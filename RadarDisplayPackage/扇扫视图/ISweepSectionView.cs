using System;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using Utilities;
using SweepDirection = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SweepDirection;
namespace RadarDisplayPackage
{
    internal abstract class ISweepSectionView : IDisposable
    {
        protected GraphicTrackDisplayer _displayer;
        protected Brush _antennaRangeAreaBrush;
        protected Brush _antennaRangeBorlderBrush;
        protected Point2F _beginLinePoint;
        protected Point2F _endLinePoint;
        protected AngleArea _sweepAngleArea;

        public ISweepSectionView(AngleArea sweepSection, GraphicTrackDisplayer displayer)
        {
            _sweepAngleArea = sweepSection;
            _displayer = displayer;

            _antennaRangeAreaBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeAreaBrush.Opacity = 0.25f;
            _antennaRangeBorlderBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeBorlderBrush.Opacity = 1;

            _beginLinePoint = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.BeginAngle);
            //gs.AddLine(_beginLinePoint);

            _endLinePoint = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.EndAngle);
        }
        public abstract void Dispose();
        public abstract void Draw();
    }
}