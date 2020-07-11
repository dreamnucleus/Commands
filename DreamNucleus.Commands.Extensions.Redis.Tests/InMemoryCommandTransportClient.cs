using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis.Tests
{
    public class InMemoryCommandTransportClient : ICommandTransportClient
    {
        public async Task SendAsync(CommandTransport commandTransport)
        {
            throw new NotImplementedException();
        }

        public async Task ListenAsync<TSuccessResult>(string commandId, Func<ResultTransport<TSuccessResult>, Task> listenFunc)
        {
            throw new NotImplementedException();
        }
    }
}
