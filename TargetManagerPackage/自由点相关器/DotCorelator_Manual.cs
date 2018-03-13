﻿using System.Collections.Generic;

namespace TargetManagerPackage
{
    class DotCorelator_Manual : DotCorelator
    {
        public DotCorelator_Manual()
        {

        }
        public DotCorelator_Manual(List<ITargetObserver> ls) : base(ls)
        {

        }
        public override void Corelate(Sector center, Sector left, Sector right) //手动起批模式，不做任何处理
        {
            return;
        }
    }
}
