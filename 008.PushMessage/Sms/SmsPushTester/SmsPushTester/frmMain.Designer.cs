namespace SmsPushTester
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
            this.btSend = new System.Windows.Forms.Button();
            this.edMobilePhone = new System.Windows.Forms.TextBox();
            this.edVerifyCode = new System.Windows.Forms.TextBox();
            this.btCheck = new System.Windows.Forms.Button();
            this.edSendLog = new System.Windows.Forms.TextBox();
            this.VerifyCodeTimer = new System.Windows.Forms.Timer(this.components);
            this.lbExpireTime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btSend
            // 
            this.btSend.Location = new System.Drawing.Point(170, 25);
            this.btSend.Name = "btSend";
            this.btSend.Size = new System.Drawing.Size(127, 23);
            this.btSend.TabIndex = 0;
            this.btSend.Text = "发送验证码";
            this.btSend.UseVisualStyleBackColor = true;
            this.btSend.Click += new System.EventHandler(this.btSend_Click);
            // 
            // edMobilePhone
            // 
            this.edMobilePhone.Location = new System.Drawing.Point(34, 29);
            this.edMobilePhone.Name = "edMobilePhone";
            this.edMobilePhone.Size = new System.Drawing.Size(130, 21);
            this.edMobilePhone.TabIndex = 1;
            this.edMobilePhone.Text = "13764167673";
            // 
            // edVerifyCode
            // 
            this.edVerifyCode.Location = new System.Drawing.Point(34, 56);
            this.edVerifyCode.Name = "edVerifyCode";
            this.edVerifyCode.Size = new System.Drawing.Size(130, 21);
            this.edVerifyCode.TabIndex = 2;
            // 
            // btCheck
            // 
            this.btCheck.Location = new System.Drawing.Point(170, 56);
            this.btCheck.Name = "btCheck";
            this.btCheck.Size = new System.Drawing.Size(127, 23);
            this.btCheck.TabIndex = 3;
            this.btCheck.Text = "校验";
            this.btCheck.UseVisualStyleBackColor = true;
            this.btCheck.Click += new System.EventHandler(this.btCheck_Click);
            // 
            // edSendLog
            // 
            this.edSendLog.Location = new System.Drawing.Point(12, 99);
            this.edSendLog.Multiline = true;
            this.edSendLog.Name = "edSendLog";
            this.edSendLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edSendLog.Size = new System.Drawing.Size(441, 208);
            this.edSendLog.TabIndex = 4;
            // 
            // VerifyCodeTimer
            // 
            this.VerifyCodeTimer.Interval = 1000;
            this.VerifyCodeTimer.Tick += new System.EventHandler(this.VerifyCodeTimer_Tick);
            // 
            // lbExpireTime
            // 
            this.lbExpireTime.AutoSize = true;
            this.lbExpireTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbExpireTime.ForeColor = System.Drawing.Color.Red;
            this.lbExpireTime.Location = new System.Drawing.Point(303, 29);
            this.lbExpireTime.Name = "lbExpireTime";
            this.lbExpireTime.Size = new System.Drawing.Size(27, 19);
            this.lbExpireTime.TabIndex = 5;
            this.lbExpireTime.Text = "60";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 329);
            this.Controls.Add(this.lbExpireTime);
            this.Controls.Add(this.edSendLog);
            this.Controls.Add(this.btCheck);
            this.Controls.Add(this.edVerifyCode);
            this.Controls.Add(this.edMobilePhone);
            this.Controls.Add(this.btSend);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "短信验证码发送测试";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSend;
        private System.Windows.Forms.TextBox edMobilePhone;
        private System.Windows.Forms.TextBox edVerifyCode;
        private System.Windows.Forms.Button btCheck;
        private System.Windows.Forms.TextBox edSendLog;
        private System.Windows.Forms.Timer VerifyCodeTimer;
        private System.Windows.Forms.Label lbExpireTime;
    }
}

