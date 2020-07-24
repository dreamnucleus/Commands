using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamNucleus.Commands.Extensions.Redis
{
    public interface ICommandTransportClient
    {
        Task SendAsync(CommandContainer commandTransport);
        // TODO: Listen is clunky, should be called StartListen at the very least
        Task ListenAsync<TSuccessResult>(string commandId, Func<ResultTransport<TSuccessResult>, Task> listenFunc);
    }
}
