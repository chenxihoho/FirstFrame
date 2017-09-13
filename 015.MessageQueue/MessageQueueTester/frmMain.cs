using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Apache.NMS.ActiveMQ.Commands;
using FirstFrame.Const;
using FirstFrame.Helper.Log;
using FirstFrame.MessageQueue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessageQueueTester
{
    public partial class frmMain : Form
    {
        public static Producer MessageProducer = new Producer(BusinessType.BT_YH_USER);
        public static Consumer MessageConsumer = new Consumer(BusinessType.BT_YH_USER, Dns.GetHostName() + "-" + Process.GetCurrentProcess().Id.ToString(), "USER", "Registered");
        public frmMain()
        {
            InitializeComponent();
            MessageConsumer.StartListen(OnRecvMessage);
        }
        public void OnRecvMessage(ITextMessage Message)
        {
            //LogHelper.Debug(string.Format("接收到用户注册消息：{0}", Message.Text));
            tbReceiveMessage.Invoke(new DelegateRevMessage(RevMessage), Message);
        }
        public delegate void DelegateRevMessage(ITextMessage message);
        public void RevMessage(ITextMessage Message)
        {
            Message.Acknowledge(); //签收消息
            tbReceiveMessage.Text += string.Format(@"接收到:{0}{1}", Message.Text, Environment.NewLine);
        }
        private void btSendMessage_Click(object sender, EventArgs e)
        {
            Stopwatch _Stopwatch = new Stopwatch();
            _Stopwatch.Start();

            int SendCount = Convert.ToInt32(tbSendCount.Text);
            for (int i = 0; i < SendCount; i++)
            {
                MessageProducer.Send(tbSendMessage.Text + i.ToString(), new TimeSpan(0), "USER", "Registered", MsgDeliveryMode.Persistent);
            }

            _Stopwatch.Stop();
            MessageBox.Show(string.Format("发送耗时：{0}", _Stopwatch.Elapsed.ToString()));
        }
    }
}
