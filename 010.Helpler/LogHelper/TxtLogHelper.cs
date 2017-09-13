using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstFrame.Helper.Log
{
    public class TxtLogHelper : TraceListener
    {
        private string FileName; // 文件名
        private string FilePath; // 文件路径

        public string FileFullPath
        {
            get
            {
                if (Directory.Exists(FilePath) == false)
                {
                    Directory.CreateDirectory(FilePath);
                }
                return FilePath.TrimEnd('/') + "/" + FileName;                
            }

        }

        public TxtLogHelper()
        {
            FileName = DateTime.Now.ToString("yyyy-MM-dd") + ".txt"; // 默认文件名为 今天的日期
            FilePath = AppDomain.CurrentDomain.BaseDirectory + "/Log";  // 默认路径在当前域log文件夹下面     
        }
        public TxtLogHelper(string _FileName)
        {
            FileName = _FileName;
            FilePath = AppDomain.CurrentDomain.BaseDirectory + "/Log";  //默认路径在当前域log文件夹下面 
        }

        public override void Write(string Message)
        {
            WriteLine(Message);
        }

        public override void WriteLine(string Message)
        {
            WriteLine(null, Message);
        }

        /// <summary>
        /// 将异常或信息写入日志
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="message"></param>
        public override void WriteLine(object _Exception, string Message)
        {
            string sMsg = Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine;
            if (!string.IsNullOrEmpty(Message))  //如果信息不为空，加在最前面
            {
                sMsg += Message;
            }
            if (_Exception is Exception)
            {
                Exception ex = (Exception)_Exception;
                sMsg += ex.Message + Environment.NewLine; // 错误提示
                sMsg += ex.StackTrace;  // 堆栈信息
            }
            else if (_Exception != null)
            {
                sMsg += _Exception.ToString();
            }
           

            File.AppendAllText(FileFullPath, sMsg);
        }
    }
}
