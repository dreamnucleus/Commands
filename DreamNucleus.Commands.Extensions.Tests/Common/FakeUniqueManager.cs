using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DreamNucleus.Commands.Extensions.Unique;

namespace DreamNucleus.Commands.Extensions.Tests.Common
{
    public class FakeUniqueManager : IUniqueManager
    {
        private readonly ConcurrentDictionary<string, DateTimeOffset> _locks = new ConcurrentDictionary<string, DateTimeOffset>();

        public Task<bool> CheckAsync(string uniqueCommandId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_locks.TryAdd(uniqueCommandId, DateTimeOffset.UtcNow.AddSeconds(5)));
        }
    }
}
