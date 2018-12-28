namespace TargetManagerPackage.目标类
{
    class CirculateStrategy : FindTrackIdStrategy
    {
        private static int latestId = 0;
        public override int NextId()
        {
            for (int i = latestId + 1; i != latestId; i++)
            {
                if (i >= TrackMaximumCount)
                    i = 0;
                if (Id[i] != 1)
                {
                    latestId = i;
                    Id[i] = 1;
                    return i + 1;
                }
            }

            return 0;
        }
    }
}
