using FirstFrame.Const;
using System;
using System.Configuration;

namespace FirstFrame.Caching
{
    /// <summary>
    /// CacheService
    /// </summary>
    public class CacheManager
    {
        private static readonly string _CacheType = ConfigurationManager.AppSettings["CacheType"];
        public static ICache GetCache()
        {
            ICache Cache = NullCache.GetInstance();
            switch (_CacheType)
            {
                case CacheType.CT_NULL:
                    Cache = NullCache.GetInstance();
                    break;
                case CacheType.CT_RUNTIME:
                    Cache = RuntimeCache.GetInstance();
                    break;
                case CacheType.CT_MEMCACHED:
                    Cache = MemCached.GetInstance();
                    break;
                case CacheType.CT_REDIS:
                    Cache = RedisCache.GetInstance();
                    break;
            }

            return Cache;
        }
    }
}
