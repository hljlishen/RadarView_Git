using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Utilities;

namespace RadarDisplayPackage
{
    internal class OverViewDisplayer_Zoom : OverViewDisplayerState
    {
        Rect zoomRect;          //zoomCircle的外切矩形
        Ellipse zoomCircle;     //拖动时，显示待放大区域的圆形
        Brush zoomCircleBrush;   //圆形区域填充画刷
        float zoomCircleBrushWidth = 3;
        Point2F mouseDragPosition;

        public OverViewDisplayer_Zoom(OverViewDisplayer displayer) : base(displayer)
        {
            //this.displayer = displayer;

            //zoom模式下要响应鼠标双击事件，偏心显示
            displayer.DisplayControl.MouseDoubleClick += DisplayControl_MouseDoubleClick;

            isMouseDown = false;
            zoomCircle = new Ellipse();
            zoomCircleBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(0 ,0 ,255));
            zoomCircleBrush.Opacity = 0.5f;
        }

        private void DisplayControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ICommand cmd = new OverViewDisplayerOffsetCommand(displayer, e.Location);
            cmd.Execute();
        }

        public override void Draw()
        {
            if (zoomCircle.RadiusX > 0 && isMouseDown)
            {
                displayer.Canvas.FillEllipse(zoomCircle, zoomCircleBrush);
                displayer.Canvas.DrawEllipse(zoomCircle, zoomCircleBrush, zoomCircleBrushWidth);
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            //计算绘制的圆形大小
            Point2F position = Tools.PointToPoint2F(e.Location);
            mouseDragPosition = position;
            float radius = (float)Tools.DistanceBetween(position, mouseDownPosition);
            zoomCircle = new Ellipse(mouseDownPosition, radius, radius);
                
            //计算矩形区域位置和大小
            zoomRect.Left = (int)(mouseDownPosition.X - zoomCircle.RadiusX);
            zoomRect.Right = (int)(mouseDownPosition.X + zoomCircle.RadiusX);
            zoomRect.Top = (int)(mouseDownPosition.Y - zoomCircle.RadiusY);
            zoomRect.Bottom = (int)(mouseDownPosition.Y + zoomCircle.RadiusY);
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (!ShouldExecuteFunction(e)) return;
            isMouseDown = true;
            mouseDownPosition = Tools.PointToPoint2F(e.Location);   //记录鼠标按下的位置
            mouseDragPosition = mouseDownPosition;
        }

        public override void Dispose()
        {
            base.Dispose();
            displayer.DisplayControl.MouseDoubleClick -= DisplayControl_MouseDoubleClick;
            zoomCircleBrush?.Dispose();
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            base.MouseUp(sender, e);
            if (Math.Abs(mouseDragPosition.X - mouseDownPosition.X) < 8 && Math.Abs(mouseDragPosition.Y - mouseDownPosition.Y) < 8)   //距离太小不放大
                return ;

            displayer.ZoomArea(zoomRect);
        }

        public override OverViewState GetState() => OverViewState.Zoom;
    }
}
