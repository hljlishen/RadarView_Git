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
            this.SuspendLayout();
            // 
            // btn_confirm
            // 
            this.btn_confirm.Location = new System.Drawing.Point(203, 35);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(75, 23);
            this.btn_confirm.TabIndex = 0;
            this.btn_confirm.Text = "设置";
            this.btn_confirm.UseVisualStyleBackColor = true;
            this.btn_confirm.Click += new System.EventHandler(this.btn_confirm_Click);
            // 
            // tb_azAngleAdjustment
            // 
            this.tb_azAngleAdjustment.Location = new System.Drawing.Point(35, 37);
            this.tb_azAngleAdjustment.Name = "tb_azAngleAdjustment";
            this.tb_azAngleAdjustment.Size = new System.Drawing.Size(142, 21);
            this.tb_azAngleAdjustment.TabIndex = 1;
            this.tb_azAngleAdjustment.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tb_azAngleAdjustment_KeyDown);
            // 
            // 方位角调整
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 88);
            this.Controls.Add(this.tb_azAngleAdjustment);
            this.Controls.Add(this.btn_confirm);
            this.Name = "方位角调整";
            this.Text = "方位角调整";
            this.Load += new System.EventHandler(this.方位角调整_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_confirm;
        private System.Windows.Forms.TextBox tb_azAngleAdjustment;
    }
}