using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    abstract class  OverViewDisplayerState : IDisposable
    {
        protected OverViewDisplayer displayer;
        protected bool isMouseDown;
        protected Point2F mouseDownPosition;
        protected Graphics drawer;
        protected bool shouldExecuteCmd = true;

        public OverViewDisplayerState(OverViewDisplayer displayer)
        {
            this.displayer = displayer;
            drawer = displayer.DisplayControl.CreateGraphics();

            //挂接代理
            displayer.DisplayControl.MouseDown += MouseDown;
            displayer.DisplayControl.MouseUp += MouseUp;
            displayer.DisplayControl.MouseMove += MouseMove;
        }

        public virtual void MouseUp(object sender, MouseEventArgs e)
        {
            if (isMouseDown && shouldExecuteCmd)
            {
                Command cmd = CreateCommand();
                cmd.Execute();
            }
            isMouseDown = false;
        }

        public abstract void MouseMove(object sender, MouseEventArgs e);

        public abstract void MouseDown(object sender, MouseEventArgs e);

        public abstract OverViewState GetState();

        public abstract void Draw();

        protected virtual Command CreateCommand()
        {
            return null;
        }

        public virtual void Dispose()
        {
            //删除代理
            drawer?.Dispose();
            displayer.DisplayControl.MouseDown -= MouseDown;
            displayer.DisplayControl.MouseUp -= MouseUp;
            displayer.DisplayControl.MouseMove -= MouseMove;
        }
    }
}
