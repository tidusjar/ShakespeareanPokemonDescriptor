using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ShakespeareanPokemonDescriptor.Services
{
    public interface ICacheService
    {
        Task<T> GetOrAddAsync<T>(string cacheKey, Func<Task<T>> factory, DateTime absoluteExpiration = default(DateTime), CancellationToken cancellationToken = default(CancellationToken));
        void Remove(string key);
    }
}
