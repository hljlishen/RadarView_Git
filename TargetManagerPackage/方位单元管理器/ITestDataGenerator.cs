using CycleDataDrivePackage;
using System.Collections.Generic;

namespace TargetManagerPackage
{
    interface ITestDataGenerator
    {
        AzimuthCell ModifyOriginalData(AzimuthCell originalData);
    }
}
