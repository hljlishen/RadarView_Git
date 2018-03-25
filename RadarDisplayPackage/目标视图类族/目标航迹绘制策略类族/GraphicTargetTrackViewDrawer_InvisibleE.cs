using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackViewDrawer_InvisibleE : GraphicTargetTrackViewDrawer_Invisible
    {
        public GraphicTargetTrackViewDrawer_InvisibleE(GraphicTargetTrackView view) : base(view) { }

        protected override PathGeometry BuildTriangle()
        {
            PathGeometry triangle = view.Factory.CreatePathGeometry();
            GeometrySink gs = triangle.Open();
            gs.BeginFigure(new Point2F(projectivePosition.X, projectivePosition.Y), FigureBegin.Filled);
            gs.AddLine(new Point2F(projectivePosition.X - 10, projectivePosition.Y - 5));
            gs.AddLine(new Point2F(projectivePosition.X - 10, projectivePosition.Y + 5));
            gs.AddLine(new Point2F(projectivePosition.X, projectivePosition.Y));
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return triangle;
        }

        protected override RoundedRectangleGeometry BuildIdTag()
        {
            RectF rect = new RectF();
            rect.Right = projectivePosition.X - 10;
            rect.Top = projectivePosition.Y - 10;
            rect.Left = rect.Right - idTagWidth;
            rect.Bottom = rect.Top + idTagHeigth;

            IdTextRect = rect;
            idTextRect.Left += 12;
            RoundedRectangleGeometry idTag = view.Factory.CreateRoundedRectangleGeometry(new RoundedRect(rect, 3, 3));
            return idTag;
        }

        protected override Point2F GetProjectivePosition()
        {
            Point2F p = new Point2F();

            p.Y = view.Position.Y;
            p.X = view.CoordinateSystem.VisibleArea.Right;

            return p;
        }
    }
}
