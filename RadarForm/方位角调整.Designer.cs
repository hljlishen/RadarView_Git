namespace RadarForm
{
    partial class 方位角调整
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
            this.btn_confirm = new System.Windows.Forms.Button();
            this.tb_azAngleAdjustment = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_elConfirm = new System.Windows.Forms.Button();
            this.tb_elAdjustment = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_confirmAntennaStopDegree = new System.Windows.Forms.Button();
            this.tb_antennaStopDegree = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.tb_elDiff = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_confirm
            // 
            this.btn_confirm.Location = new System.Drawing.Point(270, 35);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(75, 23);
            this.btn_confirm.TabIndex = 0;
            this.btn_confirm.Text = "设置";
            this.btn_confirm.UseVisualStyleBackColor = true;
            this.btn_confirm.Click += new System.EventHandler(this.btn_confirm_Click);
            // 
            // tb_azAngleAdjustment
            // 
            this.tb_azAngleAdjustment.Location = new System.Drawing.Point(99, 37);
            this.tb_azAngleAdjustment.Name = "tb_azAngleAdjustment";
            this.tb_azAngleAdjustment.Size = new System.Drawing.Size(142, 21);
            this.tb_azAngleAdjustment.TabIndex = 1;
            this.tb_azAngleAdjustment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_azAngleAdjustment_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(60, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "方位";
            // 
            // btn_elConfirm
            // 
            this.btn_elConfirm.Location = new System.Drawing.Point(270, 73);
            this.btn_elConfirm.Name = "btn_elConfirm";
            this.btn_elConfirm.Size = new System.Drawing.Size(75, 23);
            this.btn_elConfirm.TabIndex = 0;
            this.btn_elConfirm.Text = "设置";
            this.btn_elConfirm.UseVisualStyleBackColor = true;
            this.btn_elConfirm.Click += new System.EventHandler(this.btn_elConfirm_Click);
            // 
            // tb_elAdjustment
            // 
            this.tb_elAdjustment.Location = new System.Drawing.Point(99, 75);
            this.tb_elAdjustment.Name = "tb_elAdjustment";
            this.tb_elAdjustment.Size = new System.Drawing.Size(142, 21);
            this.tb_elAdjustment.TabIndex = 1;
            this.tb_elAdjustment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_azAngleAdjustment_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(60, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "仰角";
            // 
            // btn_confirmAntennaStopDegree
            // 
            this.btn_confirmAntennaStopDegree.Location = new System.Drawing.Point(270, 108);
            this.btn_confirmAntennaStopDegree.Name = "btn_confirmAntennaStopDegree";
            this.btn_confirmAntennaStopDegree.Size = new System.Drawing.Size(75, 23);
            this.btn_confirmAntennaStopDegree.TabIndex = 0;
            this.btn_confirmAntennaStopDegree.Text = "设置";
            this.btn_confirmAntennaStopDegree.UseVisualStyleBackColor = true;
            this.btn_confirmAntennaStopDegree.Click += new System.EventHandler(this.btn_confirmAntennaStopDegree_Click);
            // 
            // tb_antennaStopDegree
            // 
            this.tb_antennaStopDegree.Location = new System.Drawing.Point(99, 110);
            this.tb_antennaStopDegree.Name = "tb_antennaStopDegree";
            this.tb_antennaStopDegree.Size = new System.Drawing.Size(142, 21);
            this.tb_antennaStopDegree.TabIndex = 1;
            this.tb_antennaStopDegree.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_azAngleAdjustment_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "天线归位角度";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(270, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "设置";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tb_elDiff
            // 
            this.tb_elDiff.Location = new System.Drawing.Point(99, 144);
            this.tb_elDiff.Name = "tb_elDiff";
            this.tb_elDiff.Size = new System.Drawing.Size(142, 21);
            this.tb_elDiff.TabIndex = 1;
            this.tb_elDiff.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_azAngleAdjustment_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "仰角系数";
            // 
            // 方位角调整
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 185);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_elDiff);
            this.Controls.Add(this.tb_antennaStopDegree);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tb_elAdjustment);
            this.Controls.Add(this.btn_confirmAntennaStopDegree);
            this.Controls.Add(this.tb_azAngleAdjustment);
            this.Controls.Add(this.btn_elConfirm);
            this.Controls.Add(this.btn_confirm);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "方位角调整";
            this.Text = "角度调整";
            this.Load += new System.EventHandler(this.方位角调整_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_confirm;
        private System.Windows.Forms.TextBox tb_azAngleAdjustment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_elConfirm;
        private System.Windows.Forms.TextBox tb_elAdjustment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_confirmAntennaStopDegree;
        private System.Windows.Forms.TextBox tb_antennaStopDegree;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tb_elDiff;
        private System.Windows.Forms.Label label4;
    }
}