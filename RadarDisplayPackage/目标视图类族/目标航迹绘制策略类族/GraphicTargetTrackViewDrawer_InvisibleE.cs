using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackViewDrawer_InvisibleE : GraphicTargetTrackViewDrawer_Invisible
    {
        public GraphicTargetTrackViewDrawer_InvisibleE(CoordinateTargetTrackView view) : base(view) { }

        protected override PathGeometry BuildTriangle()
        {
            PathGeometry triangle = View.Factory.CreatePathGeometry();
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
            RectF rect = new RectF
            {
                Right = projectivePosition.X - 10,
                Top = projectivePosition.Y - 10
            };
            rect.Left = rect.Right - idTagWidth;
            rect.Bottom = rect.Top + idTagHeigth;

            IdTextRect = rect;
            idTextRect.Left += 12;
            RoundedRectangleGeometry idTag = View.Factory.CreateRoundedRectangleGeometry(new RoundedRect(rect, 3, 3));
            return idTag;
        }

        protected override Point2F GetProjectivePosition()
        {
            Point2F p = new Point2F();

            p.Y = View.Position.Y;
            p.X = View.CoordinateSystem.VisibleArea.Right;

            return p;
        }
    }
}
