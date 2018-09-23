using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Notifications;

namespace DreamNucleus.Commands.Playground
{
    public class ExecutingNotification : IExecutingNotification<FakeCommand>
    {
        public Task OnExecutingAsync(FakeCommand command)
        {
            Console.WriteLine("OnExecutingAsync");
            return Task.FromResult(0);
        }
    }
}
