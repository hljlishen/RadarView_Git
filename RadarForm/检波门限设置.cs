using RadarDisplayPackage;
using System;
using System.Windows.Forms;

namespace RadarForm
{
    public partial class 检波门限设置 : Form
    {
        private readonly SystemController _controller;
        public 检波门限设置(SystemController controller)
        {
            InitializeComponent();
            _controller = controller;
        }

        private void 检波门限设置_Load(object sender, EventArgs e)
        {
            //获取当前设置的值
            tb_amThreshold.Text = _controller.GetCycleDataFilterAmThreshold().ToString();
            tb_speedMinimum.Text = _controller.GetCycleDataFilterSpeedMinimum().ToString();
            tb_speedMaximum.Text = _controller.GetCycleDataFilterSpeedMaximum().ToString();
            tb_height.Text = _controller.GetCycleDataFilterHeightThreshold().ToString();
        }

        private void btn_speedConfirm_Click(object sender, EventArgs e)
        {
            if (Form1.ParseInt(tb_speedMinimum, out var speed))
                _controller.SetCycleDataFilterSpeedMinimum(speed);
        }

        private void btn_amConfirm_Click(object sender, EventArgs e)
        {
            if (Form1.ParseInt(tb_amThreshold, out var am))
                _controller.SetCycleDataFilterAmThreshold(am);
        }

        private void btn_speedDown_Click(object sender, EventArgs e)
        {
            tb_speedMinimum.Text = StringPlusInt(tb_speedMinimum.Text, -1);
            _controller.SetCycleDataFilterSpeedMinimum(int.Parse(tb_speedMinimum.Text));
        }

        private void btn_speedUp_Click(object sender, EventArgs e)
        {
            tb_speedMinimum.Text = StringPlusInt(tb_speedMinimum.Text, 1);
            _controller.SetCycleDataFilterSpeedMinimum(int.Parse(tb_speedMinimum.Text));
        }

        private void btn_amDown_Click(object sender, EventArgs e)
        {
            tb_amThreshold.Text = StringPlusInt(tb_amThreshold.Text, -2);
            _controller.SetCycleDataFilterAmThreshold(int.Parse(tb_amThreshold.Text));
        }

        private void btn_amUp_Click(object sender, EventArgs e)
        {
            tb_amThreshold.Text = StringPlusInt(tb_amThreshold.Text, 2);
            _controller.SetCycleDataFilterAmThreshold(int.Parse(tb_amThreshold.Text));
        }

        private void tb_speedThreshold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_speedConfirm_Click(null, e);
        }

        private void tb_amThreshold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_amConfirm_Click(null, e);
        }

        private void btn_speedMaximumConfirm_Click(object sender, EventArgs e)
        {
            if (Form1.ParseInt(tb_speedMaximum, out var speed))
                _controller.SetCycleDataFilterSpeedMaximum(speed);
        }

        private void tb_speedMaximum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_speedMaximumConfirm_Click(null, e);
        }

        private void btn_speedMaximumUp_Click(object sender, EventArgs e)
        {
            tb_speedMaximum.Text = StringPlusInt(tb_speedMaximum.Text, 1);
            _controller.SetCycleDataFilterSpeedMaximum(int.Parse(tb_speedMaximum.Text));
        }

        private void btn_speedMaximumDown_Click(object sender, EventArgs e)
        {
            tb_speedMaximum.Text = StringPlusInt(tb_speedMaximum.Text, -1);
            _controller.SetCycleDataFilterSpeedMaximum(int.Parse(tb_speedMaximum.Text));
        }

        private string StringPlusInt(string value, int plus)
        {
            try
            {
                var speed = int.Parse(value);
                speed += plus;
                return speed.ToString();
            }
            catch
            {
                MessageBox.Show("请输入数字");
                return plus.ToString();
            }
        }

        private void btn_height_Click(object sender, EventArgs e)
        {
            if (Form1.ParseInt(tb_height, out var height))
                _controller.SetCycleDataFilterHeightMinimum(height);
        }

        private void btn_heightDown_Click(object sender, EventArgs e)
        {
            tb_height.Text = StringPlusInt(tb_height.Text, -1);
            _controller.SetCycleDataFilterHeightMinimum(int.Parse(tb_height.Text));
        }

        private void btn_heightUp_Click(object sender, EventArgs e)
        {
            tb_height.Text = StringPlusInt(tb_height.Text, 1);
            _controller.SetCycleDataFilterHeightMinimum(int.Parse(tb_height.Text));
        }
    }
}
