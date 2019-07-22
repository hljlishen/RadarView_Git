using RadarDisplayPackage;
using System;
using System.Threading;
using System.Windows.Forms;

namespace RadarForm
{
    public partial class 天线归位 : Form
    {
        private SystemController controller;
        public 天线归位(SystemController ctrl)
        {
            InitializeComponent();
            controller = ctrl;
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 天线归位_Load(object sender, EventArgs e)
        {

        }

        private void ReturnToZero_btn_Click(object sender, EventArgs e)
        {
            label1.Text = "正在归位天线，30秒后自动关闭\r\n请勿结束进程！";
            Update();
            controller.AntennaSetZeroDegree();

            Thread.Sleep(30000);    //等待半分钟
            Close();
        }
    }
}
