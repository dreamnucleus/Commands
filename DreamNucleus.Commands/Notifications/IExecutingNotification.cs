using System.Threading.Tasks;

namespace DreamNucleus.Commands.Notifications
{
    // TODO: should this work for base classes or interfaces... IExecutingNotification<ISomethingCommand>
    public interface IExecutingNotification<in TCommand> : IUseCommandsBuilder
    {
        Task OnExecutingAsync(TCommand command);
    }
}
