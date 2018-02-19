using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TargetManagerPackage;
using System.Windows.Forms;
using System.Data;

namespace RadarDisplayPackage
{
    class DataGridViewTargetView : TargetView
    {
        DataGridViewDisplayer displayer;
        DataGridViewRow dr;
        int ID;

        public bool Selected
        {
            get
            {
                return dr.Selected;
            }
            set
            {
                dr.Selected = value;
            }
        }

        public DataGridViewTargetView(TargetTrack t, DataGridViewDisplayer displayer, int id) : base(t)
        {
            this.displayer = displayer;
            ID = id;
            dr = new DataGridViewRow();
            dr.CreateCells(displayer.Dgv);
            if (t != null)
            {
                dr.Cells[0].Value = ID;
                dr.Cells[1].Value = t.AZ.ToString("0.0");
                dr.Cells[2].Value = t.EL.ToString("0.0");
                dr.Cells[3].Value = t.Height.ToString("0.0");
                dr.Cells[4].Value = t.speed.ToString("0.0");
                dr.Cells[5].Value = t.score;
            }
            else
            {
                dr.Cells[0].Value = ID;
                dr.Cells[1].Value = "";
                dr.Cells[2].Value = "";
                dr.Cells[3].Value = "";
                dr.Cells[4].Value = "";
                dr.Cells[5].Value = "";
            }
        }

        public override void DisplayTarget()    //DataGridView的DisplayTarget方法只有DataGridViewTargetViewManager初始化时调用
        {
            displayer.Dgv.Rows.Add(dr);
        }

        public override Target Target
        {
            get
            {
                return base.Target;
            }

            set
            {
                base.Target = value;
                if (value != null)
                {
                    //dr.Cells[0].Value = ((TargetTrack)target).trackID;
                    dr.Cells[1].Value = target.AZ.ToString("0.0");
                    dr.Cells[2].Value = target.EL.ToString("0.0");
                    dr.Cells[3].Value = target.Dis.ToString("0.0");
                    dr.Cells[4].Value = target.Height.ToString("0.0");
                    dr.Cells[5].Value = ((TargetTrack)target).speed.ToString("0.0");
                    //dr.Visible = true;
                }
                else  //给target赋值null表示需要删除改目标航迹,只留下ID号，其余信息删除
                {
                    //dr.Cells[0].Value = ((TargetTrack)target).trackID;
                    dr.Cells[1].Value = "";
                    dr.Cells[2].Value = "";
                    dr.Cells[3].Value = "";
                    dr.Cells[4].Value = "";
                    dr.Cells[5].Value = "";
                    //dr.Visible = false;
                }
            }
        }
    }
}
