using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class SideViewSweepView : ISweepSectionView
    {
        //private static object _locker = new object();
        public SideViewSweepView(AngleArea sweepSection, GraphicTrackDisplayer displayer) : base(sweepSection, displayer)
        {
        }

        public override void Draw()
        {

            Point2F sectionBeginPointTop = new Point2F(_beginLinePoint.X, _displayer.coordinateSystem.CoordinateArea.Top);
            Point2F sectionEndPointTop = new Point2F(_endLinePoint.X, _displayer.coordinateSystem.CoordinateArea.Top);

            Brush _antennaRangeBorlderBrush = _displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 0, 0));
            _antennaRangeBorlderBrush.Opacity = 1;
            _displayer.Canvas.DrawLine(_beginLinePoint, sectionBeginPointTop, _antennaRangeBorlderBrush, 3);
            _displayer.Canvas.DrawLine(_endLinePoint, sectionEndPointTop, _antennaRangeBorlderBrush, 3);

            _antennaRangeBorlderBrush.Dispose();

        }

        public override void Dispose()
        {
        }
    }
}
