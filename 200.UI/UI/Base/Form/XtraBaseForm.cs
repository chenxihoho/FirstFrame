/*
* 窗体基类
* 张国伟
* 2014-12-17
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraSplashScreen;
using FirstFrame.UI;

namespace FirstFrame.UI.Form
{
    public delegate void OnNotifyData(object Object);
    public delegate object OnGetData(object[] Args);
    public delegate void OnFormClose();
    public partial class XtraBaseForm : DevExpress.XtraEditors.XtraForm, IPermission
    {
        public  object Sender;
        public int MenuID = 0;
        public event OnNotifyData NotifyData;
        public event OnGetData GetData;
        private Timer WaitingTimer = null;
        public bool HasPermission { get { return CheckPermission(); } }
        //public string Module(int Action = Const.DefaultMenuAction) { return MenuID.ToString() + Action.ToString(); }
        public static DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
        public XtraBaseForm()
        {
            InitializeComponent();
            WindowManager.Splash.Properties.ParentForm = this;
            WindowManager.Splash.Properties.UseFadeInEffect = false;
            WindowManager.Splash.Properties.UseFadeOutEffect = false;
        }
        public void Close(bool Dispose)
        {
            if (Dispose)
            {
                WindowManager.DisposeWindow(this.GetType());
                base.Dispose(true);
                return;
            }
            Hide();
        }
        protected override void OnClosed(EventArgs e)
        {
            if (NotifyData != null) NotifyData -= NotifyData;
            if (GetData != null) GetData -= GetData;
            base.OnClosed(e);
        }
        protected override void Dispose(bool disposing) { Hide(); }
        protected void DoNotifyData(object Object) { if (NotifyData != null) NotifyData(Object); }
        protected object DoGetData(object[] Args)
        {
            if (GetData == null) return null;
            return GetData(Args);
        }
        public bool CheckPermission()
        {
            return true;
        }
        public void ShowWaitForm(string Hint = null, bool LockForm = false)
        {
            if (!WindowManager.Splash.IsSplashFormVisible) WindowManager.Splash.ShowWaitForm();
            if (WindowManager.Splash.Properties == null) return;
            if (LockForm) WindowManager.Splash.Properties.ParentForm.Enabled = false;
            if (Hint != null) WindowManager.Splash.SetWaitFormDescription(Hint);
        }
        public void CloseWaitForm()
        {
            if (WindowManager.Splash.IsSplashFormVisible) WindowManager.Splash.CloseWaitForm();
            if (WindowManager.Splash.Properties == null) return;
            if (!WindowManager.Splash.Properties.ParentForm.Enabled) WindowManager.Splash.Properties.ParentForm.Enabled = true;
        }

        private void XtraBaseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                CloseWaitForm();
            }
        }
        protected void ShowWaitForm(string Hint = null, int interval = Const.BaseConst.RequestTimeOut, bool LockForm = false)
        {
            ShowWaitForm(Hint, LockForm);
            if (WaitingTimer == null)
            {
                WaitingTimer = new Timer();
                WaitingTimer.Tick += (s, e1) =>
                {
                    CloseWaitForm();
                    WaitingTimer.Stop();
                };
            }
            WaitingTimer.Interval = interval < 200 ? 500 : interval;
            WaitingTimer.Start();
        }
    }
}