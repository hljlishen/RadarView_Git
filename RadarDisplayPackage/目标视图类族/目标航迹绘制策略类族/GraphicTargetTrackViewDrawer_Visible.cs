using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackViewDrawer_Visible : GraphicTargetTrackViewDrawer
    {
        public GraphicTargetTrackViewDrawer_Visible(GraphicTargetTrackView view) : base(view) { }


        public override void Draw()
        {
            base.Draw();

            view.Canvas.FillEllipse(new Ellipse(view.Position, 4,4), targetViewBrush);
        }
        protected override PathGeometry BuildTriangle()
        {
            PathGeometry triangle = view.Factory.CreatePathGeometry();
            GeometrySink gs = triangle.Open();
            gs.BeginFigure(new Point2F(view.Position.X, view.Position.Y - 5), FigureBegin.Filled);
            gs.AddLine(new Point2F(view.Position.X - 5, view.Position.Y - 15));
            gs.AddLine(new Point2F(view.Position.X + 5, view.Position.Y - 15));
            gs.AddLine(new Point2F(view.Position.X, view.Position.Y - 5));
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return triangle;
        }

        protected override RoundedRectangleGeometry BuildIDTag()
        {
            RectF rect = new RectF();
            rect.Left = view.Position.X - 20;
            rect.Top = view.Position.Y - 35;
            rect.Right = rect.Left + idTagWidth;
            rect.Bottom = rect.Top + idTagHeigth;

            idTextRect = rect;
            idTextRect.Left += 12;
            RoundedRectangleGeometry idTag = view.Factory.CreateRoundedRectangleGeometry(new RoundedRect(rect,3,3));
            return idTag;
        }
    }
}
