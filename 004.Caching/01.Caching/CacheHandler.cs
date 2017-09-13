using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Runtime.Caching;

namespace FirstFrame.Caching
{
    public interface IDataUpdated  //数据更新回调
    {
        void OnDataUpdated();
    }

    public delegate void OnDataUpdated();
    /// <summary>
    /// 数据管理器
    /// </summary>
    public sealed class CacheHandler
    {
        private static object Lock = new object();
        private static ICache _CachedClient = CacheManager.GetCache(); //读取项目项目配置文件来确定缓存类型
        private static Dictionary<IDataUpdated, SynchronizationContext> Subscribers = new Dictionary<IDataUpdated, SynchronizationContext>();
        public static ICache CachedClient { get { return _CachedClient; } }
        public static readonly CacheHandler instance = new CacheHandler();
        public static CacheHandler GetInstance() { return instance; }

        #region 数据订阅事件
        public static void Subscriber(IDataUpdated OnDataUpdated, SynchronizationContext _SynchronizationContext)
        {
            if (!Subscribers.ContainsKey(OnDataUpdated)) Subscribers.Add(OnDataUpdated, _SynchronizationContext);
        }
        public static void UnSubscriber(IDataUpdated OnDataUpdated)
        {
            if (Subscribers.ContainsKey(OnDataUpdated))
            {
                lock (Lock) { Subscribers.Remove(OnDataUpdated); }
            }
        }
        public static void OnDataUpdated()
        {
            lock (Lock)
            {
                foreach (var Subscriber in Subscribers)
                {
                    Subscriber.Value.Post(delegate
                    {
                        Subscriber.Key.OnDataUpdated();
                    }, null);
                }
            }
        }
        #endregion
        public static void Set<T>(string Key, T Value, TimeSpan ExpireTime)
        {
            if (_CachedClient.IsConnected() == false) return;

            _CachedClient.Set(Key, Value, ExpireTime);
        }
        public static T Get<T>(string Key) where T : class
        {
            if (_CachedClient.IsConnected() == false) return null;

            return _CachedClient.Get<T>(Key);
        }
        public static bool Contains(string Key)
        {
            if (_CachedClient.IsConnected() == false) return false;

            return _CachedClient.Contains(Key);
        }
        public static void RemoveKey(string Key)
        {
            if (_CachedClient.IsConnected() == false) return;

            _CachedClient.RemoveKey(Key);
        }
        public static void RemovePlatform(string PlatformID)
        {
            if (_CachedClient.IsConnected() == false) return;

            _CachedClient.RemovePlatform(PlatformID);
        }
    }
}
