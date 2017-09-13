using System;
using System.Configuration;

namespace FirstFrame.Caching
{
    /// <summary>
    /// CacheService
    /// </summary>
    public class CacheService
    {
        private static readonly string CacheConfig = ConfigurationManager.AppSettings["CacheConfig"];

        /// <summary>
        /// GetCache
        /// </summary>
        /// <returns></returns>
        public static ICache GetCache()
        {
            CacheType cacheType;
            Enum.TryParse(CacheConfig, out cacheType);

            ICache cache = NullCache.GetInstance();
            switch (cacheType)
            {
                case CacheType.NullCache:
                    cache = NullCache.GetInstance();
                    break;
                case CacheType.RuntimeCache:
                    cache = RuntimeCache.GetInstance();
                    break;
                case CacheType.MemcachedCache:
                    cache = MemCached.GetInstance();
                    break;
                case CacheType.RedisCache:
                    cache = RedisCache.GetInstance();
                    break;
            }

            return cache;
        }

    }
}
