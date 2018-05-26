﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackViewDrawer_InvisibleSW : GraphicTargetTrackViewDrawer_Invisible
    {
        public GraphicTargetTrackViewDrawer_InvisibleSW(GraphicTargetTrackView view) : base(view) { }

        protected override PathGeometry BuildTriangle()
        {
            PathGeometry triangle = View.Factory.CreatePathGeometry();
            GeometrySink gs = triangle.Open();
            gs.BeginFigure(new Point2F(projectivePosition.X, projectivePosition.Y), FigureBegin.Filled);
            gs.AddLine(new Point2F(projectivePosition.X + 20, projectivePosition.Y - 10));
            gs.AddLine(new Point2F(projectivePosition.X + 5, projectivePosition.Y - 10));
            gs.AddLine(new Point2F(projectivePosition.X, projectivePosition.Y));
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return triangle;
        }

        protected override RoundedRectangleGeometry BuildIdTag()
        {
            RectF rect = new RectF();
            rect.Left = projectivePosition.X + 5;
            rect.Bottom = projectivePosition.Y - 10;
            rect.Right = rect.Left + idTagWidth;
            rect.Top = rect.Bottom - idTagHeigth;

            idTextRect = rect;
            idTextRect.Left += 12;
            RoundedRectangleGeometry idTag = View.Factory.CreateRoundedRectangleGeometry(new RoundedRect(rect, 3, 3));
            return idTag;
        }

        protected override Point2F GetProjectivePosition()
        {
            Point2F p = new Point2F();

            p.Y = Math.Min(View.CoordinateSystem.VisibleArea.Bottom, View.Position.Y);
            p.X = Math.Max(View.CoordinateSystem.VisibleArea.Left, View.Position.X);

            return p;
        }
    }
}
