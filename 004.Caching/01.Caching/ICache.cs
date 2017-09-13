using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirstFrame.Caching
{
    public interface ICache
    {
        bool Set<T>(string Key, T Value, TimeSpan ExpireTime);
        T Get<T>(string key) where T : class;
        bool Contains(string Key);
        void RemoveKey(string Key);
        void RemovePlatform(string PlatformID);
        bool IsConnected();
    }
}
