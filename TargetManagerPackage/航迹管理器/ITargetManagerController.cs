using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    public enum TargetManagerMode
    {
        Auto,
        SemiAuto,
        Manual,
        Intelligent
    }
    public interface ITargetManagerController
    {
        void SwitchMode(TargetManagerMode mode);

        void SelectTarget(Target t);

        void DeleteActiveTarget();

        void DeleteOutRangedTargets(AngleArea area);

        void ClearRawData();
    }
}
