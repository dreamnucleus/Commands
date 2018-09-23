using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands
{
    // TODO: IDisposable
    public interface ICommandProcessor
    {
        Task<TSuccessResult> ProcessAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand;

        Task<CommandProcessorSuccessResult<TSuccessResult>> ProcessResultAsync<TCommand, TSuccessResult>(ISuccessResult<TCommand, TSuccessResult> command)
            where TCommand : IAsyncCommand;
    }
}
