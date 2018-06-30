using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    class OverViewDisplayerBackground : GraphicTrackDisplayerBackground
    {
        protected Brush distanceCircleBrush;    //距离分划线的画刷
        protected Ellipse ellipse1, ellipse2, ellipse3, ellipse4;   //1-4为由外到内的4个距离分化线
        protected Brush angleLineBrush;         //角度分划线的画刷
        protected float distanceCircleWidth = 2.5f;    //距离分划线宽度
        protected float angleLineWidth = 1;     //角度分划线宽度
        private readonly TextFormat rightTextFromation;
        private readonly TextFormat leftTextFromation;
        private readonly Brush textBrush;
        private readonly int sectorCount;

        public override float Distance
        {
            get => base.Distance;

            set
            {
                base.Distance = value;
                distanceNumbers = new[] {value/4, value/2, value * 3 / 4, value };
            }
        }

        public StrokeStyle AngleLineStrokeStyle { get; set; } = null;

        public OverViewDisplayerBackground(RenderTarget canvas, D2DFactory factory, CoordinateSystem csp) : base(canvas, factory, csp)
        {
            distanceCircleBrush = canvas.CreateSolidColorBrush(new ColorF(0, 255, 0));  //绿色
            angleLineBrush = canvas.CreateSolidColorBrush(new ColorF(128, 138, 135));   //冷灰
            sectorCount = TargetManagerFactory.CreateTargetDataProvider().GetSectorCount();

            //angleLines = CalSectorBordAngles(sectorCount);//绘制扇区边界线，应注释改行
           
            //初始化四个距离分划线
            ellipse1 = new Ellipse(csp.OriginalPoint, csp.CoordinateArea.Width / 2, csp.CoordinateArea.Height / 2);
            ellipse2 = new Ellipse(csp.OriginalPoint, csp.CoordinateArea.Width * 3 / 8, csp.CoordinateArea.Height * 3 / 8);
            ellipse3 = new Ellipse(csp.OriginalPoint, csp.CoordinateArea.Width / 4, csp.CoordinateArea.Height / 4);
            ellipse4 = new Ellipse(csp.OriginalPoint, csp.CoordinateArea.Width / 8, csp.CoordinateArea.Height / 8);

            distanceNumbers = new[] { Distance * 0.25f, Distance * 0.5f, Distance * 0.75f, Distance };

            DWriteFactory dw = DWriteFactory.CreateFactory();
            rightTextFromation = dw.CreateTextFormat("Berlin Sans FB Demi", 15);
            leftTextFromation = dw.CreateTextFormat("Berlin Sans FB Demi", 15);
            leftTextFromation.TextAlignment = TextAlignment.Trailing;
            textBrush = canvas.CreateSolidColorBrush(new ColorF(new ColorI(128, 138, 135)));
        }

        private float[] CalSectorBordAngles(int sectorCount)
        {
            float step = ((float)360) / sectorCount;
            List<float> ret = new List<float>();
            for (int i = 0; i < sectorCount; i++)
            {
                ret.Add(step * i);
            }

            return ret.ToArray();
        }

        public override void Draw( )
        {
            float x, y;

            //圈
            canvas.DrawEllipse(ellipse1, distanceCircleBrush, distanceCircleWidth);
            canvas.DrawEllipse(ellipse2, distanceCircleBrush, distanceCircleWidth);
            canvas.DrawEllipse(ellipse3, distanceCircleBrush, distanceCircleWidth);
            canvas.DrawEllipse(ellipse4, distanceCircleBrush, distanceCircleWidth);

            //角度虚线
            foreach (float angle in angleLines)
            {
                x = coordinateSystem.OriginalPoint.X + ((float)coordinateSystem.CoordinateArea.Width / 2) * (float)Math.Sin(Tools.DegreeToRadian(angle));
                y = coordinateSystem.OriginalPoint.Y - ((float)coordinateSystem.CoordinateArea.Height / 2) * (float)Math.Cos(Tools.DegreeToRadian(angle));
                canvas.DrawLine(coordinateSystem.OriginalPoint, new Point2F((int)x, (int)y), angleLineBrush, angleLineWidth, AngleLineStrokeStyle);    //画线
            }

            ////画动画数字
            DrawAnimation();
        }

        public override void Dispose()
        {
            base.Dispose();
            distanceCircleBrush?.Dispose();
            angleLineBrush?.Dispose();
            AngleLineStrokeStyle?.Dispose();
        }

        protected void DrawAnimation()
        {
            Point2F[] p = new Point2F[12];

            for(int i = 0; i < 12; i++)
            {
                p[i] = coordinateSystem.CalIntersectionPoint(angleLines[i]);
            }
            string text = angleLines[0].ToString("0") + "°";
            canvas.DrawText(text, rightTextFromation, new RectF(p[0].X - 5, p[0].Y - 20, p[0].X + 10, p[0].Y), textBrush);

            text = angleLines[1].ToString("0") + "°";
            canvas.DrawText(text, rightTextFromation, new RectF(p[1].X, p[1].Y - 20, p[1].X + 40, p[1].Y), textBrush);

            text = angleLines[2].ToString("0") + "°";
            canvas.DrawText(text, rightTextFromation, new RectF(p[2].X, p[2].Y - 20, p[2].X + 40, p[2].Y), textBrush);

            text = angleLines[3].ToString("0") + "°";
            canvas.DrawText(text, rightTextFromation, new RectF(p[3].X + 2, p[3].Y - 10, p[3].X + 40, p[3].Y + 5), textBrush);

            text = angleLines[4].ToString("0") + "°";
            canvas.DrawText(text, rightTextFromation, new RectF(p[4].X , p[4].Y , p[4].X + 40, p[4].Y + 10), textBrush);

            text = angleLines[5].ToString("0") + "°";
            canvas.DrawText(text, rightTextFromation, new RectF(p[5].X, p[5].Y, p[5].X + 40, p[5].Y + 10), textBrush);

            text = angleLines[6].ToString("0") + "°";
            canvas.DrawText(text, rightTextFromation, new RectF(p[6].X - 15, p[6].Y + 2, p[6].X + 40, p[6].Y + 15), textBrush);

            text = angleLines[7].ToString("0") + "°";
            canvas.DrawText(text, leftTextFromation, new RectF(p[7].X - 45, p[7].Y + 2, p[7].X - 2, p[7].Y + 20), textBrush);

            text = angleLines[8].ToString("0") + "°";
            canvas.DrawText(text, leftTextFromation, new RectF(p[8].X - 45, p[8].Y + 2, p[8].X - 2, p[8].Y + 20), textBrush);

            text = angleLines[9].ToString("0") + "°";
            canvas.DrawText(text, leftTextFromation, new RectF(p[9].X - 45, p[9].Y - 10, p[9].X - 5, p[9].Y + 10), textBrush);

            text = angleLines[10].ToString("0") + "°";
            canvas.DrawText(text, leftTextFromation, new RectF(p[10].X - 45, p[10].Y - 15, p[10].X-5 , p[10].Y -2), textBrush);

            text = angleLines[11].ToString("0") + "°";
            canvas.DrawText(text, leftTextFromation, new RectF(p[11].X - 45, p[11].Y - 20, p[11].X + 2, p[11].Y - 2), textBrush);

            Point2F l = ((CoordinateSystemOfPolar)coordinateSystem).CalIntersectionPoint(0, coordinateSystem.CoordinateArea.Width / 2);
            text = distanceNumbers[3].ToString("0");
            canvas.DrawText(text, leftTextFromation, new RectF(l.X - 45, l.Y + 2, l.X - 2, l.Y + 20), textBrush);

            l = ((CoordinateSystemOfPolar)coordinateSystem).CalIntersectionPoint(0, coordinateSystem.CoordinateArea.Width * 3 / 8);
            text = distanceNumbers[2].ToString("0");
            canvas.DrawText(text, leftTextFromation, new RectF(l.X - 45, l.Y + 2, l.X - 2, l.Y + 20), textBrush);

            l = ((CoordinateSystemOfPolar)coordinateSystem).CalIntersectionPoint(0, coordinateSystem.CoordinateArea.Width / 4);
            text = distanceNumbers[1].ToString("0");
            canvas.DrawText(text, leftTextFromation, new RectF(l.X - 45, l.Y + 2, l.X - 2, l.Y + 20), textBrush);

            l = ((CoordinateSystemOfPolar)coordinateSystem).CalIntersectionPoint(0, coordinateSystem.CoordinateArea.Width / 8);
            text = distanceNumbers[0].ToString("0");
            canvas.DrawText(text, leftTextFromation, new RectF(l.X - 45, l.Y + 2, l.X - 2, l.Y + 20), textBrush);
        }
    }
}
