using System;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public abstract class TargetView : IDisposable
    {
        protected Target target;        //目标点数据

        public virtual Target Target
        {
            get { return target; }
            set { target = value; }
        }
        public TargetView(Target target) 
        {
            this.target = target;
        }

        public abstract void DisplayTarget();

        public virtual void Dispose() { }

        public abstract bool HandleMouseClick(object p); //处理鼠标点击事件
    }
}
