using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Notifications;

namespace Slipstream.CommonDotNet.Commands.Playground
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
