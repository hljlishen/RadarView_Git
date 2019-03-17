using System;
using System.Windows.Forms;
using RadarDisplayPackage;

namespace RadarForm
{
    public partial class 方位角调整 : Form
    {
        SystemController controller;
        public 方位角调整(SystemController controller)
        {
            InitializeComponent();
            this.controller = controller;
        }

        private void 方位角调整_Load(object sender, EventArgs e)
        {
            tb_azAngleAdjustment.Text = controller.GetAzAdjustment().ToString("0.00");
            tb_elAdjustment.Text = controller.GetElAdjustment().ToString("0.00");
            tb_antennaStopDegree.Text = controller.GetAntennaStopDegree().ToString("0.00");
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (!Form1.ParseFloat(tb_azAngleAdjustment, out var adjustment))
            {
                MessageBox.Show(@"请输入数字");
                return;
            }
            controller.SetAzAdjustment(adjustment);
        }

        private void tb_azAngleAdjustment_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //    btn_confirm_Click(null, null);
        }

        private void btn_elConfirm_Click(object sender, EventArgs e)
        {
            if (!Form1.ParseFloat(tb_elAdjustment, out var adjustment))
            {
                MessageBox.Show(@"请输入数字");
                return;
            }
            controller.SetElAdjustment(adjustment);
        }

        private void btn_confirmAntennaStopDegree_Click(object sender, EventArgs e)
        {
            if (!Form1.ParseFloat(tb_antennaStopDegree, out var adjustment))
            {
                MessageBox.Show(@"请输入数字");
                return;
            }
            controller.SetAntennaStopDegree(adjustment);
        }
    }
}
