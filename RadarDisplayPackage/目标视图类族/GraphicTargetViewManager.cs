using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class GraphicTargetViewManager :TargetViewManager
    {
        private BitmapRenderTarget[] _sectorDrawer;

        public GraphicTargetViewManager(GraphicTrackDisplayer displayer) : base(displayer)
        {
            //鼠标点击事件
            displayer.DisplayControl.MouseClick += DisplayControl_MouseClick;

            InitializeBitmaps();

            //读取所有目标
            LoadTargetViews(targetProvider.GetTargetTracks());
            LoadTargetViews(targetProvider.GetTargetDots());

            DrawSectors();
        }

        private void InitializeBitmaps()
        {
            //创建内存位图
            int count = targetProvider.GetSectorCount();
            _sectorDrawer = new BitmapRenderTarget[count];
            for (int i = 0; i < count; i++)
            {
                _sectorDrawer[i] = ((GraphicTrackDisplayer)displayer).Canvas.CreateCompatibleRenderTarget();
            }
        }

        private void DisplayControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) return;
            bool stopFindingDot = false;
            lock (_locker)
            {
                //鼠标点击事件转发给每个视图对象处理,倒叙遍历，因为遍历过程中会有航迹的删除和添加
                for (int i = dots.Length - 1; i >= 0; i--)
                {
                    for (int j = dots[i].Count - 1; j >= 0; j--)
                    {
                        if (dots[i][j].HandleMouseClick(e.Location)) //点击位置附近有多个目标时，只选取一个目标
                        {
                            stopFindingDot = true;
                            break;
                        }  
                    }

                    if (stopFindingDot)
                        break;
                }

                for (int i = tracks.Length - 1; i >= 0; i--)
                {
                    for (int j = tracks[i].Count - 1; j >= 0; j--)
                    {
                        if(tracks[i][j].HandleMouseClick(e.Location))
                            return;
                    }
                }

                //运行到此处说明没有点击位置没有任何目标,为MouseTargetTracker生成一个目标点
                GraphicTargetDotView view = GenerateGraphicTargetDotViewFromMouseLocation(e.Location);
                view.HandleMouseClick(e.Location);
            }
        }

        private GraphicTargetDotView GenerateGraphicTargetDotViewFromMouseLocation(Point mouseLocation)
        {
            PolarCoordinate coordinate =
                ((GraphicTrackDisplayer)displayer).coordinateSystem.PointToCoordinate(CoordinateSystem.PointToPoint2F(mouseLocation));
            TargetDot dot = new TargetDot(coordinate.Az, coordinate.El, coordinate.Dis) { sectorIndex = 0 };
            TargetView view = ((GraphicTrackDisplayer)displayer).targetsManager.CreateTargetView(dot);

            return (GraphicTargetDotView)view;
        }

        public override TargetView CreateTargetView(Target taget)
        {
            GraphicTargetView view;
            if (taget == null)
                return null;
            if (taget.GetType() == typeof(TargetDot))
                view = new GraphicTargetDotView(taget, _sectorDrawer[taget.sectorIndex], ((GraphicTrackDisplayer)displayer).Factory, ((GraphicTrackDisplayer)displayer).coordinateSystem);
            else
                view = new GraphicTargetTrackView(taget, _sectorDrawer[taget.sectorIndex], ((GraphicTrackDisplayer)displayer).Factory, ((GraphicTrackDisplayer)displayer).coordinateSystem);
            return view;
        }

        protected sealed override void LoadTargetViews(List<Target> tracks)
        {
            if (tracks == null)
                return;
            lock (_locker)
            {
                foreach (Target target in tracks)
                {
                    //获取目标航迹视图
                    var view = CreateTargetView(target);
                    if (target is TargetDot)
                        dots[target.sectorIndex].Add(view);
                    else
                        this.tracks[target.sectorIndex].Add(view);
                }
            }
        }

        public override void DisplayTargetViews()
        {
            base.DisplayTargetViews();
            foreach(BitmapRenderTarget brt in _sectorDrawer)     //绘制位图
            {
                ((GraphicTrackDisplayer)displayer).Canvas.DrawBitmap(brt.Bitmap);
            }
        }

        public override void NotifyUpdateSectorDot(List<TargetDot> dots, int sectorIndex)
        {
            base.NotifyUpdateSectorDot(dots, sectorIndex);

            DrawSector(sectorIndex);
        }

        public override void NotifyUpdateSectorTrack(List<TargetTrack> tracks, int sectorIndex)
        {
            base.NotifyUpdateSectorTrack(tracks, sectorIndex);

            DrawSector(sectorIndex);
        }

        private void DrawSector(int sectorIndex)
        {
            lock (_locker)
            {
                try
                {
                    _sectorDrawer[sectorIndex].BeginDraw();
                    _sectorDrawer[sectorIndex].Clear();

                    //绘制目标点
                    foreach (var view in dots[sectorIndex])
                        view.DisplayTarget();

                    //绘制目标航迹
                    foreach (var view in tracks[sectorIndex])
                        view.DisplayTarget();

                    _sectorDrawer[sectorIndex].EndDraw();
                }
                catch
                {
                    //ignore
                    //MessageBox.Show(@"GraphicTargetViewManager.DrawSector错误");
                }
            }
        }

        private void DrawSectors()
        {
            for(int i =0; i < _sectorDrawer.Length ; i++)
            {
                DrawSector(i);
            }
        }

        protected override void AddTarget(Target t)
        {
            base.AddTarget(t);
            DrawSector(t.sectorIndex);

        }

        protected override void RemoveTarget(Target t)
        {
            base.RemoveTarget(t);
            DrawSector(t.sectorIndex);
        }

        public override void Dispose()
        {
            lock (_locker)
            {
                base.Dispose();
                foreach (List<TargetView> ls in dots)
                {
                    foreach (var view in ls)
                    {
                        view.Dispose();
                    }
                }

                foreach (List<TargetView> ls in tracks)
                {
                    foreach (var targetView in ls)
                    {
                        var view = (GraphicTargetTrackView)targetView;
                        view.Dispose();
                    }
                }
                displayer.DisplayControl.MouseClick -= DisplayControl_MouseClick;
            }
        }
    }
}
