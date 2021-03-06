﻿using CycleDataDrivePackage;
using System;
using System.Windows.Forms;

namespace RadarForm
{
    public partial class 方位角调整 : Form
    {
        public 方位角调整()
        {
            InitializeComponent();
        }

        private void 方位角调整_Load(object sender, EventArgs e)
        {
            tb_azAngleAdjustment.Text = CycleDataReader.AzAdjustment.ToString("0.00");
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            if (!Form1.ParseFloat(tb_azAngleAdjustment, out var adjustment))
            {
                MessageBox.Show(@"请输入数字");
                return;
            }
            CycleDataReader.AzAdjustment = adjustment;
            Close();
        }

        private void tb_azAngleAdjustment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btn_confirm_Click(null, null);
        }
    }
}
