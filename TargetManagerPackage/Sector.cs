using System.Collections.Generic;

namespace TargetManagerPackage
{
    public class Sector : AngleArea   //扇区，目标管理器是按扇区管理和处理目标的
    {
        public int index;        //编号
        public List<TargetDot> newDots; //本圈凝聚出来的新点
        public List<TargetDot> oldDots; //上圈凝聚出来的旧点（自由点）
        public List<TargetTrack> tracks;//在本扇区中的航迹


        public Sector(int index, float beginAngle, float endAngle) : base(beginAngle, endAngle)
        {
            this.index = index;
            newDots = new List<TargetDot>();
            oldDots = new List<TargetDot>();
            tracks = new List<TargetTrack>();
        }

        public void ClearAllTargets()
        {
            newDots.Clear();
            tracks.Clear();
            oldDots.Clear();
        }

        public List<TargetTrack> GetActiveTrack()
        {
            List<TargetTrack> ls = new List<TargetTrack>();

            foreach(TargetTrack t in tracks)
            {
                if (t.active)
                    ls.Add(t);
            }
            return ls;
        }

        public void DeleteActiveTrack() //删除被选中的航迹
        {
            for( int i = tracks.Count - 1; i >= 0; i--) //逆向遍历
            {
                if (!tracks[i].active) continue;
                tracks[i].Dispose();
                tracks.RemoveAt(i);
            }
        }

        public void AddNewDot(TargetDot dot)
        {
            dot.sectorIndex = index;
            newDots.Add(dot);
        }

        public void AddTrack(TargetTrack track)
        {
            track.sectorIndex = index;
            tracks.Add(track);
        }

        public void RemoveTrack(TargetTrack track)
        {
            if (track != null)
                tracks.Remove(track);
        }
    }
}
