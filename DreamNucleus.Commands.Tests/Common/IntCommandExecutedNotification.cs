using System.Threading.Tasks;
using DreamNucleus.Commands.Notifications;

namespace DreamNucleus.Commands.Tests.Common
{
    public class IntCommandExecutedNotification : IExecutedNotification<IntCommand, int>
    {
        public Task OnExecutedAsync(IntCommand command, int result)
        {
            return Task.FromResult(0);
        }
    }
}
