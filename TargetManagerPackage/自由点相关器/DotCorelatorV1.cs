using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TargetManagerPackage
{
    class DotCorelatorV1 : DotCorelator
    {
        public const int threshold = 500;
        public override void Corelate(Sector center, Sector left, Sector right)
        {
            foreach(TargetDot oldDot in center.oldDots)
            {
                if(!CorelateDot(oldDot, center))
                {
                    if (!CorelateDot(oldDot, right))
                        CorelateDot(oldDot, left);
                }
            }

            NotifyDeleteSectorTrack(center);
        }

        private bool CorelateDot(TargetDot oldDot, Sector center)
        {
            bool ret = false;
            foreach (TargetDot newDot in center.newDots)
            {
                if (newDot.Adopted) //已经被航迹相关
                    continue;
                float distance = oldDot.DistanceTo(newDot);

                if (distance < threshold)
                {
                    TargetTrack track = TargetTrack.CreateTargetTrack(newDot.CurrentCoordinate, oldDot.CurrentCoordinate);
                    if (track == null)   //创建航迹失败
                        continue;

                    newDot.Adopted = true;
                    oldDot.Adopted = true;
                    center.AddTrack(track);
                    ret = true;
                    break;  //相关成功，返回上层循环
                }
            }

            return ret;
        }
    }
}
