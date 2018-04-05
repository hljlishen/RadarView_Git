using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackView : GraphicTargetView
    {
        private GraphicTargetTrackViewDrawer drawer;
        protected const int preLocationRadius = 2;  //历史位置圆形半径

        public List<Ellipse> PreLocations { get; set; }

        public GraphicTargetTrackView(Target target, RenderTarget rt, D2DFactory factory, CoordinateSystem cs) 
            :base(target, rt, factory, cs)
        {
            drawer = CreateDrawer();
            PreLocations = new List<Ellipse>();
            TargetTrack t = (TargetTrack)target;

            foreach(PolarCoordinate c in t.locations)
            {
                Point2F p;
                p = CoordinateSystem.CoordinateToPoint(c);
                Ellipse e = new Ellipse(p, preLocationRadius, preLocationRadius);
                PreLocations.Add(e);
            }
            targetViewRadius = 4;
        }

        protected GraphicTargetTrackViewDrawer CreateDrawer()
        {
            //判断坐标系类型，这种代码结构不易扩展，但是改变得可能性很小
            if (CoordinateSystem is CoordinateSystemOfRetangular)
            {
                if (target.Height > CoordinateSystem.Range) //目标在雷达扫描范围外，返回什么也不做的对象
                    return new GraphicTargetTrackViewDrawer_Null(this);
                return new GraphicTargetTrackViewDrawer_Visible(this);
            }

            if (target.CurrentCoordinate.ProjectedDis > CoordinateSystem.Range) //目标在雷达扫描范围外，返回什么也不做的对象
                return new GraphicTargetTrackViewDrawer_Null(this);

            if (CoordinateSystem.IsPointInVisibleRect(Position)) //目标在可视区域内
                return new GraphicTargetTrackViewDrawer_Visible(this);
            Rect r = CoordinateSystem.VisibleArea;
            Point2F p = Position;
            if (p.Y <= r.Top && p.X > r.Left && p.X < r.Right)    //北向超出
            {
                return new GraphicTargetTrackViewDrawer_InvisibleN(this);
            }
            else if (p.Y <= r.Top && p.X >= r.Right)  //东北方向超出
            {
                return new GraphicTargetTrackViewDrawer_InvisibleNE(this);
            }
            else if (p.X > r.Right && p.Y > r.Top && p.Y < r.Bottom)    //东向超出
            {
                return new GraphicTargetTrackViewDrawer_InvisibleE(this);
            }
            else if (p.X >= r.Right && p.Y >= r.Bottom)    //东南向超出
            {
                return new GraphicTargetTrackViewDrawer_InvisibleSE(this);
            }
            else if (p.Y > r.Bottom && p.X > r.Left && p.X < r.Right)    //南向超出
            {
                return new GraphicTargetTrackViewDrawer_InvisibleS(this);
            }
            else if (p.X <= r.Left && p.Y >= r.Bottom)    //西南向超出
            {
                return new GraphicTargetTrackViewDrawer_InvisibleSW(this);
            }
            else if (p.X < r.Left && p.Y > r.Top && p.Y < r.Bottom)    //西向超出
            {
                return new GraphicTargetTrackViewDrawer_InvisibleW(this);
            }
            else   //西北向超出
            {
                return new GraphicTargetTrackViewDrawer_InvisibleNW(this);
            }
        }

        public override void Draw(RenderTarget renderTarget)
        {
            base.Draw(renderTarget);
            drawer.Draw(renderTarget);
        }

        public override void Dispose()
        {
            base.Dispose();
            drawer.Dispose();
        }

        public override bool IsPointInActiveRect(Point p)
        {
            if (base.IsPointInActiveRect(p))
                return true;
            Rect rect = new Rect((int)drawer.IdTextRect.Left - GraphicTargetTrackViewDrawer.iDtextRectLeftOffset, (int)drawer.IdTextRect.Top, (int)drawer.IdTextRect.Right, (int)drawer.IdTextRect.Bottom);
            if (CoordinateSystem.IsPointInRect(CoordinateSystem.PointToPoint2F(p), rect))
            {
                return true;
            }

            return false;
        }
    }
}
