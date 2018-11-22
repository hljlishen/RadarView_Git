using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TargetManagerPackage;
using Utilities;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace RadarDisplayPackage.目标显示器类族
{
    class TextTargetViewManager : TargetViewManager
    {
        private float Top;
        private float currentTop;
        private List<TargetTrack>[] targetTracks;
        private List<TextTargetView> views;
        public TextTargetViewManager(TrackDisplayer displayer) : base(displayer)
        {
            displayer.DisplayControl.MouseClick += DisplayControl_MouseClick;
            displayer.DisplayControl.MouseDoubleClick += DisplayControlOnMouseDoubleClick;
            Top = ((TextDisplayer)displayer).DrawTargetArea.Top;
            views = new List<TextTargetView>();
            targetTracks = new List<TargetTrack>[targetProvider.GetSectorCount()];
            for (int i = 0; i < targetTracks.Length; i++)
            {
                targetTracks[i] = new List<TargetTrack>();
            }
        }

        private void DisplayControlOnMouseDoubleClick(object sender, MouseEventArgs mouseEventArgs)
        {
            lock (_locker)
            {
                foreach (var textTargetView in views)
                {
                    if (textTargetView.HandleMouseDoubleClick(mouseEventArgs.Location))
                        return;
                }
            }
        }

        private void DisplayControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            lock (_locker)
            {
                foreach (var textTargetView in views)
                {
                    if (textTargetView.HandleMouseClick(e.Location))
                        return;
                }
            }
        }

        public override void DisplayTargetViews()
        {
            currentTop = Top;
            views.Clear();
            List<TargetTrack> tracks = new List<TargetTrack>();
            lock (_locker)
            {
                foreach (List<TargetTrack> targetViews in targetTracks)
                {
                    foreach (TargetTrack targetTrack in targetViews)
                    {
                        tracks.Add(targetTrack);
                    }
                }

                tracks.Sort(new TrackIdComparer());

                foreach (var track in tracks)
                {
                    TextTargetView view = (TextTargetView) CreateTargetView(track);
                    views.Add(view);
                    view.DisplayTarget();
                }
            }
        }

        public override TargetView CreateTargetView(Target taget)
        {
            TextTargetView view = new TextTargetView(taget, (TextDisplayer)displayer, new Point2F(0, currentTop));
            currentTop += TextTargetView.Height;
            return view;
        }

        protected override void LoadTargetViews(List<Target> tracks)
        {
            if (tracks == null)
                return;

            lock (_locker)
            {
                foreach (List<TargetTrack> targetTrack in targetTracks)
                {
                    targetTrack.Clear();
                }

                foreach (Target target in tracks)
                {
                    TargetTrack track = (TargetTrack) target;
                    targetTracks[track.TrackId].Add(track);
                }
            }
        }

        public override void NotifyUpdateSectorDot(List<TargetDot> newDots, int sectorIndex)
        {
            //对点目标不做任何处理
        }

        public override void NotifyUpdateSectorTrack(List<TargetTrack> trackList, int sectorIndex)
        {
            lock (_locker)
            {
                targetTracks[sectorIndex].Clear();
                if (trackList == null) return;

                targetTracks[sectorIndex].AddRange(trackList);
            }
        }
    }

    class TextTargetView : TargetView/*, IComparable*/
    {
        private TextDisplayer _displayer;
        private Point2F leftTop;
        private int _columnWidth;
        public const int Height = 30;
        private List<RectF> _rects;
        private List<string> _texts;
        private RectF activeRect;

        private readonly Brush _borderBrush;
        private readonly TextFormat _inactiveTextFormat;
        private readonly Brush _inactiveBrush;
        private readonly TextFormat _activeTextFormat;
        private readonly Brush _activeBrush;
        public TextTargetView(Target target, TextDisplayer displayer, Point2F drawPoint) : base(target)
        {
            _displayer = displayer;
            leftTop = drawPoint;
            _columnWidth = displayer.ColumnWidth;
            _rects = CalculateRects();
            activeRect = CalculateActiveRect();
            _texts = new List<string>();
            TargetTrack track = (TargetTrack) target;
            _texts.Add(track.TrackId.ToString());
            _texts.Add(track.Az.ToString("0.0"));
            _texts.Add(track.El.ToString("0.0"));
            _texts.Add(track.Dis.ToString("0.0"));
            _texts.Add(track.Speed.ToString("0.0"));

            _borderBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(1, 1, 1));
            DWriteFactory dw = DWriteFactory.CreateFactory();
            _inactiveTextFormat = dw.CreateTextFormat("宋体", 20);
            _inactiveTextFormat.TextAlignment = TextAlignment.Center;
            _inactiveBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(0, 1, 1));

            _activeBrush = displayer.Canvas.CreateSolidColorBrush(new ColorF(0, 0, 1));
        }

        private List<RectF> CalculateRects()
        {
            List<RectF> ret = new List<RectF>();

            foreach (RectF headerRect in _displayer.HeaderRects)
            {
                Point2F leftTopPoint = new Point2F(headerRect.Left, leftTop.Y);
                RectF rect = Tools.MoveRectToPoint(headerRect, leftTopPoint);
                ret.Add(rect);
            }

            return ret;
        }

        private RectF CalculateActiveRect()
        {
            return new RectF(_rects[0].Left, _rects[0].Top, _rects[_rects.Count - 1].Right, _rects[_rects.Count-1].Bottom);
        }

        public override void DisplayTarget()
        {
            for(int i = 0; i < _rects.Count; i++)
            {
                if(!((TargetTrack)target).Active)
                {
                    _displayer.Canvas.DrawRectangle(_rects[i], _borderBrush, 2);
                    _displayer.Canvas.DrawText(_texts[i], _inactiveTextFormat, _rects[i], _inactiveBrush);
                }
                else
                {
                    _displayer.Canvas.FillRectangle(_rects[i], _activeBrush);
                    _displayer.Canvas.DrawRectangle(_rects[i], _borderBrush, 2);
                    _displayer.Canvas.DrawText(_texts[i], _inactiveTextFormat, _rects[i], _inactiveBrush);
                }
            }
        }

        public override bool HandleMouseClick(object p)
        {
            if(Tools.IsPointInRect((Point) p, activeRect))
            {
                ((TargetTrack) target).Active = !((TargetTrack) target).Active;
                return true;
            }

            return false;
        }

        public bool HandleMouseDoubleClick(object p)
        {
            if (Tools.IsPointInRect((Point)p, activeRect))
            {
                ((TargetTrack)target).Active = true;
                ((TargetTrack)target).Focus();
                return true;
            }

            return false;
        }
    }
}
