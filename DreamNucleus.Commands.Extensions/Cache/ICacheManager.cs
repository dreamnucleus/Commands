using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Cache
{
    // TODO: obviously just hacking
    public interface ICacheManager
    {
        Task AddAsync(int cacheId, object value);
        Task<bool> ContainsAsync(int cacheId);
        Task<object> GetAsync(int cacheId);
    }
}
