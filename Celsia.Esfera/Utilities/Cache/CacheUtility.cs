using System;
using Microsoft.Extensions.Caching.Memory;

namespace Utilities.Cache
{
    public class CacheUtility : ICacheUtility
    {
        private readonly MemoryCache systemCache;


        public CacheUtility()
        {
            this.systemCache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = 1024
            });
        }

        public T GetCacheValue<T>(string claveCahce, Func<T> GetValue)
        {
            if (!this.systemCache.TryGetValue(claveCahce, out T value))
            {
                value = GetValue();


                MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions()
                {
                    Size = 1,
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                };

                this.systemCache.Set(claveCahce, value, cacheOptions);
            }

            return value;
        }
    }
}
