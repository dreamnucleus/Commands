using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis.Tests
{
    public class InMemoryCommandTransportServer : ICommandTransportServer
    {
        public Task ListenAsync(Func<ICommandTransport, Task> listenFunc)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(IResultTransport resultTransport)
        {
            throw new NotImplementedException();
        }

        public Task StartAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
