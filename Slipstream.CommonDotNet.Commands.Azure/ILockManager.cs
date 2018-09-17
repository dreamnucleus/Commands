using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Azure
{
    interface ILockManager
    {
        Task<Lock> AcquireAsync(string resourceId, CancellationToken cancellationToken);
        Task<Lock> RenewAsync(string resourceId, string leaseId, CancellationToken cancellationToken);
        Task ReleaseAsync(string resourceId, string leaseId, CancellationToken cancellationToken);
    }
}
