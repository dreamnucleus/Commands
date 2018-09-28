using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Notifications;

namespace DreamNucleus.Commands.Tests.Common
{
    public class IntCommandExceptionNotification : IExceptionNotification<IntCommand>
    {
        public Task OnExceptionAsync(IntCommand command, Exception exception)
        {
            return Task.FromResult(0);
        }
    }
}
