using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using TargetManagerPackage;
using Utilities;
using SweepDirection = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SweepDirection;

namespace RadarDisplayPackage
{
    internal class SweepSectionView : IDisposable
    {
        private readonly PathGeometry _sweepSectionGraphic;
        private readonly OverViewDisplayer _displayer;
        private readonly Brush _antennaRangeAreaBrush;
        private readonly Brush _antennaRangeBorlderBrush;
        private readonly Point2F _beginLinePoint;
        private readonly Point2F _endLinePoint;
        private readonly AngleArea _sweepAngleArea;
        private readonly Brush _centerSectorBorder;

        public SweepSectionView(AngleArea sweepSection, OverViewDisplayer displayer)
        {
            _sweepAngleArea = sweepSection;
            _displayer = displayer;

            _antennaRangeAreaBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeAreaBrush.Opacity = 0.25f;
            _antennaRangeBorlderBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeBorlderBrush.Opacity = 1;
            _centerSectorBorder = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 1, 0));

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

            DrawCenterSector();     //画中央扇区
        }

        private void DrawCenterSector()
        {
            float angle1 = Tools.StandardAngle(_sweepAngleArea.BeginAngle + 23);
            float angle2 = Tools.StandardAngle(_sweepAngleArea.EndAngle - 23);

            var centerSectorBegin = _displayer.coordinateSystem.CalIntersectionPoint(angle1);
            var centerSectorEnd = _displayer.coordinateSystem.CalIntersectionPoint(angle2);

            _displayer.Canvas.DrawLine(_displayer.coordinateSystem.OriginalPoint, centerSectorBegin, _centerSectorBorder, 3);
            _displayer.Canvas.DrawLine(_displayer.coordinateSystem.OriginalPoint, centerSectorEnd, _centerSectorBorder, 3);
        }

        public void Dispose()
        {
            _antennaRangeAreaBrush?.Dispose();
            _sweepSectionGraphic?.Dispose();
        }
    }
}
