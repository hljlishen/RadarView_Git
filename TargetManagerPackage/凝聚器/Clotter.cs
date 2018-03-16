using CycleDataDrivePackage;

namespace TargetManagerPackage
{
    internal abstract class Clotter : SectorProcessor
    {
        public abstract void Clot(Sector center, Sector right, Sector left, AzimuthCell[] cells);

        public static void MoveNewDotToOldDot(Sector s)
        {
            s.oldDots.Clear();

            foreach (TargetDot dot in s.newDots)
            {
                s.oldDots.Add(dot);
            }

            s.newDots.Clear();
        }
    }
}
