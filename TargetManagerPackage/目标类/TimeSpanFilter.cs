using System;

namespace TargetManagerPackage.目标类
{
    internal class TimeSpanFilter : ITargetTrackFilter
    {
        public bool Pass(TargetTrack t)
        {
            TimeSpan span = DateTime.Now - t.LastRefreshTime;

            if (span.TotalSeconds > 35)
                return false;
            return true;
        }
    }
}
