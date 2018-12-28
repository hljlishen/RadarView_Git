using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TargetManagerPackage;

namespace RadarDisplayPackage
{
    public class DataGridViewDisplayer : TrackDisplayer
    {
        protected delegate void UpdateTracksHandler(List<TargetTrack> tracks, int sectorIndex);

        private int currentRowIndex = 0;
        private const int TrackMaxCount = 200;
        private List<TargetTrack>[] tracks;

        public DataGridViewDisplayer(Control c) : base(c)
        {
            tracks = new List<TargetTrack>[TargetManagerFactory.CreateTargetDataProvider().GetSectorCount()];

            for(int i = 0; i < tracks.Length; i++)
                tracks[i] = new List<TargetTrack>();

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
            dgv.BackgroundColor = Color.Black;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.Font = new Font("宋体", 13);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;
            dgv.AllowUserToResizeRows = false;
            dgv.RowTemplate.Height = 2;

            DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
            column.Name = "ID";
            column.HeaderText = @"批号";
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                Name = "az",
                HeaderText = @"方位"
            };
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                Name = "el",
                HeaderText = @"仰角"
            };
            dgv.Columns.Add(column);
            column = new DataGridViewTextBoxColumn
            {
                Name = "diatance",
                HeaderText = @"距离"
            };
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                Name = "height",
                HeaderText = @"高度"
            };
            dgv.Columns.Add(column);

            column = new DataGridViewTextBoxColumn
            {
                Name = "speed",
                HeaderText = @"速度"
            };
            dgv.Columns.Add(column);

            dgv.Dock = DockStyle.Fill;
            dgv.RowHeadersVisible = false;

            for (int i = 0; i < TrackMaxCount; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(Dgv);
                dgv.Rows.Add(row);
            }

            //dgv.Enabled = false;
        }

        private void SetColors()
        {
            Dgv.EnableHeadersVisualStyles = false;
            Dgv.ForeColor = Color.Chartreuse;
            foreach (DataGridViewRow dr in Dgv.Rows)
            {
                dr.DefaultCellStyle.BackColor = Color.Black;
            }

            foreach(DataGridViewColumn dc in Dgv.Columns)
            {
                dc.HeaderCell.Style.BackColor = Color.Black;
                dc.HeaderCell.Style.ForeColor = Color.Chartreuse;
            }
        }

        public void UpdateTargetTracks(List<TargetTrack> trackList, int sectorIndex)
        {
            Dgv.Invoke(new UpdateTracksHandler(UpdateTracks), new object[] { trackList, sectorIndex });
        }

        private void UpdateTracks(List<TargetTrack> trackList, int sectorIndex)
        {
            DeleteSectorTracks(sectorIndex);

            if (trackList == null) return;
            tracks[sectorIndex].AddRange(trackList);

            foreach (var targetTrack in trackList)
            {
                Dgv.Rows[currentRowIndex].Cells["ID"].Value = targetTrack.TrackId;
                Dgv.Rows[currentRowIndex].Cells["az"].Value = targetTrack.Az;
                Dgv.Rows[currentRowIndex].Cells["el"].Value = targetTrack.El;
                currentRowIndex = NextAvailableIndex();
            }
        }

        private int NextAvailableIndex()
        {
            foreach (var dgvRow in Dgv.Rows)
            {
                DataGridViewRow row = (DataGridViewRow)dgvRow;
                if (row.Cells["ID"].Value == null || row.Cells["ID"].Value.ToString() == "")
                {
                    return row.Index;
                }
            }

            return -1;
        }

        private void DeleteSectorTracks(int sectorIndex)
        {
            foreach (TargetTrack targetTrack in tracks[sectorIndex])
            {
                foreach (var dgvRow in Dgv.Rows)
                {
                    DataGridViewRow row = (DataGridViewRow)dgvRow;
                    if (row.Cells["ID"].Value == null || row.Cells["ID"].Value.ToString() == "") continue;

                    if ((int) row.Cells["ID"].Value == targetTrack.TrackId)
                    {
                        ClearRow(row);
                        //currentRowIndex--;
                    }
                }
            }

            tracks[sectorIndex].Clear();
        }

        private void ClearRow(DataGridViewRow row)
        {
            foreach (var rowCell in row.Cells)
            {
                ((DataGridViewCell)rowCell).Value = "";
            }

            //for (int i = row.Index + 1; i < Dgv.Rows.Count-1; i++)
            //{
            //    //if (Dgv.Rows[i].Cells["ID"].Value == null || Dgv.Rows[i].Cells["ID"].Value == "")
            //    //    break;
            //    for (int j = 0; j < row.Cells.Count ; j++)
            //    {
            //        Dgv.Rows[i].Cells[j].Value = Dgv.Rows[i + 1].Cells[j].Value;
            //        //ClearRow(Dgv.Rows[i + 1]);
            //    }
            //}
        }
    }
}