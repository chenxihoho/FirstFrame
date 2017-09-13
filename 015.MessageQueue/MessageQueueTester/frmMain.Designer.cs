namespace MessageQueueTester
{
    partial class frmMain
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
            this.btSendMessage = new System.Windows.Forms.Button();
            this.tbReceiveMessage = new System.Windows.Forms.TextBox();
            this.tbSendMessage = new System.Windows.Forms.TextBox();
            this.tbSendCount = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btSendMessage
            // 
            this.btSendMessage.Location = new System.Drawing.Point(55, 263);
            this.btSendMessage.Name = "btSendMessage";
            this.btSendMessage.Size = new System.Drawing.Size(104, 39);
            this.btSendMessage.TabIndex = 0;
            this.btSendMessage.Text = "发送消息";
            this.btSendMessage.UseVisualStyleBackColor = true;
            this.btSendMessage.Click += new System.EventHandler(this.btSendMessage_Click);
            // 
            // tbReceiveMessage
            // 
            this.tbReceiveMessage.Location = new System.Drawing.Point(328, 62);
            this.tbReceiveMessage.Multiline = true;
            this.tbReceiveMessage.Name = "tbReceiveMessage";
            this.tbReceiveMessage.Size = new System.Drawing.Size(191, 179);
            this.tbReceiveMessage.TabIndex = 1;
            // 
            // tbSendMessage
            // 
            this.tbSendMessage.Location = new System.Drawing.Point(55, 62);
            this.tbSendMessage.Multiline = true;
            this.tbSendMessage.Name = "tbSendMessage";
            this.tbSendMessage.Size = new System.Drawing.Size(191, 179);
            this.tbSendMessage.TabIndex = 2;
            // 
            // tbSendCount
            // 
            this.tbSendCount.Location = new System.Drawing.Point(55, 12);
            this.tbSendCount.Name = "tbSendCount";
            this.tbSendCount.Size = new System.Drawing.Size(191, 28);
            this.tbSendCount.TabIndex = 3;
            this.tbSendCount.Text = "100";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 333);
            this.Controls.Add(this.tbSendCount);
            this.Controls.Add(this.tbSendMessage);
            this.Controls.Add(this.tbReceiveMessage);
            this.Controls.Add(this.btSendMessage);
            this.Name = "frmMain";
            this.Text = "消息队列测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSendMessage;
        private System.Windows.Forms.TextBox tbReceiveMessage;
        private System.Windows.Forms.TextBox tbSendMessage;
        private System.Windows.Forms.TextBox tbSendCount;
    }
}

