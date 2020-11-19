using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> factory, DateTime absoluteExpiration = default(DateTime), CancellationToken cancellationToken = default(CancellationToken))
        {
            if (absoluteExpiration == default(DateTime))
            {
                absoluteExpiration = DateTime.Now.AddHours(1);
            }
            // locks get and set internally
            if (_memoryCache.TryGetValue<T>(cacheKey, out var result))
            {
                return result;
            }

            if (_memoryCache.TryGetValue(cacheKey, out result))
            {
                return result;
            }

            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.ThrowIfCancellationRequested();
            }

            result = await factory();
            _memoryCache.Set(cacheKey, result, absoluteExpiration);

            return result;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }
    }
}
