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

        public SweepSectionView(AngleArea sweepSection, OverViewDisplayer displayer)
        {
            _displayer = displayer;

            _antennaRangeAreaBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeAreaBrush.Opacity = 0.25f;

            _sweepSectionGraphic = displayer.Factory.CreatePathGeometry();
            GeometrySink gs = _sweepSectionGraphic.Open();
            gs.BeginFigure(displayer.coordinateSystem.OriginalPoint, FigureBegin.Filled);

            Point2F p1 = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.BeginAngle);
            gs.AddLine(p1);

            Point2F p2 = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.EndAngle);
            //扇形的X轴Y轴半径是矩形框width的一半
            SizeF size = new SizeF((float)displayer.coordinateSystem.CoordinateArea.Width / 2, (float)displayer.coordinateSystem.CoordinateArea.Height / 2);

            //添加弧线
            ArcSegment arc = new ArcSegment(p2, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            //添加第二条线
            gs.AddLine(displayer.coordinateSystem.OriginalPoint);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
        }

        public void Draw()
        {
            _displayer.Canvas.DrawGeometry(_sweepSectionGraphic, _antennaRangeAreaBrush,3);
            _displayer.Canvas.FillGeometry(_sweepSectionGraphic, _antennaRangeAreaBrush);
        }

        public void Dispose()
        {
            _antennaRangeAreaBrush?.Dispose();
            _sweepSectionGraphic?.Dispose();
        }
    }
}
