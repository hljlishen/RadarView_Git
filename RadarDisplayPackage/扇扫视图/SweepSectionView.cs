using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using SweepDirection = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SweepDirection;

namespace RadarDisplayPackage
{
    class SweepSectionView : IDisposable
    {
        AngleArea sweepSection;
        PathGeometry sweepSectionGraphic;
        OverViewDisplayer displayer;
        Brush antennaRangeAreaBrush;

        public SweepSectionView(AngleArea sweepSection, OverViewDisplayer displayer)
        {
            this.sweepSection = sweepSection;
            this.displayer = displayer;

            antennaRangeAreaBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            antennaRangeAreaBrush.Opacity = 0.25f;

            sweepSectionGraphic = displayer.Factory.CreatePathGeometry();
            GeometrySink gs = sweepSectionGraphic.Open();
            gs.BeginFigure(displayer.coordinateSystem.OriginalPoint, FigureBegin.Filled);

            Point2F p1 = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.BeginAngle);
            gs.AddLine(p1);

            Point2F p2 = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.EndAngle);
            //扇形的X轴Y轴半径是矩形框width的一半
            SizeF size = new SizeF(displayer.coordinateSystem.CoordinateArea.Width / 2, displayer.coordinateSystem.CoordinateArea.Height / 2);

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
            displayer.Canvas.DrawGeometry(sweepSectionGraphic, antennaRangeAreaBrush,3);
            displayer.Canvas.FillGeometry(sweepSectionGraphic, antennaRangeAreaBrush);
        }

        public void Dispose()
        {
            antennaRangeAreaBrush?.Dispose();
            sweepSectionGraphic?.Dispose();
        }
    }
}
