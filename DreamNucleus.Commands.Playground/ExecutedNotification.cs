using System;
using System.Threading.Tasks;
using DreamNucleus.Commands.Notifications;

namespace DreamNucleus.Commands.Playground
{
    public class ExecutedNotification : IExecutedNotification<FakeCommand, FakeData>
    {
        public Task OnExecutedAsync(FakeCommand command, FakeData result)
        {
            Console.WriteLine("OnExecutedAsync");
            return Task.FromResult(0);
        }
    }
}
