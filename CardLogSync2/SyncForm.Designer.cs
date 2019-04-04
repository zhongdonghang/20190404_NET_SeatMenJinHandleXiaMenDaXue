namespace CardLogSync2
{
    partial class SyncForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lb_name = new System.Windows.Forms.Label();
            this.rtb_Message = new System.Windows.Forms.RichTextBox();
            this.btn_End = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_Start = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lb_name
            // 
            this.lb_name.AutoSize = true;
            this.lb_name.Location = new System.Drawing.Point(37, 16);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(41, 12);
            this.lb_name.TabIndex = 11;
            this.lb_name.Text = "label1";
            // 
            // rtb_Message
            // 
            this.rtb_Message.Location = new System.Drawing.Point(37, 53);
            this.rtb_Message.Name = "rtb_Message";
            this.rtb_Message.Size = new System.Drawing.Size(420, 103);
            this.rtb_Message.TabIndex = 10;
            this.rtb_Message.Text = "";
            // 
            // btn_End
            // 
            this.btn_End.Location = new System.Drawing.Point(356, 162);
            this.btn_End.Name = "btn_End";
            this.btn_End.Size = new System.Drawing.Size(101, 23);
            this.btn_End.TabIndex = 9;
            this.btn_End.Text = "停止获取数据";
            this.btn_End.UseVisualStyleBackColor = true;
            this.btn_End.Click += new System.EventHandler(this.btn_End_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "记录信息";
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(249, 162);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(101, 23);
            this.btn_Start.TabIndex = 7;
            this.btn_Start.Text = "开始获取记录";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.btn_Start_Click);
            // 
            // SyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 201);
            this.Controls.Add(this.lb_name);
            this.Controls.Add(this.rtb_Message);
            this.Controls.Add(this.btn_End);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_Start);
            this.Name = "SyncForm";
            this.Text = "门禁刷卡记录同步程序";
            this.Load += new System.EventHandler(this.SyncForm_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_name;
        private System.Windows.Forms.RichTextBox rtb_Message;
        private System.Windows.Forms.Button btn_End;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Start;
    }
}

