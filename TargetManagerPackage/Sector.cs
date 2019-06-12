using System.Collections.Generic;

namespace TargetManagerPackage
{
    public class Sector : AngleArea   //扇区，目标管理器是按扇区管理和处理目标的
    {
        public int Index;        //编号

        public List<TargetDot> PrepareDots { get; }
        public List<TargetTrack> ExchangeTracks { get; }
        public List<TargetDot> NewDots { get; set; }
        public List<TargetDot> OldDots { get; }
        public List<TargetTrack> StableTracks { get; }
        public List<TargetTrack> TrackHeads { get; }
        public const int SectorCount = 32;
        private readonly object _locker = new object();


        public Sector(int index, float beginAngle, float endAngle) : base(beginAngle, endAngle)
        {
            Index = index;
            PrepareDots = new List<TargetDot>();
            NewDots = new List<TargetDot>();
            OldDots = new List<TargetDot>();
            StableTracks = new List<TargetTrack>();
            TrackHeads = new List<TargetTrack>();
            ExchangeTracks = new List<TargetTrack>();
        }

        public void ClearAllTargets()
        {
            lock (_locker)
            {
                NewDots?.Clear();
                StableTracks?.Clear();
                OldDots?.Clear();
                PrepareDots?.Clear();
                ExchangeTracks?.Clear();
            }
        }

        public List<TargetTrack> GetActiveTrack()
        {
            List<TargetTrack> ls = new List<TargetTrack>();

            lock (_locker)
            {
                foreach (TargetTrack t in StableTracks)
                {
                    if (t.Active)
                        ls.Add(t);
                }
            }
            return ls;
        }

        public void DeleteActiveTrack() //删除被选中的航迹
        {
            lock (_locker)
            {
                for (int i = StableTracks.Count - 1; i >= 0; i--) //逆向遍历
                {
                    if (!StableTracks[i].Active) continue;
                    StableTracks[i].Dispose();
                    StableTracks.RemoveAt(i);
                }
            }
        }

        public void AddPrepareDot(TargetDot dot)
        {
            lock (_locker)
            {
                dot.SectorIndex = Index;
                PrepareDots.Add(dot);
            }
        }

        public void LoadPrepareDots()
        {
            lock (_locker)
            {
                NewDots.AddRange(PrepareDots);
                PrepareDots.Clear();
            }
        }

        public void LoadExchangeTracks()
        {
            lock (_locker)
            {
                StableTracks.AddRange(ExchangeTracks);
                ExchangeTracks.Clear();
            }
        }

        public void AcceptTrackFromOtherSector(TargetTrack track)
        {
            lock (_locker)
            {
                ExchangeTracks.Add(track);
                track.SectorIndex = Index;
            }
        }

        public void DeleteUnqualifiedTracks()       //删除不合格航迹
        {
            lock (_locker)
            {
                for (int i = StableTracks.Count - 1; i >= 0; i--)
                {
                    if (StableTracks[i].Score <= TargetTrack.ScoreMinimum)
                    {
                        //StableTracks[i].Destory();
                        TargetTrack t = StableTracks[i];
                        StableTracks.RemoveAt(i);
                        t.Destory();
                    }
                }
            }
        }

        public void AddNewDot(TargetDot dot)
        {
            lock (_locker)
            {
                dot.SectorIndex = Index;
                NewDots.Add(dot);
            }
        }

        public void AddTrack(TargetTrack track)
        {
            lock (_locker)
            {
                track.SectorIndex = Index;
                StableTracks.Add(track);
            }
        }

        public void RemoveTrack(TargetTrack track)
        {
            lock (_locker)
            {
                if (track != null)
                    StableTracks.Remove(track);
            }
        }

        public List<Target> GetVisibleTargetDots()
        {
            lock (_locker)
            {
                List<Target> ls = new List<Target>();
                foreach (TargetDot newDot in NewDots)
                {
                    if (newDot.ShouldDisplay)
                        ls.Add(newDot);
                }

                return ls;
            }
        }

        public void BeginProcessSector()
        {
            lock (_locker)
            {
                OldDots.Clear();

                foreach (TargetDot dot in NewDots)
                {
                    OldDots.Add(dot);
                }

                NewDots.Clear();
            }
        }
    }
}
