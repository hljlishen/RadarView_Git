using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.Graphics;
using Utilities;

namespace RadarDisplayPackage
{
    internal abstract class GraphicTrackDisplayerBackground :IDisposable
    {
        protected RenderTarget canvas;
        protected D2DFactory factory;
        protected CoordinateSystem coordinateSystem;
        //protected Brush backgroundBrush;
        float beginAngle;
        protected float[] angleLines;
        protected float[] angleNumbers;
        protected float[] distanceNumbers;

        public virtual float BeginAngle
        {
            get => beginAngle;

            set
            {
                beginAngle = Tools.StandardAngle(value);
                for (int i = 0; i < angleNumbers.Length; i++)
                {
                    angleNumbers[i] = Tools.StandardAngle(angleLines[i] + BeginAngle);
                }
            }
        }

        public virtual float Distance { get; set; } = 5000;

        public abstract void Draw();

        protected GraphicTrackDisplayerBackground(RenderTarget canvas,D2DFactory factory, CoordinateSystem coordinateSystem)
        {
            this.canvas = canvas;
            this.factory = factory;
            this.coordinateSystem = coordinateSystem;

            //添加角度分划线数字
            angleLines = new float[] { 0/*,5.27f,10.54f, 15.81f*/, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330 };
            //angleLines = new float[] { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 210, 220, 230, 240, 250, 260, 270, 280, 290, 300, 310, 320, 330, 340, 350 };
            angleNumbers = new float[] { 0, 30, 60, 90, 120, 150, 180, 210, 240, 270, 300, 330 };
        }



        //protected abstract void DrawAnimation(Bitmap bmp, Rectangle displayArea);

        public virtual void Dispose()
        {
            //backgroundBrush?.Dispose();  //释放画刷，属于设备相关资源
        }
    }
}
