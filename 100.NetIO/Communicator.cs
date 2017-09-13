using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.ServiceModel;
using System.Runtime.InteropServices;
using System.IO;
using ICSharpCode.SharpZipLib;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;
using FirstFrame.Helper;
using System.ServiceModel.Channels;
using FirstFrame.Helper.Log;

#pragma warning disable 1591

namespace FirstFrame.NetIO
{
    public delegate void OnNetworkException();
    public delegate void OnServerCallbackEventHandler(long Method, string Param, bool TransportCompressed);
    public delegate void OnClientCallbackEventHandler(long Method, object Object, string Param);
    public delegate void OnNotifyPerformance(int ChannelCount, int CallbackCount, int RequestCount);
    public delegate void OnCallServerRefuse(string Param);

    public class ClientChannel<T>
    {
        public T UserProxy;
        public object Object;
        public OnClientCallbackEventHandler OnClientCallbackEventHandler;
        public AutoResetEvent ChannelWaitHandle;
        public int ExecutedTime = 0;
        public int RequestTimeOut = 5000; //请求超时设置
    }

    public class Communicator<T>
    {
        private string ServiceName = string.Empty;
        private Binding ClientBinding = null;
        private string CertificateName = string.Empty;
        private string EndpointUrl = string.Empty;
        private string Identity = string.Empty;
        private bool GUI = true;
        private object Locker = new object();
        public AutoResetEvent CommunicatorWaitHandle = new AutoResetEvent(false);
        private bool Asynchronous = true;
        private string ResultString = string.Empty; //同步调用的返回值
        private ServiceController<T> ServiceController;
        public OnNetworkException OnNetworkException;
        public OnNetworkException OnCertificateException;
        public OnCallServerRefuse OnCallServerRefuse;
        public OnClientCallbackEventHandler OnRequestTimeOut;
        public T UserProxy;
        private Callback _Callback;
        public InstanceContext instanceContext;
        private SynchronizationContext _SynchronizationContext;
        private Dictionary<long, ClientChannel<T>> MethodList = new Dictionary<long, ClientChannel<T>>();
        public DuplexChannelFactory<T> channelFactory;
        public Timer MonitorTimer;
        private const int DefaultRequestTimeOut = 10000; //默认调用超时
        public int RequestIntervalTime = 400; //防止服务器过压检测关闭连接
        public int ChannelCount = 0;
        public int RequestCount = 0;
        public int CallbackCount = 0;

        public Communicator(string _ServiceName, bool GUI) 
        {
            ServiceName = _ServiceName;
            this.GUI = GUI;
            ServiceController = new ServiceController<T>(this);
            MonitorTimer = new Timer(new TimerCallback(TimeOutCheck), null, 0, 1000);            
        }
        public Communicator(Binding _ClientBinding, string _EndpointUrl, string _Identity, string _CertificateName, bool GUI)
        {
            ClientBinding = _ClientBinding;
            EndpointUrl = _EndpointUrl;
            Identity = _Identity;
            CertificateName = _CertificateName;
            this.GUI = GUI;
            ServiceController = new ServiceController<T>(this);
            MonitorTimer = new Timer(new TimerCallback(TimeOutCheck), null, 0, 1000);
        }
        /// <summary>
        /// 向服务器发送一个请求
        /// </summary>
        /// <param name="_Action">回调执行方法</param>
        /// <param name="Asynchronous">是否异步发送请求，如果此参数为false，则整个通信器将阻塞，直到调用释放阻塞为止</param>
        public string SendRequest(Action _Action, bool Asynchronous = true)
        {
            RequestCount++;
            this.Asynchronous = Asynchronous;
            Thread.Sleep(RequestIntervalTime);            
            ServiceController.Invoke(() => _Action());
            if (!Asynchronous) CommunicatorWaitHandle.WaitOne();
            return ResultString;
        }
        /// <summary>
        /// 等待前一个请求完成，此方法将阻塞当前线程，直到调用ReleaseLastBlock方法
        /// </summary>
        public void WaitLastRequestComplete()
        {
            AutoResetEvent _AutoResetEvent = GetLastRequestWaitHandle();
            if (_AutoResetEvent != null) _AutoResetEvent.WaitOne();
        }
        /// <summary>
        /// 获取前一个请求的AutoResetEvent
        /// </summary>
        /// <returns>AutoResetEvent，如果回调方法列表为空则返回null</returns>
        public AutoResetEvent GetLastRequestWaitHandle()
        {
            ClientChannel<T> _ClientChannel;
            lock (Locker)
            {
                if (MethodList.Count == 0) return null;
                _ClientChannel = MethodList.LastOrDefault().Value;
            }
            return _ClientChannel.ChannelWaitHandle;
        }
        /// <summary>
        /// 释放请求阻塞或通信器阻塞
        /// </summary>
        /// <param name="_AutoResetEvent">指定要释放的AutoResetEvent，传null参数则释放通信器的阻塞</param>
        public void ReleaseBlock(AutoResetEvent _AutoResetEvent = null)
        {
            if (_AutoResetEvent != null)
                _AutoResetEvent.Set();
            else
                CommunicatorWaitHandle.Set();
        }
        /// <summary>
        /// 释放上一个请求的阻塞
        /// </summary>
        public void ReleaseLastRequestBlock()
        {
            AutoResetEvent _AutoResetEvent = GetLastRequestWaitHandle();
            if (_AutoResetEvent != null) _AutoResetEvent.Set();
        }
        /// <summary>
        /// 释放全部请求阻塞
        /// </summary>
        public void RelaseAllRequestBlock()
        {
            lock (Locker)
            {
                for (int i = MethodList.Count - 1; i >= 0; i--)
                {
                    ClientChannel<T> _ClientChannel = MethodList.ElementAt(i).Value;
                    _ClientChannel.ChannelWaitHandle.Set();
                }
            }
        }
        /// <summary>
        /// 注册回调事件
        /// </summary>
        /// <param name="UserProxy">客户端通道</param>
        /// <param name="Callback">回调方法</param>
        /// <param name="RequestTimeOut">超时设置</param>
        /// <param name="Object">附加对象，可在回调方法中再次获取</param>
        /// <returns>回调事件地址</returns>
        public long RegisterCallback(T UserProxy, OnClientCallbackEventHandler Callback, object Object, int RequestTimeOut = DefaultRequestTimeOut)
        {
            return ServiceController.RegisterCallback(UserProxy, Callback, Object, RequestTimeOut);
        }
        private void TimeOutCheck(object state)
        {
            lock (Locker)
            {
                for (int i = MethodList.Count - 1; i >= 0; i--)
                {
                    ClientChannel<T> _ClientChannel = MethodList.ElementAt(i).Value;
                    _ClientChannel.ExecutedTime += 1000;
                    if (_ClientChannel.ExecutedTime >= _ClientChannel.RequestTimeOut)
                    {                        
                        if (!Asynchronous) //同步方式调用，直接返回值
                        {
                            ResultString = "-1";
                            CommunicatorWaitHandle.Set();
                        }
                        else
                        {
                            OnClientCallbackEventHandler _CallBack = new OnClientCallbackEventHandler(_ClientChannel.OnClientCallbackEventHandler);
                            if (GUI)
                            {
                                _SynchronizationContext.Post(delegate
                                {
                                    _CallBack(-1, _ClientChannel.Object, string.Empty); //回调可能操作UI控件，因此放入UI线程中执行  
                                    if (OnRequestTimeOut != null) { OnRequestTimeOut.Invoke(-1, _ClientChannel.Object, string.Empty); }
                                }, null);
                            }
                            else
                            {
                                _CallBack(-1, _ClientChannel.Object, string.Empty); //如果超时，触发回调事件进行通知 
                                if (OnRequestTimeOut != null) { OnRequestTimeOut.Invoke(-1, _ClientChannel.Object, string.Empty); }
                            }
                            try
                            {
                                if (_ClientChannel.UserProxy != null) ((IClientChannel)_ClientChannel.UserProxy).Close();
                            }
                            catch (Exception)
                            {
                                if (_ClientChannel.UserProxy != null) ((IClientChannel)_ClientChannel.UserProxy).Abort();
                            }
                        }
                        MethodList.Remove(MethodList.ElementAt(i).Key);
                    }
                }
            }
        }
        private void ChannelClosing(object sender, EventArgs e)
        {
            ChannelCount--;
            ResetUserProxy(true);
        }
        private void SendTimeOut(object sender, EventArgs e)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("会话超时：{0} {1}", this.GetHashCode().ToString("x"), DateTime.Now.ToString());
        }
        public void ResetUserProxy(bool IsDispose)
        {            
            if (IsDispose)
                UserProxy = default(T);
            else
                if (((ICommunicationObject)UserProxy).State != CommunicationState.Faulted) { ((IClientChannel)UserProxy).Close(); }
        }
        public T GetUserProxy()
        {
            try
            {
                if ((UserProxy != null) && ((ICommunicationObject)UserProxy).State == CommunicationState.Opened){ return UserProxy;}

                ChannelCount++;
                _Callback = new Callback(this);
                _SynchronizationContext = SynchronizationContext.Current;
                _Callback.SynchronizationContext = _SynchronizationContext;
                instanceContext = new InstanceContext(_Callback);
                if (!string.IsNullOrEmpty(ServiceName))
                {
                    channelFactory = new DuplexChannelFactory<T>(instanceContext, ServiceName);
                }
                else
                {
                    EndpointAddress Endpoint = new EndpointAddress(new Uri(EndpointUrl), EndpointIdentity.CreateDnsIdentity(Identity));
                    channelFactory = new DuplexChannelFactory<T>(instanceContext, ClientBinding, Endpoint);
                    channelFactory.Credentials.ClientCertificate.SetCertificate(StoreLocation.CurrentUser, StoreName.Root, X509FindType.FindBySubjectName, CertificateName);
                    channelFactory.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.None; //生产环境中需要使用 ChainTrust
                }
                UserProxy = channelFactory.CreateChannel();
                ((IClientChannel)UserProxy).Closing += ChannelClosing;
                ((IClientChannel)UserProxy).OperationTimeout =  new TimeSpan(0, 0, 10);
                ((IClientChannel)UserProxy).Open();               
            }
            catch (EndpointNotFoundException e)
            {
                lock (Locker) { MethodList.Clear(); } 
                ResetUserProxy(false);
                if (OnNetworkException != null) { OnNetworkException.Invoke(); }
                LogHelper.WriteLog("GetUserProxy", e.Message);
                return default(T);
            }
            catch (InvalidOperationException e)
            {
                lock (Locker) { MethodList.Clear(); }  //出现证书问题，清除掉全部调用
                if (OnCertificateException != null) { OnCertificateException.Invoke(); }
                LogHelper.WriteLog("GetUserProxy", e.Message);
                return default(T);
            }
            catch(Exception e)
            {
                lock (Locker) { MethodList.Clear(); } 
                ResetUserProxy(true);
                if (OnNetworkException != null) { OnNetworkException.Invoke(); }
                LogHelper.WriteLog("GetUserProxy", e.Message);
                return default(T);
            }
            return UserProxy;
        }

        [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Reentrant, UseSynchronizationContext = false)]
        private class Callback : IMessageCallback
        {
            private Communicator<T> _Communicator;
            public Callback(Communicator<T> Sender) { _Communicator = Sender; }
            public SynchronizationContext SynchronizationContext;
            public void OnCallback(long Method, string Param, bool TransportCompressed)
            {
                _Communicator.CallbackCount++;
                long _Method = (int)(Method);//必须取long型高位
                try
                {                    
                    #region 同步方式调用，直接返回值
                    if (!_Communicator.Asynchronous)
                    {
                        _Communicator.ResultString = Decompress(Param, TransportCompressed);
                        return;
                    }
                    #endregion
                    #region 异步方式返回
                    if (_Communicator.MethodList.ContainsKey(_Method))
                    {
                        ClientChannel<T> _ClientChannel = _Communicator.MethodList[_Method];
                        _ClientChannel.ExecutedTime = 0;
                        OnClientCallbackEventHandler _CallBack = new OnClientCallbackEventHandler(_ClientChannel.OnClientCallbackEventHandler);
                        if (Method >> 32 == -1000) //返回结果为无权限
                        {                            
                            if (_Communicator.OnCallServerRefuse != null)
                            {
                                SynchronizationContext.Post(delegate
                                {
                                    { _Communicator.OnCallServerRefuse.Invoke(Param); } //回调可能操作UI控件，因此放入UI线程中执行    
                                }, null);
                            }
                        }
                        else
                        {
                            if (_Communicator.GUI)
                            {
                                SynchronizationContext.Post(delegate
                                {
                                    _CallBack(_Method, _ClientChannel.Object, Decompress(Param, TransportCompressed)); //回调可能操作UI控件，因此放入UI线程中执行   
                                }, null);
                            }
                            else
                            {
                                _CallBack(_Method, _ClientChannel.Object, Decompress(Param, TransportCompressed));
                            }
                        }
                    }
                    #endregion
                }
                finally
                {
                    //调用完成后清除委托
                    lock (_Communicator.Locker) {_Communicator.MethodList.Remove(_Method); }
                    if (!_Communicator.Asynchronous) _Communicator.CommunicatorWaitHandle.Set(); //清除同步阻塞
                }
            }
        }
        public void RegisterCallback(long Method, ClientChannel<T> _ClientChannel)
        {
            if (!MethodList.ContainsKey(Method)) {  MethodList.Add(Method, _ClientChannel); }
        }
        private void BeginTimeOut()
        {
            NetworkTimeOutThread _NetworkTimeOutThread = new NetworkTimeOutThread(this);
            Thread _TimeOutThread = new Thread(new ThreadStart(_NetworkTimeOutThread.Start));
            _TimeOutThread.Start();
        }
        private class NetworkTimeOutThread
        {
            private Communicator<T> _Communicator;
            public NetworkTimeOutThread(Communicator<T> Sender) { _Communicator = Sender; }
            public void Start()
            {
                Thread.Sleep(5000); //5秒后检查是否已连通
                if (_Communicator.channelFactory.State != CommunicationState.Opened)
                {
                    _Communicator.ResetUserProxy(false);
                    if (_Communicator.OnNetworkException != null) { _Communicator.OnNetworkException.Invoke(); }
                    lock (_Communicator.Locker) { _Communicator.MethodList.Clear(); } 
                }
            }
        }
        private string Compress(string Param)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(Param);
            MemoryStream ms = new MemoryStream();
            Stream stream = new ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream(ms);
            try
            {
                stream.Write(data, 0, data.Length);
            }
            finally
            {
                stream.Close();
                ms.Close();
            }
            return Convert.ToBase64String(ms.ToArray());
        }
        private static string Decompress(string Param, bool TransportCompressed)
        {
            if (!TransportCompressed) return Param;

            string commonString=string.Empty;
            byte[] buffer = Convert.FromBase64String(Param);
            MemoryStream ms = new MemoryStream(buffer);
            Stream sm = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(ms);
            StreamReader reader = new StreamReader(sm,System.Text.Encoding.UTF8);
            try
            {
                commonString=reader.ReadToEnd();
            }
            finally
            {
                sm.Close();
                ms.Close();
            }
            return commonString;
        }
        private void Close()
        {
            lock (Locker)
            {
                foreach (var item in MethodList.Values)
                {
                    try
                    {
                        ((IClientChannel)item.UserProxy).Close();
                    }
                    catch (Exception)
                    {
                        ((IClientChannel)item.UserProxy).Abort();
                    }
                }
            }
        }
    }
}
