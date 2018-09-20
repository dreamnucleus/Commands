using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slipstream.CommonDotNet.Commands.Notifications;

namespace Slipstream.CommonDotNet.Commands.Playground
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
