using System;
using System.Collections.Generic;

namespace TargetManagerPackage
{
    abstract class DotCorelator : SectorProcessor,IDisposable
    {
        //protected IDotCorelateAlgorithm DotCorelateAlgorithm;

        protected DotCorelator(List<ITargetObserver> ls) : base(ls)
        {
            //DotCorelateAlgorithm = new DotCorelatorAlgorithm_DistanceNear();
        }

        protected DotCorelator()
        {
            //DotCorelateAlgorithm = new DotCorelatorAlgorithm_DistanceNear();
        }

        public virtual void Corelate(Sector center, Sector left, Sector right)
        {
            foreach (TargetDot oldDot in center.OldDots)
            {
                if ((oldDot.Dis > 1500 && oldDot.Dis < 2500) || (oldDot.Dis> 2600 && oldDot.Dis < 2700))
                    continue; //屏蔽杂波多的区域
                if (CorelateDotToSector(oldDot, center)) continue;
                if (CorelateDotToSector(oldDot, right)) continue;
                CorelateDotToSector(oldDot, left);      //和左侧扇区中的点相关
            }

            NotifyUpdateSectorTrack(center);
        }

        protected virtual bool CorelateDotToSector(TargetDot oldDot, Sector sector)   //自由点和一个扇区的新点相关，返回true表示相关成功
        {
            bool ret = false;
            if (!oldDot.IsClotDot) return false;
            foreach (TargetDot newDot in sector.NewDots)
            {
                if (newDot.Adopted || !newDot.IsClotDot) //已经被航迹相关上的点不作处理
                    continue;

                if (DotsCanCorelate(oldDot,newDot) && oldDot.IsDotRelated(newDot))   //两个点是否相关成功
                {
                    TargetTrack track =
                        TargetTrack.CreateTargetTrack(newDot, oldDot, 3);
                    if (track == null)   //创建航迹失败，航迹编号满
                        continue;

                    //newDot.Adopted = true;    //自由点相关时，一个目标点可以与多个自由点相关
                    oldDot.Adopted = true;
                    sector.AddTrack(track);

                    ret = true;
                    break;  //相关成功，返回上层循环
                }
            }
            return ret;
        }

        protected virtual bool DotsCanCorelate(TargetDot dot1, TargetDot dot2) //根据不同起批状态判断两个点击是否可以相关，波门状态下需判断两个点是否在同一波门
            => true;

        public void Dispose()
        {
            Observers.Clear();
        }
    }
}
