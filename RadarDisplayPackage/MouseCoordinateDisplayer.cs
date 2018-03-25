using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Windows.Forms;
using TargetManagerPackage;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;

namespace RadarDisplayPackage
{
    internal class MouseCoordinateDisplayer
    {
        private OverViewDisplayer displayer;
        private BitmapRenderTarget mouseLocationCoordinateBitmap;
        private RoundedRectangleGeometry coordinateBoard;
        private Brush coordinateBoardBrush;
        private Brush textBrush;
        private string azString = "", disString = "";
        private TextFormat idFormation;

        public MouseCoordinateDisplayer(OverViewDisplayer ovd)
        {
            displayer = ovd;
            displayer.DisplayControl.MouseMove += MoveMoveHandler;
            mouseLocationCoordinateBitmap = displayer.Canvas.CreateCompatibleRenderTarget();
            coordinateBoardBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 1, 1));
            coordinateBoardBrush.Opacity = 0.5f;
            textBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(0, 1, 1));
            DWriteFactory dw = DWriteFactory.CreateFactory();
            idFormation = dw.CreateTextFormat("Berlin Sans FB Demi", 25);
        }

        public void MoveMoveHandler(object sender, MouseEventArgs e)
        {
            Point2F location = CoordinateSystem.PointToPoint2F(e.Location);
            PolarCoordinate coordinate = displayer.coordinateSystem.PointToCoordinate(location);
            azString = "方位：" + coordinate.Az.ToString("0.0");
            disString = "距离：" + coordinate.ProjectedDis.ToString("0.0");
            RectF rect = new RectF(location.X + 10, location.Y + 30, location.X + 60, location.Y + 80);
            coordinateBoard?.Dispose();
            coordinateBoard = displayer.Factory.CreateRoundedRectangleGeometry(new RoundedRect(rect,5,5));
        }

        public void Draw()
        {
            //displayer.Canvas.FillGeometry(coordinateBoard, coordinateBoardBrush);
            displayer.Canvas.DrawText(azString, idFormation,new RectF(10,10, 1000, 100), textBrush);
            displayer.Canvas.DrawText(disString, idFormation, new RectF(10, 45, 1000, 100), textBrush);
        }
    }
}
