using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis.Tests
{
    public class InMemoryCommandTransportServer : ICommandTransportServer
    {
        private readonly InMemoryTransport _inMemoryTransport;

        private bool _stopped;

        public InMemoryCommandTransportServer(InMemoryTransport inMemoryTransport)
        {
            _inMemoryTransport = inMemoryTransport;

            _stopped = false;
        }

        public Task StartAsync()
        {
            return Task.CompletedTask;
        }

        public Task ListenAsync(Func<ICommandTransport, Task> listenFunc)
        {
            _ = Task.Run(async () =>
            {
                while (!_stopped)
                {
                    if (_inMemoryTransport.TryReadCommand(out var commandTransport))
                    {
                        await listenFunc(commandTransport).ConfigureAwait(false);
                    }

                    if (!_stopped)
                    {
                        await Task.Delay(50).ConfigureAwait(false);
                    }
                }
            });

            return Task.CompletedTask;
        }

        public Task SendAsync(IResultTransport resultTransport)
        {
            _inMemoryTransport.TrySendResult(resultTransport);
            return Task.CompletedTask;
        }


        public Task StopAsync()
        {
            _stopped = true;
            return Task.CompletedTask;
        }
    }
}
