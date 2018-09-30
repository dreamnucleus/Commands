using System.Threading.Tasks;
using DreamNucleus.Commands.Notifications;

namespace DreamNucleus.Commands.Tests.Common
{
    public class IntCommandExecutingNotification : IExecutingNotification<IntCommand>
    {
        public Task OnExecutingAsync(IntCommand command)
        {
            return Task.FromResult(0);
        }
    }
}
