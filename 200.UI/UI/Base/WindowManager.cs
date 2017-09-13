/*
* 窗体管理器
* 张国伟
* 2016-4-17
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Runtime.Remoting;
using System.Windows.Forms;
using System.IO;
using DevExpress.XtraSplashScreen;
using FirstFrame.UI.Form;

namespace FirstFrame.UI
{    
    public sealed class WindowManager
    {
        private static object Lock = new object();
        private static Dictionary<string, object> WindowList = new Dictionary<string, object>();
        private static Assembly asm = Assembly.GetExecutingAssembly();
        private static AssemblyName asmName;
        public static object MainForm = null;
        public static SplashScreenManager Splash = new SplashScreenManager(typeof(WaitingForm), new SplashFormProperties());
        private static readonly WindowManager instance = new WindowManager();
        private WindowManager()
        {
            asmName = asm.GetName();
        }
        public static WindowManager GetInstance() { return instance; }

        public static object InitializeWindow(Type _TargerWindow)
        {
            if (!WindowList.ContainsKey(_TargerWindow.Name))
            {
                //DevExpress 在线程中实例化会产生异常，暂时在UI线程中进行
                /*CreateWindowThread _CreateWindowThread = new CreateWindowThread(_TargerWindow);
                Thread _CreateThread = new Thread(new ThreadStart(_CreateWindowThread.InitializeWindow));
                _CreateThread.SetApartmentState(ApartmentState.STA);
                _CreateThread.Start();*/

                return CreateWindow(_TargerWindow);
            }
            else
            {
                return GetWindow(_TargerWindow);
            }
        }
        public static object GetWindow(Type _TargerWindow)
        {
            if (WindowList.ContainsKey(_TargerWindow.Name))
            {
                lock(Lock)
                {
                    return WindowList[_TargerWindow.Name];
                }                    
            }
            else
            {                
                lock(Lock)
                {
                    return CreateWindow(_TargerWindow);
                }
            }
        }
        public static object GetWindow(string _TargerWindow)
        {
            Type type = System.Reflection.Assembly.Load(asmName.Name).GetType(_TargerWindow);
            if (WindowList.ContainsKey(type.Name))
            {
                lock (Lock)
                {
                    return WindowList[type.Name];
                }
            }
            else
            {
                lock (Lock)
                {
                    return CreateWindow(_TargerWindow);
                }
            }
        }
        //在线程中进行窗体缓存
        public class CreateWindowThread
        {
            Type TargerWindow = null;
            public CreateWindowThread(Type _TargerWindow)
            {
                this.TargerWindow = _TargerWindow;
            }
            public void InitializeWindow()
            {
                lock(Lock)
                {
                    //LogHelper.WriteLog(this.GetHashCode().ToString(), TargerWindow.Name);
                    //CreateWindow(TargerWindow);
                }
            }
        }
        public static void DisposeWindow(Type _TargerWindow)
        {
            if (WindowList.ContainsKey(_TargerWindow.Name))
            {
                lock(Lock)
                {
                    WindowList.Remove(_TargerWindow.Name);
                }
            }
        }private static object CreateWindow(Type _TargerWindow)
        {
            Type type = System.Reflection.Assembly.Load(asmName.Name).GetType(string.Format("{0}.{1}", _TargerWindow.Namespace, _TargerWindow.Name));
            object _Window = Activator.CreateInstance(type);

            if (!WindowList.ContainsKey(_TargerWindow.Name)) WindowList.Add(_TargerWindow.Name, _Window);
            return _Window;
        }
        private static object CreateWindow(string _TargerWindow)
        {
            Type type = System.Reflection.Assembly.Load(asmName.Name).GetType(_TargerWindow);
            object _Window = Activator.CreateInstance(type);

            if (!WindowList.ContainsKey(type.Name)) WindowList.Add(type.Name, _Window);
            return _Window;
        }
        public static object ShowWindow(Type _TargerWindow, object Sender, Boolean IsModel)
        {
            object _Window = GetWindow(_TargerWindow);
            string _Method = IsModel ? "ShowDialog" : "Show";
            Type type = System.Reflection.Assembly.Load(asmName.Name).GetType(string.Format("{0}.{1}", _TargerWindow.Namespace, _TargerWindow.Name));
            type.InvokeMember(_Method, BindingFlags.InvokeMethod, null, _Window, null);
            return _Window;
        }

        public static object ShowWindow(string _TargerWindow, object Sender, Boolean IsModel)
        {
            Type type = System.Reflection.Assembly.Load(asmName.Name).GetType(_TargerWindow);
            object _Window = GetWindow(type);
            string _Method = IsModel ? "ShowDialog" : "Show";
            type.InvokeMember(_Method, BindingFlags.InvokeMethod, null, _Window, null);
            return _Window;
        }

        public static void WriteTxtLog(string Message)
        {
            string LogPath = System.IO.Directory.GetCurrentDirectory() + "\\Log\\";
            string LogFileName = DateTime.Now.ToString("yyyy-MM-dd") + ".LOG";
            CreateDirectory(LogPath);
            FileStream fs = new FileStream(LogPath + LogFileName, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write(Message);
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();
        }
        public static void CreateDirectory(string LogPath)
        {
            DirectoryInfo directoryInfo = Directory.GetParent(LogPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
        }
    }   
}
