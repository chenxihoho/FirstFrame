using System;

namespace FirstFrame.Caching
{
    /// <summary>
    /// AbstractCache
    /// </summary>
    public abstract class AbstractCache : ICache
    {
        public abstract void SetCache(string key, object value, TimeSpan expiresIn);
        public abstract object GetCache(string key);
        public abstract void ClearCache(string key);

        public void Set(string key, object value, TimeSpan expiresIn)
        {
            SetCache(key, value, expiresIn);
        }

        public object Get(string key)
        {
            var obj = GetCache(key);

            return obj;
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
            ClearCache(Key);
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
