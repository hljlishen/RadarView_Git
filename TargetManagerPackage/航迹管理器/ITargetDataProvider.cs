using System.Collections.Generic;

namespace TargetManagerPackage
{
    public interface ITargetDataProvider : ITargetSubject
    {
        List<Target> GetTargetTracks();     
        List<Target> GetTargetDots();//获取当前圈新产生的目标点

        int GetSectorCount();
        TargetManagerMode GetMode();
    }
}
