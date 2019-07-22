using RadarDisplayPackage;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TargetManagerPackage;

namespace RadarForm
{

    public partial class Form1 : Form, IControlStateObserver
    {
        private OverViewDisplayer ovd;
        private SideViewDisplayer svd;
        private TextDisplayer textDisplayer;
        private SystemController controller;

        public Form1()
        {
            InitializeComponent();
        }

        private void btn_distance_Click(object sender, EventArgs e)
        {
            if (ParseInt(tb_distance, out var dis))
                ovd.Distance = dis;
        }

        private void btn_height_Click(object sender, EventArgs e)
        {
            if (ParseInt(tb_height, out var height))
                svd.Distance = height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ovd = new OverViewDisplayer(panel1);
            svd = new SideViewDisplayer(pnl_sideView);
            controller = new SystemController(ovd);
            controller.ConnectDataSource("UDP", MakeIpAddressAndPortString());    //默认链接UDP数据
            //dgvd = new DataGridViewDisplayer(pnl_gridView);
            textDisplayer = new TextDisplayer(pnl_gridView);
            ovd.RegisterObserver(this);
            svd.Distance = 500;
            btn_WaveGate.Enabled = false;
            MouseWheel += OnMouseWheel;
            ShowTrackHeight();
            rb_5.Checked = true;
            FpgaCommunicator.IsAmplifierOpen = true;    //开发射
            FpgaCommunicator.CurrentRange = RangeType.Rt5;

            btn_distance_Click(null, null); //发送p显量程

        }

        private void ShowTrackHeight() => lab_trackHeight.Text = SystemController.GetTrackHeight().ToString("0.0");

        private void OnMouseWheel(object sender, MouseEventArgs mouseEventArgs)
        {
            if (mouseEventArgs.Delta > 0)
                SystemController.SetTrackHeight(SystemController.GetTrackHeight() + 5);
            else
                SystemController.SetTrackHeight(SystemController.GetTrackHeight() - 5);
            ShowTrackHeight();
        }

        private string MakeIpAddressAndPortString() => tb_ipAddress.Text + ":" + tb_port.Text;

        private void button1_Click(object sender, EventArgs e) => controller.ResetDisplayer();

        private void btn_antennaControl_Click(object sender, EventArgs e) => controller.SwitchToAntennaControlState();

        private void btn_zoom_Click(object sender, EventArgs e) => controller.SwitchToZoomState();

        private void btn_auto_Click(object sender, EventArgs e)
        {
            controller.DeleteActiveWaveGates();
            controller.TargetManagerDeleteActiveTarget();
        }

        private void btn_resetAntenna_Click(object sender, EventArgs e)
        {
            controller.AntennaSetRotationRate(RotateRate.Rpm5);
            controller.AntennaSetRotateDirection(RotateDirection.ClockWise);    //-1为顺时针
        }

        private void rb_auto_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_auto.Checked) return;
            controller.TargetManagerSwitchMode(rb_auto.Text);   //切换到自动模式
            controller.SwitchToZoomState();     //操作模式重置为放缩
            btn_WaveGate.Enabled = true;
        }

        private void rb_semiAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_semiAuto.Checked) return;
            controller.TargetManagerSwitchMode(rb_semiAuto.Text);
            controller.SwitchToZoomState();
            btn_WaveGate.Enabled = true;
        }

        private void rb_manual_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_manual.Checked) return;
            controller.TargetManagerSwitchMode(rb_manual.Text);
            controller.SwitchToZoomState();
            btn_WaveGate.Enabled = false;
        }

        private void rb_intelligent_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_intelligent.Checked) return;
            controller.TargetManagerSwitchMode(rb_intelligent.Text);
            controller.SwitchToZoomState();
            btn_WaveGate.Enabled = false;
        }

        private void btn_WaveGate_Click(object sender, EventArgs e) => controller.SwitchToWaveGateState();

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
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void 航机显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controller.SetDisplayTrackCourseStatus(航机显示ToolStripMenuItem.Checked);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //controller.AntennaSetRotationRate(0);
            //controller.AntennaSetZeroDegree();

            //Thread.Sleep(30000);    //等待半分钟

            new 天线归位(controller).ShowDialog();
            Environment.Exit(0);    //强制退出所有线程???
        }

        private void btn_browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                tb_filePath.Text = ofd.FileName;
            }
        }

        private void btn_start_Click(object sender, EventArgs e) => controller.ConnectDataSource("BIN", tb_filePath.Text);

        private void 检波门限ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var win = new 检波门限设置(controller);
            win.Show();
        }

        private void tb_distance_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
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
                return true;
            }
            catch
            {
                result = int.MaxValue;
                MessageBox.Show(@"输入的不是数字");
                return false;
            }
        }

        public static bool ParseFloat(TextBox tb, out float result)
        {
            tb.Focus();
            tb.SelectAll();
            try
            {
                result = float.Parse(tb.Text);
                return true;
            }
            catch
            {
                result = float.MaxValue;
                MessageBox.Show(@"输入的不是数字");
                return false;
            }
        }

        private void btn_startUDP_Click_1(object sender, EventArgs e) => controller.ConnectDataSource("UDP", MakeIpAddressAndPortString());

        private void btn_clockwise_Click(object sender, EventArgs e) => controller.AntennaSetRotateDirection(RotateDirection.ClockWise);

        private void btn_counterclockwise_Click(object sender, EventArgs e) => controller.AntennaSetRotateDirection(RotateDirection.CounterClockWise);

        private void btn_Rpm0_Click(object sender, EventArgs e) => controller.AntennaSetRotationRate(RotateRate.Rpm0);

        private void btn_Rpm2_Click(object sender, EventArgs e) => controller.AntennaSetRotationRate(RotateRate.Rpm2);

        private void btn_Rpm5_Click(object sender, EventArgs e) => controller.AntennaSetRotationRate(RotateRate.Rpm5);

        private void btn_Rpm10_Click(object sender, EventArgs e) => controller.AntennaSetRotationRate(RotateRate.Rpm10);

        private void btn_slower_Click(object sender, EventArgs e) => controller.DataSourceSpeedDown();

        private void btn_faster_Click(object sender, EventArgs e) => controller.DataSourceSpeedUp();

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 33) //PageUp
                btn_faster_Click(null, null);

            if (e.KeyValue == 34) //PageDown
                btn_slower_Click(null, null);
        }

        private void btn_largeSectionSweep_Click(object sender, EventArgs e) => controller.AntennaSetSectionSweepMode(100f, 260f);

        private void 方位角调整ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            方位角调整 form = new 方位角调整(controller);
            form.ShowDialog();
        }

        private void btn_pause_Click(object sender, EventArgs e) => controller.DataSourcePause();

        private void btn_resume_Click(object sender, EventArgs e) => controller.DataSourceResume();

        private void 原始视频ToolStripMenuItem1_Click(object sender, EventArgs e) => Clotter.ShouldShowOriginalVideo = 原始视频ToolStripMenuItem1.Checked;

        private void btn_Rpm20_Click(object sender, EventArgs e) => controller.AntennaSetRotationRate(RotateRate.Rpm20);

        private void btn_powerAmplifierCtrl_Click(object sender, EventArgs e)
        {
            if (btn_powerAmplifierCtrl.Text == @"开发射")
            {
                FpgaCommunicator.IsAmplifierOpen = true;
                btn_powerAmplifierCtrl.Text = @"关发射";
            }
            else if (btn_powerAmplifierCtrl.Text == @"关发射")
            {
                FpgaCommunicator.IsAmplifierOpen = false;
                btn_powerAmplifierCtrl.Text = @"开发射";
            }
            else
            {
                //no else
            }
            //FpgaCommunicator.CurrentRange = FpgaCommunicator.CurrentRange == RangeType.Rt5 ? RangeType.Rt11 : RangeType.Rt5;
            //this.Text = $"Form{FpgaCommunicator.CurrentRange.ToString()}";
            //FpgaCommunicator.IsAmplifierOpen = true;
            //FpgaCommunicator.SetFpgaMode();
        }
        private void rb_10_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_10.Checked) return;
            FpgaCommunicator.CurrentRange = RangeType.Rt11;
            //ovd.Distance = 10000;
        }

        private void rb_5_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_5.Checked) return;
            FpgaCommunicator.CurrentRange = RangeType.Rt5;
            //ovd.Distance = 5000;
        }

        private void rb_close_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_close.Checked) return;
            FpgaCommunicator.CurrentRange = RangeType.RtClose;
        }

        private void btn_AntennaZero_Click(object sender, EventArgs e) => FpgaCommunicator.SetCurrentAntennaAngleToZero();

        private void rb_CounterclockWise_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_CounterclockWise.Checked) return;
            controller.AntennaSetRotateDirection(RotateDirection.ClockWise);
        }

        private void rb_ClockWise_CheckedChanged(object sender, EventArgs e)
        {
            if (!rb_ClockWise.Checked) return;
            controller.AntennaSetRotateDirection(RotateDirection.CounterClockWise);
        }

        private void rb_Stop_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Stop.Checked)
                controller.AntennaSetRotationRate(RotateRate.Rpm0);
        }

        private void rb_2rpm_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_2rpm.Checked)
                controller.AntennaSetRotationRate(RotateRate.Rpm2);
        }

        private void rb_5rpm_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_5rpm.Checked)
                controller.AntennaSetRotationRate(RotateRate.Rpm5);
        }

        private void rb_10rmp_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_10rmp.Checked)
                controller.AntennaSetRotationRate(RotateRate.Rpm10);
        }

        private void rb_20rmp_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_20rmp.Checked)
                controller.AntennaSetRotationRate(RotateRate.Rpm20);
        }

        private void rb_toZero_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_toZero.Checked)
                controller.AntennaSetZeroDegree();
        }

        private void btn_send12_Click(object sender, EventArgs e)
        {
            TestDataSender.SendTrack12();
        }

        private void btn_send68_Click(object sender, EventArgs e)
        {
            TestDataSender.SendTrack68();
        }

        private void btn_delete12_Click(object sender, EventArgs e)
        {
            TestDataSender.DeleteTrack12();
        }

        private void btn_delete68_Click(object sender, EventArgs e)
        {
            TestDataSender.DeleteTrack68();
        }

        private void btn_AzAdjustment_Click(object sender, EventArgs e)
        {
            try
            {
                controller.SetAzAdjustment(float.Parse(tb_AzAdjustment.Text));
            }
            catch
            {
                MessageBox.Show(@"方位偏差格式错误，请输入一个浮点数");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            controller.AntennaSetZeroDegree();
        }
    }
}
