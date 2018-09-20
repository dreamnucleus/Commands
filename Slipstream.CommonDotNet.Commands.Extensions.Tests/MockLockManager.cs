using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Extensions.Results;

namespace Slipstream.CommonDotNet.Commands.Extensions.Tests
{
    public class MockLockManager : ILockManager
    {
        private readonly ConcurrentDictionary<string, DateTimeOffset> _locks = new ConcurrentDictionary<string, DateTimeOffset>();

        public Task<Lock> AcquireAsync(string resourceId, CancellationToken cancellationToken)
        {
            if (!_locks.TryAdd(resourceId, DateTimeOffset.UtcNow.AddSeconds(5)))
            {
                throw new ConflictException();
            }

            return Task.FromResult(new Lock(resourceId, TimeSpan.FromSeconds(5), DateTimeOffset.UtcNow));
        }

        public Task<Lock> RenewAsync(string resourceId, string leaseId, CancellationToken cancellationToken)
        {
            // TODO:
            if (!_locks.TryAdd(resourceId, DateTimeOffset.UtcNow.AddSeconds(5)))
            {
                throw new Exception();
            }

            return Task.FromResult(new Lock(resourceId, TimeSpan.FromSeconds(5), DateTimeOffset.UtcNow));
        }

        public Task ReleaseAsync(string resourceId, string leaseId, CancellationToken cancellationToken)
        {
            _locks.TryRemove(resourceId, out _);

            return Task.CompletedTask;
        }
    }
}
