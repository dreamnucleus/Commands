using System.Threading;
using System.Threading.Tasks;

namespace Slipstream.CommonDotNet.Commands.Extensions
{
    // TODO: do we need multiple lock mangers...etc
    public interface ILockManager
    {
        Task<Lock> AcquireAsync(string resourceId, CancellationToken cancellationToken);
        Task<Lock> RenewAsync(string resourceId, string leaseId, CancellationToken cancellationToken);
        Task ReleaseAsync(string resourceId, string leaseId, CancellationToken cancellationToken);
    }
}
