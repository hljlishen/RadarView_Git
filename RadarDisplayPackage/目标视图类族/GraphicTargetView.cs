
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Utilities;

namespace RadarDisplayPackage
{
    public abstract class  GraphicTargetView : TargetView,IDisposable
    {
        protected RenderTarget canvas;
        protected D2DFactory factory;

        //PathGeometry trackTag;
        private readonly Point2F position;                   //dot的屏幕坐标
        private readonly Rect activeRect;    //激活区域，鼠标点击到这个区域时，激活（选中）该目标
        private const int activeRectRadius = 3; //激活区域半径
        protected int targetViewRadius = 4;           //目标圆点的半径
        protected Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush targetViewBrush;
        protected ITargetManagerController targetController;    //目标控制器

        public RenderTarget Canvas => canvas;

        public D2DFactory Factory => factory;

        public override void DisplayTarget()
        {
            Draw(canvas);
        }

        public virtual void Draw(RenderTarget renderTarget)     //默认动作，绘制一个圆点
        {
            Ellipse e = new Ellipse(Position, targetViewRadius, targetViewRadius);
            renderTarget.FillEllipse(e, targetViewBrush);
        }

        public virtual Point2F Position => position;

        public CoordinateSystem CoordinateSystem { get; }

        protected GraphicTargetView(Target target, RenderTarget canvas, D2DFactory factory, CoordinateSystem coordinateSystem)
            : base(target)
        {
            this.canvas = canvas;
            this.factory = factory;
            CoordinateSystem = coordinateSystem;

            position = coordinateSystem.CoordinateToPoint(target.CurrentCoordinate);     //计算显示坐标

            activeRect = new Rect
            {
                Left = (int) position.X - activeRectRadius,
                Top = (int) position.Y - activeRectRadius,
                Right = (int) position.X + activeRectRadius,
                Bottom = (int) position.Y + activeRectRadius
            };

            targetController = TargetManagerFactory.CreateTargetManagerController();
            targetViewBrush = canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb(255, 128, 0)); //橘黄
        }


        public virtual bool IsPointInActiveRect(Point p)
        {
            return Tools.IsPointInRect(Tools.PointToPoint2F(p), activeRect);
        }

        public override bool HandleMouseClick(Object p)
        {
            if (IsPointInActiveRect((Point)p))
            {
                target.active = !target.active;
                targetController.SelectTarget(target);
                return true;
            }

            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
            //trackTag?.Dispose();
        }
    }
}
