using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using TargetManagerPackage;
using SweepDirection = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SweepDirection;

namespace RadarDisplayPackage
{
    internal class SweepSectionView : IDisposable
    {
        private readonly PathGeometry _sweepSectionGraphic;
        private readonly OverViewDisplayer _displayer;
        private readonly Brush _antennaRangeAreaBrush;
        private readonly Brush _antennaRangeBorlderBrush;
        private Point2F _beginLinePoint;
        private Point2F _endLinePoint;

        public SweepSectionView(AngleArea sweepSection, OverViewDisplayer displayer)
        {
            _displayer = displayer;

            _antennaRangeAreaBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeAreaBrush.Opacity = 0.25f;
            _antennaRangeBorlderBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeBorlderBrush.Opacity = 1;

            _sweepSectionGraphic = displayer.Factory.CreatePathGeometry();
            GeometrySink gs = _sweepSectionGraphic.Open();
            gs.BeginFigure(displayer.coordinateSystem.OriginalPoint, FigureBegin.Filled);

            _beginLinePoint = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.BeginAngle);
            gs.AddLine(_beginLinePoint);

            _endLinePoint = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.EndAngle);
            //扇形的X轴Y轴半径是矩形框width的一半
            SizeF size = new SizeF((float)displayer.coordinateSystem.CoordinateArea.Width / 2, (float)displayer.coordinateSystem.CoordinateArea.Height / 2);

            //添加弧线
            ArcSegment arc = new ArcSegment(_endLinePoint, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            //添加第二条线
            gs.AddLine(displayer.coordinateSystem.OriginalPoint);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
        }

        public void Draw()
        {
            _displayer.Canvas.DrawGeometry(_sweepSectionGraphic, _antennaRangeAreaBrush, 3);
            _displayer.Canvas.FillGeometry(_sweepSectionGraphic, _antennaRangeAreaBrush);
            //_antennaRangeAreaBrush.Opacity = 1;
            _displayer.Canvas.DrawLine(_displayer.coordinateSystem.OriginalPoint, _beginLinePoint, _antennaRangeBorlderBrush, 3);
            _displayer.Canvas.DrawLine(_displayer.coordinateSystem.OriginalPoint, _endLinePoint, _antennaRangeBorlderBrush, 3);
        }

        public void Dispose()
        {
            _antennaRangeAreaBrush?.Dispose();
            _sweepSectionGraphic?.Dispose();
        }
    }
}
