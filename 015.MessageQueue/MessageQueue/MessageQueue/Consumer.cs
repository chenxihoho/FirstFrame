using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Apache.NMS;
using Apache.NMS.ActiveMQ.Commands;
using Apache.NMS.ActiveMQ;

namespace FirstFrame.MessageQueue
{
    public sealed class Consumer
    {
        public delegate void DelegateRecvMessage(ITextMessage Message);
        public event DelegateRecvMessage OnRecvMessage;

        private static string MessageQueueServer = string.Empty;
        private static string MessageQueueUserName = string.Empty;
        private static string MessageQueuePassword = string.Empty;

        private static IConnectionFactory ConnectionFactory = null;
        private IMessageConsumer _Consumer = null;
        private string QueueName = string.Empty;
        private string ClientID = string.Empty;
        private string Key = string.Empty;
        private string KeyValue = string.Empty;
        public Consumer(string QueueName, string ClientID, string Key, string KeyValue)
        {
            this.QueueName = QueueName;
            this.ClientID = ClientID;
            this.Key = Key;
            this.KeyValue = KeyValue;

            InitMessageQueue();
        }
        public void InitMessageQueue()
        {
            if (ConnectionFactory != null) return;

            MessageQueueServer = ConfigurationManager.AppSettings["yhAzure.MessageQueue.Server"];
            MessageQueueUserName = ConfigurationManager.AppSettings["yhAzure.MessageQueue.UserName"];
            MessageQueuePassword = ConfigurationManager.AppSettings["yhAzure.MessageQueue.Password"];
            if (string.IsNullOrEmpty(MessageQueueServer) || string.IsNullOrEmpty(MessageQueueUserName) || string.IsNullOrEmpty(MessageQueuePassword)) return;

            ConnectionFactory = new ConnectionFactory(MessageQueueServer); //创建连接工厂
        }
        #region 消费者
        public void InitConsumer(string QueueName, string ClientID, string Key, string KeyValue)
        {
            if (ConnectionFactory == null) return;

            IConnection Connection = ConnectionFactory.CreateConnection(MessageQueueUserName, MessageQueuePassword); 
            Connection.ClientId = ClientID;
            Connection.Start();
            ISession Session = Connection.CreateSession(AcknowledgementMode.ClientAcknowledge);

            _Consumer = Session.CreateConsumer(new ActiveMQQueue(QueueName), Key + "=" + "'" + KeyValue + "'");
        }
        public void StartListen(DelegateRecvMessage OnRecvMessage)
        {
            if (_Consumer == null) InitConsumer(this.QueueName, this.ClientID, this.Key, this.KeyValue);
            //if (_Consumer == null) return;

            this.OnRecvMessage = OnRecvMessage;
            //注册监听事件
            _Consumer.Listener += new MessageListener(OnMessage);
        }
        void OnMessage(IMessage Message)
        {
            ITextMessage _Message = (ITextMessage)Message;
            if (OnRecvMessage != null) OnRecvMessage(_Message);
        }
        #endregion
    }
}
