using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackViewDrawer_Visible : GraphicTargetTrackViewDrawer
    {
        public GraphicTargetTrackViewDrawer_Visible(CoordinateTargetTrackView view) : base(view) { }


        public override void Draw()
        {
            base.Draw();

            View.Canvas.FillEllipse(new Ellipse(View.Position, 4,4), targetViewBrush);

        }
        protected override PathGeometry BuildTriangle()
        {
            PathGeometry triangle = View.Factory.CreatePathGeometry();
            GeometrySink gs = triangle.Open();
            gs.BeginFigure(new Point2F(View.Position.X, View.Position.Y - 5), FigureBegin.Filled);
            gs.AddLine(new Point2F(View.Position.X - 5, View.Position.Y - 15));
            gs.AddLine(new Point2F(View.Position.X + 5, View.Position.Y - 15));
            gs.AddLine(new Point2F(View.Position.X, View.Position.Y - 5));
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return triangle;
        }

        protected override RoundedRectangleGeometry BuildIdTag()
        {
            RectF rect = new RectF
            {
                Left = View.Position.X - 20,
                Top = View.Position.Y - 35
            };
            rect.Right = rect.Left + idTagWidth;
            rect.Bottom = rect.Top + idTagHeigth;

            idTextRect = rect;
            idTextRect.Left += 12;
            RoundedRectangleGeometry idTag = View.Factory.CreateRoundedRectangleGeometry(new RoundedRect(rect,3,3));
            return idTag;
        }
    }
}
