using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace TheIntegratorLib.Utilities
{
    public enum DataCacheDuration
    {
        Short,
        Medium,
        Long
    }
    public enum DataCacheKey
    {
        Sales,
        UserSales,
        Headers
    }
    public interface IDataCache
    {
        void Create(object value, DataCacheKey cacheKey, DataCacheDuration dataCacheDuration, params object[] keys);
        T Get<T>(DataCacheKey cacheKey, params object[] keys) where T : new();
        void Remove(DataCacheKey dataCacheKey, params object[] keys);
    }
    public class UserSalesCache : IDataCache
    {
        private readonly IMemoryCache _cache;
        private const string _compositeKeySeparator = "|";

        public UserSalesCache()
        {
            MemoryCacheOptions options = new MemoryCacheOptions();
            IMemoryCache cache = new MemoryCache(options);
            _cache = cache;
        }

        private object[] MergeKeys(DataCacheKey cacheKey, params object[] keys)
        {
            var parms = keys.ToList();
            parms.Insert(0, cacheKey);
            return parms.ToArray();
        }

        public void Create(object value, DataCacheKey cacheKey, DataCacheDuration dataCacheDuration, params object[] keys)
        {
            string compositeKey = string.Join(_compositeKeySeparator, MergeKeys(cacheKey, keys));
            TimeSpan timeSpan;
            switch (dataCacheDuration)
            {
                case DataCacheDuration.Short:
                    timeSpan = TimeSpan.FromMinutes(15);
                    break;
                case DataCacheDuration.Medium:
                    timeSpan = TimeSpan.FromMinutes(30);
                    break;
                case DataCacheDuration.Long:
                    timeSpan = TimeSpan.FromHours(1);
                    break;
                default:
                    timeSpan = TimeSpan.FromMinutes(30);
                    break;
            }
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                                                              .SetSlidingExpiration(timeSpan);
            _cache.Set(compositeKey, value, cacheEntryOptions);
        }

        public T Get<T>(DataCacheKey cacheKey, params object[] keys) where T : new()
        {
            string compositeKey = string.Join(_compositeKeySeparator, MergeKeys(cacheKey, keys));
            T cacheValue = new T();
            if (_cache.TryGetValue(compositeKey, out T value))
            {
                cacheValue = value;
            };
            return cacheValue;
        }

        public void Remove(DataCacheKey dataCacheKey, params object[] keys)
        {
            string compositeKey = string.Join(_compositeKeySeparator, MergeKeys(dataCacheKey, keys));
            _cache.Remove(compositeKey);
        }
    }
}