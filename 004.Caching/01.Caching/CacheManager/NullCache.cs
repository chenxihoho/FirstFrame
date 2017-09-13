using System;

namespace FirstFrame.Caching
{
    /// <summary>
    /// Null Cache
    /// </summary>
    public class NullCache : ICache
    {
        private static readonly NullCache Instance = new NullCache();

        private NullCache()
        { }

        public static NullCache GetInstance()
        {
            return Instance;
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

        public void RemoveKey(string key)
        {
            
        }
        public void RemovePlatform(string PlatformID)
        {

        }
        public bool IsConnected()
        {
            return false;
        }
    }
}
