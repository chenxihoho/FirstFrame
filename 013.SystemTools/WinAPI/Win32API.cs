using FirstFrame.Const;
using System;
using System.Runtime.InteropServices;

namespace FirstFrame.WinAPI
{
    public class Win32API
    {       
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpClassName, string lpWindowName);

        public static void PostLogMessage(IntPtr MainHandle, int Message, string LogString, bool NewLine = false, bool ShowTime = false, int SleepTime = 2000)
        {
            Win32API.COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)100;
            LogString = ShowTime == true ? DateTime.Now.ToString() + " " + LogString : LogString;
            cds.lpData = NewLine == true ? "\r\n\r\n" + LogString : LogString;
            byte[] sarr = System.Text.Encoding.UTF8.GetBytes(cds.lpData);
            int len = sarr.Length;
            cds.cbData = len + 1;
            SendMessage(MainHandle, Message, 0, ref cds);
            System.Threading.Thread.Sleep(SleepTime);
        }
        public static void PostUserMessage(IntPtr MainHandle, int Message, string Data = BaseConst.NullString)
        {
            
            if(!string.IsNullOrEmpty(Data))
            {
                Win32API.COPYDATASTRUCT cds;
                cds.dwData = (IntPtr)100;
                cds.lpData = Data;
                byte[] sarr = System.Text.Encoding.UTF8.GetBytes(cds.lpData);
                int len = sarr.Length;
                cds.cbData = len + 1;
                SendMessage(MainHandle, Message, 0, ref cds);
            }
            else
            {
                SendMessage(MainHandle, Message, 0, 0);
            }         
        }
        /// <summary>
        /// 自定义的结构
        /// </summary>
       // 注意：必须是结构体不能是类即必须为struct关键字不能是class,否则在接收消息时会产生异常 
        public struct User_lParam        
        {
            public int i;
            public string s;
        }
        /// <summary>
        /// 使用COPYDATASTRUCT来传递字符串
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        //消息发送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            int lParam          //参数2
        );

        
        //消息发送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            ref User_lParam lParam //参数2
        );
        
        //消息发送API
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            ref  COPYDATASTRUCT lParam  //参数2
        );

        //消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            int lParam            // 参数2
        );
        
        
        
        //消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            ref User_lParam lParam //参数2
        );
        
        //异步消息发送API
        [DllImport("User32.dll", EntryPoint = "PostMessage")]
        public static extern int PostMessage(
            IntPtr hWnd,        // 信息发往的窗口的句柄
            int Msg,            // 消息ID
            int wParam,         // 参数1
            ref  COPYDATASTRUCT lParam  // 参数2
        );

    }
}