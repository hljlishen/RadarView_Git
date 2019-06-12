namespace TargetManagerPackage
{
    class DotCorelatorV1 : DotCorelator
    {
        public const int thresholdMax = 120;
        public const int thresholdMin = 0;
        //public override void Corelate(Sector center, Sector left, Sector right)
        //{
        //    foreach(TargetDot oldDot in center.OldDots)
        //    {
        //        if (CorelateDot(oldDot, center)) continue;
        //        if (CorelateDot(oldDot, right)) continue;
        //        CorelateDot(oldDot, left);
        //    }

        //    NotifyDeleteSectorTrack(center);
        //}

        private bool CorelateDot(TargetDot oldDot, Sector center)
        {
            bool ret = false;
            foreach (TargetDot newDot in center.NewDots)
            {
                if (newDot.Adopted || oldDot.Adopted) //已经被航迹相关
                    continue;
                float distance = oldDot.DistanceTo(newDot);

                if (distance < thresholdMax && distance > thresholdMin)
                {
                    TargetTrack track = TargetTrack.CreateTargetTrack(newDot, oldDot, 3);

                    if (track == null)   //创建航迹失败
                        continue;

                    newDot.Adopted = true;
                    oldDot.Adopted = true;
                    center.AddTrack(track);
                    ret = true;
                    //break;  //相关成功，返回上层循环
                }
            }

            return ret;
        }
    }
}
