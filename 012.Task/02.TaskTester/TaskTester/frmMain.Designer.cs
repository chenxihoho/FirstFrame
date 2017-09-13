namespace TaskTester
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
            this.components = new System.ComponentModel.Container();
            this.btLoadTask = new System.Windows.Forms.Button();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.lbStatus = new System.Windows.Forms.Label();
            this.tbTaskID = new System.Windows.Forms.TextBox();
            this.btExecuteTask = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btLoadTask
            // 
            this.btLoadTask.Location = new System.Drawing.Point(36, 32);
            this.btLoadTask.Name = "btLoadTask";
            this.btLoadTask.Size = new System.Drawing.Size(129, 50);
            this.btLoadTask.TabIndex = 0;
            this.btLoadTask.Text = "装载任务";
            this.btLoadTask.UseVisualStyleBackColor = true;
            this.btLoadTask.Click += new System.EventHandler(this.btLoadTask_Click);
            // 
            // Timer
            // 
            this.Timer.Enabled = true;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(47, 142);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(80, 18);
            this.lbStatus.TabIndex = 1;
            this.lbStatus.Text = "任务状态";
            // 
            // tbTaskID
            // 
            this.tbTaskID.Location = new System.Drawing.Point(207, 32);
            this.tbTaskID.Name = "tbTaskID";
            this.tbTaskID.Size = new System.Drawing.Size(197, 28);
            this.tbTaskID.TabIndex = 2;
            // 
            // btExecuteTask
            // 
            this.btExecuteTask.Location = new System.Drawing.Point(429, 32);
            this.btExecuteTask.Name = "btExecuteTask";
            this.btExecuteTask.Size = new System.Drawing.Size(118, 50);
            this.btExecuteTask.TabIndex = 3;
            this.btExecuteTask.Text = "启动任务";
            this.btExecuteTask.UseVisualStyleBackColor = true;
            this.btExecuteTask.Click += new System.EventHandler(this.btExecuteTask_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(648, 255);
            this.Controls.Add(this.btExecuteTask);
            this.Controls.Add(this.tbTaskID);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.btLoadTask);
            this.Name = "frmMain";
            this.Text = "远程任务测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btLoadTask;
        private System.Windows.Forms.Timer Timer;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TextBox tbTaskID;
        private System.Windows.Forms.Button btExecuteTask;
    }
}

