using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;
using System.Windows.Forms;

namespace RadarDisplayPackage
{
    class DataGridViewTargetViewManager : TargetViewManager
    {
        protected DataGridViewTargetView[] views;
        protected const int TrackMaximum = 16;

        public DataGridViewTargetViewManager(DataGridViewDisplayer displayer) : base(displayer)
        {
            views = new DataGridViewTargetView[TrackMaximum];   //最大16个航迹

            for (int i = 0; i < TrackMaximum; i++)
            {
                views[i] = new DataGridViewTargetView(null, displayer, i + 1);
                views[i].Target = null;         //赋值null,可以隐藏该行
                views[i].DisplayTarget();       //向displayer.dgv添加行
                views[i].Selected = false;      //默认未被选中
            }

            LoadTargetViews(targetProvider.GetTargetTracks());
        }


        public override void DisplayTargetViews()  //不定时刷新航迹信息，只有AddTarget，RemoveTarget和UpdateTarget时才刷新列表 
        {
        }

        protected override void AddTarget(Target t)
        {
            if (t is TargetTrack)
            {
                int Id = ((TargetTrack)t).trackID;
                views[Id - 1].Target = t;
                if (t.active)
                    views[Id - 1].Selected = true;
                else
                    views[Id - 1].Selected = false;
            }
        }

        protected override void RemoveTarget(Target t)
        {
            if (t is TargetTrack)
            {
                int Id = ((TargetTrack)t).trackID;
                views[Id - 1].Target = null;
                views[Id - 1].Selected = false;
            }
        }

        protected override void UpDateTarget(Target t)
        {
            AddTarget(t);
        }

        protected override void LoadTargetViews(List<Target> ls)
        {
            foreach (Target t in ls)
                AddTarget(t);
        }
    }
}
