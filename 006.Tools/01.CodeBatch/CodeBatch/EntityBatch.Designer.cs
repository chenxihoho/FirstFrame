namespace codeBatch
{
    partial class EntityBatch
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.rbtbcreate = new System.Windows.Forms.RadioButton();
            this.rbtcreate = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.tbxpath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtdb = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbxNameSpace = new System.Windows.Forms.TextBox();
            this.txtmsg = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(63, 169);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择表";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(49, 139);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "生成方式";
            // 
            // rbtbcreate
            // 
            this.rbtbcreate.AutoSize = true;
            this.rbtbcreate.Font = new System.Drawing.Font("宋体", 9F);
            this.rbtbcreate.Location = new System.Drawing.Point(128, 138);
            this.rbtbcreate.Name = "rbtbcreate";
            this.rbtbcreate.Size = new System.Drawing.Size(95, 16);
            this.rbtbcreate.TabIndex = 3;
            this.rbtbcreate.Text = "批量全部生成";
            this.rbtbcreate.UseVisualStyleBackColor = true;
            this.rbtbcreate.CheckedChanged += new System.EventHandler(this.rbtbcreate_CheckedChanged);
            // 
            // rbtcreate
            // 
            this.rbtcreate.AutoSize = true;
            this.rbtcreate.Checked = true;
            this.rbtcreate.Font = new System.Drawing.Font("宋体", 9F);
            this.rbtcreate.Location = new System.Drawing.Point(251, 138);
            this.rbtcreate.Name = "rbtcreate";
            this.rbtcreate.Size = new System.Drawing.Size(71, 16);
            this.rbtcreate.TabIndex = 4;
            this.rbtcreate.TabStop = true;
            this.rbtcreate.Text = "单表生成";
            this.rbtcreate.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(49, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "保存路径";
            // 
            // tbxpath
            // 
            this.tbxpath.Location = new System.Drawing.Point(128, 60);
            this.tbxpath.Name = "tbxpath";
            this.tbxpath.Size = new System.Drawing.Size(402, 21);
            this.tbxpath.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(245, 194);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "生成实体";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(353, 194);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "清空消息";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtdb
            // 
            this.txtdb.Location = new System.Drawing.Point(128, 19);
            this.txtdb.Name = "txtdb";
            this.txtdb.Size = new System.Drawing.Size(402, 21);
            this.txtdb.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(35, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "数据库链接";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(33, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "namespace";
            // 
            // tbxNameSpace
            // 
            this.tbxNameSpace.Location = new System.Drawing.Point(128, 101);
            this.tbxNameSpace.Name = "tbxNameSpace";
            this.tbxNameSpace.Size = new System.Drawing.Size(402, 21);
            this.tbxNameSpace.TabIndex = 13;
            // 
            // txtmsg
            // 
            this.txtmsg.Location = new System.Drawing.Point(55, 230);
            this.txtmsg.Multiline = true;
            this.txtmsg.Name = "txtmsg";
            this.txtmsg.Size = new System.Drawing.Size(475, 184);
            this.txtmsg.TabIndex = 14;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(128, 162);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(402, 20);
            this.comboBox1.TabIndex = 15;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(137, 194);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "数据链接";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // EntityBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(584, 433);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.txtmsg);
            this.Controls.Add(this.tbxNameSpace);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtdb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbxpath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbtcreate);
            this.Controls.Add(this.rbtbcreate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "EntityBatch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Entity文件生成";
            this.Load += new System.EventHandler(this.EntityBatch_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbtbcreate;
        private System.Windows.Forms.RadioButton rbtcreate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbxpath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtdb;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbxNameSpace;
        private System.Windows.Forms.TextBox txtmsg;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button3;
    }
}