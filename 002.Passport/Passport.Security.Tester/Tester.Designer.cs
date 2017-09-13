namespace PassportTester
{
    partial class Tester
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
            this.button1 = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.edValidCode = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.picValidCode = new System.Windows.Forms.PictureBox();
            this.edUserIDFromToken = new System.Windows.Forms.TextBox();
            this.edLength = new System.Windows.Forms.TextBox();
            this.btPraseToken = new System.Windows.Forms.Button();
            this.edUserID = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.edToken = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lbTestResult = new System.Windows.Forms.Label();
            this.tbUserCryptPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbDbSalt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbSalt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCheckPassword = new System.Windows.Forms.TextBox();
            this.btPasswordTest = new System.Windows.Forms.Button();
            this.tbDbPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btBatchGetUniqueID = new System.Windows.Forms.Button();
            this.lbSerialID = new System.Windows.Forms.ListBox();
            this.btReadSerialID = new System.Windows.Forms.Button();
            this.tbTwepoch = new System.Windows.Forms.TextBox();
            this.tbUniqueID = new System.Windows.Forms.TextBox();
            this.btGetUniqueID = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picValidCode)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(37, 113);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 35);
            this.button1.TabIndex = 0;
            this.button1.Text = "创建数据库密码";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(260, 38);
            this.tbPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(313, 28);
            this.tbPassword.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.TabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1281, 781);
            this.tabControl1.TabIndex = 2;
            // 
            // TabPage1
            // 
            this.TabPage1.Controls.Add(this.edValidCode);
            this.TabPage1.Controls.Add(this.button4);
            this.TabPage1.Controls.Add(this.picValidCode);
            this.TabPage1.Controls.Add(this.edUserIDFromToken);
            this.TabPage1.Controls.Add(this.edLength);
            this.TabPage1.Controls.Add(this.btPraseToken);
            this.TabPage1.Controls.Add(this.edUserID);
            this.TabPage1.Controls.Add(this.button3);
            this.TabPage1.Controls.Add(this.edToken);
            this.TabPage1.Controls.Add(this.button2);
            this.TabPage1.Controls.Add(this.lbTestResult);
            this.TabPage1.Controls.Add(this.tbUserCryptPassword);
            this.TabPage1.Controls.Add(this.label5);
            this.TabPage1.Controls.Add(this.tbDbSalt);
            this.TabPage1.Controls.Add(this.label4);
            this.TabPage1.Controls.Add(this.tbSalt);
            this.TabPage1.Controls.Add(this.label3);
            this.TabPage1.Controls.Add(this.label2);
            this.TabPage1.Controls.Add(this.tbCheckPassword);
            this.TabPage1.Controls.Add(this.btPasswordTest);
            this.TabPage1.Controls.Add(this.tbDbPassword);
            this.TabPage1.Controls.Add(this.label1);
            this.TabPage1.Controls.Add(this.button1);
            this.TabPage1.Controls.Add(this.tbPassword);
            this.TabPage1.Location = new System.Drawing.Point(4, 28);
            this.TabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TabPage1.Size = new System.Drawing.Size(1273, 749);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "密码字符测试";
            this.TabPage1.UseVisualStyleBackColor = true;
            // 
            // edValidCode
            // 
            this.edValidCode.Location = new System.Drawing.Point(590, 589);
            this.edValidCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edValidCode.Name = "edValidCode";
            this.edValidCode.Size = new System.Drawing.Size(148, 28);
            this.edValidCode.TabIndex = 24;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(886, 586);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(112, 35);
            this.button4.TabIndex = 23;
            this.button4.Text = "验证码";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // picValidCode
            // 
            this.picValidCode.Location = new System.Drawing.Point(768, 589);
            this.picValidCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picValidCode.Name = "picValidCode";
            this.picValidCode.Size = new System.Drawing.Size(109, 30);
            this.picValidCode.TabIndex = 7;
            this.picValidCode.TabStop = false;
            // 
            // edUserIDFromToken
            // 
            this.edUserIDFromToken.Location = new System.Drawing.Point(406, 589);
            this.edUserIDFromToken.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edUserIDFromToken.Name = "edUserIDFromToken";
            this.edUserIDFromToken.Size = new System.Drawing.Size(148, 28);
            this.edUserIDFromToken.TabIndex = 21;
            // 
            // edLength
            // 
            this.edLength.Location = new System.Drawing.Point(248, 589);
            this.edLength.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edLength.Name = "edLength";
            this.edLength.Size = new System.Drawing.Size(148, 28);
            this.edLength.TabIndex = 20;
            // 
            // btPraseToken
            // 
            this.btPraseToken.Location = new System.Drawing.Point(42, 589);
            this.btPraseToken.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btPraseToken.Name = "btPraseToken";
            this.btPraseToken.Size = new System.Drawing.Size(112, 35);
            this.btPraseToken.TabIndex = 19;
            this.btPraseToken.Text = "拆解Token";
            this.btPraseToken.UseVisualStyleBackColor = true;
            this.btPraseToken.Click += new System.EventHandler(this.btPraseToken_Click);
            // 
            // edUserID
            // 
            this.edUserID.Location = new System.Drawing.Point(248, 505);
            this.edUserID.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edUserID.Name = "edUserID";
            this.edUserID.Size = new System.Drawing.Size(148, 28);
            this.edUserID.TabIndex = 18;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(863, 211);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 35);
            this.button3.TabIndex = 17;
            this.button3.Text = "还原Byte[]";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // edToken
            // 
            this.edToken.Location = new System.Drawing.Point(406, 505);
            this.edToken.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.edToken.Name = "edToken";
            this.edToken.Size = new System.Drawing.Size(566, 28);
            this.edToken.TabIndex = 16;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(37, 505);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 15;
            this.button2.Text = "生成Token";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbTestResult
            // 
            this.lbTestResult.AutoSize = true;
            this.lbTestResult.Location = new System.Drawing.Point(753, 218);
            this.lbTestResult.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbTestResult.Name = "lbTestResult";
            this.lbTestResult.Size = new System.Drawing.Size(0, 18);
            this.lbTestResult.TabIndex = 14;
            // 
            // tbUserCryptPassword
            // 
            this.tbUserCryptPassword.Location = new System.Drawing.Point(248, 359);
            this.tbUserCryptPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbUserCryptPassword.Name = "tbUserCryptPassword";
            this.tbUserCryptPassword.Size = new System.Drawing.Size(725, 28);
            this.tbUserCryptPassword.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 359);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 18);
            this.label5.TabIndex = 12;
            this.label5.Text = "本次密文结果";
            // 
            // tbDbSalt
            // 
            this.tbDbSalt.Location = new System.Drawing.Point(248, 287);
            this.tbDbSalt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDbSalt.Name = "tbDbSalt";
            this.tbDbSalt.Size = new System.Drawing.Size(260, 28);
            this.tbDbSalt.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 300);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(188, 18);
            this.label4.TabIndex = 10;
            this.label4.Text = "数据库密文中提取Salt";
            // 
            // tbSalt
            // 
            this.tbSalt.Location = new System.Drawing.Point(765, 36);
            this.tbSalt.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbSalt.Name = "tbSalt";
            this.tbSalt.Size = new System.Drawing.Size(220, 28);
            this.tbSalt.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(633, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 18);
            this.label3.TabIndex = 8;
            this.label3.Text = "本次Salt";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 218);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "请输入密码";
            // 
            // tbCheckPassword
            // 
            this.tbCheckPassword.Location = new System.Drawing.Point(248, 215);
            this.tbCheckPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbCheckPassword.Name = "tbCheckPassword";
            this.tbCheckPassword.Size = new System.Drawing.Size(260, 28);
            this.tbCheckPassword.TabIndex = 5;
            // 
            // btPasswordTest
            // 
            this.btPasswordTest.Location = new System.Drawing.Point(554, 211);
            this.btPasswordTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btPasswordTest.Name = "btPasswordTest";
            this.btPasswordTest.Size = new System.Drawing.Size(159, 35);
            this.btPasswordTest.TabIndex = 4;
            this.btPasswordTest.Text = "密码校验测试";
            this.btPasswordTest.UseVisualStyleBackColor = true;
            this.btPasswordTest.Click += new System.EventHandler(this.btPasswordTest_Click);
            // 
            // tbDbPassword
            // 
            this.tbDbPassword.Location = new System.Drawing.Point(260, 113);
            this.tbDbPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbDbPassword.Name = "tbDbPassword";
            this.tbDbPassword.Size = new System.Drawing.Size(725, 28);
            this.tbDbPassword.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 53);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 18);
            this.label1.TabIndex = 2;
            this.label1.Text = "用户密码明文";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btBatchGetUniqueID);
            this.tabPage2.Controls.Add(this.lbSerialID);
            this.tabPage2.Controls.Add(this.btReadSerialID);
            this.tabPage2.Controls.Add(this.tbTwepoch);
            this.tabPage2.Controls.Add(this.tbUniqueID);
            this.tabPage2.Controls.Add(this.btGetUniqueID);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(1273, 749);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btBatchGetUniqueID
            // 
            this.btBatchGetUniqueID.Location = new System.Drawing.Point(30, 284);
            this.btBatchGetUniqueID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btBatchGetUniqueID.Name = "btBatchGetUniqueID";
            this.btBatchGetUniqueID.Size = new System.Drawing.Size(141, 61);
            this.btBatchGetUniqueID.TabIndex = 5;
            this.btBatchGetUniqueID.Text = "批量生成SerialID";
            this.btBatchGetUniqueID.UseVisualStyleBackColor = true;
            this.btBatchGetUniqueID.Click += new System.EventHandler(this.btBatchGetUniqueID_Click);
            // 
            // lbSerialID
            // 
            this.lbSerialID.FormattingEnabled = true;
            this.lbSerialID.ItemHeight = 18;
            this.lbSerialID.Location = new System.Drawing.Point(202, 284);
            this.lbSerialID.Name = "lbSerialID";
            this.lbSerialID.Size = new System.Drawing.Size(769, 346);
            this.lbSerialID.TabIndex = 4;
            // 
            // btReadSerialID
            // 
            this.btReadSerialID.Location = new System.Drawing.Point(993, 48);
            this.btReadSerialID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btReadSerialID.Name = "btReadSerialID";
            this.btReadSerialID.Size = new System.Drawing.Size(141, 34);
            this.btReadSerialID.TabIndex = 3;
            this.btReadSerialID.Text = "解读SerialID";
            this.btReadSerialID.UseVisualStyleBackColor = true;
            this.btReadSerialID.Click += new System.EventHandler(this.btReadSerialID_Click);
            // 
            // tbTwepoch
            // 
            this.tbTwepoch.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbTwepoch.Location = new System.Drawing.Point(202, 172);
            this.tbTwepoch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbTwepoch.Name = "tbTwepoch";
            this.tbTwepoch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbTwepoch.Size = new System.Drawing.Size(769, 90);
            this.tbTwepoch.TabIndex = 2;
            this.tbTwepoch.Text = "989048574657";
            // 
            // tbUniqueID
            // 
            this.tbUniqueID.Font = new System.Drawing.Font("宋体", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbUniqueID.Location = new System.Drawing.Point(202, 48);
            this.tbUniqueID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbUniqueID.Name = "tbUniqueID";
            this.tbUniqueID.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tbUniqueID.Size = new System.Drawing.Size(769, 90);
            this.tbUniqueID.TabIndex = 1;
            // 
            // btGetUniqueID
            // 
            this.btGetUniqueID.Location = new System.Drawing.Point(30, 44);
            this.btGetUniqueID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btGetUniqueID.Name = "btGetUniqueID";
            this.btGetUniqueID.Size = new System.Drawing.Size(141, 34);
            this.btGetUniqueID.TabIndex = 0;
            this.btGetUniqueID.Text = "生成SerialID";
            this.btGetUniqueID.UseVisualStyleBackColor = true;
            this.btGetUniqueID.Click += new System.EventHandler(this.btGetUniqueID_Click);
            // 
            // Tester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 781);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Tester";
            this.Text = "Passport测试";
            this.tabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picValidCode)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage TabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDbPassword;
        private System.Windows.Forms.Button btPasswordTest;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCheckPassword;
        private System.Windows.Forms.TextBox tbSalt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDbSalt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbUserCryptPassword;
        private System.Windows.Forms.Label lbTestResult;
        private System.Windows.Forms.TextBox edToken;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox edUserID;
        private System.Windows.Forms.Button btPraseToken;
        private System.Windows.Forms.TextBox edUserIDFromToken;
        private System.Windows.Forms.TextBox edLength;
        private System.Windows.Forms.PictureBox picValidCode;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox edValidCode;
        private System.Windows.Forms.TextBox tbUniqueID;
        private System.Windows.Forms.Button btGetUniqueID;
        private System.Windows.Forms.TextBox tbTwepoch;
        private System.Windows.Forms.Button btReadSerialID;
        private System.Windows.Forms.Button btBatchGetUniqueID;
        private System.Windows.Forms.ListBox lbSerialID;
    }
}

