using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TargetManagerPackage;
using System.Threading;
using System.Data;

namespace RadarDisplayPackage
{
    public class DataGridViewDisplayer : TrackDisplayer
    {
        public DataGridViewDisplayer(Control c) : base(c)
        {
            Dgv = new DataGridView();

            SetDataGridViewFormat(Dgv); //设置格式

            c.Controls.Clear();
            c.Controls.Add(Dgv);

            timer.Enabled = false;

            targetsManager = new DataGridViewTargetViewManager(this);

            SetColors();
        }

        public DataGridView Dgv { get; set; }

        private void SetDataGridViewFormat(DataGridView dgv)
        {
            dgv.ReadOnly = true;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgv.BackgroundColor = System.Drawing.Color.Black;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Font = new System.Drawing.Font("宋体", 13);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgv.AllowUserToResizeRows = false;
            dgv.RowTemplate.Height = 2;

            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.Name = "ID";
            column.HeaderText = "批号";
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "az";
            column.HeaderText = "方位";
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "el";
            column.HeaderText = "仰角";
            dgv.Columns.Add(column);
            column = new DataGridViewTextBoxColumn();
            column.Name = "diatance";
            column.HeaderText = "距离";
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "height";
            column.HeaderText = "高度";
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.Name = "speed";
            column.HeaderText = "速度";
            dgv.Columns.Add(column);

            dgv.Dock = DockStyle.Fill;
            dgv.RowHeadersVisible = false;

            //dgv.Enabled = false;
        }

        private void SetColors()
        {
            Dgv.EnableHeadersVisualStyles = false;
            Dgv.ForeColor = System.Drawing.Color.Chartreuse;
            foreach (DataGridViewRow dr in Dgv.Rows)
            {
                dr.DefaultCellStyle.BackColor = System.Drawing.Color.Black;
            }

            foreach(DataGridViewColumn dc in Dgv.Columns)
            {
                dc.HeaderCell.Style.BackColor = System.Drawing.Color.Black;
                dc.HeaderCell.Style.ForeColor = System.Drawing.Color.Chartreuse;
            }
        }
    }
}