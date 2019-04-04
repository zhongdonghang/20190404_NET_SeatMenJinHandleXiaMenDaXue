namespace CardLogSync
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
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SyncForm));
            this.btn_Start = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_End = new System.Windows.Forms.Button();
            this.rtb_Message = new System.Windows.Forms.RichTextBox();
            this.lb_name = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_Start
            // 
            this.btn_Start.Location = new System.Drawing.Point(224, 159);
            this.btn_Start.Name = "btn_Start";
            this.btn_Start.Size = new System.Drawing.Size(101, 23);
            this.btn_Start.TabIndex = 1;
            this.btn_Start.Text = "开始获取记录";
            this.btn_Start.UseVisualStyleBackColor = true;
            this.btn_Start.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "记录信息";
            // 
            // btn_End
            // 
            this.btn_End.Location = new System.Drawing.Point(331, 159);
            this.btn_End.Name = "btn_End";
            this.btn_End.Size = new System.Drawing.Size(101, 23);
            this.btn_End.TabIndex = 4;
            this.btn_End.Text = "停止获取数据";
            this.btn_End.UseVisualStyleBackColor = true;
            this.btn_End.Click += new System.EventHandler(this.btn_End_Click);
            // 
            // rtb_Message
            // 
            this.rtb_Message.Location = new System.Drawing.Point(12, 50);
            this.rtb_Message.Name = "rtb_Message";
            this.rtb_Message.Size = new System.Drawing.Size(420, 103);
            this.rtb_Message.TabIndex = 5;
            this.rtb_Message.Text = "";
            // 
            // lb_name
            // 
            this.lb_name.AutoSize = true;
            this.lb_name.Location = new System.Drawing.Point(12, 13);
            this.lb_name.Name = "lb_name";
            this.lb_name.Size = new System.Drawing.Size(41, 12);
            this.lb_name.TabIndex = 6;
            this.lb_name.Text = "label1";
            // 
            // SyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 194);
            this.Controls.Add(this.lb_name);
            this.Controls.Add(this.rtb_Message);
            this.Controls.Add(this.btn_End);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_Start);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SyncForm";
            this.Text = "门禁刷卡记录同步程序";
            this.Load += new System.EventHandler(this.SyncForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_End;
        private System.Windows.Forms.RichTextBox rtb_Message;
        private System.Windows.Forms.Label lb_name;
    }
}

