﻿using System;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using TargetManagerPackage;
using Utilities;

namespace RadarDisplayPackage
{
    abstract class OverViewDisplayer_WaveGate : OverViewDisplayerState
    {
        Point2F mouseDragPosition;
        float mouseDownPositionRadius;  //鼠标点击点与圆心连线的距离
        float mouseDragPositionRadius;  //鼠标拖动点与圆心连线的距离
        float beginAngle;       //鼠标点击位置与原点连线与正北夹角
        float dragAngle;        //鼠标拖动位置与原点连线与正北夹角
        const int MinimumDragDistance = 5;  //最小拖动距离，小于该值不绘制图形
        protected Brush waveGateBrush;
        StrokeStyle borderStyle;
        protected bool isSemiAutoWaveGate;    //true表示半自动波门，false表示自动波门

        protected OverViewDisplayer_WaveGate(OverViewDisplayer displayer) : base(displayer)
        {
            StrokeStyleProperties ssp = new StrokeStyleProperties {DashStyle = DashStyle.Dash};
            borderStyle = displayer.Factory.CreateStrokeStyle(ssp);
        }

        public override void Dispose()
        {
            waveGateBrush?.Dispose();
            borderStyle?.Dispose();
            base.Dispose();
        }

        public override void Draw()
        {
            if(isMouseDown)
            {
                //根据鼠标点击坐标和鼠标拖动坐标生成波门几何图形
                float dis = (float)Tools.DistanceBetween(mouseDownPosition, mouseDragPosition);
                if (dis < MinimumDragDistance)    //拖动距离太小，不画
                    return;
                PathGeometry waveGate = displayer.coordinateSystem.BuildWaveGateGeometry(mouseDragPosition, mouseDownPosition);
                displayer.Canvas.DrawGeometry(waveGate, waveGateBrush, 2, borderStyle);
                displayer.Canvas.FillGeometry(waveGate, waveGateBrush);
            }
        }

        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (!isMouseDown)
                return;
            if (displayer.coordinateSystem.PointOutOfRange(e.Location)) //拖动超出极坐标范围，直接返回
                return;

            if (Tools.DistanceBetween(displayer.coordinateSystem.OriginalPoint, Tools.PointToPoint2F(e.Location)) < 5)
                return;

            //记录师表拖动位置
            mouseDragPosition = Tools.PointToPoint2F( e.Location);

            //计算鼠标拖动点的径向距离
            mouseDragPositionRadius = (float)Tools.DistanceBetween(displayer.coordinateSystem.OriginalPoint, Tools.PointToPoint2F(e.Location));

            //计算鼠标点击点在mouseDragPositionRadius长度上对应的坐标
            displayer.CoordinateSystem.RadiusWiseZoomPosition(mouseDownPosition, mouseDragPositionRadius);

            //计算鼠标拖动点在mouseDownPositionRadius长度上对应的坐标
            displayer.CoordinateSystem.RadiusWiseZoomPosition(Tools.PointToPoint2F(e.Location), mouseDownPositionRadius);

            //计算拖拽位置和坐标原点连线的正北夹角
            dragAngle = Tools.AngleToNorth(displayer.coordinateSystem.OriginalPoint, Tools.PointToPoint2F(e.Location));
        }

        public override void MouseDown(object sender, MouseEventArgs e)
        {
            if (!ShouldExecuteFunction(e)) return;

            isMouseDown = true;
            //记录鼠标按下的位置
            mouseDownPosition = Tools.PointToPoint2F(e.Location);
            mouseDragPosition = mouseDownPosition;

            //计算第一个径向距离
            mouseDownPositionRadius =
                (float) Tools.DistanceBetween(displayer.coordinateSystem.OriginalPoint, mouseDownPosition);

            //计算点击位置和坐标原点连线的正北夹角
            beginAngle = Tools.AngleToNorth(displayer.coordinateSystem.OriginalPoint, mouseDownPosition);
        }

        public override void MouseUp(object sender, MouseEventArgs e)
        {
            if (!isMouseDown) return;
            base.MouseUp(sender, e);
            WaveGate waveGate = CalWaveGate();
            if (waveGate != null)
                displayer.SendNewWaveGate(waveGate);
        }

        private WaveGate CalWaveGate()
        {
            float r1 = (float)Tools.DistanceBetween(displayer.coordinateSystem.OriginalPoint, mouseDownPosition);
            float r2 = (float)Tools.DistanceBetween(displayer.coordinateSystem.OriginalPoint, mouseDragPosition);

            //拖动距离太小不处理，这个判断主要是排除鼠标点击操作
            if (Math.Abs(r1 - r2) < 5 || Math.Abs(beginAngle - dragAngle) < 1)
                return null;
            //发送波门位置信息
            Point2F dragPosition = mouseDragPosition;
            PolarCoordinate c = displayer.coordinateSystem.PointToCoordinate(mouseDownPosition);
            //float dis1 = c.ProjectedDis;
            float dis1 = c.Dis;
            c = displayer.coordinateSystem.PointToCoordinate(dragPosition);
            //float dis2 = c.ProjectedDis;
            float dis2 = c.Dis;

            float begin = Tools.FindSmallArcBeginAngle(beginAngle, dragAngle);
            float end = Tools.FindSmallArcEndAngle(beginAngle, dragAngle);
            return new WaveGate(begin, end, dis1, dis2, isSemiAutoWaveGate);

        }
    }
}
