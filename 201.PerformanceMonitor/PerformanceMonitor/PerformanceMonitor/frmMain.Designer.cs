namespace PerformanceMonitor
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
            this.gbRequestExecutionTime = new System.Windows.Forms.GroupBox();
            this.MonitorTime = new System.Windows.Forms.Timer(this.components);
            this.lbRequestExecutionTime = new PerformanceMonitor.PerformanceMonitorLabel();
            this.gbWebServiceCurrentConnections = new System.Windows.Forms.GroupBox();
            this.lbWebCurrentConnections = new PerformanceMonitor.PerformanceMonitorLabel();
            this.gbRequestExecutionTime.SuspendLayout();
            this.gbWebServiceCurrentConnections.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbRequestExecutionTime
            // 
            this.gbRequestExecutionTime.Controls.Add(this.lbRequestExecutionTime);
            this.gbRequestExecutionTime.Location = new System.Drawing.Point(33, 30);
            this.gbRequestExecutionTime.Name = "gbRequestExecutionTime";
            this.gbRequestExecutionTime.Size = new System.Drawing.Size(355, 430);
            this.gbRequestExecutionTime.TabIndex = 1;
            this.gbRequestExecutionTime.TabStop = false;
            this.gbRequestExecutionTime.Text = "Request Execution Time";
            // 
            // MonitorTime
            // 
            this.MonitorTime.Interval = 1000;
            this.MonitorTime.Tick += new System.EventHandler(this.MonitorTime_Tick);
            // 
            // lbRequestExecutionTime
            // 
            this.lbRequestExecutionTime.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRequestExecutionTime.Location = new System.Drawing.Point(36, 44);
            this.lbRequestExecutionTime.Margin = new System.Windows.Forms.Padding(4);
            this.lbRequestExecutionTime.Name = "lbRequestExecutionTime";
            this.lbRequestExecutionTime.Size = new System.Drawing.Size(274, 379);
            this.lbRequestExecutionTime.TabIndex = 0;
            // 
            // gbWebServiceCurrentConnections
            // 
            this.gbWebServiceCurrentConnections.Controls.Add(this.lbWebCurrentConnections);
            this.gbWebServiceCurrentConnections.Location = new System.Drawing.Point(447, 30);
            this.gbWebServiceCurrentConnections.Name = "gbWebServiceCurrentConnections";
            this.gbWebServiceCurrentConnections.Size = new System.Drawing.Size(355, 430);
            this.gbWebServiceCurrentConnections.TabIndex = 2;
            this.gbWebServiceCurrentConnections.TabStop = false;
            this.gbWebServiceCurrentConnections.Text = "Web Current Connections";
            // 
            // lbWebCurrentConnections
            // 
            this.lbWebCurrentConnections.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbWebCurrentConnections.Location = new System.Drawing.Point(36, 44);
            this.lbWebCurrentConnections.Margin = new System.Windows.Forms.Padding(4);
            this.lbWebCurrentConnections.Name = "lbWebCurrentConnections";
            this.lbWebCurrentConnections.Size = new System.Drawing.Size(274, 379);
            this.lbWebCurrentConnections.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(974, 626);
            this.Controls.Add(this.gbWebServiceCurrentConnections);
            this.Controls.Add(this.gbRequestExecutionTime);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Text = "服务器状态监控";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.gbRequestExecutionTime.ResumeLayout(false);
            this.gbWebServiceCurrentConnections.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PerformanceMonitorLabel plRequestExecutionTime = new PerformanceMonitorLabel();
        private System.Windows.Forms.GroupBox gbRequestExecutionTime;
        private System.Windows.Forms.Timer MonitorTime;
        private PerformanceMonitorLabel lbRequestExecutionTime;
        private System.Windows.Forms.GroupBox gbWebServiceCurrentConnections;
        private PerformanceMonitorLabel lbWebCurrentConnections;

    }
}

