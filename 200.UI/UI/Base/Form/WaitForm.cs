/*
* Loading窗体
* 张国伟
* 2016-11-17
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraWaitForm;
using FirstFrame.Const;

namespace FirstFrame.UI.Form
{
    public partial class WaitingForm : WaitForm 
    {
        private ProgressPanel progressPanel;
    
        public WaitingForm()
        {
            InitializeComponent();
            progressPanel.Caption = Resource.ProgressPanelCaption;
            progressPanel.Description = Resource.ProgressPanelDescription;
        }
        private void InitializeComponent()
        {
            this.progressPanel = new DevExpress.XtraWaitForm.ProgressPanel();
            this.SuspendLayout();
            // 
            // progressPanel
            // 
            this.progressPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressPanel.Appearance.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progressPanel.Appearance.Options.UseBackColor = true;
            this.progressPanel.Appearance.Options.UseFont = true;
            this.progressPanel.AppearanceCaption.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.progressPanel.AppearanceCaption.Options.UseFont = true;
            this.progressPanel.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.progressPanel.AppearanceDescription.Options.UseFont = true;
            this.progressPanel.Caption = "请稍候";
            this.progressPanel.Description = "正在从服务器中获取内容 ...";
            this.progressPanel.Location = new System.Drawing.Point(19, 15);
            this.progressPanel.Name = "progressPanel";
            this.progressPanel.Size = new System.Drawing.Size(212, 38);
            this.progressPanel.TabIndex = 0;
            this.progressPanel.Text = "progressPanel";
            // 
            // WaitingForm
            // 
            this.ClientSize = new System.Drawing.Size(246, 67);
            this.Controls.Add(this.progressPanel);
            this.Name = "WaitingForm";
            this.ShowOnTopMode = DevExpress.XtraWaitForm.ShowFormOnTopMode.AboveParent;
            this.ResumeLayout(false);
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion
    }
}
