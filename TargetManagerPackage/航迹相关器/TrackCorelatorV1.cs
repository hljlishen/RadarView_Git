using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class TrackCorelatorV1 : TrackCorelator
    {
        public const int threshold = 50;
        public override void Corelate(Sector center, Sector next, Sector previous)
        {
            for(int i = center.tracks.Count - 1; i >=0; i--)
            {
                if(!CorelateTrack(center.tracks[i], center,true))  //本扇区未有相关点
                {
                    if(!CorelateTrack(center.tracks[i], next, false))  //下个扇区未有相关点
                    {
                        if(CorelateTrack(center.tracks[i], previous, true))   //上个扇区有相关点
                        {
                            //与上个扇区的点相关上
                            previous.AddTrack(center.tracks[i]);
                            center.RemoveTrack(center.tracks[i]);
                        }
                        else
                        {
                            //与三个扇区的点都未相关上，应该对航迹减分

                        }
                    }
                    else
                    {
                        //与下个扇区的点相关上
                        next.AddTrack(center.tracks[i]);
                        center.RemoveTrack(center.tracks[i]);
                    }
                }
                else
                {

                }

                NotifyUpdateSectorTrack(center);    //通知更新该扇区的航迹
            }
        }

        private bool CorelateTrack(TargetTrack track, Sector center, bool shouldAdoptDot)
        {
            bool ret = false;
            foreach (TargetDot newDot in center.newDots)
            {
                if (newDot.Adopted) //被之前的航迹相关上了
                    continue;
                if (track.DistanceTo(newDot) < threshold)
                {
                    if (shouldAdoptDot)
                    {
                        newDot.Adopted = true;
                        track.Update(newDot.CurrentCoordinate);
                        //NotifyAllObservers(track, NotifyType.Update);
                        //NotifyAllObservers(newDot, NotifyType.Delete);
                    }
                    ret = true;
                    break;  //已经相关上，返回上层循环
                }
            }
            return ret;
        }
    }
}
