using System;

using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace FirstFrame.Caching
{
    /// <summary>
    /// MemCached Cache
    /// </summary>    
    public class MemCached : ICache
    {
        private static readonly MemcachedClient MemClient = new MemcachedClient();
        private static readonly MemCached Instance = new MemCached();

        private MemCached()
        { }

        public static MemCached GetInstance()
        {
            return Instance;
        }

        public void Set(string key, object value, TimeSpan expiresIn)
        {
            MemClient.Store(StoreMode.Set, key, value, expiresIn);
        }

        public object Get(string key)
        {
            return MemClient.Get(key);
        }

        public bool Set<T>(string Key, T Value, TimeSpan ExpireTime)
        {
            return false;
        }

        public T Get<T>(string key) where T : class
        {
            return null;
        }
        public bool Contains(string Key)
        {
            return false;
        }
        public void RemoveKey(string Key)
        {
            MemClient.Remove(Key);
        }
        public void RemovePlatform(string PlatformID)
        {
 
        }
        public bool IsConnected()
        {
            //MemClient.Stats
            return true;
        }
    }
}
