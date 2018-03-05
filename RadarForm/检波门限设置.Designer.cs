namespace RadarForm
{
    partial class 检波门限设置
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tb_speedMinimum = new System.Windows.Forms.TextBox();
            this.btn_speedConfirm = new System.Windows.Forms.Button();
            this.tb_amThreshold = new System.Windows.Forms.TextBox();
            this.btn_amConfirm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_speedDown = new System.Windows.Forms.Button();
            this.btn_speedUp = new System.Windows.Forms.Button();
            this.btn_amDown = new System.Windows.Forms.Button();
            this.btn_amUp = new System.Windows.Forms.Button();
            this.tb_speedMaximum = new System.Windows.Forms.TextBox();
            this.btn_speedMaximumConfirm = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_speedMaximumDown = new System.Windows.Forms.Button();
            this.btn_speedMaximumUp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tb_speedMinimum
            // 
            this.tb_speedMinimum.Location = new System.Drawing.Point(112, 27);
            this.tb_speedMinimum.Name = "tb_speedMinimum";
            this.tb_speedMinimum.Size = new System.Drawing.Size(100, 21);
            this.tb_speedMinimum.TabIndex = 0;
            this.tb_speedMinimum.Text = "1";
            this.tb_speedMinimum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_speedThreshold_KeyDown);
            // 
            // btn_speedConfirm
            // 
            this.btn_speedConfirm.Location = new System.Drawing.Point(235, 25);
            this.btn_speedConfirm.Name = "btn_speedConfirm";
            this.btn_speedConfirm.Size = new System.Drawing.Size(75, 50);
            this.btn_speedConfirm.TabIndex = 1;
            this.btn_speedConfirm.Text = "设置";
            this.btn_speedConfirm.UseVisualStyleBackColor = true;
            this.btn_speedConfirm.Click += new System.EventHandler(this.btn_speedConfirm_Click);
            // 
            // tb_amThreshold
            // 
            this.tb_amThreshold.Location = new System.Drawing.Point(112, 186);
            this.tb_amThreshold.Name = "tb_amThreshold";
            this.tb_amThreshold.Size = new System.Drawing.Size(100, 21);
            this.tb_amThreshold.TabIndex = 0;
            this.tb_amThreshold.Text = "20";
            this.tb_amThreshold.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_amThreshold_KeyDown);
            // 
            // btn_amConfirm
            // 
            this.btn_amConfirm.Location = new System.Drawing.Point(235, 186);
            this.btn_amConfirm.Name = "btn_amConfirm";
            this.btn_amConfirm.Size = new System.Drawing.Size(75, 48);
            this.btn_amConfirm.TabIndex = 1;
            this.btn_amConfirm.Text = "设置";
            this.btn_amConfirm.UseVisualStyleBackColor = true;
            this.btn_amConfirm.Click += new System.EventHandler(this.btn_amConfirm_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(39, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "速度下限";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 189);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "幅度门限";
            // 
            // btn_speedDown
            // 
            this.btn_speedDown.Location = new System.Drawing.Point(112, 54);
            this.btn_speedDown.Name = "btn_speedDown";
            this.btn_speedDown.Size = new System.Drawing.Size(46, 21);
            this.btn_speedDown.TabIndex = 3;
            this.btn_speedDown.Text = "<<";
            this.btn_speedDown.UseVisualStyleBackColor = true;
            this.btn_speedDown.Click += new System.EventHandler(this.btn_speedDown_Click);
            // 
            // btn_speedUp
            // 
            this.btn_speedUp.Location = new System.Drawing.Point(166, 54);
            this.btn_speedUp.Name = "btn_speedUp";
            this.btn_speedUp.Size = new System.Drawing.Size(46, 21);
            this.btn_speedUp.TabIndex = 3;
            this.btn_speedUp.Text = ">>";
            this.btn_speedUp.UseVisualStyleBackColor = true;
            this.btn_speedUp.Click += new System.EventHandler(this.btn_speedUp_Click);
            // 
            // btn_amDown
            // 
            this.btn_amDown.Location = new System.Drawing.Point(112, 213);
            this.btn_amDown.Name = "btn_amDown";
            this.btn_amDown.Size = new System.Drawing.Size(46, 21);
            this.btn_amDown.TabIndex = 3;
            this.btn_amDown.Text = "<<";
            this.btn_amDown.UseVisualStyleBackColor = true;
            this.btn_amDown.Click += new System.EventHandler(this.btn_amDown_Click);
            // 
            // btn_amUp
            // 
            this.btn_amUp.Location = new System.Drawing.Point(166, 213);
            this.btn_amUp.Name = "btn_amUp";
            this.btn_amUp.Size = new System.Drawing.Size(46, 21);
            this.btn_amUp.TabIndex = 3;
            this.btn_amUp.Text = ">>";
            this.btn_amUp.UseVisualStyleBackColor = true;
            this.btn_amUp.Click += new System.EventHandler(this.btn_amUp_Click);
            // 
            // tb_speedMaximum
            // 
            this.tb_speedMaximum.Location = new System.Drawing.Point(112, 104);
            this.tb_speedMaximum.Name = "tb_speedMaximum";
            this.tb_speedMaximum.Size = new System.Drawing.Size(100, 21);
            this.tb_speedMaximum.TabIndex = 0;
            this.tb_speedMaximum.Text = "1";
            this.tb_speedMaximum.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_speedMaximum_KeyDown);
            // 
            // btn_speedMaximumConfirm
            // 
            this.btn_speedMaximumConfirm.Location = new System.Drawing.Point(235, 102);
            this.btn_speedMaximumConfirm.Name = "btn_speedMaximumConfirm";
            this.btn_speedMaximumConfirm.Size = new System.Drawing.Size(75, 50);
            this.btn_speedMaximumConfirm.TabIndex = 1;
            this.btn_speedMaximumConfirm.Text = "设置";
            this.btn_speedMaximumConfirm.UseVisualStyleBackColor = true;
            this.btn_speedMaximumConfirm.Click += new System.EventHandler(this.btn_speedMaximumConfirm_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "速度上限";
            // 
            // btn_speedMaximumDown
            // 
            this.btn_speedMaximumDown.Location = new System.Drawing.Point(112, 131);
            this.btn_speedMaximumDown.Name = "btn_speedMaximumDown";
            this.btn_speedMaximumDown.Size = new System.Drawing.Size(46, 21);
            this.btn_speedMaximumDown.TabIndex = 3;
            this.btn_speedMaximumDown.Text = "<<";
            this.btn_speedMaximumDown.UseVisualStyleBackColor = true;
            this.btn_speedMaximumDown.Click += new System.EventHandler(this.btn_speedMaximumDown_Click);
            // 
            // btn_speedMaximumUp
            // 
            this.btn_speedMaximumUp.Location = new System.Drawing.Point(166, 131);
            this.btn_speedMaximumUp.Name = "btn_speedMaximumUp";
            this.btn_speedMaximumUp.Size = new System.Drawing.Size(46, 21);
            this.btn_speedMaximumUp.TabIndex = 3;
            this.btn_speedMaximumUp.Text = ">>";
            this.btn_speedMaximumUp.UseVisualStyleBackColor = true;
            this.btn_speedMaximumUp.Click += new System.EventHandler(this.btn_speedMaximumUp_Click);
            // 
            // 检波门限设置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(377, 266);
            this.Controls.Add(this.btn_amUp);
            this.Controls.Add(this.btn_speedMaximumUp);
            this.Controls.Add(this.btn_speedUp);
            this.Controls.Add(this.btn_amDown);
            this.Controls.Add(this.btn_speedMaximumDown);
            this.Controls.Add(this.btn_speedDown);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_amConfirm);
            this.Controls.Add(this.btn_speedMaximumConfirm);
            this.Controls.Add(this.tb_amThreshold);
            this.Controls.Add(this.tb_speedMaximum);
            this.Controls.Add(this.btn_speedConfirm);
            this.Controls.Add(this.tb_speedMinimum);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Location = new System.Drawing.Point(1000, 300);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "检波门限设置";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "检波门限设置";
            this.Load += new System.EventHandler(this.检波门限设置_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_speedMinimum;
        private System.Windows.Forms.Button btn_speedConfirm;
        private System.Windows.Forms.TextBox tb_amThreshold;
        private System.Windows.Forms.Button btn_amConfirm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_speedDown;
        private System.Windows.Forms.Button btn_speedUp;
        private System.Windows.Forms.Button btn_amDown;
        private System.Windows.Forms.Button btn_amUp;
        private System.Windows.Forms.TextBox tb_speedMaximum;
        private System.Windows.Forms.Button btn_speedMaximumConfirm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_speedMaximumDown;
        private System.Windows.Forms.Button btn_speedMaximumUp;
    }
}