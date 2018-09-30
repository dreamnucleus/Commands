using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Notifications;

namespace DreamNucleus.Commands.Tests.Common
{
    public class ExceptionCommandExceptionNotification : IExceptionNotification<ExceptionCommand>
    {
        public Task OnExceptionAsync(ExceptionCommand command, Exception exception)
        {
            return Task.FromResult(0);
        }
    }
}
