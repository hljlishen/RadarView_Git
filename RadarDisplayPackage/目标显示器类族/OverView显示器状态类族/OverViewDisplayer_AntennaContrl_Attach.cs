using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Windows.Forms;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    internal class OverViewDisplayerAntennaContrlAttach : OverViewDisplayerAntennaControlFixed60
    {
        private readonly int _sectiorCount;
        private float _sectorCoverage;
        private readonly float[] _sectorCenter;
        private const int SectorBorderWidth = 3;
        private readonly Brush _sectorBorderBrush;
        private float _centerSectorBorderAngle1;
        private float _centerSectorBorderAngle2;
        private Point2F _centerSectorBorderIntersectorPoint1;
        private Point2F _centerSectorBorderIntersectorPoint2;
        public OverViewDisplayerAntennaContrlAttach(OverViewDisplayer displayer) : base(displayer)
        {
            _sectiorCount = GetSectorCount();
            _sectorCenter = CalSectorCenter();
            FixedSweepAngle = _sectorCoverage * 5;
            _sectorBorderBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1.0f, 0.0f, 0.0f));
            //_sectorBorderBrush.Opacity = 0.7f;
        }

        private static int GetSectorCount()
        {
            ITargetDataProvider provider = TargetManagerFactory.CreateTargetDataProvider();
            return provider.GetSectorCount();
        }

        private float[] CalSectorCenter()
        {
            _sectorCoverage = 360.0f / _sectiorCount;

            float[] ret = new float[_sectiorCount];
            for (int i = 0; i < _sectiorCount; i++)
            {
                ret[i] = i * _sectorCoverage + _sectorCoverage / 2;
            }

            return ret;
        }

        protected override float CalDragAngle(MouseEventArgs e)
        {
            float centerAngle = base.CalDragAngle(e);
            return FindNearestValueInSortedArray(centerAngle, _sectorCenter);
        }

        public static float FindNearestValueInSortedArray(float value, float[] sortedArray)
        {
            if (value <= sortedArray[0])
                return sortedArray[0];
            if (value >= sortedArray[sortedArray.Length - 1])
                return sortedArray[sortedArray.Length - 1];

            float ret = 0.0f;
            for (int index = 1; index < sortedArray.Length; index++)
            {
                if (value >= sortedArray[index - 1] && value <= sortedArray[index])
                {
                    float diff1 = value - sortedArray[index - 1];
                    float diff2 = sortedArray[index] - value;
                    ret = diff1 <= diff2 ? sortedArray[index - 1] : sortedArray[index];
                }
                else
                {
                    continue;
                }
            }

            return ret;
        }

        protected override void DrawSectionSweepGeometry()
        {
            (_centerSectorBorderAngle1, _centerSectorBorderAngle2) = CalCenterSectorBorderAngles();

            (_centerSectorBorderIntersectorPoint1, _centerSectorBorderIntersectorPoint2) = CalCenterSectorBorderIntersectorPoints();

            DrawCenterSectorBorder();

            base.DrawSectionSweepGeometry();
        }

        private (float, float) CalCenterSectorBorderAngles()
        {
            float sectorAngle2 = BeginAngle + FixedSweepAngle * 2 / 5;
            float sectorAngle3 = BeginAngle + FixedSweepAngle * 3 / 5;

            return (sectorAngle2, sectorAngle3);
        }

        private (Point2F, Point2F) CalCenterSectorBorderIntersectorPoints()
        {
            Point2F sectorAngle2Point = displayer.coordinateSystem.CalIntersectionPoint(Tools.StandardAngle(_centerSectorBorderAngle1));
            Point2F sectorAngle3Point = displayer.coordinateSystem.CalIntersectionPoint(Tools.StandardAngle(_centerSectorBorderAngle2));
            return (sectorAngle2Point, sectorAngle3Point);
        }

        private void DrawCenterSectorBorder()
        {
            displayer.Canvas.DrawLine(displayer.coordinateSystem.OriginalPoint, _centerSectorBorderIntersectorPoint1, _sectorBorderBrush, SectorBorderWidth);
            displayer.Canvas.DrawLine(displayer.coordinateSystem.OriginalPoint, _centerSectorBorderIntersectorPoint2, _sectorBorderBrush, SectorBorderWidth);
        }
    }
}
