using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    class CrossSectorTarget : ITestDataGenerator
    {
        public AzimuthCell ModifyOriginalData(AzimuthCell originalData)
        {
            originalData.DisCells.Clear();
            AngleArea testAngleArea = new AngleArea(0f, 33.75f);
            AngleArea testAngleArea1 = new AngleArea(20f, 25f);
            //if (testAngleArea.IsAngleInArea(realData.Angle) /*|| testAngleArea1.IsAngleInArea(realData.Angle)*/)
            if (!testAngleArea1.IsAngleInArea(originalData.Angle))
            {
                DistanceCell distanceCell = new DistanceCell() { index = 300, sumAM = 100000, az = originalData.Angle };
                originalData.DisCells.Add(300, distanceCell);
                return originalData;
            }

            return null;
        }
    }
}
