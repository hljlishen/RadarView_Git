using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    abstract class GraphicTargetTrackViewDrawer : IDisposable
    {
        protected GraphicTargetTrackView view;
        protected Brush idBrush; //标签文本画刷
        protected Brush tagBrush; //标签框画刷
        //PathGeometry pg;
        TextFormat idFormation;
        protected Brush targetViewBrush;    //
        protected int idTagWidth = 40, idTagHeigth = 20;
        protected RectF idTextRect;
        protected int smallIDOffset = 3;    //只有一位数字的ID需要向右偏移的距离
        public const int iDtextRectLeftOffset = 12;
        protected Brush preLocationsBrush;  //历史位置画刷

        public RectF IdTextRect
        {
            get
            {
                return idTextRect;
            }

            set
            {
                idTextRect = value;
            }
        }

        public GraphicTargetTrackViewDrawer(GraphicTargetTrackView view)
        {
            this.view = view;

            DWriteFactory dw = DWriteFactory.CreateFactory();
            idFormation = dw.CreateTextFormat("Berlin Sans FB Demi", 15);
            targetViewBrush = view.Canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(255 ,128 ,0)); //橘黄
            idBrush = view.Canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(255 ,192 ,203)); //粉色
            idBrush.Opacity = 1;

            if (!view.Target.active)
            {
                tagBrush = view.Canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(65, 105, 225)); //品蓝
            }
            else
                tagBrush = view.Canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(255, 0, 0)); //red
            tagBrush.Opacity = 0.6f;

            preLocationsBrush = view.Canvas.CreateSolidColorBrush(GraphicTrackDisplayer.GetColorFFromRgb(255, 0, 0));   //red
            preLocationsBrush.Opacity = 0.7f;
            dw.Dispose();
        }

        public virtual void Draw(RenderTarget renderTarget)
        {
            //计算ID框和三角形位置
            PathGeometry pg = BuildTriangle();
            RoundedRectangleGeometry IDTag = BuildIDTag();

            //画ID标签
            renderTarget.FillGeometry(pg, tagBrush);
            renderTarget.FillGeometry(IDTag, tagBrush);
            TargetTrack t = (TargetTrack)view.Target;
            if (t.trackID < 10)     //标签是一位数还是两位数
                idTextRect.Left += smallIDOffset;
            renderTarget.DrawText(t.trackID.ToString(), idFormation, idTextRect, idBrush);

            //画历史航迹
            //foreach(Ellipse e in view.PreLocations)
            //{
            //    renderTarget.FillEllipse(e, preLocationsBrush);
            //}

            //释放资源
            pg.Dispose();
            IDTag.Dispose();
        }
        public virtual void Draw()
        {
            Draw(view.Canvas);
        }

        protected abstract PathGeometry BuildTriangle();

        protected abstract RoundedRectangleGeometry BuildIDTag();

        public virtual void Dispose()
        {
            idBrush?.Dispose();
            tagBrush?.Dispose();
            idFormation?.Dispose();
            targetViewBrush?.Dispose();
            preLocationsBrush?.Dispose();
        }
    }
}
