using System;
using System.Collections.Generic;

namespace TargetManagerPackage
{
    class TrackCorelatorV1 : TrackCorelator
    {
        public const int threshold = 50;
        public override void Corelate(Sector center, Sector next, Sector previous)
        {
            center.StableTracks.Sort(); //排序,分高的航迹在最后
            //List<TargetTrack> exchangeTracks = new List<TargetTrack>();

            for(int i = center.StableTracks.Count - 1; i >=0; i--)
            {
                if(!CorelateTrack(center.StableTracks[i], center))  //本扇区未有相关点
                {
                    if(!CorelateTrack(center.StableTracks[i], next))  //下个扇区未有相关点
                    {
                        if(CorelateTrack(center.StableTracks[i], previous))   //上个扇区有相关点
                        {
                            //与上个扇区的点相关上
                            previous.AcceptTrackFromOtherSector(center.StableTracks[i]);
                            //exchangeTracks.Add(center.StableTracks[i]);
                            center.RemoveTrack(center.StableTracks[i]);
                        }
                        else
                        {
                            //与三个扇区的点都未相关上，应该对航迹减分
                            center.StableTracks[i].ScoreAdd(-3);    //未相关上,减三分
                            if (center.StableTracks[i].Score <= 0)
                            {
                                TargetTrack t = center.StableTracks[i];
                                center.RemoveTrack(center.StableTracks[i]);

                                t.Destory();
                            }
                            else
                            {
                                if (!center.StableTracks[i].IsFake)
                                    center.StableTracks[i].Update(center.StableTracks[i].PredictCoordinate(DateTime.Now));  //用预测位置更新
                                if (!center.IsAngleInArea(center.StableTracks[i].CurrentCoordinate.Az))
                                {
                                    TargetTrack t = center.StableTracks[i];
                                    center.RemoveTrack(center.StableTracks[i]);
                                    if (next.IsAngleInArea(t.CurrentCoordinate.Az))
                                    {
                                        //exchangeTracks.Add(t);
                                        next.AcceptTrackFromOtherSector(t);
                                    }
                                    else if (previous.IsAngleInArea(t.CurrentCoordinate.Az))
                                    {
                                        //exchangeTracks.Add(t);
                                        previous.AcceptTrackFromOtherSector(t);
                                    }
                                    else
                                    {
                                        t.Destory();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //与下个扇区的点相关上
                        next.AcceptTrackFromOtherSector(center.StableTracks[i]);
                        //exchangeTracks.Add(center.StableTracks[i]);
                        center.RemoveTrack(center.StableTracks[i]);
                    }
                }
                else  //与本扇区相关上，不做任何处理
                {

                }
            }

            center.DeleteUnqualifiedTracks();
            center.LoadExchangeTracks();        //将上一扇区移动过来的航迹添加进来
            NotifyUpdateSectorTrack(center);    //通知更新该扇区的航迹
        }

        private bool CorelateTrack(TargetTrack track, Sector sector)
        {
            bool ret = false;
            if (track.IsFake)       //假航迹，满分，直接返回
            {
                track.ScoreAdd(TargetTrack.ScoreMaximum);
                return false;
            }
            PolarCoordinate predictCoordinate = track.PredictCoordinate(DateTime.Now);  //预测的位置
            //PolarCoordinate predictCoordinate = track.CurrentCoordinate;
            foreach (TargetDot newDot in sector.NewDots)
            {
                if (newDot.Adopted || !newDot.IsClotDot) //被之前的航迹相关上了,或者不是凝聚点
                    continue;
                if (!(predictCoordinate.DistanceTo(newDot.CurrentCoordinate) < track.GetCorelateRadius()))
                    continue;
                //Console.WriteLine(predictCoordinate.DistanceTo(newDot.CurrentCoordinate));
                newDot.Adopted = true;
                track.Update(newDot.CurrentCoordinate);
                track.ScoreAdd(3);
                ret = true;
                break; //已经相关上，返回上层循环  
            }

            return ret;
        }
    }
}
