﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    public abstract class GraphicTargetTrackViewDrawer : IDisposable
    {
        public static bool ShouldDrawCourse { get; set; } = true;
        protected CoordinateTargetTrackView View;
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
            get => idTextRect;

            set => idTextRect = value;
        }

        protected GraphicTargetTrackViewDrawer(CoordinateTargetTrackView view)
        {
            this.View = view;

            DWriteFactory dw = DWriteFactory.CreateFactory();
            idFormation = dw.CreateTextFormat("Berlin Sans FB Demi", 15);
            targetViewBrush = view.Canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb(255 ,128 ,0)); //橘黄
            idBrush = view.Canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb(255 ,192 ,203)); //粉色
            idBrush.Opacity = 1;
            tagBrush = view.Canvas.CreateSolidColorBrush(!view.Target.Active ? Tools.GetColorFFromRgb(65, 105, 225) : Tools.GetColorFFromRgb(255, 0, 0));
            tagBrush.Opacity = 0.6f;

            preLocationsBrush = view.Canvas.CreateSolidColorBrush(Tools.GetColorFFromRgb(255, 255, 255));   //white
            preLocationsBrush.Opacity = 0.7f;
            dw.Dispose();
        }

        public virtual void Draw(RenderTarget renderTarget)
        {
            //计算ID框和三角形位置
            PathGeometry pg = BuildTriangle();
            if (pg == null) return;
            RoundedRectangleGeometry idTag = BuildIdTag();

            if (ShouldDrawCourse)
                DrawTrackCourse(renderTarget);

            //画ID标签
            renderTarget.FillGeometry(pg, tagBrush);
            renderTarget.FillGeometry(idTag, tagBrush);
            TargetTrack t = (TargetTrack) View.Target;
            if (t.TrackId < 10) //标签是一位数还是两位数
                idTextRect.Left += smallIDOffset;
            renderTarget.DrawText(t.TrackId.ToString(), idFormation, idTextRect, idBrush);

            //释放资源
            pg.Dispose();
            idTag.Dispose();
        }

        public virtual void Draw() => Draw(View.Canvas);

        protected abstract PathGeometry BuildTriangle();

        protected abstract RoundedRectangleGeometry BuildIdTag();

        protected void DrawTrackCourse(RenderTarget renderTarget)
        {
            //画历史航迹
            foreach (Ellipse e in View.PreLocations)
            {
                renderTarget.FillEllipse(e, preLocationsBrush);
            }
            for (int i = 0; i < View.PreLocations.Count; i++)
            {
                if (i == View.PreLocations.Count - 1)
                    break;
                View.Canvas.DrawLine(View.PreLocations[i].Point, View.PreLocations[i + 1].Point, preLocationsBrush, 1);
            }
            if (View.PreLocations.Count != 0)
                View.Canvas.DrawLine(View.PreLocations[View.PreLocations.Count - 1].Point, View.Position, preLocationsBrush, 1);
        }

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
