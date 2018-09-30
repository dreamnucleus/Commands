using System.Threading;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Unique
{
    public interface IUniqueManager
    {
        Task<bool> CheckAsync(string uniqueCommandId, CancellationToken cancellationToken);
    }
}
