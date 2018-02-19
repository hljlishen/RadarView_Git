//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;

namespace RadarDisplayPackage
{
    abstract class  GraphicTargetView : TargetView,IDisposable
    {
        protected RenderTarget canvas;
        protected D2DFactory factory;
        private CoordinateSystem coordinateSystem;
        //PathGeometry trackTag;
        Point2F position;                   //dot的屏幕坐标
        Rect activeRect;    //激活区域，鼠标点击到这个区域时，激活（选中）该目标
        int activeRectRadius = 5;   //激活区域半径
        protected int targetViewRadius = 4;           //目标圆点的半径
        protected Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush targetViewBrush;
        protected ITargetManagerController targetController;    //目标控制器

        public RenderTarget Canvas
        {
            get { return canvas; }
        }

        public D2DFactory Factory
        {
            get { return factory; }
        }

        public override void DisplayTarget()
        {
            Draw(canvas);
        }

        public virtual void Draw(RenderTarget renderTarget)     //默认动作，绘制一个圆点
        {
            Ellipse e = new Ellipse(Position, targetViewRadius, targetViewRadius);
            renderTarget.FillEllipse(e, targetViewBrush);
        }

        public virtual Point2F Position
        {
            get
            {
                return position;
            }
        }

        public CoordinateSystem CoordinateSystem
        {
            get
            {
                return coordinateSystem;
            }
        }

        public GraphicTargetView(Target target, RenderTarget canvas, D2DFactory factory, CoordinateSystem coordinateSystem)
            : base(target)
        {
            this.canvas = canvas;
            this.factory = factory;
            this.coordinateSystem = coordinateSystem;

            position = coordinateSystem.CoordinateToPoint(target.CurrentCoordinate);     //计算显示坐标

            activeRect = new Rect();
            activeRect.Left = (int)position.X - activeRectRadius;
            activeRect.Top = (int)position.Y - activeRectRadius;
            activeRect.Right = (int)position.X + activeRectRadius;
            activeRect.Bottom = (int)position.Y + activeRectRadius;

            targetController = TargetManagerFactory.CreateTargetManagerController();
            targetViewBrush = canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(255, 128, 0)); //橘黄
        }


        public virtual bool IsPointInActiveRect(Point p)
        {
            return CoordinateSystem.IsPointInRect(CoordinateSystem.PointToPoint2F(p), activeRect);
        }

        public override void HandleMouseClick(Object p)
        {
            base.HandleMouseClick(p);

            if (IsPointInActiveRect((Point)p))
            {
                target.active = !target.active;
                targetController.SelectTarget(target);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            //trackTag?.Dispose();
        }
    }
}
