using System;
using System.Drawing;
using System.Windows.Forms;
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

        protected OverViewDisplayerState(OverViewDisplayer displayer)
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
                ICommand cmd = CreateCommand();
                cmd.Execute();
            }
            isMouseDown = false;
        }

        public abstract void MouseMove(object sender, MouseEventArgs e);

        public abstract void MouseDown(object sender, MouseEventArgs e);

        public abstract OverViewState GetState();

        public abstract void Draw();

        protected virtual ICommand CreateCommand()
        {
            return new NullCommand();
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
