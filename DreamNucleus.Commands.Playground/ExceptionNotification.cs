using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Notifications;

namespace DreamNucleus.Commands.Playground
{
    public class ExceptionNotification : IExceptionNotification<FakeCommand>
    {
        public Task OnExceptionAsync(FakeCommand command, Exception exception)
        {
            Console.WriteLine("OnExceptionAsync");
            return Task.FromResult(0);
        }
    }
}
