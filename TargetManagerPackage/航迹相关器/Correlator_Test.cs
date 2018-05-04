namespace TargetManagerPackage
{
    class Corelator_Test : TrackCorelator
    {
        public override void Corelate(Sector center, Sector left, Sector right)
        {
            //foreach (TargetDot dot in center.newDots)
            //{
            //    NotifyAllObservers(dot, NotifyType.Delete);
            //}
            NotifyDeleteSectorDot(center);
            center.NewDots.Clear();
        }
    }
}
