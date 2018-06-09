using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    internal class GraphicTargetDotView : GraphicTargetView
    {
        private const float ColorValueMax = 1;
        private const float ColorValueMin = 0.586f;
        private const float RedColorConst = ColorValueMin;
        private const float RedColorCoefficient = (ColorValueMax - RedColorConst) / TargetManagerPackage.Target.AmValueMaximum;
        private const float BlueColorConst = ColorValueMax;
        private const float BlueColorCoefficient = -RedColorCoefficient;

        public GraphicTargetDotView(Target target, RenderTarget rt, D2DFactory factory, CoordinateSystem cs)
            : base(target, rt, factory, cs)
        {
            if (!((TargetDot) target).IsClotDot)
            {
                targetViewRadius = 2;
                float redColorValue = CalRedColorValue(target.AmValue);
                float blueColorValue = CalBlueColorValue(target.AmValue);
                targetViewBrush = canvas.CreateSolidColorBrush(new ColorF(redColorValue, 0.184f, blueColorValue));
            }
           else
            {
                targetViewBrush = canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb((int)(((TargetDot)target).DotWidth * 3.5f), 150, 150)); //绿色
                targetViewRadius = 4;
            }
        }

        private float CalRedColorValue(float am)
        {
            float ret = RedColorCoefficient * am + RedColorConst;
            return ret;
        }

        private float CalBlueColorValue(float am)
        {
            float ret = BlueColorCoefficient * am + BlueColorConst;
            return ret;
        }

        public override void Draw(RenderTarget renderTarget)
        {
            if (!CoordinateSystem.PointOutOfRange(Tools.Point2FToPoint(Position)))
            {
                base.Draw(renderTarget);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            targetViewBrush?.Dispose();
        }
    }
}
