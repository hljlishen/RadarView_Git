using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackViewDrawer_InvisibleNE : GraphicTargetTrackViewDrawer_Invisible
    {
        public GraphicTargetTrackViewDrawer_InvisibleNE(GraphicTargetTrackView view) : base(view) { }

        protected override PathGeometry BuildTriangle()
        {
            PathGeometry triangle = view.Factory.CreatePathGeometry();
            GeometrySink gs = triangle.Open();
            gs.BeginFigure(new Point2F(projectivePosition.X, projectivePosition.Y), FigureBegin.Filled);
            gs.AddLine(new Point2F(projectivePosition.X - 20, projectivePosition.Y + 10));
            gs.AddLine(new Point2F(projectivePosition.X-5, projectivePosition.Y + 10));
            gs.AddLine(new Point2F(projectivePosition.X, projectivePosition.Y));
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return triangle;
        }

        protected override RoundedRectangleGeometry BuildIDTag()
        {
            RectF rect = new RectF();
            rect.Right = projectivePosition.X - 5;
            rect.Top = projectivePosition.Y + 10;
            rect.Left = rect.Right - idTagWidth;
            rect.Bottom = rect.Top + idTagHeigth;

            idTextRect = rect;
            idTextRect.Left += 12;
            RoundedRectangleGeometry idTag = view.Factory.CreateRoundedRectangleGeometry(new RoundedRect(rect, 3, 3));
            return idTag;
        }

        protected override Point2F GetProjectivePosition()
        {
            Point2F p = new Point2F();

            p.Y = Math.Max( view.CoordinateSystem.VisibleArea.Top, view.Position.Y);
            p.X = Math.Min(view.CoordinateSystem.VisibleArea.Right, view.Position.X);

            return p;
        }
    }
}
