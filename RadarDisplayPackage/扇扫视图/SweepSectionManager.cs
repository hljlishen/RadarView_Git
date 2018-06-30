﻿using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class SweepSectionManager : ISweepModeObserver
    {
        OverViewDisplayer displayer;
        ISweepModeSubject sweepModeSubject;
        SweepSectionView view;

        public SweepSectionManager(OverViewDisplayer displayer)
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
            //view = sweepModeSubject.IsSectionSweeping() ? new SweepSectionView(sweepModeSubject.GetSweepSection(), displayer) : null;

            if(sweepModeSubject.IsSectionSweeping())
                view = new SweepSectionView(sweepModeSubject.GetSweepSection(), displayer);
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
