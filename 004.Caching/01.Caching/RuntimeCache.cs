using System;
using System.Runtime.Caching;

namespace FirstFrame.Caching
{
    /// <summary>
    /// Runtime Cache
    /// </summary>
    public class RuntimeCache : ICache
    {
        private static readonly ObjectCache OCache = MemoryCache.Default;
        private static readonly RuntimeCache Instance = new RuntimeCache();

        private RuntimeCache()
        { }

        public static RuntimeCache GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// Set Cache Object
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public bool Set<T>(string Key, T Value, TimeSpan ExpireTime)
        {
            Set(Key, Value, null);
            return true;
        }

        private void Set(string key, object value, CacheItemPolicy cacheItemPolicy)
        {
            Set(key, value, cacheItemPolicy, null);
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="cacheItemPolicy"></param>
        /// <param name="regionName"></param>
        private void Set(string key, object value, CacheItemPolicy cacheItemPolicy, string regionName)
        {
            OCache.Set(key, value, cacheItemPolicy, regionName);
        }

        /// <summary>
        /// Set
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="dateTimeOffset"></param>
        /// <param name="regionName"></param>
        private void Set(string key, object value, DateTimeOffset dateTimeOffset, string regionName)
        {
            OCache.Set(key, value, dateTimeOffset, regionName);
        }

        /// <summary>
        /// Get Cache Object
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object Get(string Key)
        {
            return OCache.Get(Key);
        }

        public T Get<T>(string Key) where T : class
        {
            return null;
        }

        public bool Contains(string Key)
        {
            return false;
        }
        /// <summary>
        /// Clear Cache Object
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string Key)
        {
            OCache.Remove(Key);
        }
        public void RemovePlatform(string PlatformID)
        {

        }
        public bool IsConnected()
        {
            return true;
        }
    }
}
