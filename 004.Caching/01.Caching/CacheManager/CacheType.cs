namespace FirstFrame.Caching
{
    /// <summary>
    /// Cache Type
    /// </summary>
    public enum CacheType
    {
        /// <summary>
        /// Null Cache
        /// </summary>
        NullCache = 0,

        /// <summary>
        /// Runtime Cache
        /// </summary>
        RuntimeCache = 1,

        /// <summary>
        /// Memcached Cache
        /// </summary>
        MemcachedCache = 2,

        /// <summary>
        /// Redis Cache
        /// </summary>
        RedisCache = 3
    }
}
