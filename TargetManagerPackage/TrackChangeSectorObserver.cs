using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace TargetManagerPackage
{
    internal class TrackChangeSectorObserver
    {
        private TargetTrack subjectTrack;
        public TrackChangeSectorObserver()
        {
        }

        public void RegisterTrack(TargetTrack track)
        {
            UnregisterTrack(track);
            subjectTrack = track;
            ChangeSectorHandler(track, track.SectorIndex);
            track.ChangeSector += ChangeSectorHandler;
        }

        public void UnregisterTrack(TargetTrack track)
        {
            if (subjectTrack != null && subjectTrack == track)
                subjectTrack.ChangeSector -= ChangeSectorHandler;
            subjectTrack = null;
        }

        private void ChangeSectorHandler(Target target, int tosectorindex)
        {
            AntennaSectionSweepController sweepController = TargetManagerFactory.CreateAntennaManager();
            sweepController.SetSectionSweepMode(CalculateSweepArea(tosectorindex));
        }

        private AngleArea CalculateSweepArea(int sectorIndex)
        {
            float sectorCoverage = (float) 360 / TargetManagerFactory.CreateTargetDataProvider().GetSectorCount();
            float beginAngle = sectorIndex * sectorCoverage;
            float endAngle = (sectorIndex + 1) * sectorCoverage;
            beginAngle = Tools.StandardAngle(beginAngle - sectorCoverage * 2);
            endAngle = Tools.StandardAngle(endAngle + sectorCoverage * 2);

            return new AngleArea(beginAngle, endAngle);
        }
    }
}
