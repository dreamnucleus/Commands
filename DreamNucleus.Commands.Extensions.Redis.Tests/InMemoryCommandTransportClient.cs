using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis.Tests
{
    public class InMemoryCommandTransportClient : ICommandTransportClient
    {
        private readonly InMemoryTransport _inMemoryTransport;

        public InMemoryCommandTransportClient(InMemoryTransport inMemoryTransport)
        {
            _inMemoryTransport = inMemoryTransport;
        }

        public Task SendAsync(CommandTransport commandTransport)
        {
            _inMemoryTransport.TrySendCommand(commandTransport);
            return Task.CompletedTask;
        }

        public Task ListenAsync<TSuccessResult>(string commandId, Func<ResultTransport<TSuccessResult>, Task> listenFunc)
        {
            _ = Task.Run(async () =>
            {
                var found = false;

                while (!found)
                {
                    if (_inMemoryTransport.TryReadResult(commandId, out var resultTransport))
                    {
                        await listenFunc((ResultTransport<TSuccessResult>)resultTransport).ConfigureAwait(false);
                        found = true;
                    }

                    if (!found)
                    {
                        await Task.Delay(50).ConfigureAwait(false);
                    }
                }
            });

            return Task.CompletedTask;
        }
    }
}
