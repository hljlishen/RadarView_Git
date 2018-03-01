using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RadarDisplayPackage;

namespace RadarForm
{
    
    public partial class Form1 : Form, IControlStateObserver
    {
        OverViewDisplayer ovd;
        SideViewDisplayer svd;
        SystemController controller;
        DataGridViewDisplayer dgvd;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_distance_Click(object sender, EventArgs e)
        {
            int dis;
            if (ParseInt(tb_distance, out dis))
                ovd.Distance = dis;
            //tb_distance.SelectAll();
        }

        private void btn_height_Click(object sender, EventArgs e)
        {
            //svd.Distance = int.Parse(tb_height.Text);
            //tb_height.SelectAll();
            int height;
            if (ParseInt(tb_height, out height))
                svd.Distance = height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cb_antennaRotationRateN.SelectedIndex = 1;
            tb_angleBegin.Enabled = false;
            tb_angleEnd.Enabled = false;

            ovd = new OverViewDisplayer(panel1);
            svd = new SideViewDisplayer(pnl_sideView);
            controller = new SystemController(ovd);
            controller.ConnectDataSource("UDP", "2013");    //默认链接UDP数据
            dgvd = new DataGridViewDisplayer(pnl_gridView);
            ovd.RegisterObserver(this);
            svd.Distance = 1000;
            ovd.Distance = 4000;
            btn_WaveGate.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            controller.ResetDisplayer();
        }

        private void btn_antennaControl_Click(object sender, EventArgs e)
        {
            controller.SwitchToAntennaControlState();
        }

        private void btn_zoom_Click(object sender, EventArgs e)
        {
            controller.SwitchToZoomState();
        }

        private void btn_auto_Click(object sender, EventArgs e)
        {
            controller.DeleteActiveWaveGates();
            controller.TargetManagerDeleteActiveTarget();
        }

        private void btn_resetAntenna_Click(object sender, EventArgs e)
        {
            controller.AntennaSetRotationRate(5);
            controller.AntennaSetNormalSweepMode(-1);    //-1为顺时针
        }

        private void btn_antennaConfirmN_Click(object sender, EventArgs e)
        {
            controller.AntennaSetRotationRate(uint.Parse(cb_antennaRotationRateN.Text));
        }

        private void rb_section_CheckedChanged(object sender, EventArgs e)
        {
            tb_angleBegin.Enabled = rb_section.Checked;
            tb_angleEnd.Enabled = rb_section.Checked;
        }

        private void btn_sweepModeConfirm_Click(object sender, EventArgs e)
        {
            if(rb_section.Checked)
            {
                float begin;
                float end;

                try
                {
                    begin = float.Parse(tb_angleBegin.Text);
                    end = float.Parse(tb_angleEnd.Text);
                }
                catch
                {
                    MessageBox.Show("请输入数字！");
                    return;
                }

                controller.AntennaSetSectionSweepMode(begin, end);
            }

            if(rb_clockWise.Checked)
            {
                controller.AntennaSetNormalSweepMode(-1);
            }

            if(rb_counterClockWise.Checked)
            {
                controller.AntennaSetNormalSweepMode(1);
            }
            if(rb_antennaStop.Checked)
            {
                //controller.AntennaSetNormalSweepMode(0);
            }
        }

        private void rb_auto_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_auto.Checked)
            {
                controller.TargetManagerSwitchMode(rb_auto.Text);   //切换到自动模式
                controller.SwitchToZoomState();     //操作模式重置为放缩
                btn_WaveGate.Enabled = true;
            }
        }

        private void rb_semiAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_semiAuto.Checked)
            {
                controller.TargetManagerSwitchMode(rb_semiAuto.Text);
                controller.SwitchToZoomState();
                btn_WaveGate.Enabled = true;
            }
        }

        private void rb_manual_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_manual.Checked)
            { 
                controller.TargetManagerSwitchMode(rb_manual.Text);
                controller.SwitchToZoomState();
                btn_WaveGate.Enabled = false;
            }
        }

        private void rb_intelligent_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_intelligent.Checked)
            { 
                controller.TargetManagerSwitchMode(rb_intelligent.Text);
                controller.SwitchToZoomState();
                btn_WaveGate.Enabled = false;
            }
        }

        private void btn_WaveGate_Click(object sender, EventArgs e)
        {
            controller.SwitchToWaveGateState();
        }

        public void NotifyChange(OverViewState state)
        {
            switch (state)
            {
                case OverViewState.AntennaControl:
                    btn_antennaControl.BackColor = Color.Khaki;
                    btn_WaveGate.BackColor = Color.LightSteelBlue;
                    btn_zoom.BackColor = Color.LightSteelBlue;
                    break;
                case OverViewState.Zoom:
                    btn_antennaControl.BackColor = Color.LightSteelBlue;
                    btn_WaveGate.BackColor = Color.LightSteelBlue;
                    btn_zoom.BackColor = Color.Khaki;
                    break;
                case OverViewState.AutoWaveGate:
                    btn_antennaControl.BackColor = Color.LightSteelBlue;
                    btn_WaveGate.BackColor = Color.Khaki;
                    btn_zoom.BackColor = Color.LightSteelBlue;
                    break;
                case OverViewState.SemiAutoWaveGate:
                    btn_antennaControl.BackColor = Color.LightSteelBlue;
                    btn_WaveGate.BackColor = Color.Khaki;
                    btn_zoom.BackColor = Color.LightSteelBlue;
                    break;
                default:
                    break;
            }
        }

        private void 航机显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //controller.AntennaSetNormalSweepMode(0);
            controller.AntennaSetRotationRate(0);
            Environment.Exit(0);    //强制退出所有线程???
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(DialogResult.OK == ofd.ShowDialog())
            {
                tb_filePath.Text = ofd.FileName;
            }
        }

        private void btn_startUDP_Click(object sender, EventArgs e)
        {
            controller.ConnectDataSource("UDP", "2013");
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            controller.ConnectDataSource("BIN", tb_filePath.Text);
        }

        private void 检波门限ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            检波门限设置 win = new 检波门限设置(controller);
            win.Show();
        }

        private void tb_distance_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
                btn_distance_Click(sender, null);
        }

        private void tb_height_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_height_Click(sender, null);
        }

        public static bool ParseInt(TextBox tb, out int result)
        {
            tb.Focus();
            tb.SelectAll();
            try
            {
                result = int.Parse(tb.Text);
                return  true;
            }
            catch
            {
                result = int.MaxValue;
                MessageBox.Show("输入的不是数字");
                return false;
            }
        }
    }
}
