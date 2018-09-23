using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands
{
    public interface IAsyncCommandHandler<in TCommand, TSuccessResult>
        where TCommand : ISuccessResult<TCommand, TSuccessResult>
    {
        Task<TSuccessResult> ExecuteAsync(TCommand command);
    }

}
