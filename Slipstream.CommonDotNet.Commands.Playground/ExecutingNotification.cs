using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Notifications;

namespace Slipstream.CommonDotNet.Commands.Playground
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
