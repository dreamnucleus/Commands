using System;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Notifications
{
    public interface IExceptionNotification<in TCommand> : IUseCommandsBuilder
    {
        Task OnExceptionAsync(TCommand command, Exception exception);
    }
}
