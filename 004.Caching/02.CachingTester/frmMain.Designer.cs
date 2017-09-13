namespace _02.CachingTester
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
            this.btSet = new System.Windows.Forms.Button();
            this.btGet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btSet
            // 
            this.btSet.Location = new System.Drawing.Point(92, 123);
            this.btSet.Name = "btSet";
            this.btSet.Size = new System.Drawing.Size(108, 69);
            this.btSet.TabIndex = 0;
            this.btSet.Text = "写入";
            this.btSet.UseVisualStyleBackColor = true;
            this.btSet.Click += new System.EventHandler(this.btSet_Click);
            // 
            // btGet
            // 
            this.btGet.Location = new System.Drawing.Point(257, 122);
            this.btGet.Name = "btGet";
            this.btGet.Size = new System.Drawing.Size(108, 69);
            this.btGet.TabIndex = 1;
            this.btGet.Text = "读取";
            this.btGet.UseVisualStyleBackColor = true;
            this.btGet.Click += new System.EventHandler(this.btGet_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 308);
            this.Controls.Add(this.btGet);
            this.Controls.Add(this.btSet);
            this.Name = "frmMain";
            this.Text = "缓存测试";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btSet;
        private System.Windows.Forms.Button btGet;
    }
}

