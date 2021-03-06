﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace RadarDisplayPackage
{
    class GraphicTargetTrackViewDrawer_InvisibleS : GraphicTargetTrackViewDrawer_Invisible
    {
        public GraphicTargetTrackViewDrawer_InvisibleS(CoordinateTargetTrackView view) : base(view) { }

        protected override PathGeometry BuildTriangle()
        {
            PathGeometry triangle = View.Factory.CreatePathGeometry();
            GeometrySink gs = triangle.Open();
            gs.BeginFigure(new Point2F(projectivePosition.X, projectivePosition.Y), FigureBegin.Filled);
            gs.AddLine(new Point2F(projectivePosition.X - 5, projectivePosition.Y - 10));
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
            rect.Right = projectivePosition.X + 20;
            rect.Top = projectivePosition.Y - 30;
            rect.Left = rect.Right - idTagWidth;
            rect.Bottom = rect.Top + idTagHeigth;

            idTextRect = rect;
            idTextRect.Left += 12;
            RoundedRectangleGeometry idTag = View.Factory.CreateRoundedRectangleGeometry(new RoundedRect(rect, 3, 3));
            return idTag;
        }

        protected override Point2F GetProjectivePosition()
        {
            Point2F p = new Point2F();

            p.Y =  View.CoordinateSystem.VisibleArea.Bottom;
            p.X =  View.Position.X;

            return p;
        }
    }
}
