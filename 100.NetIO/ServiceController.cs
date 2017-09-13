using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Runtime.InteropServices;
using System.ServiceModel;

#pragma warning disable 1591

namespace FirstFrame.NetIO
{
    public class ServiceController<T>
    {        
        private Communicator<T> _Communicator;
        public ServiceController(Communicator<T> Sender) { _Communicator = Sender; }
        public void Send(Action _Action, OnClientCallbackEventHandler Callback)
        {
            Thread ServiceThread = new Thread(delegate() { WcfService(_Action); });
            ServiceThread.Start();            
        }
        private void WcfService(Action _Action)
        {
            Invoke(() => _Action());
        }

        public long RegisterCallback(T _UserProxy, OnClientCallbackEventHandler Callback, object Object, int RequestTimeOut)
        {           
            long Method;
            if (Callback == null) //以同步方式调用，可以不提供回调函数
            {
                Method = 0;
            }
            else
            {
                Method = Marshal.GetFunctionPointerForDelegate(Callback).ToInt32();
            }
            ClientChannel<T> _ClientChannel = new ClientChannel<T>();
            _ClientChannel.OnClientCallbackEventHandler = Callback;
            _ClientChannel.ChannelWaitHandle = new AutoResetEvent(false);
            _ClientChannel.UserProxy = _UserProxy;
            _ClientChannel.Object = Object;
            _ClientChannel.RequestTimeOut = RequestTimeOut;
            _ClientChannel.ExecutedTime = 0;
            _Communicator.RegisterCallback(Method, _ClientChannel);
            return Method;
        }

        public void Invoke(Action _Action)
        {
            try
            {
                _Action();
            }
            catch (ProtocolException)
            {
                _Communicator.ResetUserProxy(false);
                if (_Communicator.OnNetworkException != null) { _Communicator.OnNetworkException.Invoke(); }
            }
        }
    }
}
