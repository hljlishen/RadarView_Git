using System;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    class TargetSimulator : ITestDataGenerator
    {
        public double Distance { get; set; } = 0;
        private DateTime _lastRefreshTime;
        public float TargetSpeed { get; set; }
        private AngleArea movementArea;

        public TargetSimulator(AngleArea targetMovementArea)
        {
            _lastRefreshTime = DateTime.Now;
            TargetSpeed = 10;
            movementArea = targetMovementArea;
        }

        public AzimuthCell ModifyOriginalData(AzimuthCell originalData)
        {
            //AngleArea testAngleArea1 = new AngleArea(20f, 30f);
            if (!movementArea.IsAngleInArea(originalData.Angle))
                return originalData;
            originalData.DisCells.Clear();
            TimeSpan interval = DateTime.Now - _lastRefreshTime;
            _lastRefreshTime = DateTime.Now;
            Distance += TargetSpeed * interval.TotalSeconds;
            //Console.WriteLine(Distance);
            int distanceCellIndex = (int)(Distance / 2.92f);
            DistanceCell distanceCell = new DistanceCell() { index = distanceCellIndex, sumAM = 100000, az = originalData.Angle };
            originalData.DisCells.Add(distanceCellIndex, distanceCell);
            return originalData;
        }
    }
}
