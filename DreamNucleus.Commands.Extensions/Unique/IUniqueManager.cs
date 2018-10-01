using System.Threading;
using System.Threading.Tasks;

// TODO: REMOVE
namespace DreamNucleus.Commands.Extensions.Unique
{
    public interface IUniqueManager
    {
        Task<bool> CheckAsync(string uniqueCommandId, CancellationToken cancellationToken);
    }
}
