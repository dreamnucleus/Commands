using System.Threading.Tasks;
using DreamNucleus.Commands.Results;

namespace DreamNucleus.Commands.Notifications
{
    public interface IExecutedNotification<in TCommand, in TResult> : IUseCommandsBuilder
        where TCommand : ISuccessResult<TCommand, TResult>
    {
        Task OnExecutedAsync(TCommand command, TResult result);
    }
}
