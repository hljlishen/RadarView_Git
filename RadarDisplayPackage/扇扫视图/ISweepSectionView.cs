using System;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
namespace RadarDisplayPackage
{
    internal abstract class ISweepSectionView : IDisposable
    {
        protected GraphicTrackDisplayer _displayer;
        protected Point2F _beginLinePoint;
        protected Point2F _endLinePoint;
        protected AngleArea _sweepAngleArea;
        //protected static object _locker = new object();

        public ISweepSectionView(AngleArea sweepSection, GraphicTrackDisplayer displayer)
        {
            _sweepAngleArea = sweepSection;
            _displayer = displayer;

            _beginLinePoint = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.BeginAngle);  //sweepSection==null problem
            //gs.AddLine(_beginLinePoint);

            _endLinePoint = displayer.coordinateSystem.CalIntersectionPoint(sweepSection.EndAngle);
        }
        public virtual void Dispose()
        {
        }
        public abstract void Draw();
    }
}