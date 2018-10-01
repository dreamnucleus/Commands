using System.Threading;
using System.Threading.Tasks;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Semaphore
{
    // TODO: do we need multiple lock mangers...etc
    public interface ILockManager
    {
        Task<Lock> AcquireAsync(string resourceId, CancellationToken cancellationToken);
        Task<Lock> RenewAsync(string resourceId, string leaseId, CancellationToken cancellationToken);
        Task ReleaseAsync(string resourceId, string leaseId, CancellationToken cancellationToken);
    }
}
