using System.Collections.Generic;

namespace TargetManagerPackage
{
    public class Sector : AngleArea   //扇区，目标管理器是按扇区管理和处理目标的
    {
        public int Index;        //编号
        public List<TargetDot> NewDots { get; set; }
        public List<TargetDot> OldDots { get; }
        public List<TargetTrack> Tracks { get; set; }
        public const int SectorCount = 32;
        private readonly object _locker = new object();


        public Sector(int index, float beginAngle, float endAngle) : base(beginAngle, endAngle)
        {
            Index = index;
            NewDots = new List<TargetDot>();
            OldDots = new List<TargetDot>();
            Tracks = new List<TargetTrack>();
        }

        public void ClearAllTargets()
        {
            lock (_locker)
            {
                NewDots?.Clear();
                Tracks?.Clear();
                OldDots?.Clear();
            }
        }

        public List<TargetTrack> GetActiveTrack()
        {
            List<TargetTrack> ls = new List<TargetTrack>();

            lock (_locker)
            {
                foreach (TargetTrack t in Tracks)
                {
                    if (t.active)
                        ls.Add(t);
                }
            }
            return ls;
        }

        public void DeleteActiveTrack() //删除被选中的航迹
        {
            lock (_locker)
            {
                for (int i = Tracks.Count - 1; i >= 0; i--) //逆向遍历
                {
                    if (!Tracks[i].active) continue;
                    Tracks[i].Dispose();
                    Tracks.RemoveAt(i);
                }
            }
        }

        public void AddNewDot(TargetDot dot)
        {
            lock (_locker)
            {
                dot.sectorIndex = Index;
                NewDots.Add(dot);
            }
        }

        public void AddTrack(TargetTrack track)
        {
            lock (_locker)
            {
                track.sectorIndex = Index;
                Tracks.Add(track);
            }
        }

        public void RemoveTrack(TargetTrack track)
        {
            lock (_locker)
            {
                if (track != null)
                    Tracks.Remove(track);
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
