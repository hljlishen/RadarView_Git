namespace TargetManagerPackage
{
    class FreeDotDeleter : SectorProcessor
    {
        public void DeleteFreeDot(Sector s)
        {
            s.oldDots.Clear();
        }
    }
}
