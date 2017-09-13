using FirstFrame.PacketProtocol;
using FirstFrame.PushMessage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmsPushTester
{
    public partial class frmMain : Form
    {
        private AlidayuSmsPusher _SmsPush = new AlidayuSmsPusher();
        private string SerialNumber = string.Empty;
        public frmMain()
        {
            InitializeComponent();
        }

        private void btSend_Click(object sender, EventArgs e)
        {
            string SendResult = _SmsPush.SendSmsVerifyCode(edMobilePhone.Text, "VerifyCodeTester", "晨曦外汇交易助手");
            if (ProtocolManager.GetCode(SendResult) == "0")
            {
                SerialNumber = ProtocolManager.GetMessage(SendResult).ToString();
                lbExpireTime.Text = "60";
                VerifyCodeTimer.Enabled = true;
            }
            edSendLog.AppendText(SendResult);
        }

        private void btCheck_Click(object sender, EventArgs e)
        {
            if(_SmsPush.CheckVerifyCode(edMobilePhone.Text, SerialNumber, edVerifyCode.Text))
            {
                edSendLog.AppendText("验证码通过");
            }
            else
            {
                edSendLog.AppendText("验证码无效");
            }
        }

        private void VerifyCodeTimer_Tick(object sender, EventArgs e)
        {
            if (int.Parse(lbExpireTime.Text) == 0)
            {
                VerifyCodeTimer.Enabled = false;
                return;
            }
            lbExpireTime.Text = (int.Parse(lbExpireTime.Text) - 1).ToString();
        }
    }
}
