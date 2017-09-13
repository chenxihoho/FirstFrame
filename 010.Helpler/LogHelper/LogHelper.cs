using Apache.NMS;
using FirstFrame.Const;
using FirstFrame.MessageQueue;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using System;
using System.Diagnostics;
using System.IO;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Log.config", Watch = true)]

namespace FirstFrame.Helper.Log
{
    /// <summary>
    /// 日志类：类别包括：FATAL（致命错误）、ERROR（错误）、WARN（警告）、INFO（信息）、DEBUG（调试信息）
    /// </summary>
    public class LogHelper
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static ILog NoConfigLog = null;
        private static string NewLineString = "\r\n";

        public static Producer LogProducer = new Producer(BusinessType.YH_LOG);
        #region 构造函数
        static LogHelper()
        {
            #region 无配置文件使用log4net
            //string LOG_PATTERN = "%d [%t] %-5p %c [%x] - %m%n";
            //string LOG_FILE_PATH = System.IO.Directory.GetCurrentDirectory() + "\\Log\\" + DateTime.Now.ToString("yyyy-MM-dd") + ".LOG";

            //Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            //hierarchy.Name = "FirstFrame.Log.NoConfig";
            //TraceAppender tracer = new TraceAppender();

            //PatternLayout patternLayout = new PatternLayout();
            //patternLayout.ConversionPattern = LOG_PATTERN;
            //patternLayout.ActivateOptions();

            //tracer.Layout = patternLayout;
            //tracer.ActivateOptions();
            //hierarchy.Root.AddAppender(tracer);

            //RollingFileAppender roller = new RollingFileAppender();
            //roller.Layout = patternLayout;
            //roller.AppendToFile = true;
            //roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            //roller.MaxSizeRollBackups = 4;
            //roller.MaximumFileSize = "10240KB"; 
            //roller.StaticLogFileName = true;
            //roller.File = LOG_FILE_PATH;
            //roller.ActivateOptions();
            //hierarchy.Root.AddAppender(roller);

            //hierarchy.Root.Level = log4net.Core.Level.All;
            //hierarchy.Configured = true;

            //NoConfigLog = LogManager.GetLogger("FirstFrame.Log.NoConfig");
            #endregion
        }
        #endregion
        public static void Debug(string Message, bool SendMQ = false, MsgDeliveryMode DeliveryMode = MsgDeliveryMode.NonPersistent)
        {
            Log.Debug(Message);
            if (SendMQ) LogProducer.Send(Message, new TimeSpan(30, 0, 0, 0), "LOG", "Debug", DeliveryMode); //日志消息在消息队列中保存一个月，不做持久化
        }
        public static void Info(string Message)
        {
            Log.Info(Message);
        }
        public static void Warn(string Message)
        {
            Log.Warn(Message);
        }
        public static void Error(string Message, bool SendMQ = false, MsgDeliveryMode DeliveryMode = MsgDeliveryMode.NonPersistent)
        {
            Log.Error(new StackFrame(1).GetMethod().Name + "：" + Message);
            if (SendMQ) LogProducer.Send(Message, new TimeSpan(30, 0, 0, 0), "LOG", "Debug", DeliveryMode); //日志消息在消息队列中保存一个月，不做持久化
        }
        public static void Fatal(string Message)
        {
            Log.Fatal(Message);
        }
        #region 无配置文本日志
        public static void WriteLog(string Sender, string Message)
        {
            log4net.ILog _Log = log4net.LogManager.GetLogger(Sender);
            _Log.Error(Message);
        }

        public static void WriteString(string Message, ref Exception e)
        {
            NoConfigLog.Debug(Message + e == null ? string.Empty : NewLineString + e.Message + NewLineString + e.StackTrace.ToString());
        }
        public static void WriteTxtLog(string Message)
        {
            string LogPath = System.IO.Directory.GetCurrentDirectory() + "\\Log\\";
            string LogFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".LOG";
            CreateDirectory(LogPath);
            using (FileStream fs = new FileStream(LogPath + LogFileName, FileMode.Create))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    //开始写入
                    sw.Write(Message);
                    //清空缓冲区
                    sw.Flush();
                    //关闭流
                    sw.Close();
                    fs.Close();
                }
            }
        }
        public static void CreateDirectory(string LogPath)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(LogPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }
        #endregion
    }
}


