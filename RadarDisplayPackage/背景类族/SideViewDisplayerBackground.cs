using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;

namespace RadarDisplayPackage
{
    class SideViewDisplayerBackground : GraphicTrackDisplayerBackground
    {
        private Brush axisBrush;        //坐标轴画刷
        private float axisStrokeWidth = 1.5f;

        private Brush dashLineBrush;    //方位和高度分划线的画刷
        private float dashLineStrokewidth = 1;
        StrokeStyle dashLineStrokeStyle = null;       //线条风格，DashDotDot

        TextFormat xTextFromation;
        TextFormat yTextFromation;
        Brush textBrush;
        private int Ysteps = 6;

        public SideViewDisplayerBackground(RenderTarget canvas, D2DFactory factory, CoordinateSystem csp) : base(canvas, factory, csp)
        {
            axisBrush = canvas.CreateSolidColorBrush(new ColorF(0, 255, 0));  //绿色
            dashLineBrush = canvas.CreateSolidColorBrush(new ColorF(128, 138, 135));   //冷灰

            DWriteFactory dw = DWriteFactory.CreateFactory();
            xTextFromation = dw.CreateTextFormat("Berlin Sans FB Demi", 15);
            yTextFromation = dw.CreateTextFormat("Berlin Sans FB Demi", 15);
            yTextFromation.TextAlignment = TextAlignment.Trailing;
            textBrush = canvas.CreateSolidColorBrush(new ColorF(new ColorI(128, 138, 135)));

            angleLines = new float[] {  30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330};
            angleNumbers = new float[] { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330, 360 };

            distanceNumbers = new float[] { 0, Distance / 6, Distance / 3, Distance / 2, Distance * 2 / 3, Distance * 5 / 6, Distance };

            dw.Dispose();
        }

        public override float Distance
        {
            get
            {
                return base.Distance;
            }

            set
            {
                base.Distance = value;
                distanceNumbers = new float[] { 0, Distance / 6, Distance / 3, Distance / 2, Distance * 2 / 3, Distance * 5 / 6, Distance };
                //Draw();
            }
        }

        public StrokeStyle DashLineStrokeStyle
        {
            get
            {
                return dashLineStrokeStyle;
            }

            set
            {
                dashLineStrokeStyle = value;
            }
        }

        public override void Draw()
        {
            //Y轴画虚线
            for (int i = 1; i < Ysteps; i++)
            {
                int step = coordinateSystem.CoordinateArea.Height / Ysteps;
                int y = (int)(coordinateSystem.CoordinateArea.Bottom - (step * i));
                canvas.DrawLine(new Point2F(coordinateSystem.CoordinateArea.Left, y), new Point2F(coordinateSystem.CoordinateArea.Right, y),
                    dashLineBrush, dashLineStrokewidth, DashLineStrokeStyle);
            }

            //X轴画虚线
            foreach (float angle in angleLines)
            {
                float x = coordinateSystem.OriginalPoint.X + (int)((coordinateSystem.CoordinateArea.Width * angle) / 360);
                canvas.DrawLine(new Point2F(x, coordinateSystem.CoordinateArea.Bottom), new Point2F(x, coordinateSystem.CoordinateArea.Top)
                    , dashLineBrush, dashLineStrokewidth, DashLineStrokeStyle);
            }

            //坐标框
            RectF rf = new RectF(coordinateSystem.CoordinateArea.Left, coordinateSystem.CoordinateArea.Top,
                coordinateSystem.CoordinateArea.Right, coordinateSystem.CoordinateArea.Bottom);
            canvas.DrawRectangle(rf, axisBrush, axisStrokeWidth);

            //画坐标刻度
            DrawAnimation();
        }

        public override void Dispose()
        {
            base.Dispose();
            axisBrush?.Dispose();
            dashLineBrush?.Dispose();
            dashLineStrokeStyle?.Dispose();
        }

        protected void DrawAnimation()
        {
            Point2F[] Locations = new Point2F[13]
            {
                new Point2F(68,470),      //0
                new Point2F(137,470),     //30
                new Point2F(210,470),     //60
                new Point2F(279,470),     //90
                new Point2F(350,470),     //120
                new Point2F(421,470),     //150
                new Point2F(490,470),     //180
                new Point2F(561,470),     //210
                new Point2F(630,470),     //240
                new Point2F(705,470),      //270
                new Point2F(771,470),     //300
                new Point2F(843,470),       //330
                new Point2F(910,470)       //360
            };

            int width = 40;
            for (int j = 0; j < 13; j++)    //横轴
            {
                int x = 44 + j * 76;
                RectF rect = new RectF(x, 528, x + width, 528 + width);
                canvas.DrawText(angleNumbers[j].ToString() + "°", xTextFromation, rect, textBrush);
            }


            for (int j = 0; j < 7; j++)    //纵轴
            {
                int y = 515 - j * 82;
                RectF rect = new RectF(4, y, 4 + width, y + width);
                canvas.DrawText(distanceNumbers[j].ToString("0"), yTextFromation, rect, textBrush);
            }
        }
    }
}
