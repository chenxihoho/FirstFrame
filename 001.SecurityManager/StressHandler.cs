using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;

namespace FirstFrame.Security
{
    public class ClientEndPoint
    {
        public RemoteEndpointMessageProperty endPoint;
        public OperationContext Context;
        public List<long> LastRequestTime = new List<long>();
        public long ResurrectionTime = 0;
    }
    public interface IStressHandler
    {
        bool StressLimit(OperationContext Context, RemoteEndpointMessageProperty endPoint);
        void StressCheck(object state);
    }
    public class StressHandler : IStressHandler
    {
        public bool StressSafe { get { return GetStressSafe(); } }
        private const int RequestRateLimit = 5; //请求频率阀值，单位 次/秒。
        private const int ResurrectionTime = 1000 * 10; //屏蔽时间，单位毫秒。
        public static Timer MonitorTimer;
        private static Dictionary<string, ClientEndPoint> BlockList = new Dictionary<string, ClientEndPoint>();        
        private static readonly StressHandler instance = new StressHandler();
        public StressHandler()
        {
            MonitorTimer = new Timer(new TimerCallback(StressCheck), null, 0, 1000); //检查间隔时间
        }
        public static StressHandler GetInstance() { return instance; }
        public bool GetStressSafe()
        {
            MessageProperties properties = OperationContext.Current.IncomingMessageProperties;
            RemoteEndpointMessageProperty endPoint = properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (StressLimit(OperationContext.Current, endPoint)) { return false; }
            return true;
        }
        public bool StressLimit(OperationContext Context, RemoteEndpointMessageProperty endPoint)
        {
            lock (instance)
            {
                string IPPort = endPoint.Address;
                if (!BlockList.ContainsKey(IPPort))
                {
                    ClientEndPoint _ClientEndPoint = new ClientEndPoint();
                    _ClientEndPoint.endPoint = endPoint;
                    _ClientEndPoint.Context = Context;
                    _ClientEndPoint.LastRequestTime.Add(Environment.TickCount);
                    BlockList.Add(IPPort, _ClientEndPoint);
                    return false;
                }
                else
                {
                    int StressTime = 0;
                    ClientEndPoint _ClientEndPoint = BlockList[IPPort];
                    if (_ClientEndPoint.ResurrectionTime > 0) //在屏蔽期间继续请求
                    {
                        _ClientEndPoint.ResurrectionTime = ResurrectionTime; //重新设置屏蔽时间
                        try
                        {
                            _ClientEndPoint.Context.Channel.Close();
                        }
                        catch (Exception)
                        {
                            _ClientEndPoint.Context.Channel.Abort();
                        }
                        return true;
                    }

                    _ClientEndPoint.LastRequestTime.Add(Environment.TickCount);  //加入请求时间点                    
                    if (_ClientEndPoint.LastRequestTime.Count > RequestRateLimit) //最多只在指定的阀值内检查
                    {
                        for (int i = _ClientEndPoint.LastRequestTime.Count - 1; i >= 1; i--) //开始检查
                        {
                            if ((_ClientEndPoint.LastRequestTime[i] - _ClientEndPoint.LastRequestTime[i - 1]) < 300) //相邻的两次请求太近
                            {
                                StressTime++; //计1次
                                if (StressTime < RequestRateLimit) continue; //小于压力阀值则不处理

                                _ClientEndPoint.ResurrectionTime = ResurrectionTime; //设置屏蔽时间
                                try
                                {
                                    _ClientEndPoint.Context.Channel.Close();
                                }
                                catch (Exception)
                                {
                                    _ClientEndPoint.Context.Channel.Abort();
                                }
                                return true;
                            }
                        }
                        _ClientEndPoint.LastRequestTime.RemoveAt(0);
                    }
                    return false;
                }
            }
        }
        public void StressCheck(object State)
        {
            lock (instance)
            {
                for (int i = 0; i < BlockList.Count; i++)
                {
                    ClientEndPoint _ClientEndPoint = BlockList.ElementAt(i).Value;
                    if (_ClientEndPoint != null)
                    {
                        if (_ClientEndPoint.ResurrectionTime > 0) { _ClientEndPoint.ResurrectionTime -= 1000; }
                        if (_ClientEndPoint.ResurrectionTime <= 0) //屏蔽时间结束
                        {
                            BlockList.Remove(BlockList.ElementAt(i).Key);
                            continue;
                        }
                    }
                }
            }
        }
    }
}
