using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Resources;
using TargetManagerPackage;
using System.Collections;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;

namespace RadarDisplayPackage
{
    internal abstract class TargetView : IDisposable
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

        public virtual void HandleMouseClick(object p) { }  //处理鼠标点击事件
    }
}
