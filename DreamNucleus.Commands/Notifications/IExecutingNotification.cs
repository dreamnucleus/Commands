using System.Threading.Tasks;

namespace DreamNucleus.Commands.Notifications
{
    public interface IExecutingNotification<in TCommand> : IUseCommandsBuilder
    {
        Task OnExecutingAsync(TCommand command);
    }
}
