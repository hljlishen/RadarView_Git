using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class SweepSectionManager : ISweepModeObserver
    {
        GraphicTrackDisplayer displayer;
        ISweepModeSubject sweepModeSubject;
        ISweepSectionView view;

        public SweepSectionManager(GraphicTrackDisplayer displayer)
        {
            this.displayer = displayer;
            sweepModeSubject = TargetManagerFactory.CreateSweepModeSubject();
            sweepModeSubject.RegisterSweepModeObserver(this);
        }

        public void NotifySweepModeChange(SweepMode mode)
        {
            if(mode == SweepMode.Normal)
            {
                view = null;
            }
            else
            {
                CalSweepSectionView();
            }
        }

        public void CalSweepSectionView()
        {
            view?.Dispose();
            if(sweepModeSubject.IsSectionSweeping())
            {
                view = displayer.CreateSweepSwctionView(sweepModeSubject.GetSweepSection());
            }  
            else
            {
                view = null;
            }
        }

        public void Draw()
        {
            view?.Draw();
        }
    }
}
