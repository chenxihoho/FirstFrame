using System;
using System.Configuration;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace FirstFrame.Caching
{
    /// <summary>
    /// Redis Cache
    /// </summary>
    public class RedisCache : ICache
    {
        private static string RedisServerUrl = ConfigurationManager.AppSettings["yhAzure.RedisServer"] ?? "47.93.120.162:63790,password=ZmlK_PHGL6wy6X67zZ9jH2ORXsMn03ETed3SB67D-Fdc1qhx";
        private static int CacheDbIndex = 0;
        private static ConnectionMultiplexer RedisClient = ConnectionMultiplexer.Connect(RedisServerUrl);
        private static readonly RedisCache Instance = new RedisCache();

        private RedisCache()
        { }

        public static RedisCache GetInstance()
        {
            Int32.TryParse(ConfigurationManager.AppSettings["yhAzure.CacheDbIndex"], out CacheDbIndex);
            return Instance;
        }

        public bool Set<T>(string Key, T Value, TimeSpan ExpireTime)
        {
            //if (RedisClient.GetDatabase(CacheDbIndex).KeyExists(Key)) { RemoveKey(Key); }

            var _String = JsonConvert.SerializeObject(Value);
            return RedisClient.GetDatabase(CacheDbIndex).StringSet(Key, _String, ExpireTime);
        }
        public T Get<T>(string Key) where T : class
        {
            var StringValue = RedisClient.GetDatabase(CacheDbIndex).StringGet(Key);
            return string.IsNullOrEmpty(StringValue) ? null : JsonConvert.DeserializeObject<T>(StringValue);
        }
        public bool Contains(string Key)
        {
            return RedisClient.GetDatabase(CacheDbIndex).KeyExists(Key);
        }
        public void RemoveKey(string Key)
        {
            RedisClient.GetDatabase(CacheDbIndex).KeyDelete(Key);
        }

        public void RemovePlatform(string PlatformID)
        {
            var RedisServer = RedisClient.GetServer(RedisServerUrl.Substring(0, RedisServerUrl.IndexOf(",")));

            var KeyList = RedisServer.Keys(CacheDbIndex, pattern: PlatformID + "*", pageSize: int.MaxValue);
            foreach (var Key in KeyList)
            {
                RemoveKey(Key);
            }
        }
        public bool IsConnected()
        {
            return RedisClient.IsConnected;
        }
    }
}
