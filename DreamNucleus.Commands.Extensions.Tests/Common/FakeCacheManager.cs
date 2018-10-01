using System.Collections.Concurrent;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Cache;
using DreamNucleus.Commands.Extensions.Results;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    public class FakeCacheManager : ICacheManager
    {
        private readonly ConcurrentDictionary<int, object> _cache = new ConcurrentDictionary<int, object>();


        public Task AddAsync(int cacheId, object value)
        {
            _cache.AddOrUpdate(cacheId, value, (k, o) => value);
            return Task.CompletedTask;
        }

        public Task<bool> ContainsAsync(int cacheId)
        {
            return Task.FromResult(_cache.ContainsKey(cacheId));
        }

        public Task<object> GetAsync(int cacheId)
        {
            if (_cache.TryGetValue(cacheId, out var cachedValue))
            {
                return Task.FromResult(cachedValue);
            }
            else
            {
                throw new NotFoundException();
            }
        }
    }
}
