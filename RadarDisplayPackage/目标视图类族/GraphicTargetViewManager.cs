using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    class GraphicTargetViewManager :TargetViewManager
    {
        BitmapRenderTarget[] sectorDrawer;

        public GraphicTargetViewManager(GraphicTrackDisplayer displayer) : base(displayer)
        {
            //鼠标点击事件
            displayer.DisplayControl.MouseDoubleClick += DisplayControl_MouseClick;

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
            sectorDrawer = new BitmapRenderTarget[count];
            for (int i = 0; i < count; i++)
            {
                sectorDrawer[i] = ((GraphicTrackDisplayer)displayer).Canvas.CreateCompatibleRenderTarget();
            }
        }

        private void DisplayControl_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            lock (this)
            {
                //鼠标点击事件转发给每个视图对象处理,倒叙遍历，因为遍历过程中会有航迹的删除和添加
                for (int i = dots.Length - 1; i >= 0; i--)
                {
                    for (int j = dots[i].Count - 1; j >= 0; j--)
                    {
                        dots[i][j].HandleMouseClick(e.Location);
                    }
                }

                for (int i = tracks.Length - 1; i >= 0; i--)
                {
                    for (int j = tracks[i].Count - 1; j >= 0; j--)
                    {
                        tracks[i][j].HandleMouseClick(e.Location);
                    }
                }
            }
        }

        protected override TargetView CreateTargetView(Target taget)
        {
            GraphicTargetView view;
            if (taget == null)
                return null;
            if (taget.GetType() == typeof(TargetDot))
                view = new GraphicTargetDotView(taget, sectorDrawer[taget.sectorIndex], ((GraphicTrackDisplayer)displayer).Factory, ((GraphicTrackDisplayer)displayer).coordinateSystem);
            else
                view = new GraphicTargetTrackView(taget, sectorDrawer[taget.sectorIndex], ((GraphicTrackDisplayer)displayer).Factory, ((GraphicTrackDisplayer)displayer).coordinateSystem);
            return view;
        }

        protected override void LoadTargetViews(List<Target> tracks)
        {
            //获取目标航迹视图
            foreach (Target target in tracks)
            {
                var view = CreateTargetView(target);
                if (target is TargetDot)
                    dots[target.sectorIndex].Add(view);
                else
                    lock (this)
                    {
                        this.tracks[target.sectorIndex].Add(view);
                    }
            }
        }

        public override void DisplayTargetViews()
        {
            base.DisplayTargetViews();
            foreach(BitmapRenderTarget brt in sectorDrawer)     //绘制位图
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
            lock (this)
            {
                try
                {
                    sectorDrawer[sectorIndex].BeginDraw();
                    sectorDrawer[sectorIndex].Clear();

                    //绘制目标点
                    foreach (GraphicTargetView view in dots[sectorIndex])
                    {
                        view.DisplayTarget();
                    }

                    //绘制目标航迹
                    foreach (GraphicTargetView view in tracks[sectorIndex])
                    {
                        view.DisplayTarget();
                    }

                    sectorDrawer[sectorIndex].EndDraw();
                }
                catch
                {
                    //ignore
                }
            }
        }

        private void DrawSectors()
        {
            for(int i =0; i < sectorDrawer.Length ; i++)
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
            base.Dispose();
            foreach (List<TargetView> ls in dots)
            {
                foreach (GraphicTargetDotView view in ls)
                {
                    view.Dispose();
                }
            }

            foreach (List<TargetView> ls in tracks)
            {
                foreach (GraphicTargetTrackView view in ls)
                {
                    view.Dispose();
                }
            }
            displayer.DisplayControl.MouseClick -= DisplayControl_MouseClick;
        }
    }
}
