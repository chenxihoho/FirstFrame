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
    public sealed class Producer
    {
        private static string MessageQueueServer = string.Empty;
        private static string MessageQueueUserName = string.Empty;
        private static string MessageQueuePassword = string.Empty;

        private static IConnectionFactory ConnectionFactory = null;
        private IMessageProducer _Producer = null;
        private string QueueName = string.Empty;
        public Producer(string QueueName)
        {
            this.QueueName = QueueName;

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
        #region 生产者
        public void InitProducer(string QueueName)
        {
            if (ConnectionFactory == null) return;

            IConnection Connection = ConnectionFactory.CreateConnection(MessageQueueUserName, MessageQueuePassword); //通过工厂构建连接
            ISession Session = Connection.CreateSession(); //通过连接创建一个会话
            _Producer = Session.CreateProducer(new ActiveMQQueue(QueueName)); //通过会话创建一个消费者，这里就是Queue这种会话类型的监听参数设置
        }
        public void Send(string Content, TimeSpan TimeToLive, string Key, string KeyValue, MsgDeliveryMode _MsgDeliveryMode)
        {
            if (_Producer == null) InitProducer(this.QueueName);
            //if (_Producer == null) return; 

            ITextMessage Message = _Producer.CreateTextMessage();
            Message.Text = Content;
            Message.Properties.SetString(Key, KeyValue);
            _Producer.Send(Message, _MsgDeliveryMode, MsgPriority.Normal, TimeToLive);
        }
        #endregion
    }
}
