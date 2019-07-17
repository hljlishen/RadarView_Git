using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using TargetManagerPackage;
using Utilities;
using SweepDirection = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SweepDirection;

namespace RadarDisplayPackage
{
    internal class OverSweepSectionView : ISweepSectionView
    {
        //private readonly PathGeometry _sweepSectionGraphic;
        private readonly Brush _centerSectorBorder;
        //private static object _locker = new object();

        public OverSweepSectionView(AngleArea sweepSection, GraphicTrackDisplayer displayer) : base(sweepSection, displayer)
        {
            //_sweepSectionGraphic = displayer.Factory.CreatePathGeometry();
            //GeometrySink gs = _sweepSectionGraphic.Open();
            //gs.BeginFigure(displayer.coordinateSystem.OriginalPoint, FigureBegin.Filled);
            //gs.AddLine(_beginLinePoint);
            ////扇形的X轴Y轴半径是矩形框width的一半
            //SizeF size = new SizeF((float)displayer.coordinateSystem.CoordinateArea.Width / 2, (float)displayer.coordinateSystem.CoordinateArea.Height / 2);

            ////添加弧线
            //ArcSegment arc = new ArcSegment(_endLinePoint, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            //gs.AddArc(arc);

            ////添加第二条线
            //gs.AddLine(displayer.coordinateSystem.OriginalPoint);
            //gs.EndFigure(FigureEnd.Closed);
            //gs.Close();

            _centerSectorBorder = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 1, 0));
        }

        private PathGeometry CalGeomitry()
        {
            PathGeometry sweepSectionGraphic = _displayer.Factory.CreatePathGeometry();
            GeometrySink gs = sweepSectionGraphic.Open();
            gs.BeginFigure(_displayer.coordinateSystem.OriginalPoint, FigureBegin.Filled);
            gs.AddLine(_beginLinePoint);
            //扇形的X轴Y轴半径是矩形框width的一半
            SizeF size = new SizeF((float)_displayer.coordinateSystem.CoordinateArea.Width / 2, (float)_displayer.coordinateSystem.CoordinateArea.Height / 2);

            //添加弧线
            ArcSegment arc = new ArcSegment(_endLinePoint, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            //添加第二条线
            gs.AddLine(_displayer.coordinateSystem.OriginalPoint);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();

            return sweepSectionGraphic;
        }

        public override void Draw() //problem1
        {

            //lock (_locker)
            {
                Brush _antennaRangeAreaBrush = _displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
                _antennaRangeAreaBrush.Opacity = 0.25f;
                Brush _antennaRangeBorlderBrush = _displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
                _antennaRangeBorlderBrush.Opacity = 1;
                PathGeometry section = CalGeomitry();
                _displayer.Canvas.DrawGeometry(section, _antennaRangeAreaBrush, 3);
                _displayer.Canvas.FillGeometry(section, _antennaRangeAreaBrush);
                _displayer.Canvas.DrawLine(_displayer.coordinateSystem.OriginalPoint, _beginLinePoint, _antennaRangeBorlderBrush, 3);
                _displayer.Canvas.DrawLine(_displayer.coordinateSystem.OriginalPoint, _endLinePoint, _antennaRangeBorlderBrush, 3);

                _antennaRangeAreaBrush.Dispose();
                _antennaRangeBorlderBrush.Dispose();
                section.Dispose();
                DrawCenterSector();     //画中央扇区
            }
        }

        private void DrawCenterSector()
        {
            float angle1 = Tools.StandardAngle(_sweepAngleArea.BeginAngle + 23);
            float angle2 = Tools.StandardAngle(_sweepAngleArea.EndAngle - 23);

            var centerSectorBegin = _displayer.coordinateSystem.CalIntersectionPoint(angle1);
            var centerSectorEnd = _displayer.coordinateSystem.CalIntersectionPoint(angle2);

            _centerSectorBorder.Opacity = 0.5f;
            _displayer.Canvas.DrawLine(_displayer.coordinateSystem.OriginalPoint, centerSectorBegin, _centerSectorBorder, 1);
            _displayer.Canvas.DrawLine(_displayer.coordinateSystem.OriginalPoint, centerSectorEnd, _centerSectorBorder, 1);
        }

        public override void Dispose()
        {
        }
    }
}
