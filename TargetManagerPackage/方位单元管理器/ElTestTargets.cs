using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;
using Utilities;

namespace TargetManagerPackage.方位单元管理器
{
    class ElTestTargets :ITestDataGenerator
    {
        public AzimuthCell ModifyOriginalData(AzimuthCell originalData)
        {
            originalData.DisCells.Clear();

            DistanceCell cell1 = new DistanceCell
            {
                az = originalData.Angle,
                index = 100,
                el = 10,
                sumAM = 100000
            };

            //double height = Math.Sin(Tools.DegreeToRadian(cell1.el)) * cell1.Distance;

            DistanceCell cell2 = new DistanceCell
            {
                az = originalData.Angle,
                index = 200,
                el = 15,
                sumAM = 100000
            };

            originalData.DisCells.Add(100, cell1);
            originalData.DisCells.Add(200, cell2);
            return originalData;
        }
    }
}
