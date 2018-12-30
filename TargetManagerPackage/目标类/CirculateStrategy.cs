namespace TargetManagerPackage.目标类
{
    public class CirculateStrategy : FindTrackIdStrategy
    {
        private static int latestId = 0;
        public override int NextId()
        {
            bool isFirstLoop = true;
            for (int i = latestId;; i++)
            {
                if(!isFirstLoop)
                {
                    if (i == latestId) break;
                }
                if (i >= TrackMaximumCount)
                    i = 0;
                if (Id[i] != 1)
                {
                    latestId = i+1;
                    Id[i] = 1;
                    return i + 1;
                }
                isFirstLoop = false;
            }

            return 0;
        }
    }
}
