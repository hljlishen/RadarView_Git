using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class ScoreBiggerThanDecorator : ITrackSender
    {
        private TrackSender sender;

        public ScoreBiggerThanDecorator(TrackSender sender)
        {
            this.sender = sender;
        }
        public void DestoryTrack(TargetTrack track)
        {
            sender.DestoryTrack(track);
        }

        public void NewTrack(TargetTrack track)
        {
        }

        public void UpdateTrack(TargetTrack track)
        {
            if (track.Score >= 6)
                sender.UpdateTrack(track);
            else
                sender.DestoryTrack(track);
        }
    }
}
